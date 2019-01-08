using StreamWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          
            documentoDao doc = new documentoDao();
            
            return View(doc);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}