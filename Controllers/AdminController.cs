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
    //[LoggedIn, IsAdmin]
    public class AdminController : Controller
    {
        private readonly TimologioEntities db = new TimologioEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewPermissions(int? id)
        {
            int perpage = 5;
            int currentpage = id ?? 1;
            var _permissions = this.db.Permissions.ToList();

            var permissions = _permissions.Skip((currentpage - 1) * perpage).Take(perpage).ToList();
            int maxpage = (int)Math.Ceiling(Convert.ToDouble(_permissions.Count()) / Convert.ToDouble(perpage));

            ViewBag.Permissions = permissions;

            if (currentpage < maxpage)
            {
                ViewBag.NextPageUrl = Url.Action("ViewPermissions", new { id = currentpage + 1 });
            }

            if (1 < currentpage)
            {
                ViewBag.PreviousPageUrl = Url.Action("ViewPermissions", new { id = currentpage - 1 });
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreatePermission()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePermission(PermissionCreateAdminModel _permission)
        {
            if(!ModelState.IsValid)
            {
                return View(_permission);
            }

            var permission = new Permission
            {
                Name = _permission.Name.ToString(),
                InvoiceAdd = _permission.InvoiceAdd != null ? 1 : 0,
                InvoiceManage = _permission.InvoiceManage != null ? 1 : 0,
                InventoryManage = _permission.InventoryManage != null ? 1 : 0,
                CategoryManage = _permission.CategoryManage != null ? 1 : 0,
                StationManage = _permission.StationManage != null ? 1 : 0,
                OperationManage = _permission.OperationManage != null ? 1 : 0,
                UserManage = _permission.UserManage != null ? 1 : 0,
                PermissionManage = _permission.PermissionManage != null ? 1 : 0,
            };

            this.db.Permissions.Add(permission);

            this.db.SaveChanges();

            TempData["success_message"] = "Successfully created Permission";

            return RedirectToAction("CreatePermission", "Admin");
        }

        [HttpGet]
        public ActionResult EditPermission(int id)
        {
            //if(id != null)
            //{
            //    TempData["error_message"] = "Invalid Permission Id";
            //    return RedirectToAction("ViewPermissions", "Admin");
            //}

            var permission = db.Permissions.Where(p => p.Id == id).SingleOrDefault();

            if(permission == null)
            {
                TempData["error_message"] = "Permission not found";

                return RedirectToAction("ViewPermissions", "Admin");
            }

            var _permission = new PermissionEditAdminModel
            {
                Id = permission.Id,
                Name = permission.Name,
                InvoiceAdd = permission.InvoiceAdd == 1 ? "InvoiceAdd" : "",
                InvoiceManage = permission.InvoiceManage == 1 ? "InvoiceManage" : "",
                InventoryManage = permission.InventoryManage == 1 ? "InventoryManage" : "",
                CategoryManage = permission.CategoryManage == 1 ? "CategoryManage" : "",
                StationManage = permission.StationManage == 1 ? "StationManage" : "",
                OperationManage = permission.OperationManage == 1 ? "OperationManage" : "",
                UserManage = permission.UserManage == 1 ? "UserManage" : "",
                PermissionManage = permission.PermissionManage == 1 ? "PermissionManage" : "",
            };

            return View(_permission);
        }

        [HttpPost]
        public ActionResult EditPermission(PermissionEditAdminModel _permission, int id)
        {
            //if (id != null)
            //{
            //    TempData["error_message"] = "Invalid Permission Id";
            //    return RedirectToAction("ViewPermissions", "Admin");
            //}

            if (!ModelState.IsValid)
            {
                TempData["error_message"] = "Permission not found";

                return RedirectToAction("EditPermission", "Admin", new { id });
            }

            var permission = this.db.Permissions.Where(p => p.Id == id).SingleOrDefault();

            if (permission == null)
            {
                TempData["error_message"] = "Permission not found";

                return RedirectToAction("ViewPermissions", "Admin");
            }

            permission.Name = _permission.Name;
            permission.InvoiceAdd = _permission.InvoiceAdd != null && _permission.InvoiceAdd.Equals("InvoiceAdd") ? 1 : 0;
            permission.InvoiceManage = _permission.InvoiceManage != null && _permission.InvoiceManage.Equals("InvoiceManage") ? 1 : 0;
            permission.InventoryManage = _permission.InventoryManage != null && _permission.InventoryManage.Equals("InventoryManage") ? 1 : 0;
            permission.CategoryManage = _permission.CategoryManage != null && _permission.CategoryManage.Equals("CategoryManage") ? 1 : 0;
            permission.StationManage = _permission.StationManage != null && _permission.StationManage.Equals("StationManage") ? 1 : 0;
            permission.OperationManage = _permission.OperationManage != null && _permission.OperationManage.Equals("OperationManage") ? 1 : 0;
            permission.UserManage = _permission.UserManage != null && _permission.UserManage.Equals("UserManage") ? 1 : 0;
            permission.PermissionManage = _permission.PermissionManage != null && _permission.PermissionManage.Equals("PermissionManage") ? 1 : 0;

            this.db.SaveChanges();

            TempData["success_message"] = "Successfully updated permission.";

            return RedirectToAction("EditPermission", "Admin", new { id });
        }

        public ActionResult VerifyUser(int id)
        {
            var user = this.db.Users.Where(u => u.Id == id).SingleOrDefault();

            if (user == null)
            {
                TempData["error_message"] = "User not found";
                return RedirectToAction("ViewUnverifiedUsers", "Admin");
            }

            user.Verified = 1;

            this.db.SaveChanges();

            TempData["success_message"] = "Successfully Verified the user";

            return RedirectToAction("ViewUnverifiedUsers", "Admin");
        }

        public ActionResult UnverifyUser(int id)
        {
            var user = this.db.Users.Where(u => u.Id == id).SingleOrDefault();

            if (user == null)
            {
                TempData["error_message"] = "User not found";
                return RedirectToAction("ViewVerifiedUsers", "Admin");
            }

            user.Verified = 0;

            this.db.SaveChanges();

            TempData["success_message"] = "Successfully Unverified the user";

            return RedirectToAction("ViewVerifiedUsers", "Admin");
        }

        [HttpGet]
        public ActionResult ViewUnverifiedUsers(int? id)
        {
            int perpage = 5;
            int currentpage = id ?? 1;
            var _users = this.db.Users.Where(u => u.Verified == 0).ToList();

            var users = _users.Skip((currentpage - 1) * perpage).Take(perpage).ToList();
            int maxpage = (int)Math.Ceiling(Convert.ToDouble(_users.Count()) / Convert.ToDouble(perpage));

            ViewBag.Users = users;

            if (currentpage < maxpage)
            {
                ViewBag.NextPageUrl = Url.Action("ViewUnverifiedUsers", new { id = currentpage + 1 });
            }

            if (1 < currentpage)
            {
                ViewBag.PreviousPageUrl = Url.Action("ViewUnverifiedUsers", new { id = currentpage - 1 });
            }

            return View();
        }

        [HttpPost]
        public ActionResult ViewUnverifiedUsers(string SearchValue, string SearchBy)
        {
            List<User> users = null;

            if (SearchValue.Trim().Length == 0)
            {
                TempData["error_message"] = "Enter search value";
                ViewBag.Users = this.db.Users.Where(u => u.Verified == 0).ToList();
                return RedirectToAction("ViewUnverifiedUsers", "Admin");
            }
            else if (SearchBy == "Id")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Id.ToString().ToLower() == SearchValue.ToString().ToLower()) select user).ToList();
            }
            else if (SearchBy == "Username")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Username.ToString().ToLower() == SearchValue.ToString().ToLower()) select user).ToList();
            }
            else if (SearchBy == "Name")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Email")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Name.ToString().ToLower() == SearchValue.ToString().ToLower()) select user).ToList();
            }
            else if (SearchBy == "LocalAddress")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Address.LocalAddress.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "PoliceStation")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Address.PoliceStation.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "City")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Address.City.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "ZipCode")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Address.ZipCode.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Country")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Address.Country.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Region")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Region != null && user.Region.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Branch")
            {
                users = this.db.Users.Where(u => u.Verified == 0).ToList();
                users = (from user in users where (user.Branch != null && user.Branch.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Permission")
            {
                users = (from user in this.db.Users.Where(u => u.Verified == 0).ToList() where user.Permissions.Any(p => p.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else
            {
                users = this.db.Users.ToList();
            }

            if (users == null || users.Count() == 0)
            {
                TempData["error_message"] = "No users found";
                return RedirectToAction("ViewUnverifiedUsers", "Admin");
            }

            //int perpage = 5;
            //users = users.Skip((int)((id - 1) * perpage)).Take(perpage).ToList();

            ViewBag.Users = users;
            //ViewBag.NextPageUrl = Url.Action("ViewVerifiedUsers", new { id });

            return View();
        }

        [HttpGet]
        public ActionResult ViewVerifiedUsers(int? id)
        {
            int perpage = 5;
            int currentpage = id ?? 1;
            var _users = this.db.Users.Where(u => u.Verified == 1).ToList();

            var users = _users.Skip((currentpage - 1) * perpage).Take(perpage).ToList();
            int maxpage = (int)Math.Ceiling(Convert.ToDouble(_users.Count()) / Convert.ToDouble(perpage));

            ViewBag.Users = users;

            if (currentpage < maxpage)
            {
                ViewBag.NextPageUrl = Url.Action("ViewVerifiedUsers", new { id = currentpage + 1 });
            }

            if (1 < currentpage)
            {
                ViewBag.PreviousPageUrl = Url.Action("ViewVerifiedUsers", new { id = currentpage - 1 });
            }

            return View();
        }

        [HttpPost]
        public ActionResult ViewVerifiedUsers(string SearchValue, string SearchBy)
        {
            List<User> users = null;

            if (SearchValue.Trim().Length == 0)
            {
                TempData["error_message"] = "Enter search value";
                ViewBag.Users = this.db.Users.Where(u => u.Verified == 1).ToList();
                return RedirectToAction("ViewUnverifiedUsers", "Admin");
            }
            else if (SearchBy == "Id")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Id.ToString().ToLower() == SearchValue.ToString().ToLower()) select user).ToList();
            }
            else if (SearchBy == "Username")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Username.ToString().ToLower() == SearchValue.ToString().ToLower()) select user).ToList();
            }
            else if (SearchBy == "Name")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Email")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Name.ToString().ToLower() == SearchValue.ToString().ToLower()) select user).ToList();
            }
            else if (SearchBy == "LocalAddress")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.LocalAddress.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "PoliceStation")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.PoliceStation.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "City")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.City.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "ZipCode")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.ZipCode.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Country")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.Country.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Region")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Region != null && user.Region.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Branch")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Branch != null && user.Branch.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else if (SearchBy == "Permission")
            {
                users = (from user in this.db.Users.Where(u => u.Verified == 1).ToList() where user.Permissions.Any(p => p.Name.ToString().ToLower().Contains(SearchValue.ToString().ToLower())) select user).ToList();
            }
            else
            {
                users = this.db.Users.ToList();
            }

            if (users == null || users.Count() == 0)
            {
                TempData["error_message"] = "No users found";
                return RedirectToAction("ViewVerifiedUsers", "Admin");
            }

            //int perpage = 5;
            //users = users.Skip((int)((id - 1) * perpage)).Take(perpage).ToList();

            ViewBag.Users = users;
            //ViewBag.NextPageUrl = Url.Action("ViewVerifiedUsers", new { id });

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