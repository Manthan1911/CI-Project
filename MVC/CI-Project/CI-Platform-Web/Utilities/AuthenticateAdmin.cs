using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CI_Platform_Web.Utilities
{
    public class Authenticate : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.DisplayName;
            if (!actionName.Contains("Logout"))
            {
                string isAdmin = filterContext.HttpContext.Session.GetString("IsAdmin");
                if (isAdmin == "True")
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Admin" },
                        { "Action", "Index" },
                    });
                }
            }
        }
    }
}
