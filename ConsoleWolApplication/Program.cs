using System;

namespace ConsoleWolApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var nasMac = "FF-FF-FF-FF-FF-FF";
            var magicPacketSender = new MagicPacketSender(nasMac.Replace("-", ""));
            magicPacketSender.Send();
        }
    }
}
