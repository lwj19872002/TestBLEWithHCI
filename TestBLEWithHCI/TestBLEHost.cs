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

        public TestBLEHost()
        {
            _hc = new BLEHostController();
            _hc.OnDeviceInitDone += Hc_OnDeviceInitDone;
            _hc.OnCMDStatus += Hc_OnCMDStatus;
            _hc.OnScanDeviceInformation += Hc_OnScanDeviceInformation;
            _hc.OnDeviceDiscoveryDone += Hc_OnDeviceDiscoveryDone;

        }

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
            _hc.StartDeviceScan();
            while (!_bToNext) { Thread.Sleep(200); }

            _hc.ClosePort();
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
