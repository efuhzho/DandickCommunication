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
        public DKCommunicationTypes DKCommunicationType { get; set; }   //TODO 预留协议扩展支持

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
        /// 实例化一个指定ID的对象，使用默认的地址（0x0000）、默认的协议类型(81)
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
        /// 实例化一个指定命令码、指定协议类型的对象
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

        /// <summary>
        /// 根据丹迪克协议类型创建一个联机命令对象，不含校验码
        /// </summary>
        /// <returns>返回原始的联机指令，不含校验码</returns>
        public byte[] CreateHandShake( )
        {
            switch (DKCommunicationType)
            {
                case DK81CommunicationInfo.CommunicationType:
                    byte[] buffer = new byte[6];
                    buffer[0] = DK81CommunicationInfo.FrameID;
                    buffer[1] = RxID;
                    buffer[2] = TxID;
                    buffer[3] = BitConverter.GetBytes(buffer.Length + 1)[0];
                    buffer[4] = BitConverter.GetBytes(buffer.Length + 1)[1];
                    buffer[5] = DK81CommunicationInfo.HandShake;
                    return buffer;

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                case DK55CommunicationInfo.CommunicationType:
                    byte[] buffer2 = new byte[8];
                    buffer2[0] = DK55CommunicationInfo.FrameID; //待修改
                    buffer2[1] = DK55CommunicationInfo.FrameID; //待修改
                    buffer2[2] = BitConverter.GetBytes(buffer2.Length)[0];
                    buffer2[3] = BitConverter.GetBytes(buffer2.Length)[1];
                    buffer2[4] = RxID;  //待修改
                    buffer2[5] = 0x00;  //待定义
                    buffer2[6] = 0x00;  //待定义
                    buffer2[7] = 0x00;  //待定义
                                        //buffer2[8] = DK55CRCCheck[0]
                                        //buffer2[9] = DK55CRCCheck[1]
                                        //*buffer2[10] = 0x96;*/  //CRCSurport里添加
                    return buffer2;
                #endregion

                default: return default;    //返回null
            }
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个系统模式命令对象，不含校验码
        /// </summary>
        /// <returns>返回原始的系统模式设置指令，不含校验码</returns>
        public byte[] CreateSystemMode(DK81CommunicationInfo.SystemMode mode)
        {
            switch (DKCommunicationType)
            {
                case DK81CommunicationInfo.CommunicationType:
                    byte[] buffer = new byte[7];
                    buffer[0] = DK81CommunicationInfo.FrameID;
                    buffer[1] = RxID;
                    buffer[2] = TxID;
                    buffer[3] = BitConverter.GetBytes(buffer.Length + 1)[0];
                    buffer[4] = BitConverter.GetBytes(buffer.Length + 1)[1];
                    buffer[5] = DK81CommunicationInfo.SetSystemMode;
                    buffer[6] = Convert.ToByte(mode);   //如果mode为空，返回0
                    return buffer;

                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                /*case DK55CommunicationInfo.CommunicationType:
                    byte[] buffer2 = new byte[8];                   
                    return buffer2;*/
                #endregion

                default: return default;    //返回null
            }
        }
        #endregion

    }
}
