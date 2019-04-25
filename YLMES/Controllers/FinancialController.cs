using iTextSharp.text;
using iTextSharp.text.pdf;
using PrintLib.Printers.Zebra;
using Spire.Pdf;
using Spire.Pdf.Annotations;
using Spire.Pdf.Annotations.Appearance;
using Spire.Pdf.Bookmarks;
using Spire.Pdf.Graphics;
using Spire.Xls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;
using PdfAppearance = Spire.Pdf.Annotations.Appearance.PdfAppearance;
using PdfDocument = Spire.Pdf.PdfDocument;


//using ZXing.Common;  //QRcode操作库


namespace YLMES.Controllers
{
    public class FinancialController : Controller
    {
        // GET: Financial
        //采购记账页面
        //凭证审核
        public void PzAudit(int PzID) {
            using (YLMES_newEntities ys=new YLMES_newEntities ()) {
              FI_Accounting_pz list= ys.FI_Accounting_pz.Where(p=>p.编号==PzID).FirstOrDefault();
                list.Status = 1;
                ys.SaveChanges();
            }
        }
        public ActionResult CAccounting() {
            return View();
        }
        public ActionResult Make() {
            return View();
        }
        //获取数据
        public ActionResult CheckMake() {
            using (YLMES_newEntities ys=new YLMES_newEntities ()) {
                Dictionary<string, object> map = new Dictionary<string, object>()
                {
                    {"Make",ys.CheckMake().ToList() },
                    {"Hit",ys.HitList().ToList() }
                };
                return Json(map, JsonRequestBehavior.AllowGet);
            }
        }
        //获取数据2
        public ActionResult CheckData()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Dictionary<string, object> map = new Dictionary<string, object>()
                {
                    {"PzMake",ys.CheckPzMake().ToList() } ,
                    {"Pz",ys.CheckPz().ToList() },
                    {"Hit",ys.HitList().ToList() }
                };
                return Json(map, JsonRequestBehavior.AllowGet);
            }
        }
        public void UpMake(string type,string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                ys.UpMake(type, id,Session["name"].ToString());
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult SubjectType()
        {

            return View();
        }
        public ActionResult Accounting()
        {

            return View();
        }
        public ActionResult Dept()
        {

            return View();
        }
        public ActionResult Laiming()
        {
            return View();
        }
        public ActionResult Laimingsh()
        {
            return View();
        }
        public ActionResult Voucher()
        {
            return View();
        }
        public ActionResult ZhiFu()
        {
            return View();
        }
        public ActionResult Vouchertwo()
        {
            return View();
        }
        public ActionResult Laimingqr()
        {
            return View();
        }

        #region //搜索科目类别
        public ActionResult SeSubjectTypeList(string Name, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", Name);
                var list = ys.Database.SqlQuery<SP_PM_SubjectCategory_Result>("exec SP_PM_SubjectCategory @Type,@line", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_SubjectCategory_Result> pageList = new PageList<SP_PM_SubjectCategory_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 查询科目类别名称
        public ActionResult SeSubjectTypeLists()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                return Json(ys.Database.SqlQuery<SP_PM_SubjectCategory_Result>("exec SP_PM_SubjectCategory 'check'").ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region//搜索科目
        public ActionResult SeAccountingList(string Name, int page, int limit)
        {
            if (Name == null)
            {
                Name = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", Name);
                var list = ys.Database.SqlQuery<SP_PM_Caption_Result>("exec SP_PM_Caption @Type,@line", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_Caption_Result> pageList = new PageList<SP_PM_Caption_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region//搜索科目名称
        public ActionResult SeAccountingListpz()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    return Json(ys.Database.SqlQuery<SP_PM_Caption_Result>("exec SP_PM_Caption 'check'").ToList(), JsonRequestBehavior.AllowGet);
                }

            }
            #endregion

            #region 验证名称否存在
            public ActionResult CheckName(string Name, string num)
        {
            if (Name == null || Name.Trim() == "")
            {
                return Content("False");
            }
            if (num == null || num.Trim() == "")
            {
                return Content("False");
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parma = new SqlParameter[2];
                parma[0] = new SqlParameter("@Name", Name);
                parma[1] = new SqlParameter("@Num", num);
                var lista = ys.Database.SqlQuery<SP_PM_CheckName_Result>("exec [SP_PM_CheckName] @Name,@Num", parma).ToList();//验证名称是否存在
                if (lista.Count > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("---------------------------------------------------信息*99-----" );
                    return Content("False");
                }
            }
            return Content("True");
        }
        #endregion

        #region 新增科目类别
        public ActionResult AddSubjectType(string name)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "add");
                parms[1] = new SqlParameter("@line", name);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_SubjectCategory  @Type,@line", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion

        #region 删除科目类别
        public ActionResult DlSubjectType(string ID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "del");
                parms[1] = new SqlParameter("@line", ID);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_SubjectCategory  @Type,@line", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion


        #region 修改科目类别
        public ActionResult UpSubjectType(string name, string status, string ID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "up");
                parms[1] = new SqlParameter("@line", name);
                parms[2] = new SqlParameter("@status", status);
                parms[3] = new SqlParameter("@id", ID);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_SubjectCategory  @Type,@line,@status,@id", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion

        #region 新增科目
        public ActionResult AddAccounting(string SubjectCode , string SubjectTypeName, string code2, string gss ,
            string sexa , string AllName , string sexb , string sexd , string fu)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[12];
                parms[0] = new SqlParameter("@TYPE", "add");
                parms[1] = new SqlParameter("@line", SubjectCode);
                parms[2] = new SqlParameter("@status", "");
                parms[3] = new SqlParameter("@id", "");
                parms[4] = new SqlParameter("@SubjectTypeName", SubjectTypeName);
                parms[5] = new SqlParameter("@code2", code2);
                parms[6] = new SqlParameter("@gss", gss);
                parms[7] = new SqlParameter("@sexa", sexa);
                parms[8] = new SqlParameter("@AllName", AllName);
                parms[9] = new SqlParameter("@sexb", sexb);
                parms[10] = new SqlParameter("@sexd", sexd);
                parms[11] = new SqlParameter("@fu", fu);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_Caption  @TYPE,@line,@status," +
                    "@id,@SubjectTypeName,@code2,@gss,@sexa,@AllName,@sexb,@sexd,@fu", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion

        #region 修改科目
        public ActionResult UpAccounting(string SubjectCode, string SubjectTypeName, string code2, string gss,
            string sexa, string AllName, string sexb, string sexd, string fu,string id, string status)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "up" },
                { "line", SubjectCode },
                { "status", status },
                { "id", id },
                { "SubjectTypeName", SubjectTypeName },
                { "code2", code2 },
                { "gss", gss },
                { "sexa", sexa },
                { "sexb", sexb },
                { "sexd", sexd },
                { "fu", fu },
                { "AllName", AllName }
            };
            int i= Tools<object>.SqlComm("exec SP_PM_Caption ", data);
               if (i > 0)
                {
                    return Content("True");
                }
            return Content("False");
        }
        #endregion
        #region 删除科目
        public ActionResult DlAccounting(string ID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@TYPE", "del");
                parms[1] = new SqlParameter("@line", ID);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_Caption  @Type,@line", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion



        #region 查询费用部门名称
        public ActionResult SeDepts()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                return Json(ys.Database.SqlQuery<SP_PM_KQXTDept_Result>("exec SP_PM_KQXTDept ").ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion


        #region 查询支付科目名称 
        public ActionResult SeCaption()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                return Json(ys.Database.SqlQuery<SP_PM_Captionn_Result>("exec SP_PM_Captionn ").ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion


        #region 查询支付科目名称 
        public ActionResult SeCaptionall()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                return Json(ys.Database.SqlQuery<SP_PM_Captionnall_Result>("exec SP_PM_Captionnall ").ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
        #region 查询支付科目名称 
        public ActionResult SeContract()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                return Json(ys.Database.SqlQuery<SP_PM_SeContract_Result>("exec SP_PM_SeContract ").ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 新增费用部门
        public ActionResult AddDept(string name)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "add");
                parms[1] = new SqlParameter("@line", name);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_Dept  @Type,@line", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion

        #region 删除费用部门
        public ActionResult DlDept(string ID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "del");
                parms[1] = new SqlParameter("@line", ID);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_Dept  @Type,@line", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion


        #region 修改费用部门
        public ActionResult UpDept(string name, string status, string ID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "up");
                parms[1] = new SqlParameter("@line", name);
                parms[2] = new SqlParameter("@status", status);
                parms[3] = new SqlParameter("@id", ID);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_Dept  @Type,@line,@status,@id", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion

        #region//搜索费用报销单
        public ActionResult SeLaimingList(string danh,string dates, string datee, int page, int limit, string status,string id)
        {
            if (dates == null)
            {
                dates = "";
            }
            if (datee == null)
            {
                datee = "";
            }
            if (status == null)
            {
                status = "";
            }
            if (danh == null)
            {
                danh = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@TYPE", "check");
                parms[1] = new SqlParameter("@line", dates);
                parms[2] = new SqlParameter("@line2", datee);
                parms[3] = new SqlParameter("@status", status);
                parms[4] = new SqlParameter("@id", id);
                parms[5] = new SqlParameter("@ytu", danh);

                var list = ys.Database.SqlQuery<SP_PM_LaimingBXD_Result>("exec SP_PM_LaimingBXD @TYPE,@line,@line2,@status,@id,@ytu", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_LaimingBXD_Result> pageList = new PageList<SP_PM_LaimingBXD_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region//搜索费用报销单
        public ActionResult SeLaimingListsh(string dates, string datee, int page, int limit, string name, string status)
        {
            if (dates == null)
            {
                dates = "";
            }
            if (datee == null)
            {
                datee = "";
            }
            if (status == null)
            {
                status = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", dates);
                parms[2] = new SqlParameter("@line2", datee);
                parms[3] = new SqlParameter("@status", status);
                parms[4] = new SqlParameter("@id", name);

                var list = ys.Database.SqlQuery<SP_PM_LaimingBXDshTWO_Result>("exec SP_PM_LaimingBXDshTWO @Type,@line,@line2,@status,@id", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_LaimingBXDshTWO_Result> pageList = new PageList<SP_PM_LaimingBXDshTWO_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region//新增报销单

        public ActionResult AddLaiming(string ytu, string mans, string manf, string manb,
            string manl, string money, string note, string date4, string dept, string id, string danh)
        {
            string test = "";
            if (danh == null)
            {
                danh = "";
            }
            if (dept=="3")
            {
                 test="1";
            }
            if (mans == null)
            {
                mans = "";
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                            SqlParameter[] parms = new SqlParameter[17];
                            parms[0] = new SqlParameter("@TYPE", "add");
                            parms[1] = new SqlParameter("@line", test);
                            parms[2] = new SqlParameter("@line2", "");
                            parms[3] = new SqlParameter("@status", "");
                            parms[4] = new SqlParameter("@id", id);
                            parms[5] = new SqlParameter("@ytu", ytu);
                            parms[6] = new SqlParameter("@mans", mans);
                            parms[7] = new SqlParameter("@manf", manf);
                            parms[8] = new SqlParameter("@manb", manb);
                            parms[9] = new SqlParameter("@manl", manl);
                            parms[10] = new SqlParameter("@money", money);
                            parms[11] = new SqlParameter("@note", note);
                            parms[12] = new SqlParameter("@date", date4);
                            parms[13] = new SqlParameter("@count", "0");
                            parms[14] = new SqlParameter("@src", "");
                            parms[15] = new SqlParameter("@dept", dept);
                            parms[16] = new SqlParameter("@danh", danh);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_LaimingAD  @TYPE,@line,@line2,@status,@id,@ytu," +
                                "@mans,@manf,@manb,@manl,@money,@note,@date,@count,@src,@dept,@danh", parms);
                            if (w > 0)
                            {
                                   return Content("True");

                            }
                return Content("False");
            }
        }
        #endregion

        #region//新增报销单(批量上传)
        public ActionResult AddLaimingLoad(string id,HttpPostedFileBase file,string type)
        {

            Dictionary<string, object> hasmap = new Dictionary<string, Object>();
            try
            {
                if (file == null)
                {
                    hasmap.Add("code", 3);
                    hasmap.Add("msg", "未选择文件！");
                    hasmap.Add("data", "");
                    return Json(hasmap, JsonRequestBehavior.AllowGet);
                }
                else
                { 
                    var fileName1 = Path.Combine(Request.MapPath("~/Uploadf"), Path.GetFileName(file.FileName));
                    file.SaveAs(fileName1);
                    var fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
                        using (YLMES_newEntities ys = new YLMES_newEntities())
                        {
                            SqlParameter[] parms = new SqlParameter[5];
                            parms[0] = new SqlParameter("@TYPE", type);
                            parms[1] = new SqlParameter("@line",1);
                            parms[2] = new SqlParameter("@line2",fname+"&$");
                            parms[3] = new SqlParameter("@status", "");
                            parms[4] = new SqlParameter("@id",id);
                            int w = ys.Database.ExecuteSqlCommand("exec SP_PM_Laimingzful  @TYPE,@line,@line2,@status,@id", parms);
                            if (w > 0)
                            {
                                hasmap.Add("code", 0);
                                hasmap.Add("msg", "上传成功！");
                                hasmap.Add("data", "");
                                return Json(hasmap, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                hasmap.Add("code", 1);
                                hasmap.Add("msg", "修改插入出现异常！");
                                hasmap.Add("data", "");
                                return Json(hasmap, JsonRequestBehavior.AllowGet);
                            }
                        }        
                }
            }
            catch (Exception ex)
            {
                hasmap.Add("code",2);
                hasmap.Add("msg", "上传出现异常！");
                hasmap.Add("data", "");
                Console.WriteLine(ex.Message);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 修改报销单
        public ActionResult UpLaiming(string id, string ytu, string mans, string manf, string manb,
            string manl, string money, string note, string date4, string dept)

        {
            if (mans == null)
            {
                mans = "";
            }
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "up" },
                { "line", "" },
                { "line2", "" },
                { "status", "" },
                { "id", id },
                { "ytu", ytu },
                { "mans", mans },
                { "manf", manf },
                { "manb", manb },
                { "manl", manl },
                { "money", money },
                { "note", note },
                { "date", date4 },
                { "count", "" },
                { "src", "" },
                { "dept", dept }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_LaimingAD ", data);
            if (i > 0)
            {
                return Content("True");
            }
            return Content("False");
        }
        #endregion

        #region 删除报销单
        public ActionResult DelLaiming(string id)
        {
            Dictionary<string, string> datad = new Dictionary<string, string>
            {
                { "TYPE", "check" },
                { "str", "&$" },
                { "id", id },
                { "index", "1" }
            };
            var list = Tools<SP_PM_GETLaiminglj_Result>.SqlList("exec [SP_PM_GETLaiminglj]", datad);
            if (list != null)
            {
                foreach (var a in list)
                {
                    string fileds = "~/Uploadf/" + a.col;
                    string filePath = Server.MapPath(fileds);
                    System.IO.File.Delete(filePath);
                    //System.Diagnostics.Debug.WriteLine("---------------------------------信息*99---" + a.col);
                }   
            }
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "del" },
                { "line", id }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_LaimingAD ", data);
            if (i > 0)
            {
                return Content("True");
            }
            return Content("False");
        }
        #endregion

        #region 查询上传附件路径
        public ActionResult SeLaiminlj(string id,string type)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", type },
                { "str", "&$" },
                { "id", id },
                { "index", "1" }
            };
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = Tools<SP_PM_GETLaiminglj_Result>.SqlList("exec [SP_PM_GETLaiminglj]", data);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 删除上传附件路径
        public ActionResult DelLoadlj(string id,List<string> delstr,string type)
        {        
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (delstr != null)
                {
                    foreach (string str in delstr)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>
                        {
                            { "TYPE", type },
                            { "id", id },
                            { "index", str + "&$" }
                        };
                        int i = Tools<object>.SqlComm("exec [SP_PM_DelLaiminglj] ", data);
                        if (i < 1)
                        {
                            return Content("False");
                        }
                        else
                        {                   
                            string fileds = "~/Uploadf/" + str;
                            string filePath = Server.MapPath(fileds);
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
            }
            return Content("True");
        }
        #endregion

        #region 修改提交报销单
        public ActionResult UpLaimingztai(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@TYPE", "up3");
                parms[1] = new SqlParameter("@line", id);

                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_LaimingAD  @TYPE,@line", parms);
                if (w > 0)
                {
                    return Content("True");
                }

            }
            return Content("False");
        }
        #endregion


        #region 修改提交报销单
        public ActionResult UpLaimingztaifs(string id, string dh, string name)
        {
   
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@TYPE", "up");
                parms[1] = new SqlParameter("@line", id);
                parms[2] = new SqlParameter("@line2", name);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_LaimingUPA  @TYPE,@line,@line2", parms);
        
                SqlParameter[] parms2 = new SqlParameter[3];
                parms2[0] = new SqlParameter("@TYPE", "up");
                parms2[1] = new SqlParameter("@line", dh);
                parms2[2] = new SqlParameter("@line2", name);
                int w2 = ys.Database.ExecuteSqlCommand("exec SP_PM_LaimingUPA  @TYPE,@line,@line2", parms2);
     

            }
            return Content("True");
        }
        #endregion
        #region 审核报销单
        public ActionResult UpLaimingsh(List<string> delList, List<string> delList2, string name)
        {
            int a = 0;
            if (name == null)
            {
                name = "";
            }

            if (delList != null)
            {
                foreach (var sb in delList)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "TYPE", "up4" },
                        { "line", sb },
                        { "line2", name }
                    };

                    int i = Tools<object>.SqlComm("exec SP_PM_LaimingADTWO ", data);
                 
                }
            }

            if (delList2 != null)
            {
                foreach (var sb in delList2)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "TYPE", "up4" },
                        { "line", sb },
                        { "line2", name }
                    };

                    int i = Tools<object>.SqlComm("exec SP_PM_LaimingADTWO ", data);
                 
                }
            }
            return Content("True");
        }
        #endregion
        #region 二审审核报销单
        public ActionResult UpLaimingqr(List<string> delList, string dept)
        {
            if (dept == null)
            {
                dept = "";
            }
          
            if (delList != null)
            {
                foreach (var sb in delList)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "TYPE", "up2" },
                        { "line", sb },
                        { "line2", dept }
                    };
                    if (dept=="2")
                    {
                        data.Add("status", dept);
                    }
             
                    int i = Tools<object>.SqlComm("exec SP_PM_LaimingUp ", data);
                    if (i < 0)
                    {
                        return Content("False");
                    }
                }
            }

            return Content("True");
        }
        #endregion

        #region 查询凭证录入报销单
        //public ActionResult SeLaimingListpz(string dates, string datee, int page, int limit, string status)
        //{
        //    if (dates == null)
        //    {
        //        dates = "";
        //    }
        //    if (datee == null)
        //    {
        //        datee = "";
        //    }
        //    if (status == null || status == "全部")
        //    {
        //        status = "";
        //    }

        //    Dictionary<string, Object> hasmap;
        //    using (YLMES_newEntities ys = new YLMES_newEntities())
        //    {
        //        SqlParameter[] parms = new SqlParameter[4];
        //        parms[0] = new SqlParameter("@Type", "check");
        //        parms[1] = new SqlParameter("@line", dates);
        //        parms[2] = new SqlParameter("@line2", datee);
        //        parms[3] = new SqlParameter("@status", status);
  
        //        var list = ys.Database.SqlQuery<SP_PM_SeLaimingListpz_Result>("exec SP_PM_SeLaimingListpz @Type,@line,@line2,@status", parms).ToList();
        //        hasmap = new Dictionary<string, Object>();
        //        PageList<SP_PM_SeLaimingListpz_Result> pageList = new PageList<SP_PM_SeLaimingListpz_Result>(list, page, limit);
        //        int count = list.Count();
        //        hasmap.Add("code", 0);
        //        hasmap.Add("msg", "");
        //        hasmap.Add("count", count);
        //        hasmap.Add("data", pageList);
        //    }
        //    return Json(hasmap, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region 查询凭证
        public ActionResult SeAccuntingListpz(string dates, string datee, int page, int limit)
        {
            if (dates == null)
            {
                dates = "";
            }
            if (datee == null)
            {
                datee = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@TYPE", "check");
                parms[1] = new SqlParameter("@line", dates);
                parms[2] = new SqlParameter("@line2", datee);
                var list = ys.Database.SqlQuery<SP_PM_SeAccuntingpz_Result>("exec SP_PM_SeAccuntingpz @TYPE,@line,@line2", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_SeAccuntingpz_Result> pageList = new PageList<SP_PM_SeAccuntingpz_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 支付查询报销单
        public ActionResult SeLaimingListzf(string dates, string datee, int page, int limit, string status)
        {
            if (dates == null)
            {
                dates = "";
            }
            if (datee == null)
            {
                datee = "";
            }
            if (status == null || status == "全部")
            {
                status = "";
            }

            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", dates);
                parms[2] = new SqlParameter("@line2", datee);
                parms[3] = new SqlParameter("@status", status);

                var list = ys.Database.SqlQuery<SP_PM_LaimingZF_Result>("exec SP_PM_LaimingZF @Type,@line,@line2,@status", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_LaimingZF_Result> pageList = new PageList<SP_PM_LaimingZF_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 记账查询报销单
        public ActionResult SeLaimingListzfpz(string dates, string datee, int page, int limit, string status)
        {
            if (dates == null)
            {
                dates = "";
            }
            if (datee == null)
            {
                datee = "";
            }
            if (status == null || status == "全部")
            {
                status = "";
            }

            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", dates);
                parms[2] = new SqlParameter("@line2", datee);
                parms[3] = new SqlParameter("@status", status);

                var list = ys.Database.SqlQuery<SP_PM_LaimingZFpz_Result>("exec SP_PM_LaimingZFpz @Type,@line,@line2,@status", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_LaimingZFpz_Result> pageList = new PageList<SP_PM_LaimingZFpz_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 记账
        public ActionResult UpLaimingpz(string name,string id,string note5,string km)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "up" },
                { "line", km },
                { "line2", name },
                { "status", note5 },
                { "id", id }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_SeLaimingListpz ", data);
                    if (i < 0)
                    {
                        return Content("False");
                    }
            return Content("True");
        }
        #endregion

        #region//新增凭证录入

        public ActionResult AddLaimingpz(string ytu, string mans, string manf, string manb,
            string manl, string money, string note, string date4, string dept)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[16];
                parms[0] = new SqlParameter("@TYPE", "add");
                parms[1] = new SqlParameter("@line", "");
                parms[2] = new SqlParameter("@line2", "");
                parms[3] = new SqlParameter("@status", "");
                parms[4] = new SqlParameter("@id", manb);
                parms[5] = new SqlParameter("@ytu", ytu);
                parms[6] = new SqlParameter("@mans", mans);
                parms[7] = new SqlParameter("@manf", manf);
                parms[8] = new SqlParameter("@manb", manb);
                parms[9] = new SqlParameter("@manl", manl);
                parms[10] = new SqlParameter("@money", money);
                parms[11] = new SqlParameter("@note", note);
                parms[12] = new SqlParameter("@date", date4);
                parms[13] = new SqlParameter("@count", "0");
                parms[14] = new SqlParameter("@src", "");
                parms[15] = new SqlParameter("@dept", dept);
                int w = ys.Database.ExecuteSqlCommand("exec SP_PM_SeLaimingListpz  @TYPE,@line,@line2,@status,@id,@ytu," +
                    "@mans,@manf,@manb,@manl,@money,@note,@date,@count,@src,@dept", parms);
                if (w > 0)
                {
                    return Content("True");

                }
                return Content("False");
            }
        }
        #endregion

        #region 修改报销单2
        public ActionResult UpLaimingpzt(string id, string ytu, string mans, string manf, string manb,
            string manl, string money, string note, string date4, string dept)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "up2" },
                { "line", "" },
                { "line2", "" },
                { "status", "" },
                { "id", id },
                { "ytu", ytu },
                { "mans", mans },
                { "manf", manf },
                { "manb", manb },
                { "manl", manl },
                { "money", money },
                { "note", note },
                { "date", date4 },
                { "count", "" },
                { "src", "" },
                { "dept", dept }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_SeLaimingListpz ", data);
            if (i > 0)
            {
                return Content("True");
            }
            return Content("False");
        }
        #endregion


        #region 记账反审
        public ActionResult UpLaimingpzfs(string id)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "up3" },
                { "line", id }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_SeLaimingListpz ", data);
            if (i > 0)
            {
                return Content("True");
            }
            return Content("False");
        }
        #endregion

        
        #region 支付
        public ActionResult UpLaimingzhifu(string name,string danh)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "up" },
                { "line", name },
                { "line2", danh }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_LaimingZhifu ", data);
            if (i > 0)
            {
                return Content("True");
            }
            return Content("False");
        }
        #endregion


        #region 记账2
        public string AccuntingAD(string pzdanh,string jzh,List<Dictionary<string,string>> data )
        {
            using (YLMES_newEntities y=new YLMES_newEntities ()) {
                for (var a = 0; a < data.Count; a++) {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@jzh", jzh);
                    parameters[1] = new SqlParameter("@pzdanh", pzdanh);
                    parameters[2] = new SqlParameter("@CrName", Session["name"].ToString());
                    var i = 3;
                    foreach (var va in data[a]) {
                        parameters[i] = new SqlParameter("@"+va.Key,va.Value);
                        i++;
                    }
                    y.Database.ExecuteSqlCommand("exec AddPz @jzh=@jzh,@Pnumber=@pzdanh,@Cnumber=@numBering,@Dept=@name4,@j=@name2,@d=@name3,@zy=@name,@km=@name1,@CrName=@CrName", parameters);
                }
            }
            return "true";
        }
        #endregion

        #region 查询报销单单号
        public ActionResult SeDanhList(string yh)
        {
            if (yh == null)
            {
                yh = "";
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Name", "");
                parms[1] = new SqlParameter("@Num", "6");
                parms[2] = new SqlParameter("@yh",yh);
                return Json(ys.Database.SqlQuery<SP_PM_CheckDanH_Result>("exec SP_PM_CheckDanH @Name,@Num,@yh", parms).ToList(), JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region//根据单号搜索费用报销单
        public ActionResult SeLaimingListDanh(string danh, string id, int page, int limit)
        {
            if (id == null)
            {
                id = "";
            }
            if (danh == null)
            {
                danh = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@TYPE", "check");
                parms[1] = new SqlParameter("@line", id);
                parms[2] = new SqlParameter("@line2", danh);
       
                var list = ys.Database.SqlQuery<SP_PM_LaimingDanh_Result>("exec SP_PM_LaimingDanh @TYPE,@line,@line2", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_LaimingDanh_Result> pageList = new PageList<SP_PM_LaimingDanh_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region//根据二审状态搜索费用报销单
        public ActionResult SeLaimingListqr(string dates, string datee, int page, int limit , string status,string id)
        {
            if (dates == null)
            {
                dates = "";
            }
            if (datee == null)
            {
                datee = "";
            }
            if (status == null)
            {
                status = "";
            }
            if (id == null)
            {
                id = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "check");
                parms[1] = new SqlParameter("@line", dates);
                parms[2] = new SqlParameter("@line2", datee);
                parms[3] = new SqlParameter("@status", status);
                parms[4] = new SqlParameter("@id", id);
                var list = ys.Database.SqlQuery<SP_PM_LaimingBXDqr_Result>("exec SP_PM_LaimingBXDqr @TYPE,@line,@line2,@status,@id", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_LaimingBXDqr_Result> pageList = new PageList<SP_PM_LaimingBXDqr_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region//根据凭证单号查询凭证
        public ActionResult SeAccuntingse(string danh, int page, int limit)
        {
            if (danh == null)
            {
                danh = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@TYPE", "check");
                parms[1] = new SqlParameter("@line", danh);

                var list = ys.Database.SqlQuery<SP_PM_Accuntingse_Result>("exec SP_PM_Accuntingse @TYPE,@line", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_Accuntingse_Result> pageList = new PageList<SP_PM_Accuntingse_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion

 
        public ActionResult DelAccounting(string id,string danh)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "del" },
                { "line", id },
                { "line2", danh }
            };
            int i = Tools<object>.SqlComm("exec SP_PM_LaimingJZ ", data);
            if (i > 0)
            {
                return Content("True");
            }
            return Content("False");
        }

  
        public ActionResult SeAccuntingJZH(string date)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "TYPE", "sel" },
                { "line", date }
            };
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = Tools<SP_PM_AccountingJZH_Result>.SqlList("exec [SP_PM_AccountingJZH]", data);
                if (list != null)
                {
                    int i = 4;
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
    }

}