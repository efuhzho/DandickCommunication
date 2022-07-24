using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using DKCommunication.BasicFramework;
using DKCommunication.Core;
namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的相关信息
    /// </summary>
    internal class DK81CommunicationInfo
    {
        #region 扩展定义
        /// <summary>
        /// 标识丹迪克设备的通信协议类型
        /// </summary>
        //public const DKCommunicationTypes CommunicationType = DKCommunicationTypes.DK81CommunicationType;

        #endregion       

        /*******************/

        #region FrameID 报文头0x81
        /// <summary>
        /// 报文头必须为0x81
        /// </summary>
        public const byte FrameID = 0x81;
        #endregion

        /*******************/

        #region CommandCode Declaration 命令码定义

        #region 系统
        /// <summary>
        /// 系统应答命令:OK   ('K')
        /// </summary>
        public const byte OK = 0x4B;
        public const ushort OKLength = 8;

        /// <summary>
        /// 联机命令，读取终端型号和版本号：Link    ('L')
        /// </summary>
        public const byte HandShake = 0x4C;
        public const ushort HandShakeCommandLength = 7;

        /// <summary>
        /// 设置系统模式
        /// </summary>
        public const byte SetSystemMode = 0x44;
        public const ushort SetSystemModeCommandLength = 8;

        /// <summary>
        /// 发送故障代码，带枚举数据
        /// </summary>
        public const byte ErrorCode = 0x52;

        /// <summary>
        /// 设置当前终端显示界面
        /// </summary>
        public const byte SetDisplayPage = 0x4A;
        public const ushort SetDisplayPageCommandLength = 8;
        #endregion

        #region 交流源/表
        /// <summary>
        /// 交流源关闭命令
        /// </summary>
        public const byte StopACSource = 0x4F;  //2022年7月7日
        public const ushort StopACSourceLength = 7;

        /// <summary>
        /// 交流源打开命令
        /// </summary>
        public const byte StartACSource = 0x54; //2022年7月7日
        public const ushort StartACSourceLength = 7;

        /// <summary>
        /// 设置交流源档位参数 
        /// </summary>
        public const byte SetACSourceRanges = 0x31; //2022年7月7日
        public const ushort SetRangesLength = 16;  //!51F具备IPa,IPb,IPc

        /// <summary>
        /// 设置源幅度参数
        /// </summary>
        public const byte WriteACSourceAmplitude = 0x32;    //2022年7月7日
        public const ushort WriteACSourceAmplitudeLength = 43; //!51F具备IPa,IPb,IPc

        /// <summary>
        /// 设置源相位参数
        /// </summary>
        public const byte WritePhase = 0x33;    //2022年7月8日 10点22分
        public const ushort WritePhaseLength = 31;

        /// <summary>
        /// 设置源频率参数:当 Fa=Fb!=Fc 时，Flag=2；Fa=Fb=Fc 时，Flag=3,只设置Fa则三相同频
        /// </summary>
        public const byte WriteFrequency = 0x34;    //2022年7月8日 12点34分
        public const ushort WriteFrequencyLength = 20;//注意：设置时 Fa=Fb，Fc 可以设置为与 AB 相不同的频率
                                                      //也可以只设置 Fa，则默认为三相同频，用于兼容以前的设备通讯程序

        /// <summary>
        /// 设置源接线模式:
        /// </summary>
        public const byte SetWireMode = 0x35;   //2022年7月8日 19点31分
        public const ushort SetWireModeLength = 8;

        /// <summary>
        /// 闭环控制使能命令：HarmonicMode ：谐波模式，0-以真有效值的百分比输入谐波（有效值恒定）；1-以基波值的百分比输入谐波（基波恒定）
        /// </summary>
        public const byte SetClosedLoop = 0x36;     //2022年7月9日
        public const ushort SetClosedLoopLength = 9;

        /// <summary>
        /// 设置谐波参数：注意：建议协议长度不超过 256，超过 256 个字节建议分批发送。
        /// </summary>
        public const byte WriteHarmonics = 0x58; //2022年7月10日
        public const ushort WriteHarmonicsClearLength = 9;

        /// <summary>
        /// 设置有功功率
        /// </summary>
        public const byte WriteWattPower = 0x50;
        public const ushort WriteWattPowerLength = 12;

        /// <summary>
        /// 设置无功功率
        /// </summary>
        public const byte WriteWattlessPower = 0x51; //TODO 确认协议描述是否有误
        public const byte WriteWattlessPowerLength = 12;

        /// <summary>
        /// 读交流标准表参数/数据：读标准源输出值
        /// </summary>
        public const byte ReadACSourceData = 0x4D;
        public const byte ReadACSourceDataLength = 7;

        /// <summary>
        /// 读系统状态位：Flag=0表示输出稳定，Flag=1表示输出未稳定。：读标准源输出状态
        /// </summary>
        public const byte ReadACStatus = 0x4E;
        public const byte ReadACStatusLength = 7;
        #endregion

        #region 电能
        /// <summary>
        /// 读电能误差
        /// </summary>
        public const byte ReadElectricityDeviation = 0x45;
        public const byte ReadElectricityDeviationLength = 7;

        /// <summary>
        /// 设置电能校验参数并启动电能校验
        /// </summary>
        public const byte WriteElectricity = 0x37;
        public const byte WriteElectricityLength = 32;
        #endregion

        #region 直流表
        /// <summary>
        /// 设置直流表量程
        /// </summary>
        public const byte SetDCMeterRange = 0x61;
        public const byte SetDCMeterRangeLength = 9;

        /// <summary>
        /// 读直流表测量参数/数据
        /// </summary>
        public const byte ReadDCMeterData = 0x62;
        public const byte ReadDCMeterDataLength = 7;

        /// <summary>
        /// 设置直流表测量类型：DCU or DCI
        /// </summary>
        public const byte SetDCMeterMesureType = 0x63;

        /// <summary>
        /// 设置直流表测量参数/数据，适用于双通道
        /// </summary>
        public const byte SetDCMeterDataWithTwoCh = 0x64;

        /// <summary>
        /// 读直流表测量参数/数据，适用于双通道
        /// </summary>
        public const byte ReadDCMeterDataWithTwoCh = 0x65;
        #endregion

        #region 直流源
        /// <summary>
        /// 设置直流源档位参数
        /// </summary>
        public const byte SetDCSourceRange = 0x66;

        /// <summary>
        /// 打开直流源
        /// </summary>
        public const byte StartDCSource = 0x67;
        public const byte StartDCSourceLength = 8;

        /// <summary>
        /// 关闭直流源
        /// </summary>
        public const byte StopDCSource = 0x68;

        /// <summary>
        /// 设置直流源幅值
        /// </summary>
        public const byte WriteDCSourceAmplitude = 0x69;

        /// <summary>
        /// 读直流源参数/数据
        /// </summary>
        public const byte ReadDCSourceData = 0x79;
        #endregion

        #region 校准
        /// <summary>
        /// 清空校准参数，恢复初始状态
        /// </summary>
        public const byte Calibrate_ClearData = 0x20;

        /// <summary>
        /// 切换交流校准档位
        /// </summary>
        public const byte Calibrate_SwitchACRange = 0x21;

        /// <summary>
        /// 确认执行当前校准点的校准数据：在输入标准表数据后执行*！0x22还是0x23存疑，说明书不一致
        /// </summary>
        public const byte Calibrate_DoAC = 0x22;

        /// <summary>
        /// 切换校准点命令
        /// </summary>
        public const byte Calibrate_SwitchACPoint = 0x23;

        /// <summary>
        /// 保存校准参数
        /// </summary>
        public const byte Calibrate_Save = 0x24;

        /// <summary>
        /// 交流标准表和钳形表校准命令
        /// </summary>
        public const byte Calibrate_DoACMeter = 0x25;

        /// <summary>
        /// 设置直流源校准点
        /// </summary>
        public const byte Calibrate_SwitchDCPoint = 0x26;

        /// <summary>
        /// 直流源校准
        /// </summary>
        public const byte Calibrate_DoDC = 0x27;

        /// <summary>
        /// 直流表校准
        /// </summary>
        public const byte Calibrate_DoDCMeter = 0x28;
        #endregion

        #region 设备信息
        /// <summary>
        /// 读取交流标准源档位信息
        /// </summary>
        public const byte ReadACSourceRanges = 0x11;
        public const byte ReadACSourceRangesLength = 7;

        /// <summary>
        /// 读取直流源档位信息
        /// </summary>
        public const byte ReadDCSourceRanges = 0x12;
        public const byte ReadDCSourceRangesLength = 7;

        /// <summary>
        /// 读取直流表档位/量程信息
        /// </summary>
        public const byte ReadDCMeterRanges = 0x13;
        public const byte ReadDCMeterRangesLength = 7;
        #endregion   

        #endregion

        /*******************/

        #region Static Helper Method

        /// <summary>
        /// Error_Code 解析：从错误码中解析错误信息。此为获取故障码的第一种方式
        /// </summary>
        /// <param name="Error_Code">接收的下位机错误码</param>
        /// <returns>错误消息</returns>
        public static string GetErrorMessageByErrorCode(byte Error_Code)
        {
            string errorMessage = StringResources.Language.ExceptionMessage;

            if ((Error_Code & 0x01) == 0x01)
            {
                errorMessage += StringResources.Language.ErrorUa;
            }
            if ((Error_Code & 0x02) == 0x02)
            {
                errorMessage += StringResources.Language.ErrorUb;
            }
            if ((Error_Code & 0x04) == 0x04)
            {
                errorMessage += StringResources.Language.ErrorUc;
            }
            if ((Error_Code & 0x08) == 0x08)
            {
                errorMessage += StringResources.Language.ErrorIa;
            }
            if ((Error_Code & 0x10) == 0x10)
            {
                errorMessage += StringResources.Language.ErrorIb;
            }
            if ((Error_Code & 0x20) == 0x20)
            {
                errorMessage += StringResources.Language.ErrorIc;
            }
            if ((Error_Code & 0x40) == 0x40)
            {
                errorMessage += StringResources.Language.ErrorDC;
            }
            return errorMessage;
        }

        /// <summary>
        /// FunB解析：从字节解析设备具备了哪些功能
        /// </summary>
        /// <param name="inByte">读取的功能信息</param>
        /// <returns>含有3个功能指示的数组：1为具备功能，0为不具备；Index(0)=直流源，Index(1)=直流表，Index(2)=电能校验</returns>
        public static bool[] GetFunctionB(byte inByte)
        {
            bool[] functions = SoftBasic.ByteToBoolArray(inByte); //0x01:ACS;0x02:ACM;0x04:DCS;0x08:DCM;0x10:PQ            
            bool[] funcB = new bool[5];
            funcB[0] = functions[0]; //交流源
            funcB[1] = functions[1]; //交流表
            funcB[2] = functions[2]; //直流源
            funcB[3] = functions[3]; //直流表
            funcB[4] = functions[4]; //电能校验
            return funcB;
        }
        //TODO 解析FuncS

        /// <summary>
        /// 获取对应的数据的CRC校验码（异或和）
        /// </summary>
        /// <param name="sendBytes">需要校验的数据，不包含CRC字节，包含报文头0x81</param>
        /// <returns>返回CRC校验码</returns>
        public static byte CRCcalculator(byte[] sendBytes)
        {
            byte crc = 0;

            //从第二个字节开始执行异或:忽略报文头
            for (int i = 1; i < sendBytes.Length; i++)
            {
                crc ^= sendBytes[i];
            }
            return crc;
        }

        /// <summary>
        /// 核验接收的下位机数据校验码
        /// </summary>
        /// <param name="responseBytes">下位机回复的报文</param>
        /// <returns>核验结果</returns>
        public static bool CheckCRC(byte[] responseBytes)
        {
            if (responseBytes == null) return false;
            if (responseBytes.Length < 2) return false;

            int length = responseBytes.Length;
            byte[] buf = new byte[length - 1];
            Array.Copy(responseBytes, 0, buf, 0, buf.Length);

            //断言
            byte CRC_Code = CRCcalculator(buf);
            if (CRC_Code == responseBytes[length - 1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion
    }

    /*****************************************************************************************************/

    #region Enum Classes

    #region DK81Device所支持的设备型号
    /// <summary>
    /// DK81Device所支持的设备型号
    /// </summary>
    public enum DK_DeviceModel
    {
        [Description("DK-34B1交流采样变送器检定装置")]
        DK_34B1 = 81,

        [Description("DK-34B2")]
        DK_34B2 = 81,

        [Description("DK-34B3")]
        DK_34B3 = 55,

        [Description("DK-34F1")]
        DK_34F1 = 81,

        [Description("DK-PTS1")]
        DK_PTS1 = 55,
    }
    #endregion

    #region 交流输出档位枚举
    /// <summary>
    /// 交流输出档位枚举
    /// </summary>
    [Flags] //TODO 删除
    public enum RangeAC : byte
    {
        [Description("交流电压380V")]
        ACU_380V = 0b_0000_0000,    //0

        [Description("交流电压220V")]
        ACU_220V = 0b_0000_0001,    //1

        [Description("交流电压100V")]
        ACU_100V = 0b_0000_0010,    //2

        [Description("交流电压57.7V")]
        ACU_57V = 0b_0000_0011,     //3

        [Description("交流电流20A")]
        ACI_20A = 0b_0000_0000,     //0

        [Description("交流电流5A")]
        ACI_5A = 0b_0000_0001,      //1

        [Description("交流电流2A")]
        ACI_2A = 0b_0000_0010,      //2

        [Description("交流电流1A")]
        ACI_1A = 0b_0000_0011,      //3
    }
    #endregion

    #region Mode Declaration 系统模式定义

    public enum SystemMode : byte
    {
        /// <summary>
        /// 标准源模式
        /// </summary>
        ModeDefault = 0,

        /// <summary>
        /// 标准表模式
        /// </summary>
        ModeStandardMeter = 1,

        /// <summary>
        /// 标准表（钳表）模式
        /// </summary>
        ModeStandardMeterClamp = 2,

        //TODO 核实协议里的数字是10进制还是16进制，暂时认为是10进制
        /// <summary>
        /// 标准源校准模式
        /// </summary>
        ModeStandardSourceCalibrate = 10,    //?需确认是0x10还是10

        /// <summary>
        /// 标准表校准模式
        /// </summary>
        ModeStandardMeterCalibrate = 11,

        /// <summary>
        /// 钳表校准模式
        /// </summary>
        ModeStandardClampCalibrate = 12,

        /// <summary>
        /// 直流源校准模式
        /// </summary>
        ModeDCSourceCalibrate = 13,

        /// <summary>
        /// 直流表校准模式
        /// </summary>
        ModeDCMeterCalibrate = 14
    }

    #endregion 系统模式定义

    #region Page Declaration 显示页面定义
    //TODO 确认彩屏显示界面，以下是黑白屏数据，不同型号可能界面不一样

    public enum DisplayPage : byte
    {
        //TODO 使用【Atribute】特性显示信息

        /// <summary>
        /// Menu功能选择界面：仅彩屏有效，黑白屏无效。需先切换到Menu界面才能继续操作显示其他界面
        /// </summary>
        PageMenu = 0,

        /// <summary>
        /// 默认的开机界面：交流源输出界面
        /// </summary>
        PageDefault = 1,

        /// <summary>
        /// 相位输出界面：黑白屏，彩屏是波形显示
        /// </summary>
        PagePhase = 2,     //黑白屏，彩屏是波形显示。

        /// <summary>
        /// 矢量显示界面
        /// </summary>
        PagePhasor = 3,

        /// <summary>
        /// 谐波设置界面
        /// </summary>
        PageHarmony = 4,

        /// <summary>
        /// 电能校验界面
        /// </summary>
        PageElectricity = 8,

        /// <summary>
        /// 直流测量界面
        /// </summary>
        PageDCMeter = 5,

        /// <summary>
        /// 直流输出界面
        /// </summary>
        PageDC = 6,

        /// <summary>
        /// 参数测量界面
        /// </summary>
        PageStandardMeter = 9,

        /// <summary>
        /// 相位测量界面：黑白屏，彩屏是波形显示
        /// </summary>
        PageStandardMeterPhase = 10, //黑白屏，彩屏是波形显示

        /// <summary>
        /// 矢量显示界面
        /// </summary>
        PageStandardMeterPhasor = 11,

        #region 标准表钳表

        /// <summary>
        /// 钳表测量界面
        /// </summary>
        PageClampMesure = 12,

        /// <summary>
        /// 钳表相位测量界面
        /// </summary>
        PageClampPhase = 13, //黑白屏，彩屏为波形显示

        /// <summary>
        /// 钳表测试矢量显示界面
        /// </summary>
        PageClampPhasor = 14,

        #endregion

    }
    #endregion  DisplayPage   

    #region WireMode 接线方式
    public enum WireMode : byte
    {
        /// <summary>
        /// 三相四线制
        /// </summary>
        WireMode_3P4L = 00,

        /// <summary>
        /// 三相三线制
        /// </summary>
        WireMode_3P3L = 01,

        /// <summary>
        /// 单相
        /// </summary>
        WireMode_1P1L = 02,

        /// <summary>
        /// 二线两元件（两个互感器）
        /// </summary>
        WireMode_2Component = 03,

        /// <summary>
        /// 二线三元件（三个互感器）
        /// </summary>
        WireMode_3Component = 04,
    }
    #endregion WireMode

    #region CloseLoop 闭环控制定义、谐波模式
    /// <summary>
    /// 闭环控制定义
    /// </summary>
    public enum CloseLoopMode : byte
    {
        /// <summary>
        /// 闭环
        /// </summary>
        CloseLoop = 0,

        /// <summary>
        /// 开环
        /// </summary>
        OpenLoop = 1
    }

    /// <summary>
    /// 谐波模式
    /// </summary>
    public enum HarmonicMode : byte
    {
        /// <summary>
        /// 以真有效值的百分比输入谐波
        /// </summary>
        ValidValuesConstant = 0,

        /// <summary>
        /// 以基波值的百分比输入谐波
        /// </summary>
        FundamentalConstant = 1
    }
    #endregion

    #region Error_Code Declaration 故障码解析
    /// <summary>
    /// 故障码定义：枚举。此为获取故障信息的第二种方式
    /// </summary>
    [Flags]
    public enum ErrorCode : byte
    {
        ErrorUa = 0b_0000_0001,    // 0x01 // 1
        ErrorUb = 0b_0000_0010,    // 0x02 // 2
        ErrorUc = 0b_0000_0100,    // 0x04 // 4
        ErrorIa = 0b_0000_1000,    // 0x08 // 8
        ErrorIb = 0b_0001_0000,    // 0x10 // 16
        ErrorIc = 0b_0010_0000,    // 0x20 // 32
        ErrorDC = 0b_0100_0000     // 0x40 // 64
    }
    #endregion Error_Code Declaration 故障码解析

    #region 设置谐波参数Channel
    /// <summary>
    /// 谐波设置通道选择
    /// </summary>
    [Flags]
    public enum ChannelsHarmonic : byte
    {
        Channel_Ua = 0b_0000_0001,  // 0x01 // 1
        Channel_Ub = 0b_0000_0010,  // 0x02 // 2
        Channel_Uc = 0b_0000_0100,  // 0x04 // 4
        Channel_Ia = 0b_0000_1000,  // 0x08 // 8
        Channel_Ib = 0b_0001_0000,  // 0x10 // 16
        Channel_Ic = 0b_0010_0000,  // 0x20 // 32
        Channel_U = 0b_0000_0111,   // 0x07 // 7
        Channel_I = 0b_0011_1000,   // 0x38 // 56
        Channel_All = 0b_0011_1111, // 0x3F // 63
    }
    #endregion 设置谐波参数Channel

    #region 设置有功功率 Channel
    /// <summary>
    /// 设置有功功率通道枚举
    /// </summary>
    public enum ChannelWattPower : byte
    {
        Channel_Pa = 0,
        Channel_Pb = 1,
        Channel_Pc = 2,
        Channel_Pall = 3
    }
    #endregion 设置有功功率 Channel

    #region 设置无功功率 Channel
    /// <summary>
    /// 设置有功功率通道枚举
    /// </summary>
    public enum ChannelWattLessPower : byte
    {
        Channel_Qa = 0,
        Channel_Qb = 1,
        Channel_Qc = 2,
        Channel_Qall = 3
    }
    #endregion 设置有功功率 Channel

    #region 电能校验类型
    /// <summary>
    /// 电能校验类型（ 电能测量）
    /// </summary>
    public enum ElectricityType : byte
    {
        /// <summary>
        /// 有功功率
        /// </summary>
        P = 0x50,

        /// <summary>
        /// 无功功率
        /// </summary>
        Q = 0x51
    }
    #endregion 电能校验类型

    #region 直流表测量类型
    /// <summary>
    /// 直流表测量类型
    /// </summary>
    public enum DCMerterMeasureType : byte
    {
        /// <summary>
        /// 直流电压
        /// </summary>
        DCM_Voltage = 0,

        /// <summary>
        /// 直流电流
        /// </summary>
        DCM_Current = 1,

        /// <summary>
        /// 纹波电压
        /// </summary>
        DCM_VoltageRipple = 2,

        /// <summary>
        /// 纹波电流
        /// </summary>
        DCM_CurrentRipple = 3
    }
    #endregion

    #region 直流源操作类型
    /// <summary>
    /// 直流源操作类型
    /// </summary>
    public enum DCSourceType : byte
    {
        DCSourceType_U = (byte)'U', //85;0x55
        DCSourceType_I = (byte)'I', //73;0x49
        DCSourceType_R = (byte)'R'  //82;0x52
    }
    #endregion

    #endregion Enum Classes

    #region Structs
    /// <summary>
    /// 设置谐波参数:9个字节长度
    /// </summary>
    public struct Harmonics
    {

        private byte _harmonicTimes;
        /// <summary>
        /// 谐波次数:2--32次
        /// </summary>
        public byte HarmonicTimes
        {
            get { return _harmonicTimes; }
            set
            {
                if (value > 1 && value < 32)    //谐波次数为2到31次
                {
                    _harmonicTimes = value;
                }
                else
                {
                    MessageBox.Show("谐波次数支持范围为2至31次");
                }
            }
        }

        private float _amplitude;
        /// <summary>
        /// 谐波幅度：0--0.4（0%--40%）
        /// </summary>
        public float Amplitude
        {
            get { return _amplitude; }
            set
            {
                if (value >= 0 && value <= 0.4F)  //谐波幅度叠加不超过40%；
                {
                    _amplitude = value;
                }
                else
                {
                    MessageBox.Show("幅度支持范围为0至0.4，单位‘%’");
                }
            }
        }

        private float _angle;
        /// <summary>
        /// 谐波相位：0--359.99
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                if (value >= 0 && value <= 359.99F)
                {
                    _angle = value;
                }
                else
                {
                    MessageBox.Show("谐波相位支持范围为0.00至359.99，单位‘°’");
                }
            }
        }

        //internal byte[] HarmonicToBytes(Harmonics harmonics)
        //{
        //    byte[] bytes = new byte[9];
        //    bytes[0] = _harmonicTimes;

        //}
    }
    #endregion Structs
}
