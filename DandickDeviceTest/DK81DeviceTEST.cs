using DKCommunication.Dandick.DK81Series;
using Xunit;
using DKCommunication.Dandick.DK81Series;

namespace DandickDeviceTest
{
    public class DK81DeviceTEST
    {
        /// <summary>
        /// ʵ����һ��ָ��ID�Ķ���ID=5
        /// </summary>
        readonly DK81Device dandick = new();   //����ID�����Ƿ�����

        /// <summary>
        /// ���ֲ���
        /// </summary>
        [Fact]
        public void HandshakeTEST()
        {
            dandick.SerialPortInni("com3");
            dandick.Open();
            if (dandick.IsOpen())
            {
                var result = dandick.Handshake();
                if (result.IsSuccess)
                {
                    foreach (var item in result.Content)
                    {
                        System.Console.WriteLine(item.ToString("x2"));
                    }
                    System.Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// ��ʾ�����л�����
        /// </summary>
        [Fact]
        public void SetDisplayPageTEST()
        {
            //var result = dandick.SetDisplayPage(DisplayPage.PagePhase);
            //if (result.IsSuccess)
            //{
            //    Assert.Equal(DK81CommunicationInfo.SetDisplayPageCommandLength, result.Content.Length);
            //}
        }
    }
}
