using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCommunication.Dandick.DKInterface
{
    interface IDK_IOModel
    {
        /// <summary>
        /// 指示是否具有开关量模块
        /// </summary>
        bool IsIO_Activated { get; set; }
    }
}
