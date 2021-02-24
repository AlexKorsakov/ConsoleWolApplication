using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;

namespace ConsoleWolApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                          .WithParsed(o =>
            {
                if (IsMacAddressValid(o.MacAddress))
                {
                    Console.WriteLine("MacAddress is valid. Try to send magic paket...");
                    var magicPacketSender = new MagicPacketSender(o.MacAddress);
                    magicPacketSender.SendAsync().Wait();
                    Console.WriteLine("Done!");
                }
                else
                {
                    Console.WriteLine("Check MAC address validity!");
                }
            });

            Console.ReadKey();
        }

        private static bool IsMacAddressValid(string mac)
        {
            var macRegularExpression = "^[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}$";

            if (string.IsNullOrEmpty(mac)) return false;

            var regex = new Regex(macRegularExpression);
            return regex.IsMatch(mac);
        }
    }
}
