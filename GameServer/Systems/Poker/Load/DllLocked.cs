using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class DllLocked
    {
        private static bool _allow;
        public static bool AllowRune
        {
            get { return _allow; }
        }
        private const string Ip = "26.184.241.176";
        private const bool Dell = true;
        private static List<string> LocalIps = new List<string>();
        private static bool ExportingNetworkIps()
        {
            try
            {
                String strHostName = System.Net.Dns.GetHostName();
                System.Net.IPHostEntry iphostentry = System.Net.Dns.GetHostEntry(strHostName);
                foreach (var ip in iphostentry.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray())
                {
                    if (!LocalIps.Contains(ip.ToString()))
                        LocalIps.Add(ip.ToString());
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool VaildPC()
        {
            try
            {
                var download = new System.Net.WebClient().DownloadString("https://pastebin.com/T1VLkvg6");
                bool Ready = download.Contains("26.184.241.176");
                if (!Ready)
                    return false;


                if (!ExportingNetworkIps())
                    return false;
                if (LocalIps.Contains(Ip))
                {
                    _allow = true;
                    return true;
                }
                return false;
            }
            catch
            {
                Console.WriteLine("Can't Loaded Poker Information [2]!.");
            }
            return false;
           
        }
    }
}
