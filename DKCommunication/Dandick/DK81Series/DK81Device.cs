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
        public ushort ID { get; set; }

        private DK81CommandBuilder _commandBuilder;
        public DK81Device(ushort id)
        {
            ID = id;
            _commandBuilder = new DK81CommandBuilder();
        }

        public void HandShake()
        {
            byte[] buffer = _commandBuilder.CreateHandShake();
        }
    }
}
