using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class StatisticalController : Controller
    {
        // GET: Statistical
        string connString = "Data Source=192.168.1.251;Initial Catalog=KQXT;User ID=admin;Password=admin123";
        public ActionResult Department()
        {
            return View();
        }
        #region 部门人员工作状况
        public JsonResult DepartmentName()
        {
            List<string> list = new List<string>();
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand Mycommand = null;
            SqlDataReader dr = null;
            conn.Open();
            string sqlStr = "select distinct DepartMent  from kqxt.dbo.Employee where StatusID =1";
            Mycommand = new SqlCommand(sqlStr, conn);
            dr = Mycommand.ExecuteReader();
            list.Add("全部");
            while (dr.Read())
            {
                list.Add(dr[0].ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Departmentlist(string Name, string department, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@owner", Name);
                parms[1] = new SqlParameter("@Depart", department);
                var list = ys.Database.SqlQuery<Statistics_Departmentlist_Result>(" exec [Statistics_Departmentlist]@owner,@Depart", parms).ToList();
                PageList<Statistics_Departmentlist_Result> pageList = new PageList<Statistics_Departmentlist_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 任务统计报表
        public ActionResult Report()
        {
            return View();
        }
        public JsonResult ContractNumber()
        {
            List<string> list = new List<string>();
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand Mycommand = null;
            SqlDataReader dr = null;
            conn.Open();
            string sqlStr = "select ContractNumber  from dbo.C_Contract";
            Mycommand = new SqlCommand(sqlStr, conn);
            dr = Mycommand.ExecuteReader();
            list.Add("全部");
            while (dr.Read())
            {
                list.Add(dr[0].ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Reportlist(string ProjectName, string Name, string IsOverDue, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@ProjectName", ProjectName);
                parms[1] = new SqlParameter("@owner", Name);
                parms[2] = new SqlParameter("@Overduday", IsOverDue);
                var list = ys.Database.SqlQuery<Statistics_TaskAlllist_Result>(" exec [Statistics_TaskAlllist] @ProjectName,@owner,@Overduday", parms).ToList();
                PageList<Statistics_TaskAlllist_Result> pageList = new PageList<Statistics_TaskAlllist_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Reportdepartment()
        {
            List<string> list = new List<string>();
            Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand Mycommand = null;
            SqlDataReader dr = null;
            conn.Open();
            string sqlStr = " SELECT distinct [部门] FROM [YLMES_TEST].[dbo].[toji]";
            Mycommand = new SqlCommand(sqlStr, conn);
            dr = Mycommand.ExecuteReader();
            while (dr.Read())
            {
                list.Add(dr[0].ToString());


            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Reportdepartmentlist(string department)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@department", department);
                var list = ys.Database.SqlQuery<Statisticschar_Departmentlist_Result>(" exec [Statisticschar_Departmentlist] @department", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 个人成绩
        public ActionResult Scroe()
        {
            return View();
        }
        public JsonResult Scroelist(string Name, string StartTime, string EndTime, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@owner", Name);
                parms[1] = new SqlParameter("@startTime", StartTime);
                parms[2] = new SqlParameter("@EndTime", EndTime);
                var list = ys.Database.SqlQuery<Statistics_Scroelist_Result>("exec Statistics_Scroelist @owner,@startTime,@EndTime", parms).ToList();
                PageList<Statistics_Scroelist_Result> pageList = new PageList<Statistics_Scroelist_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Scroechartlist()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@owner", "");
                parms[1] = new SqlParameter("@startTime", "");
                parms[2] = new SqlParameter("@EndTime", "");
                var list = ys.Database.SqlQuery<Statistics_Scroelist_Result>(" exec [Statistics_Scroelist] @owner,@startTime,@EndTime", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //个人成绩统计图
        public JsonResult Scroechart(string Name)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Name", Name);
                var list = ys.Database.SqlQuery<Statistics_Scroechart_Result>(" exec [Statistics_Scroechart] @Name", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}