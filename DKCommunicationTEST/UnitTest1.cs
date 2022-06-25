
namespace DKCommunicationTEST
{
    public class UnitTest1
    {
        DK81Device device = new DK81Device();

        [Fact]
        public void Handshake()
        {
            var a = device.HandShake();
            var b = device.SetSystemMode(SystemMode.ModeStandardSourceCalibrate);
            var c= device.SetSystemMode(SystemMode.ModeStandardMeterCalibrate);
            
            Assert.True(a[a.Length-1].Equals(0x4b));
            Assert.True(b[b.Length-1].Equals(0x46));
            Assert.True(c[c.Length-1].Equals(0x47));

            
        }
    }
}