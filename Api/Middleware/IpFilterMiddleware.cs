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
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            var remoteHost = remoteIp == "127.0.0.1" ? "::1" : remoteIp;

            if (_allowedIps.Contains(remoteIp) || _allowedIps.Contains(remoteHost))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
        }
    }
}
