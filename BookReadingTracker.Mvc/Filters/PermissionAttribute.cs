using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookReadingTracker.Mvc.Filters;

public class PermissionAttribute : TypeFilterAttribute
{
    public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
    {
        Arguments = [permission];
    }
}

public class PermissionFilter : IAuthorizationFilter
{
    private readonly string _permission;

    public PermissionFilter(string permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        var hasPermission = context.HttpContext.User.Claims
            .Where(c => c.Type == "permission")
            .SelectMany(c => c.Value.Split(','))
            .Any(p => p.Trim() == _permission);

        if (!hasPermission)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
        }
    }
}
