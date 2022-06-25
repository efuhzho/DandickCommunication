using DKCommunication.Dandick.Communication;
using System;

namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的相关信息
    /// </summary>
    public class DK81CommunicationInfo
    {
        #region 扩展定义
        /// <summary>
        /// 标识丹迪克设备的通信协议类型
        /// </summary>
        public const DKCommunicationTypes CommunicationType = DKCommunicationTypes.DK81CommunicationType;

        #endregion       

        #region 报文头0x81
        /// <summary>
        /// 报文头必须为0x81
        /// </summary>
        public const byte FrameID = 0x81;
        #endregion

        #region CommandCode Declaration 命令码定义

        #region 系统
        /// <summary>
        /// 系统应答命令:OK   ('K')
        /// </summary>
        public const byte Confirmed = 0x4B;
        public const ushort ConfirmedLength = 8;

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
        public const byte Stop = 0x4F;
        public const ushort StopLength = 7;

        /// <summary>
        /// 交流源打开命令
        /// </summary>
        public const byte Start = 0x54;
        public const ushort StartLength = 7;

        /// <summary>
        /// 设置源档位参数
        /// </summary>
        public const byte WriteRange = 0x31;
        public const ushort WriteRangeLength = 16;  //!51F具备IPa,IPb,IPc

        /// <summary>
        /// 设置源幅度参数
        /// </summary>
        public const byte WriteAmplitude = 0x32;
        public const ushort WriteAmplitudeLength = 43; //!51F具备IPa,IPb,IPc

        /// <summary>
        /// 设置源相位参数
        /// </summary>
        public const byte WriteSetPhase = 0x33;
        public const ushort WriteSetPhaseLength = 31;

        /// <summary>
        /// 设置源频率参数:当 Fa=Fb!=Fc 时，Flag=2；Fa=Fb=Fc 时，Flag=3,只设置Fa则三相同频
        /// </summary>
        public const byte WriteFrequency = 0x34;
        public const ushort WriteFrequencyLength = 20;//注意：设置时 Fa=Fb，Fc 可以设置为与 AB 相不同的频率
                                                      //也可以只设置 Fa，则默认为三相同频，用于兼容以前的设备通讯程序

        /// <summary>
        /// 设置源接线模式:
        /// </summary>
        public const byte WetWireMode = 0x35;   //TODO 枚举   PAGE 8
        public const ushort WetWireModeLength = 8;

        /// <summary>
        /// 闭环控制使能命令：HarmonicMode ：谐波模式，0-以真有效值的百分比输入谐波（有效值恒定）；1-以基波值的百分比输入谐波（基波恒定）
        /// </summary>
        public const byte WriteClosedLoop = 0x36;
        public const ushort WriteClosedLoopLength = 9;

        /// <summary>
        /// 设置电能校验参数
        /// </summary>
        public const byte WriteElectricity = 0x37;

        /// <summary>
        /// 设置谐波参数：注意：建议协议长度不超过 256，超过 256 个字节建议分批发送。
        /// </summary>
        public const byte WriteHarmonics = 0x58;
        public const short WriteHarmonicsLength = 0x58;

        /// <summary>
        /// 读电能误差
        /// </summary>
        public const byte ReadElectricity = 0x45;

        /// <summary>
        /// 设置有功功率参数
        /// </summary>
        public const byte WriteWattPower = 0x50;

        /// <summary>
        /// 设置无功功率参数
        /// </summary>
        public const byte WriteWattlessPower = 0x51;

        /// <summary>
        /// 读交流标准表参数/数据：读标准源输出值
        /// </summary>
        public const byte ReadACData = 0x4D;

        /// <summary>
        /// 读系统状态位：Flag=0表示输出稳定，Flag=1表示输出未稳定。：读标准源输出状态
        /// </summary>
        public const byte ReadACStatus = 0x4E;
        #endregion

        #region 直流表
        /// <summary>
        /// 设置直流表量程
        /// </summary>
        public const byte SetDCMeterRange = 0x61;

        /// <summary>
        /// 读直流表测量参数/数据
        /// </summary>
        public const byte ReadDCMeterData = 0x62;

        /// <summary>
        /// 设置直流表测量类型：DCU or DCI
        /// </summary>
        public const byte SetDCMeterMesureType = 0x63;

        /// <summary>
        /// *已过时！设置直流表测量参数/数据，适用于双通道
        /// </summary>
        public const byte SetDCMeterDataWithTwoCh = 0x64;

        /// <summary>
        /// *已过时！读直流表测量参数/数据，适用于双通道
        /// </summary>
        public const byte ReadDCMeterDataWithTwoCh = 0x65;
        #endregion

        #region 直流源
        /// <summary>
        /// 设置直流源档位参数
        /// </summary>
        public const byte SetDCRange = 0x66;

        /// <summary>
        /// 打开直流源
        /// </summary>
        public const byte StartDC = 0x67;

        /// <summary>
        /// 关闭直流源
        /// </summary>
        public const byte StopDC = 0x68;

        /// <summary>
        /// 设置直流源幅值
        /// </summary>
        public const byte WriteDCAmplitude = 0x69;

        /// <summary>
        /// 读直流源参数/数据
        /// </summary>
        public const byte ReadDCData = 0x79;
        #endregion

        #region 校准
        /// <summary>
        /// 清空校准参数，恢复初始状态
        /// </summary>
        public const byte ClearCalibratedData = 0x20;

        /// <summary>
        /// 切换交流校准档位
        /// </summary>
        public const byte SwitchACRangeCalibrating = 0x21;

        /// <summary>
        /// 确认执行当前校准点的校准数据：在输入标准表数据后执行*！0x22还是0x23存疑，说明书不一致
        /// </summary>
        public const byte DoCalibrate = 0x22;

        /// <summary>
        /// 切换校准点命令
        /// </summary>
        public const byte SwitchACPointCalibrating = 0x23;

        /// <summary>
        /// 确认交流源校准，保存校准参数
        /// </summary>
        public const byte SaveCalibratedData = 0x24;

        /// <summary>
        /// 交流标准表和钳形表校准命令
        /// </summary>
        public const byte DoACMeterCalibrate = 0x25;

        /// <summary>
        /// 设置直流源校准点
        /// </summary>
        public const byte SwithACPointCalibrating = 0x26;

        /// <summary>
        /// 直流源校准
        /// </summary>
        public const byte DoDCCalibrate = 0x27;

        /// <summary>
        /// 直流表校准
        /// </summary>
        public const byte DoDCMeterCalibrate = 0x28;
        #endregion

        #region 设备信息
        /// <summary>
        /// 读取交流标准源档位信息
        /// </summary>
        public const byte ReadRangeInfo = 0x11;

        /// <summary>
        /// 读取直流源档位信息
        /// </summary>
        public const byte ReadDCRangeInfo = 0x12;

        /// <summary>
        /// 读取直流表档位/量程信息
        /// </summary>
        public const byte ReadDCMeterRangeInfo = 0x13;

        #region 联机命令读取的型号和版本号
        /// <summary>
        /// 终端产品型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Serail { get; set; }

        /// <summary>
        /// 主版本号
        /// </summary>
        public ushort VerA { get; set; }

        /// <summary>
        /// 次版本号
        /// </summary>
        public ushort VerB { get; set; }

        /// <summary>
        /// 基本功能状态
        /// </summary>
        public byte FunB { get; set; }

        /// <summary>
        /// 特殊功能状态
        /// </summary>
        public byte FunS { get; set; }
        #endregion

        #endregion

        #endregion

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
        /// <param name="FuncB">读取的功能信息</param>
        /// <returns>含有3个功能指示的数组：1为具备功能，0为不具备；Index(0)=直流源，Index(1)=直流表，Index(2)=电能校验</returns>
        public static byte[] GetFunctionsInfo(byte FuncB)
        {
            byte[] functions = new byte[3];   //0x01:ACS;0x02:ACM;0x04:DCS;0x08:DCM;0x10:PQ

            functions[0] = (byte)(((FuncB & 0x04) == 0x04) ? 1 : 0); //直流源
            functions[1] = (byte)(((FuncB & 0x08) == 0x08) ? 1 : 0); //直流表
            functions[2] = (byte)(((FuncB & 0x10) == 0x10) ? 1 : 0); //电能校验

            return functions;
        }
        //TODO 解析FuncS

        /// <summary>
        /// 获取对应的数据的CRC校验码
        /// </summary>
        /// <param name="value">需要校验的数据，不包含CRC字节，包含报文头0x81</param>
        /// <returns>返回带CRC校验码的字节数据，可用于串口发送</returns>
        public static byte[] CRCcalculator(byte[] value)
        {
            byte crc = 0;   //CRC寄存器

            //从第二个字节开始执行异或，结果返回CRC寄存器
            for (int i = 1; i < value.Length; i++)
            {
                crc ^= value[i];
            }
            value[value.Length - 1] = crc;

            //返回最终带有CRC校验码结尾的信息
            return value;
        }
        #endregion      

    }

    #region Enum Classes

    #region Mode Declaration 系统模式定义

    public enum SystemMode : byte
    {
        /// <summary>
        /// 标准源模式
        /// </summary>
        ModeStandardSource = 0,

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

    #endregion

    #region Page Declaration 显示页面定义
    //TODO 确认彩屏显示界面，以下是黑白屏数据，不同型号可能界面不一样

    public enum DisplayPage : byte
    {
        //TODO 使用【Atribute】特性显示信息
        #region 交流标准源输出
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
        #endregion

        #region 直流
        /// <summary>
        /// 直流测量界面
        /// </summary>
        PageDCMeter = 5,

        /// <summary>
        /// 直流输出界面
        /// </summary>
        PageDC = 6,
        #endregion

        #region 交流标准表
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
        #endregion
    }
    #endregion

    #region Range Declartion 档位定义
    public enum RangeACU : ushort
    {
        ACU_380 = 0,
        ACU_220 = 1,
        ACU_100 = 2,
        ACU_57 = 3,
    }

    public enum RangACI : byte
    {

    }
    #endregion

    #region Error_Code Declaration 故障码解析
    /// <summary>
    /// 故障码定义：枚举。此为获取故障信息的第二种方式
    /// </summary>
    [Flags]
    public enum ErrorCode
    {
        ErrorUa = 0b_0000_0001,    // 0x01 // 1
        ErrorUb = 0b_0000_0010,    // 0x02 // 2
        ErrorUc = 0b_0000_0100,    // 0x04 // 4
        ErrorIa = 0b_0000_1000,    // 0x08 // 8
        ErrorIb = 0b_0001_0000,    // 0x10 // 16
        ErrorIc = 0b_0010_0000,    // 0x20 // 32
        ErrorDC = 0b_0100_0000     // 0x40 // 64
    }
    #endregion


    #endregion

}
