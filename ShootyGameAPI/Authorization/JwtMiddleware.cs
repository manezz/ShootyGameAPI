using ShootyGameAPI.Services;

namespace ShootyGameAPI.Authorization
{
    public class JwtMiddleware
    {

        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            int? userId = jwtUtils.ValidateJwtToken(token!);
            if (userId != null)
            {
                context.Items["User"] = await userService.FindUserByIdAsync(userId.Value);
            }

            await _next(context);
        }
    }
}
