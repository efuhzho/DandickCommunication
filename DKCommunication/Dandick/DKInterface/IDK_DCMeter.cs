using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_DCMeter<TDCMeterMeasureType>
    {
        /// <summary>
        /// 指示是否具有直流测量模块
        /// </summary>
        bool IsDCM_Activated { get; set; }

        // TODO byte M_Type { get; set; }   //单通道OR双通道
        // TODO bool P_EN { get; set; }     //纹波

        /// <summary>
        /// 直流表电压档位数量
        /// </summary>
        byte DCM_URangesCount { get; }

        /// <summary>
        /// 直流表电流档位数量
        /// </summary>
        byte DCM_IRangesCount { get; }

        /// <summary>
        /// 直流表电压档位集合
        /// </summary>
        List<float> DCM_URanges { get; }

        /// <summary>
        /// 直流表电流档位集合
        /// </summary>
        List<float> DCM_IRanges { get; }

        /// <summary>
        /// 直流表测量类型
        /// </summary>
        TDCMeterMeasureType DCM_MeasureType { get; set; }

        /// <summary>
        /// 当前直流表档位索引字
        /// </summary>
        byte DCM_RangeIndex { get; }

        /// <summary>
        /// 当前直流表测量数据
        /// </summary>
        float DCM_Data { get; }

        /// <summary>
        /// 设置直流表量程
        /// </summary>
        /// <param name="rangeIndex">当前直流表档位索引字</param>
        /// <param name="type">直流表测量类型</param>
        /// <returns></returns>
        OperateResult<byte[]> SetDCMeterRange(byte rangeIndex, TDCMeterMeasureType type);

        /// <summary>
        /// 读直流表测量参数/数据
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCMeterData();

        /// <summary>
        /// 设置直流表测量类型：直流或纹波
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> SetDCMeterMesureType(); //TODO 暂时忽略

        /// <summary>
        /// 设置直流表测量参数/数据，适用于双通道
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> SetDCMeterDataWithTwoCh();

        /// <summary>
        /// 读直流表测量参数/数据，适用于双通道
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCMeterDataWithTwoCh();     

        /// <summary>
        /// 读取指标表档位信息
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadDCMeterRanges();
    }
}
