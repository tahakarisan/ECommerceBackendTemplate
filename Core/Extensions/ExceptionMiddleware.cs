using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }
        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            string message = e.ToString();
            IEnumerable<ValidationFailure> errors;

            if (e.GetType() == typeof(UnauthorizedAccessException))
            {
                //Refresh Token
                httpContext.Response.StatusCode = 401;
                return httpContext.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = 401,
                    Message = "Yetkiniz Yoktur.",
                }.ToString());
            }

            else if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 422;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    StatusCode = 422,
                    Message = message,
                    Errors = errors
                }.ToString());
            }

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
