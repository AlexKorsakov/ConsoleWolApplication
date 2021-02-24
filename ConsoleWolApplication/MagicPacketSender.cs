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

        private byte[] GetTimingChain()
        {
            const int timingChainSize = 6;
            var result = new byte[timingChainSize];

            for (int i = 0; i < timingChainSize; i++)
                result[i] = 0xFF;

            return result;
        }

        private IList<byte> GetMagicPacketBody(string macAddress)
        {
            var counter = 0;
            var bytes = new byte[96];
            var result = new List<byte>();

            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    //var oneByteFromMac = byte.Parse(_macAddress.Substring(i, 2), NumberStyles.HexNumber);
                    //result.Add(oneByteFromMac);
                    bytes[counter++] = byte.Parse(_macAddress.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }

            }

            return bytes;
        }
    }
}
