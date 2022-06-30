using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
   public interface DKInterfaceBase
    {
        #region Public Properties

      

        /// <summary>
        /// 指示是否具有直流电压输出功能
        /// </summary>
        bool IsDCU_Activated { get; set; }

        /// <summary>
        /// 指示是否具有直流电压输出功能
        /// </summary>
        bool IsDCI_Activated { get; set; }

        /// <summary>
        /// 指示是否具有交流测量功能
        /// </summary>
        bool IsACM_Activated { get; set; }

        /// <summary>
        /// 指示是否具有直流测量模块
        /// </summary>
        bool IsDCM_Activated { get; set; }

        /// <summary>
        /// 指示是否具有电能测量模块
        /// </summary>
        bool IsPQ_Activated { get; set; }

        /// <summary>
        /// 指示是否具有开关量模块
        /// </summary>
        bool IsIO_Activated { get; set; }
        #endregion

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
        OperateResult<byte[]> SetDisplayPage(int page);

        /// <summary>
        /// 设置【系统模式】
        /// </summary>
        /// <param name="mode">模式</param>
        /// <returns>带信息的结果</returns>
        OperateResult<byte[]> SetSystemMode(int mode); 
        #endregion
    }
}
