var conversationJoinedMessage = GetValueFromKey("LOCID Participant Join").replace("%@", "");
var audioConnectedInP2PCallMessage = "all participants are connected to audio modality in p2p call";
var verifyAudioConnectedInConferenceMessage = "verify audio modality connected in conference";
var audioConnectedInConferenceMessage = "all participants are connected to audio modality in conference";
var appshareConnectedInConferenceMessage = "all participants are connected to appshare modality in conference";
var dataConnectedInConferenceMessage = "all participants are connected to data modality in conference";
var verifyMutedInConferenceMessage = "verify is muted in conference";
var verifyUnMutedInConferenceMessage = "verify is unmuted in conference";
var mutedMessage = "muted";
var unMutedMessage = "unmuted";
var testMessage = "message";
var replyMessage = "reply to message";
var firstMessage = "first message";
var secondMessage = "second message";
var replySecondMessage = "reply to second message";
var noteText = "Note, ";
var presenterMutedMessage = "A presenter has muted you";
var startPPTShareMessage = "startpptshare";
var startAppShareMessage = "startappshare";
var stopPPTShareMessage = "stoppptshare";
var stopAppShareMessage = "stopappshare";
var holdMessage = "hold";
var lobbyJoinMessage = "Welcome! Lync has sent a message to the meeting presenters letting them know you’re here. Please wait to be admitted. Please do not move away from the application till admitted. value:Welcome! Lync has sent a message to the meeting presenters letting them know you’re here. Please wait to be admitted. Please do not move away from the application till admitted."
var UnholdMessage = "Unhold";
var EndCall = "EndCall";
var verifyAudioHoldInConferenceMessage = "verify audio modality hold in conference";
var audioHeldInConferenceMessage = "call is on hold";
var audioNotHeldInConferenceMesssage = "call is not on hold";
var AcceptString = GetValueFromKey("LOCID Answer Call");
var IgnoreString = GetValueFromKey("LOCID Dismiss Call");
var REJOIN = GetValueFromKey("LOCID Rejoin");
var AVAILABLE = GetValueFromKey("LOCID Available");
var BUSY = GetValueFromKey("LOCID Busy");
var key_Delete = 10;
var key_Send = 0;
var EXPANDED = GetValueFromKey("LOCID Expanded");
var COLLAPSED = GetValueFromKey("LOCID Collapsed");
var IM_MODALITY_BUTTON = GetValueFromKey("LOCID Instant Messaging");
var VOICE_MODALITY_BUTTON = GetValueFromKey("LOCID Voice");
var VIDEO_MODALITY_BUTTON = GetValueFromKey("LOCID Video");
var HOLD_BUTTON = GetValueFromKey("LOCID Hold Call");
var STOP_VIDEO_BUTTON = GetValueFromKey("LOCID Stop Video");
var CAMERA_OFF_BUTTON = GetValueFromKey("LOCID My Camera Off");
var CAMERA_ON_BUTTON = GetValueFromKey("LOCID My Camera On");
var START_VIDEO_BUTTON = GetValueFromKey("LOCID Start Video");
var SWITCH_CAMERA_BUTTON = GetValueFromKey("LOCID Switch Camera");
var videoConnectedInP2PCallMessage = "all participants are connected to video modality in p2p call";
var pausedMessage = "paused";
var resumedMessage = "resumed";

var DEVICELEFT = UIA_DEVICE_ORIENTATION_LANDSCAPELEFT;
var DEVICEPROT = UIA_DEVICE_ORIENTATION_PORTRAIT;

var Target = UIATarget.localTarget();
var MainWindow = UIATarget.localTarget().frontMostApp().mainWindow();
var TabBar = UIATarget.localTarget().frontMostApp().tabBar();

var SIPURI = GetTestCaseParameters("SipUri2");
var PASSWORD = GetTestCaseParameters("Password2");
var PHONENUMBER = GetTestCaseParameters("PhoneNumber2");

var SIPURI_1 = GetTestCaseParameters("SipUri1");
var PASSWORD_1 = GetTestCaseParameters("Password1");
var PHONENUMBER_1 = GetTestCaseParameters("PhoneNumber1");

var SIPURI_2 = GetTestCaseParameters("SipUri3");
var PASSWORD_2 = GetTestCaseParameters("Password3");
var PHONENUMBER_2 = GetTestCaseParameters("PhoneNumber3");

var SIPURI_3 = GetTestCaseParameters("SipUri4");
var PASSWORD_3 = GetTestCaseParameters("Password4");
var PHONENUMBER_3 = GetTestCaseParameters("PhoneNumber4");

var SIPURI_5 = GetTestCaseParameters("SipUri5");
var PASSWORD_5 = GetTestCaseParameters("Password5");
var PHONENUMBER_5 = GetTestCaseParameters("PhoneNumber5");

var BUDDYDISPLAYNAME = "Lync Mobile Test24";
var SELFDISPLAYNAME = "Lync Mobile Test25";

var MEETINGNAME = "iPhoneTest";

var MEETINGNAME_1 = "Canceled: Online Meeting (Test)";

var MEETINGNAME_2 = "iPhone DQP";
