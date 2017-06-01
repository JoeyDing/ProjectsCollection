using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace TheLongestString.Model
{
    public class CultureFontUtil
    {
        public static List<FontConfigModel> UpdateCultureFontsFromLocString(List<FontConfigModel> cultureFonts, LocString loc)
        {
            //-----------------------
            // Save culture to file
            if (cultureFonts == null)
            {
                cultureFonts = new List<FontConfigModel>();
            }
            var cultureFont = cultureFonts.Where(c => c.Culture.Equals(loc.Dictionary.CultureName)).FirstOrDefault();
            if (cultureFont != null)
            {
                cultureFont.FontFamily = loc.FontFamilyName;
                cultureFont.FontSize = loc.FontSize.Value;
            }
            else
            {
                var cf = new FontConfigModel();
                cf.Culture = loc.Dictionary.CultureName;
                cf.FontFamily = loc.FontFamilyName;
                cf.FontSize = loc.FontSize.Value;
                cultureFonts.Add(cf);
            }
            return cultureFonts;
        }

        public static void UpdateLocStringFromCultureFont(ref List<FontConfigModel> cultureFonts, LocString locString, string defaultFont, double defaultSize)
        {
            if (locString.IsSelected == null)
                locString.IsSelected = false;
            //if (locString.FontSize == null)
            //    locString.FontSize = defaultSize;
            if (cultureFonts == null)
            {
                cultureFonts = new List<FontConfigModel>();
            }

            var cultureFont = cultureFonts.Where(c => c.Culture.Equals(locString.Dictionary.CultureName)).FirstOrDefault();
            if (cultureFont != null)
            {
                locString.FontFamilyName = cultureFont.FontFamily;
                locString.FontFamily = new System.Windows.Media.FontFamily(locString.FontFamilyName);
                locString.FontSize = cultureFont.FontSize;
                Typeface tf = new Typeface(locString.FontFamilyName);
                locString.Width = LocString.CalculateTextWidth(locString, tf, locString.FontSize.Value);
                locString.Height = LocString.CalculateTextHeight(locString, tf, locString.FontSize.Value);
            }
            else
            {
                var cf = new FontConfigModel();
                cf.Culture = locString.Dictionary.CultureName;
                cf.FontFamily = defaultFont;
                cf.FontSize = defaultSize;
                cultureFonts.Add(cf);

                locString.FontFamilyName = defaultFont;
                locString.FontFamily = new System.Windows.Media.FontFamily(locString.FontFamilyName);
                locString.FontSize = defaultSize;
                Typeface tf = new Typeface(locString.FontFamilyName);
                locString.Width = LocString.CalculateTextWidth(locString, tf, locString.FontSize.Value);
                locString.Height = LocString.CalculateTextHeight(locString, tf, locString.FontSize.Value);
            }
        }

        public static LocStringCollection UpdateLocStringCollectionFromCultureFont(ref List<FontConfigModel> cultureFonts, LocStringCollection localizations, string defaultFont, double defaultSize)
        {
            if (cultureFonts == null)
            {
                cultureFonts = new List<FontConfigModel>();
            }
            foreach (var vm in localizations.Items)
            {
                var cultureFont = cultureFonts.Where(c => c.Culture.Equals(vm.Dictionary.CultureName)).FirstOrDefault();
                if (cultureFont != null)
                {
                    vm.FontFamilyName = cultureFont.FontFamily;
                    vm.FontFamily = new System.Windows.Media.FontFamily(vm.FontFamilyName);
                    vm.FontSize = cultureFont.FontSize;
                    Typeface tf = new Typeface(vm.FontFamilyName);
                    vm.Width = LocString.CalculateTextWidth(vm, tf, vm.FontSize.Value);
                    vm.Height = LocString.CalculateTextHeight(vm, tf, vm.FontSize.Value);
                }
                else
                {
                    var cf = new FontConfigModel();
                    cf.Culture = vm.Dictionary.CultureName;
                    cf.FontFamily = defaultFont;
                    cf.FontSize = defaultSize;
                    cultureFonts.Add(cf);

                    vm.FontFamilyName = defaultFont;
                    vm.FontFamily = new System.Windows.Media.FontFamily(vm.FontFamilyName);
                    vm.FontSize = defaultSize;
                    Typeface tf = new Typeface(vm.FontFamilyName);
                    vm.Width = LocString.CalculateTextWidth(vm, tf, vm.FontSize.Value);
                    vm.Height = LocString.CalculateTextHeight(vm, tf, vm.FontSize.Value);
                }
            }
            return localizations;
        }

        public static void SaveFontConfigureToFile(string fn, List<FontConfigModel> cultureFonts)
        {
            if (cultureFonts == null) return;
            XmlDocument xd = new XmlDocument();
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(cultureFonts.GetType());
                xz.Serialize(sw, cultureFonts);
                xd.LoadXml(sw.ToString());
                xd.Save(fn);
            }
        }

        public static List<FontConfigModel> LoadCultureFontsFromConfig(string filename)
        {
            if (File.Exists(filename))
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    List<FontConfigModel> cultureFonts = new List<FontConfigModel>();
                    XmlSerializer xz = new XmlSerializer(cultureFonts.GetType());
                    cultureFonts = (List<FontConfigModel>)xz.Deserialize(reader);
                    return cultureFonts;
                }
            }
            return null;
        }
    }
}