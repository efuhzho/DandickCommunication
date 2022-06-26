using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Dandick;

namespace DKCommunication.Core
{
    public interface IReadWriteDK
    {
        bool Handshake();

        #region SetSupport
        bool SetDisplayPage(int page);
        #endregion

        #region ReadSupport

        #endregion

        #region WriteSupport

        #endregion
    }
}
