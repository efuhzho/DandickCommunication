using DKCommunication.Dandick.DK81Series;
using Xunit;

namespace DandickDeviceTest
{
    public class DK81DeviceTEST
    {
        /// <summary>
        /// 实例化一个指定ID的对象：ID=5
        /// </summary>
        readonly DK81Device dandick = new(5);
        
        /// <summary>
        /// 握手测试
        /// </summary>
        [Fact]
        public void HandshakeTEST( )
        {           
            dandick.SerialPortInni("com3");
            dandick.Open();
            if (dandick.IsOpen())
            {
                var result = dandick.Handshake();
                if (dandick.Handshake().IsSuccess)
                {                    
                    Assert.Equal(result.Content[5],DK81CommunicationInfo.Confirmed);
                }
                else
                {
                    Assert.False(true);
                }
            }
                      
        }

        /// <summary>
        /// 显示界面切换测试
        /// </summary>
        [Fact]
        public void SetDisplayPageTEST( )
        {
            //var result = dandick.SetDisplayPage(DisplayPage.PagePhase);
            //if (result.IsSuccess)
            //{
            //    Assert.Equal(DK81CommunicationInfo.SetDisplayPageCommandLength, result.Content.Length);
            //}
        }
    }
}
