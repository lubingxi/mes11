using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;
using System.Collections;
using System.Text;

namespace YlMES.Controllers
{
    public class ProductionPlanningController : Controller
    {
        string connString = "Data Source=192.168.1.251;Initial Catalog=KQXT;User ID=admin;Password=admin123";
        // GET: ProductionPlanning
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PPCreaty()
        {
            return View();
        }
        //查询历史合同
        public ActionResult CheckHtml(string cuid) {
            List<cuhtml> cuhtmls;
            using (YLMES_newEntities ys =new YLMES_newEntities()) {
                cuhtmls = ys.cuhtml.Where(p => p.cuid == cuid).ToList();
            }
            return Json(cuhtmls,JsonRequestBehavior.AllowGet);
        }
        //历史合同保存
        [ValidateInput(false)]
        public string Addhtml(string id,string ids,string html) {
            string type = "add";
            if (ids!="0") {
                type = "up";
                id = ids;
            }
            string html1 = html.Replace("'", "\"");
            using (YLMES_newEntities ys=new YLMES_newEntities()) {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@cuid",id);
                parameters[1] = new SqlParameter("@text", html1);
                parameters[2] = new SqlParameter("@type", type);
                ys.Database.ExecuteSqlCommand("exec Addhtml @type=@type,@text=@text,@cuid=@cuid",parameters);

            }
                return "true";
        }
        //订单明细
        public ActionResult notice(string id, string studs)
        {
            TempData["notice"] = id;
            ViewData["stu"] = studs;
            TempData["not"] = id;
            Session["TdId"] = id;
            Session["ZdId"] = id;
            ViewData["ids"] = id;
            ViewData["idw"] = id;
            return View();
        }
        public ActionResult Queren(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                int val = 0;
                var con = ys.C_Contract.Where(c => c.ID == i).FirstOrDefault();
                if (con.StatusID == "计划部订单处理中心确认完成生产订单转换")
                {
                    return Content("two");
                }
                var list = ys.C_ContractProductDetail.Where(c => c.ContractID == i).ToList();
                foreach (var s in list)
                {
                    if (s.Status == "计划部订单处理中心确认收到生产订单")
                    {
                        val = 1;

                    }
                }
                if (val == 1)
                {
                    return Content("queshao");
                }
                else
                {
                    con.StatusID = "计划部订单处理中心确认完成生产订单转换";
                    ys.SaveChanges();
                }
            }
            return Content("true");
        }
        public ActionResult Contract_Check(string id)
        {
            //显示合同信息
            XianShiContract(id);
            ViewData["ContractDetialsCheckId"] = id;
            return View();
        }

        #region 工单分配
        public ActionResult Distribution()
        {
            return View();
        }
        public JsonResult DistributionName()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_WorkOrder.Where(p => p.Status.Contains("新建状态")).ToList();
                var Line = ys.PM_ProductLine.ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("list", list);
                hasmap.Add("Line", Line);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult WorkorderNOlist(string WorkorderNO)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_WorkOrder.Where(p => p.WorkorderNO.Contains(WorkorderNO)).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Linelist(string Line)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_ProductLine.Where(p => p.Line.Contains(Line)).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //验证
        public string EmployeeName(string uPwd)
        {
            SqlConnection conn = new SqlConnection(connString);
            string uName = Session["name"].ToString();
            SqlCommand Mycommand = null;
            SqlDataReader dr = null;
            try
            {
                conn.Open();
                string sqlStr = "select * from  [Employee]   where [username]='" + uName + "' and [PWD] ='" + uPwd + "' ";
                Mycommand = new SqlCommand(sqlStr, conn);
                dr = Mycommand.ExecuteReader();
                if (dr.Read())
                {
                    return "true";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                conn.Close();
            }
            return "false";
        }
        public string DistributionWorkorderNO(string WorkorderNO, string Line,string DueData)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@WorkorderNO", WorkorderNO);
                parms[1] = new SqlParameter("@Line", Line);
                parms[2] = new SqlParameter("@DueData", DueData);
                int i = ys.Database.ExecuteSqlCommand("exec ProductionPlanning_Distribution @WorkorderNO,@Line,@DueData", parms);
                if (i > 0)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
        }
        #endregion

        #region 显示转订单信息
        public ActionResult PPadd(string Cid, string pid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Session["PDId"] = pid;
                int cid = int.Parse(Cid);
                int ppid = int.Parse(pid);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ContractID", cid);
                parms[1] = new SqlParameter("@ProductDetailID", ppid);
                var can = ys.Database.SqlQuery<ProductDetail_Order_check_Result>("exec ProductDetail_Order_check  @ContractID,@ProductDetailID", parms).FirstOrDefault();
                ViewData["PojectName"] = can.CustomerName;
                ViewData["select"] = can.产品类型;
                ViewData["ProductName"] = can.ProductName;
                ViewData["P_Speed"] = can.速度;
                ViewData["ElectricalRequirements"] = can.电气要求;
                ViewData["materialofworkpiece"] = can.工件主要材质;
                ViewData["ChildPartWeight"] = can.工件重量;
                ViewData["RollerDiameter"] = can.滚筒直径;
                ViewData["RollerSurface"] = can.滚筒表面处理形式;
                ViewData["MainBeaMaterial"] = can.主梁材质;
                ViewData["ProductSpec"] = can.产品规格;
                ViewData["CarryingCapacity"] = can.载重;
                ViewData["EquipmentWorkplace"] = can.设备工作场所;
                ViewData["ChildPartSpecRange"] = can.工件最大最小外形尺寸;
                ViewData["WorkpieceFeedingMode"] = can.工件来料输送方式;
                ViewData["RollerMaterial"] = can.滚筒材质;
                ViewData["RollerTransferMode"] = can.滚筒输送形式;
                ViewData["Count"] = can.合同数量;
                ViewData["Units"] = can.单位;
                ViewData["selectd"] = can.是否动力;
                Session["ProductTypeID"] = can.产品类型序号;
            }
            return View();
        }
        #endregion 

        #region 显示修改订单信息
        public ActionResult PPEdit(string Cid, string pid, string inputs)
        {
            Session["PDId"] = pid;
            ViewData["puts"] = inputs;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int cid = int.Parse(Cid);
                int ppid = int.Parse(pid);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ContractID", cid);
                parms[1] = new SqlParameter("@ProductDetailID", ppid);
                var can = ys.Database.SqlQuery<ProductDetail_Order_Edit_Check_Result>("exec ProductDetail_Order_Edit_Check @ContractID,@ProductDetailID", parms).FirstOrDefault();
                ViewData["PojectName"] = can.项目名称;
                ViewData["Status"] = can.订单状态;
                ViewData["select"] = can.产品类型;
                ViewData["ProductName"] = can.产品名称;
                ViewData["ProductSpec"] = can.产品规格;
                ViewData["Count"] = can.合同数量;
                ViewData["Units"] = can.单位;
                ViewData["selectd"] = can.是否动力;
                ViewData["JieDate"] = can.截止日期;
                Session["ProductTypeID"] = can.产品类型序号;
                string filo = can.附件1;
                if (string.IsNullOrEmpty(filo))
                {
                    ViewData["file1"] = "";
                }
                else
                {
                    ViewData["file1"] = filo.Substring(filo.LastIndexOf('/') + 1);
                }
                ViewData["fileDownA"] = can.附件1;
                string filt = can.附件2;
                if (string.IsNullOrEmpty(filt))
                {
                    ViewData["file2"] = "";
                }
                else
                {
                    ViewData["file2"] = filt.Substring(filt.LastIndexOf('/') + 1);
                }
                ViewData["fileDownB"] = can.附件2;
                string filtr = can.附件3;
                if (string.IsNullOrEmpty(filtr))
                {
                    ViewData["file3"] = "";
                }
                else
                {
                    ViewData["file3"] = filtr.Substring(filtr.LastIndexOf('/') + 1);
                }
                ViewData["fileDownC"] = can.附件3;
            }
            return View();
        }
        #endregion

        #region 修改订单

        public ActionResult EdPo(string PojectName, string Status, string select, string ProductName, string ProductSpec, string Count, string Units, string selectd, string JieDate, string fileDownA, HttpPostedFileBase file1, string fileDownB, HttpPostedFileBase file2, string fileDownC, HttpPostedFileBase file3)
        {
            try
            {
                if (string.IsNullOrEmpty(select))
                {
                    select = "";
                }
                if (string.IsNullOrEmpty(selectd))
                {
                    selectd = "";
                }
                int PTiD = 0;
                string ContractID = Session["TdId"].ToString();
                string PdId = Session["PDId"].ToString();
                int cotId = int.Parse(ContractID);
                int pId = int.Parse(PdId);
                string PtID = Session["ProductTypeID"].ToString();
                if (PtID == "0")
                {
                    PTiD = 0;
                }
                else
                {
                    PTiD = int.Parse(PtID);
                }
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[26];
                    parms[0] = new SqlParameter("@ContractID", cotId);
                    parms[1] = new SqlParameter("@ProductDetailID", pId);
                    parms[2] = new SqlParameter("@ProductType", select);
                    parms[3] = new SqlParameter("@ProductName", ProductName);
                    parms[4] = new SqlParameter("@Count", Count);
                    parms[5] = new SqlParameter("@ProductSpect", ProductSpec);
                    parms[6] = new SqlParameter("@Units", Units);
                    if (file1 != null)
                    {
                        var fileName1 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file1.FileName));
                        file1.SaveAs(fileName1);
                        string fl1 = "~/Upload/" + file1.FileName;
                        parms[7] = new SqlParameter("@TaskFile1", fl1);
                        if (!string.IsNullOrEmpty(fileDownA))
                        {
                            string fld = "~/Upload/" + fileDownA;
                            string FilePath = Server.MapPath(fld);
                            System.IO.File.Delete(FilePath);
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(fileDownA))
                        {
                            parms[7] = new SqlParameter("@TaskFile1", "");
                        }
                        else
                        {
                            parms[7] = new SqlParameter("@TaskFile1", "~/Upload/" + fileDownA);
                        }

                    }
                    if (file2 != null)
                    {
                        var fileName2 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file2.FileName));
                        file2.SaveAs(fileName2);
                        string fl2 = "~/Upload/" + file2.FileName;
                        parms[8] = new SqlParameter("@TaskFile2", fl2);
                        if (!string.IsNullOrEmpty(fileDownB))
                        {
                            string fld = "~/Upload/" + fileDownB;
                            string FilePath = Server.MapPath(fld);
                            System.IO.File.Delete(FilePath);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(fileDownB))
                        {
                            parms[8] = new SqlParameter("@TaskFile2", "");
                        }
                        else
                        {
                            parms[8] = new SqlParameter("@TaskFile2", "~/Upload/" + fileDownB);
                        }
                    }
                    if (file3 != null)
                    {
                        var fileName3 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file3.FileName));
                        file3.SaveAs(fileName3);
                        string fl3 = "~/Upload/" + file3.FileName;
                        parms[8] = new SqlParameter("@TaskFile2", fl3);
                        if (!string.IsNullOrEmpty(fileDownC))
                        {
                            string fld = "~/Upload/" + fileDownC;
                            string FilePath = Server.MapPath(fld);
                            System.IO.File.Delete(FilePath);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(fileDownB))
                        {
                            parms[9] = new SqlParameter("@TaskFile3", "");
                        }
                        else
                        {
                            parms[9] = new SqlParameter("@TaskFile3", "~/Upload/" + fileDownC);
                        }

                    }
                    parms[10] = new SqlParameter("@P_Speed", "");
                    parms[11] = new SqlParameter("@DueDay", JieDate);
                    parms[12] = new SqlParameter("@P_CarryingCapacity", "");
                    parms[13] = new SqlParameter("@P_ElectricalRequirements", "");
                    parms[14] = new SqlParameter("@P_EquipmentWorkplace", "");
                    parms[15] = new SqlParameter("@P_Main_materialofworkpiece", "");
                    parms[16] = new SqlParameter("@P_ChildPartSpecRange", "");
                    parms[17] = new SqlParameter("@P_ChildPartWeight", "");
                    parms[18] = new SqlParameter("@P_WorkpieceFeedingMode", "");
                    parms[19] = new SqlParameter("@P_RollerDiameter", "");
                    parms[20] = new SqlParameter("@P_RollerMaterial", "");
                    parms[21] = new SqlParameter("@P_RollerSurface", "");
                    parms[22] = new SqlParameter("@P_RollerTransferMode", "");
                    parms[23] = new SqlParameter("@P_MainBeaMaterial", "");
                    parms[24] = new SqlParameter("@ProductTypeID", PTiD);
                    parms[25] = new SqlParameter("@ifDrive", selectd);
                    ys.Database.ExecuteSqlCommand("exec ProductDetail_Order_Confirm  @ContractID,@ProductDetailID,@ProductType,@ProductName,@Count,@ProductSpect,@Units,@TaskFile1,@TaskFile2," +
                      "@TaskFile3,@P_Speed,@DueDay,@P_CarryingCapacity,@P_ElectricalRequirements,@P_EquipmentWorkplace,@P_Main_materialofworkpiece,@P_ChildPartSpecRange,@P_ChildPartWeight,@P_WorkpieceFeedingMode," +
                      "@P_RollerDiameter,@P_RollerMaterial,@P_RollerSurface,@P_RollerTransferMode,@P_MainBeaMaterial,@ProductTypeID,@ifDrive", parms);
                }
                return Content("true");
            }

            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region 下载文件

        public ActionResult FileADownload(string file1)
        {
            if (string.IsNullOrEmpty(file1))
            {
                return Content("<script>alert('没有文件哦!');history.go(-1);</script>");
            }
            else
            {
                string filePath = Server.MapPath(file1);
                return File(filePath, "text/plain", file1.Substring(file1.LastIndexOf('/') + 1));
            }

        }
        public ActionResult FileBDownload(string file2)
        {
            if (string.IsNullOrEmpty(file2))
            {
                return Content("<script>alert('没有文件哦!');history.go(-1);</script>");
            }
            else
            {
                string filePath = Server.MapPath(file2);
                return File(filePath, "text/plain", file2.Substring(file2.LastIndexOf('/') + 1));
            }

        }
        public ActionResult FileCDownload(string file3)
        {
            if (string.IsNullOrEmpty(file3))
            {
                return Content("<script>alert('没有文件哦!');history.go(-1);</script>");
            }
            else
            {
                string filePath = Server.MapPath(file3);
                return File(filePath, "text/plain", file3.Substring(file3.LastIndexOf('/') + 1));
            }
        }
        #endregion

        #region 转生产订单
        [HttpPost]
        public ActionResult TtPo(string PojectName, string select, string ProductName, string ProductSpec, string Count, string Units, string selectd, string JieDate, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            try
            {
                string ContractID = Session["ZdId"].ToString();
                string PdId = Session["PDId"].ToString();
                int cotId = int.Parse(ContractID);
                int pId = int.Parse(PdId);
                //string PtID = Session["ProductTypeID"].ToString();
                //int PTiD = int.Parse(PtID);

                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[26];
                    parms[0] = new SqlParameter("@ContractID", cotId);
                    parms[1] = new SqlParameter("@ProductDetailID", pId);
                    parms[2] = new SqlParameter("@ProductType", select);
                    parms[3] = new SqlParameter("@ProductName", ProductName);
                    parms[4] = new SqlParameter("@Count", Count);
                    parms[5] = new SqlParameter("@ProductSpect", ProductSpec);
                    parms[6] = new SqlParameter("@Units", Units);
                    if (file1 != null)
                    {
                        var fileName1 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file1.FileName));
                        file1.SaveAs(fileName1);
                        string fl1 = "~/Upload/" + file1.FileName;
                        parms[7] = new SqlParameter("@TaskFile1", fl1);
                    }
                    else
                    {
                        parms[7] = new SqlParameter("@TaskFile1", "");
                    }
                    if (file2 != null)
                    {
                        var fileName2 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file2.FileName));
                        file2.SaveAs(fileName2);
                        string fl2 = "~/Upload/" + file2.FileName;
                        parms[8] = new SqlParameter("@TaskFile2", fl2);
                    }
                    else
                    {
                        parms[8] = new SqlParameter("@TaskFile2", "");
                    }
                    if (file3 != null)
                    {
                        var fileName3 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file3.FileName));
                        file3.SaveAs(fileName3);
                        string fl3 = "~/Upload/" + file3.FileName;
                        parms[9] = new SqlParameter("@TaskFile3", fl3);
                    }
                    else
                    {
                        parms[9] = new SqlParameter("@TaskFile3", "");
                    }
                    parms[10] = new SqlParameter("@P_Speed", "");
                    parms[11] = new SqlParameter("@DueDay", JieDate);
                    parms[12] = new SqlParameter("@P_CarryingCapacity", "");
                    parms[13] = new SqlParameter("@P_ElectricalRequirements", "");
                    parms[14] = new SqlParameter("@P_EquipmentWorkplace", "");
                    parms[15] = new SqlParameter("@P_Main_materialofworkpiece", "");
                    parms[16] = new SqlParameter("@P_ChildPartSpecRange", "");
                    parms[17] = new SqlParameter("@P_ChildPartWeight", "");
                    parms[18] = new SqlParameter("@P_WorkpieceFeedingMode", "");
                    parms[19] = new SqlParameter("@P_RollerDiameter", "");
                    parms[20] = new SqlParameter("@P_RollerMaterial", "");
                    parms[21] = new SqlParameter("@P_RollerSurface", "");
                    parms[22] = new SqlParameter("@P_RollerTransferMode", "");
                    parms[23] = new SqlParameter("@P_MainBeaMaterial", "");
                    parms[24] = new SqlParameter("@ProductTypeID", 666);
                    parms[25] = new SqlParameter("@ifDrive", selectd);
                    ys.Database.ExecuteSqlCommand("exec ProductDetail_Order_Confirm  @ContractID,@ProductDetailID,@ProductType,@ProductName,@Count,@ProductSpect,@Units,@TaskFile1,@TaskFile2," +
                      "@TaskFile3,@P_Speed,@DueDay,@P_CarryingCapacity,@P_ElectricalRequirements,@P_EquipmentWorkplace,@P_Main_materialofworkpiece,@P_ChildPartSpecRange,@P_ChildPartWeight,@P_WorkpieceFeedingMode," +
                      "@P_RollerDiameter,@P_RollerMaterial,@P_RollerSurface,@P_RollerTransferMode,@P_MainBeaMaterial,@ProductTypeID,@ifDrive", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region 显示生产订单

        public JsonResult Get_PP(string CName, string CNumber, string Statusd, string strattime, string endtime, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                if (CName == null)
                {
                    CName = "";
                }
                if (CNumber == null)
                {
                    CNumber = "";
                }
                if (Statusd == null)
                {
                    Statusd = "全部";
                }
                else if (Statusd == "全部")
                {
                    Statusd = "全部";
                }
                if (Statusd == null)
                {
                    Statusd = "已转生产订单";
                }
                else if (Statusd == "已转生产订单")
                {
                    Statusd = "已转生产订单";
                }
                if (Statusd == "未转生产订单")
                {
                    Statusd = "未转生产订单";
                }
                if (strattime == null)
                {
                    strattime = "";
                }
                if (endtime == null)
                {
                    endtime = "";
                }
                parms[0] = new SqlParameter("@CreatedTimeStart", strattime);
                parms[1] = new SqlParameter("@CreatedTimeEnd", endtime);
                parms[2] = new SqlParameter("@StatusID", "");
                parms[3] = new SqlParameter("@ProductOrderStatus", Statusd);
                parms[4] = new SqlParameter("@CustomerName", CName);
                parms[5] = new SqlParameter("@ContractNumber", CNumber);
                var list = ys.Database.SqlQuery<SP_Contract_checkProductOrder_Result>("exec SP_Contract_checkProductOrder  @CreatedTimeStart,@CreatedTimeEnd,@StatusID,@ProductOrderStatus,@CustomerName,@ContractNumber", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<SP_Contract_checkProductOrder_Result> pageList = new PageList<SP_Contract_checkProductOrder_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Get_PPd(string CName, string CNumber, string strattime, string endtime, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (CName == null)
                {
                    CName = "";
                }
                if(CNumber==null)
                {
                    CNumber = "";
                }
                if(strattime==null)
                {
                    strattime = "";
                }
                if(endtime==null)
                {
                    endtime = "";
                }
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@CreatedTimeStart", strattime);
                parms[1] = new SqlParameter("@CreatedTimeEnd", endtime);
                parms[2] = new SqlParameter("@StatusID", "");
                parms[3] = new SqlParameter("@ProductOrderStatus", "未转生产订单");
                parms[4] = new SqlParameter("@CustomerName", CName);
                parms[5] = new SqlParameter("@ContractNumber", CNumber);
                var list = ys.Database.SqlQuery<SP_Contract_checkProductOrder_Result>("exec SP_Contract_checkProductOrder  @CreatedTimeStart,@CreatedTimeEnd,@StatusID,@ProductOrderStatus,@CustomerName,@ContractNumber", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<SP_Contract_checkProductOrder_Result> pageList = new PageList<SP_Contract_checkProductOrder_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Get_PPds(int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@CreatedTimeStart", "");
                parms[1] = new SqlParameter("@CreatedTimeEnd", "");
                parms[2] = new SqlParameter("@StatusID", "");
                parms[3] = new SqlParameter("@ProductOrderStatus", "已转生产订单");
                parms[4] = new SqlParameter("@CustomerName", "");
                parms[5] = new SqlParameter("@ContractNumber", "");
                var list = ys.Database.SqlQuery<SP_Contract_checkProductOrder_Result>("exec SP_Contract_checkProductOrder  @CreatedTimeStart,@CreatedTimeEnd,@StatusID,@ProductOrderStatus,@CustomerName,@ContractNumber", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<SP_Contract_checkProductOrder_Result> pageList = new PageList<SP_Contract_checkProductOrder_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 显示销售订单转生产订单
        public JsonResult NoticeCheck(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ContractID", i);
                parms[1] = new SqlParameter("@ProductDetailID", "");
                var list = ys.Database.SqlQuery<ProductDetail_Order_check_Result>("exec ProductDetail_Order_check  @ContractID,@ProductDetailID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 显示合同

        public void XianShiContract(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@type", "checkByContractID");
                parms[1] = new SqlParameter("@ID", id);

                var list = ys.Database.SqlQuery<SP_ContractEdit_Result>("exec SP_ContractEdit @type,@ID", parms).FirstOrDefault();

                ViewData["CuName"] = list.CustomerName;
                ViewData["CuNumber"] = list.ContractNumber;
                ViewData["Money"] = list.合同金额;
                ViewData["IfInstall"] = list.是否安装;
                ViewData["id"] = list.id;
                ViewData["CreatedTime"] = list.创建时间;
                ViewData["IfIncludeTax"] = list.是否含税;
                ViewData["AmountCollected"] = list.收款金额;
                ViewData["DateOfSign"] = list.合同签订日期;
                ViewData["IfTransport"] = list.是否运输;
                ViewData["Pay"] = list.收款方式;
                ViewData["SendDate"] = list.交期;
                ViewData["weiyuetiaojian"] = list.违约条件;
                ViewData["CZongJie"] = list.合同总结;
                List<TableStatus> list1 = ys.TableStatus.ToList();

                List<SelectListItem> it = new List<SelectListItem>();
                foreach (var t in list1)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.Name, Text = t.Name };
                    if (t.Name == list.合同状态)
                    {
                        itemselectLanguage.Selected = true;
                    }
                    it.Add(itemselectLanguage);
                }
                ViewData["select"] = it;
            }
        }

        #endregion

        #region 工单管理
        //工单页面
        public ActionResult WorkManagement(string taskid,string cancle)
        {
            //Session["tide"] = taskid;
            Session["cancle"] = cancle;
            return View();
        }
        //显示工单信息
        public ActionResult CheckWorkManagement(string CName,string RepairOrder,int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@CustomerName", CName);
                parms[1] = new SqlParameter("@ContractNumber", "");
                parms[2] = new SqlParameter("@WorkorderNO", RepairOrder);
                parms[3] = new SqlParameter("@CreateTime", "");
                parms[4] = new SqlParameter("@CreateTimeTo", "");
                var list = ys.Database.SqlQuery<WOCheck_Result>("exec WOCheck @CustomerName,@ContractNumber,@WorkorderNO,@CreateTime,@CreateTimeTo", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<WOCheck_Result> pageList = new PageList<WOCheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //显示合同详细信息
        public ActionResult Contract_Checks(string id)
        {
            //显示合同信息
            XianShiContracts(id);
            ViewData["ContractDetialsCheckId"] = id;
            return View();
        }
        public void XianShiContracts(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[20];
                parms[0] = new SqlParameter("@type", "checkByContractID");
                parms[1] = new SqlParameter("@ID", id);
                parms[2] = new SqlParameter("@CustomerName", "");
                parms[3] = new SqlParameter("@ContractNumber", "");
                parms[4] = new SqlParameter("@DateOfSign", "");
                parms[5] = new SqlParameter("@Money", 315000.00);
                parms[6] = new SqlParameter("@PaymentMethod", "");
                parms[7] = new SqlParameter("@IfInstall", "");
                parms[8] = new SqlParameter("@IfTransport", "");
                parms[9] = new SqlParameter("@IfIncludeTax", "");
                parms[10] = new SqlParameter("@DeliveryTime", "");
                parms[11] = new SqlParameter("@ConditionsOfbreachOfContract", "");
                parms[12] = new SqlParameter("@Summary", "");
                parms[13] = new SqlParameter("@CreatedBy", "");
                parms[14] = new SqlParameter("@CreatedTime", "");
                parms[15] = new SqlParameter("@StatusID", "");
                parms[16] = new SqlParameter("@CreatedTimeStart", "");
                parms[17] = new SqlParameter("@CreatedTimeEnd", "");
                parms[18] = new SqlParameter("@AmountCollected", 927335.85);
                parms[19] = new SqlParameter("@ProductOrderStatus", "");
                var list = ys.Database.SqlQuery<SP_ContractEdit_Result>("exec SP_ContractEdit @type,@ID,@CustomerName,@ContractNumber,@DateOfSign,@Money,@PaymentMethod,@IfInstall,@IfTransport,@IfIncludeTax,@DeliveryTime," +
               "@ConditionsOfbreachOfContract,@Summary,@CreatedBy,@CreatedTime,@StatusID,@CreatedTimeStart,@CreatedTimeEnd,@AmountCollected,@ProductOrderStatus", parms).FirstOrDefault();
                ViewData["CuName"] = list.CustomerName;
                ViewData["CuNumber"] = list.ContractNumber;
                ViewData["Money"] = list.合同金额;
                ViewData["IfInstall"] = list.是否安装;
                ViewData["id"] = list.id;
                ViewData["select"] = list.合同状态;
                ViewData["IfIncludeTax"] = list.是否含税;
                ViewData["AmountCollected"] = list.收款金额;
                ViewData["DateOfSign"] = list.合同签订日期;
                ViewData["IfTransport"] = list.是否运输;
                ViewData["Pay"] = list.收款方式;
                ViewData["SendDate"] = list.交期;
                ViewData["weiyuetiaojian"] = list.违约条件;
                ViewData["CZongJie"] = list.合同总结;
            }

        }
        //工单详细信息页面
        public ActionResult RoDetail(string id)
        {
            ViewData["gongdan"] = id;
          
            return View();
        }
        //显示工单详细信息
        public ActionResult CheckRoDetail(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@WorkorderNO", id);
                var list = ys.Database.SqlQuery<WOdetail_Result>("exec WOdetail  @WorkorderNO", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //工单详细
        public ActionResult ByStation(string type, string dan)
        {
            ViewData["type"] = type;
            ViewData["dan"] = dan;
            return View();
        }
        //显示工单详细
        public ActionResult CheckByStation()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string type = ViewData["type"].ToString();
                string dan = ViewData["dan"].ToString();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@StationType", type);
                parms[1] = new SqlParameter("@WorkorderNO", dan);
                var list = ys.Database.SqlQuery<WOdetail_byStation_Result>("exec WOdetail_byStation  @StationType,@WorkorderNO", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 包装
        public ActionResult Pack()
        {


            return View();
        }
        public JsonResult Productlist(string WorkorderNO)
        {              
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@Type", "CheckWO");
                    parms[1] = new SqlParameter("@WorkorderNO", WorkorderNO);
                    var list = ys.Database.SqlQuery<SP_CheckWO_Package_Result>(" exec SP_CheckWO_Package @Type,@WorkorderNO", parms).ToList();
                    Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                    hasmap.Add("code", 0);
                    hasmap.Add("msg", "");
                    hasmap.Add("data", list);
                    return Json(hasmap, JsonRequestBehavior.AllowGet);
                }                
        }
        public JsonResult CheckWOpackage(string WorkorderNO)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "CheckWOpackage");
                parms[1] = new SqlParameter("@WorkorderNO", WorkorderNO);
                var list = ys.Database.SqlQuery<SP_Contract_Package_Result>(" exec SP_Contract_Package @Type,@WorkorderNO", parms).ToList();


                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ProductAdd(string WorkorderNO, string QTY, string number)
        {
            if (WorkorderNO != "" && QTY != "" && number == "1")
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[5];
                    parms[0] = new SqlParameter("@Type", "ADD");
                    parms[1] = new SqlParameter("@WorkorderNO", WorkorderNO);
                    parms[2] = new SqlParameter("@TotalPCS", Convert.ToInt32(QTY));
                    parms[3] = new SqlParameter("@CreatedEmployee", Session["name"]);
                    parms[4] = new SqlParameter("@PackageLabelQTY", Convert.ToInt32(number));
                    int i = ys.Database.ExecuteSqlCommand(" exec SP_ADD_Package @Type,@WorkorderNO,@TotalPCS,@CreatedEmployee,@PackageLabelQTY", parms);
                    if (i > 0)
                    {
                        return Json(i, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }


        }


        public JsonResult ProductWorkorderNO(string workorder)
        {
            if (workorder != "")
            {
                // var query=from user in db.PM_WorkOrder  where (user .nickname.Contains("小明")) select  ；




                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var ss1 = ys.PM_WorkOrder.Where(p => p.WorkorderNO.Contains(workorder)).ToList();
                    //var list = ys.Database.SqlQuery<PM_WorkOrder>(" SELECT *  FROM PM_WorkOrder ").ToList();
                    return Json(ss1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }
        }
        #endregion


        public ActionResult SeleHtml(string CuId)
        {
            List<htmltext> list;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                list = ys.htmltext.Where(p => p.cuid == CuId).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //保存历史合同
        [ValidateInput(false)]
        public string SaveUpHtml(string CuNumber, string HtmlText, string Type, string CuId, string ids)
        {
            if (ids != "0")
            {

                CuId = ids;
            }
            string html = HtmlText.Replace("'", "\"");

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", Type);
                parms[1] = new SqlParameter("@html", html);
                parms[2] = new SqlParameter("@CuNumber", CuNumber);
                parms[3] = new SqlParameter("@CuId", CuId);
                ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @TYPE=@Type,@html=@html,@CuNumber=@CuNumber,@cuId=@CuId", parms);
            }
            return "true";
        }
        //添加明细
        public string AddDetai(List<string> DetailName, List<string> DetailValue, string id, string ids, int dex)
        {
            StringBuilder sb = new StringBuilder();
            string Type = "add";
            if (ids != "0")
            {
                id = ids;
                Type = "up";
            }
            SqlParameter[] prams = new SqlParameter[DetailName.Count()];
            Dictionary<string, string> Applier = new Dictionary<string, string>();

            for (int index = 0; index < DetailName.Count(); index++)
            {

                Applier.Add(DetailName[index].ToString(), DetailValue[index].ToString());

            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int j = 0;
                sb.Append("exec DetailAdd @id=" + dex + ", @CreatedBy='" + Session["name"] + "', @type='" + Type + "'," + "@ContractID=" + id + ",");
                foreach (var app in Applier)
                {
                    prams[j] = new SqlParameter("@" + app.Key + "1", app.Value);


                    if ((j + 1) == DetailName.Count())
                    {
                        sb.Append("@" + app.Key + "=" + "@" + app.Key + "1");
                    }
                    else
                    {

                        sb.Append("@" + app.Key + "=" + "@" + app.Key + "1" + ",");
                        j++;
                    }
                }

                ys.Database.ExecuteSqlCommand(sb.ToString(), prams);
            }

            return "true";
        }
        //检查合同编号
        public string SeleContractNumber(string ContractNumber)
        {

            List<C_Contract> C_ContractList = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                C_ContractList = ys.C_Contract.Where(p => p.ContractNumber.Contains(ContractNumber)).ToList();

            }
            if (C_ContractList.Count != 0)
            {
                return "false";
            }
            else
            {
                return "true";

            }

        }
        //客户信息查询
        public ActionResult SelApplier(int CustomerId)
        {

            List<PM_Customer> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                list = ys.PM_Customer.Where(p => p.ID == CustomerId).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //添加或修改合同
        public string AddApplierList(List<string> ApplierName, List<string> ApplierNameValue, string id)
        {
            StringBuilder sb = new StringBuilder();
            string Type = "add";
            if (id != "0")
            {
                Type = "update";
            }
            SqlParameter[] prams = new SqlParameter[ApplierName.Count];
            Dictionary<string, string> Applier = new Dictionary<string, string>();

            for (int index = 0; index < ApplierName.Count; index++)
            {

                Applier.Add(ApplierName[index].ToString(), ApplierNameValue[index].ToString());

            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int j = 0;
                sb.Append("exec SP_ContractEdit @StatusID='销售部新建销售合同', @CreatedBy='" + Session["name"] + "', @type='" + Type + "'," + "@ID=" + id.ToString() + ",");
                foreach (var app in Applier)
                {
                    prams[j] = new SqlParameter("@" + app.Key + "1", app.Value);


                    if ((j + 1) == ApplierName.Count)
                    {
                        sb.Append("@" + app.Key + "=" + "@" + app.Key + "1");
                    }
                    else
                    {

                        sb.Append("@" + app.Key + "=" + "@" + app.Key + "1" + ",");
                        j++;
                    }
                }

                ys.Database.ExecuteSqlCommand(sb.ToString(), prams);
            }

            return "true";
        }
        //获取最新id
        public string ById()
        {
            string id;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var C_ContractList = ys.C_Contract.ToList();
                id = (C_ContractList.Max(p => p.ID)).ToString();
            }
            return id;
        }
        //删除货物
        public string Del(string cuid, string index)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] prams = new SqlParameter[3];
                prams[0] = new SqlParameter("@id", index);
                prams[1] = new SqlParameter("@ContractID", cuid);
                prams[2] = new SqlParameter("@type", "del");
                ys.Database.ExecuteSqlCommand("exec DetailAdd", prams);
            }
            return "true";
        }

        //查询客户名称
        public ActionResult SeList()
        {
            List<PM_Customer> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                list = ys.PM_Customer.ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //查询客户信息
        public ActionResult SeList1(string name)
        {
            List<PM_Customer> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                list = ys.PM_Customer.Where(p => p.CustomerName == name).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #region 工单分配查询
        //查询页面
        public ActionResult DistributedQuery(string id)
        {
            ViewData["wn"] = id;
            return View();
        }
        //查询显示
        public JsonResult Disbuquery(string WorkOrderName)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                
                if (WorkOrderName == null)
                {
                    WorkOrderName = "";
                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@StationType", "");
                parms[1] = new SqlParameter("@WorkorderNO", WorkOrderName);
                var list = ys.Database.SqlQuery<WOdetail_byStation_Result>("exec WOdetail_byStation  @StationType,@WorkorderNO", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        // 工单名称模糊查询
        public JsonResult WorkOrderNamelike(string WorkOrderName)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_WorkOrder.Where(p => p.WorkorderNO.Contains(WorkOrderName)&&p.Status=="进行中").Select(s => new { s.WorkorderNO }).Distinct().ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //删除工单匹配
        public ActionResult DeleteDisbuQuery(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int ids = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@id", ids);
                int i=ys.Database.ExecuteSqlCommand("exec DeletePM_WorkDetailHistorys @id", parms);
                if (i > 0)
                {
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
        }
        //工位详细信息页面
        public ActionResult WorkStationDetail(string line,string dan)
        {
            ViewData["line"] = line;
            ViewData["dan"] = dan;
            Session["line"] = line;
            Session["dan"] = dan;
            return View();
        }
        //显示工位详细信息
        public JsonResult CheckWorkStation(string line, string dan)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@line", line);
                parms[1] = new SqlParameter("@WorkorderNO", dan);
                var list = ys.Database.SqlQuery<CheckWorkStation_Result>("exec CheckWorkStation  @line,@WorkorderNO", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
           
        }
        //工位类型页面
        public ActionResult StationDetails()
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<PM_ProductStation> list = ys.PM_ProductStation.ToList();              
                List<SelectListItem> it = new List<SelectListItem>();
                foreach (var t in list)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.Station, Text = t.Station };
                    
                    it.Add(itemselectLanguage);
                }              
                ViewData["Station"] = it;

                List<ProductionName_Result> list1 = ys.Database.SqlQuery<ProductionName_Result>("exec ProductionName").ToList();
                List<SelectListItem> its = new List<SelectListItem>();
                foreach (var t in list1)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.UserName, Text = t.UserName };

                    its.Add(itemselectLanguage);
                }
                ViewData["responsible"] = its;
            }
            return View();
        }
        //新增工位信息
        public ActionResult AddStationDetails(string Station,string responsible,string RecievedPCS)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string line=Session["line"].ToString();
                    string dan = Session["dan"].ToString();
                    int rpcs = int.Parse(RecievedPCS);
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@Station", Station);
                    parms[1] = new SqlParameter("@WorkOrderName", dan);
                    parms[2] = new SqlParameter("@Name", responsible);
                    parms[3] = new SqlParameter("@Line", line);
                    parms[4] = new SqlParameter("@RecievedPCS", rpcs);
                    parms[5] = new SqlParameter("@type", "add");
                    parms[6] = new SqlParameter("@id", 0);
                    ys.Database.ExecuteSqlCommand("exec StationDetails @Station,@WorkOrderName,@Name,@Line,@RecievedPCS,@type,@id", parms);
                    return Content("true");
                }              
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
            
        }
        //删除工位信息
        public ActionResult DeleteStationDetail(string id)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);
                    PM_WorkStationDetail WorkStationDetail = ys.PM_WorkStationDetail.Where(c => c.ID == i).FirstOrDefault();
                    ys.PM_WorkStationDetail.Remove(WorkStationDetail);
                    ys.SaveChanges();
                }
                return Content("true");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
          
        }
        //修改工位信息
        public ActionResult EditStationDetail(string id,string pcs,string res)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);                  
                    int rpcs = int.Parse(pcs);
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@Station", "");
                    parms[1] = new SqlParameter("@WorkOrderName", "");
                    parms[2] = new SqlParameter("@Name", res);
                    parms[3] = new SqlParameter("@Line", "");
                    parms[4] = new SqlParameter("@RecievedPCS", rpcs);
                    parms[5] = new SqlParameter("@type", "update");
                    parms[6] = new SqlParameter("@id", i);
                    ys.Database.ExecuteSqlCommand("exec StationDetails @Station,@WorkOrderName,@Name,@Line,@RecievedPCS,@type,@id", parms);
                    return Content("true");
                }          
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");               
            }           
        }
        #endregion
    }
}