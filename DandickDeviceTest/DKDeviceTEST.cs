using DKCommunication.Dandick;
using DKCommunication.Dandick.DK81Series;
using Xunit;
using System.IO.Ports;

namespace DandickDeviceTest
{
    public class DKDeviceTEST
    {
        readonly DandickSource dandick = new(DK_DeviceModel.DK_34B1);
        

        [Fact]
        public void HandshakeTEST( )
        {
            dandick.SerialPortInni("com6");
            dandick.Open();
            if (dandick.IsOpen())
            {
                var result = dandick.Handshake();
                if (dandick.Handshake().IsSuccess)
                {
                    Assert.Equal(DK81CommunicationInfo.HandShakeCommandLength, result.Content.Length);
                }
            }
                      
        }

        [Fact]
        public void SetDisplayPageTEST( )
        {
            var result = dandick.SetDisplayPage(DisplayPage.PagePhase);
            if (result.IsSuccess)
            {
                Assert.Equal(DK81CommunicationInfo.SetDisplayPageCommandLength, result.Content.Length);
            }
        }
    }
}
