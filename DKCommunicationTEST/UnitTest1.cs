
namespace DKCommunicationTEST
{
    public class UnitTest1
    {
        DK81CommandBuilder Command = new();
        [Fact]
        public void Handshake()
        {          
            Command.CreateHandShake();
        }
    }
}