
using DKCommunication.Serial;
using System.Text;
using System;
using DKCommunication.BasicFramework;

byte[] bytes = new byte[] { 0XA5, 0X5A, 0X02 , 0X00, 0XE0, 0X00, 0X02, 0X41, 0X01, 0X00, /*0X26, 0X01,*/ 0X96 };

String hexString = "A5 5A 0E 00 E0 00 02 34 01 01 01 00 90 01 00 00 E8 03 0A 00 0A 00 96";///* B7 02 */
var buffer= SoftBasic.HexStringToBytes(hexString);

var result = DK55CRC.CRCcalculator(buffer);
var Buffer = DK55CRC.CRCcalculator(bytes);

//byte[] bytes = {14, 01 ,05, 16,85};
//string buf = "1A253815233F00";
//var A= SoftBasic.HexStringToBytes(buf);



foreach (var item in result)
{
    Console.WriteLine(item.ToString("X2").ToUpper());
}


