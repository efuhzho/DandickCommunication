using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Core;
using DKCommunication.Dandick.DK81Series;

namespace DKCommunication.Dandick
{
    /// <summary>
    /// 实例化一个丹迪克设备对象
    /// </summary>
    public class DandickDevice:IReadWriteDK
    {
        private ushort _id;       
       
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

        public DandickDevice(DK_DeviceModel deviceModel,ushort id)
        {

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
