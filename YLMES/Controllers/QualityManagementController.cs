using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YLMES.Controllers
{
    public class QualityManagementController : Controller
    {
        // GET: QualityManagement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QualityReport()
        {
            return View();
        }
    }
}