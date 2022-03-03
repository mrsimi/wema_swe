using System.Net;
using wema_swe.DTO.Responses;

namespace wema_swe.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new GenericResponse<string>()
            {
                Data = ex.Message,
                ResponseMessage = "Sorry your Request cannot be completed. It is not you, it is us",
                HttpStatusCode = (int)HttpStatusCode.InternalServerError
            }.ToString());
        }
    }
}
