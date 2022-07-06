using DKCommunication.Dandick.DK81Series;
using Xunit;

namespace DandickDeviceTest
{
    public class DK81DeviceTEST
    {
        /// <summary>
        /// 实例化一个对象
        /// </summary>
        readonly DK81Device dandick = new();   //TODO 测试ID解析是否正常

        /// <summary>
        /// 握手测试,验证设备信息初始化结果
        /// </summary>
        [Fact]
        public void HandshakeTEST()
        {
            dandick.SerialPortInni("com3");
            dandick.Open();

            var result = dandick.Handshake();
            Assert.True(result.IsSuccess);
            Assert.True(dandick.ID == 0);
            Assert.Equal( "101F-000B1-0045\0",dandick.SN);
            Assert.Equal("V2.7", dandick.Version);
            Assert.True(dandick.Model == "DK-34F1\0");
            Assert.True(dandick.IsACI_Activated == true);
            Assert.True(dandick.IsACU_Activated == true);
            Assert.True(dandick.IsDCU_Activated == true);
            Assert.True(dandick.IsDCI_Activated == true);
            Assert.True(dandick.IsDCM_Activated == true);
            Assert.True(dandick.IsPQ_Activated == true);
        }

        /// <summary>
        /// 显示界面切换测试
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
