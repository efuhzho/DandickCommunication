
namespace DKCommunicationTEST
{
    public class UnitTest1
    {
       DK81Device device=new DK81Device();
        [Fact]
        public void Handshake()
        {          
          var a=  device.HandShake();
        }
    }
}