using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace ConsoleWolApplication
{
    public class MagicPacketSender
    {
        private readonly string _macAddress;

        public MagicPacketSender(string macAddress)
        {
            _macAddress = macAddress;
        }

        public void Send()
        {
            var client = new WakeOnLanClient();
            client.Connect(new IPAddress(0xffffffff),  //255.255.255.255  i.e broadcast
                           0x2fff);                    // port=12287 let's use this one 

            client.SetClientToBrodcastMode();

            //set sending bites
            var counter = 0;
            //buffer to be send
            var bytes = new byte[1024];   // more than enough :-)
            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;

            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] = byte.Parse(_macAddress.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
        }

        private byte[] GetTimingChain()
        {
            var result = new byte[12];
            for (int i = 0; i < 6; i++)
                result[i++] = 0xFF;

            return result;
        }
    }
}
