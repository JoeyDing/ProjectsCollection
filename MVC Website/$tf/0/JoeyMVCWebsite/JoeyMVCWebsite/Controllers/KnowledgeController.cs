using JoeyMVCWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoeyMVCWebsite.Controllers
{
    public class KnowledgeController : Controller
    {
        // GET: Knoweledge
        public ActionResult Knowledge()
        {
            JoeyDataModel d = new JoeyDataModel { KnoweledgeName = "dasda", KnoweledgeDescription = "fsdfdasda", Comments = "fdfses" };
            return View(d);
        }
    }
}