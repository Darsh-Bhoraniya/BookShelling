using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Build.Framework;
namespace BookSheling.BAL
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("UserId") == null) context.Result = new RedirectResult("~/User/Login");
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["pragma"] = "no-cache";
            base.OnResultExecuting(context);
        }
    }
}
