using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _allowedIps;

        public IpFilterMiddleware(RequestDelegate next)
        {
            _next = next;

            var allowedIpsEnv = Environment.GetEnvironmentVariable("ALLOWED_IPS");
            _allowedIps = string.IsNullOrEmpty(allowedIpsEnv)
                ? Array.Empty<string>()
                : allowedIpsEnv.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            if (remoteIp != null && (remoteIp.IsIPv4MappedToIPv6 || remoteIp.IsIPv6Teredo))
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            var remoteIpString = remoteIp?.ToString();
            var remoteHost = remoteIpString == "127.0.0.1" ? "::1" : remoteIpString;

            Console.WriteLine($"Request from IP: {remoteIpString}, Host: {remoteHost}");

            if (_allowedIps.Contains(remoteIpString) || _allowedIps.Contains(remoteHost))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden");
            }
        }
    }
}
