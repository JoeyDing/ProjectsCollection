using SkypeIntlPortfolio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkypeIntlPortfolio.Controllers
{
    public class FabricOnboardingsStatusController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Fabric Onboardings Overview";
            var currentUri = HttpContext.Request.Url;
            var detailsUri = new Uri(currentUri, "fabriconboardingsdetails");
            var model = new FabricOnboardingsHighLevelViewModel()
            {
                FabricOnboardingsDetailsUrl = detailsUri.ToString(),
            };
            return View(model);
        }
    }
}