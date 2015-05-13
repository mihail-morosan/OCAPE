using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebFrontend.Controllers
{
    public class RolesController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult RoleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(string RoleName)
        {

            Roles.CreateRole(Request.Form["RoleName"]);
            ViewBag.ResultMessage = "Role created successfully !";

            return RedirectToAction("RoleCreate", "Roles");
        }
    }
}