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
        /// <summary>
        /// 协议类型：预留支持协议扩展，暂时不用
        /// </summary>
        public DKCommunicationTypes DKCommunicationType { get; set; }

        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）和命令码(0x4C)、默认的协议类型(81)
        /// </summary>
        public DKCommand( )
        {
            DKCommunicationType = DK81CommunicationInfo.CommunicationType;
            RxID = 0x00;
            TxID = 0x00;
            CommandCode = DK81CommunicationInfo.HandShake;
        }

        /// <summary>
        /// 实例化一个指定ID的对象，默认的协议类型(81)、使用默认的地址（0x0000）=>>>推介的实例化方法
        /// </summary>
        /// <param name="id">传入的指定ID</param>
        public DKCommand(byte commandCode)
        {
            DKCommunicationType = DK81CommunicationInfo.CommunicationType;
            RxID = 0x00;
            TxID = 0x00;
            CommandCode = commandCode;
        }

        /// <summary>
        /// 实例化一个指定命令码、指定协议类型的对象=>>>推介的实例化方法
        /// </summary>
        /// <param name="commandCode">传入的命令码</param>
        /// <param name="dkType">丹迪克协议类型</param>
        public DKCommand(byte commandCode, DKCommunicationTypes dkType)
        {
            RxID = 0x00;
            TxID = 0x00;
            CommandCode = commandCode;
            DKCommunicationType = dkType;
        }

        /// <summary>
        /// 实例化一个指定ID、指定命令码的对象
        /// </summary>
        /// <param name="id">读取的终端ID</param>
        /// <param name="commandCode">传入的命令码</param>
        public DKCommand(ushort id, byte commandCode, DKCommunicationTypes dkType)
        {
            AnalysisID(id);
            CommandCode = commandCode;
            DKCommunicationType = dkType;
        }
        #endregion

        #region Create read and write command 

        #region Private Create read and write command Helper
        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <param name="lenthOverall">完整指令【长度】，包含校验码的空字节数据空间</param>
        /// <returns>commandMissData:【缺少CRC数据】的完整指令长度的字节数组</returns>
        private byte[] CreateCommandHelper(ushort lenthOverall)
        {
            switch (DKCommunicationType)
            {
                case DKCommunicationTypes.DK81CommunicationType:
                    byte[] commandMissData = new byte[lenthOverall];
                    commandMissData[0] = DK81CommunicationInfo.FrameID;
                    commandMissData[1] = RxID;
                    commandMissData[2] = TxID;
                    commandMissData[3] = BitConverter.GetBytes(commandMissData.Length)[0];
                    commandMissData[4] = BitConverter.GetBytes(commandMissData.Length)[1];
                    commandMissData[5] = CommandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake
                    return commandMissData;

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                //case DK55CommunicationInfo.CommunicationType:                

                #endregion

                default: return default;     //返回null
            }
        }

        #endregion

        #region 系统命令
        /// <summary>
        /// 根据丹迪克协议类型创建一个【联机】指令对象
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateHandShake( )
        {
            switch (DKCommunicationType)
            {
                case DK81CommunicationInfo.CommunicationType:
                    byte[] buffer = CreateCommandHelper(7);     //默认即为联机指令，所以无需再处理数据
                    return buffer;

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
                    byte[] buffer = CreateCommandHelper(8);
                    buffer[6] = Convert.ToByte(mode);   //如果mode为空，返回0
                    return buffer;

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
                    byte[] buffer = CreateCommandHelper(8);
                    buffer[6] = Convert.ToByte(page);   //如果mode为空，返回0
                    return buffer;

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
