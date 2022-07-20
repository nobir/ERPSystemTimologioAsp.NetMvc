using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPSystemTimologio.Auth;

namespace ERPSystemTimologio.Controllers
{
    [LoggedIn]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewProfile()
        {
            return View();
        }
    }
}