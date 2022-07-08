using System.Collections.Generic;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_ACSource<TwireMode> where TwireMode:System.Enum
    {
        #region Properties
        /// <summary>
        /// 指示是否具有交流电压输出功能
        /// </summary>
        bool IsACU_Activated { get; set; }

        /// <summary>
        /// 指示是否具有交流电流输出功能
        /// </summary>
        bool IsACI_Activated { get; set; }

        /// <summary>
        /// 电压档位个数
        /// </summary>
        byte ACU_RangesCount { get; }

        /// <summary>
        /// 电流档位个数
        /// </summary>
        byte ACI_RangesCount { get; }

        /// <summary>
        /// 保护电流档位个数
        /// </summary>
        byte IProtectRangesCount { get; }

        /// <summary>
        /// 只支持A相电压输出的起始档位号
        /// </summary>
        byte URanges_Asingle { get; }

        /// <summary>
        /// 只支持A相电流输出的起始档位号
        /// </summary>
        byte IRanges_Asingle { get; }

        /// <summary>
        /// 只支持A相保护电流输出的起始档位号
        /// </summary>
        byte IProtectRanges_Asingle { get; }

        /// <summary>
        /// 电压档位集合
        /// </summary>
        List<float> ACU_RangesList { get; set; }

        /// <summary>
        /// 电流档位集合
        /// </summary>
        List<float> ACI_RangesList { get; set; }

        /// <summary>
        /// 保护电流档位集合
        /// </summary>
        List<float> IProtect_RangesList { get; set; }

       
        #endregion

        /******************************************************************************************************************************/

        #region Methods

        /// <summary>
        /// 源关闭命令
        /// </summary>
        OperateResult<byte[]> StopACSource();

        /// <summary>
        /// 源打开命令
        /// </summary>
        OperateResult<byte[]> StartACSource();

        /// <summary>
        /// 设置档位命令
        /// </summary>
        OperateResult<byte[]> SetACSourceRange(int ACU_RangesIndex, int ACI_RangesIndex, int IProtect_RangesIndex);
        OperateResult<byte[]> SetACSourceRange(int ACU_RangesIndex, int ACI_RangesIndex);
        /// <summary>
        /// 设置输出幅度参数
        /// </summary>
        OperateResult<byte[]> WriteACSourceAmplitude(float[] data);
        OperateResult<byte[]> WriteACSourceAmplitude(float UA, float UB, float UC, float IA, float IB, float IC, float IPA, float IPB, float IPC);
        OperateResult<byte[]> WriteACSourceAmplitude(float UA, float UB, float UC, float IA, float IB, float IC);
        OperateResult<byte[]> WriteACSourceAmplitude(float U, float I,float IP);
        OperateResult<byte[]> WriteACSourceAmplitude(float U, float I);

        /// <summary>
        /// 设置相位参数
        /// </summary>
        OperateResult<byte[]> WritePhase(float[] data);
        OperateResult<byte[]> WritePhase(float PhaseUb, float PhaseUc, float PhaseIa, float PhaseIb, float PhaseIc);

        /// <summary>
        /// 设置频率参数
        /// </summary>
        /// <param name="data">浮点数组：FrequencyA，FrequencyB(必须等于A相)，FrequencyC</param>
        /// <returns></returns>
        OperateResult<byte[]> WriteFrequency(float[] data);

        /// <summary>
        /// 【设置源频率】，返回OK，【推荐使用】
        /// </summary>
        /// <param name="FrequencyAll">设置三相频率值</param>
        /// <returns>带成功标志的操作结果</returns>
        OperateResult<byte[]> WriteFrequency(float FrequencyAll);

        /// <summary>
        /// 【设置源频率】，返回OK
        /// </summary>
        /// <param name="FrequencyAB">设置AB相频率值</param>
        /// <param name="FrequencyC">设置C相频率值</param>
        /// <returns>带成功标志的操作结果</returns>
        OperateResult<byte[]> WriteFrequency(float FrequencyAB,float FrequencyC);

        /// <summary>
        /// 设置接线模式
        /// </summary>
        OperateResult<byte[]> SetWireMode(TwireMode wireMode);

        /// <summary>
        /// 闭环控制使能命令
        /// </summary>
        OperateResult<byte[]> SetClosedLoop();

        /// <summary>
        /// 设置谐波参数
        /// </summary>
        OperateResult<byte[]> WriteHarmonics();

        /// <summary>
        /// 读取交流源当前输出数据
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadACSourceData();

        /// <summary>
        /// 交流源输出状态标志位：Flag=0表示输出稳定，Flag=1表示输出未稳定
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadACSourceStatus();

        /// <summary>
        /// 设置有功功率参数
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> WriteWattPower();

        /// <summary>
        /// 设置无功功率参数
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> WriteWattlessPower();

        /// <summary>
        /// 切换交流校准档位
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_SwitchACRange();

        /// <summary>
        /// 确认执行当前校准点的校准数据：在输入标准表数据后执行
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoAC();

        /// <summary>
        /// 读取交流源档位信息
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadACSourceRanges();

        /// <summary>
        /// 切换校准点命令
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_SwitchACPoint();

        /// <summary>
        /// 交流标准表和钳形表校准命令
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> Calibrate_DoACMeter();
        #endregion

    }
}
