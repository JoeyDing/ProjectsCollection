using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Sfb.Core.Services
{
    public class GetConfigurationLanguagesService : IGetConfigurationLanguages
    {
        public List<LocCulture> GetLanguages()
        {
            XNamespace ns = "sfb";
            string locCulturesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sfb_config.xml");
            XDocument xmlFile = XDocument.Load(locCulturesPath);
            bool isLip = false;
            var result = xmlFile.Root.Element(ns + "LocCultures").Elements().Select(c => new LocCulture
            {
                CultureName = c.Attribute("CultureName").Value.ToString(),
                EnglishName = c.Attribute("EnglishName").Value.ToString(),
                Lcid = int.Parse(c.Attribute("LCID").Value.ToString()),
                IsLip = bool.TryParse(c.Attribute("IsLip") != null ? c.Attribute("IsLip").Value : "", out isLip),
            }).ToList();

            return result;
        }
    }
}