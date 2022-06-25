﻿using System;
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
    public class DKCommandBuilder : DKCommandCodeBase
    {
        #region MyRegion

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
        public DKCommandBuilder()
        {
            DKCommunicationType = DK81CommunicationInfo.CommunicationType;
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
        public DKCommandBuilder(byte commandCode, ushort commandLength)
        {
            DKCommunicationType = DK81CommunicationInfo.CommunicationType;
            RxID = AnalysisID(0)[1];
            TxID = AnalysisID(0)[0];
            CommandCode = commandCode;
            CommandLength = commandLength;
        }

        /// <summary>
        /// 实例化一个指定命令码、指定协议类型的对象=>>>推介的实例化方法
        /// </summary>
        /// <param name="commandCode">传入的命令码</param>
        /// <param name="dkType">丹迪克协议类型</param>
        /// <param name="commandLength">完整【指令长度】，包含校验码的空字节数据空间</param>
        public DKCommandBuilder(DKCommunicationTypes dkType, byte commandCode, ushort commandLength)
        {
            DKCommunicationType = dkType;
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
        public DKCommandBuilder(DKCommunicationTypes dkType, ushort id, byte commandCode, ushort commandLength)
        {
            DKCommunicationType = dkType;
            RxID = AnalysisID(id)[1];
            TxID = AnalysisID(id)[0];
            CommandCode = commandCode;
            CommandLength = commandLength;
        }
        #endregion

        #region Create read and write command 读写指令创建方法

        #region Private Create read and write command Helper 私有读写指令创建方法
        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <returns>commandMissData:【缺少CRC数据】的完整指令长度的字节数组</returns>
        private byte[] CreateCommandHelper()
        {
            byte[] commandMissData = new byte[CommandLength];

            switch (DKCommunicationType)
            {
                case DKCommunicationTypes.DK81CommunicationType:
                    commandMissData[0] = DK81CommunicationInfo.FrameID;
                    commandMissData[1] = RxID;
                    commandMissData[2] = TxID;
                    commandMissData[3] = BitConverter.GetBytes(CommandLength)[0];
                    commandMissData[4] = BitConverter.GetBytes(CommandLength)[1];
                    commandMissData[5] = CommandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake
                    break;
                #region 待扩展：DK55CommunicationInfo
                //TODO 待扩展55协议：DK55CommunicationInfo
                //case DK55CommunicationInfo.CommunicationType:                

                #endregion                 
            }
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
            #region 待扩展：DK55CommunicationInfo
            //TODO 待扩展55协议：DK55CommunicationInfo
            //case DK55CommunicationInfo.CommunicationType:                

            #endregion

            byte[] buffer = CreateCommandHelper();
            buffer[6] = Convert.ToByte(data);
            return buffer;
        }
        public byte[] Create(int a)
        {
            return default;
        }
        #endregion

        #region 系统命令
        /// <summary>
        /// 根据丹迪克协议类型创建一个【联机指令】对象
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateHandShake()
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
        public byte[] CreateSystemMode(SystemMode mode)
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
        public byte[] CreateDisplayPage(DisplayPage page)
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
        /// <summary>
        /// 创建一个【源关闭】命令
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateStop()
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
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateStart()
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
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>缺少CRC数据的完整指令长度的字节数组</returns>
        public byte[] CreateRange(DisplayPage page)
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

        #endregion

    }
}