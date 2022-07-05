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

        public bool IsACU_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsACI_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDCM_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDCU_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDCI_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsPQ_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsIO_Activated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
        /// 解析回复报文并返回解析数据
        /// </summary>
        /// <returns>解析的数据</returns>
        public OperateResult<byte[]> Handshake()
        {
            if (HandshakeCommand().IsSuccess)
            {
                AnalysisHandshake();
            }
            return HandshakeCommand();
        }

        public OperateResult<byte[]> ReadACSourceData()
        {
            throw new NotImplementedException();
        }

        public OperateResult<byte[]> ReadACSourceRangeInfo()
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

        public OperateResult<byte[]> ReadRangeInfo()
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
        /// 
        /// </summary>
        private void AnalysisHandshake(OperateResult<byte[]> response)
        {           
            if (response.IsSuccess)
            {
                byte[] data = response.Content;
                List<byte> dataList = data.ToList();
                int modelLength = dataList.IndexOf(0x00, 6) - 5;    //model字节长度，包含0x00结束符
                int serialLength = dataList.IndexOf(0x00, 8 + modelLength);     //设备编号字节长度，包含0x00结束符

                byte[] model = new byte[modelLength - 1];
                Array.Copy(data, 6, model, 0, modelLength - 1);

                byte verA = data[modelLength + 6];
                byte verB = data[modelLength + 7];

                byte[] serial = new byte[serialLength - 1];
                Array.Copy(data, 8 + modelLength, serial, 0, serialLength - 1);

                byte funcB = data[data.Length - 3];
                byte funcS = data[data.Length - 2];
                var funb = DK81CommunicationInfo.GetFunctionsInfo(funcB);
               //TODO var funs = DK81CommunicationInfo.GetFunctionsInfo(funcS);

                Model = ByteTransform.TransString(data, 6, 8, Encoding.ASCII);
                SN = Encoding.ASCII.GetString(serial);
                Version = $"V+{verA}+.+{verB}";
                IsDCU_Activated = IsDCI_Activated = funb[0] == 1 ? true : false;    //直流源
                IsDCM_Activated = funb[1] == 1 ? true : false;  //直流表
                IsPQ_Activated = funb[2] == 1 ? true : false;   //电能校验
            }         
        }
        #endregion




    }
}
