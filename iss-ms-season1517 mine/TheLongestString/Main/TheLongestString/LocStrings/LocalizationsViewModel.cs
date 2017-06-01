using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using TheLongestString.LocFileProvider;
using TheLongestString.Model;

namespace TheLongestString
{
    public class LocalizationsViewModel : BindableBase
    {
        public LocalizationsModel _model { get; set; }

        protected LocStringCollection _locStrings;
        protected List<string> _locIds;
        protected int _selectedLocIdIndex = -1;

        protected string _buildNumber = null;

        protected Typeface _renderTypeface = null;
        protected double _renderFontSize = 12;
        protected string _folderPath = null;

        protected List<string> _fontNames;
        protected List<double> _fontSizes;
        protected string _selectedFontName;

        protected bool _isBusy;
        protected bool _withSourceString;
        protected bool _canShowNotSupportedByFont;
        protected string _currentDisplayedItemID = null;
        protected IEnumerable<LocString> _currentStringsInfo = null;

        public List<FontConfigModel> cultureFonts;
        public bool LoadedWithFile { get; set; }
        public string defaultFontName { get; set; }
        public double defaultFontSize { get; set; }

        /// <summary>
        /// Loc Id used to identify where the build number is written.
        /// </summary>
        private const string BUILD_NUMBER_LOC_ID = @"ResTFileVersion";

        public LocalizationsViewModel()
        {
        }

        public LocalizationsViewModel(string fontName, double fontSize, string folder, string fileSearchPattern)
        {
            this._renderFontSize = ConvertFromPoint.PointsToDIP(fontSize);
            this._selectedFontName = fontName;
            this.UpdateFontTypeFace(this.SelectedFontName, resetWidth: false);
            this.FontSizes = new List<double>()
            {
                8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
            };
            this.FontNames = System.Drawing.FontFamily.Families.Select(f => f.Name).ToList();
            this.LoadFiles(folder, fileSearchPattern);
            this.CanShowNotSupportedByFont = true;
            this.defaultFontName = fontName;
            this.defaultFontSize = fontSize;
        }

        #region Methods

        /// <summary>
        /// Recursively searchs a folder for LCX files that match the search
        /// pattern and parses them so they're ready to display.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileSearchPattern"></param>
        protected void LoadFiles(string folder, string fileSearchPattern)
        {
            string fullPath = null;
            if (Path.IsPathRooted(folder))
            {
                fullPath = folder;
            }
            else
            {
                var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                // get path relative to application folder
                fullPath = Path.Combine(
                    Path.GetDirectoryName(exePath),
                    folder
                    );
            }

            // get a list of all the files
            var files = Directory.EnumerateFiles(fullPath, "*.*", SearchOption.AllDirectories).Where(f => IsValidExtension(f, fileSearchPattern)).ToList();
            files.RemoveAll(x =>
                x.IndexOf("pseudo", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("hashid", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("af-za", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("ga-ie", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("qps-ploc", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("qps-plocm", StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            // load the required files
            _model = new LocalizationsModel();
            //_model.LoadFiles(files.ToArray());
            var fileLoaded = _model.LoadFilesInParallel(files, RenderTypeface, RenderFontSize);

            if (fileLoaded)
            {
                this.WithSourceStrings = _model.SourceDictionary.WithSourceStrings;
                this.FolderPath = fullPath;
                this.OnPropertyChanged("SourceDictionary");
                Ids = _model.SourceDictionary.LocIds.OrderBy(x => x).ToList();

                // try to get the build number of these LCX files [this is LMX-specific]
                var versionString = _model.SourceDictionary.GetLocString(BUILD_NUMBER_LOC_ID);
                if (versionString != null)
                {
                    BuildNumber = versionString.String;
                }
                else
                {
                    BuildNumber = "Unknown";
                }

                this.LoadedWithFile = true;
            }
            else
            {
                this.LoadedWithFile = false;
            }
        }

        protected void UpdateFontTypeFace(string fontName, bool resetWidth)
        {
            if (!string.IsNullOrWhiteSpace(fontName))
            {
                this.IsBusy = true;
                var t = Task.Factory.StartNew(() =>
                {
                    if (this.RenderTypeface == null || this.RenderTypeface.FontFamily.Source != fontName)
                    {
                        this.RenderTypeface = new Typeface(
                                                               new FontFamily(fontName),
                                                               FontStyles.Normal,
                                                               FontWeights.Normal,
                                                               FontStretches.Normal
                                                               );
                    }

                    if (resetWidth)
                    {
                        //update widths info with new font/fontsize
                        this._model.UpdateModelFontInfo(this.RenderTypeface, this.RenderFontSize);
                        //reset localizations
                        if (_selectedLocIdIndex >= 0 && _selectedLocIdIndex < Ids.Count())
                            this.RefreshLocalizations();
                    }
                });
                t.ContinueWith((p) => { this.IsBusy = false; });
            }
        }

        protected bool IsValidExtension(string filePath, string pattern)
        {
            foreach (var extension in pattern.Split(new char[] { ',' }))
            {
                if (filePath.EndsWith(extension.Trim()))
                    return true;
            }

            return false;
        }

        public async Task RefreshLocalizations()
        {
            this.IsBusy = true;

            await Task.Factory.StartNew(() =>
                {
                    if (this._selectedLocIdIndex >= 0)
                    {
                        var currentItemID = this.Ids.ElementAt(_selectedLocIdIndex);
                        this.Localizations = _model.GetLocStrings(currentItemID, ref this.cultureFonts, this.defaultFontName, this.defaultFontSize, allowNonSupportedFont: this.CanShowNotSupportedByFont);
                        this._currentDisplayedItemID = currentItemID;
                    }
                });

            this.IsBusy = false;
        }

        #endregion Methods

        #region Properties

        public int SelectedLocIdIndex
        {
            get { return _selectedLocIdIndex; }
            set
            {
                if (SetProperty<int>(ref _selectedLocIdIndex, value))
                {
                    this.RefreshLocalizations();
                }
            }
        }

        public async Task SetSelectedIndex(int i)
        {
            this._selectedLocIdIndex = i;
            await this.RefreshLocalizations();
        }

        public List<string> Ids
        {
            get { return _locIds; }
            private set
            {
                SetProperty<List<string>>(ref _locIds, value);
            }
        }

        public LocStringCollection Localizations
        {
            get
            {
                return _locStrings;
            }
            private set
            {
                SetProperty<LocStringCollection>(ref _locStrings, value);
            }
        }

        /// <summary>
        /// Build number of the LCX files. Can be null. [this is LMX-specific]
        /// </summary>
        public string BuildNumber
        {
            get { return _buildNumber; }
            private set
            {
                SetProperty<string>(ref _buildNumber, value);
            }
        }

        public LocDictionary SourceDictionary
        {
            get
            {
                if (_model != null)
                {
                    return _model.SourceDictionary;
                }

                return null;
            }
        }

        public Typeface RenderTypeface
        {
            get { return _renderTypeface; }
            private set
            {
                SetProperty<Typeface>(ref _renderTypeface, value);
            }
        }

        public double RenderFontSize
        {
            get
            {
                //convert points into device independent units
                return _renderFontSize;
                //return this.PointsToDIP(_renderFontSize);
            }
            set
            {
                SetProperty<double>(ref _renderFontSize, value);
                this.UpdateFontTypeFace(this.SelectedFontName, resetWidth: true);
            }
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set { SetProperty<string>(ref _folderPath, value); }
        }

        public List<string> FontNames
        {
            get { return _fontNames; }
            set { SetProperty<List<string>>(ref _fontNames, value); }
        }

        public List<double> FontSizes
        {
            get { return _fontSizes; }
            set { SetProperty<List<double>>(ref _fontSizes, value); }
        }

        public string SelectedFontName
        {
            get { return _selectedFontName; }
            set
            {
                SetProperty<string>(ref _selectedFontName, value);
                this.UpdateFontTypeFace(_selectedFontName, resetWidth: true);
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetProperty<bool>(ref _isBusy, value);
            }
        }

        public bool WithSourceStrings
        {
            get { return _withSourceString; }
            set
            {
                _withSourceString = value;
                SetProperty<bool>(ref _withSourceString, value);
            }
        }

        public bool CanShowNotSupportedByFont
        {
            get { return _canShowNotSupportedByFont; }
            set
            {
                SetProperty<bool>(ref _canShowNotSupportedByFont, value);
                this.RefreshLocalizations();
            }
        }

        #endregion Properties

        #region Static Method

        public static Task<LocalizationsViewModel> CreateAsync(string fontName, double fontSize, string folder, string fileSearchPattern)
        {
            return new Task<LocalizationsViewModel>(() => new LocalizationsViewModel(fontName, fontSize, folder, fileSearchPattern));
        }

        #endregion Static Method
    }
}