﻿using System.Collections.Generic;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_ACSource
        <TWireMode,
        TCloseLoopMode,
        THarmonicMode,
        THarmonicChannels,
        THarmonics,
        TChannelWattPower,
        TChannelWattLessPower>
    {
        #region 属性   

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
        /// 当前电压档位的索引值，0为最大档位
        /// </summary>
        int ACU_RangeIndex { get; set; }

        /// <summary>
        /// 当前交流电压档位值，单位V
        /// </summary>
        float ACU_Range { get;  }

        /// <summary>
        /// 当前电流档位的索引值，0为最大档位
        /// </summary>
        int ACI_RangeIndex { get; set; }

        /// <summary>
        /// 当前交流电流档位值，单位A
        /// </summary>
        float ACI_Range { get; }

        /// <summary>
        /// 保护电流档位的索引值，0为最大档位
        /// </summary>
        int IProtect_RangeIndex { get; set; }

        /// <summary>
        /// 当前保护电流档位值，单位A
        /// </summary>
        float IProtect_Range { get; }

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

        /// <summary>
        /// 接线模式枚举
        /// </summary>
        TWireMode WireMode { get; set; }

        /// <summary>
        /// 闭环模式枚举
        /// </summary>
        TCloseLoopMode CloseLoopMode { get; set; }

        /// <summary>
        /// 谐波模式枚举
        /// </summary>
        THarmonicMode HarmonicMode { get; set; }

        /// <summary>
        /// AB相频率(A相、B相频率必须相同)，可当作所有【频率】
        /// </summary>
        float Frequency { get; set; }

        /// <summary>
        /// C相频率
        /// </summary>
        float FrequencyC { get; set; }

        /// <summary>
        /// 当前输出的谐波个数
        /// </summary>
        byte HarmonicCount { get; set; }

        /// <summary>
        /// 当前所有谐波输出通道
        /// </summary>
        THarmonicChannels HarmonicChannels { get; set; }

        /// <summary>
        /// 当前所有谐波输出数据
        /// </summary>
        THarmonics[] Harmonics { get; set; }

        float UA { get; set; }
        float UB { get; set; }
        float UC { get; set; }
        float IA { get; set; }
        float IB { get; set; }
        float IC { get; set; }
        float IProtectA { get; set; }
        float IProtectB { get; set; }
        float IProtectC { get; set; }
        float UaPhase { get; set; }
        float UbPhase { get; set; }
        float UcPhase { get; set; }
        float IaPhase { get; set; }
        float IbPhase { get; set; }
        float IcPhase { get; set; }
        float Pa { get; set; }
        float Pb { get; set; }
        float Pc{ get; set; }
        float P { get; set; }
        float Qa { get; set; }
        float Qb { get; set; }
        float Qc { get; set; }
        float Q { get; set; }
        float Sa { get; set; }
        float Sb { get; set; }
        float Sc { get; set; }
        float S { get; set; }
        float CosFaiA { get; set; }
        float CosFaiB { get; set; }
        float CosFaiC { get; set; }
        float CosFai { get; set; }

        /// <summary>
        /// FLAG=1表示输出不稳定，FLAG=0表示输出已稳定
        /// </summary>
        byte Flag_A { get; }

        /// <summary>
        /// FLAG=1表示输出不稳定，FLAG=0表示输出已稳定
        /// </summary>
        byte Flag_B { get; }

        /// <summary>
        /// FLAG=1表示输出不稳定，FLAG=0表示输出已稳定
        /// </summary>
        byte Flag_C { get; }


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

        #region 【设置档位命令】

        /// <summary>
        /// 设置档位命令
        /// </summary>
        OperateResult<byte[]> SetACSourceRange(int ACU_RangesIndex, int ACI_RangesIndex, int IProtect_RangesIndex);
        OperateResult<byte[]> SetACSourceRange(int ACU_RangesIndex, int ACI_RangesIndex);
        #endregion 设置档位命令

        #region 【设置输出幅度参数】

        /// <summary>
        /// 设置输出幅度参数 【不推荐使用】
        /// </summary>
        OperateResult<byte[]> WriteACSourceAmplitude(float[] data);
        OperateResult<byte[]> WriteACSourceAmplitude(float UA, float UB, float UC, float IA, float IB, float IC, float IPA, float IPB, float IPC);
        OperateResult<byte[]> WriteACSourceAmplitude(float UA, float UB, float UC, float IA, float IB, float IC);
        OperateResult<byte[]> WriteACSourceAmplitude(float U, float I, float IP);
        OperateResult<byte[]> WriteACSourceAmplitude(float U, float I);
        #endregion

        #region 【设置相位参数】

        /// <summary>
        /// 设置相位参数
        /// </summary>
        OperateResult<byte[]> WritePhase(float[] data);
        OperateResult<byte[]> WritePhase(float PhaseUb, float PhaseUc, float PhaseIa, float PhaseIb, float PhaseIc);
        #endregion 设置相位参数

        #region 【设置源频率】

        /// <summary>
        /// 设置频率参数，返回OK，【不推荐使用】
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
        OperateResult<byte[]> WriteFrequency(float FrequencyAB, float FrequencyC);
        #endregion 【设置源频率】

        /// <summary>
        /// 设置接线模式
        /// </summary>
        OperateResult<byte[]> SetWireMode(TWireMode wireMode);

        #region 【设置闭环模式】

        /// <summary>
        /// 【设置闭环模式】
        /// </summary>
        /// <param name="closeLoopMode">枚举闭环模式</param>
        /// <param name="harmonicMode">枚举谐波模式</param>
        /// <returns>带成功标志的操作结果</returns>
        OperateResult<byte[]> SetClosedLoop(TCloseLoopMode closeLoopMode, THarmonicMode harmonicMode);

        /// <summary>
        /// 【设置闭环模式】
        /// </summary>
        /// <param name="closeLoopMode">枚举闭环模式</param>
        /// <returns>带成功标志的操作结果</returns>
        OperateResult<byte[]> SetClosedLoop(TCloseLoopMode closeLoopMode);

        /// <summary>
        /// 【设置闭环模式】
        /// </summary>
        /// <param name="harmonicMode">枚举谐波模式</param>
        /// <returns>带成功标志的操作结果</returns>
        OperateResult<byte[]> SetClosedLoop(THarmonicMode harmonicMode);
        #endregion 【设置闭环模式】

        /// <summary>
        /// 设置多个谐波参数【建议一次最多27个】
        /// </summary>
        OperateResult<byte[]> WriteHarmonics(THarmonicChannels harmonicChannels, THarmonics[] harmonics);

        /// <summary>
        /// 设置一个谐波参数
        /// </summary>
        /// <param name="harmonicChannels"></param>
        /// <param name="harmonics"></param>
        /// <returns></returns>
        OperateResult<byte[]> WriteHarmonics(THarmonicChannels harmonicChannels, THarmonics harmonics);

        /// <summary>
        /// 清空谐波
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        OperateResult<byte[]> ClearHarmonics(THarmonicChannels harmonicChannels);

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
        OperateResult<byte[]> WriteWattPower(TChannelWattPower channel, float p);

        /// <summary>
        /// 设置无功功率参数
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> WriteWattLessPower(TChannelWattLessPower channel, float q);

        /// <summary>
        /// 读取交流源档位信息
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadACSourceRanges();

        #endregion

    }
}
