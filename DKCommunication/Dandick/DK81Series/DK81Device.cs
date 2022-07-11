using DKCommunication.Dandick.DKInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81Command,
        IDK_BaseInterface<DisplayPage, SystemMode>, //系统接口
        IDK_ACSource<WireMode, CloseLoopMode, HarmonicMode, ChannelsHarmonic, Harmonics, ChannelWattPower, ChannelWattLessPower>,    //交流源接口
        IDK_DCMeter,    //直流表接口
        IDK_DCSource,   //直流源接口
        IDK_ElectricityModel,   //电能模块接口
        IDK_IOModel     //开关量模块接口
    {

        #region --------------------------------- 私有字段 ----------------------------------------

        #region ACSource 交流源
        /// <summary>
        /// 交流电压档位数量
        /// </summary>
        private byte _uRangesCount;

        /// <summary>
        /// 交流电流档位数量
        /// </summary>
        private byte _iRangesCount;

        /// <summary>
        /// 保护电流档位数量
        /// </summary>
        private byte _iProtectRangesCount;

        /// <summary>
        /// 只支持A相输出的电压档位起始索引
        /// </summary>
        private byte _uRanges_Asingle;

        /// <summary>
        /// 只支持A相输出的电流档位起始索引
        /// </summary>
        private byte _iRanges_Asingle;

        /// <summary>
        /// 只支持A相输出的保护电流档位起始索引
        /// </summary>
        private byte _iProtectRanges_Asingle;

        /// <summary>
        /// 交流电压档位列表:如果初始化失败则默认DK-34B1档位
        /// </summary>
        private List<float> _uRanges = new List<float> { 380f, 220f, 100f, 57.7f };

        /// <summary>
        /// 交流电流档位列表:如果初始化失败则默认DK-34B1档位
        /// </summary>
        private List<float> _iRanges = new List<float> { 20f, 5f, 2f, 1f };

        /// <summary>
        /// 保护电流档位列表
        /// </summary>
        private List<float> _iProtectRanges = new List<float> { 0, 0, 0, 0 };       //TODO 删除默认值设定？


        #endregion ACSource 交流源

        #region DCSource 直流源
        /// <summary>
        /// 直流电压档位数量
        /// </summary>
        private byte _DCURangesCount;

        /// <summary>
        /// 直流电流档位数量
        /// </summary>
        private byte _DCIRangesCount;

        /// <summary>
        /// 直流电压档位列表
        /// </summary>
        private List<float> _DCURanges;

        /// <summary>
        /// 直流电流档位列表
        /// </summary>
        private List<float> _DCIRanges;
        #endregion

        #region DCMeter 直流表

        /// <summary>
        /// /直流表电压档位数量
        /// </summary>
        private byte _DCMeterURangesCount;

        /// <summary>
        /// 直流表电流档位数量
        /// </summary>
        private byte _DCMeterIRangesCount;

        /// <summary>
        /// 直流电压档位列表
        /// </summary>
        private List<float> _DCMeterURanges;

        /// <summary>
        /// 直流电流档位列表
        /// </summary>
        private List<float> _DCMeterIRanges;
        #endregion

        #endregion 私有字段

        #region --------------------------------- Constructor -------------------------------------  
        /// <summary>
        /// 无参构造方法，默认ID = 0;
        /// </summary>
        public DK81Device() : base()
        {

        }

        /// <summary>
        /// 指定ID的默认构造方法
        /// </summary>
        /// <param name="id"></param>
        public DK81Device(ushort id) : base(id)
        {

        }
        #endregion Constructor

        #region --------------------------------- Public Properties 属性 --------------------------

        #region Base 系统信号
        /// <summary>
        /// 当前显示页面
        /// </summary>
        public DisplayPage DisplayPage { get; set; } = DisplayPage.PageDefault;

        /// <summary>
        /// 系统模式
        /// </summary>
        public SystemMode SystemMode { get; set; } = SystemMode.ModeDefault;

        #endregion Base 系统信号

        #region Functions

        public bool IsDCM_Activated { get; set; } = true;
        public bool IsDCU_Activated { get; set; } = true;
        public bool IsDCI_Activated { get; set; } = true;
        public bool IsPQ_Activated { get; set; } = true;
        public bool IsIO_Activated { get; set; }
        #endregion

        #region ACSource 交流源 
        /// <summary>
        /// 是否具备交流电压功能
        /// </summary>
        public bool IsACU_Activated { get; set; } = true;

        /// <summary>
        /// 是否具备交流电流功能
        /// </summary>
        public bool IsACI_Activated { get; set; } = true;

        /// <summary>
        /// 当前电压档位的索引值，0为最大档位，例如：0-380V；1-220V......
        /// </summary>
        public int ACU_Range { get; set; } = 2;

        /// <summary>
        /// 当前电流档位的索引值，0为最大档位,例如：0-20A；1-5A......
        /// </summary>
        public int ACI_Range { get; set; } = 1;

        /// <summary>
        /// 保护电流档位的索引值，0为最大档位
        /// </summary>
        public int IProtect_Range { get; set; } = 0;

        /// <summary>
        /// 交流电压档位个数
        /// </summary>
        public byte ACU_RangesCount => _uRangesCount;

        /// <summary>
        /// 交流电流档位个数
        /// </summary>
        public byte ACI_RangesCount => _iRangesCount;

        /// <summary>
        /// 保护电流档位个数
        /// </summary>
        public byte IProtectRangesCount => _iProtectRangesCount;

        /// <summary>
        /// 交流电压档位列表
        /// </summary>
        public List<float> ACU_RangesList
        {
            get { return _uRanges; }
            set { _uRanges = value; }
        }

        /// <summary>
        /// 交流电流档位列表
        /// </summary>
        public List<float> ACI_RangesList
        {
            get { return _iRanges; }
            set { _iRanges = value; }
        }

        /// <summary>
        /// 保护电流档位列表
        /// </summary>
        public List<float> IProtect_RangesList
        {
            get { return _iProtectRanges; }
            set { _iProtectRanges = value; }
        }

        /// <summary>
        /// 只支持A相电流输出的起始档位号
        /// </summary>
        public byte IRanges_Asingle => _iRanges_Asingle;

        /// <summary>
        /// 只支持A相保护电流输出的起始档位号
        /// </summary>
        public byte IProtectRanges_Asingle => _iProtectRanges_Asingle;

        /// <summary>
        /// 只支持A相电压输出的起始档位号
        /// </summary>
        public byte URanges_Asingle => _uRanges_Asingle;

        /// <summary>
        /// 接线模式枚举
        /// </summary>
        public WireMode WireMode { get; set; } = WireMode.WireMode_3P4L;

        /// <summary>
        /// 闭环模式枚举
        /// </summary>
        public CloseLoopMode CloseLoopMode { get; set; } = 0;

        /// <summary>
        /// 谐波模式枚举
        /// </summary>
        public HarmonicMode HarmonicMode { get; set; } = 0;

        /// <summary>
        /// 【频率F】:，通常情况下为【频率F】，特殊情况下指AB相频率(A相、B相频率必须相同)
        /// </summary>
        public float Frequency { get; set; } = 50F;

        /// <summary>
        /// C相频率
        /// </summary>
        public float FrequencyC { get; set; } = 50F;

        private byte _harmonicCount = 0;
        /// <summary>
        /// 当前输出的谐波个数
        /// </summary>
        public byte HarmonicCount
        {
            get { return _harmonicCount; }
            set
            {
                if (_harmonicCount >= 0 && _harmonicCount < 28) //协议建议长度超过256最好分批发送
                {
                    _harmonicCount = value;
                }
            }
        }

        /// <summary>
        /// 当前所有谐波输出通道
        /// </summary>
        public ChannelsHarmonic HarmonicChannels { get; set; }

        /// <summary>
        /// 当前所有谐波输出数据
        /// </summary>
        public Harmonics[] Harmonics { get; set; }


        private float _UA;
        /// <summary>
        /// 当前UA的输出值
        /// </summary>  
        public float UA
        {
            get { return _UA; }
            set { _UA = value; }
        }

        private float _UB;
        /// <summary>
        /// 当前UB的输出值
        /// </summary>  
        public float UB
        {
            get { return _UB; }
            set { _UB = value; }
        }

        private float _UC;
        /// <summary>
        /// 当前UC的输出值
        /// </summary>  
        public float UC
        {
            get { return _UC; }
            set { _UC = value; }
        }

        private float _IA;
        /// <summary>
        /// 当前IA的输出值
        /// </summary>  
        public float IA
        {
            get { return _IA; }
            set { _IA = value; }
        }

        private float _IB;
        /// <summary>
        /// 当前IB的输出值
        /// </summary>  
        public float IB
        {
            get { return _IB; }
            set { _IB = value; }
        }

        private float _IC;
        /// <summary>
        /// 当前IC的输出值
        /// </summary>  
        public float IC
        {
            get { return _IC; }
            set { _IC = value; }
        }

        private float _IProtectA;
        /// <summary>
        /// 当前IProtectA的输出值
        /// </summary>  
        public float IProtectA
        {
            get { return _IProtectA; }
            set { _IProtectA = value; }
        }

        private float _IProtectB;
        /// <summary>
        /// 当前IProtectB的输出值
        /// </summary>  
        public float IProtectB
        {
            get { return _IProtectB; }
            set { _IProtectB = value; }
        }

        private float _IProtectC;
        /// <summary>
        /// 当前IProtectC的输出值
        /// </summary>  
        public float IProtectC
        {
            get { return _IProtectC; }
            set { _IProtectC = value; }
        }

        private float _UaPhase;
        /// <summary>
        /// Ua相位
        /// </summary>
        public float UaPhase
        {
            get { return _UaPhase; }
            set { _UaPhase = value; }
        }

        private float _UbPhase;
        /// <summary>
        /// Ub相位
        /// </summary>
        public float UbPhase
        {
            get { return _UbPhase; }
            set { _UbPhase = value; }
        }

        private float _UcPhase;
        /// <summary>
        /// Uc相位
        /// </summary>
        public float UcPhase
        {
            get { return _UcPhase; }
            set { _UcPhase = value; }
        }

        private float _IaPhase;
        /// <summary>
        /// Ia相位
        /// </summary>
        public float IaPhase
        {
            get { return _IaPhase; }
            set { _IaPhase = value; }
        }

        private float _IbPhase;
        /// <summary>
        /// Ib相位
        /// </summary>
        public float IbPhase
        {
            get { return _IbPhase; }
            set { _IbPhase = value; }
        }

        private float _IcPhase;
        /// <summary>
        /// Ic相位
        /// </summary>
        public float IcPhase
        {
            get { return _IcPhase; }
            set { _IcPhase = value; }
        }
        #endregion ACSource 交流源

        #region DCSource
        public byte DCU_RangesCount => _DCURangesCount;
        public byte DCI_RangesCount => _DCIRangesCount;
        public List<float> DCU_Ranges => _DCURanges;
        public List<float> DCI_Ranges => _DCIRanges;
        #endregion

        #region DCMeter
        public byte DCM_URangesCount => _DCMeterURangesCount;

        public byte DCM_IRangesCount => _DCMeterIRangesCount;

        public List<float> DCM_URanges => _DCMeterURanges;

        public List<float> DCM_IRanges => _DCMeterIRanges;



        #endregion

        #endregion Public Properties 属性

        #region --------------------------------- Public Methods ----------------------------------

        #region 系统信号
        /// <summary>
        /// 解析【联机指令】的回复报文，并单向初始化设备信息，不对初始化信息做任何判断
        /// 【并非所有设备都会返回准确的设备信息】
        /// </summary>
        /// <returns>包含信息的操作结果</returns>
        public OperateResult<byte[]> Handshake()
        {
            OperateResult<byte[]> response = HandshakeCommand();

            if (response.IsSuccess)
            {
                AnalysisHandshake(response.Content);
                ReadACSourceRanges();
                ReadDCSourceRanges();
                ReadDCMeterRanges();
            }
            return response;
        }

        /// <summary>
        /// 【系统模式设置】返回OK
        /// </summary>
        /// <param name="mode">系统模式</param>
        /// <returns>返回OK,下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> SetSystemMode(SystemMode mode)
        {
            OperateResult<byte[]> response = SetSystemModeCommand(mode);
            SystemMode = mode;
            return response;
        }

        /// <summary>
        /// 【显示页面设置】返回OK
        /// </summary>
        /// <param name="page">要设置的显示页面</param>
        /// <returns>返回OK,下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> SetDisplayPage(DisplayPage page)
        {
            OperateResult<byte[]> response = SetDisplayPageCommand(page);
            DisplayPage = page;
            return response;
        }
        #endregion 系统信号             

        #region 设备信息
        /// <summary>
        /// 读取【交流源档位】，初始化设备只读属性
        /// </summary>
        /// <returns>下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> ReadACSourceRanges()
        {
            OperateResult<byte[]> response = ReadACSourceRangesCommand();
            if (response.IsSuccess)
            {
                AnalysisReadACSourceRanges(response.Content);
            }
            return response;
        }

        /// <summary>
        /// 读取【直流表档位】，初始化设备只读属性
        /// </summary>
        /// <returns>下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> ReadDCMeterRanges()
        {
            OperateResult<byte[]> response = ReadDCMeterRangesCommand();
            if (response.IsSuccess)
            {
                AnalysisReadDCMeterRanges(response.Content);
            }
            return response;
        }

        /// <summary>
        /// 读取【直流源档位】，初始化设备只读属性
        /// </summary>
        /// <returns>下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> ReadDCSourceRanges()
        {
            OperateResult<byte[]> response = ReadDCSourceRangesCommand();
            if (response.IsSuccess)
            {
                AnalysisReadDCSourceRanges(response.Content);
            }
            return response;
        }
        #endregion 设备信息

        #region 交流源（表）操作命令
        /// <summary>
        /// 交流源关闭命令,返回OK
        /// </summary>
        /// <returns>下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> StopACSource()
        {
            OperateResult<byte[]> response = StopACSourceCommand();
            return response;
        }

        /// <summary>
        /// 【交流源打开】命令,返回OK
        /// </summary>
        /// <returns>下位机回复的原始报文，用于自主解析，通常可忽略</returns>
        public OperateResult<byte[]> StartACSource()
        {
            OperateResult<byte[]> response = StartACSourceCommand();
            return response;
        }

        #region 【档位设置】

        /// <summary>
        /// 【档位设置】命令，返回OK
        /// </summary>
        /// <param name="ACU_RangesIndex">交流电压档位的索引值，最大档位索引为0</param>
        /// <param name="ACI_RangesIndex">交流电流档位的索引值，最大档位索引为0</param>
        /// <param name="IProtect_RangesIndex">【仅DK-51F】保护电流档位的索引值，最大档位索引为0</param>
        /// <returns></returns>
        public OperateResult<byte[]> SetACSourceRange(int ACU_RangesIndex, int ACI_RangesIndex, int IProtect_RangesIndex)  //TODO 档位有效值在属性中限定
        {
            OperateResult<byte[]> response = SetACSourceRangeCommand(ACU_RangesIndex, ACI_RangesIndex, IProtect_RangesIndex);
            ACU_Range = ACU_RangesIndex;
            ACI_Range = ACI_RangesIndex;
            IProtect_Range = IProtect_RangesIndex;
            return response;
        }

        /// <summary>
        /// 【档位设置】命令，返回OK
        /// </summary>
        /// <param name="ACU_RangesIndex">交流电压档位的索引值，最大档位索引为0</param>
        /// <param name="ACI_RangesIndex">交流电流档位的索引值，最大档位索引为0</param>
        /// <returns></returns>
        public OperateResult<byte[]> SetACSourceRange(int ACU_RangesIndex, int ACI_RangesIndex)  //TODO 档位有效值在属性中限定
        {
            OperateResult<byte[]> response = SetACSourceRangeCommand(ACU_RangesIndex, ACI_RangesIndex, IProtect_Range);
            return response;
        }
        #endregion 【档位设置】

        #region 【设置交流源幅度】

        /// <summary>
        /// 【设置交流源幅度】命令，返回OK，【不推荐使用】
        /// </summary>
        /// <param name="data">将要设定的幅值（6个或9个浮点数据：UA,UB,UC,IA,IB,IC,IPA(可选),IPB(可选),IPC(可选)）</param>
        /// <returns>操作结果</returns>
        public OperateResult<byte[]> WriteACSourceAmplitude(float[] data)
        {
            if (data.Length != 6 && data.Length != 9)
            {
                return new OperateResult<byte[]>("请输入6个浮点数据或9个浮点数据!");
            }
            OperateResult<byte[]> response = WriteACSourceAmplitudeCommand(data);
            return response;
        }

        /// <summary>
        /// 【设置交流源幅度】命令，返回OK，【推荐使用】
        /// </summary>
        /// <param name="UA">A相电压幅值</param>
        /// <param name="UB">B相电压幅值</param>
        /// <param name="UC">C相电压幅值</param>
        /// <param name="IA">A相电流幅值</param>
        /// <param name="IB">B相电流幅值</param>
        /// <param name="IC">C相电流幅值</param>
        /// <param name="IPA">相保护电流幅值</param>
        /// <param name="IPB">相保护电流幅值</param>
        /// <param name="IPC">相保护电流幅值</param>
        /// <returns></returns>
        public OperateResult<byte[]> WriteACSourceAmplitude(float UA, float UB, float UC, float IA, float IB, float IC, float IPA, float IPB, float IPC)
        {
            float[] data = new float[9] { UA, UB, UC, IA, IB, IC, IPA, IPB, IPC };
            return WriteACSourceAmplitude(data);
        }

        /// <summary>
        /// 【设置交流源幅度】命令，返回OK，【推荐使用】
        /// </summary>
        /// <param name="UA">A相电压幅值</param>
        /// <param name="UB">B相电压幅值</param>
        /// <param name="UC">C相电压幅值</param>
        /// <param name="IA">A相电流幅值</param>
        /// <param name="IB">B相电流幅值</param>
        /// <param name="IC">C相电流幅值</param>
        /// <returns></returns>
        public OperateResult<byte[]> WriteACSourceAmplitude(float UA, float UB, float UC, float IA, float IB, float IC)
        {
            float[] data = new float[9] { UA, UB, UC, IA, IB, IC, 0, 0, 0 };
            return WriteACSourceAmplitude(data);
        }

        /// <summary>
        /// 【设置交流源幅度】命令，返回OK，【推荐使用】
        /// </summary>
        /// <param name="U">三相电压幅值</param>
        /// <param name="I">三相电流幅值</param>
        /// <param name="IP">三相保护电流幅值</param>
        /// <returns></returns>
        public OperateResult<byte[]> WriteACSourceAmplitude(float U, float I, float IP)
        {
            float[] data = new float[9] { U, U, U, I, I, I, IP, IP, IP };
            return WriteACSourceAmplitude(data);
        }

        /// <summary>
        /// 【设置交流源幅度】命令，返回OK，【推荐使用】
        /// </summary>
        /// <param name="U">三相电压幅值</param>
        /// <param name="I">三相电流幅值</param>
        /// <returns></returns>
        public OperateResult<byte[]> WriteACSourceAmplitude(float U, float I)
        {
            float[] data = new float[9] { U, U, U, I, I, I, 0, 0, 0 };
            return WriteACSourceAmplitude(data);
        }
        #endregion 【设置交流源幅度】

        #region 【设置源相位】

        /// <summary>
        /// 【设置源相位】，返回OK，【不推荐使用】
        /// </summary>
        /// <param name="data">6个浮点数据：PhaseUA（基准相位必须是0）,PhaseUB,PhaseUC,PhaseIA,PhaseIB,PhaseIC</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WritePhase(float[] data)
        {
            if (data.Length != 6)
            {
                return new OperateResult<byte[]>("请输入正确的频率值组合：必须是6个浮点型数据");
            }
            OperateResult<byte[]> response = WritePhaseCommand(data);
            return response;
        }

        /// <summary>
        /// 【设置源相位】，返回OK，【推荐使用】
        /// </summary>
        /// <param name="PhaseUb"></param>
        /// <param name="PhaseUc"></param>
        /// <param name="PhaseIa"></param>
        /// <param name="PhaseIb"></param>
        /// <param name="PhaseIc"></param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WritePhase(float PhaseUb, float PhaseUc, float PhaseIa, float PhaseIb, float PhaseIc)
        {
            float[] data = new float[] { 0f, PhaseUb, PhaseUc, PhaseIa, PhaseIb, PhaseIc };
            OperateResult<byte[]> response = WritePhase(data);
            return response;
        }
        #endregion 【设置源相位】

        #region 【设置源频率】

        /// <summary>
        /// 【设置源频率】，返回OK，【不推荐使用】
        /// </summary>
        /// <param name="data">浮点数组：FrequencyA，FrequencyB(必须等于A相)，FrequencyC</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteFrequency(float[] data)
        {
            //数据长度不是3：直接返回失败结果
            if (data.Length != 3 || data == null)
            {
                return new OperateResult<byte[]>("请输入正确的频率值个数");
            }

            //数据长度是3
            byte Flag = 3;
            if (data[0] == data[1] && data[0] != data[2])
            {
                Flag = 2;
            }

            OperateResult<byte[]> response = WriteFrequencyCommand(data, Flag);
            return response;
        }

        /// <summary>
        /// 【设置源频率】，返回OK，【推荐使用】
        /// </summary>
        /// <param name="frequencyAll">设置三相频率值</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteFrequency(float frequencyAll)
        {
            return WriteFrequency(frequencyAll, frequencyAll);
        }

        /// <summary>
        /// 【设置源频率】，返回OK
        /// </summary>
        /// <param name="frequencyAB">设置AB相频率值</param>
        /// <param name="frequencyC">设置C相频率值</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteFrequency(float frequencyAB, float frequencyC)
        {
            float[] data = new float[] { frequencyAB, frequencyAB, frequencyC };
            Frequency = frequencyAB;
            FrequencyC = frequencyC;
            return WriteFrequency(data);
        }
        #endregion 【设置源频率】

        /// <summary>
        /// 【设置接线方式】，返回OK
        /// </summary>
        /// <param name="wireMode">枚举接线方式</param>
        /// <returns></returns>
        public OperateResult<byte[]> SetWireMode(WireMode wireMode)
        {
            OperateResult<byte[]> response = SetWireModeCommmand(wireMode);
            WireMode = wireMode;
            return response;
        }

        #region 【设置闭环模式】

        /// <summary>
        /// 【设置闭环模式】
        /// </summary>
        /// <param name="closeLoopMode">枚举闭环模式</param>
        /// <param name="harmonicMode">枚举谐波模式</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> SetClosedLoop(CloseLoopMode closeLoopMode, HarmonicMode harmonicMode)
        {
            OperateResult<byte[]> response = SetClosedLoopCommmand(closeLoopMode, harmonicMode);
            CloseLoopMode = closeLoopMode;
            HarmonicMode = harmonicMode;
            return response;
        }

        /// <summary>
        /// 【设置闭环模式】
        /// </summary>
        /// <param name="closeLoopMode">枚举闭环模式</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> SetClosedLoop(CloseLoopMode closeLoopMode)
        {
            return SetClosedLoop(closeLoopMode, HarmonicMode);
        }

        /// <summary>
        /// 【设置闭环模式】：谐波模式设置
        /// </summary>
        /// <param name="harmonicMode">枚举谐波模式</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> SetClosedLoop(HarmonicMode harmonicMode)
        {
            return SetClosedLoop(CloseLoopMode, harmonicMode);
        }
        #endregion 设置闭环模式

        #region 【设置谐波参数】
        /// <summary>
        /// 【谐波参数设置】:设置多个谐波参数
        /// </summary>
        /// <param name="harmonicChannels">枚举谐波通道</param>
        /// <param name="harmonics">谐波数据结构体组合</param>
        /// <remarks>
        /// 使用方法举例1：this.WriteHarmonics(（HarmonicChannels）30,data)则表示选择了Ub,Uc,Ia,Ib通道
        /// 使用方法举例2：this.WriteHarmonics(HarmonicChannels.Channel_Ua,data)则表示选择了Ua通道
        /// </remarks>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteHarmonics(ChannelsHarmonic harmonicChannels, Harmonics[] harmonics)
        {
            OperateResult<byte[]> response = WriteHarmonicsCommmand(harmonicChannels, harmonics);
            return response;
        }

        /// <summary>
        /// 【谐波参数设置】：设置一个谐波参数
        /// </summary>
        /// <param name="harmonicChannels">谐波通道选择</param>
        /// <param name="harmonic">谐波参数</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteHarmonics(ChannelsHarmonic harmonicChannels, Harmonics harmonic)
        {
            Harmonics[] data = new Harmonics[1] { harmonic };
            OperateResult<byte[]> response = WriteHarmonicsCommmand(harmonicChannels, data);
            return response;
        }

        /// <summary>
        /// 【清空谐波】
        /// </summary>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> ClearHarmonics()
        {
            return WriteHarmonicsClearCommmand();
        }

        #endregion 设置谐波参数

        /// <summary>
        /// 【设置有功功率】
        /// </summary>
        /// <param name="channel">0-Pa，1-Pb，2-Pc，3-ΣP</param>
        /// <param name="p">有功功率设定值</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteWattPower(ChannelWattPower channel, float p)
        {
            return WriteWattPowerCommmand(channel, p);
        }

        /// <summary>
        /// 【设置无功功率】
        /// </summary>
        /// <param name="channel">0-Qa，1-Qb，2-Qc，3-ΣQ</param>
        /// <param name="p">无功功率设定值</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteWattLessPower(ChannelWattLessPower channel, float q)
        {
            return WriteWattPowerLessCommmand(channel, q);
        }

        public OperateResult<byte[]> ReadACSourceData()
        {
            OperateResult<byte[]> response = ReadACSourceDataCommmand();
            if (!response.IsSuccess)
            {
                return response;
            }

            //命令执行成功则解析数据
            AnalysisReadACSourceData(response.Content);
            return response;
        }

        #endregion 交流源（表）操作命令       

        #endregion Public Methods



        public OperateResult<byte[]> Calibrate_ClearData()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_DoAC()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_DoACMeter()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_DoDC()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_DoDCMeter()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_Save()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_SwitchACPoint()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_SwitchACRange()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Calibrate_SwitchDCPoint()
        {
            throw new NotImplementedException();
        }





        public OperateResult<byte[]> ReadACSourceStatus()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadDCMeterData()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadDCMeterDataWithTwoCh()
        {
            throw new NotImplementedException();
        }



        public OperateResult<byte[]> ReadDCSourceData()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadElectricityDeviation()
        {
            throw new NotImplementedException();
        }





        public OperateResult<byte[]> SetDCMeterDataWithTwoCh()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> SetDCMeterMesureType()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> SetDCMeterRange()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> SetDCSourceRange()
        {
            throw new NotImplementedException();
        }







        public OperateResult<byte[]> Start()
        {
            throw new NotImplementedException();
        }



        public OperateResult<byte[]> StartDCSource()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> Stop()
        {
            throw new NotImplementedException();
        }



        public OperateResult<byte[]> StopDCSource()
        {
            throw new NotImplementedException();
        }



        public OperateResult<byte[]> WriteDCSourceAmplitude()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> WriteElectricity()
        {
            throw new NotImplementedException();
        }



        public OperateResult<byte[]> WriteHarmonics()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> WritePhase()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> WriteWattlessPower()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> WriteWattPower()
        {
            throw new NotImplementedException();
        }

        #region --------------------------------- private Methods Helper 解析数据------------------

        #region 解析【系统信号】
        /// <summary>
        /// 解析设备信息：并非所有设备都会回复有效信息
        /// </summary>
        private void AnalysisHandshake(byte[] response)
        {
            //byte[] data = response;
            List<byte> responseList = response.ToList();    //可忽略null异常

            //解析设备型号
            int endIndex = responseList.IndexOf(0x00, 6);
            int modelLength = endIndex - 5;    //model字节长度，包含0x00结束符           
            Model = ByteTransform.TransString(response, 6, modelLength, Encoding.ASCII);

            //解析下位机版本号
            byte verA = response[modelLength + 6];
            byte verB = response[modelLength + 7];
            Version = $"V{verA}.{verB}";

            //解析设备编号
            int serialEndIndex = responseList.IndexOf(0x00, 8 + modelLength);
            int serialLength = serialEndIndex - 7 - modelLength;     //设备编号字节长度，包含0x00结束符            
            SN = ByteTransform.TransString(response, 8 + modelLength, serialLength, Encoding.ASCII);

            //解析基本功能激活状态
            byte funcB = response[response.Length - 3];
            // byte funcS = data[data.Length - 2];
            var funb = DK81CommunicationInfo.GetFunctionB(funcB);
            IsACI_Activated = funb[0];   //交流电流源
            IsACU_Activated = funb[0];   //交流电压源
            IsDCU_Activated = funb[2];   //直流电压源功能
            IsDCI_Activated = funb[2];   //直流电流源功能
            IsDCM_Activated = funb[3];   //直流表功能
            IsPQ_Activated = funb[4];    //电能校验功能


            //TODO var funs = DK81CommunicationInfo.GetFunctionS(funcS);    //特殊功能状态解析，暂不处理
        }
        #endregion 

        #region 解析【设备信息】

        /// <summary>
        /// 解析交流源档位，并初始化设备属性
        /// </summary>
        /// <param name="response">下位机回复的档位信息报文</param>
        private void AnalysisReadACSourceRanges(byte[] response)
        {
            _uRangesCount = response[6];
            _uRanges_Asingle = response[7];
            _iRangesCount = response[8];
            _iRanges_Asingle = response[9];
            _iProtectRangesCount = response[10];
            _iProtectRanges_Asingle = response[11];

            if (_uRangesCount > 0)
            {
                float[] uRanges = ByteTransform.TransSingle(response, 12, _uRangesCount);
                _uRanges = uRanges.ToList();
            }

            if (_iRangesCount > 0)
            {
                float[] iRanges = ByteTransform.TransSingle(response, 12 + 4 * _uRangesCount, _iRangesCount);
                _iRanges = iRanges.ToList();
            }

            if (_iProtectRangesCount > 0)
            {
                float[] iProtectRanges = ByteTransform.TransSingle(response, 12 + 4 * _uRangesCount + 4 * _iRangesCount, _iProtectRangesCount);
                _iProtectRanges = iProtectRanges.ToList();
            }
        }

        /// <summary>
        /// 解析直流源档位，并初始化设备属性
        /// </summary>
        /// <param name="response"></param>
        private void AnalysisReadDCSourceRanges(byte[] response)
        {
            if (response.Length > 8)
            {
                _DCURangesCount = response[6];
                _DCIRangesCount = response[7];
                float[] dcuRanges = ByteTransform.TransSingle(response, 8, _DCURangesCount);
                float[] dciRanges = ByteTransform.TransSingle(response, 8 + _DCURangesCount * 4, _DCIRangesCount);
                _DCURanges = dcuRanges.ToList();
                _DCIRanges = dciRanges.ToList();
            }
        }

        /// <summary>
        /// 解析直流表档位，并初始化设备属性
        /// </summary>
        /// <param name="response">经过验证的有效回复数据</param>
        private void AnalysisReadDCMeterRanges(byte[] response)
        {
            if (response.Length > 10)
            {
                //TODO 测试异常是否能在底层被完全捕获，确保response数据有效性
                _DCMeterURangesCount = response[8];
                _DCMeterIRangesCount = response[9];
                float[] dcmURanges = ByteTransform.TransSingle(response, 10, _DCMeterURangesCount);
                float[] dcmIanges = ByteTransform.TransSingle(response, 10 + 4 * _DCMeterURangesCount, _DCMeterIRangesCount);
                _DCMeterURanges = dcmURanges.ToList();
                _DCMeterIRanges = dcmIanges.ToList();
            }
        }
        #endregion

        #region 解析交流源（表）
        private void AnalysisReadACSourceData(byte[] response)
        {
            //byte[] id = new byte[2]{ response[1], response[2] };
            //ID = BitConverter.ToUInt16(id, 0);
            Frequency = ByteTransform.TransSingle(response, 6);
            ACU_Range = response[7];
            ACI_Range = response[10];

            _UA = ByteTransform.TransSingle(response, 16);
            _UB = ByteTransform.TransSingle(response, 20);
            _UC = ByteTransform.TransSingle(response, 24);
            _IA = ByteTransform.TransSingle(response, 28);
            _IB = ByteTransform.TransSingle(response, 32);
            _IC = ByteTransform.TransSingle(response, 36);
            _UaPhase = ByteTransform.TransSingle(response, 40);
            _UbPhase = ByteTransform.TransSingle(response, 44);
            _UcPhase = ByteTransform.TransSingle(response, 48);
            _IaPhase = ByteTransform.TransSingle(response, 52);
            _IbPhase = ByteTransform.TransSingle(response, 56);
            _IcPhase = ByteTransform.TransSingle(response, 60);


        }
        #endregion

        #endregion private Methods Helper 解析数据
    }
}
