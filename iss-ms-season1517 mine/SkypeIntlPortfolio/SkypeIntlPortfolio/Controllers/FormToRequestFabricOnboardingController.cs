using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SkypeIntlPortfolio.Controllers
{
    public class FormToRequestFabricOnboardingController : Controller
    {
        // GET: GetDataFromDB
        public ActionResult Index()
        {
            using (EOLandProductsEntities eolProducts = new EOLandProductsEntities())
            {
                List<string> productsList = new List<string>();
                //get all the products from db
                productsList = (from f in eolProducts.Products
                                select f.Product1).ToList();
                ViewBag.Products = productsList;
            }

            return View();
        }

        [HttpPost]
        public ActionResult GetDataFromForm()
        {
            //get the returned values from the data-entry form
            string productName = Request.Form["productsList"].Replace(" ", "_");
            string epicLabel = Request.Form["epicLabel"].Replace(" ", "_");
            string core_intl_folders_loc = Request.Form["core_ifl"];
            string core_Source_File_path = Request.Form["coresource_fp"];
            string eopermission = Request.Form["checkbox"];
            bool permissionCheckedOrNot = true;
            if (string.IsNullOrEmpty(eopermission))
            {
                permissionCheckedOrNot = false;
            }
            string eol = Request.Form["eol"];
            string expectedWalkingDate = Request.Form["datepicker_walkingDate"];
            string expectedRunningDate = Request.Form["datepicker_runningDate"];

            //make sure all the fields must be filled before creating a new pbi
            string[] fieldsArray = new string[] { productName, core_intl_folders_loc, core_Source_File_path, eol, expectedWalkingDate, expectedRunningDate };
            // See if any elements are not null.
            if (!fieldsArray.Any(item => string.IsNullOrEmpty(item)))
            {
                Utils.CreateAAAPTJira(productName, epicLabel, core_intl_folders_loc, core_Source_File_path, permissionCheckedOrNot, eol, expectedWalkingDate, expectedRunningDate);
                Utils.CreateLYNCFABJira(productName, epicLabel);
                //ViewBag.SuccessMessage = String.Format("You have successfully requested Fabric Onboarding for product {0} (check out all PBIs by using this Jira query: https://jira.skype.net/issues/?jql=cf%5B10241%5D%20%3D%20Fabric_{0}", productName);
                ViewBag.contextualState = "success";
                ViewBag.Message = String.Format(@"You have successfully requested Fabric Onboarding for product {0}", productName);
                ViewBag.JIRAPbiLink = String.Format("https://jiratest.skype.net/issues/?jql=cf%5B10241%5D%20%3D%20Fabric_{0}", productName);
            }
            else
            {
                ViewBag.contextualState = "danger";
                ViewBag.Message = "Please make sure you have filled all the fileds with data";
            }
            return View();
        }
    }
}