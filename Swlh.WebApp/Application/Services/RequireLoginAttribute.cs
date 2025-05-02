namespace Swlh.WebApp.Application.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Swlh.WebApp.Controllers;
public class RequireLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var sessionId = context.HttpContext.Session.GetString("Id");

        if (string.IsNullOrWhiteSpace(sessionId))
        {
            var refererUrl = context.HttpContext.Request.Headers.Referer.ToString();

            // If there's no referer, fallback to a default route
            if (string.IsNullOrWhiteSpace(refererUrl))
            {
                refererUrl = context.HttpContext.Request.PathBase + context.HttpContext.Request.Path;
            }

            // Redirect to Login page with backurl
            context.Result = new RedirectToActionResult
            (
                nameof(AuthenticationController.Login),
                nameof(AuthenticationController)[..^10], 
                new { backurl = refererUrl }
            );

        }

        base.OnActionExecuting(context);
    }
}


