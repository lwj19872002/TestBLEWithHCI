using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLEHostControl
{
    public partial class BLEHostController
    {
        private void SendCommand(Opcode opcode, List<byte> param)
        {
            List<byte> frame = new List<byte>();

            frame.Add((byte)PacketType.Command);
            frame.Add((byte)((UInt16)opcode & 0x00FF));
            frame.Add((byte)(((UInt16)opcode >> 8) & 0x00FF));
            if (param != null)
            {
                frame.Add((byte)param.Count);
                if (param.Count > 0)
                {
                    frame.AddRange(param);
                }
            }
            else
            {
                frame.Add(0x00);
            }
            

            _linker.WritePort(frame);
        }

        /// <summary>
        /// This command is used to setup the device in a GAP Role and should only be called once per reboot. To
        /// enable multiple combinations setup multiple GAP Roles(profileRole parameter).
        /// </summary>
        private void SendGAPDevInitCMD()
        {
            List<byte> param = new List<byte>();
            UInt32 signCounter = 0x00000001;

            // 0x01 GAP_PROFILE_BROADCASTER
            // 0x02 GAP_PROFILE_OBSERVER
            // 0x04 GAP_PROFILE_PERIPHERAL
            // 0x08 GAP_PROFILE_CENTRAL
            param.Add(0x08); // Central

            // Central or Observer only: The device will allocate buffer
            // space for received advertisement packets.The default is
            // 3.The larger the number, the more RAM that is needed
            // and maintained.
            param.Add(0x05); // MaxScanRsps

            // 16 byte Identity Resolving Key (IRK). If this value is all
            // 0’s, the GAP will randomly generate all 16 bytes.This
            // key is used to generate Resolvable Private Addresses.
            for (int i = 0; i < 16; i++) // IRK
            {
                param.Add(0x00);
            }

            // 16 byte Connection Signature Resolving Key (CSRK). If
            // this value is all 0’s, the GAP will randomly generate all 16
            // bytes.This key is used to generate data Signatures.
            for (int i = 0; i < 16; i++) // CSRK
            {
                param.Add(0x00);
            }

            // 32 bit Signature Counter. Initial signature counter.
            param.Add((byte)(signCounter & 0x000000FF));
            param.Add((byte)((signCounter >> 8) & 0x000000FF));
            param.Add((byte)((signCounter >> 16) & 0x000000FF));
            param.Add((byte)((signCounter >> 24) & 0x000000FF));

            SendCommand(Opcode.GAPDevInit, param);
        }

        /// <summary>
        /// Send this command to read a GAP Parameter.
        /// </summary>
        private void SendGAPGetParamCMD(ParamID paramID)
        {
            List<byte> param = new List<byte>();

            _getParamIDList.Enqueue(paramID);

            param.Add((byte)paramID);

            SendCommand(Opcode.GAPGetParam, param);
        }

        private void SendGAPDeviceDiscoveryRequestCMD(ScanMode mode, ActiveScan active, WhiteList white)
        {
            List<byte> param = new List<byte>();

            param.Add((byte)mode);
            param.Add((byte)active);
            param.Add((byte)white);

            SendCommand(Opcode.GAPDevDiscReq, param);
        }

        private void SendGAPDeviceDiscoveryCancelCMD()
        {
            SendCommand(Opcode.GAPDevDiscCancel, null);
        }
    }
}
