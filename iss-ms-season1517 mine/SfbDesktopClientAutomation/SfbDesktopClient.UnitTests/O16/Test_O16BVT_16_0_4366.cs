using Microsoft.VisualStudio.TestTools.UnitTesting;
using O16;
using System;
using System.IO;

namespace O15.UnitTests
{
    [TestClass]
    public class Test_O16BVT_16_0_4366
    {
        private static string ResultFolderPath;

        //[ClassInitialize]
        //public static void ClassSetup(TestContext a)
        //{
        //    ResultFolderPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, @"result\O16", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_CallHandling()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 Office16 = new O16BVT_16_0_4366(ResultFolderPath);
        //    Office16.Options_CallHandling("");
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_VideoDevice()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 Office16 = new O16BVT_16_0_4366(ResultFolderPath);
        //    Office16.Options_VideoDevice("");
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_SkypeMeeting()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 Office16 = new O16BVT_16_0_4366(ResultFolderPath);
        //    //Invoke
        //    Office16.Options_SkypeMeetings("");
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_IM()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 Office16 = new O16BVT_16_0_4366(ResultFolderPath);
        //    Office16.Options_IM("");
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Call_Forwarding_Setting()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 Office16 = new O16BVT_16_0_4366(ResultFolderPath);
        //    Office16.Call_Forwarding_Setting("");
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_Recording()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_Recording("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_PersoanlG()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_PersonalG("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_Phones()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_Phones("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_Alerts()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_Alerts("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_General()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_General("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_Audio_Device_Settings()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_Audio_Device_Settings("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Edit_Location_Dlg()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Edit_Location_Dlg("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_ConferenceJoin_MeetNow()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.ConferenceJoin_MeetNow("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_Status()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_Status("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_SignOut_Audio_Device()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options__SignOut_Audio_Device("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_Options_IM_Emoticons()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_IM_Emoticons("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Options_RingtonesAndSounds()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Options_RingtonesAndSounds("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void O16BVT_16_0_4366_Recording_Publish_Recording()
        //{
        //    //Set up
        //    O16BVT_16_0_4366 office16 = new O16BVT_16_0_4366(ResultFolderPath);

        //    //Invoke
        //    bool result = office16.Recording_Publish_Recording("");

        //    //Assert
        //    Assert.IsTrue(result);
        //}
    }
}