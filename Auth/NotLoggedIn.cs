using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERPSystemTimologio.Auth
{
    public class NotLoggedIn : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool sessionLoggedin = filterContext.HttpContext.Session["loggedin"] != null && filterContext.HttpContext.Session["loggedin"].ToString().Equals("True");
            if (sessionLoggedin == true)
            {
                filterContext.Controller.TempData["error_message"] = "You are alreay logged in";

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Dashboard", action = "Index" })
                );

                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }
    }
}