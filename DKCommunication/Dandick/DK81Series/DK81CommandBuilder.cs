using DKCommunication.Dandick.Base;
using System;

namespace DKCommunication.Dandick.DK81Series
{
    /// <summary>
    /// 丹迪克81协议的命令格式，可以携带站号（ID）、命令码（CommandCode）、数据（DATA）
    /// </summary>
    public class DK81CommandBuilder : DK_DeviceBase
    {
        #region 私有字段
        #region ID
        /// <summary>
        /// 接收终端的设备ID
        /// </summary>
        private readonly byte RxID;

        /// <summary>
        /// 发送终端的设备ID
        /// </summary>
        private readonly byte TxID;
        #endregion

        #region Ranges
        private byte RangeUa;
        private byte RangeUb;
        private byte RangeUc;
        private byte RangeIa;
        private byte RangeIb;
        private byte RangeIc;
        private byte RangeIPa;
        private byte RangeIPb;
        private byte RangeIPc;

        #endregion


        #endregion

        #region Public Properties
        #region 档位
        public byte Range_ACU { get; set; }
        public byte Range_ACI { get; set; }
        public byte Range_DCU { get; set; }
        public byte Range_DCI { get; set; }
        public byte Range_DCM { get; set; }
        public byte Range_IProtect { get; set; }
        #endregion

        #region 相别
        public float UA { get; set; }
        public float UB { get; set; }
        public float UC { get; set; }
        public float IA { get; set; }
        public float IB { get; set; }
        public float IC { get; set; }
        public float IProtectA { get; set; }
        public float IProtectB { get; set; }
        public float IProtectC { get; set; }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// 实例化一个默认的对象，使用默认的地址（0x0000）
        /// </summary>
        public DK81CommandBuilder()
        {
            var result = AnalysisID(0);
            if (result.IsSuccess)
            {
                RxID = result.Content[1];
                TxID = result.Content[0];
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
        public DK81CommandBuilder(ushort id)
        {
            var result = AnalysisID(id);
            if (result.IsSuccess)
            {
                RxID = result.Content[1];
                TxID = result.Content[0];
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        #endregion

        #region Private create command helper 私有指令创建辅助方法
        /// <summary>
        /// 创建7个字节长度指令时的【统一预处理】，不带CRC
        /// </summary>
        /// <param name="commandCode">命令码</param>
        /// <param name="commandLength">指令长度</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateCommandHelper(byte commandCode, ushort commandLength)
        {
            try
            {
                byte[] buffer = new byte[commandLength];
                buffer[0] = DK81CommunicationInfo.FrameID;
                buffer[1] = RxID;
                buffer[2] = TxID;
                buffer[3] = BitConverter.GetBytes(commandLength)[0];
                buffer[4] = BitConverter.GetBytes(commandLength)[1];
                buffer[5] = commandCode;   //默认为：联机命令：DK81CommunicationInfo.HandShake 
                                           // buffer[6] = DK81CommunicationInfo.CRCcalculator(buffer);  //CRC放在后面调用者函数里添加，提高运行效率
                return OperateResult.CreateSuccessResult(buffer);
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811200, ex.Message);
            }
        }

        /// <summary>
        /// 创建8个字节长度指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
        /// </summary>
        /// <typeparam name="T">泛型类，必须可以被转换为byte</typeparam>
        /// <param name="data">数据</param>
        /// <returns>带指令信息的结果</returns>
        private OperateResult<byte[]> CreateCommandHelper<T>(byte commandCode, ushort commandLength, T data) where T : Enum //TODO 添加T类型约束
        {
            try
            {
                var result = CreateCommandHelper(commandCode, commandLength);

                if (result.IsSuccess)
                {
                    result.Content[6] = Convert.ToByte(data);
                    result.Content[7] = DK81CommunicationInfo.CRCcalculator(result.Content);
                    return result;
                }
                else
                {
                    return new OperateResult<byte[]>() { ErrorCode = 811201, Message = "创建指令失败" };
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811202, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandCode"></param>
        /// <param name="commandLength"></param>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateCommandHelper(byte commandCode, ushort commandLength, byte[] dataBytes)
        {
            try
            {
                OperateResult<byte[]> header = CreateCommandHelper(commandCode, commandLength);
                if (header.IsSuccess)
                {
                    Array.Copy(dataBytes, 0, header.Content, 6, dataBytes.Length);
                    header.Content[commandLength - 1] = DK81CommunicationInfo.CRCcalculator(header.Content);
                    return header;
                }
                else
                {
                    return new OperateResult<byte[]>() { ErrorCode = 811203, Message = "创建指令失败" };
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811204, ex.Message);
            }
        }
        #endregion

        #region 指令生成器
        #region 系统命令
        /// <summary>
        /// 根据丹迪克协议类型创建一个【联机指令】对象
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateHandShake()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.HandShake, DK81CommunicationInfo.HandShakeCommandLength);

            if (bytesHeader.IsSuccess)
            {
                bytesHeader.Content[6] = DK81CommunicationInfo.CRCcalculator(bytesHeader.Content);
                return bytesHeader;
            }
            else
            {
                return new OperateResult<byte[]>(811201, "创建指令失败");
            }
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateSystemMode(SystemMode mode)
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.SetSystemMode, DK81CommunicationInfo.SetSystemModeCommandLength, mode);
            if (bytesHeader.IsSuccess)
            {
                return bytesHeader;
            }
            else
            {
                return new OperateResult<byte[]>(811202, "创建指令失败");
            }
        }

        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateDisplayPage(DisplayPage page)    //TODO 将DisplayPage写成属性
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
        #endregion

        #region 交流表源命令
        /// <summary>
        /// 创建一个【源关闭】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateStop()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.Stop, DK81CommunicationInfo.StopLength);
            if (bytesHeader.IsSuccess)
            {
                return bytesHeader;
            }
            else
            {
                return new OperateResult<byte[]>(811204, "创建指令失败");
            }
        }

        /// <summary>
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateStart()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.Start, DK81CommunicationInfo.StartLength);
            if (bytesHeader.IsSuccess)
            {
                return bytesHeader;
            }
            else
            {
                return new OperateResult<byte[]>(811205, "创建指令失败");
            }
        }

        //public OperateResult<byte[]> CreatSetRange( )
        //{
        //    OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.SetRange, DK81CommunicationInfo.SetRangeLength);

        //}

        #endregion

        #region 设备信息
        public OperateResult<byte[]> CreateReadRangeInfo()
        {
            OperateResult<byte[]> bytesHeader = CreateCommandHelper(DK81CommunicationInfo.ReadRangeInfo, DK81CommunicationInfo.ReadRangeInfoLength);

            if (bytesHeader.IsSuccess)
            {
                bytesHeader.Content[6] = DK81CommunicationInfo.CRCcalculator(bytesHeader.Content);
                return bytesHeader;
            }
            else
            {
                return new OperateResult<byte[]>(811201, "创建指令失败");
            }
        }
        #endregion

        #endregion
    }
}
