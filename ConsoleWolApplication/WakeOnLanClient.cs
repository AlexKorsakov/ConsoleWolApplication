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

        public async Task BypassIpAddressesAndSendMagicPackageAsync(byte[] magicPacket)
        {
            var upAndNotLoopbackNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                                                                                           && n.OperationalStatus == OperationalStatus.Up);

            foreach (var networkInterface in upAndNotLoopbackNetworkInterfaces)
            {
                var iPInterfaceProperties = networkInterface.GetIPProperties();
                var unicastIpAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault(u => u.Address.AddressFamily == AddressFamily.InterNetwork
                                                                                                             && !iPInterfaceProperties.GetIPv4Properties().IsAutomaticPrivateAddressingActive);
                if (unicastIpAddressInformation == null) continue;

                await SendMagicPackageAsync(unicastIpAddressInformation.Address, magicPacket);
            }
        }

        private async Task SendMagicPackageAsync(IPAddress localIpAddress, byte[] magicPacket)
        {
            using var client = new UdpClient(new IPEndPoint(localIpAddress, Port));
            await client.SendAsync(magicPacket, magicPacket.Length, IPAddress.Broadcast.ToString(), Port);
        }
    }
}
