using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace BlazorApp.Auth;

// The login is stored in the browser, so the server cannot see it when a page first loads.
// This lets the page load, and AuthorizeRouteView then checks the user inside Blazor.
public class LetBlazorAuthorizeHandler : IAuthorizationMiddlewareResultHandler
{
    public Task HandleAsync(RequestDelegate next, HttpContext context,
        AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        return next(context);
    }
}