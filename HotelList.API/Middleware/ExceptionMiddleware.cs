using HotelList.API.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelList.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Logger
                _logger.LogError(ex, $"Something went wrong in the {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType= "application/json";
            HttpStatusCode statuCode = HttpStatusCode.InternalServerError;
            var errorDetails = new ErrorDetails 
            {
                ErrorType = "Failure",
                ErrorMessage= ex.Message,
            };

            switch (ex)
            {
                case NotFoundException notFoundException:
                    statuCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;
                default:
                    break;
            }

            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int) statuCode;
            return context.Response.WriteAsync(response);
        }
    }
}
