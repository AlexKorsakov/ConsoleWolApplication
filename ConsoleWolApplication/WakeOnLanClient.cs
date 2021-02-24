using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ConsoleWolApplication
{
    public class WakeOnLanClient : UdpClient
    {
        public WakeOnLanClient() : base()
        {

        }

        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.Broadcast, 0);
        }
    }
}
