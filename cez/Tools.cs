using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
// using System.Net.Sockets;

/* Tools used in project */
namespace cez
{
    public static class Tools
    {
        public static bool IsJson(this string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]");
        }

        /* Check network connection */
        public static bool checkNetwork()
        {
            var addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (addr.Length >= 1)
            {
                string myIP = addr[1].ToString();
                IPAddress ip1 = IPAddress.Parse(myIP);
                IPAddress ip2 = IPAddress.Parse(GlobalVar.serverIP);
                return ip1.AddressFamily.Equals(ip2.AddressFamily);
            }
            return false;
        }

        public static IEnumerable<string> GetAddresses()
        {
            return (from ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList where ip.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork) select ip.ToString()).ToList();
        }

        public static bool sameNetwork()
        {
            // string.Join(",", Tools.GetAddresses().ToArray());
            IPAddress ip1 = IPAddress.Parse(getMyIP());
            IPAddress ip2 = IPAddress.Parse(GlobalVar.serverIP);
            return ip1.AddressFamily.Equals(ip2.AddressFamily);
        }

        public static string getMyIP()
        {
            IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(address => address.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork)).First();
            return ip.ToString();
        }
    }
}