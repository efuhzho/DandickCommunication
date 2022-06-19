
using DKCommunication.Serial;

byte[] bytes = new byte[] { 0x81, 0x00, 0x00, 0x8, 0x00, 0x35, 0x01 };
var Buffer = DK81CRC.CRCcalculator(bytes);

foreach (var item in Buffer)
{
    Console.WriteLine(item.ToString("x2").ToUpper());
}


