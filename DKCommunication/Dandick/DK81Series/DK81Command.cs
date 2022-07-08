﻿using DKCommunication.Dandick.Base;
using System;
using DKCommunication.BasicFramework;
using DKCommunication.Core;

namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的命令格式，可以携带站号（ID）、命令码（CommandCode）、数据（DATA）
    /// </summary>
    public class DK81Command : DK_DeviceBase<RegularByteTransform>
    {


        #region 私有字段
        #region ID
        /// <summary>
        /// 接收终端的设备ID
        /// </summary>
        private readonly byte _RxID;

        /// <summary>
        /// 发送终端的设备ID
        /// </summary>
        private readonly byte _TxID;



        #endregion

        /*******************/

        #region Ranges
        //private byte RangeUa;
        //private byte RangeUb;
        //private byte RangeUc;
        //private byte RangeIa;
        //private byte RangeIb;
        //private byte RangeIc;
        //private byte RangeIPa;
        //private byte RangeIPb;
        //private byte RangeIPc;
        #endregion Ranges

        #endregion 私有字段

        /*****************************************************************************************************/

        #region Public Properties
        public WireMode WireMode { get; set; }

        public DisplayPage DisplayPage { get; set; }

        public SystemMode SystemMode { get; set; }

        #region 档位
        //public byte Range_ACU { get; set; }
        //public byte Range_ACI { get; set; }
        //public byte Range_DCU { get; set; }
        //public byte Range_DCI { get; set; }
        //public byte Range_DCM { get; set; }
        //public byte Range_IProtect { get; set; }
        #endregion

        /*******************/

        #region 相别
        public float UA { get; }
        public float UB { get; }
        public float UC { get; }
        public float IA { get; }
        public float IB { get; }
        public float IC { get; }
        public float IProtectA { get; }
        public float IProtectB { get; }
        public float IProtectC { get; }
        #endregion

        #endregion Public Properties

        /*****************************************************************************************************/

        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）
        /// </summary>
        public DK81Command()
        {
            var result = AnalysisID(0);
            if (result.IsSuccess)
            {
                _RxID = result.Content[1];
                _TxID = result.Content[0];
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>
        /// 实例化一个指定ID的对象
        /// </summary>
        /// <param name="id">读取的终端ID</param>
        public DK81Command(ushort id)
        {
            var result = AnalysisID(id);
            if (result.IsSuccess)
            {
                _RxID = result.Content[1];
                _TxID = result.Content[0];
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        #endregion

        /*****************************************************************************************************/

        #region private CommandBuilder【报文创建】
        #region 系统命令【报文创建】
        /// <summary>
        /// 创建一个【联机指令】原始报文
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateHandShake()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.HandShake, DK81CommunicationInfo.HandShakeCommandLength);

            return bytesHeader;
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateSetSystemMode(SystemMode mode)
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.SetSystemMode, DK81CommunicationInfo.SetSystemModeCommandLength, mode);
            if (bytesHeader.IsSuccess)
            {
                return bytesHeader;
            }
            else
            {
                return OperateResult.CreateFailedResult<byte[]>(bytesHeader);
            }
        }

        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateSetDisplayPage(DisplayPage page)    //TODO 将DisplayPage写成属性
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.SetDisplayPage, DK81CommunicationInfo.SetDisplayPageCommandLength, page);
            if (bytesHeader.IsSuccess)
            {
                return bytesHeader;
            }
            else
            {
                return new OperateResult<byte[]>(811203, "创建指令失败");
            }
        }
        #endregion 系统命令【报文创建】

        /*******************/

        #region 交流表源命令【报文创建】
        /// <summary>
        /// 创建一个【交流源关闭】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateStopACSource()
        {
            OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.StopACSource, DK81CommunicationInfo.StopACSourceLength);

            return bytes;
        }

        /// <summary>
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateStartACSource()
        {
            OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.StartACSource, DK81CommunicationInfo.StartACSourceLength);

            return bytes;
        }

        /// <summary>
        /// 创建【交流源档位设置】报文，返回带信息的报文
        /// </summary>
        /// <param name="urange">交流电压档位</param>
        /// <param name="irange">交流电流档位</param>
        /// <param name="ipranges">保护电流档位</param>
        /// <returns></returns>
        private OperateResult<byte[]> CreatSetACSourceRange(int urange, int irange, int ipranges)
        {
            try
            {
                byte[] ranges = new byte[9];
                for (int i = 0; i < 3; i++)
                {
                    ranges[i] = (byte)urange;
                    ranges[i + 3] = (byte)irange;
                    ranges[i + 6] = (byte)ipranges;
                }

                OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.SetACSourceRanges, DK81CommunicationInfo.SetRangesLength, ranges);
                return bytes;
            }
            catch (Exception ex)
            {

                return new OperateResult<byte[]>(8112, "请输入正确的档位索引值，0为最大档位" + ex.Message);
            }

        }

        /// <summary>
        /// 创建【设置交流源幅度】的完整报文
        /// </summary>
        /// <param name="data">将要设置的浮点型幅值</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteACSourceAmplitude(float[] data)
        {
            OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.WriteACSourceAmplitude, DK81CommunicationInfo.WriteACSourceAmplitudeLength, data);
            return bytes;
        }

        /// <summary>
        /// 创建【设置源相位】的完整报文，返回OK
        /// </summary>
        /// <param name="data">浮点数组：PhaseUa，PhaseUb，PhaseUc，PhaseIa，PhaseIb，PhaseIc</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWritePhase(float[] data)
        {
            OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.WritePhase, DK81CommunicationInfo.WritePhaseLength, data);
            return bytes;
        }

        /// <summary>
        /// 创建【设置源频率】的完整报文，返回OK
        /// </summary>
        /// <param name="data">浮点型数组</param>
        /// <param name="Flag">标志</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateWriteFrequency(float[] data, byte Flag)
        {
            OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.WriteFrequency, DK81CommunicationInfo.WriteFrequencyLength, data, Flag);
            return bytes;
        }

        /// <summary>
        /// 创建【设置接线模式】完整报文
        /// </summary>
        /// <param name="wireMode">接线模式</param>
        /// <returns>带成功标志的操作结果</returns>
        private OperateResult<byte[]> CreateSetWireMode(WireMode wireMode)
        {
            OperateResult<byte[]> bytes = CreateCommandHelper(DK81CommunicationInfo.SetWireMode, DK81CommunicationInfo.SetWireModeLength, wireMode);
            return bytes;
        }
        #endregion 交流表源命令【报文创建】

        /*******************/

        #region 设备信息【报文创建】
        /// <summary>
        /// 创建读取交流标准源和标准表档位信息报文
        /// </summary>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadACSourceRanges()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.ReadACSourceRanges, DK81CommunicationInfo.ReadACSourceRangesLength);

            return bytesHeader;
        }

        /// <summary>
        /// 创建读取直流源档位信息的报文
        /// </summary>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadDCSourceRanges()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.ReadDCSourceRanges, DK81CommunicationInfo.ReadDCSourceRangesLength);

            return bytesHeader;
        }

        /// <summary>
        /// 创建读取直流表档位信息的报文
        /// </summary>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadDCMeterourceRanges()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.ReadDCMeterRanges, DK81CommunicationInfo.ReadDCMeterRangesLength);

            return bytesHeader;
        }
        #endregion 设备信息【报文创建】

        /*******************/

        #region Private CommandBuilder Helper 【报文创建】
        /// <summary>
        /// 创建完整指令长度的【指令头】，长度大于7的报文不带CRC校验码，不可直接发送给串口，长度等于7则带校验码
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">指令长度</param>
        /// <returns>带指令信息的结果：完整指令长度</returns>
        private OperateResult<byte[]> CreateCommandHelper(byte commandCode, ushort commandLength)
        {
            //尝试预创建报文
            try
            {
                byte[] buffer = new byte[commandLength];
                buffer[0] = DK81CommunicationInfo.FrameID;
                buffer[1] = _RxID;
                buffer[2] = _TxID;
                buffer[3] = BitConverter.GetBytes(commandLength)[0];
                buffer[4] = BitConverter.GetBytes(commandLength)[1];
                buffer[5] = commandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake 
                if (commandLength == 7)
                {
                    buffer[6] = DK81CommunicationInfo.CRCcalculator(buffer);    //如果是不带数据的命令则加上校验码
                }
                return OperateResult.CreateSuccessResult(buffer);
            }

            //发生异常
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811200, ex.Message + "From:CreateCommandHelper");
            }
        }

        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <typeparam name="T">泛型类，必须可以被转换为byte</typeparam>
        /// <param name="data">数据</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateCommandHelper<T>(byte commandCode, ushort commandLength, T byteData) where T : System.Enum //TODO 添加T类型约束
        {
            try
            {
                OperateResult<byte[]> result = CreateCommandHelper(commandCode, commandLength);

                if (result.IsSuccess)
                {
                    result.Content[6] = Convert.ToByte(byteData);
                    result.Content[7] = DK81CommunicationInfo.CRCcalculator(result.Content);
                    return result;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811202, ex.Message + "From:CreateCommandHelper");
            }
        }

        /// <summary>
        /// 创建完整的指令，带CRC校验码，可直接发送给串口
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">完整指令长度</param>
        /// <param name="dataBytes">传入的数据</param>
        /// <returns>完整的指令，带CRC校验码，可直接发送给串口</returns>
        private OperateResult<byte[]> CreateCommandHelper(byte commandCode, ushort commandLength, byte[] dataBytes)
        {
            try
            {
                OperateResult<byte[]> dataBytesLess = CreateCommandHelper(commandCode, commandLength);
                if (dataBytesLess.IsSuccess)
                {
                    Array.Copy(dataBytes, 0, dataBytesLess.Content, 6, dataBytes.Length);
                    dataBytesLess.Content[commandLength - 1] = DK81CommunicationInfo.CRCcalculator(dataBytesLess.Content);
                    return dataBytesLess;
                }
                else
                {
                    return dataBytesLess;
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811204, ex.Message + ":CreateCommandHelper");
            }
        }

        /// <summary>
        /// 创建完整的指令，带CRC校验码，可直接发送给串口
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">报文长度</param>
        /// <param name="data">传入的浮点型数据</param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCommandHelper(byte commandCode, ushort commandLength, float[] data)
        {
            try
            {
                //将浮点型数据转换成字节数组
                byte[] dataBytes = ByteTransform.TransByte(data);

                //创建完整的指令报文
                OperateResult<byte[]> commandBytes = CreateCommandHelper(commandCode, commandLength, dataBytes);
                return commandBytes;
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811205, ex.Message + "8112:CreateCommandHelper");
            }
        }

        private OperateResult<byte[]> CreateCommandHelper(byte commandCode, ushort commandLength, float[] data, byte Flag)
        {
            try
            {
                //将浮点型数据转换成字节数组
                byte[] datas = ByteTransform.TransByte(data);

                //将Flag数据和data合并在一起
                byte[] dataBytes = new byte[datas.Length + 1];
                Array.Copy(datas, dataBytes, datas.Length);
                dataBytes[dataBytes.Length - 1] = Flag;

                //创建完整的指令报文
                OperateResult<byte[]> commandBytes = CreateCommandHelper(commandCode, commandLength, dataBytes);
                return commandBytes;
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811205, ex.Message + "8112:CreateCommandHelper");
            }
        }

        #endregion

        #endregion private CommandBuilder【报文创建】

        /*****************************************************************************************************/

        #region Core Interative 核心交互
        protected virtual OperateResult<byte[]> CheckResponse(byte[] send)
        {
            // 发送报文并获取回复报文
            OperateResult<byte[]> response = ReadBase(send);

            //获取回复不成功
            if (!response.IsSuccess)
            {
                return response;
            }

            // 长度校验
            if (response.Content.Length < 7)
            {
                return new OperateResult<byte[]>(StringResources.Language.ReceiveDataLengthTooShort + "811300");
            }

            // 检查crc
            if (!DK81CommunicationInfo.CheckCRC(response.Content))
            {
                return new OperateResult<byte[]>(StringResources.Language.CRCCheckFailed + SoftBasic.ByteToHexString(response.Content, ' '));
            }

            //回复OK
            if (response.Content[5] == DK81CommunicationInfo.OK)
            {
                return response;
            }

            // 检查是否报故障：是     //TODO 随时主动报故障的问题
            if (response.Content[5] == DK81CommunicationInfo.ErrorCode)
            {
                return new OperateResult<byte[]>(response.Content[6], ((ErrorCode)response.Content[6]).ToString()); //TODO 测试第二种故障码解析:/*DK81CommunicationInfo.GetErrorMessageByErrorCode(response.Content[6])*/
            }

            //检查命令码：命令码不一致且不是OK命令
            if (send[5] != response.Content[5])
            {
                return new OperateResult<byte[]>(response.Content[5], $"Receive Command Check Failed: ");
            }

            return response;
        }
        #endregion

        /*****************************************************************************************************/

        #region protected Commands【操作命令】

        #region 系统命令【操作命令】

        /// <summary>
        /// 执行【联机命令】并返回回复报文
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> HandshakeCommand()
        {
            OperateResult<byte[]> createResult = CreateHandShake();
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 执行【系统模式设置】并返回回复报文
        /// </summary>
        /// <param name="mode">要设置的系统模式</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetSystemModeCommand(SystemMode mode)
        {
            OperateResult<byte[]> createResult = CreateSetSystemMode(mode);
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 执行【显示页面设置】并返回回复报文
        /// </summary>
        /// <param name="mode">要显示的系统页面</param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetDisplayPageCommand(DisplayPage page)
        {
            OperateResult<byte[]> createResult = CreateSetDisplayPage(page);
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        #endregion 系统命令【操作命令】

        /*******************/

        #region 设备信息【操作命令】
        /// <summary>
        /// 读取交流源档位
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadACSourceRangesCommand()
        {
            OperateResult<byte[]> createResult = CreateReadACSourceRanges();
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 读取直流源档位
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadDCSourceRangesCommand()
        {
            OperateResult<byte[]> createResult = CreateReadDCSourceRanges();
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 读取直流表档位
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> ReadDCMeterRangesCommand()
        {
            OperateResult<byte[]> createResult = CreateReadDCMeterourceRanges();
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }
        #endregion 设备信息【操作命令】

        /*******************/

        #region 交流源（表）【操作命令】
        /// <summary>
        /// 交流源关闭命令,返回OK
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> StopACSourceCommand()
        {
            OperateResult<byte[]> createResult = CreateStopACSource();
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 交流源关闭命令,返回OK
        /// </summary>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> StartACSourceCommand()
        {
            OperateResult<byte[]> createResult = CreateStartACSource();
            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 执行【设置交流源档位】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns>下位机回复的有效报文</returns>
        internal OperateResult<byte[]> SetACSourceRangeCommand(int urange, int irange, int ipranges)
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreatSetACSourceRange(urange, irange, ipranges);

            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            //创建指令成功则获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> responseBytes = CheckResponse(createResult.Content);
            return responseBytes;
        }

        /// <summary>
        /// 执行【设置交流源幅值】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="data">操作结果信息</param>
        /// <returns></returns>
        internal OperateResult<byte[]> WriteACSourceAmplitudeCommand(float[] data)
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreateWriteACSourceAmplitude(data);

            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse(createResult.Content);
            return response;
        }

        /// <summary>
        /// 执行【设置源相位】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="data">6个浮点数据：PhaseUA（基准相位必须是0）,PhaseUB,PhaseUC,PhaseIA,PhaseIB,PhaseIC</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WritePhaseCommand(float[] data)
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreateWritePhase(data);

            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse(createResult.Content);
            return response;
        }

        /// <summary>
        /// 执行【设置源频率】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="data">浮点数组：FrequencyA，FrequencyB(必须等于A相)，FrequencyC</param>
        /// <param name="Flag">标志</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> WriteFrequencyCommand(float[] data, byte Flag)
        {
            //创建完整指令报文
            OperateResult<byte[]> createResult = CreateWriteFrequency(data, Flag);

            //创建指令失败
            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            //创建指令成功则发送并获取回复数据：（已保证数据的有效性）
            OperateResult<byte[]> response = CheckResponse(createResult.Content);
            return response;
        }

        /// <summary>
        /// 执行【设置接线模式】命令并获取下位机回复的报文：返回OK
        /// </summary>
        /// <param name="wireMode">枚举接线方式</param>
        /// <returns>带成功标志的操作结果</returns>
        internal OperateResult<byte[]> SetWireModeCommmand(WireMode wireMode)
        {
            OperateResult<byte[]> createResult = CreateSetWireMode(wireMode);
            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            OperateResult<byte[]> response = CheckResponse(createResult.Content);
            return response;
        }
        #endregion 交流源（表）【操作命令】

        #endregion protected Commands【操作命令】

    }
}
