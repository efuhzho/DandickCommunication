﻿using System;
using DKCommunication.Serial;
using DKCommunication.Core;
using DKCommunication.Dandick.DK81Series;
using DKCommunication.Dandick.DKInterface;

namespace DKCommunication.Dandick.Base
{
    /// <summary>
    /// 所有丹迪克设备通信协议的地址基础类
    /// </summary>
    public class DK_DeviceBase : SerialBase
    {
        #region Public Properties
        /// <summary>
        /// 设备地址ID
        /// </summary>
        public ushort ID { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 设备版本号
        /// </summary>
        public string Version{ get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string SN { get; set; }    
        #endregion

        #region 解析ID
        /// <summary>
        /// 解析ID,转换为两个字节
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>返回带有信息的结果</returns>
        public virtual OperateResult<byte[]> AnalysisID(ushort id)
        {
            try
            {
                byte[] twoBytesID = BitConverter.GetBytes(id);  //低位在前
                return OperateResult.CreateSuccessResult(twoBytesID);
            }
            catch (Exception)
            {
                return new OperateResult<byte[]>(1001, "请输入正确的ID!");
            }
        }

        /// <summary>
        /// 解析ID，转换为1个字节
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>返回带有信息的结果</returns>
        public virtual OperateResult<byte> AnalysisIDtoByte(ushort id)
        {
            try
            {
                byte oneByteID = BitConverter.GetBytes(id)[0]; ;  //低位在前
                return OperateResult.CreateSuccessResult(oneByteID);
            }
            catch (Exception)
            {
                return new OperateResult<byte>(1001, "请输入正确的ID!");
            }
        }
        #endregion

        #region Object Override
        /// <summary>
        /// 返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串数据</returns>
        public override string ToString( )
        {
            return "所有丹迪克设备的基类";
        }
        #endregion

    }
}
