using DKCommunication.Dandick.DK81Series;
using Xunit;
using DKCommunication.Core;

namespace DandickDeviceTest
{
    public class DK81DeviceTEST
    {
        readonly DK81Device dandick = new();   //TODO ����ID�����Ƿ�����
        public DK81DeviceTEST()
        {
            dandick.SerialPortInni("com3");
            //dandick.Open();
            dandick.ReceiveTimeout = 5000;
            dandick.SleepTime = 20;
            dandick.ByteTransform.DataFormat = DataFormat.ABCD;
        }
      

        /// <summary>
        /// ���ֲ���,��֤�豸��Ϣ��ʼ�����
        /// </summary>
        [Fact]
        public void HandshakeTEST()
        {
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

            //TODO ReadACSourceRanges();
            //TODO ReadDCSourceRanges();
            //TODO ReadDCMeterRanges();
            dandick.Close();
        }

        /// <summary>
        /// ��ȡ����Դ��λ����
        /// </summary>
        [Fact]
        public void ReadACSourceRangesTEST()
        {
            dandick.Open();

            var result = dandick.ReadACSourceRanges();
            Assert.True(result.IsSuccess);
            Assert.True(dandick.ACI_Ranges[0] == 20F);
            Assert.True(dandick.ACI_Ranges[1] == 5F);
            Assert.True(dandick.ACI_Ranges[2] == 2F);
            Assert.True(dandick.ACI_Ranges[3] == 1F);

            Assert.True(dandick.ACU_Ranges[0] == 380F);
            Assert.True(dandick.ACU_Ranges[1] == 220F);
            Assert.True(dandick.ACU_Ranges[2] == 100F);
            Assert.True(dandick.ACU_Ranges[3] == 57.7F);

            dandick.Close();
        }

        /// <summary>
        /// ϵͳģʽ����
        /// </summary>
        [Fact]
        public void SetSystemModeTEST()
        {
            dandick.Open();
            var result = dandick.SetSystemMode(SystemMode.ModeDCMeterCalibrate);
            Assert.True(result.IsSuccess==true);
            Assert.True(result.Content[5] == 0x4b);
            Assert.True(result.Content[6] == 0x4b);
            dandick.Close();
        }
        /// <summary>
        /// ��ʾ�����л�����
        /// </summary>
        [Fact]
        public void SetDisplayPageTEST()
        {
            dandick.Open();
            var result = dandick.SetDisplayPage(DisplayPage.PageDC);
            Assert.True(result.IsSuccess);
            Assert.True(result.Content[5] == 0x4b);
            Assert.True(result.Content[6] == 0x4b);
            dandick.Close();
        }
    }
}
