using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Devon.Azure.AI.GPT.Filter
{
    public class AuthorizedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { //Do your logic here
            var cookieValue = filterContext.HttpContext.Request.Cookies["secret"];
            if (cookieValue == "test@ExactGPT007") return;
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Error" }, });
        }
    }
}
