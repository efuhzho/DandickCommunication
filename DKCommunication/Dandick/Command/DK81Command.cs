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
    public class DK81Command:CommandCodeBase
    {
        /// <summary>
        /// 站号
        /// </summary>
        public byte[] ID { get; set; }

        #region 构造函数
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址和命令码
        /// </summary>
        public DK81Command( )
        {
            ID[0] = 0x00;
            ID[1] = 0x00;
            CommandCode = DK81Info.HandShake;
        }

        /// <summary>
        /// 实例化一个指定ID的对象，使用默认的命令码
        /// </summary>
        /// <param name="id">传入的指定ID</param>
        public DK81Command(byte commandCode)
        {
            ID[0] = 0x00;
            ID[1] = 0x00;
            CommandCode = commandCode;
        }

        /// <summary>
        /// 实例化一个指定ID、指定命令码的对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commandCode"></param>
        public DK81Command(byte[] id,byte commandCode)
        {
            ID[0] = id[0];
            ID[1] = id[1];
            CommandCode = commandCode;
        }
        #endregion

    }
}
