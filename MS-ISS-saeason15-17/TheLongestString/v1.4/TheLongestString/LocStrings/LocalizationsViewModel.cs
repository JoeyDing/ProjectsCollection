using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TheLongestString
{
    public class LocalizationsViewModel : BindableBase
    {
        private LocalizationsModel _model;

        private LocStringCollection _locStrings;
        private IEnumerable<string> _locIds;
        private int _selectedLocIdIndex = -1;

        private string _buildNumber = null;

        private Typeface _renderTypeface = null;
        private double _renderFontSize = 12;
        private string _folderPath = null;

        private List<string> _fontNames;
        private List<double> _fontSizes;
        private string _selectedFontName;

        private bool _isBusy;

        /// <summary>
        /// Loc Id used to identify where the build number is written.
        /// </summary>
        private const string BUILD_NUMBER_LOC_ID = @"ResTFileVersion";

        public LocalizationsViewModel(string fontName, double fontSize, string folder, string fileSearchPattern)
        {
            this._renderFontSize = fontSize;
            this._selectedFontName = fontName;
            this.UpdateFontTypeFace(this.SelectedFontName, resetWidth: false);
            this.FontSizes = new List<double>()
            {
                8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
            };
            this.FontNames = System.Drawing.FontFamily.Families.Select(f => f.Name).ToList();
            this.LoadFiles(folder, fileSearchPattern);
        }

        #region Methods

        /// <summary>
        /// Recursively searchs a folder for LCL files that match the search
        /// pattern and parses them so they're ready to display.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileSearchPattern"></param>
        private void LoadFiles(string folder, string fileSearchPattern)
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
            var files = new List<string>(Directory.GetFiles(fullPath, fileSearchPattern, SearchOption.AllDirectories));
            files.RemoveAll(x =>
                x.IndexOf("pseudo", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("hashid", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("af-za", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                x.IndexOf("ga-ie", StringComparison.InvariantCultureIgnoreCase) >= 0
            );


            // load the required files
            _model = new LocalizationsModel();
            //_model.LoadFiles(files.ToArray());
            _model.LoadFilesInParallel(files, RenderTypeface, RenderFontSize);

            this.FolderPath = fullPath;
            this.OnPropertyChanged("SourceDictionary");
            Ids = _model.SourceDictionary.LocIds.OrderBy(x => x);

            // try to get the build number of these LCL files
            var versionString = _model.SourceDictionary.GetLocString(BUILD_NUMBER_LOC_ID);
            if (versionString != null)
            {
                BuildNumber = versionString.String;
            }
            else
            {
                BuildNumber = "Unknown";
            }
        }

        private void UpdateFontTypeFace(string fontName, bool resetWidth)
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
                            Localizations = _model.GetLocStrings(Ids.ElementAt(_selectedLocIdIndex));
                    }
                });
                t.ContinueWith((p) => { this.IsBusy = false; });
            }
        }

        #endregion

        #region Properties

        public int SelectedLocIdIndex
        {
            get { return _selectedLocIdIndex; }
            set
            {
                if (SetProperty<int>(ref _selectedLocIdIndex, value))
                {
                    Localizations = _model.GetLocStrings(Ids.ElementAt(_selectedLocIdIndex));
                }
            }
        }

        public IEnumerable<string> Ids
        {
            get { return _locIds; }
            private set
            {
                SetProperty<IEnumerable<string>>(ref _locIds, value);
            }
        }

        public LocStringCollection Localizations
        {
            get { return _locStrings; }
            private set
            {
                SetProperty<LocStringCollection>(ref _locStrings, value);
            }
        }

        /// <summary>
        /// Build number of the LCL files. Can be null.
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
            get { return _renderFontSize; }
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
        #endregion

        #region Static Method
        public static Task<LocalizationsViewModel> CreateAsync(string fontName, double fontSize, string folder, string fileSearchPattern)
        {
            return new Task<LocalizationsViewModel>(() => new LocalizationsViewModel(fontName, fontSize, folder, fileSearchPattern));
        }

        #endregion
       
    }
}
