using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using WebAPI.Services;

namespace WebAPI.Middelware
{
    public class TitoPayloadVerifierMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITitoRequestVerifyer _titoRequestVerifyer;
        private readonly ILogger<TitoPayloadVerifierMiddleware> _logger;

        public TitoPayloadVerifierMiddleware(RequestDelegate next, ITitoRequestVerifyer titoRequestVerifyer, ILogger<TitoPayloadVerifierMiddleware> logger)
        {
            _next = next;
            _titoRequestVerifyer = titoRequestVerifyer;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Get tito-signature header
            var signatureHeader = context.Request.Headers["tito-signature"];

            if (signatureHeader.Any() == false)
            {
                RejectAsync(context);
                _logger.LogWarning("Request without tito-signature");
                return;
            }
            var signature = signatureHeader.ToString();

            string payload;

            try
            {
                context.Request.EnableRewind();
                var body = context.Request.Body;
                
                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];

                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);

                payload = Encoding.UTF8.GetString(buffer);

                body.Position = 0;
                context.Request.Body = body;
                
                if (!_titoRequestVerifyer.VerifyPayload(payload, signature))
                {
                    RejectAsync(context);
                    _logger.LogWarning("Request with invalid tito-signature");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TitoPayloadVerifierMiddleware failed");
                throw;
            }

            await _next.Invoke(context);
        }

        private static async void RejectAsync(HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 403;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("tito-signature does not match payload");
        }
    }

    public static class TitoPayloadVerifierMiddlewareExtensions
    {
        public static IApplicationBuilder UseTitoPayloadVerifierMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TitoPayloadVerifierMiddleware>();
        }
    }
}