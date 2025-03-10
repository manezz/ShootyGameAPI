using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;
        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? Array.Empty<Role>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            //login returns "null" - should return a userlogin when authorized
            UserResponse user = (UserResponse)context.HttpContext.Items["User"];
            if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                //outcommented as workaround untill fixed
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
