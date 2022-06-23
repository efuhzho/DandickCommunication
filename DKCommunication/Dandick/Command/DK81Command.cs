using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Dandick.Command;

namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的命令格式，可以携带站号（ID）、命令码（CommandCode）
    /// </summary>
    public class DK81Command : CommandCodeBase
    {
        /// <summary>
        /// 协议类型：预留支持协议扩展，暂时不用
        /// </summary>
        public int DKType { get; set; }   //TODO 预留协议扩展支持

        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址和命令码
        /// </summary>
        public DK81Command()
        {
            DKType = DK81Info.CommunicationType;
            RxID = 0x00;
            TxID = 0x00;
            CommandCode = DK81Info.HandShake;
        }

        /// <summary>
        /// 实例化一个指定ID的对象，使用默认的命令码
        /// </summary>
        /// <param name="id">传入的指定ID</param>
        public DK81Command(byte commandCode)
        {
            DKType = DK81Info.CommunicationType;
            RxID = 0x00;
            TxID = 0x00;
            CommandCode = commandCode;
        }

        public DK81Command(byte commandCode, int dkType)
        {
            RxID = 0x00;
            TxID = 0x00;
            CommandCode = commandCode;
            DKType = dkType;

        }

        /// <summary>
        /// 实例化一个指定ID、指定命令码的对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandCode"></param>
        public DK81Command(ushort id, byte commandCode, int dkType)
        {
            AnalysisID(id);
            CommandCode = commandCode;
            DKType = dkType;
        }
        #endregion

        #region Create read and write command 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rxid"></param>
        /// <param name="txid"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] CreateHandShake(byte rxid, byte txid, ushort length)
        {
            byte[] buffer = new byte[6];
            buffer[0] = DK81Info.FrameID;
            buffer[1] = rxid;
            buffer[2] = txid;
            buffer[3] = BitConverter.GetBytes(length)[1];
            buffer[4] = BitConverter.GetBytes(length)[0];
            buffer[5] = CommandCode;
            return buffer;
        }
        #endregion

    }
}
