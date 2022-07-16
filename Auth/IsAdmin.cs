using ERPSystemTimologio.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERPSystemTimologio.Auth
{
    public class IsAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User sessionUser = null;

            if(filterContext.HttpContext.Session["user"] != null)
            {
                sessionUser = (User)filterContext.HttpContext.Session["user"];
            }

            if (!(sessionUser != null && sessionUser.Type <= 1))
            {
                filterContext.Controller.TempData["error_message"] = "Your not Admin or System User";

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Dashboard", action = "Index" })
                );

                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }
    }
}