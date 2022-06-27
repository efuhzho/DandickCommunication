namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device/* :*/ /*SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
    {
        #region 私有字段
        /// <summary>
        ///声明一个指令生成器
        /// </summary>
        private readonly DK81CommandBuilder _commandBuilder;
        #endregion

        

        #region Constructor   
        /// <summary>
        /// 无参构造方法，默认ID = 0;
        /// </summary>
        public DK81Device( )
        {
            _commandBuilder = new DK81CommandBuilder();           
        }
        /// <summary>
        /// 指定ID的默认构造方法
        /// </summary>
        /// <param name="id"></param>
        public DK81Device(ushort id)
        {
            _commandBuilder = new DK81CommandBuilder(id);
        }
        #endregion

        public OperateResult<byte[]> Handshake( )
        {
            //TODO 添加串口操作
            return _commandBuilder.CreateHandShake();
        }

        public OperateResult<byte[]> SetDisplayPage(DisplayPage page)
        {
            return _commandBuilder.CreateDisplayPage(page);//TODO 添加串口操作
        }

        public OperateResult<byte[]> SetSystemMode(SystemMode mode)
        {
            return (_commandBuilder.CreateSystemMode(mode));//TODO 添加串口操作
        }


        #region Public Methods 公共方法


        #endregion
    }
}
