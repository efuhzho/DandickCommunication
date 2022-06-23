using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.Command
{
    /// <summary>
    /// 所有丹迪克设备通信协议的命令码基础类
    /// </summary>
    public class DKCommandCodeBase
    {
        /// <summary>
        /// 命令码
        /// </summary>
        public byte CommandCode { get; set; }

        /// <summary>
        /// 接收终端的设备ID
        /// </summary>
        public byte RxID { get; set; }

        /// <summary>
        /// 发送终端的设备ID
        /// </summary>
        public byte TxID { get; set; }

        /// <summary>
        /// 解析读取的ushort形式的id码
        /// </summary>
        /// <param name="id">读取的地址</param>
        public virtual void AnalysisID(ushort id)
        {
            RxID = BitConverter.GetBytes(id)[1];
            TxID = BitConverter.GetBytes(id)[0];
        }


        /// <summary>
        /// 返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串数据</returns>
        public override string ToString( )
        {
            return CommandCode.ToString();
        }
    }
}
