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
        #region Public Properties
        /// <summary>
        /// 设备ID
        /// </summary>
         ushort ID { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
         string Model { get; set; }

        /// <summary>
        /// 设备版本号
        /// </summary>
         string Version { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
         string Serial { get; set; }

         bool IsACU_Activated { get; set; }
         bool IsACI_Activated { get; set; } 

         bool IsDCU_Activated { get; set; } 
         bool IsDCI_Activated { get; set; } 

         bool IsACM_Activated { get; set; } 
         bool IsDCM_Activated { get; set; } 

         bool IsPQ_Activated { get; set; }
         bool IsIO_Activated { get; set; } 
        #endregion


      
        #region SetSupport:Set意为【无需数据返回】的操作
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

        /// <summary>
        /// 交流源关闭命令
        /// </summary>
        /// <returns>携带信息的结果</returns>
        OperateResult<byte[]> Stop( );

        /// <summary>
        /// 交流源打开命令
        /// </summary>
        /// <returns>携带信息的结果</returns>
        OperateResult<byte[]> Start( );

        //OperateResult<byte[]> SetRange(RangeACU rangeACU);
             //OperateResult<byte[]> 
             //OperateResult<byte[]>
             //OperateResult<byte[]> 
             //OperateResult<byte[]>
             //OperateResult<byte[]> 
             //OperateResult<byte[]>

        #endregion

        #region ReadSupport

        #endregion

        #region WriteSupport

        #endregion
    }
}
