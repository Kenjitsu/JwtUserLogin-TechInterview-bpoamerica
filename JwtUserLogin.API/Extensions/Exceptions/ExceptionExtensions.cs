using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace JwtUserLogin.API.Extensions.Exceptions;

public static class ExceptionExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionFeature?.Error;

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred during the request.",
                    Detail = exception?.Message,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });
    }
}
