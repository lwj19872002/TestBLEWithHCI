using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace BLEHostControl
{
    class HCLinkerLayer
    {
        private SerialPort _port;
        private Task _readTask;
        private bool _taskRun;
        private bool _taskOver;

        public Action<Byte[], int> ReadDataProc { get; set; }

        private Byte[] _readDataBuff;
        private int _readLen;

        private const int DATABUFFLEN = 2048;

        public HCLinkerLayer(Action<Byte[], int> readDataProc)
        {
            if (readDataProc == null)
            {
                throw new ArgumentNullException("readDataProc is NULL");
            }

            _readDataBuff = new byte[DATABUFFLEN];
            _readLen = 0;

            ReadDataProc = readDataProc;

            _port = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);
            _readTask = new Task(new Action(ReadTaskProc), TaskCreationOptions.LongRunning);
            _readTask.Start();
            _taskRun = false;
            _taskOver = false;
        }

        ~HCLinkerLayer()
        {
            _taskRun = false;

            _taskOver = true;
            while (!_readTask.IsCompleted) ;
            _readTask.Dispose();
            
            if (_port.IsOpen)
            {
                _port.Close();
            }
            _port.Dispose();
        }

        public void SetPort(string portName = "COM1", int baudRate = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            if (_port.IsOpen)
            {
                _port.Close();
            }
                
            _port.PortName = portName;
            _port.BaudRate = baudRate;
            _port.Parity = parity;
            _port.DataBits = dataBits;
            _port.StopBits = stopBits;
        }

        public void OpenPort()
        {
            if (_port.IsOpen)
            {
                return;
            }

            _port.Open();
        
        }

        public void ClosePort()
        {
            if (_port.IsOpen)
            {
                _port.Close();
            }
        }
        
        public void StartReadTask()
        {
            _taskRun = false;
            if (!_port.IsOpen)
            {
                _port.Open();
            }
            _taskRun = true;
        }

        public void StopReadTask()
        {
            _taskRun = false;
        }

        private void ReadTaskProc()
        {
            while (!_taskOver)
            {
                if (_taskRun == true)
                {
                    try
                    {
                        _readLen = _port.Read(_readDataBuff, 0, DATABUFFLEN);
                        if ((_readLen > 0) && (ReadDataProc != null))
                        {
                            ReadDataProc(_readDataBuff, _readLen);
                        }
                    }
                    catch
                    {
                        //_taskRun = false;
                        //throw;
                    }
                    
                    Thread.Sleep(200);
                }
            }
        }

        public void WritePort(List<Byte> datas)
        {
            if (_port.IsOpen)
            {
                _port.Write(datas.ToArray(), 0, datas.Count);
            }
        }
    }
}
