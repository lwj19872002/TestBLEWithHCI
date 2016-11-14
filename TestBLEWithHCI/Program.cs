using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using BLEHostControl;

namespace TestBLEWithHCI
{
    class Program
    {
        static void Main(string[] args)
        {
            BLEHostController hc = new BLEHostController();
            hc.OnDeviceInitDone += Hc_OnDeviceInitDone;
            hc.OnCMDStatus += Hc_OnCMDStatus;

            try
            {
                hc.OpenPort("COM5");
                Console.WriteLine("Port open -- OK");
            }
            catch (Exception e)
            {
                Console.WriteLine("Open port failed!");
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }

            hc.ConnectToHost();

            Thread.Sleep(200);

            hc.InitHost();

            Console.ReadKey();

            hc.ClosePort();

        }

        private static void Hc_OnCMDStatus(object sender, CMDStatusArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.OpcodeVal == Opcode.GAPGetParam)
            {
                Console.WriteLine(((GetParamVal)e.Value).Message);
            }
            
        }

        private static void Hc_OnDeviceInitDone(object sender, DevInitDoneArgs e)
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
            }
            else
            {
                Console.WriteLine("Device init failed!");
                Console.WriteLine("Error:{0:X}", e.Status);
            }
        }
    }
}
