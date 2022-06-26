using DKCommunication.Dandick.Command;
using System;

namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的命令格式，可以携带站号（ID）、命令码（CommandCode）、数据（DATA）
    /// </summary>
    public class DK81CommandBuilder : DKCommandBase
    {
        #region 私有字段
        /// <summary>
        /// 接收终端的设备ID
        /// </summary>
        private readonly byte RxID;

        /// <summary>
        /// 发送终端的设备ID
        /// </summary>
        private readonly byte TxID;
        #endregion

        #region 公开属性
        
        #endregion

        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）
        /// </summary>
        public DK81CommandBuilder()
        {
            RxID = AnalysisID(0)[1];
            TxID = AnalysisID(0)[0];
        }      

        /// <summary>
        /// 实例化一个指定ID的对象
        /// </summary>
        /// <param name="id">读取的终端ID</param>
        public DK81CommandBuilder(ushort id)
        {
            RxID = AnalysisID(id)[1];
            TxID = AnalysisID(id)[0];           
        }
        #endregion

        #region Private create command helper 私有指令创建辅助方法
        /// <summary>
        /// 创建指令时的【统一预处理】
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">指令长度</param>
        /// <returns>发送给串口的指令</returns>
        private byte[] CreateCommandHelper(byte commandCode,ushort commandLength)
        {
            byte[] buffer = new byte[commandLength];
            buffer[0] = DK81CommunicationInfo.FrameID;
            buffer[1] = RxID;
            buffer[2] = TxID;
            buffer[3] = BitConverter.GetBytes(commandLength)[0];
            buffer[4] = BitConverter.GetBytes(commandLength)[1];
            buffer[5] = commandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake 
            buffer[6] = DK81CommunicationInfo.CRCcalculator(buffer);
            return buffer;
        }

        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <typeparam name="T">泛型类，必须可以被转换为byte</typeparam>
        /// <param name="data">数据</param>
        /// <returns>【缺少CRC数据】的完整指令长度的字节数组</returns>
        private byte[] CreateCommandHelper<T>(byte commandCode, ushort commandLength,T data) where T : Enum //TODO 添加T类型约束
        {
            byte[] buffer = CreateCommandHelper(commandCode, commandLength);
            buffer[6] = Convert.ToByte(data);
            buffer[7] = DK81CommunicationInfo.CRCcalculator(buffer);
            return buffer;
        }
        #endregion

        #region 指令生成器
        #region 系统命令
        /// <summary>
        /// 根据丹迪克协议类型创建一个【联机指令】对象
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateHandShake()
        {
           return CreateCommandHelper(DK81CommunicationInfo.HandShake,DK81CommunicationInfo.HandShakeCommandLength);            
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateSystemMode(SystemMode mode)
        {
            return CreateCommandHelper(DK81CommunicationInfo.SetSystemMode,DK81CommunicationInfo.SetSystemModeCommandLength,mode);
        }

        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateDisplayPage(DisplayPage page)
        {
            return CreateCommandHelper(DK81CommunicationInfo.SetDisplayPage,DK81CommunicationInfo.SetDisplayPageCommandLength,page);
        }

        #endregion

        #region 交流表源命令
        /// <summary>
        /// 创建一个【源关闭】命令
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateStop()
        {
            return CreateCommandHelper(DK81CommunicationInfo.Stop,DK81CommunicationInfo.StopLength);
        }

        /// <summary>
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateStart()
        {
            return CreateCommandHelper(DK81CommunicationInfo.Start,DK81CommunicationInfo.StartLength);
        }                
        #endregion
        #endregion
    }
}
