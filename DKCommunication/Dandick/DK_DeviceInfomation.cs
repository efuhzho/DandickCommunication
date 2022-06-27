using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick
{
    /// <summary>
    /// 枚举本通信类库支持的所有【设备型号】
    /// </summary>
    public enum DK_DeviceModel
    {
        [Description("DK-34B1交流采样变送器检定装置")]
        DK_34B1 = 81,

        [Description("DK-34B2")]
        DK_34B2 = 81,

        [Description("DK-34B3")]
        DK_34B3 = 55,

        [Description("DK-34F1")]
        DK_34F1 = 81,

        [Description("DK-PTS1")]
        DK_PTS1 = 55,
    }
    public enum RangeACU : ushort
    {
        [Description("交流电压380V")]
        ACU_380 = 0,

        [Description("交流电压220V")]
        ACU_220 = 1,

        [Description("交流电压100V")]
        ACU_100 = 2,

        [Description("交流电压57.7V")]
        ACU_57 = 3,
    }
}
