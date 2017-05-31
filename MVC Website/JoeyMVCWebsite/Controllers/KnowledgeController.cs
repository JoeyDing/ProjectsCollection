using JoeyMVCWebsite.Models;
using JoeyMVCWebsite.Helpers;
using System;
using System.Web.Mvc;

namespace JoeyMVCWebsite.Controllers
{
    //[HandleError]
    [MyErrorHandler]
    public class KnowledgeController : Controller
    {
        // GET: Knoweledge
        public ActionResult Knowledge()
        {
            //this view is knowledge(you can  point to another specified view)
            JoeyDataModel d = new JoeyDataModel { KnowledgeName = "dasda", KnowledgeDescription = "fsdfdasda21", Comments = "fdfses" };
            return View(d);
        }

        public ActionResult Index()
        {
            //this view is Index

            //by default， the view will navigate to the  one with same controller name,if no corresponding view file, then go to the one in route config,like
            //the one  above, you can set index as the default action from routeconfig,this is the convention introdcued by many tutorials
            JoeyDataModel d = new JoeyDataModel { KnowledgeName = "dasda", KnowledgeDescription = "fsdfdasda", Comments = "fdfses" };
            ViewBag.StoredTemperoryValue = "temperaryValue";
            //ViewData.Add("StoredTemperoryValue", "1");
            TempData["TestKey1"] = "hi_001";
            TempData.Keep();

            //throw new Exception("This is unhandled exception");
            return View(d);

            //return View(d);
        }

        //public string Index()
        //{
        //    return "this is index action method of KnowledgeController";
        //}

        public ActionResult Index2()
        {
            JoeyDataModel d = new JoeyDataModel { KnowledgeName = "dasda", KnowledgeDescription = "fsdfdasda", Comments = "fdfses" };
            //return View("Knowledge", d);
            return Content("das");
        }

        public ActionResult Index3()
        {
            JoeyDataModel d = new JoeyDataModel { KnowledgeName = "dasda", KnowledgeDescription = "fsdfdasda", Comments = "fdfses" };
            return Content("das");
        }

        //url must includes id
        //public ActionResult Edit(int id)
        //{
        //    return RedirectToAction("Knowledge");
        //}

        //url must includes id
        public ActionResult Edit11()
        {
            return RedirectToAction("Knowledge");
        }

        //[HttpPost],with this attribute
        //http://localhost:30434/Knowledge/Edit/2 will notbe navigated

        //http://localhost:30434/Knowledge/Edit/1 works
        [HttpGet]
        public ActionResult Edit(string name)
        {
            //display on th control
            //JoeyDataModel d = new JoeyDataModel { KnowledgeName = "JD" };
            JoeyDataModel d = new JoeyDataModel { KnowledgeName = name };
            var getValueFromTempData = TempData["TestKey1"];
            //when the navagation back to index from edit page ,in irder to avoid the tempdat lost we can use .keep method
            TempData.Keep();
            return View(d);
        }

        [HttpPost]
        public ActionResult Edit(JoeyDataModel jmodel)
        {
            //set new value to db/control
            JoeyDataModel d = new JoeyDataModel { KnowledgeName = jmodel.KnowledgeName, Comments = jmodel.Comments };
            return View(d);
        }
    }
}