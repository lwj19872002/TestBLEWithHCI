using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BLEHostControl;

namespace TestBLEWithHCI
{
    public class TestBLEHost
    {
        private bool _bToNext = false;
        private BLEHostController _hc;
        private List<DeviceInfoVal> _BLEDevs;
        private LinkEstablishedArgs _linkEstablishedDev;

        public void DoTest()
        {
            try
            {
                _hc.OpenPort("COM5");
                Console.WriteLine("Port open -- OK");
            }
            catch (Exception e)
            {
                Console.WriteLine("Open port failed!");
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }

            //_bToNext = false;
            _hc.ConnectToHost();
            Thread.Sleep(200);
            //while (!_bToNext) { Thread.Sleep(20); }

            _bToNext = false;
            _hc.InitHost();
            while (!_bToNext) { Thread.Sleep(20); }

            _bToNext = false;
            _BLEDevs.Clear();
            _hc.StartDeviceScan();
            while (!_bToNext) { Thread.Sleep(20); }
            
            if (_BLEDevs.Count > 0)
            {
                _bToNext = false;
                _hc.StopDeviceScan();
                while (!_bToNext) { Thread.Sleep(20); }
                
                _bToNext = false;
                _hc.EstablishLinkRequest(_BLEDevs[0]);
                while (!_bToNext) { Thread.Sleep(20); }
            }
            else
            {
                Console.WriteLine("No device!");
                Console.ReadKey();
                return;
            }


            _hc.GATTWriteCharValue(_linkEstablishedDev.ConnHandle, 0x000F, 0x01);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.KeyChar == 'q')
                {
                    break;
                }
            }
            
            
            _bToNext = false;
            _hc.GATTWriteCharValue(_linkEstablishedDev.ConnHandle, 0x000F, 0x00);
            _hc.TerminateLink(_linkEstablishedDev);
            while (!_bToNext) { Thread.Sleep(20); }

            _hc.ClosePort();

            Console.ReadKey();
        }

        public TestBLEHost()
        {
            _hc = new BLEHostController();
            _BLEDevs = new List<DeviceInfoVal>();
            _hc.OnDeviceInitDone += Hc_OnDeviceInitDone;
            _hc.OnCMDStatus += Hc_OnCMDStatus;
            _hc.OnScanDeviceInformation += Hc_OnScanDeviceInformation;
            _hc.OnDeviceDiscoveryDone += Hc_OnDeviceDiscoveryDone;
            _hc.OnLinkEstablished += hc_OnLinkEstablished;
            _hc.OnHandleValNotification += hc_OnHandleValNotification;
            _hc.OnLinkTerminated += hc_OnLinkTerminated;

        }

        private void hc_OnLinkTerminated(object sender, LinkTerminatedArgs e)
        {
            if (e.ConnHandle == _linkEstablishedDev.ConnHandle)
            {
                _bToNext = true;

                Console.WriteLine("Device link terminated!");
            }
        }

        private void hc_OnHandleValNotification(object sender, HandleValNotificationArgs e)
        {
            if (e.Status == 0x00)
            {
                Console.WriteLine("Handle data OK");
                Console.WriteLine("ConnHanle: {0:X4}", e.ConnHandle);
                Console.WriteLine("Hanle: {0:X4}", e.Handle);
                Console.Write("Value: ");
                if ((e.Value[0] & 0x01) == 0x00)
                {
                    Console.Write("{0:d}", e.Value[1]);
                }
                else
                {
                    Console.Write("{0:d}", (UInt16)e.Value[1] | ((UInt16)e.Value[2] << 8));
                }
                Console.WriteLine("");

                Console.Write("Value(HEX): ");
                for (int i = 0; i < e.Value.Count; i++)
                {
                    Console.Write(":{0:X2}", e.Value[i]);
                }
                Console.WriteLine(":");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Handle data failed");
            }
            
        }

        private void hc_OnLinkEstablished(object sender, LinkEstablishedArgs e)
        {
            if (e.Status == 0x00)
            {
                _linkEstablishedDev = e;
                Console.WriteLine("Device link established!");
                Console.WriteLine("Addr: {0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}", e.Addr[5], e.Addr[4], e.Addr[3], e.Addr[2], e.Addr[1], e.Addr[0]);
                _bToNext = true;
            }
            else
            {
                Console.WriteLine("Device link failed!");
            }

        }

        private void Hc_OnDeviceDiscoveryDone(object sender, DeviceDiscDoneArgs e)
        {
            Console.WriteLine("Device discovery done! Code: {0:X2}", e.Status);
            _bToNext = true;
        }

        private void Hc_OnScanDeviceInformation(object sender, DeviceInfoVal e)
        {
            Console.WriteLine("Device discovered:");
            Console.WriteLine("AddrType: 0x{0:X2}", (byte)e.AddrTypeVal);
            Console.WriteLine("EventType: 0x{0:X2}", (byte)e.EventTypeVal);
            Console.WriteLine("Addr: {0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}", e.Addr[5], e.Addr[4], e.Addr[3], e.Addr[2], e.Addr[1], e.Addr[0]);
            Console.WriteLine("Device Name: {0}", e.DeviceName);
            Console.WriteLine("Sevices:");
            if (e.CompleteServiceUUIDs.Count > 0)
            {
                for (int i = 0; i < e.CompleteServiceUUIDs.Count; i++)
                {
                    Console.WriteLine("0x{0:X4}", e.CompleteServiceUUIDs[i]);
                    if (e.CompleteServiceUUIDs[i] == 0x180D)
                    {
                        _BLEDevs.Add(e);
                    }
                }
            }

            if (_BLEDevs.Count > 0 )
            {
                _bToNext = true;
            }
            
        }

        private void Hc_OnCMDStatus(object sender, CMDStatusArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.OpcodeVal == Opcode.GAPGetParam)
            {
                Console.WriteLine(((GetParamVal)e.Value).Message);
            }

        }

        private void Hc_OnDeviceInitDone(object sender, DevInitDoneArgs e)
        {

            if (e.Status == 0x00)
            {
                Console.WriteLine("Device init success!");

                string str = string.Format("{0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}",
                    e.DevAddr[5], e.DevAddr[4], e.DevAddr[3], e.DevAddr[2], e.DevAddr[1], e.DevAddr[0]);
                Console.WriteLine("DevAddr: " + str);

                Console.WriteLine("DataPktLen: 0x{0:X2}", e.DataPktLen);

                Console.WriteLine("NumDataPkts: 0x{0:X2}", e.NumDataPkts);

                string strIRK = "";
                for (int i = 0; i < e.IRK.Count; i++)
                {
                    strIRK += string.Format("{0:X2}:", e.IRK[i]);
                }
                Console.WriteLine("IRK: " + strIRK);

                string strCSRK = "";
                for (int i = 0; i < e.CSRK.Count; i++)
                {
                    strCSRK += string.Format("{0:X2}:", e.CSRK[i]);
                }
                Console.WriteLine("CSRK: " + strCSRK);

                _bToNext = true;
            }
            else
            {
                Console.WriteLine("Device init failed!");
                Console.WriteLine("Error:{0:X}", e.Status);
            }
        }
    }
}
