using System;

namespace DKCommunication.Dandick.DKInterface
{
    public interface IDK_BaseInterface<TDisplayPage, TSystemMode> where TDisplayPage : Enum where TSystemMode : Enum
    {
        #region Methods
        /// <summary>
        /// 【联机】
        /// </summary>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> Handshake();

        /// <summary>
        /// 交流源关闭命令
        /// </summary>
        /// <returns>携带信息的结果</returns>
        OperateResult<byte[]> Stop();

        /// <summary>
        /// 交流源打开命令
        /// </summary>
        /// <returns>携带信息的结果</returns>
        OperateResult<byte[]> Start();

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

        /// <summary>
        /// 保存校准参数
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_Save();

        /// <summary>
        /// 清除已校准的数据
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_ClearData();

        #endregion
    }    
}
