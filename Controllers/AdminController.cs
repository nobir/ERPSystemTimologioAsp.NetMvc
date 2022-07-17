using ERPSystemTimologio.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPSystemTimologio.EF;
using ERPSystemTimologio.Models;

namespace ERPSystemTimologio.Controllers
{
    [LoggedIn, IsAdmin]
    public class AdminController : Controller
    {
        private readonly TimologioEntities db = new TimologioEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.Branches = this.db.Branches.ToList();
            ViewBag.Regions = this.db.Regions.ToList();
            ViewBag.Permissions = this.db.Permissions.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserCreateAdminModel _user)
        {
            ViewBag.Branches = this.db.Branches.ToList();
            ViewBag.Regions = this.db.Regions.ToList();
            ViewBag.Permissions = this.db.Permissions.ToList();

            if (_user.PermissionIds.Count() > 0)
            {
                _user.PermissionIds.RemoveAll(n => n == 0);
                _user.PermissionIds = _user.PermissionIds.Distinct().ToList();
            }

            if (!ModelState.IsValid)
            {
                return View(_user);
            }

            User user = new User
            {
                Verified = _user.Verified,
                Name = _user.Name,
                Username = _user.Username,
                Email = _user.Email,
                Salary = _user.Salary,
                Password = _user.Password,
                Type = _user.Type,
                HireDate = _user.HireDate
            };

            if (_user.RegionId != null && _user.BranchId == null)
            {
                if(db.Regions.Where(r => r.Id == _user.RegionId).Count() == 0)
                {
                    TempData["error_message"] = "Region not found";
                    return RedirectToAction("CreateUser", "Admin");
                }

                user.RegionId = _user.RegionId;
            }
            else if(_user.BranchId != null)
            {
                if (db.Branches.Where(b => b.Id == _user.BranchId).Count() == 0)
                {
                    TempData["error_message"] = "Branch not found";
                    return RedirectToAction("CreateUser", "Admin");
                }

                user.BranchId = _user.BranchId;
            }

            Address address = new Address
            {
                LocalAddress = _user.LocalAddress,
                PoliceStation = _user.PoliceStation,
                City = _user.City,
                Country = _user.Country,
                ZipCode = _user.ZipCode,
            };

            db.Addresses.Add(address);

            db.SaveChanges();

            user.AddressId = address.Id;

            db.Users.Add(user);

            db.SaveChanges();

            for (var i = 0; i < _user.PermissionIds.Count(); ++i)
            {
                var id = _user.PermissionIds[i];
                var permission = db.Permissions.Where(p => p.Id == id).SingleOrDefault();

                db.Permissions.Attach(permission);
                user.Permissions.Add(permission);
                db.SaveChanges();
            }

            db.SaveChanges();

            TempData["success_message"] = "Successfully created user";

            return RedirectToAction("CreateUser", "Admin");
        }
    }
}