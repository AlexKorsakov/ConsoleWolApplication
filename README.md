# ConsoleWolApplication
Console application to send magic packet over Ethernet to implement Wake-On-LAN.
Consist of two modules: 
* MagicPacketSender - builds a magic package from a mac address string and passes it to WakeOnLanClient
* WakeOnLanClient - bypasses network interfaces and broadcasts a magic packet on the network
