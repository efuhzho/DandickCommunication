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
    /// 实例化一个丹迪克设备对象
    /// </summary>
    public class DandickDevice:IReadWriteDK
    {
        #region Constructor      
        /// <summary>
        /// 根据设备型号创建设备对象【推荐使用】
        /// </summary>
        /// <param name="deviceModel">设备型号</param>
        public DandickDevice(DK_DeviceModel deviceModel)
        {
            switch ((int)deviceModel)
            {
                case 55:
                    break;
                case 81:
                    
                    break;
            }
        }
        /// <summary>
        /// 根据型号和设备ID创建设备对象【推荐使用】
        /// </summary>
        /// <param name="deviceModel">设备型号</param>
        /// <param name="id">设备ID</param>
        public DandickDevice(DK_DeviceModel deviceModel,ushort id)
        {
            switch ((int)deviceModel)
            {
                case 55:
                    break;
                case 81:                    
                    break;
            }
        }
        /// <summary>
        /// 根据协议类型创建设备对象【！不推荐使用】【丹迪克内部专用】【当设备型号列表里没有对应的型号时的备用方法】
        /// </summary>
        /// <param name="communicationType">协议类型</param>
        public DandickDevice(DKCommunicationType communicationType)
        {
            switch (communicationType)
            {
                case DKCommunicationType.DK55CommunicationType:
                    break;
                case DKCommunicationType.DK81CommunicationType:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Set / Write Methods

        #endregion

        #region Get / Read Methods

        #endregion

      
        /// <summary>
        /// 【联机】
        /// </summary>
        /// <returns></returns>
        public bool Handshake()
        {
            
            return true;
        }

        /// <summary>
        /// 【设置显示页面】
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool SetDisplayPage(int page)
        {
            throw new NotImplementedException();
        }      
    }
}
