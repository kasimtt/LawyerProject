using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Net.Mime;
using System.Text.Json;

namespace LawyerProject.API.Extensions
{
    static public class ConfigureExceptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var userName = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Guest";
                        LogContext.PushProperty("UserName", userName);
                        LogContext.PushProperty("ApiPath", context.Request.Path);
                        LogContext.PushProperty("IpAdres", context.Connection.RemoteIpAddress);

                        var problemDetails = new ProblemDetails
                        {
                            Title = "Bir hata meydana geldi!",
                            Status = context.Response.StatusCode,
                            Detail = contextFeature.Error.Message,
                            Instance = context.Request.Path
                        };

                        logger.LogError(JsonSerializer.Serialize(problemDetails));
                        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                    }
                });
            });
        }
    }
}
