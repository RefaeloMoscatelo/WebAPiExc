using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPiExc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PersonManager pm = new PersonManager();
            var list = pm.GetAll();

            return View(list);
        }
    }
}
