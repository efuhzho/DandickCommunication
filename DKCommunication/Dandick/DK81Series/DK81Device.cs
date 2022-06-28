using DKCommunication.Core;
using System;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81CommandBuilder, IReadWriteDK                     /* :SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
    {
        #region 私有字段
        
        #endregion

        #region Constructor   
        /// <summary>
        /// 无参构造方法，默认ID = 0;
        /// </summary>
        public DK81Device( ) : base()
        {
            SerialPortInni("com5");
        }

        /// <summary>
        /// 指定ID的默认构造方法
        /// </summary>
        /// <param name="id"></param>
        public DK81Device(ushort id) : base(id)
        {
            //_commandBuilder = new DK81CommandBuilder(id);
        }
      
        #endregion

        /// <summary>
        /// 【联机命令】
        /// </summary>
        /// <returns>带有信息的结果</returns>
        public OperateResult<byte[]> Handshake( )
        {
            var result = CreateHandShake();
            try
            {
                if (result.IsSuccess)
                {
                    return OperateResult.CreateSuccessResult(result.Content);
                }
                else
                {
                    return new OperateResult<byte[]>(811300, result.Message);
                }
            }
            catch (Exception ex)
            {
                return new OperateResult<byte[]>(811301, ex.Message);
            }           
        }

        public OperateResult<byte[]> SetDisplayPage(DisplayPage page)
        {
            return CreateDisplayPage(page);//TODO 添加串口操作
        }

        public OperateResult<byte[]> SetSystemMode(SystemMode mode)
        {
            return (CreateSystemMode(mode));//TODO 添加串口操作
        }

        public OperateResult<byte[]> Start( )
        {
            throw new System.NotImplementedException();
        }

        public OperateResult<byte[]> Stop( )
        {
            throw new System.NotImplementedException();
        }
       
        #region Public Methods 公共方法


        #endregion
    }
}
