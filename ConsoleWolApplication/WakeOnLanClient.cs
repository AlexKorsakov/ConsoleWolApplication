using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleWolApplication
{
    public class WakeOnLanClient
    {
        private const int Port = 15000;

        public async Task SendAsync(byte[] magicPacket)
        {
            var networkInterfaces = GetNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
                await SendMagicPackageAsync(networkInterface, magicPacket);
        }

        private IList<IPAddress> GetNetworkInterfaces()
        {
            var result = new List<IPAddress>();
            var upAndNotLoopbackNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                                                                                          && n.OperationalStatus == OperationalStatus.Up);

            foreach (var networkInterface in upAndNotLoopbackNetworkInterfaces)
            {
                var iPInterfaceProperties = networkInterface.GetIPProperties();
                var unicastIpAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault(u => u.Address.AddressFamily == AddressFamily.InterNetwork
                                                                                                             && !iPInterfaceProperties.GetIPv4Properties().IsAutomaticPrivateAddressingActive);
                if (unicastIpAddressInformation == null) continue;

                result.Add(unicastIpAddressInformation.Address);
            }

            return result;
        }

        private async Task SendMagicPackageAsync(IPAddress localIpAddress, byte[] magicPacket)
        {
            using var client = new UdpClient(new IPEndPoint(localIpAddress, Port));
            await client.SendAsync(magicPacket, magicPacket.Length, IPAddress.Broadcast.ToString(), Port);
        }
    }
}
