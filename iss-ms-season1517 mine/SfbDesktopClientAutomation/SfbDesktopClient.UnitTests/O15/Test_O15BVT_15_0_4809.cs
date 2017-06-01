using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace O15.UnitTests
{
    [TestClass]
    public class Test_O15BVT_15_0_4809
    {
        //private static string ResultFolderPath;

        //[ClassInitialize]
        //public static void ClassSetup(TestContext a)
        //{
        //    ResultFolderPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, @"result\O15", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4809_Options_CallHandling()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);
        //    //Invoke
        //    office15.Options_CallHandling("");
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_IM()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);
        //    office15.Options_IM("");
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_SkypeMeetings()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_SkypeMeetings("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_Status()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_Status("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_RingtonesAndSounds()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_RingtonesAndSounds("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_IM_Emoticons()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_IM_Emoticons("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_SignOut_Audio_Device()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options__SignOut_Audio_Device("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_Audio_Device_Settings()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_Audio_Device_Settings("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Call_Forwarding_Setting()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);
        //    //Invoke
        //    var result = office15.Call_Forwarding_Setting("");
        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Edit_Location_Dlg()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Edit_Location_Dlg("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_Alerts()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_Alerts("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_General()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_General("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_Phones()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_Phones("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_Recording()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_Recording("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_ConferenceJoin_MeetNow()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.ConferenceJoin_MeetNow("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_Options_PersoanlG()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_PersonalG("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void Office15BVT_15_0_4889_VideoDevice()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);
        //    //Invoke
        //    var result = office15.Options_VideoDevice("");
        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O15BVT_15_0_4366_Recording_Publish_Recording()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Recording_Publish_Recording("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O15BVT_15_0_4366_Option_MyPicture()
        //{
        //    //Set up
        //    O15BVT_15_0_4809 office15 = new O15BVT_15_0_4809(ResultFolderPath);

        //    //Invoke
        //    bool result = office15.Options_MyPicture("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}
    }
}