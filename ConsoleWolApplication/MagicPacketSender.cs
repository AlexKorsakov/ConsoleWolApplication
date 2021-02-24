using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace ConsoleWolApplication
{
    public class MagicPacketSender
    {
        private readonly string _macAddress;
        private readonly IPAddress _broadcastIpAddress = new IPAddress(0xffffffff);
        private const int Port = 12287;
        private List<byte> _magicPacket;

        public MagicPacketSender(string macAddress)
        {
            _macAddress = macAddress;
            _magicPacket = new List<byte>();
        }

        public void Send()
        {
            var client = new WakeOnLanClient();
            client.Connect(_broadcastIpAddress, Port);                    
            client.SetClientToBrodcastMode();

            var timingChain = GetTimingChain();
            var magicPacketBody = GetMagicPacketBody(_macAddress);

            _magicPacket.AddRange(timingChain);
            _magicPacket.AddRange(magicPacketBody);

            var returnedValue = client.Send(_magicPacket.ToArray(), _magicPacket.Count);
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
                foreach (var macByte in macAddress.Split('-'))
                {
                    result.Add(byte.Parse(macByte, NumberStyles.HexNumber));
                }
            }

            return result;
        }
    }
}
