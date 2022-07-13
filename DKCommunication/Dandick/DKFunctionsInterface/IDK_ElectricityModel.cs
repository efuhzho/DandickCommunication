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
        bool IsPQ_Activated { get; set; }


        TElectricityType ElectricityType { get; set; }
        float MeterPConst { get; set; }
        float MeterQConst { get; set; }
        float SourcePConst { get; set; }
        float SourceQConst { get; set; }
        float MeterDIV { get; set; }
        float MeterRounds { get; set; }
        /// <summary>
        /// 读电能误差
        /// </summary>
        /// <returns></returns>
        OperateResult<byte, float, uint, byte[]> ReadElectricityDeviation();

        /// <summary>
        /// 设置电能校验参数并启动电能校验
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity(
            TElectricityType electricityType,
            float meterPConst,
            float meterQConst,           
            float meterDIV,
            float meterRounds);

        /// <summary>
        /// 设置源脉冲常数：有功脉冲常数和无功脉冲常数将同时设置为相同
        /// </summary>
        /// <param name="sourcePConst"></param>
        /// <param name="sourceQConst"></param>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity(float sourceConst);

        /// <summary>
        /// 设置源有功脉冲常数和无功脉冲常数
        /// </summary>
        /// <param name="sourcePConst"></param>
        /// <param name="sourceQConst"></param>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity(float sourcePConst, float sourceQConst);

    }
}
