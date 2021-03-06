﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLEHostControl
{
    public class DevInitDoneArgs
    {
        public byte Status { get; set; }
        public List<byte> DevAddr { get; set; }
        public UInt16 DataPktLen { get; set; }
        public byte NumDataPkts { get; set; }
        public List<byte> IRK { get; set; }
        public List<byte> CSRK { get; set; }

        public DevInitDoneArgs()
        {
            DevAddr = new List<byte>();
            IRK = new List<byte>();
            CSRK = new List<byte>();
        }
    }

    public class CMDStatusArgs
    {
        public byte Status { get; set; }
        public Opcode OpcodeVal { get; set; }
        public object Value { get; set; }
        public string Message { get; set; }

        public CMDStatusArgs()
        {
            Status = 0x01;
            OpcodeVal = Opcode.Reserved;
            Value = null;
            Message = "";
        }
    }

    public class GetParamVal
    {
        public ParamID ParamIDVal { get; set; }
        public int DataLen { get; set; }
        public UInt16 ParamValue { get; set; }
        public string Message { get; set; }

        public GetParamVal()
        {
            Message = "";
        }
    }

    public class DeviceSimpleInfoVal
    {
        public EventType EventTypeVal { get; set; }
        public AddrType AddrTypeVal { get; set; }
        public List<byte> Addr { get; set; }

        public DeviceSimpleInfoVal()
        {
            Addr = new List<byte>();
        }
    }

    public class DeviceDiscDoneArgs
    {
        public byte Status { get; set; }
        public byte NumDevs { get; set; }
        public List<DeviceSimpleInfoVal> DevInfo { get; set; }

        public DeviceDiscDoneArgs()
        {
            DevInfo = new List<DeviceSimpleInfoVal>();
        }
    }

    public class DeviceInfoVal
    {
        public EventType EventTypeVal { get; set; }
        public AddrType AddrTypeVal { get; set; }
        public List<byte> Addr { get; set; }
        public byte RSSI { get; set; }
        public byte DataLen { get; set; }
        public List<byte> DataField { get; set; }
        public List<UInt16> CompleteServiceUUIDs { get; set; }
        public string DeviceName { get; set; }

        public DeviceInfoVal()
        {
            Addr = new List<byte>();
            DataField = new List<byte>();
            CompleteServiceUUIDs = new List<ushort>();
            DeviceName = "";
        }
    }

    public class LinkEstablishedArgs
    {
        public byte Status { get; set; }
        public AddrType AddrTypeVal { get; set; }
        public List<byte> Addr { get; set; }
        public UInt16 ConnHandle { get; set; }
        public byte ConnRole { get; set; }
        public UInt16 ConnInterval { get; set; }
        public UInt16 ConnLatency { get; set; }
        public UInt16 ConnTimeout { get; set; }
        public byte ClockAccuracy { get; set; }

        public LinkEstablishedArgs()
        {
            Addr = new List<byte>();
        }
    }

    public class HandleValNotificationArgs
    {
        public byte Status { get; set; }
        public UInt16 ConnHandle { get; set; }
        public byte PduLen { get; set; }
        public UInt16 Handle { get; set; }
        public List<byte> Value { get; set; }

        public HandleValNotificationArgs()
        {
            Value = new List<byte>();
        }
    }

    public class LinkTerminatedArgs
    {
        public byte Status { get; set; }
        public UInt16 ConnHandle { get; set; }
        public byte Reason { get; set; }

    }

    public partial class BLEHostController
    {
        private List<byte> _dataBuf = new List<byte>();

        /// <summary>
        /// USB Dongle 初始化完成后的事件
        /// </summary>
        public event EventHandler<DevInitDoneArgs> OnDeviceInitDone;
        /// <summary>
        /// 每个CMD发送完后返回的事件
        /// </summary>
        public event EventHandler<CMDStatusArgs> OnCMDStatus;
        /// <summary>
        /// 扫描过程中，如果每扫描到一个设备就会触发一次此事件
        /// </summary>
        public event EventHandler<DeviceInfoVal> OnScanDeviceInformation;
        /// <summary>
        /// 扫描完成事件
        /// </summary>
        public event EventHandler<DeviceDiscDoneArgs> OnDeviceDiscoveryDone;
        /// <summary>
        /// 连接完成事件
        /// </summary>
        public event EventHandler<LinkEstablishedArgs> OnLinkEstablished;
        /// <summary>
        /// 服务器返回Notification数据是的事件
        /// </summary>
        public event EventHandler<HandleValNotificationArgs> OnHandleValNotification;
        /// <summary>
        /// 连接中断事件
        /// </summary>
        public event EventHandler<LinkTerminatedArgs> OnLinkTerminated;


        private void ReadDataProc(Byte[] datas, int iLen)
        {
            int iProIndex = 0;

            for (int i = 0; i < iLen; i++)
            {
                _dataBuf.Add(datas[i]);
            }

            while (true)
            {
                if ((_dataBuf.Count - iProIndex) > 0)
                {
                    if (_dataBuf[iProIndex] == (byte)PacketType.Event)
                    {
                        if ((_dataBuf.Count - iProIndex) > 3)
                        {
                            if ((_dataBuf.Count - iProIndex) >= (_dataBuf[iProIndex+2] + 3))
                            {
                                List<byte> eventFrame = new List<byte>();
                                for (int i = 0; i < (_dataBuf[iProIndex + 2] + 3); i++)
                                {
                                    eventFrame.Add(_dataBuf[iProIndex + i]);
                                }
                                // 处理数据帧
                                ProcessEventFrame(eventFrame);

                                // 标记已经处理过的数据，最后删除
                                iProIndex += eventFrame.Count;
                            }
                            else { break;} // 数据不全，退出
                        }
                        else { break;}// 数据不全，退出
                    }
                    else if (_dataBuf[iProIndex] == (byte)PacketType.AsynData)
                    {

                    }
                    else // 错误数据不能解析，删除
                    {
                        iProIndex++;
                    }
                }
                else { break;} // 数据处理完了或者没有数据需要处理
            }

            // 删除需要删除的数据，包括已处理的数据以及无效数据
            if (iProIndex > 0)
            {
                _dataBuf.RemoveRange(0, iProIndex);
            }
        }

        private void ProcessEventFrame(List<byte> frame)
        {
            switch ((BTBLEEvent)frame[1])
            {
                case BTBLEEvent.LEEvent:
                    break;
                case BTBLEEvent.DiscComplete:
                    break;
                case BTBLEEvent.EncryptionChange:
                    break;
                case BTBLEEvent.ReadRemoteVerInfoComplete:
                    break;
                case BTBLEEvent.CommandComplete:
                    break;
                case BTBLEEvent.CommandStatus:
                    break;
                case BTBLEEvent.HardwareErr:
                    break;
                case BTBLEEvent.NumOfCompletedPackets:
                    break;
                case BTBLEEvent.DataBuffOverflow:
                    break;
                case BTBLEEvent.EncryptionKeyRefreshComplete:
                    break;
                case BTBLEEvent.AuthPayloadTimeoutExpired:
                    break;
                case BTBLEEvent.VendorSpecificEvent:
                    ProcessVendorSpecificEvent(frame);
                    break;
                default:
                    break;
            }
        }

        private void ProcessVendorSpecificEvent(List<byte> frame)
        {
            List<byte> datas = new List<byte>();

            for (int i = 0; i < frame[2]; i++)
            {
                datas.Add(frame[i + 3]);
            }

            SpecEventOpcode seo = (SpecEventOpcode)((UInt16)datas[0] | ((UInt16)datas[1] << 8));

            switch (seo)
            {
                #region HCIEvent
                case SpecEventOpcode.HCIExtSetRxGain:
                    break;
                case SpecEventOpcode.HCIExtSetTxPower:
                    break;
                case SpecEventOpcode.HCIExtOnePacketPerEvent:
                    break;
                case SpecEventOpcode.HCIExtClockDivideOnHalt:
                    break;
                case SpecEventOpcode.HCIExtDeclareNVUsage:
                    break;
                case SpecEventOpcode.HCIExtDecrypt:
                    break;
                case SpecEventOpcode.HCIExtSetLocalSupportedFeatures:
                    break;
                case SpecEventOpcode.HCIExtSetFastTxRespTime:
                    break;
                case SpecEventOpcode.HCIExtModemTestTx:
                    break;
                case SpecEventOpcode.HCIExtModemHopTestTx:
                    break;
                case SpecEventOpcode.HCIExtModemTestRx:
                    break;
                case SpecEventOpcode.HCIExtEndModemTest:
                    break;
                case SpecEventOpcode.HCIExtSetBDADDR:
                    break;
                case SpecEventOpcode.HCIExtSetSCA:
                    break;
                case SpecEventOpcode.HCIExtEnablePTM3:
                    break;
                case SpecEventOpcode.HCIExtSetFreqTuning:
                    break;
                case SpecEventOpcode.HCIExtSaveFreqTuning:
                    break;
                case SpecEventOpcode.HCIExtSetMaxDTMTxPower:
                    break;
                case SpecEventOpcode.HCIExtMapPMIOPort:
                    break;
                case SpecEventOpcode.HCIExtDiscImmediate:
                    break;
                case SpecEventOpcode.HCIExtPacketErrRate:
                    break;
                case SpecEventOpcode.HCIExtPacketErrRatebyCh3:
                    break;
                case SpecEventOpcode.HCIExtExtendRFRange:
                    break;
                case SpecEventOpcode.HCIExtAdverEventNotice3:
                    break;
                case SpecEventOpcode.HCIExtConnEventNotice3:
                    break;
                case SpecEventOpcode.HCIExtHaltDuringRF:
                    break;
                case SpecEventOpcode.HCIExtSetSlaveLatencyOverride:
                    break;
                case SpecEventOpcode.HCIExtBuildRev:
                    break;
                case SpecEventOpcode.HCIExtDelaySleep:
                    break;
                case SpecEventOpcode.HCIExtResetSys:
                    break;
                case SpecEventOpcode.HCIExtOverlappedProc:
                    break;
                case SpecEventOpcode.HCIExtNumCompPacketsLim:
                    break;
                case SpecEventOpcode.HCIExtGetConnInfo:
                    break;
                #endregion

                #region L2CAPEvent
                case SpecEventOpcode.L2CAPCMDReject:
                    break;
                case SpecEventOpcode.L2CAPConnParamUpdateResp:
                    break;
                case SpecEventOpcode.L2CAPConnReq:
                    break;
                case SpecEventOpcode.L2CAPChEstablished:
                    break;
                case SpecEventOpcode.L2CAPChTerminated:
                    break;
                case SpecEventOpcode.L2CAPOutOfCredit:
                    break;
                case SpecEventOpcode.L2CAPPeerCreditThreshold:
                    break;
                case SpecEventOpcode.L2CAPSendSDUDone:
                    break;
                case SpecEventOpcode.L2CAPData:
                    break;
                #endregion

                #region ATTEvent
                case SpecEventOpcode.ATTErrResp:
                    break;
                case SpecEventOpcode.ATTExcMTUReq:
                    break;
                case SpecEventOpcode.ATTExcMTUResp:
                    break;
                case SpecEventOpcode.ATTFindInfoReq:
                    break;
                case SpecEventOpcode.ATTFindInfoReq1:
                    break;
                case SpecEventOpcode.ATTFindByTypeValReq:
                    break;
                case SpecEventOpcode.ATTFindByTypeValResp:
                    break;
                case SpecEventOpcode.ATTReadByTypeReq:
                    break;
                case SpecEventOpcode.ATTReadByTypeResp:
                    break;
                case SpecEventOpcode.ATTReadReq:
                    break;
                case SpecEventOpcode.ATTReadResp:
                    break;
                case SpecEventOpcode.ATTReadBlobReq:
                    break;
                case SpecEventOpcode.ATTReadBlobResp:
                    break;
                case SpecEventOpcode.ATTReadMultReq:
                    break;
                case SpecEventOpcode.ATTReadMultResp:
                    break;
                case SpecEventOpcode.ATTReadByGroupTypeReq:
                    break;
                case SpecEventOpcode.ATTReadByGroupTypeResp:
                    break;
                case SpecEventOpcode.ATTWriteReq:
                    break;
                case SpecEventOpcode.ATTWriteResp:
                    break;
                case SpecEventOpcode.ATTPrepareWriteReq:
                    break;
                case SpecEventOpcode.ATTPrepareWriteResp:
                    break;
                case SpecEventOpcode.ATTExeWriteReq:
                    break;
                case SpecEventOpcode.ATTExeWriteResp:
                    break;
                case SpecEventOpcode.ATTHandleValNoti:
                    ProcessATTHandleValueNotification(datas);
                    break;
                case SpecEventOpcode.ATTHandleValIndi:
                    break;
                case SpecEventOpcode.ATTHandleValConf:
                    break;
                #endregion

                #region GAPEvent
                case SpecEventOpcode.GAPDevInitDone:
                    ProcessDevInitDone(datas);
                    break;
                case SpecEventOpcode.GAPDeviDisc:
                    ProcessDeviceDiscoveryDone(datas);
                    break;
                case SpecEventOpcode.GAPAdvertDataUpdateDone:
                    break;
                case SpecEventOpcode.GAPMakeDiscDone:
                    break;
                case SpecEventOpcode.GAPEndDiscDone:
                    break;
                case SpecEventOpcode.GAPLinkEstablished:
                    ProcessLinkEstablished(datas);
                    break;
                case SpecEventOpcode.GAPLinkTerminated:
                    ProcessGAPLinkTerminated(datas);
                    break;
                case SpecEventOpcode.GAPLinkParamUpdate:
                    break;
                case SpecEventOpcode.GAPRandomAddrChanged:
                    break;
                case SpecEventOpcode.GAPSignatureUpdated:
                    break;
                case SpecEventOpcode.GAPAuthComplete:
                    break;
                case SpecEventOpcode.GAPPasskeyNeeded:
                    break;
                case SpecEventOpcode.GAPSlaveReqSecurity:
                    break;
                case SpecEventOpcode.GAPDevInfor:
                    ProcessDeviceInformation(datas);
                    break;
                case SpecEventOpcode.GAPBondComplete:
                    break;
                case SpecEventOpcode.GAPPairingReq:
                    break;
                #endregion

                #region OtherEvent
                case SpecEventOpcode.CMDStatus:
                    ProcessCMDStatus(datas);
                    break;
                #endregion
                default:
                    break;
            }
        }
        
        private void ProcessCMDStatus(List<byte> datas)
        {
            CMDStatusArgs args = new CMDStatusArgs();

            args.Status = datas[2];

            // if cmd status is success
            if (args.Status == 0x00)
            {
                args.OpcodeVal = (Opcode)((UInt16)datas[3] | ((UInt16)datas[4] << 8));
                switch (args.OpcodeVal)
                {
                    #region LEOpcode
                    case Opcode.LESetEventMask:
                        break;
                    case Opcode.LEReadBufferSize:
                        break;
                    case Opcode.LEReadLocalSupportedFeatures:
                        break;
                    case Opcode.LESetRandomAddr:
                        break;
                    case Opcode.LESetAdvParam:
                        break;
                    case Opcode.LEReadAdvChTXPower:
                        break;
                    case Opcode.LESetAdvData:
                        break;
                    case Opcode.LESetScanRespData:
                        break;
                    case Opcode.LESetAdvertiseEn:
                        break;
                    case Opcode.LESetScanParam:
                        break;
                    case Opcode.LESetScanEn:
                        break;
                    case Opcode.LECreateCon:
                        break;
                    case Opcode.LECreateConCancel:
                        break;
                    case Opcode.LEReadWhiteListSize:
                        break;
                    case Opcode.LEClearWhiteList:
                        break;
                    case Opcode.LEAddDevToWhiteList:
                        break;
                    case Opcode.LERemoveDevFromWhiteList:
                        break;
                    case Opcode.LEConUpdate:
                        break;
                    case Opcode.LESetHostChClassification:
                        break;
                    case Opcode.LEReadChMap:
                        break;
                    case Opcode.LEReadRemoteUsedFeatures:
                        break;
                    case Opcode.LEEncrypt:
                        break;
                    case Opcode.LERand:
                        break;
                    case Opcode.LEStartEncryption:
                        break;
                    case Opcode.LELongTermKeyReqReply:
                        break;
                    case Opcode.LELongTermKeyReqNegativeReply:
                        break;
                    case Opcode.LEReadSupportedStates:
                        break;
                    case Opcode.LEReceiverTest:
                        break;
                    case Opcode.LETransTest:
                        break;
                    case Opcode.LETestEndCMD:
                        break;
                    case Opcode.LERemoteConParamReqReply:
                        break;
                    case Opcode.LERemoteConParamReqNegativeReply:
                        break;
                    #endregion
                    #region BTLEOpcode
                    case Opcode.BTLEDisconnect:
                        break;
                    case Opcode.BTLEReadRemoteVerInfo:
                        break;
                    case Opcode.BTLESetEventMask:
                        break;
                    case Opcode.BTLEReset:
                        break;
                    case Opcode.BTLEReadTransPowerLevel:
                        break;
                    case Opcode.BTLESetCtrlToHostFlowCtrl:
                        break;
                    case Opcode.BTLEHostBufSize:
                        break;
                    case Opcode.BTLEHostNumOfCompPackets:
                        break;
                    case Opcode.BTLESetEventMaskPage:
                        break;
                    case Opcode.BTLEReadAuthPayloadTimeout:
                        break;
                    case Opcode.BTLEWriteAuthPayloadTimeout:
                        break;
                    case Opcode.BTLEReadLocalVerInfo:
                        break;
                    case Opcode.BTLEReadLocalSupCMD:
                        break;
                    case Opcode.BTLEReadLocalSupFeatures:
                        break;
                    case Opcode.BTLEReadBD_ADDR:
                        break;
                    case Opcode.BTLEReadRSSI:
                        break;
                    #endregion
                    #region HCIOpcode
                    case Opcode.HCIExtModemHopTestTx:
                        break;
                    case Opcode.HCIExtModemTestRx:
                        break;
                    case Opcode.HCIExtEndModemTest:
                        break;
                    case Opcode.HCIExtSetBDADDR:
                        break;
                    case Opcode.HCIExtSetSCA:
                        break;
                    case Opcode.HCIExtEnablePTM:
                        break;
                    case Opcode.HCIExtSetFreqTuning:
                        break;
                    case Opcode.HCIExtSaveFreqTuning:
                        break;
                    case Opcode.HCIExtSetMaxDTMTxPower:
                        break;
                    case Opcode.HCIExtMapPMIOPort:
                        break;
                    case Opcode.HCIExtDiscImmediate:
                        break;
                    case Opcode.HCIExtPacketErrRate:
                        break;
                    case Opcode.HCIExtPacketErrRatebyCh:
                        break;
                    case Opcode.HCIExExtendRFRange:
                        break;
                    case Opcode.HCIExtAdverEventNotice:
                        break;
                    case Opcode.HCIExtConnEventNotice:
                        break;
                    case Opcode.HCIExtHaltDuringRF:
                        break;
                    case Opcode.HCIExtSetSlaveLatencyOverride:
                        break;
                    case Opcode.HCIExtBuildRev:
                        break;
                    case Opcode.HCIExtDelaySleep:
                        break;
                    case Opcode.HCIExtResetSys:
                        break;
                    case Opcode.HCIExtOverlappedProc:
                        break;
                    case Opcode.HCIExtNumCompletedPacketsLimit:
                        break;
                    case Opcode.HCIExtGetConnInfo:
                        break;
                    #endregion
                    #region L2CAPOpcode
                    case Opcode.L2CAPDiscReq:
                        break;
                    case Opcode.L2CAPConnParamUpdateReq:
                        break;
                    case Opcode.L2CAPConnReq:
                        break;
                    case Opcode.L2CAPConnResp:
                        break;
                    case Opcode.L2CAPFlowCtrlCredit:
                        break;
                    case Opcode.L2CAPData:
                        break;
                    case Opcode.L2CAPRegPSM:
                        break;
                    case Opcode.L2CAPDeregPSM:
                        break;
                    case Opcode.L2CAPPSMInfo:
                        break;
                    case Opcode.L2CAPPSMCh:
                        break;
                    case Opcode.L2CAPChInfo:
                        break;
                    #endregion
                    #region ATTOpcode
                    case Opcode.ATTErrResp:
                        break;
                    case Opcode.ATTExcMTUReq:
                        break;
                    case Opcode.ATTExcMTUResp:
                        break;
                    case Opcode.ATTFindInforReq:
                        break;
                    case Opcode.ATTFindInforResp:
                        break;
                    case Opcode.ATTFindByTypeValReq:
                        break;
                    case Opcode.ATTFindByTypeValResp:
                        break;
                    case Opcode.ATTReadByTypeReq:
                        break;
                    case Opcode.ATTReadByTypeResp:
                        break;
                    case Opcode.ATTReadReq:
                        break;
                    case Opcode.ATTReadResp:
                        break;
                    case Opcode.ATTReadBlobReq:
                        break;
                    case Opcode.ATTReadBlobResp:
                        break;
                    case Opcode.ATTReadMultReq:
                        break;
                    case Opcode.ATTReadMultResp:
                        break;
                    case Opcode.ATTReadByGroupTypeReq:
                        break;
                    case Opcode.ATTReadByGroupTypeResp:
                        break;
                    case Opcode.ATTWriteReq:
                        break;
                    case Opcode.ATTWriteResp:
                        break;
                    case Opcode.ATTPrepareWriteReq:
                        break;
                    case Opcode.ATTPrepareWriteResp:
                        break;
                    case Opcode.ATTExeWriteReq:
                        break;
                    case Opcode.ATTExeWriteResp:
                        break;
                    case Opcode.ATTHandleValNotif:
                        break;
                    case Opcode.ATTHandleValIndi:
                        break;
                    case Opcode.ATTHandleValConf:
                        break;
                    #endregion
                    #region GATTOpcode
                    case Opcode.GATTDiscCharaByUUID:
                        break;
                    case Opcode.GATTWriteLong:
                        break;
                    #endregion
                    #region GAPOpcode
                    case Opcode.GAPDevInit:
                        args.Message = "GAP Device init CMD processed.";
                        break;
                    case Opcode.GAPConfDevAddr:
                        break;
                    case Opcode.GAPDevDiscReq:
                        args.Message = "GAP Device Scan CMD processed.";
                        break;
                    case Opcode.GAPDevDiscCancel:
                        args.Message = "GAP Stop Device Scan CMD processed.";
                        break;
                    case Opcode.GAPMakeDisc:
                        break;
                    case Opcode.GAPUpdateAdverData:
                        break;
                    case Opcode.GAPEndDisc:
                        break;
                    case Opcode.GAPEstablishLinkReq:
                        break;
                    case Opcode.GAPTerminateLinkReq:
                        break;
                    case Opcode.GAPAuthenticate:
                        break;
                    case Opcode.GAPPasskeyUpdate:
                        break;
                    case Opcode.GAPSlaveSecurityReq:
                        break;
                    case Opcode.GAPSignable:
                        break;
                    case Opcode.GAPBond:
                        break;
                    case Opcode.GAPTerminateAuth:
                        break;
                    case Opcode.GAPUpdateLinkParamReq:
                        break;
                    case Opcode.GAPSetParam:
                        break;
                    case Opcode.GAPGetParam:
                        GetParamVal val = ProcessGetParam(datas);
                        args.Value = val;
                        args.Message = "GAP get param CMD processed.";
                        break;
                    case Opcode.GAPResolvePrivateAddr:
                        break;
                    case Opcode.GAPSetAdverToken:
                        break;
                    case Opcode.GAPRemoveAdverToken:
                        break;
                    case Opcode.GAPUpdateAdverTokens:
                        break;
                    case Opcode.GAPBondSetParam:
                        break;
                    case Opcode.GAPBondGetParam:
                        break;
                    #endregion
                    #region UITLOpcode
                    case Opcode.UTILReserved:
                        break;
                    case Opcode.UTILNVRead:
                        break;
                    case Opcode.UTILNVWrite:
                        break;
                    #endregion
                    #region OtherOpcode
                    case Opcode.Reserved:
                        break;
                    case Opcode.UserProfiles:
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            else
            {
                args.Message = "CMD Failed!";
            }

            OnCMDStatus?.BeginInvoke(this, args, null, null);
        }

        private void ProcessDevInitDone(List<byte> datas)
        {
            DevInitDoneArgs args = new DevInitDoneArgs();

            args.Status = datas[2];

            if (args.Status == 0x00)
            {
                args.DevAddr.Clear();
                for (int i = 0; i < 6; i++)
                {
                    args.DevAddr.Add(datas[3 + i]);
                }

                args.DataPktLen = (UInt16)((UInt16)datas[9] | ((UInt16)datas[10] << 8));
                args.NumDataPkts = datas[11];

                args.IRK.Clear();
                for (int i = 0; i < 16; i++)
                {
                    args.IRK.Add(datas[12 + i]);
                }

                args.CSRK.Clear();
                for (int i = 0; i < 16; i++)
                {
                    args.CSRK.Add(datas[28 + i]);
                }
            }

            OnDeviceInitDone?.BeginInvoke(this, args, null, null);
        }

        private GetParamVal ProcessGetParam(List<byte> datas)
        {
            GetParamVal val = new GetParamVal();

            val.DataLen = datas[5];
            val.ParamValue = (UInt16)((UInt16)datas[6] | ((UInt16)datas[7] << 8));
            val.ParamIDVal = _getParamIDList.Dequeue();
            val.Message = string.Format("0x{0:X4}-({0:d})", val.ParamValue);

            #region SaveTheParam
            switch (val.ParamIDVal)
            {
                case ParamID.TGAPLimAdvTimeout:
                    val.Message = "TGAPLimAdvTimeout:" + val.Message;
                    break;
                case ParamID.TGAPGenDiscScan:
                    val.Message = "TGAPGenDiscScan:" + val.Message;
                    break;
                case ParamID.TGAPLimDiscScan:
                    val.Message = "TGAPLimDiscScan:" + val.Message;
                    break;
                case ParamID.TGAPConnEstAdvTimeout:
                    val.Message = "TGAPConnEstAdvTimeout:" + val.Message;
                    break;
                case ParamID.TGAPConnParamTimeout:
                    val.Message = "TGAPConnParamTimeout:" + val.Message;
                    break;
                case ParamID.TGAPLimDiscAdvIntMin:
                    val.Message = "TGAPLimDiscAdvIntMin:" + val.Message;
                    break;
                case ParamID.TGAPLimDiscAdvIntMax:
                    val.Message = "TGAPLimDiscAdvIntMax:" + val.Message;
                    break;
                case ParamID.TGAPGenDiscAdvIntMin:
                    val.Message = "TGAPGenDiscAdvIntMin:" + val.Message;
                    break;
                case ParamID.TGAPGenDiscAdvIntMax:
                    val.Message = "TGAPGenDiscAdvIntMax:" + val.Message;
                    break;
                case ParamID.TGAPConnAdvIntMin:
                    val.Message = "TGAPConnAdvIntMin:" + val.Message;
                    break;
                case ParamID.TGAPConnAdvIntMax:
                    val.Message = "TGAPConnAdvIntMax:" + val.Message;
                    break;
                case ParamID.TGAPConnScanInt:
                    val.Message = "TGAPConnScanInt:" + val.Message;
                    break;
                case ParamID.TGAPConnScanWind:
                    val.Message = "TGAPConnScanWind:" + val.Message;
                    break;
                case ParamID.TGAPConnHighScanInt:
                    val.Message = "TGAPConnHighScanInt:" + val.Message;
                    break;
                case ParamID.TGAPConnHighScanWind:
                    val.Message = "TGAPConnHighScanWind:" + val.Message;
                    break;
                case ParamID.TGAPGenDiscScanInt:
                    val.Message = "TGAPGenDiscScanInt:" + val.Message;
                    break;
                case ParamID.TGAPGenDiscScanWind:
                    val.Message = "TGAPGenDiscScanWind:" + val.Message;
                    break;
                case ParamID.TGAPLimDiscScanInt:
                    val.Message = "TGAPLimDiscScanInt:" + val.Message;
                    break;
                case ParamID.TGAPLimDiscScanWind:
                    val.Message = "TGAPLimDiscScanWind:" + val.Message;
                    break;
                case ParamID.TGAPConnEstAdv:
                    val.Message = "TGAPConnEstAdv:" + val.Message;
                    break;
                case ParamID.TGAPConnEstIntMin:
                    val.Message = "TGAPConnEstIntMin:" + val.Message;
                    break;
                case ParamID.TGAPConnEstIntMax:
                    val.Message = "TGAPConnEstIntMax:" + val.Message;
                    break;
                case ParamID.TGAPConnEstScanInt:
                    val.Message = "TGAPConnEstScanInt:" + val.Message;
                    break;
                case ParamID.TGAPConnEstScanWind:
                    val.Message = "TGAPConnEstScanWind:" + val.Message;
                    break;
                case ParamID.TGAPConnEstSupervTimeout:
                    val.Message = "TGAPConnEstSupervTimeout:" + val.Message;
                    break;
                case ParamID.TGAPConnEstLatency:
                    val.Message = "TGAPConnEstLatency:" + val.Message;
                    break;
                case ParamID.TGAPConnEstMinCeLen:
                    val.Message = "TGAPConnEstMinCeLen:" + val.Message;
                    break;
                case ParamID.TGAPConnEstMaxCeLen:
                    val.Message = "TGAPConnEstMaxCeLen:" + val.Message;
                    break;
                case ParamID.TGAPPrivateAddrInt:
                    val.Message = "TGAPPrivateAddrInt:" + val.Message;
                    break;
                case ParamID.TGAPConnPauseCentral:
                    val.Message = "TGAPConnPauseCentral:" + val.Message;
                    break;
                case ParamID.TGAPConnPausePeripheral:
                    val.Message = "TGAPConnPausePeripheral:" + val.Message;
                    break;
                case ParamID.TGAPSmTimeout:
                    val.Message = "TGAPSmTimeout:" + val.Message;
                    break;
                case ParamID.TGAPSmMinKeyLen:
                    val.Message = "TGAPSmMinKeyLen:" + val.Message;
                    break;
                case ParamID.TGAPSmMaxKeyLen:
                    val.Message = "TGAPSmMaxKeyLen:" + val.Message;
                    break;
                case ParamID.TGAPFilterAdvReports:
                    val.Message = "TGAPFilterAdvReports:" + val.Message;
                    break;
                case ParamID.TGAPScanRspRssiMin:
                    val.Message = "TGAPScanRspRssiMin:" + val.Message;
                    break;
                case ParamID.TGAPRejectConnParams:
                    val.Message = "TGAPRejectConnParams:" + val.Message;
                    break;
                case ParamID.TGAPGapTestcode:
                    val.Message = "TGAPGapTestcode:" + val.Message;
                    break;
                case ParamID.TGAPSmTestcode:
                    val.Message = "TGAPSmTestcode:" + val.Message;
                    break;
                case ParamID.TGAPGattTestCode:
                    val.Message = "TGAPGattTestCode:" + val.Message;
                    break;
                case ParamID.TGAPAttTestCode:
                    val.Message = "TGAPAttTestCode:" + val.Message;
                    break;
                case ParamID.TGAPGgsTestCode:
                    val.Message = "TGAPGgsTestCode:" + val.Message;
                    break;
                case ParamID.TGAPL2capTestCode:
                    val.Message = "TGAPL2capTestCode:" + val.Message;
                    break;
                default:
                    break;
            }
            #endregion

            return val;
        }

        private void ProcessDeviceInformation(List<byte> datas)
        {
            DeviceInfoVal devInfo = new DeviceInfoVal();
            
            devInfo.EventTypeVal = (EventType)datas[3];

            devInfo.AddrTypeVal = (AddrType)datas[4];

            devInfo.Addr.Clear();
            for (int i = 0; i < 6; i++)
            {
                devInfo.Addr.Add(datas[5 + i]);
            }

            devInfo.RSSI = datas[11];

            devInfo.DataLen = datas[12];

            devInfo.DataField.Clear();
            for (int i = 0; i < devInfo.DataLen; i++)
            {
                devInfo.DataField.Add(datas[13 + i]);
            }

            int iDex = 0;
            while (iDex < devInfo.DataField.Count)
            {
                List<byte> dataBuf = new List<byte>();
                for (int i = 0; i < devInfo.DataField[iDex] + 1; i++)
                {
                    dataBuf.Add(devInfo.DataField[iDex + i]);
                }

                switch (dataBuf[1])
                {
                    case 0x03: // Complete list of 16 bit service UUIDs.
                        for (int i = 0; i < (dataBuf[0] - 1) / 2; i++)
                        {
                            devInfo.CompleteServiceUUIDs.Add((UInt16)((UInt16)dataBuf[2 + 2 * i] | ((UInt16)dataBuf[3 + i * 2] << 8)));
                        }
                        break;
                    case 0x09: // Complete local device name
                        List<char> temp = new List<char>();
                        for (int i = 0; i < dataBuf[0] - 1; i++)
                        {
                            temp.Add((char)dataBuf[2 + i]);
                        }
                        devInfo.DeviceName = new string(temp.ToArray());
                        break;
                    default:
                        break;
                }

                iDex += (devInfo.DataField[iDex] + 1);
            }

            OnScanDeviceInformation?.BeginInvoke(this, devInfo, null, null);
        }

        private void ProcessDeviceDiscoveryDone(List<byte> datas)
        {
            DeviceDiscDoneArgs args = new DeviceDiscDoneArgs();

            args.Status = datas[2];
            args.NumDevs = datas[3];

            if ((args.Status == 0x00) && (args.NumDevs > 0))
            {
                args.DevInfo.Clear();
                for (int i = 0; i < args.NumDevs; i++)
                {
                    DeviceSimpleInfoVal devInfo = new DeviceSimpleInfoVal();

                    devInfo.EventTypeVal = (EventType)datas[4 + 8 * i];
                    devInfo.AddrTypeVal = (AddrType)datas[5 + 8 * i];
                    devInfo.Addr.Clear();
                    for (int j = 0; j < 6; j++)
                    {
                        devInfo.Addr.Add(datas[6 + 8 * i + j]);
                    }

                    args.DevInfo.Add(devInfo);
                }
            }

            OnDeviceDiscoveryDone?.BeginInvoke(this, args, null, null);
        }

        private void ProcessLinkEstablished(List<byte> datas)
        {
            LinkEstablishedArgs args = new LinkEstablishedArgs();

            args.Status = datas[2];
            args.AddrTypeVal = (AddrType)datas[3];
            for (int i = 0; i < 6; i++)
            {
                args.Addr.Add(datas[4 + i]);
            }
            args.ConnHandle = (UInt16)(datas[10] | ((UInt16)datas[11] << 8));
            args.ConnRole = datas[12];
            args.ConnInterval = (UInt16)(datas[13] | ((UInt16)datas[14] << 8));
            args.ConnLatency = (UInt16)(datas[15] | ((UInt16)datas[16] << 8));
            args.ConnTimeout = (UInt16)(datas[17] | ((UInt16)datas[18] << 8));
            args.ClockAccuracy = datas[19];

            OnLinkEstablished?.BeginInvoke(this, args, null, null);
        }

        private void ProcessATTHandleValueNotification(List<byte> datas)
        {
            HandleValNotificationArgs args = new HandleValNotificationArgs();

            args.Status = datas[2];
            args.ConnHandle = (UInt16)(datas[3] | ((UInt16)datas[4] << 8));
            args.PduLen = datas[5];
            args.Handle = (UInt16)(datas[6] | ((UInt16)datas[7] << 8));

            args.Value.Clear();
            for (int i = 0; i < datas.Count - 8; i++)
            {
                args.Value.Add(datas[8 + i]);
            }

            OnHandleValNotification?.BeginInvoke(this, args, null, null);
        }

        private void ProcessGAPLinkTerminated(List<byte> datas)
        {
            LinkTerminatedArgs args = new LinkTerminatedArgs();

            args.Status = datas[2];
            args.ConnHandle = (UInt16)(datas[3] | ((UInt16)datas[4] << 8));
            args.Reason = datas[5];

            OnLinkTerminated?.BeginInvoke(this, args, null, null);
        }
    }
}
