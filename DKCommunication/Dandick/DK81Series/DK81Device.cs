using DKCommunication.BasicFramework;
using System;
using DKCommunication.Dandick.DKInterface;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81CommandBuilder,IDK_ACSource,IDK_BaseInterface,IDK_DCMeter,IDK_DCSource,IDK_ElectricityModel,IDK_IOModel                          /* :SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
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

        #region Base
        public OperateResult<byte[]> Calibrate_ClearData()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_Save()
        {
            throw new NotImplementedException();
        }

        public OperateResult Handshake()
        {
            throw new NotImplementedException();

            //try
            //{
            //    OperateResult result = HandshakeCommand();
            //    if (result.IsSuccess)
            //    {

            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        public OperateResult<byte[]> SetDisplayPage(int page)
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> SetSystemMode(int mode)
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Start()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Stop()
        {
            throw new NotImplementedException();
        }

        OperateResult IDK_BaseInterface.Start()
        {
            throw new NotImplementedException();
        }

        OperateResult IDK_BaseInterface.Stop()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region ACSource


        #endregion




    }
}
