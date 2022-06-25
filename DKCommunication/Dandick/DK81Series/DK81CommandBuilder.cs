using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Dandick.Command;
using DKCommunication.Dandick.Communication;

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

        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）、默认命令码(0x4C)、默认的协议类型(81)、默认的指令长度（7）
        /// </summary>
        public DK81CommandBuilder()
        {
            RxID = AnalysisID(0)[1];
            TxID = AnalysisID(0)[0];
            CommandCode = DK81CommunicationInfo.HandShake;
            CommandLength = DK81CommunicationInfo.HandShakeCommandLength;
        }

        /// <summary>
        /// 实例化一个指定命令码和指令长度的对象  =>>>推介的实例化方法
        /// </summary>
        /// <param name="id">传入的指定ID</param>
        /// <param name="commandLength">完整的【指令长度】，包含校验码的空字节数据空间</param>
        public DK81CommandBuilder(byte commandCode, ushort commandLength)
        {
            RxID = AnalysisID(0)[1];
            TxID = AnalysisID(0)[0];
            CommandCode = commandCode;
            CommandLength = commandLength;
        }

        /// <summary>
        /// 实例化一个指定ID、指定命令码的对象
        /// </summary>
        /// <param name="id">读取的终端ID</param>
        /// <param name="commandCode">传入的命令码</param>
        /// <param name="commandLength">完整的【指令长度】，包含校验码的空字节数据空间</param>
        public DK81CommandBuilder(ushort id, byte commandCode, ushort commandLength)
        {
            RxID = AnalysisID(id)[1];
            TxID = AnalysisID(id)[0];
            CommandCode = commandCode;
            CommandLength = commandLength;
        }
        #endregion

        #region Private create command helper 私有指令创建辅助方法
        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <returns>commandMissData:【缺少CRC数据】的完整指令长度的字节数组</returns>
        private byte[] CreateCommandHelper()
        {
            byte[] commandMissData = new byte[CommandLength];
            commandMissData[0] = DK81CommunicationInfo.FrameID;
            commandMissData[1] = RxID;
            commandMissData[2] = TxID;
            commandMissData[3] = BitConverter.GetBytes(CommandLength)[0];
            commandMissData[4] = BitConverter.GetBytes(CommandLength)[1];
            commandMissData[5] = CommandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake 
            return commandMissData;
        }

        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <typeparam name="T">泛型类，必须可以被转换为byte</typeparam>
        /// <param name="data">数据</param>
        /// <returns>【缺少CRC数据】的完整指令长度的字节数组</returns>
        private byte[] CreateCommandHelper<T>(T data) where T : Enum //TODO 添加T类型约束
        {
            byte[] buffer = CreateCommandHelper();
            buffer[6] = Convert.ToByte(data);
            return buffer;
        }
        #endregion

        #region 指令生产器
        #region 系统命令
        /// <summary>
        /// 根据丹迪克协议类型创建一个【联机指令】对象
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateHandShake()
        {
            return CreateCommandHelper();
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateSystemMode(SystemMode mode)
        {
            return CreateCommandHelper(mode);
        }


        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateDisplayPage(DisplayPage page)
        {
            return CreateCommandHelper(page);
        }

        #endregion

        #region 交流表源命令
        /// <summary>
        /// 创建一个【源关闭】命令
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateStop()
        {
            return CreateCommandHelper();
        }

        /// <summary>
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateStart()
        {
            return CreateCommandHelper();
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateRange(DisplayPage page)
        {
            return CreateCommandHelper(page);
        }
        #endregion
        #endregion

    }
}
