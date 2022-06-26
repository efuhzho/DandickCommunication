using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Dandick.DK55Series;
using DKCommunication.Dandick.DK81Series;


namespace DKCommunication.Core
{
    public interface IReadWriteDK
    {
        /// <summary>
        /// 【联机】
        /// </summary>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> Handshake();

        #region SetSupport:Set意为【不需要用户提供数据】或【选择枚举项】的操作
        /// <summary>
        /// 设置【显示界面】
        /// </summary>
        /// <param name="page">显示页面</param>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> SetDisplayPage(DisplayPage page);

        /// <summary>
        /// 设置【系统模式】
        /// </summary>
        /// <param name="mode">模式</param>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> SetSystemMode(SystemMode mode);
        #endregion

        #region ReadSupport

        #endregion

        #region WriteSupport

        #endregion
    }
}
