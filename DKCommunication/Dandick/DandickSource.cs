using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Core;
using DKCommunication.Dandick.DK81Series;
using DKCommunication.Serial;

namespace DKCommunication.Dandick
{
    /// <summary>
    /// 实例化一个设备对象。支持的型号：【DK-34B1】【DK-34B2】【DK-34F1】【DK-56B1】
    /// </summary>
    public class DandickSource
    {
        #region 私有字段
        /// <summary>
        /// 动态设备类型
        /// </summary>
        private readonly dynamic _device;
        #endregion

        //#region 公开属性 ?用于尝试第二种方式：属性赋值再实例化对象
        ///// <summary>
        ///// 设备ID
        ///// </summary>
        //public ushort ID { get; set; }
        ///// <summary>
        ///// 设备型号
        ///// </summary>
        //public DK_DeviceModel DeviceModel { get; set; }
        //#endregion    

        #region Constructor      
        /// <summary>
        /// 根据设备型号创建设备对象【推荐使用】
        /// </summary>
        /// <param name="deviceModel">设备型号</param>
        public DandickSource(DK_DeviceModel deviceModel)    //TODO 尝试采用构造函数先给属性赋值再实例化对象的方式
        {
            switch ((int)deviceModel)
            {
                case 55:
                    _device = new DK81Device();
                    break;
                case 81:
                    _device = new DK81Device();
                    break;
            }
        }
        /// <summary>
        /// 根据型号和设备ID创建设备对象【推荐使用】
        /// </summary>
        /// <param name="deviceModel">设备型号</param>
        /// <param name="id">设备ID</param>
        public DandickSource(DK_DeviceModel deviceModel, ushort id)
        {
            switch ((int)deviceModel)
            {
                case 55:
                    _device = new DK81Device(id);
                    break;
                case 81:
                    _device = new DK81Device(id);
                    break;
            }
        }
        /// <summary>
        /// 根据协议类型创建设备对象【！不推荐使用】【丹迪克内部专用】【当设备型号列表里没有对应的型号时的备用方法】
        /// </summary>
        /// <param name="communicationType">协议类型</param>
        public DandickSource(DKCommunicationType communicationType)
        {
            switch (communicationType)
            {
                case DKCommunicationType.DK55CommunicationType:
                    break;
                case DKCommunicationType.DK81CommunicationType:
                    _device = new DK81Device();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 根据协议类型创建设备对象【！不推荐使用】【丹迪克内部专用】【当设备型号列表里没有对应的型号时的备用方法】
        /// </summary>
        /// <param name="communicationType">协议类型</param>
        /// <param name="id">设备ID</param>
        public DandickSource(DKCommunicationType communicationType, ushort id)
        {
            switch (communicationType)
            {
                case DKCommunicationType.DK55CommunicationType:
                    break;
                case DKCommunicationType.DK81CommunicationType:
                    _device = new DK81Device(id);
                    break;
                default:
                    break;
            }
        }


        public OperateResult<byte[]> Handshake()
        {
            return _device.Handshake();
        }

        public OperateResult<byte[]> SetDisplayPage(DisplayPage page)
        {
            return _device.SetDisplayPage(page);
        }

        public OperateResult<byte[]> SetSystemMode(SystemMode mode)
        {
            return _device.SetSystemMode(mode);
        }
        #endregion

        #region Set / Write Methods

        #endregion

        #region Get / Read Methods

        #endregion



    }
}
