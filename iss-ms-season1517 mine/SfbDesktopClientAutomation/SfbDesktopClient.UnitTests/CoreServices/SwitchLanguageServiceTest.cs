using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfb.Core;
using Sfb.Core.Services;

namespace Office15Automation.UnitTests.CoreServices
{
    [TestClass]
    public class SwitchLanguageServiceTest
    {
        [TestMethod]
        public void IT_SwitchLanguageService_O15_EnglishToFrench()
        {
            //arrange
            var closeSfbClientService = new CloseSfbClientService();
            var switchLangService = new SwitchLanguageService(closeSfbClientService);
            int frenchLcid = 1036;
            int englishLcid = 1033;

            //act
            switchLangService.SwitchLanguage(new LocCulture { Lcid = 1036, CultureName = "fr-FR" }, OfficeType.O15);

            //assert
        }

        [TestMethod]
        public void IT_SwitchLanguageService_O16_EnglishToFrench()
        {
            //arrange
            var closeSfbClientService = new CloseSfbClientService();
            var switchLangService = new SwitchLanguageService(closeSfbClientService);
            int frenchLcid = 1036;
            int englishLcid = 1033;

            //act
            switchLangService.SwitchLanguage(new LocCulture { Lcid = 1036, CultureName = "fr-FR" }, OfficeType.O16);

            //assert
        }
    }
}