using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;


namespace YLMES.Controllers
{
    public class TechnicalDepartmentController : Controller
    {
        // GET: TechnicalDepartment
        public ActionResult Index()
        {
            return View();
        }
        //图纸显示页面
        public ActionResult FileManagement()
        {
            return View();
        }

        #region  技术部
        //图纸页面
        public ActionResult UploadTheDrawings()
        {
            return View();
        }
        //上传图纸
        [HttpPost]
        public ActionResult UploadDrawings(string hao, string name, string spec, string partMet, HttpPostedFileBase files)
        {
            if (hao == "")
            {
                return Content("<script>alert('图号不能为空');history.go(-1);</script>");
            }
            else if (files == null)
            {
                return Content("<script>alert('请选择文件');history.go(-1);</script>");
            }
            else
            {
                using (YLMES_newEntities yd = new YLMES_newEntities())
                {
                    var fileName2 = files.FileName;
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@FileName", fileName2);
                    var list = yd.Database.SqlQuery<PM_FiguresCheck_Result>("exec PM_FiguresCheck @FileName", parms).ToList();
                    string s = "";
                    if (list.Count != 0)
                    {
                        s = "0";
                    }
                    if (s == "0")
                    {
                        return Content("<script>alert('该文件名字重复不能上传,请重新修改文件名');history.go(-1);</script>");
                    }
                }
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string names = Session["name"].ToString();
                    var fileName1 = Path.Combine(Request.MapPath("~/pdf"), Path.GetFileName(files.FileName));
                    //files.MoveTo(fileName1, hao + fileName1);
                    files.SaveAs(fileName1);
                    string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
                    string geshi = fname.Substring(fname.LastIndexOf('.') + 1);
                    if (geshi == "PDF")
                    {
                        fname = fname.Substring(0, fname.IndexOf('.'));
                        fname = fname + ".pdf";
                    }
                    Nullable<int> Materid = 0;
                    SqlParameter[] parmse = new SqlParameter[3];
                    parmse[0] = new SqlParameter("@PartNumber", name);
                    parmse[1] = new SqlParameter("@PartSpec", spec);
                    parmse[2] = new SqlParameter("@PartMaterial", partMet);
                    var list = ys.Database.SqlQuery<PM_CheckMaterID_Result>("exec PM_CheckMaterID @PartNumber,@PartSpec,@PartMaterial", parmse).FirstOrDefault();
                    Materid = list.MatertID;
                    SqlParameter[] parmd = new SqlParameter[6];
                    parmd[0] = new SqlParameter("@Type", "Uploaddrawings");
                    parmd[1] = new SqlParameter("@FigureNumber", hao);
                    parmd[2] = new SqlParameter("@FolderName", "Upload");
                    parmd[3] = new SqlParameter("@FileName", files.FileName);
                    parmd[4] = new SqlParameter("@CreatedBy", name);
                    parmd[5] = new SqlParameter("@MarterID", Materid);
                    ys.Database.ExecuteSqlCommand("exec UploadTheDrawings  @Type,@FigureNumber,@FolderName,@FileName,@CreatedBy,@MarterID", parmd);      
                }
                return Content("<script>alert('上传成功!');history.back(-1);</script>");
            }
        }

        public JsonResult Historical(string FigureNumber)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (FigureNumber == null)
                {
                    FigureNumber = "";
                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "Historical");
                parms[1] = new SqlParameter("@FigureNumber", FigureNumber);
                var list = ys.Database.SqlQuery<UploadTheDrawings_Result>("exec UploadTheDrawings @Type,@FigureNumber", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FigureNumberlike(string FigureNumber)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_Figure_history.Where(p => p.FigureNumber.Contains(FigureNumber)).Select(s => new { s.FigureNumber }).Distinct().ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //查看PDF
        public ActionResult CheckPdf(string src)
        {
            ViewData["pdf"] = src;
            return View();
        }
        //显示图纸信息
        public ActionResult CheckFigure(string hao, string PartNumber, string UploadTheName, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (hao == null)
                {
                    hao = "";
                }
                if (UploadTheName == null)
                {
                    UploadTheName = "";
                }
                if (PartNumber == null)
                {
                    PartNumber = "";
                }
                Dictionary<string, Object> hasmap;
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@FigureNumber", hao);
                parms[1] = new SqlParameter("@CreatedBy", UploadTheName);
                parms[2] = new SqlParameter("@PartNumber", PartNumber);
                var list = ys.Database.SqlQuery<PM_CheckFigure_Result>("exec PM_CheckFigure @FigureNumber,@CreatedBy,@PartNumber", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<PM_CheckFigure_Result> pageList = new PageList<PM_CheckFigure_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //修改图纸图号
        public ContentResult EditFigureNumber(string ID, string FigureNumber)
        {
            try
            {
                int i = int.Parse(ID);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    PM_Figure pf = ys.PM_Figure.Where(f => f.ID == i).FirstOrDefault();
                    pf.FigureNumber = FigureNumber;
                    ys.SaveChanges();
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }

        }
        //删除图纸
        public ActionResult DeleteFigureNumber(string ID)
        {
            try
            {
                int i = int.Parse(ID);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    PM_Figure pf = ys.PM_Figure.Where(f => f.ID == i).FirstOrDefault();
                    ys.PM_Figure.Remove(pf);
                    ys.SaveChanges();
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //更新图纸页面
        public ActionResult EditDrawing(string pn, string ps, string pm, string pnu)
        {
            ViewData["FigureNumber"] = pnu;
            ViewData["PartNumber"] = pn;
            ViewData["PartSpec"] = ps;
            ViewData["PartMaterial"] = pm;
            return View();
        }
        #endregion

        #region 查看任务清单

        public ActionResult TaskList()
        {
            return View();
        }
        //获取清单数据
        public ActionResult TaskListJson(string name,string CName, int page, int limit)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (CName == null)
                {
                    CName = "";
                }
                Dictionary<string, Object> hasmap;
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@type", "check");
                parms[1] = new SqlParameter("@name", name);
                parms[2] = new SqlParameter("@cname", CName);
                var list = ys.Database.SqlQuery<PM_CheckDesignBom_Result>("exec PM_CheckDesignBom @type,@name,@cname", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<PM_CheckDesignBom_Result> pageList = new PageList<PM_CheckDesignBom_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


    }
}