using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_ElectricityModel
    {
        /// <summary>
        /// 指示是否具有电能测量模块
        /// </summary>
        bool IsPQ_Activated { get; set; }

        /// <summary>
        /// 读电能误差
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> ReadElectricityDeviation( );

        /// <summary>
        /// 设置电能校验参数
        /// </summary>
        /// <returns></returns>
        OperateResult<byte[]> WriteElectricity( );
    }
}
