using wema_swe.Middlewares;

namespace wema_swe.Extensions
{
    public static class ExceptionExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
