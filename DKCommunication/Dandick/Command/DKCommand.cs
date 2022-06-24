using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Dandick.Command;
using DKCommunication.Dandick.DK81Series;
using DKCommunication.Dandick.DK55Series;
using DKCommunication.Dandick.Communication;

namespace DKCommunication.Dandick.Command
{
    /// <summary>
    /// 丹迪克81协议的命令格式，可以携带站号（RxID、TxID）、命令码（CommandCode）、可扩展支持的协议类型DKType
    /// </summary>
    public class DKCommand : DKCommandCodeBase
    {
        #region Public Properties
        /// <summary>
        /// 协议类型：预留支持协议扩展，暂时不用
        /// </summary>

        #endregion


        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）、默认命令码(0x4C)、默认的协议类型(81)、默认的指令长度（7）
        /// </summary>
        public DKCommand( )
        {
            DKCommunicationType = DK81CommunicationInfo.CommunicationType;
            AnalysisID(0);
            CommandCode = DK81CommunicationInfo.HandShake;
            CommandLength = DK81CommunicationInfo.HandShakeCommandLength;
        }

        /// <summary>
        /// 实例化一个指定命令码和指令长度的对象  =>>>推介的实例化方法
        /// </summary>
        /// <param name="id">传入的指定ID</param>
        /// <param name="commandLength">完整的【指令长度】，包含校验码的空字节数据空间</param>
        public DKCommand(byte commandCode, ushort commandLength)
        {
            DKCommunicationType = DK81CommunicationInfo.CommunicationType;
            AnalysisID(0);
            CommandCode = commandCode;
            CommandLength = commandLength;
        }

        /// <summary>
        /// 实例化一个指定命令码、指定协议类型的对象=>>>推介的实例化方法
        /// </summary>
        /// <param name="commandCode">传入的命令码</param>
        /// <param name="dkType">丹迪克协议类型</param>
        /// <param name="commandLength">完整【指令长度】，包含校验码的空字节数据空间</param>
        public DKCommand(DKCommunicationTypes dkType, byte commandCode, ushort commandLength)
        {
            DKCommunicationType = dkType;
            AnalysisID(0);
            CommandCode = commandCode;
            CommandLength = commandLength;
        }

        /// <summary>
        /// 实例化一个指定ID、指定命令码的对象
        /// </summary>
        /// <param name="id">读取的终端ID</param>
        /// <param name="commandCode">传入的命令码</param>
        /// <param name="commandLength">完整的【指令长度】，包含校验码的空字节数据空间</param>
        public DKCommand(DKCommunicationTypes dkType, ushort id, byte commandCode, ushort commandLength)
        {
            DKCommunicationType = dkType;
            AnalysisID(id);
            CommandCode = commandCode;
            CommandLength = commandLength;
        }
        #endregion

        #region Create read and write command 

        #region Private Create read and write command Helper
        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <returns>commandMissData:【缺少CRC数据】的完整指令长度的字节数组</returns>
        private byte[] CreateCommandHelper( )
        {
            switch (DKCommunicationType)
            {
                case DKCommunicationTypes.DK81CommunicationType:
                    byte[] commandMissData = new byte[CommandLength];
                    commandMissData[0] = DK81CommunicationInfo.FrameID;
                    commandMissData[1] = RxID;
                    commandMissData[2] = TxID;
                    commandMissData[3] = BitConverter.GetBytes(CommandLength)[0];
                    commandMissData[4] = BitConverter.GetBytes(CommandLength)[1];
                    commandMissData[5] = CommandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake
                    return commandMissData;

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                //case DK55CommunicationInfo.CommunicationType:                

                #endregion

                default: return default;     //返回null
            }
        }

        private byte[] CreateCommandHelper<T>(T data) 
        {
            byte[] buffer = CreateCommandHelper();
            for (int i = 6; i < CommandLength - 1; i++)
            {
                buffer[i] = Convert.ToByte(data);   //如果data为空，返回0
            }
            return buffer;
        }

        #endregion

        #region 系统命令
        /// <summary>
        /// 根据丹迪克协议类型创建一个【联机指令】对象
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateHandShake( )
        {
            switch (DKCommunicationType)
            {
                case DK81CommunicationInfo.CommunicationType:
                    return CreateCommandHelper();

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                //case DK55CommunicationInfo.CommunicationType:                

                #endregion

                default: return default;    //返回null
            }
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateSystemMode(DK81CommunicationInfo.SystemMode mode)
        {
            switch (DKCommunicationType)
            {
                case DK81CommunicationInfo.CommunicationType:
                    return CreateCommandHelper(mode);

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                //case DK55CommunicationInfo.CommunicationType:

                #endregion

                default: return default;    //返回null
            }
        }

        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateDisplayPage(DK81CommunicationInfo.DisplayPage page)
        {
            switch (DKCommunicationType)
            {
                case DK81CommunicationInfo.CommunicationType:                    
                    return CreateCommandHelper(page);

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                //case DK55CommunicationInfo.CommunicationType:

                #endregion

                default: return default;    //返回null
            }
        }
        #endregion

        #region 交流表源命令

        #endregion

        #endregion

    }
}
