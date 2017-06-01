using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfb.Core;
using Sfb.Core.Services;
using Sfb.Installer.Core;
using Sfb.Installer.Core.Interfaces;
using Sfb.Installer.Core.Services;
using Sfb.LanguageInstaller.Wpf;
using Sfb.LanguageInstaller.Wpf.Interface;
using Sfb.LanguageInstaller.Wpf.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sfb.Installer.UnitTests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public async Task MainWindowViewModelTests_OnInstallOffice_ReturnsInstallOfficeOperationOnly()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());
            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns(() =>
            {
                return new SfbInstallationInfo()
                {
                    OfficeType = OfficeType.O15,
                    SfbInstallationFileName = "setup.com",
                    SfbInstallationFolderPath = "installationFolder",
                    LanguagePackInfos = new List<SfbLanguagePackInfo> {
                    new SfbLanguagePackInfo {
                        IsChecked = false,
                        Language = new LocCulture { CultureName = "fr-FR", EnglishName = "French", IsLip = false, Lcid = 1001},
                        LanguagePackFolderPath = "languageFolderPath",
                        LanguagePackInstallationFileName = "setup.com"
                    }
                }
                };
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
               getInstallationInfoServiceMoq.Object,
               installeOfficeServiceMoq.Object,
               removeOfficeServiceMoq.Object,
               removeLanguagesServiceMoq.Object,
               installLanguageServiceMoq.Object,
               loadHistoryServiceMoq.Object);
            viewModel.ExpandService = expandServiceMoq.Object;

            //environment: no office installed, no language selected, fresh install of office 16.0.0.0
            List<Operation> results = null;
            viewModel.UserTypedBuildVersion = "16.0.0.0";
            viewModel.CurrentBuildVersion = null;

            await viewModel.OnInstallOffice(null);
            results = viewModel.LastRunOperations;

            //assert
            Assert.AreEqual(results.Count, 1);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_OnInstallOffice_ReturnsInstallOfficeAnd2Languages()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());
            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns<string>((buildNumber) =>
            {
                SfbInstallationInfo result = null;
                if (buildNumber.StartsWith("15."))
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O15,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = false,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                else if (buildNumber == "16.0.0.0")
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O16,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            },
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "de-DE",
                                    EnglishName = "German",
                                    IsLip = false,
                                    Lcid = 1002},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                return result;
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
                getInstallationInfoServiceMoq.Object,
                installeOfficeServiceMoq.Object,
                removeOfficeServiceMoq.Object,
                removeLanguagesServiceMoq.Object,
                installLanguageServiceMoq.Object,
                loadHistoryServiceMoq.Object);

            //environment: no office installed, no language selected, fresh install of office 16.0.0.0 + 2 languages
            viewModel.ExpandService = expandServiceMoq.Object;
            List<Operation> results = null;
            viewModel.UserTypedBuildVersion = "16.0.0.0";
            viewModel.CurrentBuildVersion = null;

            await viewModel.OnInstallOffice(null);
            results = viewModel.LastRunOperations;

            //assert
            Assert.AreEqual(results.Count, 3);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_OnInstallOffice_ReturnsRemoveOfficeAnd2LanguagesAndInstall1Language()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());

            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns<string>((buildNumber) =>
            {
                SfbInstallationInfo result = null;
                if (buildNumber.StartsWith("15."))
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O15,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = false,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "office15languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                else if (buildNumber == "16.0.0.0")
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O16,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "office16languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                return result;
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
                getInstallationInfoServiceMoq.Object,
                installeOfficeServiceMoq.Object,
                removeOfficeServiceMoq.Object,
                removeLanguagesServiceMoq.Object,
                installLanguageServiceMoq.Object,
                loadHistoryServiceMoq.Object);

            //environment: one office installed 15.0.0.0, two language selected, fresh install of office 16.0.0.0 + 1 languages
            viewModel.ExpandService = expandServiceMoq.Object;
            List<Operation> results = null;
            viewModel.UserTypedBuildVersion = "16.0.0.0";
            viewModel.CurrentBuildVersion = "15.0.0.0";
            viewModel.InstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>
            {
               new SfbLanguagePackInfo
               {
                   Language = new LocCulture { CultureName = "de-DE",
                                    EnglishName = "German",
                                    IsLip = false,
                                    Lcid = 1002},
                   IsChecked =true,
                   LanguagePackFolderPath="office15languageFolderPath",
                   LanguagePackInstallationFileName ="step.com"
               },
               new SfbLanguagePackInfo
               {
                   Language = new LocCulture { CultureName = "fi-FI",
                                    EnglishName = "Finnish",
                                    IsLip = false,
                                    Lcid = 1003},
                   IsChecked =true,
                   LanguagePackFolderPath="office15languageFolderPath",
                   LanguagePackInstallationFileName ="step.com"
               }
            };

            await viewModel.OnInstallOffice(null);
            results = viewModel.LastRunOperations;

            //assert
            Assert.AreEqual(results.Count, 5);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_OnInstallOffice_ReturnsRemove2LanguagesAndInstallOfficeAndInstall1Language()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());

            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns<string>((buildNumber) =>
            {
                SfbInstallationInfo result = null;
                if (buildNumber.StartsWith("15."))
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O15,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = false,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                else if (buildNumber == "16.0.0.0")
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O16,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                return result;
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
                getInstallationInfoServiceMoq.Object,
                installeOfficeServiceMoq.Object,
                removeOfficeServiceMoq.Object,
                removeLanguagesServiceMoq.Object,
                installLanguageServiceMoq.Object,
                loadHistoryServiceMoq.Object);

            //environment: two language's been installed, remove them and install office 16.0.0.0 + 1 language
            viewModel.ExpandService = expandServiceMoq.Object;
            List<Operation> results = null;
            viewModel.UserTypedBuildVersion = "16.0.0.0";
            viewModel.InstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>
            {
               new SfbLanguagePackInfo
               {
                   Language = new LocCulture { CultureName = "de-DE",
                                    EnglishName = "German",
                                    IsLip = false,
                                    Lcid = 1002},
                   IsChecked =true,
                   LanguagePackFolderPath="languageFolderPath",
                   LanguagePackInstallationFileName ="step.com"
               },
               new SfbLanguagePackInfo
               {
                   Language = new LocCulture { CultureName = "fi-FI",
                                    EnglishName = "Finnish",
                                    IsLip = false,
                                    Lcid = 1003},
                   IsChecked =true,
                   LanguagePackFolderPath="languageFolderPath",
                   LanguagePackInstallationFileName ="step.com"
               }
            };
            viewModel.LanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>
            {
                new SfbLanguagePackInfo
                {
                    Language = new LocCulture()
                    {
                        CultureName = "sq-AL",
                        EnglishName = "Albanian",
                        IsLip = true,
                        Lcid = 1052
                    },
                    LanguagePackFolderPath = "languagePackFolderPath",
                    LanguagePackInstallationFileName = "lip.msi",
                    IsChecked = true,
                }
            };

            await viewModel.OnInstallOffice(null);
            results = viewModel.LastRunOperations;

            //assert
            Assert.AreEqual(results.Count, 4);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_OnRemoveOffice_ReturnsOffice()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());

            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns<string>((buildNumber) =>
            {
                SfbInstallationInfo result = null;
                if (buildNumber.StartsWith("15."))
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O15,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = false,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                else if (buildNumber == "16.0.0.0")
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O16,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                return result;
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
                getInstallationInfoServiceMoq.Object,
                installeOfficeServiceMoq.Object,
                removeOfficeServiceMoq.Object,
                removeLanguagesServiceMoq.Object,
                installLanguageServiceMoq.Object,
                loadHistoryServiceMoq.Object);

            //environment: two language's been installed, remove them and install office 16.0.0.0 + 1 language
            viewModel.ExpandService = expandServiceMoq.Object;
            List<Operation> results = null;

            viewModel.CurrentBuildVersion = "15.0.0.0";
            await viewModel.OnRemoveOffice(null);
            results = viewModel.LastRunOperations;
            //assert
            Assert.AreEqual(results.Count, 1);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_OnInstallLanguages()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());

            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns<string>((buildNumber) =>
            {
                SfbInstallationInfo result = null;
                if (buildNumber.StartsWith("15."))
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O15,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = false,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                else if (buildNumber == "16.0.0.0")
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O16,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                return result;
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
                getInstallationInfoServiceMoq.Object,
                installeOfficeServiceMoq.Object,
                removeOfficeServiceMoq.Object,
                removeLanguagesServiceMoq.Object,
                installLanguageServiceMoq.Object,
                loadHistoryServiceMoq.Object);

            //environment: two language's been installed, remove them and install office 16.0.0.0 + 1 language
            viewModel.ExpandService = expandServiceMoq.Object;
            List<Operation> results = null;
            viewModel.UninstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>
            {
                new SfbLanguagePackInfo
                {
                    Language = new LocCulture
                    {
                        CultureName ="Fi-FI",
                        IsLip=false,
                        EnglishName="Finnish",
                        Lcid = 1005
                    },
                    IsChecked=true,
                    LanguagePackFolderPath="LanguagePackFolderPath",
                    LanguagePackInstallationFileName="setup.com",
                }
            };
            //viewModel.CurrentBuildVersion = "15.0.0.0";
            await viewModel.OnInstallLanguages(null);
            results = viewModel.LastRunOperations;
            //assert
            Assert.AreEqual(results.Count, 1);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_OnRemoveLanguages()
        {
            //arrange
            var getCurrentOfficeVersionServiceMoq = new Mock<IGetCurrentOfficeVersion>();
            var getInstallationInfoServiceMoq = new Mock<IGetInstallationInfo>();
            var installeOfficeServiceMoq = new Mock<ISfbOfficeInstaller>();
            var removeOfficeServiceMoq = new Mock<ISfbOfficeUnInstaller>();
            var removeLanguagesServiceMoq = new Mock<ISfbOfficeLanguageUninstaller>();
            var installLanguageServiceMoq = new Mock<ISfbOfficeLanguageInstaller>();
            var loadHistoryServiceMoq = new Mock<ILoadHistory>();
            var expandServiceMoq = new Mock<ICanExpand>();

            //Mock initialization
            loadHistoryServiceMoq.Setup(x => x.Load()).Returns(new ObservableCollection<InstallerHistory>());

            getInstallationInfoServiceMoq.Setup(x => x.GetInstallationInfo(It.Is<string>(y => true))).Returns<string>((buildNumber) =>
            {
                SfbInstallationInfo result = null;
                if (buildNumber.StartsWith("15."))
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O15,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = false,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                else if (buildNumber == "16.0.0.0")
                {
                    result = new SfbInstallationInfo()
                    {
                        OfficeType = OfficeType.O16,
                        SfbInstallationFileName = "setup.com",
                        SfbInstallationFolderPath = "installationFolder",
                        LanguagePackInfos = new List<SfbLanguagePackInfo>
                        {
                            new SfbLanguagePackInfo {
                                IsChecked = true,
                                Language = new LocCulture { CultureName = "fr-FR",
                                    EnglishName = "French",
                                    IsLip = false,
                                    Lcid = 1001},
                                LanguagePackFolderPath = "languageFolderPath",
                                LanguagePackInstallationFileName = "setup.com"
                            }
                        }
                    };
                }
                return result;
            });

            //act
            var viewModel = new MainWindowViewModel(getCurrentOfficeVersionServiceMoq.Object,
                getInstallationInfoServiceMoq.Object,
                installeOfficeServiceMoq.Object,
                removeOfficeServiceMoq.Object,
                removeLanguagesServiceMoq.Object,
                installLanguageServiceMoq.Object,
                loadHistoryServiceMoq.Object);

            //environment: two language's been installed, remove them and install office 16.0.0.0 + 1 language
            viewModel.ExpandService = expandServiceMoq.Object;
            List<Operation> results = null;
            viewModel.InstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>
            {
                new SfbLanguagePackInfo
                {
                    Language = new LocCulture
                    {
                        CultureName ="fil-PH",
                        IsLip=true,
                        EnglishName="SFASG",
                        Lcid = 1025
                    },
                    IsChecked=true,
                    LanguagePackFolderPath="office15LanguagePackFolderPath",
                    LanguagePackInstallationFileName="setup.com",
                },
                new SfbLanguagePackInfo
                {
                    Language = new LocCulture
                    {
                        CultureName ="sq-AL",
                        IsLip=true,
                        EnglishName="adsdafa",
                        Lcid = 1005
                    },
                    IsChecked=true,
                    LanguagePackFolderPath="office15LanguagePackFolderPath",
                    LanguagePackInstallationFileName="setup.com",
                }
            };
            viewModel.CurrentBuildVersion = "15.0.0.0";
            await viewModel.OnRemoveLanguages(null);
            results = viewModel.LastRunOperations;
            //assert
            Assert.AreEqual(results.Count, 2);
        }

        [TestMethod]
        public async Task MainWindowViewModelTests_CheckAllLanugage()
        {
            //arrange
            GetCurrentOfficeVersionService getCurrentOfficeVersionService = new GetCurrentOfficeVersionService();
            GetInstallationInfoService getInstallationInfoService = new GetInstallationInfoService();
            RunCmdCommandService runCmdCommandService = new RunCmdCommandService();
            CloseSfbClientService closeSfbClientService = new CloseSfbClientService();
            SfbOfficeInstallationService installOfficeService = new SfbOfficeInstallationService(runCmdCommandService, closeSfbClientService);
            SfbOfficeUninstallationService sfbOfficeUninstallationService = new SfbOfficeUninstallationService(runCmdCommandService, closeSfbClientService);
            SfbOfficeLanguageInstallationService sfbOfficeLanguageInstallationService = new SfbOfficeLanguageInstallationService(runCmdCommandService, closeSfbClientService);
            SfbOfficeLanguageUninstallationService SfbOfficeLanguageUninstallationService = new SfbOfficeLanguageUninstallationService(runCmdCommandService, closeSfbClientService);
            LoadHistoryService loadHistoryService = new LoadHistoryService();
            //act
            var viewModel = new MainWindowViewModel
            (
              getCurrentOfficeVersionService,
              getInstallationInfoService,
              installOfficeService,
              sfbOfficeUninstallationService,
              SfbOfficeLanguageUninstallationService,
              sfbOfficeLanguageInstallationService,
              loadHistoryService
            );

            viewModel.InstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>
            {
                 new SfbLanguagePackInfo
                {
                    Language = new LocCulture
                    {
                        CultureName ="fi-FI",
                        IsLip=false,
                        EnglishName="Finnish",
                        Lcid = 1005
                    },
                    IsChecked=false,
                    LanguagePackFolderPath="office15LanguagePackFolderPath",
                    LanguagePackInstallationFileName="setup.com",
                },
                  new SfbLanguagePackInfo
                {
                    Language = new LocCulture
                    {
                        CultureName ="fil-PH",
                        IsLip=true,
                        EnglishName="Filipino",
                        Lcid = 1124
                    },
                    IsChecked=false,
                    LanguagePackFolderPath="office15LanguagePackFolderPath",
                    LanguagePackInstallationFileName="setup.com",
                }
            };

            viewModel.OnCheckAllLanguages(null);
            //assert
            Assert.IsFalse(viewModel.InstalledLanguagesPackages.Any(l => l.IsChecked = false));
        }
    }
}