using DKCommunication.BasicFramework;
using System;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81CommandBuilder /*IReadWriteDK*/                     /* :SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
    {

        #region 私有字段

        #endregion

        #region Constructor   
        /// <summary>
        /// 无参构造方法，默认ID = 0;
        /// </summary>
        public DK81Device() : base()
        {

        }

        /// <summary>
        /// 指定ID的默认构造方法
        /// </summary>
        /// <param name="id"></param>
        public DK81Device(ushort id) : base(id)
        {

        }
        #endregion

        #region Core Interative
        protected virtual OperateResult<byte[]> CheckResponse(byte[] send)
        {
            // 核心交互
            OperateResult<byte[]> response = ReadBase(send);
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

            // 发生了错误
            if (response.Content[5] == 0x52)
            {
                return new OperateResult<byte[]>(response.Content[6], ((ErrorCode)response.Content[6]).ToString()); //TODO 测试第二种故障码解析:/*DK81CommunicationInfo.GetErrorMessageByErrorCode(response.Content[6])*/
            }

            if (send[5] != response.Content[5] && send[5] != 0x4B)
            {
                return new OperateResult<byte[]>(response.Content[5], $"Receive Command Check Failed: ");
            }

            // 移除CRC校验
            //byte[] buffer = new byte[response.Content.Length - 1];
            //Array.Copy(response.Content, 0, buffer, 0, buffer.Length);
            return OperateResult.CreateSuccessResult(response.Content);
        }
        #endregion

        /// <summary>
        /// 【联机命令】
        /// </summary>
        /// <returns>带有信息的结果</returns>
        public OperateResult<byte[]> Handshake()
        {
            try
            {
                OperateResult<byte[]> buffer = CreateHandShake();
                OperateResult<byte[]> response = CheckResponse(buffer.Content);
                if (response.IsSuccess)
                {
                    return OperateResult.CreateSuccessResult(response.Content);
                }
                else
                {
                    return new OperateResult<byte[]>(811300, response.Message);
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811301, ex.Message);
            }
        }
        #region Public Methods 公共方法


        #endregion
    }
}
