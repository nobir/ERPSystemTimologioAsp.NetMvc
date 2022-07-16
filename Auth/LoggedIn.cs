using ERPSystemTimologio.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERPSystemTimologio.Auth
{
    public class LoggedIn : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool sessionLoggedin = filterContext.HttpContext.Session["loggedin"] != null && filterContext.HttpContext.Session["loggedin"].ToString().Equals("True");
            if (sessionLoggedin != true)
            {
                filterContext.Controller.TempData["error_message"] = "You need to login first";

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "Login" })
                );

                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }
    }
}