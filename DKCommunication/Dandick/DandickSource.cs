﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DKCommunication.Core;
//using DKCommunication.Dandick.DK81Series;
//using DKCommunication.Serial;
//using DKCommunication.Dandick.Base;
//using System.Reflection;
//using System.Runtime.InteropServices.ComTypes;

//namespace DKCommunication.Dandick
//{
//    /// <summary>
//    /// 实例化一个设备对象。支持的型号：【DK-34B1】【DK-34B2】【DK-34F1】【DK-56B1】
//    /// </summary>
//    public class DandickSource : IReadWriteDK
//    {
//        #region 私有字段
//        /// <summary>
//        /// 动态设备类型
//        /// </summary>
//        private readonly dynamic _device;

//        void innit()
//        {
//            SN = _device.SerialNumber;
//            ID = _device.ID;
//            Model = _device.Model;
//            Version = _device.Version;
//            SN = _device.SerialNumber;
//            IsACU_Activated = _device.IsACU_Activated;
//            IsACI_Activated = _device.IsACI_Activated;
//            IsDCU_Activated = _device.IsDCU_Activated;
//            IsDCI_Activated = _device.IsDCI_Activated;
//            IsACM_Activated = _device.IsACM_Activated;
//            IsDCM_Activated = _device.IsDCM_Activated;
//            IsPQ_Activated = _device.IsPQ_Activated;
//            IsIO_Activated = _device.IsIO_Activated;
//        }
//        public ushort ID { get; set; }
//        public string Model { get; set; }
//        public string Version { get; set; }
//        public string SN { get; set; }
//        public bool IsACU_Activated { get; set; }
//        public bool IsACI_Activated { get; set; }
//        public bool IsDCU_Activated { get; set; }
//        public bool IsDCI_Activated { get; set; }
//        public bool IsACM_Activated { get; set; }
//        public bool IsDCM_Activated { get; set; }
//        public bool IsPQ_Activated { get; set; }
//        public bool IsIO_Activated { get; set; }

//        //private DK_Model DK;
//        #endregion

//        #region Constructor      
//        /// <summary>
//        /// 根据设备型号创建设备对象【推荐使用】
//        /// </summary>
//        /// <param name="deviceModel">设备型号</param>
//        public DandickSource(DK_DeviceModel deviceModel)    //TODO 尝试采用构造函数先给属性赋值再实例化对象的方式
//        {
//            switch ((int)deviceModel)
//            {
//                case 55:
//                    break;
//                case 81:
//                    _device = new DK81Device();
//                    break;
//            }
//        }
//        /// <summary>
//        /// 根据型号和设备ID创建设备对象【推荐使用】
//        /// </summary>
//        /// <param name="deviceModel">设备型号</param>
//        /// <param name="id">设备ID</param>
//        public DandickSource(DK_DeviceModel deviceModel, ushort id)
//        {
//            switch ((int)deviceModel)
//            {
//                case 55:
//                    _device = new DK81Device(id);
//                    break;
//                case 81:
//                    _device = new DK81Device(id);
//                    break;
//            }
//        }
//        /// <summary>
//        /// 根据协议类型创建设备对象【！不推荐使用】【丹迪克内部专用】【当设备型号列表里没有对应的型号时的备用方法】
//        /// </summary>
//        /// <param name="communicationType">协议类型</param>
//        public DandickSource(DKCommunicationType communicationType)
//        {
//            switch (communicationType)
//            {
//                case DKCommunicationType.DK55CommunicationType:
//                    break;
//                case DKCommunicationType.DK81CommunicationType:
//                    _device = new DK81Device();
//                    break;
//                default:
//                    break;
//            }
//        }
//        /// <summary>
//        /// 根据协议类型创建设备对象【！不推荐使用】【丹迪克内部专用】【当设备型号列表里没有对应的型号时的备用方法】
//        /// </summary>
//        /// <param name="communicationType">协议类型</param>
//        /// <param name="id">设备ID</param>
//        public DandickSource(DKCommunicationType communicationType, ushort id)
//        {
//            switch (communicationType)
//            {
//                case DKCommunicationType.DK55CommunicationType:
//                    break;
//                case DKCommunicationType.DK81CommunicationType:
//                    _device = new DK81Device(id);
//                    break;
//                default:
//                    break;
//            }
//        }

//        public OperateResult<byte[]> SetDisplayPage(DisplayPage page)
//        {
//            throw new NotImplementedException();
//        }

//        public OperateResult<byte[]> SetSystemMode(SystemMode mode)
//        {
//            throw new NotImplementedException();
//        }

//        public OperateResult<byte[]> Stop()
//        {
//            throw new NotImplementedException();
//        }

//        public OperateResult<byte[]> Start()
//        {
//            throw new NotImplementedException();
//        }

//        public OperateResult<byte[]> Handshake()
//        {
//            throw new NotImplementedException();
//        }

//        public OperateResult<byte[]> SetDisplayPage(int page)
//        {
//            throw new NotImplementedException();
//        }

//        public OperateResult<byte[]> SetSystemMode(int mode)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region Set / Write Methods

//        #endregion

//        #region Get / Read Methods

//        #endregion



//    }
//}
