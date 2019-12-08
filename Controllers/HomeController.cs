using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zilla.Controllers
{
    public class HomeController : Controller
    {
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

        [Route("/Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "ce.";

            return View();
        }
    }
}