using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace BLEHostControl
{
    public partial class BLEHostController
    {
        private HCLinkerLayer _linker;
        private Queue<ParamID> _getParamIDList;

        public BLEHostController()
        {
            _getParamIDList = new Queue<ParamID>();
            try
            {
                _linker = new HCLinkerLayer(ReadDataProc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 打开串口，
        /// </summary>
        /// <param name="portName">串口号：COM1</param>
        /// <param name="baudRate">波特率：115200</param>
        /// <param name="parity">奇偶校验</param>
        /// <param name="dataBits">数据长度</param>
        /// <param name="stopBits">停止位</param>
        public void OpenPort(string portName, int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            _linker.SetPort(portName, baudRate, parity, dataBits, stopBits);
            _linker.OpenPort();
            _linker.StartReadTask();
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void ClosePort()
        {
            _linker.StopReadTask();
            _linker.ClosePort();
        }
        
        /// <summary>
        /// 链接USB dongle，发送初始化命令。之前必须先调用OpenPort
        /// </summary>
        public void ConnectToHost()
        {
            SendGAPDevInitCMD();
        }

        /// <summary>
        /// 初始化USB dongle，读取一些配置参数。
        /// </summary>
        public void InitHost()
        {
            Thread.Sleep(50);
            SendGAPGetParamCMD(ParamID.TGAPConnEstIntMin);
            Thread.Sleep(50);
            SendGAPGetParamCMD(ParamID.TGAPConnEstIntMax);
            Thread.Sleep(50);
            SendGAPGetParamCMD(ParamID.TGAPConnEstLatency);
            Thread.Sleep(50);
            SendGAPGetParamCMD(ParamID.TGAPConnEstSupervTimeout);
        }

        /// <summary>
        /// 开始设备扫描
        /// </summary>
        public void StartDeviceScan()
        {
            SendGAPDeviceDiscoveryRequestCMD(ScanMode.AllDev, ActiveScan.TurnOn, WhiteList.Disable);
        }

        /// <summary>
        /// 停止设备扫描
        /// </summary>
        public void StopDeviceScan()
        {
            SendGAPDeviceDiscoveryCancelCMD();
        }

        /// <summary>
        /// 开始请求连接设备
        /// </summary>
        /// <param name="dev">设备信息</param>
        public void EstablishLinkRequest(DeviceInfoVal dev)
        {
            SendEstablishLinkReqCMD(0x00, 0x00, dev.AddrTypeVal, dev.Addr);
        }

        /// <summary>
        /// 终止链接
        /// </summary>
        /// <param name="dev">需要断开连接的设备</param>
        public void TerminateLink(LinkEstablishedArgs dev)
        {
            SendGAPTerminateLinkRequestCMD(dev.ConnHandle, 0x13);
        }

        /// <summary>
        /// 写Char到服务器
        /// </summary>
        /// <param name="connHandle"></param>
        /// <param name="handle"></param>
        /// <param name="val"></param>
        public void GATTWriteCharValue(UInt16 connHandle, UInt16 handle, byte val)
        {
            SendWriteCharValueCMD(connHandle, handle, val);
        }
    }
}
