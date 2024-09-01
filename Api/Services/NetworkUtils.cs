using System.Net;

namespace Api.Services
{
    public static class NetworkUtils
    {
        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));
            return ip?.ToString() ?? "IP not found";
        }
    }
}
