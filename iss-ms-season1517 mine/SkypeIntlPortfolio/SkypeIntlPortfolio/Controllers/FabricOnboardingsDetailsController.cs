using SkypeIntlPortfolio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkypeIntlPortfolio.Controllers
{
    public class FabricOnboardingsDetailsController : Controller
    {
        //
        // GET: /FabricOnboardingsDetails/
        public ActionResult Index(string epicLabel)
        {
            this.ViewBag.Title = epicLabel;
            var model = new FabricOnboardingsDetailsViewModel()
            {
                EpicLabel = epicLabel
            };
            return View(model);
        }
    }
}