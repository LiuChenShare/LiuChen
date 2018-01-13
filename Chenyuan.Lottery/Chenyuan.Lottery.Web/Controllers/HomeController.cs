using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chenyuan.Lottery.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authentication]
        public ActionResult Index()
        {
            return View();
        }

        [Authentication]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authentication]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}