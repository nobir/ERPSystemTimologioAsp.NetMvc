using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPSystemTimologio.Auth;
using ERPSystemTimologio.EF;
using ERPSystemTimologio.Models;

namespace ERPSystemTimologio.Controllers
{
    [LoggedIn]
    public class DashboardController : Controller
    {
        private readonly TimologioEntities db = new TimologioEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewProfile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProfilePicture()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProfilePicture(HttpPostedFileBase Avatar)
        {
            var pp = new ProfilePicture
            {
                Filesize = 1024 // 1mb
            };

            var message = pp.Upload(Avatar, ((User)Session["user"]).Username.ToString());

            if(message != null)
            {
                TempData["error_message"] = message;
                return View();
            }

            var userId = ((User)Session["user"]).Id;
            var user = this.db.Users.Where(u => u.Id == userId).SingleOrDefault();

            user.Avatar = user.Username.ToString() + "." + Path.GetExtension(Avatar.FileName).Substring(1);

            this.db.SaveChanges();

            Session["user"] = user;

            TempData["success_message"] = "Successfully Uploaded";

            return View();
        }
    }
}