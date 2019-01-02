using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class IncomingController : Controller
    {
        #region 来料检查
        //来料检查页面
        public ActionResult IncomingCheck()
        {
            return View();
        }
        //来料检查信息显示
        public JsonResult IncomingChecks(string pono,int page,int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Dictionary<string, Object> hasmap;
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@PONOID", "");
                parms[1] = new SqlParameter("@PONO", pono);
                var list = ys.Database.SqlQuery<RawWHIQCList_Result>("exec RawWHIQCList @PONOID,@PONO", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<RawWHIQCList_Result> pageList = new PageList<RawWHIQCList_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //显示采购订单号
        public JsonResult IncomingName(string Order)
        {
            if (Order != "")
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var ss1 = ys.PM_PONO.Where(p => p.PONO.Contains(Order)).Select(s => new { s.PONO }).ToList().Distinct();
                    //var list = ys.Database.SqlQuery<PM_WorkOrder>(" SELECT *  FROM PM_WorkOrder ").ToList();
                    return Json(ss1, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        #endregion
    }
}