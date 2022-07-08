using DKCommunication.Dandick.DK81Series;
using Xunit;
using DKCommunication.Core;

namespace DandickDeviceTest
{
    public class DK81DeviceTEST
    {
        readonly DK81Device dandick = new();   //TODO 测试ID解析是否正常
        public DK81DeviceTEST()
        {
            dandick.SerialPortInni("com3");          
            dandick.ReceiveTimeout = 1000;
            dandick.SleepTime = 20;
            dandick.ByteTransform.DataFormat = DataFormat.DCBA;
            dandick.SetWireMode(WireMode.WireMode_3P4L);
            //dandick.WireMode =(WireMode)1;           
        }


        /// <summary>
        /// 握手测试,验证设备信息初始化结果
        /// </summary>
        [Fact]
        public void HandshakeTEST()
        {
            dandick.Open();
            var result = dandick.Handshake();
            Assert.True(result.IsSuccess);
            Assert.True(dandick.ID == 0);
            Assert.Equal("101F-000B1-0045\0", dandick.SN);
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
        /// 读取交流源档位测试
        /// </summary>
        [Fact]
        public void ReadACSourceRangesTEST()
        {
            dandick.Open();

            var result = dandick.ReadACSourceRanges();
            Assert.True(result.IsSuccess);
            Assert.True(dandick.ACI_RangesList[0] == 20F);
            Assert.True(dandick.ACI_RangesList[1] == 5F);
            Assert.True(dandick.ACI_RangesList[2] == 2F);
            Assert.True(dandick.ACI_RangesList[3] == 1F);

            Assert.True(dandick.ACU_RangesList[0] == 380F);
            Assert.True(dandick.ACU_RangesList[1] == 220F);
            Assert.True(dandick.ACU_RangesList[2] == 100F);
            Assert.True(dandick.ACU_RangesList[3] == 57.7F);

            dandick.Close();
        }

        /// <summary>
        /// 系统模式设置
        /// </summary>
        [Fact]
        public void SetSystemModeTEST()
        {
            dandick.Open();
            var result = dandick.SetSystemMode(SystemMode.ModeDCMeterCalibrate);
            Assert.True(result.IsSuccess == true);
            Assert.True(result.Content[5] == 0x4b);
            Assert.True(result.Content[6] == 0x4b);
            dandick.Close();
        }
        /// <summary>
        /// 显示界面切换测试
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

        /// <summary>
        /// 交流源关闭命令测试
        /// </summary>
        [Fact]
        public void StopACSourceTEST()
        {
            dandick.Open();
            var result = dandick.StopACSource();
            Assert.True(result.IsSuccess);
            dandick.Close();
        }

        /// <summary>
        /// 交流源打开命令测试
        /// </summary>
        [Fact]
        public void StartACSourceTEST()
        {
            dandick.Open();
            var result = dandick.StartACSource();            
            Assert.True(result.IsSuccess);           
            dandick.Close();
        }


        /// <summary>
        /// 设置交流源档位测试
        /// </summary>
        [Fact]
        public void SetACSourceRangeTEST()
        {

            dandick.Open();
            //dandick.ReadACSourceRanges();
            //var result = dandick.SetACSourceRange(dandick.ACU_RangesList.IndexOf(380F), dandick.ACI_RangesList.IndexOf(20F));
            var result2 = dandick.SetACSourceRange(0, 0);
            //Assert.True(result.IsSuccess);
            Assert.True(result2.IsSuccess);
            dandick.Close();
            
        }

        /// <summary>
        /// 设置交流源幅值测试
        /// </summary>
        [Fact]
        public void WriteACSourceAmplitudeTEST()
        {
            dandick.Open();
            var result = dandick.WriteACSourceAmplitude(57.7f, 100f, 220f, 1f, 2f, 3f);
            var result2 = dandick.WriteACSourceAmplitude(57.7f, 1f);
            Assert.True(result.IsSuccess);
            Assert.True(result2.IsSuccess);
            dandick.Close();
        }

        [Fact]
        public void WritePhaseTEST()
        {
            dandick.Open();            
            var result = dandick.WritePhase(121F,241F,0,121F,241F);
            Assert.True(result.IsSuccess);         
            dandick.Close();
        }

        /// <summary>
        /// 设置频率测试
        /// </summary>
        [Fact]
        public void WriteFrequencyTEST()
        {
            dandick.Open();
            var result = dandick.WriteFrequency(52F);
            var result2 = dandick.WriteFrequency(52F,50F);
            Assert.True(result.IsSuccess);
            Assert.True(result2.IsSuccess);
            dandick.Close();
        }

        /// <summary>
        /// 设置接线模式测试
        /// </summary>
        [Fact]
        public void SetWireModeTEST()
        {
            dandick.Open();
            var result = dandick.SetWireMode(WireMode.WireMode_3Component);
            Assert.True(result.IsSuccess);
            dandick.Close();
        }

        /// <summary>
        /// 设置闭环模式和谐波模式测试
        /// </summary>
        [Fact]
        public void SetClosedLoopTEST()
        {
            dandick.Open();
            var result =  dandick.SetClosedLoop(CloseLoopMode.OpenLoop, HarmonicMode.FundamentalConstant);
            var result2 =  dandick.SetClosedLoop(CloseLoopMode.OpenLoop);
            Assert.True(result.IsSuccess);
            Assert.True(result2.IsSuccess);
            dandick.Close();
        }
    }
}
