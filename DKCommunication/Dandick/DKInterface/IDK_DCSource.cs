using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_DCSource<TDCSourceType>
    {
        /// <summary>
        /// 指示是否具有直流电压输出功能
        /// </summary>
        bool IsDCU_Activated { get; set; }

        /// <summary>
        /// 指示是否具有直流电流输出功能
        /// </summary>
        bool IsDCI_Activated { get; set; }

        /// <summary>
        /// 直流源电压档位个数
        /// </summary>
        byte DCU_RangesCount { get;  }

        /// <summary>
        /// 直流源电流档位个数
        /// </summary>
        byte DCI_RangesCount { get;  }

        /// <summary>
        /// 当前直流源幅值
        /// </summary>
        float DCSourceData { get; }

        /// <summary>
        /// 当前档位索引值
        /// </summary>
        byte DCSourceRangeIndex { get; set; }

        /// <summary>
        /// 直流源电压档位列表
        /// </summary>
        List<float> DCU_Ranges { get; }

        /// <summary>
        /// 直流源电流档位列表
        /// </summary>
        List<float> DCI_Ranges { get; }

        /// <summary>
        /// 直流源输出类型
        /// </summary>
        TDCSourceType DCS_Type { get; set; }

        /// <summary>
        /// 当前直流源输出状态：true=源打开；false=源关闭
        /// </summary>
        bool DCS_Status { get; }

        /// <summary>
        /// 设置直流源档位
        /// </summary>
        /// <param name="rangeIndex"></param>
        /// <param name="dCSourceType"></param>
        /// <returns></returns>
        OperateResult<byte[]> SetDCSourceRange(byte rangeIndex, TDCSourceType dCSourceType);

        /// <summary>
        /// 设置直流源档位：自动档位模式
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> SetDCSourceRangeAuto();

        /// <summary>
        /// 打开直流源
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <returns></returns>
        OperateResult<byte[]> StartDCSource(TDCSourceType dCSourceType);

        /// <summary>
        /// 关闭直流源
        /// </summary>
        /// <param name="dCSourceType">直流源输出类型</param>
        /// <returns></returns>
        OperateResult<byte[]> StopDCSource(TDCSourceType dCSourceType);

        /// <summary>
        /// 关闭直流源
        /// </summary>
        /// <returns>直流源输出类型</returns>
        OperateResult<byte[]> StopDCSource();

        /// <summary>
        /// 【设置直流源幅值】
        /// </summary>
        /// <param name="rangeIndex"></param>
        /// <param name="SData"></param>
        /// <param name="dCSourceType"></param>
        /// <returns></returns>
        OperateResult<byte[]> WriteDCSourceAmplitude(byte rangeIndex, float SData, TDCSourceType dCSourceType);

        /// <summary>
        /// 读取直流源数据
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCSourceData(TDCSourceType dCSourceType);



        /// <summary>
        /// 读取直流源档位
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCSourceRanges();

    }
}
