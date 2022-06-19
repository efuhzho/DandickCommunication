using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Serial;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
           

            byte[] bytes = new byte[] { 0x81, 0x00, 0x00, 0x8, 0x00, 0x35, 0x01 };
            var Buffer = DKCRC81.CRC8(bytes);

            Console.WriteLine(Buffer);
        }
    }
}
