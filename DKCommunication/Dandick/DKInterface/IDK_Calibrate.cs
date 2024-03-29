﻿namespace DKCommunication.Dandick.DKInterface
{
    internal interface IDK_Calibrate<TCalibrateType, TCalibrateLevel, TCalibrate_DCSourceType,TCalibrate_DCMeterType>
    {
        /// <summary>
        /// 当前校准类型
        /// </summary>
        TCalibrateType CalibrateType { get; set; }

        /// <summary>
        /// 当前校准点
        /// </summary>
        TCalibrateLevel CalibrateLevel { get; set; }

        /// <summary>
        /// 清除已校准的数据
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_ClearData(TCalibrateType calibrateType, byte uRange, byte iRange);

        /// <summary>
        /// 【清空校准参数：直流电压】
        /// </summary>
        /// <param name="calibrateType"></param>
        /// <param name="uRangeIndex"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_ClearDCU_Data(TCalibrateType calibrateType, byte uRangeIndex);

        /// <summary>
        /// 【清空校准参数：直流电流】
        /// </summary>
        /// <param name="calibrateType"></param>
        /// <param name="iRangeIndex"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_ClearDCI_Data(TCalibrateType calibrateType, byte iRangeIndex);

        /// <summary>
        /// 【切换交流源（表）校准档位】
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_SwitchACRange(byte uRangeIndex, byte iRangeIndex);

        // <summary>
        /// 【切换交流源（表）校准点】
        /// </summary>
        /// <param name="uRangeIndex">电压档位索引值</param>
        /// <param name="iRangeIndex">电流档位索引值</param>
        /// <param name="calibrateLevel">当前校准点</param>
        /// <param name="sUA">校准点的标准值</param>
        /// <param name="sUB">校准点的标准值</param>
        /// <param name="sUC">校准点的标准值</param>
        /// <param name="sIA">校准点的标准值</param>
        /// <param name="sIB">校准点的标准值</param>
        /// <param name="sIC">校准点的标准值</param>
        /// <returns>下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        OperateResult<byte[]> Calibrate_SwitchACPoint(byte uRangeIndex, byte iRangeIndex, TCalibrateLevel calibrateLevel, float sUA, float sUB, float sUC, float sIA, float sIB, float sIC);

        /// <summary>
        /// 【设置相位校准点】
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_SwitchACPoint_Phase(byte uRangeIndex, byte iRangeIndex);

        /// <summary>
        /// 【执行交流源校准】
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="mUA"></param>
        /// <param name="mUB"></param>
        /// <param name="mUC"></param>
        /// <param name="mIA"></param>
        /// <param name="mIB"></param>
        /// <param name="mIC"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoAC(byte uRangeIndex, byte iRangeIndex, TCalibrateLevel calibrateLevel, float mUA, float mUB, float mUC, float mIA, float mIB, float mIC);

        /// <summary>
        /// 【确认交流源校准，保存校准参数】
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_Save(byte uRangeIndex, byte iRangeIndex, TCalibrateLevel calibrateLevel);

        /// <summary>
        /// 【交流标准表和钳形表校准】
        /// </summary>
        /// <param name="uRangeIndex"></param>
        /// <param name="iRangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoACMeter(
            byte uRangeIndex,
            byte iRangeIndex,
            TCalibrateLevel calibrateLevel,
            float UA,
            float UB,
            float UC,
            float IA,
            float IB,
            float IC);

        /// <summary>
        /// 【设置直流源校准点】
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_SwitchDCPoint(
           TCalibrate_DCSourceType dCSourceType,
           byte rangeIndex,
           TCalibrateLevel calibrateLevel,
           float sDCAmplitude);

        /// <summary>
        /// 【执行直流源校准】
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoDC(
         TCalibrate_DCSourceType dCSourceType,
         byte rangeIndex,
         TCalibrateLevel calibrateLevel,
         float sDCAmplitude);

        /// <summary>
        /// 【直流表校准】
        /// </summary>
        /// <param name="dCSourceType"></param>
        /// <param name="rangeIndex"></param>
        /// <param name="calibrateLevel"></param>
        /// <param name="sDCAmplitude"></param>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoDCMeter(
        TCalibrate_DCMeterType dCSourceType,
        byte rangeIndex,
        TCalibrateLevel calibrateLevel,
        float sDCAmplitude);
    }
}
