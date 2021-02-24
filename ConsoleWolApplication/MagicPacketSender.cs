using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleWolApplication
{
    public class MagicPacketSender
    {
        private readonly string _macAddress;
        private readonly WakeOnLanClient _wakeOnLanClient;

        public MagicPacketSender(string macAddress)
        {
            _macAddress = macAddress;
            _wakeOnLanClient = new WakeOnLanClient();
        }

        public async Task SendAsync()
        {
            var magicPacket = GetMagicPackage();
            await  _wakeOnLanClient.SendAsync(magicPacket.ToArray());
        }

        private IList<byte> GetMagicPackage()
        {
            var magicPacket = new List<byte>();

            var timingChain = GetTimingChain();
            var magicPacketBody = GetMagicPacketBody(_macAddress);

            magicPacket.AddRange(timingChain);
            magicPacket.AddRange(magicPacketBody);

            return magicPacket;
        }
        
        private IList<byte> GetTimingChain()
        {
            const int timingChainSize = 6;
            var result = new List<byte>();

            for (int i = 0; i < timingChainSize; i++)
                result.Add(0xFF);

            return result;
        }

        private IList<byte> GetMagicPacketBody(string macAddress)
        {
            const int macRepeatCount = 16;
            var result = new List<byte>();

            for (int i = 0; i < macRepeatCount; i++)
            {
                var macInBytes = macAddress.Split('-')
                                           .Select(x => byte.Parse(x, NumberStyles.HexNumber));
                result.AddRange(macInBytes);
            }

            return result;
        }
    }
}
