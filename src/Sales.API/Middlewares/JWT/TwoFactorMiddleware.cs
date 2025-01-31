namespace Sales.API.Middlewares.JWT
{
    public class TwoFactorMiddleware
    {
        private readonly RequestDelegate _next;

        public TwoFactorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Session.Keys.Contains("2FA_Passed"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Two-Factor Authentication required.");
                return;
            }

            await _next(context);
        }
    }
}
