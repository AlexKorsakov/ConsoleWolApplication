using System;
using System.Threading.Tasks;

namespace ConsoleWolApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var nasMac = "FF-FF-FF-FF-FF-FF";
            var magicPacketSender = new MagicPacketSender(nasMac);
            await magicPacketSender.SendAsync();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
