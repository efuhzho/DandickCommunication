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

    /// <summary>
    /// 交流输出档位枚举
    /// </summary>
    [Flags]
    public enum RangeAC : ushort
    {
        [Description("交流电压380V")]
        ACU_380V = 0b_0000_0000,    //0

        [Description("交流电压220V")]
        ACU_220V = 0b_0000_0001,    //1

        [Description("交流电压100V")]
        ACU_100V = 0b_0000_0010,    //2

        [Description("交流电压57.7V")]
        ACU_57V = 0b_0000_0011,     //3

        [Description("交流电流20A")]
        ACI_20A = 0b_0000_0000,     //0

        [Description("交流电流5A")]
        ACI_5A = 0b_0000_0001,      //1

        [Description("交流电流2A")]
        ACI_2A = 0b_0000_0010,      //2

        [Description("交流电流1A")]
        ACI_1A = 0b_0000_0011,      //3
    }
}
