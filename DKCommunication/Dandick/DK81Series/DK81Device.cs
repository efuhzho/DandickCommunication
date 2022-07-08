using DKCommunication.Dandick.DKInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81Command, IDK_BaseInterface<DisplayPage,SystemMode>, IDK_ACSource<WireMode>, IDK_DCMeter, IDK_DCSource, IDK_ElectricityModel, IDK_IOModel                          /* :SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
    {
     
        #region 私有字段
        #region ACSource
        private byte _uRangesCount;
        private byte _iRangesCount;
        private byte _iProtectRangesCount;
        private byte _uRanges_Asingle;
        private byte _iRanges_Asingle;
        private byte _iProtectRanges_Asingle;
        private List<float> _uRanges /*= new List<float> { 380f, 220f, 100f, 57.7f }*/;
        private List<float> _iRanges /*= new List<float> { 20f, 5f, 2f, 1f }*/;
        private List<float> _iProtectRanges/* = new List<float> { 0, 0, 0, 0 }*/;       //TODO 默认值设定？
        #endregion
        #region DCSource
        private byte _DCURangesCount;
        private byte _DCIRangesCount;
        private List<float> _DCURanges;
        private List<float> _DCIRanges;
        #endregion
        #region DCMeter
        private byte _DCMeterURangesCount;
        private byte _DCMeterIRangesCount;
        private List<float> _DCMeterURanges;
        private List<float> _DCMeterIRanges;
        #endregion
        #endregion

        /*****************************************************************************************************/

        #region Constructor   
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
        #endregion

        /*****************************************************************************************************/

        #region Public Properties
        #region Functions
        public bool IsACU_Activated { get; set; } = true;
        public bool IsACI_Activated { get; set; } = true;
        public bool IsDCM_Activated { get; set; } = true;
        public bool IsDCU_Activated { get; set; } = true;
        public bool IsDCI_Activated { get; set; } = true;
        public bool IsPQ_Activated { get; set; } = true;
        public bool IsIO_Activated { get; set; } = true;
        #endregion

        #region ACSource
        public byte ACU_RangesCount => _uRangesCount;
        public byte ACI_RangesCount => _iRangesCount;
        public byte IProtectRangesCount => _iProtectRangesCount;
        public List<float> ACU_RangesList
        {
            get { return _uRanges; }
            set { _uRanges = value; }
        }
        public List<float> ACI_RangesList
        {
            get { return _iRanges; }
            set { _iRanges = value; }
        }
        public List<float> IProtect_RangesList
        {
            get { return _iProtectRanges; }
            set { _iProtectRanges = value; }
        }
        public byte IRanges_Asingle => _iRanges_Asingle;
        public byte IProtectRanges_Asingle => _iProtectRanges_Asingle;
        public byte URanges_Asingle => _uRanges_Asingle;

      
        #endregion
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
        #endregion

        /*****************************************************************************************************/

        #region Public Methods
        /*******************/

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
            return response;
        }
        #endregion 系统信号

        /*******************/

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

        /*******************/

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
            OperateResult<byte[]> response = SetACSourceRangeCommand(ACU_RangesIndex, ACI_RangesIndex, 0);
            return response;
        }

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
        /// <param name="FrequencyAll">设置三相频率值</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteFrequency(float FrequencyAll)
        {
            return WriteFrequency(FrequencyAll, FrequencyAll);
        }

        /// <summary>
        /// 【设置源频率】，返回OK
        /// </summary>
        /// <param name="FrequencyAB">设置AB相频率值</param>
        /// <param name="FrequencyC">设置C相频率值</param>
        /// <returns>带成功标志的操作结果</returns>
        public OperateResult<byte[]> WriteFrequency(float FrequencyAB, float FrequencyC)
        {
            float[] data = new float[] { FrequencyAB, FrequencyAB, FrequencyC };
            return WriteFrequency(data);
        }
       

       public OperateResult<byte[]> SetWireMode(WireMode wireMode)
        {
            OperateResult<byte[]> response = SetWireModeCommmand(wireMode);
            return response;
        }

        
        #endregion 交流源（表）操作命令

        /*******************/

        #endregion Public Methods

        /******************************************************************************************************/


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



        public OperateResult<byte[]> ReadACSourceData()
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




        public OperateResult<byte[]> SetClosedLoop()
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






        public OperateResult<byte[]> SetWireMode()
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

        public OperateResult<byte[]> WriteFrequency()
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
        /******************************************************************************************************/

        #region private Methods Helper 解析
        /******************************************************************************************************************************/
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

            //特殊功能状态解析，暂不处理
            //TODO var funs = DK81CommunicationInfo.GetFunctionS(funcS);    
        }
        #endregion 
        /******************************************************************************************************************************/
        #region 解析【设备信息】

        /// <summary>
        /// 解析交流源档位
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
        /// 解析直流源档位
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
        /// 解析直流表档位
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

        #endregion

        /******************************************************************************************************/

      

    }
}
