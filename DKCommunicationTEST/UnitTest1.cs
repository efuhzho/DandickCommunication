
namespace DKCommunicationTEST
{
    public class UnitTest1
    {
        DandickDevice dandick = new DandickDevice(DK_DeviceModel.DK_34B1,65535);

        [Fact]
        public void HandshakeTest()
        {
            var result = dandick.Handshake();
            if (dandick.Handshake().IsSuccess)
            {
                Assert.Equal(DK81CommunicationInfo.HandShakeCommandLength, result.Content.Length);
            }
        }
        [Fact]
        public void page()
        {
            var result = dandick.SetDisplayPage(DisplayPage.PagePhase);
            if (result.IsSuccess)
            {
                Assert.Equal(DK81CommunicationInfo.SetDisplayPageCommandLength, result.Content.Length);
            }
        }
    }
}