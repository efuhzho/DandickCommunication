using DKCommunication.BasicFramework;
using System;
using DKCommunication.Core;
using DKCommunication.Dandick.DKInterface;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DKCommunication.Core;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81Command, IDK_BaseInterface, IDK_ACSource, IDK_DCMeter, IDK_DCSource, IDK_ElectricityModel, IDK_IOModel                          /* :SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
    {
        #region 私有字段
        private byte _uRangesCount;
        private byte _iRangesCount;
        private byte _iProtectRangesCount;
        private byte _uRanges_Asingle;
        private byte _iRanges_Asingle;
        private byte _iProtectRanges_Asingle;
        private List<float> _uRanges;
        private List<float> _iRanges;
        private List<float> _iProtectRanges;

        private byte _DCURangesCount;
        private byte _DCIRangesCount;
        private List<float> _DCURanges;
        private List<float> _DCIRanges;

        #endregion

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

        public bool IsACU_Activated { get; set; }
        public bool IsACI_Activated { get; set; }
        public bool IsDCM_Activated { get; set; }
        public bool IsDCU_Activated { get; set; }
        public bool IsDCI_Activated { get; set; }
        public bool IsPQ_Activated { get; set; }
        public bool IsIO_Activated { get; set; }

        public byte ACU_RangesCount => _uRangesCount;
        public byte ACI_RangesCount => _iRangesCount;
        public byte IProtectRangesCount => _iProtectRangesCount;
        public List<float> ACU_Ranges => _uRanges;
        public List<float> ACI_Ranges => _iRanges;
        public List<float> IProtectRanges => _iProtectRanges;
        public byte IRanges_Asingle => _iRanges_Asingle;
        public byte IProtectRanges_Asingle => _iProtectRanges_Asingle;
        public byte URanges_Asingle => _uRanges_Asingle;

        public byte DCU_RangesCount => _DCURangesCount;
        public byte DCI_RangesCount => _DCIRangesCount;
        public List<float> DCU_Ranges => _DCURanges;
        public List<float> DCI_Ranges => _DCIRanges;

        #endregion

        #region Base
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

        /// <summary>
        /// 解析【联机指令】的回复报文，并单向初始化设备信息，不对初始化信息做任何判断
        /// 【并非所有设备都会返回准确的设备信息】
        /// </summary>
        /// <returns>包含信息的操作结果</returns>
        public OperateResult<byte[]> Handshake()
        {
            if (HandshakeCommand().IsSuccess)
            {
                AnalysisHandshake(HandshakeCommand().Content);
                ReadACSourceRanges();
            }
            return HandshakeCommand();
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

        public OperateResult<byte[]> ReadDCMeterRangeInfo()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadDCSourceRanges()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadDCSourceData()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadDCSourceRangeInfo()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadElectricityDeviation()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadACSourceRanges()
        {
            if (ReadACSourceRangesCommand().IsSuccess)
            {
                AnalysisReadACSourceRanges(ReadACSourceRangesCommand().Content);
            }
            return ReadACSourceRangesCommand();
        }

        public OperateResult<byte[]> SetACSourceRange()
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

        public OperateResult<byte[]> SetDisplayPage(int page)
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> SetSystemMode(int mode)
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

        public OperateResult<byte[]> StartACSource()
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

        public OperateResult<byte[]> StopACSource()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> StopDCSource()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> WriteACSourceAmplitude()
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
        #endregion

        #region private Methods Helper
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

        /// <summary>
        /// 解析交流源档位
        /// </summary>
        /// <param name="response">下位机回复的档位信息报文</param>
        private void AnalysisReadACSourceRanges(byte[] response)
        {
            try
            {
                _uRangesCount = response[6];
                _uRanges_Asingle = response[7];
                _iRangesCount = response[8];
                _iRanges_Asingle = response[9];
                _iProtectRangesCount = response[10];
                _iProtectRanges_Asingle = response[11];
                float[] uRanges = ByteTransform.TransSingle(response, 12, _uRangesCount);
                float[] iRanges = ByteTransform.TransSingle(response, 12 + 4 * _uRangesCount, _iRangesCount);
                float[] iProtectRanges = ByteTransform.TransSingle(response, 12 + 4 * _uRangesCount + 4 * _iRangesCount, _iProtectRangesCount);
                _uRanges = uRanges.ToList();
                _iRanges = iRanges.ToList();
                _iProtectRanges = iProtectRanges.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AnalysisReadDCSourceRanges(byte[] response)
        {

        }

        #endregion




    }
}
