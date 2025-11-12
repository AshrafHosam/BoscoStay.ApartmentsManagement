using Application.Response;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericException(context, ex);
            }
        }

        private async Task HandleValidationException(HttpContext context, ValidationException ex)
        {
            var result = ex.Errors.Select(a => a.ErrorMessage).ToList();

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }

        private async Task HandleGenericException(HttpContext context, Exception ex)
        {
#if DEBUG
            var result = new BaseResponse<Exception>
            {
                Data = ex,
                Errors = new List<string>
                {
                    ex.Message,
                    ex.InnerException?.Message ?? string.Empty,
                    ex.StackTrace ?? string.Empty
                }
            };

#else
            var result = new BaseResponse<Exception>
            {
                Errors = new List<string>
                {
                    ex.Message,
                }
            };
#endif

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
