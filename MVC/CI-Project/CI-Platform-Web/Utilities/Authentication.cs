using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Utilities
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var arguments = filterContext.ActionArguments;
            if (filterContext.HttpContext.Session.GetString("adminEmail") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    { "Controller", "Home" },
                    { "Action", "Index" },
                });
            }
        }
    }
}
