using DKCommunication.Serial;
namespace CRCSupportTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            byte[] bytes = new byte[6] {  0x00, 0x00, 0x08, 0x00, 0x35, 0x01 };
            var a = SoftCRC8.CRC8(bytes);
            Console.WriteLine(a);
        }
    }
}