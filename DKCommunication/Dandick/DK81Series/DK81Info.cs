using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的相关信息
    /// </summary>
    public class DK81Info
    {
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

        /// <summary>
        /// 联机命令，读取终端型号和版本号：Link    ('L')
        /// </summary>
        public const byte HandShake = 0x4C;

        /// <summary>
        /// 设置系统模式
        /// </summary>
        public const byte SetSystemMode = 0x44;

        /// <summary>
        /// 发送故障代码，带枚举数据
        /// </summary>
        public const byte ErrorCode = 0x52;

        /// <summary>
        /// 设置当前终端显示界面
        /// </summary>
        public const byte SetDisplayPage = 0x4A;
        #endregion

        #region 交流源/表
        /// <summary>
        /// 交流源关闭命令
        /// </summary>
        public const byte Stop = 0x4F;

        /// <summary>
        /// 交流源打开命令
        /// </summary>
        public const byte Start = 0x54;

        /// <summary>
        /// 设置源档位参数
        /// </summary>
        public const byte SetRange = 0x31;

        /// <summary>
        /// 设置源幅度参数
        /// </summary>
        public const byte WriteAmplitude = 0x32;

        /// <summary>
        /// 设置相位参数
        /// </summary>
        public const byte WriteSetPhase = 0x33;

        /// <summary>
        /// 设置源频率参数
        /// </summary>
        public const byte WriteFrequency = 0x34;

        /// <summary>
        /// 设置源接线模式
        /// </summary>
        public const byte SetWireMode = 0x35;

        /// <summary>
        /// 闭环控制使能命令
        /// </summary>
        public const byte SetClosedLoop = 0x36;

        /// <summary>
        /// 设置电能校验参数
        /// </summary>
        public const byte WriteElectricity = 0x37;

        /// <summary>
        /// 设置谐波参数
        /// </summary>
        public const byte WriteHarmonics = 0x58;

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
        #region Error_Code Declaration 错误码信息定义
        //public const byte ErrorCodeUa = 0x01;   //0b00000001; 
        //public const byte ErrorCodeUb = 0x02;   //0b00000010; 
        //public const byte ErrorCodeUc = 0x04;   //0b00000100; 
        //public const byte ErrorCodeIa = 0x08;   //0b00001000;
        //public const byte ErrorCodeIb = 0x10;   //0b00010000;
        //public const byte ErrorCodeIc = 0x20;   //0b00100000;
        //public const byte ErrorCodeDC = 0x40;   //0b01000000;
        #endregion
        public static string GetErrorMessageByErrorCode(byte code)
        {
            string errorInfo = string.Empty;
            if ((code & 0x01) == 0x01)
            {
                errorInfo += StringResources.Language.ErrorCodeUa;
            }
            if ((code & 0x02) == 0x02)
            {
                errorInfo += StringResources.Language.ErrorCodeUb;
            }
            if ((code & 0x04) == 0x04)
            {
                errorInfo += StringResources.Language.ErrorCodeUc;
            }
            if ((code & 0x08) == 0x08)
            {
                errorInfo += StringResources.Language.ErrorCodeIa;
            }
            if ((code & 0x10) == 0x10)
            {
                errorInfo += StringResources.Language.ErrorCodeIb;
            }
            if ((code & 0x20) == 0x20)
            {
                errorInfo += StringResources.Language.ErrorCodeIc;
            }
            if ((code & 0x40) == 0x40)
            {
                errorInfo += StringResources.Language.ErrorCodeDC;
            }
            return errorInfo;
        }
        #endregion

        #region Mode Declaration 系统模式定义
        /// <summary>
        /// 标准源模式
        /// </summary>
        public const byte ModeStandardSource = 0x00;

        /// <summary>
        /// 标准表模式
        /// </summary>
        public const byte ModeStandardMeter = 0x01;

        /// <summary>
        /// 标准表（钳表）模式
        /// </summary>
        public const byte ModeStandardMeterClamp = 0x02;

        /// <summary>
        /// 标准源校准模式
        /// </summary>
        public const byte ModeStandardSourceCalibrate = 0x10;     //?需确认是0x10还是0x0A

        /// <summary>
        /// 标准表校准模式
        /// </summary>
        public const byte ModeStandardMeterCalibrate = 0x11;

        /// <summary>
        /// 钳表校准模式
        /// </summary>
        public const byte ModeStandardClampCalibrate = 0x12;

        /// <summary>
        /// 直流源校准模式
        /// </summary>
        public const byte ModeDCSourceCalibrate = 0x13;

        /// <summary>
        /// 直流表校准模式
        /// </summary>
        public const byte ModeDCMeterCalibrate = 0x14;
        #endregion
    }
}
