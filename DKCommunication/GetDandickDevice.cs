using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Dandick;
using DKCommunication.Core;
using DKCommunication.Dandick.DK81Series;

namespace DKCommunication
{
    public class GetDandickDevice<T> where T : Dandick.Base.DK_DeviceBase, new()
    {
        //public GetDandickDevice(DK_DeviceModel model)
        //{
        //    switch ((int)model)
        //    {
        //        case 55:
        //            Device = new T();
        //            break;
        //        case 81:
        //            Device = new T();
        //            break;
        //    }
        //}

        ////public static T Get(DK_DeviceModel model)
        ////{
        ////    switch ((int)model)
        ////    {
        ////        case 55:
        ////           return new DK81Device();
                   
        ////        case 81:
        ////            return new T();                    
        ////    }
        ////    return null;
        ////}
        ////public T Device { get; set; }

        
    }
}
