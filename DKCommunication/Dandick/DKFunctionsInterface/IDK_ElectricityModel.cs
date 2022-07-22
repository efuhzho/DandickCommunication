using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_ElectricityModel<TElectricityType>
    {
        /// <summary>
        /// 指示是否具有电能测量模块
        /// </summary>
        bool IsElectricity_Activated { get; set; }

        /// <summary>
        /// 电能校验类型
        /// </summary>
        TElectricityType ElectricityType { get; set; }

        /// <summary>
        /// 表有功常数
        /// </summary>
        float ElectricityMeterPConst { get; set; }

        /// <summary>
        /// 表无功常数
        /// </summary>
        float ElectricityMeterQConst { get; set; }

        /// <summary>
        /// 源有功常数
        /// </summary>
        float ElectricitySourcePConst { get; set; }

        /// <summary>
        /// 源无功常数
        /// </summary>
        float ElectricitySourceQConst { get; set; }

        /// <summary>
        /// （表）分频系数
        /// </summary>
        uint ElectricityMeterDIV { get; set; }

        /// <summary>
        /// （表）设置的校验圈数
        /// </summary>
        uint ElectricitySetMeterRounds { get; set; }

        /// <summary>
        /// 当前校验圈数
        /// </summary>
        uint ElectricityMeasuredRounds { get; }

        /// <summary>
        /// 电能误差有效标志位:Flag=0 表示EV值无效，Flag=80 表示EV值为有功电能校验误差，Flag=81 表示EV值为无功电能校验误差
        /// </summary>
        byte ElectricityDeviationDataFlag { get; }

        /// <summary>
        /// 电能误差数据
        /// </summary>
        float ElectricityDeviationData { get; }   

        /// <summary>
        /// 读取电能误差数据
        /// </summary>
        /// <returns></returns>
        OperateResult< byte[]> ReadElectricityDeviation();

        /// <summary>
        /// 设置电能校验参数并启动电能校验
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity(
            TElectricityType electricityType,
            float meterPConst,
            float meterQConst,           
            uint meterDIV,
            uint meterRounds);

        /// <summary>
        /// 设置源脉冲常数：有功脉冲常数和无功脉冲常数将同时设置为相同
        /// </summary>
        /// <param name="sourcePConst"></param>
        /// <param name="sourceQConst"></param>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity(float sourceConst);

        /// <summary>
        /// 分别设置源有功脉冲常数和无功脉冲常数
        /// </summary>
        /// <param name="sourcePConst"></param>
        /// <param name="sourceQConst"></param>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity(float sourcePConst, float sourceQConst);

    }
}
