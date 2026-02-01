using Microsoft.AspNetCore.Diagnostics;

namespace Project.Api.Presentation.Middlewares;

public static class ExceptionHandlerMiddleware
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();

                var statusCode = (int)HttpStatusCode.InternalServerError;
                var message = "Internal Server Error";

                if (feature?.Error is not null)
                {
                    if (feature.Error is IBaseException baseEx)
                    {
                        statusCode = baseEx.StatusCode;
                        message = baseEx.Message ?? message;
                    }
                    else
                    {
                        message = feature.Error.Message;
                    }
                }

                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = statusCode,
                    Message = message
                });
            });
        });

        return app;
    }
}
