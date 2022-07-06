using DKCommunication.BasicFramework;
using System;
using DKCommunication.Core;
using DKCommunication.Dandick.DKInterface;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device : DK81Command, IDK_BaseInterface, IDK_ACSource, IDK_DCMeter, IDK_DCSource, IDK_ElectricityModel, IDK_IOModel                          /* :SerialDeviceBase<RegularByteTransform>,*//*IReadWriteDK*/
    {
        #region 私有字段


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

        public OperateResult<byte[]> ReadDCRangeInfo()
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
            throw new NotImplementedException();
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
        /// 自动获取设备信息：并非所有设备都会回复有效信息
        /// </summary>
        private void AnalysisHandshake(byte[] response)
        {
            byte[] data = response;
            List<byte> dataList = data.ToList();

            //解析设备型号
            int endIndex = dataList.IndexOf(0x00, 6);
            int modelLength = endIndex - 5;    //model字节长度，包含0x00结束符           
            Model = ByteTransform.TransString(data, 6, modelLength, Encoding.ASCII);

            //解析下位机版本号
            byte verA = data[modelLength + 6];
            byte verB = data[modelLength + 7];
            Version = $"V{verA}.{verB}";

            //解析设备编号
            int serialEndIndex = dataList.IndexOf(0x00, 8 + modelLength);
            int serialLength = serialEndIndex - 7 - modelLength;     //设备编号字节长度，包含0x00结束符            
            SN = ByteTransform.TransString(data, 8 + modelLength, serialLength, Encoding.ASCII);

            //解析基本功能激活状态
            byte funcB = data[data.Length - 3];
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

        private void AnalysisReadACSourceRange()
        {

        }

        public OperateResult<byte[]> ReadACSourceRangeInfo()
        {
            throw new NotImplementedException();
        }
        #endregion




    }
}
