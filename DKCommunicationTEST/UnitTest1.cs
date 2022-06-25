
namespace DKCommunicationTEST
{
    public class UnitTest1
    {
        [Fact]
        public void Handshake()
        {
            DK81CommandBuilder dKCommand = new DK81CommandBuilder();
            dKCommand.CreateHandShake();
        }
    }
}