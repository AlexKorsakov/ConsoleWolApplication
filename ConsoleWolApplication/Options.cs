using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace ConsoleWolApplication
{
    public class Options
    {
        [Option('m', "mac", Required = true, HelpText = "Set the MAC address of the device to be enabled by WOL. Example: 'FF-FF-FF-FF-FF-FF'")]
        public string MacAddress{ get; set; }
    }
}
