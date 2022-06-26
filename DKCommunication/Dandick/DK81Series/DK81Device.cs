using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Core;
using DKCommunication.Serial;
using DKCommunication.Dandick.Command;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : SerialDeviceBase<RegularByteTransform>
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
        public DK81Device()
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

        #region Public Methods 公共方法

        /// <summary>
        /// 【联机】
        /// </summary>
        /// <returns></returns>
        public bool HandShake()//TODO 用operateResult类完善返回值
        {
            _commandBuilder.CreateHandShake();
            return true;
        }

        /// <summary>
        /// 【设置显示界面】
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool SetSystemMode(SystemMode mode)
        {
            _commandBuilder.CreateSystemMode(mode);
            return true;
        }
        #endregion
    }
}
