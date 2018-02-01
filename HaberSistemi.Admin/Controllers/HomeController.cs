using HaberSistemi.Admin.CustomFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSistemi.Admin.Controllers
{
    public class HomeController : Controller
    {
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}