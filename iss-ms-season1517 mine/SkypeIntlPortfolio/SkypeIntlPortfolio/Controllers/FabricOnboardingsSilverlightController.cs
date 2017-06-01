using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkypeIntlPortfolio.Controllers
{
    public class FabricOnboardingsSilverlightController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Silverlight";

            return View();
        }
    }
}