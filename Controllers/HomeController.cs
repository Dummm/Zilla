using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zilla.Models;

namespace Zilla.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("About")]
        public ActionResult About()
        {
            ViewBag.Message = "mhm da?";

            return View();
        }

        [Route("Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "acasa la billy gates.";

            return View();
        }

        [Authorize(Roles = "Administrator, User")]
        [Route("Dashboard")]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "deshing.";
            ApplicationUser currentUser = db.Users.Find(HttpContext.User.Identity.GetUserId());

            return View(currentUser);
        }
    }
}
