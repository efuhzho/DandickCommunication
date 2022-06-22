using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.Command
{
    /// <summary>
    /// 所有丹迪克设备通信协议的命令基础类
    /// </summary>
    public class CommandCodeBase
    {
        /// <summary>
        /// 命令码
        /// </summary>
        public byte CommandCode { get; set; }


        /// <summary>
        /// 解析字符串形式的命令码
        /// </summary>
        /// <param name="address">地址信息</param>
        public virtual void AnalysisCommandCode(string commandCode)
        {
            CommandCode = byte.Parse(commandCode);
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
