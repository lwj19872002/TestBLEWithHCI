using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLEHostControl
{
    public enum PacketType
    {
        /// <summary>
        /// Command Packet
        /// </summary>
        Command = 0x01,
        /// <summary>
        /// Asynchronous Data Packet
        /// </summary>
        AsynData = 0x02,
        /// <summary>
        /// Synchronous Data Packet
        /// </summary>
        SyncData = 0x03,
        /// <summary>
        /// Event Packet
        /// </summary>
        Event = 0x04,
    }

    public enum OpcodeOGF
    {
        /// <summary>
        /// Link Control Commands
        /// </summary>
        LinkControlCMD = 0x01,
        /// <summary>
        /// Link Policy Commands
        /// </summary>
        LinkPolicyCMD = 0x02,
        /// <summary>
        /// Controller and Baseband Commands
        /// </summary>
        ControllerAndBasebandCMD = 0x03,
        /// <summary>
        /// Informational Parameters
        /// </summary>
        InformationalParam = 0x04,
        /// <summary>
        /// Status Parameters
        /// </summary>
        StatusParam = 0x05,
        /// <summary>
        /// Testing Commands
        /// </summary>
        TestingCMD = 0x06,
        /// <summary>
        /// LE Only Commands
        /// </summary>
        LEOnlyCMD = 0x08,
        /// <summary>
        /// Vendor Specific Commands
        /// </summary>
        VendorSpecificCMD = 63,
    }

    public enum Opcode
    {
        /// <summary>
        /// LE Set Event Mask
        /// </summary>
        LESetEventMask = 0x2001,
        /// <summary>
        /// LE Read Buffer Size
        /// </summary>
        LEReadBufferSize = 0x2002,
        /// <summary>
        /// LE Read Local Supported Features
        /// </summary>
        LEReadLocalSupportedFeatures = 0x2003,
        /// <summary>
        /// LE Set Random Address
        /// </summary>
        LESetRandomAddr = 0x2005,
        /// <summary>
        /// LE Set Advertising Parameters
        /// </summary>
        LESetAdvParam = 0x2006,
        /// <summary>
        /// LE Read Advertising Channel TX Power
        /// </summary>
        LEReadAdvChTXPower = 0x2007,
        /// <summary>
        /// LE Set Advertising Data
        /// </summary>
        LESetAdvData = 0x2008,
        /// <summary>
        /// LE Set Scan Response Data
        /// </summary>
        LESetScanRespData = 0x2009,
        /// <summary>
        /// LE Set Advertise Enable
        /// </summary>
        LESetAdvertiseEn = 0x200A,
        /// <summary>
        /// LE Set Scan Parameters
        /// </summary>
        LESetScanParam = 0x200B,
        /// <summary>
        /// LE Set Scan Enable
        /// </summary>
        LESetScanEn = 0x200C,
        /// <summary>
        /// LE Create Connection
        /// </summary>
        LECreateCon = 0x200D,
        /// <summary>
        /// LE Create Connection Cancel
        /// </summary>
        LECreateConCancel = 0x200E,
        /// <summary>
        /// LE Read White List Size
        /// </summary>
        LEReadWhiteListSize = 0x200F,
        /// <summary>
        /// LE Clear White List
        /// </summary>
        LEClearWhiteList = 0x2010,
        /// <summary>
        /// LE Add Device To White List
        /// </summary>
        LEAddDevToWhiteList = 0x2011,
        /// <summary>
        /// LE Remove Device From White List
        /// </summary>
        LERemoveDevFromWhiteList = 0x2012,
        /// <summary>
        /// LE Connection Update
        /// </summary>
        LEConUpdate = 0x2013,
        /// <summary>
        /// LE Set Host Channel Classification
        /// </summary>
        LESetHostChClassification = 0x2014,
        /// <summary>
        /// LE Read Channel Map
        /// </summary>
        LEReadChMap = 0x2015,
        /// <summary>
        /// LE Read Remote Used Features
        /// </summary>
        LEReadRemoteUsedFeatures = 0x2016,
        /// <summary>
        /// LE Encrypt
        /// </summary>
        LEEncrypt = 0x2017,
        /// <summary>
        /// LE Rand
        /// </summary>
        LERand = 0x2018,
        /// <summary>
        /// LE Start Encryption
        /// </summary>
        LEStartEncryption = 0x2019,
        /// <summary>
        /// LE Long Term Key Requested Reply
        /// </summary>
        LELongTermKeyReqReply = 0x201A,
        /// <summary>
        /// LE Long Term Key Requested Negative Reply
        /// </summary>
        LELongTermKeyReqNegativeReply = 0x201B,
        /// <summary>
        /// LE Read Supported States
        /// </summary>
        LEReadSupportedStates = 0x201C,
        /// <summary>
        /// LE Receiver Test
        /// </summary>
        LEReceiverTest = 0x201D,
        /// <summary>
        /// LE Transmitter Test (max TX power for CC2541 is 0 dBm)
        /// </summary>
        LETransTest = 0x201E,
        /// <summary>
        /// LE Test End Command
        /// </summary>
        LETestEndCMD = 0x201F,
        /// <summary>
        /// LE Remote Connection Parameter Request Reply
        /// </summary>
        LERemoteConParamReqReply = 0x2020,
        /// <summary>
        /// LE Remote Connection Parameter Request Negative Reply
        /// </summary>
        LERemoteConParamReqNegativeReply = 0x2021,

        /// <summary>
        /// Disconnect
        /// </summary>
        BTLEDisconnect = 0x0406,
        /// <summary>
        /// Read Remote Version Information
        /// </summary>
        BTLEReadRemoteVerInfo = 0x041D,
        /// <summary>
        /// Set Event Mask
        /// </summary>
        BTLESetEventMask = 0x0C01,
        /// <summary>
        /// Reset
        /// </summary>
        BTLEReset = 0x0C03,
        /// <summary>
        /// Read Transmit Power Level
        /// </summary>
        BTLEReadTransPowerLevel = 0x0C2D,
        /// <summary>
        /// Set Controller To Host Flow Control (optional)
        /// </summary>
        BTLESetCtrlToHostFlowCtrl = 0x0C31,
        /// <summary>
        /// Host Buffer Size (optional)
        /// </summary>
        BTLEHostBufSize = 0x0C33,
        /// <summary>
        /// Host Number Of Completed Packets (optional)
        /// </summary>
        BTLEHostNumOfCompPackets = 0x0C35,
        /// <summary>
        /// Set Event Mask Page 2
        /// </summary>
        BTLESetEventMaskPage = 0x0C63,
        /// <summary>
        /// Read Authenticated Payload Timeout
        /// </summary>
        BTLEReadAuthPayloadTimeout = 0x0C7B,
        /// <summary>
        /// Write Authenticated Payload Timeout
        /// </summary>
        BTLEWriteAuthPayloadTimeout = 0x0C7C,
        /// <summary>
        /// Read Local Version Information
        /// </summary>
        BTLEReadLocalVerInfo = 0x1001,
        /// <summary>
        /// Read Local Supported Commands (optional)
        /// </summary>
        BTLEReadLocalSupCMD = 0x1002,
        /// <summary>
        /// Read Local Supported Features
        /// </summary>
        BTLEReadLocalSupFeatures = 0x1003,
        /// <summary>
        /// Read BD_ADDR
        /// </summary>
        BTLEReadBD_ADDR = 0x1009,
        /// <summary>
        /// Read RSSI
        /// </summary>
        BTLEReadRSSI = 0x1405,

        ///===================Vendor Specific Commands====================
        /// <summary>
        /// HCI Extension Modem Hop Test Tx
        /// </summary>
        HCIExtModemHopTestTx = 0xFC09,
        /// <summary>
        /// HCI Extension Modem Test Rx
        /// </summary>
        HCIExtModemTestRx = 0xFC0A,
        /// <summary>
        /// HCI Extension End Modem Test
        /// </summary>
        HCIExtEndModemTest = 0xFC0B,
        /// <summary>
        /// HCI Extension Set BDADDR
        /// </summary>
        HCIExtSetBDADDR = 0xFC0C,
        /// <summary>
        /// HCI Extension Set SCA
        /// </summary>
        HCIExtSetSCA = 0xFC0D,
        /// <summary>
        /// HCI Extension Enable PTM
        /// </summary>
        HCIExtEnablePTM = 0xFC0E,
        /// <summary>
        /// HCI Extension Set Frequency Tuning
        /// </summary>
        HCIExtSetFreqTuning = 0xFC0F,
        /// <summary>
        /// HCI Extension Save Frequency Tuning
        /// </summary>
        HCIExtSaveFreqTuning = 0xFC10,
        /// <summary>
        /// HCI Extension Set Max DTM Tx Power
        /// </summary>
        HCIExtSetMaxDTMTxPower = 0xFC11,
        /// <summary>
        /// HCI Extension Map PM IO Port
        /// </summary>
        HCIExtMapPMIOPort = 0xFC12,
        /// <summary>
        /// HCI Extension Disconnect Immediate
        /// </summary>
        HCIExtDiscImmediate = 0xFC13,
        /// <summary>
        /// HCI Extension Packet Error Rate
        /// </summary>
        HCIExtPacketErrRate = 0xFC14,
        /// <summary>
        /// HCI Extension Packet Error Rate by Channel
        /// </summary>
        HCIExtPacketErrRatebyCh = 0xFC15,
        /// <summary>
        /// HCI Extension Extend RF Range
        /// </summary>
        HCIExExtendRFRange = 0xFC16,
        /// <summary>
        /// HCI Extension Advertiser Event Notice
        /// </summary>
        HCIExtAdverEventNotice = 0xFC17,
        /// <summary>
        /// HCI Extension Connection Event Notice
        /// </summary>
        HCIExtConnEventNotice = 0xFC18,
        /// <summary>
        /// HCI Extension Halt During RF
        /// </summary>
        HCIExtHaltDuringRF = 0xFC19,
        /// <summary>
        /// HCI Extension Set Slave Latency Override
        /// </summary>
        HCIExtSetSlaveLatencyOverride = 0xFC1A,
        /// <summary>
        /// HCI Extension Build Revision
        /// </summary>
        HCIExtBuildRev = 0xFC1B,
        /// <summary>
        /// HCI Extension Delay Sleep
        /// </summary>
        HCIExtDelaySleep = 0xFC1C,
        /// <summary>
        /// HCI Extension Reset System
        /// </summary>
        HCIExtResetSys = 0xFC1D,
        /// <summary>
        /// HCI Extension Overlapped Processing
        /// </summary>
        HCIExtOverlappedProc = 0xFC1E,
        /// <summary>
        /// HCI Extension Number Completed Packets Limit
        /// </summary>
        HCIExtNumCompletedPacketsLimit = 0xFC1F,
        /// <summary>
        /// HCI Extension Get Connection Information
        /// </summary>
        HCIExtGetConnInfo = 0xFC20,

        /// <summary>
        /// L2CAP Disconnection Request
        /// </summary>
        L2CAPDiscReq = 0xFC86,
        /// <summary>
        /// L2CAP Connection Parameter Update Request
        /// </summary>
        L2CAPConnParamUpdateReq = 0xFC92,
        /// <summary>
        /// L2CAP Connection Request
        /// </summary>
        L2CAPConnReq = 0xFC94,
        /// <summary>
        /// L2CAP Connection Response
        /// </summary>
        L2CAPConnResp = 0xFC95,
        /// <summary>
        /// L2CAP Flow Control Credit
        /// </summary>
        L2CAPFlowCtrlCredit = 0xFC96,
        /// <summary>
        /// L2CAP Data
        /// </summary>
        L2CAPData = 0xFCF0,
        /// <summary>
        /// L2CAP Register PSM
        /// </summary>
        L2CAPRegPSM = 0xFCF1,
        /// <summary>
        /// L2CAP Deregister PSM
        /// </summary>
        L2CAPDeregPSM = 0xFCF2,
        /// <summary>
        /// L2CAP PSM Info
        /// </summary>
        L2CAPPSMInfo = 0xFCF3,
        /// <summary>
        /// L2CAP PSM Channels
        /// </summary>
        L2CAPPSMCh = 0xFCF4,
        /// <summary>
        /// L2CAP Channel Info
        /// </summary>
        L2CAPChInfo = 0xFCF5,

        /// <summary>
        /// ATT Error Response
        /// </summary>
        ATTErrResp = 0xFD01,
        /// <summary>
        /// ATT Exchange MTU Request
        /// </summary>
        ATTExcMTUReq = 0xFD02,
        /// <summary>
        /// ATT Exchange MTU Response
        /// </summary>
        ATTExcMTUResp = 0xFD03,
        /// <summary>
        /// ATT Find Information Request
        /// </summary>
        ATTFindInforReq = 0xFD04,
        /// <summary>
        /// ATT Find Information Response
        /// </summary>
        ATTFindInforResp = 0xFD05,
        /// <summary>
        /// ATT Find By Type Value Request
        /// </summary>
        ATTFindByTypeValReq = 0xFD06,
        /// <summary>
        /// ATT Find By Type Value Response
        /// </summary>
        ATTFindByTypeValResp = 0xFD07,
        /// <summary>
        /// ATT Read By Type Request
        /// </summary>
        ATTReadByTypeReq = 0xFD08,
        /// <summary>
        /// ATT Read By Type Respons
        /// </summary>
        ATTReadByTypeResp = 0xFD09,
        /// <summary>
        /// ATT Read Request
        /// </summary>
        ATTReadReq = 0xFD0A,
        /// <summary>
        /// ATT Read Response
        /// </summary>
        ATTReadResp = 0xFD0B,
        /// <summary>
        /// ATT Read Blob Request
        /// </summary>
        ATTReadBlobReq = 0xFD0C,
        /// <summary>
        /// ATT Read Blob Response
        /// </summary>
        ATTReadBlobResp = 0xFD0D,
        /// <summary>
        /// ATT Read Multiple Request
        /// </summary>
        ATTReadMultReq = 0xFD0E,
        /// <summary>
        /// ATT Read Multiple Response
        /// </summary>
        ATTReadMultResp = 0xFD0F,
        /// <summary>
        /// ATT Read By Group Type Request
        /// </summary>
        ATTReadByGroupTypeReq = 0xFD10,
        /// <summary>
        /// ATT Read By Group Type Response
        /// </summary>
        ATTReadByGroupTypeResp = 0xFD11,
        /// <summary>
        /// ATT Write Request
        /// </summary>
        ATTWriteReq = 0xFD12,
        /// <summary>
        /// ATT Write Response
        /// </summary>
        ATTWriteResp = 0xFD13,
        /// <summary>
        /// ATT Prepare Write Request
        /// </summary>
        ATTPrepareWriteReq = 0xFD16,
        /// <summary>
        /// ATT Prepare Write Response
        /// </summary>
        ATTPrepareWriteResp = 0xFD17,
        /// <summary>
        /// ATT Execute Write Request
        /// </summary>
        ATTExeWriteReq = 0xFD18,
        /// <summary>
        /// ATT Execute Write Response
        /// </summary>
        ATTExeWriteResp = 0xFD19,
        /// <summary>
        /// ATT Handle Value Notification
        /// </summary>
        ATTHandleValNotif = 0xFD1B,
        /// <summary>
        /// ATT Handle Value Indication
        /// </summary>
        ATTHandleValIndi = 0xFD1D,
        /// <summary>
        /// ATT Handle Value Confirmation
        /// </summary>
        ATTHandleValConf = 0xFD1E,

        /// <summary>
        /// GATT Discover Characteristics By UUID
        /// </summary>
        GATTDiscCharaByUUID = 0xFD88,
        /// <summary>
        /// GATT Write Long
        /// </summary>
        GATTWriteLong = 0xFD96,

        /// <summary>
        /// GAP Device Initialization
        /// </summary>
        GAPDevInit = 0xFE00,
        /// <summary>
        /// GAP Configure Device Address
        /// </summary>
        GAPConfDevAddr = 0xFE03,
        /// <summary>
        /// GAP Device Discovery Request
        /// </summary>
        GAPDevDiscReq = 0xFE04,
        /// <summary>
        /// GAP Device Discovery Cancel
        /// </summary>
        GAPDevDiscCancel = 0xFE05,
        /// <summary>
        /// GAP Make Discoverable
        /// </summary>
        GAPMakeDisc = 0xFE06,
        /// <summary>
        /// GAP Update Advertising Data
        /// </summary>
        GAPUpdateAdverData = 0xFE07,
        /// <summary>
        /// GAP End Discoverable
        /// </summary>
        GAPEndDisc = 0xFE08,
        /// <summary>
        /// GAP Establish Link Request
        /// </summary>
        GAPEstablishLinkReq = 0xFE09,
        /// <summary>
        /// GAP Terminate Link Request
        /// </summary>
        GAPTerminateLinkReq = 0xFE0A,
        /// <summary>
        /// GAP Authenticate
        /// </summary>
        GAPAuthenticate = 0xFE0B,
        /// <summary>
        /// GAP Passkey Update
        /// </summary>
        GAPPasskeyUpdate = 0xFE0C,
        /// <summary>
        /// GAP Slave Security Request
        /// </summary>
        GAPSlaveSecurityReq = 0xFE0D,
        /// <summary>
        /// GAP Signable
        /// </summary>
        GAPSignable = 0xFE0E,
        /// <summary>
        /// GAP Bond
        /// </summary>
        GAPBond = 0xFE0F,
        /// <summary>
        /// GAP Terminate Auth
        /// </summary>
        GAPTerminateAuth = 0xFE10,
        /// <summary>
        /// GAP Update Link Parameter Request
        /// </summary>
        GAPUpdateLinkParamReq = 0xFE11,
        /// <summary>
        /// GAP Set Parameter
        /// </summary>
        GAPSetParam = 0xFE30,
        /// <summary>
        /// GAP Get Parameter
        /// </summary>
        GAPGetParam = 0xFE31,
        /// <summary>
        /// GAP Resolve Private Address
        /// </summary>
        GAPResolvePrivateAddr = 0xFE32,
        /// <summary>
        /// GAP Set Advertisement Token
        /// </summary>
        GAPSetAdverToken = 0xFE33,
        /// <summary>
        /// GAP Remove Advertisement Token
        /// </summary>
        GAPRemoveAdverToken = 0xFE34,
        /// <summary>
        /// GAP Update Advertisement Tokens
        /// </summary>
        GAPUpdateAdverTokens = 0xFE35,
        /// <summary>
        /// GAP Bond Set Parameter
        /// </summary>
        GAPBondSetParam = 0xFE36,
        /// <summary>
        /// GAP Bond Get Parameter
        /// </summary>
        GAPBondGetParam = 0xFE37,

        /// <summary>
        /// UTIL Reserved
        /// </summary>
        UTILReserved = 0xFE80,
        /// <summary>
        /// UTIL NV Read
        /// </summary>
        UTILNVRead = 0xFE81,
        /// <summary>
        /// UTIL NV Write
        /// </summary>
        UTILNVWrite = 0xFE82,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved = 0xFF00,
        /// <summary>
        /// User Profiles
        /// </summary>
        UserProfiles = 0xFF80,
    }

    public enum LESubEvent
    {
        /// <summary>
        /// LE Connection Complete
        /// </summary>
        LEConnComplete = 0x01,
        /// <summary>
        /// LE Advertising Report
        /// </summary>
        LEAdverRep = 0x02,
        /// <summary>
        /// LE Connection Update Complete
        /// </summary>
        LEConnUpdateComplete = 0x03,
        /// <summary>
        /// LE Read Remote Used Features Complete
        /// </summary>
        LEReadRemoteUsedFeaturesComplete = 0x04,
        /// <summary>
        /// LE Long Term Key Requested
        /// </summary>
        LELongTermKeyReq = 0x05,
        /// <summary>
        /// LE Remote Connection Parameter Request
        /// </summary>
        LERemoteConnParamReq = 0x06,
    }

    public enum BTBLEEvent
    {
        /// <summary>
        /// LE Event
        /// </summary>
        LEEvent = 0x3E,
        /// <summary>
        /// Disconnection Complete
        /// </summary>
        DiscComplete = 0x05,
        /// <summary>
        /// Encryption Change
        /// </summary>
        EncryptionChange = 0x08,
        /// <summary>
        /// Read Remote Version Information Complete
        /// </summary>
        ReadRemoteVerInfoComplete = 0x0C,
        /// <summary>
        /// Command Complete
        /// </summary>
        CommandComplete = 0x0E,
        /// <summary>
        /// Command Status
        /// </summary>
        CommandStatus = 0x0F,
        /// <summary>
        /// Hardware Error (optional)
        /// </summary>
        HardwareErr = 0x10,
        /// <summary>
        /// Number Of Completed Packets
        /// </summary>
        NumOfCompletedPackets = 0x13,
        /// <summary>
        /// Data Buffer Overflow
        /// </summary>
        DataBuffOverflow = 0x1A,
        /// <summary>
        /// Encryption Key Refresh Complete
        /// </summary>
        EncryptionKeyRefreshComplete = 0x30,
        /// <summary>
        /// Authenticated Payload Timeout Expired
        /// </summary>
        AuthPayloadTimeoutExpired = 0x57,
        /// <summary>
        /// Vendor Specific Event
        /// </summary>
        VendorSpecificEvent = 0xFF,
    }

    public enum SpecEventOpcode
    {
        HCIExtSetRxGain = 0x0400,
        HCIExtSetTxPower = 0x0401,
        HCIExtOnePacketPerEvent = 0x0402,
        HCIExtClockDivideOnHalt = 0x0403,
        HCIExtDeclareNVUsage = 0x0404,
        HCIExtDecrypt = 0x0405,
        HCIExtSetLocalSupportedFeatures = 0x0406,
        HCIExtSetFastTxRespTime = 0x0407,
        HCIExtModemTestTx = 0x0408,
        HCIExtModemHopTestTx = 0x0409,
        HCIExtModemTestRx = 0x040A,
        HCIExtEndModemTest = 0x040B,
        HCIExtSetBDADDR = 0x040C,
        HCIExtSetSCA = 0x040D,
        HCIExtEnablePTM3 = 0x040E,
        HCIExtSetFreqTuning = 0x040F,
        HCIExtSaveFreqTuning = 0x0410,
        HCIExtSetMaxDTMTxPower = 0x0411,
        HCIExtMapPMIOPort = 0x0412,
        HCIExtDiscImmediate = 0x0413,
        HCIExtPacketErrRate = 0x0414,
        HCIExtPacketErrRatebyCh3 = 0x0415,
        HCIExtExtendRFRange = 0x0416,
        HCIExtAdverEventNotice3 = 0x0417,
        HCIExtConnEventNotice3 = 0x0418,
        HCIExtHaltDuringRF = 0x0419,
        HCIExtSetSlaveLatencyOverride = 0x041A,
        HCIExtBuildRev = 0x041B,
        HCIExtDelaySleep = 0x041C,
        HCIExtResetSys = 0x041D,
        HCIExtOverlappedProc = 0x041E,
        HCIExtNumCompPacketsLim = 0x041F,
        HCIExtGetConnInfo = 0x0420,

        L2CAPCMDReject = 0x0481,
        L2CAPConnParamUpdateResp = 0x0493,
        L2CAPConnReq = 0x0494,
        L2CAPChEstablished = 0x04E0,
        L2CAPChTerminated = 0x04E1,
        L2CAPOutOfCredit = 0x04E2,
        L2CAPPeerCreditThreshold = 0x04E3,
        L2CAPSendSDUDone = 0x04E4,
        L2CAPData = 0x04F0,

        ATTErrResp = 0x0501,
        ATTExcMTUReq = 0x0502,
        ATTExcMTUResp = 0x0503,
        ATTFindInfoReq = 0x0504,
        ATTFindInfoReq1 = 0x0505,
        ATTFindByTypeValReq = 0x0506,
        ATTFindByTypeValResp = 0x0507,
        ATTReadByTypeReq = 0x0508,
        ATTReadByTypeResp = 0x0509,
        ATTReadReq = 0x050A,
        ATTReadResp = 0x050B,
        ATTReadBlobReq = 0x050C,
        ATTReadBlobResp = 0x050D,
        ATTReadMultReq = 0x050E,
        ATTReadMultResp = 0x050F,
        ATTReadByGroupTypeReq = 0x0510,
        ATTReadByGroupTypeResp = 0x0511,
        ATTWriteReq = 0x0512,
        ATTWriteResp = 0x0513,
        ATTPrepareWriteReq = 0x0516,
        ATTPrepareWriteResp = 0x0517,
        ATTExeWriteReq = 0x0518,
        ATTExeWriteResp = 0x0519,
        ATTHandleValNoti = 0x051B,
        ATTHandleValIndi = 0x051D,
        ATTHandleValConf = 0x051E,

        /// <summary>
        /// GAP Device Init Done
        /// </summary>
        GAPDevInitDone = 0x0600,
        /// <summary>
        /// GAP Device Discovery
        /// </summary>
        GAPDeviDisc = 0x0601,
        /// <summary>
        /// GAP Advert Data Update Done
        /// </summary>
        GAPAdvertDataUpdateDone = 0x0602,
        /// <summary>
        /// GAP Make Discoverable Done
        /// </summary>
        GAPMakeDiscDone = 0x0603,
        /// <summary>
        /// GAP End Discoverable Done
        /// </summary>
        GAPEndDiscDone = 0x0604,
        /// <summary>
        /// GAP Link Established
        /// </summary>
        GAPLinkEstablished = 0x0605,
        /// <summary>
        /// GAP Link Terminated
        /// </summary>
        GAPLinkTerminated = 0x0606,
        /// <summary>
        /// GAP Link Parameter Update
        /// </summary>
        GAPLinkParamUpdate = 0x0607,
        /// <summary>
        /// GAP Random Address Changed
        /// </summary>
        GAPRandomAddrChanged = 0x0608,
        /// <summary>
        /// GAP Signature Updated
        /// </summary>
        GAPSignatureUpdated = 0x0609,
        /// <summary>
        /// GAP Authentication Complete
        /// </summary>
        GAPAuthComplete = 0x060A,
        /// <summary>
        /// GAP Passkey Needed
        /// </summary>
        GAPPasskeyNeeded = 0x060B,
        /// <summary>
        /// GAP Slave Requested Security
        /// </summary>
        GAPSlaveReqSecurity = 0x060C,
        /// <summary>
        /// GAP Device Information
        /// </summary>
        GAPDevInfor = 0x060D,
        /// <summary>
        /// GAP Bond Complete
        /// </summary>
        GAPBondComplete = 0x060E,
        /// <summary>
        /// GAP Pairing Requested
        /// </summary>
        GAPPairingReq = 0x060F,

        /// <summary>
        /// Command Status
        /// </summary>
        CMDStatus = 0x067F,
    }

    public enum ParamID
    {
        // 1 TGAP_LIM_ADV_TIMEOUT - Maximum time to remain advertising, when in
        // Limited Discoverable mode. In seconds (default 180 seconds).
        TGAPLimAdvTimeout = 0x01,
        // 2 TGAP_GEN_DISC_SCAN - Minimum time to perform scanning, when
        // performing General Discovery proc (mSec).
        TGAPGenDiscScan = 2,
        // 3 TGAP_LIM_DISC_SCAN - Minimum time to perform scanning, when
        // performing Limited Discovery proc (mSec).
        TGAPLimDiscScan = 3,
        // 4 TGAP_CONN_EST_ADV_TIMEOUT - Advertising timeout, when
        // performing Connection Establishment proc (mSec).
        TGAPConnEstAdvTimeout = 4,
        // 5 TGAP_CONN_PARAM_TIMEOUT - Link Layer connection parameter
        // update notification timer, connection parameter update proc (mSec).
        TGAPConnParamTimeout = 5,
        // 6 TGAP_LIM_DISC_ADV_INT_MIN - Minimum advertising interval, when in
        // limited discoverable mode (n * 0.625 mSec).
        TGAPLimDiscAdvIntMin = 6,
        // 7 TGAP_LIM_DISC_ADV_INT_MAX - Maximum advertising interval, when in
        // limited discoverable mode (n * 0.625 mSec).
        TGAPLimDiscAdvIntMax = 7,
        // 8 TGAP_GEN_DISC_ADV_INT_MIN - Minimum advertising interval, when in
        // General discoverable mode (n * 0.625 mSec).
        TGAPGenDiscAdvIntMin = 8,
        // 9 TGAP_GEN_DISC_ADV_INT_MAX - Maximum advertising interval, when in
        // General discoverable mode (n * 0.625 mSec).
        TGAPGenDiscAdvIntMax = 9,
        // 10 TGAP_CONN_ADV_INT_MIN - Minimum advertising interval, when in
        // Connectable mode (n * 0.625 mSec).
        TGAPConnAdvIntMin = 10,
        // 11 TGAP_CONN_ADV_INT_MAX - Maximum advertising interval, when in
        // Connectable mode (n * 0.625 mSec).
        TGAPConnAdvIntMax = 11,
        // 12 TGAP_CONN_SCAN_INT - Scan interval used during Link Layer Initiating
        // state, when in Connectable mode (n * 0.625 mSec).
        TGAPConnScanInt = 12,
        // 13 TGAP_CONN_SCAN_WIND - Scan window used during Link Layer
        // Initiating state, when in Connectable mode (n * 0.625 mSec).
        TGAPConnScanWind = 13,
        // 14 TGAP_CONN_HIGH_SCAN_INT - Scan interval used during Link Layer
        // Initiating state, when in Connectable mode, high duty scan cycle scan
        // paramaters (n * 0.625 mSec).
        TGAPConnHighScanInt = 14,
        // 15 TGAP_CONN_HIGH_SCAN_WIND - Scan window used during Link Layer
        // Initiating state, when in Connectable mode, high duty scan cycle scan
        // paramaters (n * 0.625 mSec).
        TGAPConnHighScanWind = 15,
        // 16 TGAP_GEN_DISC_SCAN_INT - Scan interval used during Link Layer
        // Scanning state, when in General Discovery proc (n * 0.625 mSec).
        TGAPGenDiscScanInt = 16,
        // 17 TGAP_GEN_DISC_SCAN_WIND - Scan window used during Link Layer
        // Scanning state, when in General Discovery proc (n * 0.625 mSec).
        TGAPGenDiscScanWind = 17,
        // 18 TGAP_LIM_DISC_SCAN_INT - Scan interval used during Link Layer
        // Scanning state, when in Limited Discovery proc (n * 0.625 mSec).
        TGAPLimDiscScanInt = 18,
        // 19 TGAP_LIM_DISC_SCAN_WIND - Scan window used during Link Layer
        // Scanning state, when in Limited Discovery proc (n * 0.625 mSec).
        TGAPLimDiscScanWind = 19,
        // 20 TGAP_CONN_EST_ADV - Advertising interval, when using Connection
        // Establishment proc (n * 0.625 mSec). Obsolete - Do not use.
        TGAPConnEstAdv = 20,
        // 21 TGAP_CONN_EST_INT_MIN - Minimum Link Layer connection interval,
        // when using Connection Establishment proc(n* 1.25 mSec).
        TGAPConnEstIntMin = 21,
        // 22 TGAP_CONN_EST_INT_MAX - Maximum Link Layer connection interval,
        // when using Connection Establishment proc(n* 1.25 mSec).
        TGAPConnEstIntMax = 22,
        // 23 TGAP_CONN_EST_SCAN_INT - Scan interval used during Link Layer
        // Initiating state, when using Connection Establishment proc(n* 0.625
        // mSec).
        TGAPConnEstScanInt = 23,
        // 24 TGAP_CONN_EST_SCAN_WIND - Scan window used during Link Layer
        // Initiating state, when using Connection Establishment proc(n* 0.625
        // mSec).
        TGAPConnEstScanWind = 24,
        // 25 TGAP_CONN_EST_SUPERV_TIMEOUT - Link Layer connection
        // supervision timeout, when using Connection Establishment proc(n* 10
        // mSec).
        TGAPConnEstSupervTimeout = 25,
        // 26 TGAP_CONN_EST_LATENCY - Link Layer connection slave latency, when
        // using Connection Establishment proc(in number of connection events)
        TGAPConnEstLatency = 26,
        // 27 TGAP_CONN_EST_MIN_CE_LEN - Local informational parameter about
        // min len of connection needed, when using Connection Establishment proc
        // (n* 0.625 mSec).
        TGAPConnEstMinCeLen = 27,
        // 28 TGAP_CONN_EST_MAX_CE_LEN - Local informational parameter about
        // max len of connection needed, when using Connection Establishment proc
        // (n* 0.625 mSec).
        TGAPConnEstMaxCeLen = 28,
        // 29 TGAP_PRIVATE_ADDR_INT - Minimum Time Interval between private
        // (resolvable) address changes.In minutes (default 15 minutes).
        TGAPPrivateAddrInt = 29,
        // 30 TGAP_CONN_PAUSE_CENTRAL – Central idle timer.In seconds(default
        // 1 second).
        TGAPConnPauseCentral = 30,
        // 31 TGAP_CONN_PAUSE_PERIPHERAL – Minimum time upon connection
        // establishment before the peripheral starts a connection update procedure.
        // In seconds(default 5 seconds).
        TGAPConnPausePeripheral = 31,
        // 32 TGAP_SM_TIMEOUT - SM Message Timeout(milliseconds). Default 30
        // seconds.
        TGAPSmTimeout = 32,
        // 33 TGAP_SM_MIN_KEY_LEN - SM Minimum Key Length supported.Default 7.
        TGAPSmMinKeyLen = 33,
        // 34 TGAP_SM_MAX_KEY_LEN - SM Maximum Key Length supported.Default
        // 16.
        TGAPSmMaxKeyLen = 34,
        // 35 TGAP_FILTER_ADV_REPORTS - Filter duplicate advertising reports.
        // Default TRUE.
        TGAPFilterAdvReports = 35,
        // 36 TGAP_SCAN_RSP_RSSI_MIN - Minimum RSSI required for scan
        // responses to be reported to the app. Default -127.
        TGAPScanRspRssiMin = 36,
        // 37 TGAP_REJECT_CONN_PARAMS- Whether or not to reject Connection
        // Parameter Update Request received on Central device.Default FALSE.
        TGAPRejectConnParams = 37,
        //38 TGAP_GAP_TESTCODE - GAP Test Modes:
        //    0 – Test Mode Off
        //    1 – No Response
        TGAPGapTestcode = 38,
        //39 TGAP_SM_TESTCODE - SM Test Modes:
        //    0 – Test Mode Off
        //    1 – No Response
        //    2 – Send Bad Confirm
        //    3 – Bad Confirm Verify
        //    4 – Send SMP Confirm Message
        TGAPSmTestcode = 39,
        //100 TGAP_GATT_TESTCODE - GATT Test Modes:
        //    0 – Test Mode Off
        //    1 – Ignore incoming request
        //    2 – Forward Prepare Write Request right away
        //    3 – Use Max ATT MTU size with Exchange MTU Response
        //    4 – Corrupt incoming Prepare Write Request data
        TGAPGattTestCode = 100,
        //101 TGAP_ATT_TESTCODE - ATT Test Modes:
        //    0 – Test Mode Off
        //    1 – Do Not authenticate incoming signature
        TGAPAttTestCode = 101,
        //102 TGAP_GGS_TESTCODE - GGS Test Modes:
        //    0 – Test Mode Off
        //    1 – Make Device Name attribute writable
        //    2 – Make Appearance attribute writable
        //    3 – Make Peripheral Privacy Flag attribute writable with authentication
        TGAPGgsTestCode = 102,
        //103 TGAP_L2CAP_TESTCODE – L2CAP Test Modes:
        //    0 – Test Mode Off
        //    1 – Reserved
        //    2 – Refuse connection – LE_PSM not supported
        //    3 – Reserved
        //    4 – Refuse connection – no resources available
        //    5 – Refuse connection – insufficient authentication
        //    6 – Refuse connection – insufficient authorization
        //    7 – Refuse connection – insufficient encryption key size
        //    8 – Refuse connection – insufficient encryption
        //    9 – Allow out of range credits to be sent (0 or exceeding 65535)
        //    10 – Send SDU larger than SDU length specified in first LE-frame of SDU
        //    11 – Send LE-frame when local device has a credit count of zero
        TGAPL2capTestCode = 103,
    }

    public enum ScanMode
    {
        /// <summary>
        /// 0 Non-Discoverable Scan
        /// </summary>
        NonDisc = 0x00,
        /// <summary>
        /// 1 General Mode Scan
        /// </summary>
        General = 1,
        /// <summary>
        /// 2 Limited Mode Scan
        /// </summary>
        Limited = 2,
        /// <summary>
        /// 3 Scan for all devices
        /// </summary>
        AllDev = 3,
    }

    public enum ActiveScan
    {
        /// <summary>
        /// Turn off active scanning (SCAN_REQ)
        /// </summary>
        TurnOff = 0x00,
        /// <summary>
        /// Turn on active scanning (SCAN_REQ)
        /// </summary>
        TurnOn = 0x01,
    }

    public enum WhiteList
    {
        /// <summary>
        /// 0 Don’t use the white list during a scan
        /// </summary>
        Disable = 0x00,
        
        /// <summary>
        /// 1 Use the white list during a scan
        /// </summary>
        Enable = 0x01,
    }
}
