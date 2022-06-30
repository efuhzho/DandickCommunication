using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_DCMeter
    {
        /// <summary>
        /// 指示是否具有直流测量模块
        /// </summary>
        bool IsDCM_Activated { get; set; }

        /// <summary>
        /// 设置直流表量程
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> SetDCMeterRange( );

        /// <summary>
        /// 读直流表测量参数/数据
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCMeterData( );

        /// <summary>
        /// 设置直流表测量类型：DCU or DCI
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> SetDCMeterMesureType( );

        /// <summary>
        /// 设置直流表测量参数/数据，适用于双通道
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> SetDCMeterDataWithTwoCh( );

        /// <summary>
        /// 读直流表测量参数/数据，适用于双通道
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCMeterDataWithTwoCh( );

        /// <summary>
        /// 直流表校准
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoDCMeter( );
    }
}
