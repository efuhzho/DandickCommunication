using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.BasicFramework;
using DKCommunication.Core;
using DKCommunication.Serial;

namespace DKCommunication
{
    public class DKSource : SerialDeviceBase<RegularByteTransform>
    {
        public DKSource()
        {
            WordLength = 1;
        }


     
    }

}
