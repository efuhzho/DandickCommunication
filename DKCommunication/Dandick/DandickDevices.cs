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
        private OperateResult _dkDevice;
       
        public DandickDevice(DK_DeviceModel deviceModel)
        {
            switch ((int)deviceModel)
            {
                case 55:
                    break;
                case 81:
                    _dkDevice = OperateResult<DK81Device>.CreateSuccessResult(new DK81Device());
                    break;
            }
        }

        public DandickDevice(DK_DeviceModel deviceModel,ushort id)
        {
            switch ((int)deviceModel)
            {
                case 55:
                    break;
                case 81:
                    _dkDevice = OperateResult<DK81Device>.CreateSuccessResult(new DK81Device(id));
                    break;
            }
        }

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
