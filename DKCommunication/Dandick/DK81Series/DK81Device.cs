using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCommunication.Core;
using DKCommunication.Serial;
using DKCommunication.Dandick.Command;

namespace DKCommunication.Dandick.DK81Series
{
    public class DK81Device
    {
        private readonly DK81CommandBuilder _commandBuilder;

        public DK81Device()
        {
            _commandBuilder = new DK81CommandBuilder();
        }
        public DK81Device(ushort id)
        {

            _commandBuilder = new DK81CommandBuilder(id);
        }

        public byte[] HandShake()//TODO 用operateResult
        {
            return _commandBuilder.CreateHandShake();           
        }
    }
}
