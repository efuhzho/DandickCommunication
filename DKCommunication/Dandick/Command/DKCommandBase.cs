using System;
using DKCommunication.Dandick.Communication;

namespace DKCommunication.Dandick.Command
{
    /// <summary>
    /// 所有丹迪克设备通信协议的地址基础类
    /// </summary>
    public class DKCommandBase
    {         
        /// <summary>
        /// 设备ID
        /// </summary>
        public ushort ID { get; set; }        
        
        /// <summary>
        /// 解析ID,转换为两个字节
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>返回：两个字节长度的数组，低位在前</returns>
        public virtual byte[] AnalysisID(ushort id)
        {
            return BitConverter.GetBytes(id);   //低位在前
        }

        /// <summary>
        /// 解析ID，转换为1个字节
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>字节类型ID</returns>
        public virtual byte AnalysisIDtoByte(ushort id)
        {
            return BitConverter.GetBytes(id)[0];   //低位在前
        }


        /// <summary>
        /// 返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串数据</returns>
        public override string ToString()
        {
            return "所有丹迪克设备的地址类";
        }
    }
}
