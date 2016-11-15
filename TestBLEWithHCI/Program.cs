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
            TestBLEHost tester = new TestBLEHost();

            tester.DoTest();

            Console.ReadKey();

        }
    }
}
