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
        /// <summary>
        /// 接收终端的设备ID
        /// </summary>
        private readonly byte RxID;

        /// <summary>
        /// 发送终端的设备ID
        /// </summary>
        private readonly byte TxID;
        #endregion

        #region Public Properties
        public byte ACU_Range { get; set; } = 2;
        public byte ACI_Range { get; set; } = 1;
        public byte DCU_Range { get; set; } 
        public byte DCI_Range { get; set; } 
        public byte ACM_Range { get; set; } 
        public byte DCM_Range { get; set; } 
        public byte IP_Range { get; set; }
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
        /// 创建指令时的【统一预处理】
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
                buffer[6] = DK81CommunicationInfo.CRCcalculator(buffer);
                return OperateResult.CreateSuccessResult(buffer);
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811200, ex.Message);
            }
        }

        /// <summary>
        /// 创建指令时的【统一预处理】：返回完整指令长度的字节数组，即：包含校验码的空字节空间
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
                    return OperateResult.CreateSuccessResult(result.Content);
                }
                else
                {
                    return new OperateResult<byte[]>() { ErrorCode=811201,Message="创建指令失败"};
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811202, ex.Message);
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
            return CreateCommandHelper(DK81CommunicationInfo.HandShake, DK81CommunicationInfo.HandShakeCommandLength);
        }

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【系统模式】指令对象
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateSystemMode(SystemMode mode)
        {
            return CreateCommandHelper(DK81CommunicationInfo.SetSystemMode, DK81CommunicationInfo.SetSystemModeCommandLength, mode);
        }

        //TODO  建立故障代码监视器：ErrorCodeMonitor. //Page 5

        /// <summary>
        /// 根据丹迪克协议类型创建一个：【当前显示页面】指令对象
        /// </summary>
        /// <param name="page">当前显示页面</param>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateDisplayPage(DisplayPage page)
        {
            return CreateCommandHelper(DK81CommunicationInfo.SetDisplayPage, DK81CommunicationInfo.SetDisplayPageCommandLength, page);
        }

        #endregion

        #region 交流表源命令
        /// <summary>
        /// 创建一个【源关闭】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateStop()
        {
            return CreateCommandHelper(DK81CommunicationInfo.Stop, DK81CommunicationInfo.StopLength);
        }

        /// <summary>
        /// 创建一个【源打开】命令
        /// </summary>
        /// <returns>带指令信息的结果</returns>
        public OperateResult<byte[]> CreateStart()
        {
            return CreateCommandHelper(DK81CommunicationInfo.Start, DK81CommunicationInfo.StartLength);
        }

        #endregion

        #endregion
    }
}
