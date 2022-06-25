
namespace DKCommunicationTEST
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            DKCommandBuilder builder = new DKCommandBuilder();
           byte[] result= builder.CreateSystemMode(SystemMode.ModeStandardSource);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }
    }
}