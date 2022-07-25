using System;

namespace DKCommunication.Dandick.DKInterface
{
    public interface IDK_BaseInterface<TDisplayPage, TSystemMode> 
    {
        TDisplayPage DisplayPage { get; set; }

        TSystemMode SystemMode { get; set; }

        #region Methods
        /// <summary>
        /// 【联机】
        /// </summary>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> Handshake();     

        /// <summary>
        /// 设置【显示界面】
        /// </summary>
        /// <param name="page">显示页面</param>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> SetDisplayPage(TDisplayPage page) ;

        /// <summary>
        /// 设置【系统模式】
        /// </summary>
        /// <param name="mode">模式</param>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> SetSystemMode(TSystemMode mode);
        #endregion
    }    
}
