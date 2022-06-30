using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_DCSource
    {
        /// <summary>
        /// 指示是否具有直流电压输出功能
        /// </summary>
        bool IsDCU_Activated { get; set; }

        /// <summary>
        /// 指示是否具有直流电压输出功能
        /// </summary>
        bool IsDCI_Activated { get; set; }

        OperateResult<byte[]> SetDCSourceRange( );
        OperateResult<byte[]> StartDCSource( );
        OperateResult<byte[]> StopDCSource( );
        OperateResult<byte[]> WriteDCSourceAmplitude( );
        OperateResult<byte[]> ReadDCSourceData( );

        /// <summary>
        /// 设置直流源校准点
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_SwitchDCPoint( );

        /// <summary>
        /// 直流源校准
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoDC( );

        /// <summary>
        /// 读取直流源档位信息
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCRangeInfo( );

    }
}
