using DKCommunication.Dandick.DK81Series;
using Xunit;

namespace DandickDeviceTest
{
    public class DK81DeviceTEST
    {
        /// <summary>
        /// ʵ����һ��ָ��ID�Ķ���ID=5
        /// </summary>
        readonly DK81Device dandick = new(5);   //����ID�����Ƿ�����
        
        /// <summary>
        /// ���ֲ���
        /// </summary>
        [Fact]
        public void HandshakeTEST( )
        {  
            
            dandick.SerialPortInni("com6");
            dandick.Open();
            if (dandick.IsOpen())
            {
                //var result = dandick.Handshake();
                //if (dandick.Handshake().IsSuccess)
                //{
                //    Assert.Equal(0x05, result.Content[1]);
                //    Assert.Equal(38, result.Content.Length);
                //    Assert.Equal(DK81CommunicationInfo.HandShake,result.Content[5]);
                //}
                //else 
                //{
                //    Assert.True(result.Content==null);                    
                //}
            }
                      
        }

        /// <summary>
        /// ��ʾ�����л�����
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
