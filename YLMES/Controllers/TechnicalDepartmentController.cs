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

        #region  技术部
        //图纸页面
        public ActionResult UploadTheDrawings()
        {
            return View();
        }
        //上传图纸
        [HttpPost]
        public ActionResult UploadDrawings(string hao, HttpPostedFileBase files)
        {
            try
            {
                if (hao == "")
                {
                    return Content("<script>alert('图号不能为空');history.go(-1);</script>");
                }
               else  if (files == null)
                {
                    return Content("<script>alert('请选择文件');history.go(-1);</script>");
                }
                else
                {
                    var fileName1 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(files.FileName));
                    files.SaveAs(fileName1);
                    string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        string name = Session["name"].ToString();
                        SqlParameter[] parms = new SqlParameter[5];
                        parms[0] = new SqlParameter("@Type", "Uploaddrawings");
                        parms[1] = new SqlParameter("@FigureNumber", hao);
                        parms[2] = new SqlParameter("@FolderName", "Upload");
                        parms[3] = new SqlParameter("@FileName", fname);
                        parms[4] = new SqlParameter("@CreatedBy", name);
                        ys.Database.ExecuteSqlCommand("exec UploadTheDrawings  @Type,@FigureNumber,@FolderName,@FileName,@CreatedBy", parms);
                    }
                    return Content("<script>alert('上传成功！');window.parent.location.reload();</script>");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("<script>alert('出现异常');history.go(-1);</script>");
            }

            //try
            //{
            //    if (FilePath == null)
            //    {
            //        return "";
            //    }

            //    if (hao == "")
            //    {
            //        return "";
            //    }
            //    else
            //    {
            //        using (YLMES_newEntities ys = new YLMES_newEntities())
            //        {

            //            string name = Session["name"].ToString();
            //            SqlParameter[] parms = new SqlParameter[5];
            //            parms[0] = new SqlParameter("@Type", "Uploaddrawings");
            //            parms[1] = new SqlParameter("@FigureNumber", hao);
            //            parms[2] = new SqlParameter("@FolderName", "Upload");
            //            parms[3] = new SqlParameter("@FileName", FilePath);
            //            parms[4] = new SqlParameter("@CreatedBy", name);
            //            ys.Database.ExecuteSqlCommand("exec UploadTheDrawings @Type, @FigureNumber,@FolderName,@FileName,@CreatedBy", parms);
            //        }
            //        return "true";
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return "err";
            //}

        }

        public JsonResult Historical(string FigureNumber)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
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
                var list = ys.PM_Figure_history.Where(p => p.FigureNumber.Contains(FigureNumber)).Select(s => new { s.FigureNumber}).Distinct().ToList();            
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


    }
}