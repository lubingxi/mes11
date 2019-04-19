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
using PrintLib.Printers.Zebra;
using System.Drawing;
using System.Data;
using Spire.Xls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Spire.Pdf;
using Spire.Pdf.Bookmarks;
using Spire.Pdf.Graphics;

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
        public ActionResult upload(HttpPostedFileBase file)
        {
            var fileName1 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
            file.SaveAs(fileName1);
            Dictionary<string, object> list = new Dictionary<string, object>();
            list.Add("src", "~/Upload/" + file.FileName);

            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("code", 0);
            map.Add("msg", "");
            map.Add("data", list);
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        //查询历史合同
        public ActionResult CheckHtml(string cuid)
        {
            List<cuhtml> cuhtmls;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                cuhtmls = ys.cuhtml.Where(p => p.cuid == cuid).ToList();
            }
            return Json(cuhtmls, JsonRequestBehavior.AllowGet);
        }
        //历史合同保存
        [ValidateInput(false)]
        public string Addhtml(string id, string ids, string html)
        {
            string type = "add";
            if (ids != "0")
            {
                type = "up";
                id = ids;
            }
            string html1 = html.Replace("'", "\"");
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@cuid", id);
                parameters[1] = new SqlParameter("@text", html1);
                parameters[2] = new SqlParameter("@type", type);
                ys.Database.ExecuteSqlCommand("exec Addhtml @type=@type,@text=@text,@cuid=@cuid", parameters);

            }
            return "true";
        }
        //订单明细
        public ActionResult notice(string id, string studs,string cnumber)
        {
            TempData["notice"] = id;
            ViewData["stu"] = studs;
            ViewData["cnumber"] = cnumber;
            TempData["not"] = id;
            Session["TdId"] = id;
            Session["ZdId"] = id;
            ViewData["ids"] = id;
            ViewData["idw"] = id;
            return View();
        }
        //工单明细页面
        public ActionResult OrderDetail(string dan)
        {
            ViewData["dan"] = dan;
            return View();
        }
        //显示工单信息
        public JsonResult WOdetail()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Type", "查找物料");
                var list = ys.Database.SqlQuery<SP_PM_Material_Result>("exec SP_PM_Material  @Type,@PONO,@QTY,@MaterialID,@CreatedBy,@Status,@Desc,@Location,@ProjectName,@MaterialType,@PartNumber,@Category1ID,@Category2ID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Queren(string id,string pnumber)
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
                    YLMES.Sms.CheckSms("设计部",pnumber);
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
        public string DistributionWorkorderNO(string WorkorderNO, string Line, string DueData)
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
                    //SqlParameter[] parm = new SqlParameter[1];
                    //parm[0] = new SqlParameter("@WorkOrderNO", WorkorderNO);
                    //ys.Database.ExecuteSqlCommand("exec PM_AddWorkDetial @WorkorderNO", parm);
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
                ViewData["BreakUp"] = can.拆分;
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

        public ActionResult EdPo(string PojectName, string Status, string select, string ProductName, string ProductSpec, string Count, string Units, string selectd, string JieDate, string fileDownA, HttpPostedFileBase file1, string fileDownB, HttpPostedFileBase file2, string fileDownC, HttpPostedFileBase file3, string breakup)
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
                    SqlParameter[] parms = new SqlParameter[27];
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
                    parms[26] = new SqlParameter("@BreakUp", breakup);
                    ys.Database.ExecuteSqlCommand("exec ProductDetail_Order_Confirm  @ContractID,@ProductDetailID,@ProductType,@ProductName,@Count,@ProductSpect,@Units,@TaskFile1,@TaskFile2," +
                      "@TaskFile3,@P_Speed,@DueDay,@P_CarryingCapacity,@P_ElectricalRequirements,@P_EquipmentWorkplace,@P_Main_materialofworkpiece,@P_ChildPartSpecRange,@P_ChildPartWeight,@P_WorkpieceFeedingMode," +
                      "@P_RollerDiameter,@P_RollerMaterial,@P_RollerSurface,@P_RollerTransferMode,@P_MainBeaMaterial,@ProductTypeID,@ifDrive,@BreakUp", parms);
                }
                return Content("true");
            }

            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region 产品包装

        public ActionResult ProductPackaging()
        {
            return View();
        }
        //显示模糊查询信息
        public JsonResult TypeLikeSearch(string pns, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (pns == null)
                {
                    pns = "";
                }
                SqlParameter[] parms = new SqlParameter[1];        
                parms[0] = new SqlParameter("@PartNumber", pns);     
                var list = ys.Database.SqlQuery<SP_PM_Materials_Result>("exec SP_PM_Materials  @PartNumber", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_Materials_Result> pageList = new PageList<SP_PM_Materials_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);

            }
        }
        //打印标签页面
        public ActionResult PrintLabel(string pnumber, string pspec, string pmater, string fnumber, string id)
        {
            ViewData["pnumber"] = pnumber;
            ViewData["pspec"] = pspec;
            ViewData["pmater"] = pmater;
            ViewData["fnumber"] = fnumber;
            ViewData["id"] = id;
            return View();
        }
        public ContentResult ProductAdds(string id, string pnumber, string pspec, string pmater, string fnumber,string ProductQuantity, string PackageNumber, string CNumber,string Units)
        {
            string valuesd = "";
            if (string.IsNullOrEmpty(ProductQuantity))
            {
                valuesd = "1";
            }
             else if (string.IsNullOrEmpty(PackageNumber))
            {
                valuesd = "2";
            }
            else
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int pq = int.Parse(ProductQuantity);
                    int i = int.Parse(PackageNumber);
                    if (i > 20)
                    {
                        valuesd = "3";
                    }else
                    {
                        int mid = int.Parse(id);
                        SqlParameter[] parms = new SqlParameter[9];
                        parms[0] = new SqlParameter("@Type", "ADD");
                        parms[1] = new SqlParameter("@WorkorderNO", "");
                        parms[2] = new SqlParameter("@TotalPCS", pq);
                        parms[3] = new SqlParameter("@CreatedEmployee", Session["name"]);
                        parms[4] = new SqlParameter("@PackageLabelQTY", i);
                        parms[5] = new SqlParameter("@MaterId", mid);
                        parms[6] = new SqlParameter("@Outvalue", SqlDbType.NVarChar, 50);
                        parms[6].Direction = ParameterDirection.Output;
                        parms[7] = new SqlParameter("@CNumber", CNumber);
                        parms[8] = new SqlParameter("@Units", Units);
                        ys.Database.ExecuteSqlCommand(" exec SP_ADD_Package @Type,@WorkorderNO,@TotalPCS,@CreatedEmployee,@PackageLabelQTY,@MaterId,@Outvalue out,@CNumber,@Units", parms);
                        valuesd = (string)parms[6].Value;
                    }
                                       
                }
            }
            return Content(valuesd);
        }
        //更新物料信息
        public ActionResult ProductUpdate(string id, string pnumber, string pspec)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                var pm = ys.PM_Material.Where(s => s.ID == i).FirstOrDefault();
                pm.PartNumber = pnumber;
                pm.PartSpec = pspec;
                ys.SaveChanges();
                return Content("true");
            }
        }
        //显示打包明细全部页面
        public ActionResult AllPacking()
        {
            return View();
        }
        //显示打包明细页面
        public ActionResult PackingDetail(string packname)
        {
            ViewData["paname"] = packname;
            return View();
        }
        
        //显示打包明细信息
        public ActionResult DetailInfo(string pcname)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (pcname == null)
                {
                    pcname = "";
                }
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@PackageName", pcname);
                var list = ys.Database.SqlQuery<CheckProductDetalInfo_Result>("exec CheckProductDetalInfo @PackageName", parms).ToList();
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("code", 0);
                map.Add("msg", "");
                map.Add("data", list);
                return Json(map, JsonRequestBehavior.AllowGet);
            }
        }
        //修改打包数量
        public ActionResult UpdateCount(string id,string Count)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                int o = int.Parse(Count);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "update");
                parms[1] = new SqlParameter("@id", i);
                parms[2] = new SqlParameter("@Count",o);
               int s=ys.Database.ExecuteSqlCommand("exec PM_MakePackage @Type,@id,@Count", parms);
                if (s > 0)
                {
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }           
        }
        //删除打包信息
        public ActionResult DeleteCount(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "delete");
                parms[1] = new SqlParameter("@id", i);
                int s = ys.Database.ExecuteSqlCommand("exec PM_MakePackage @Type,@id", parms);
                if (s > 0)
                {
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
        }
        //显示打印页面
        public ActionResult txt(string pnumber, string pspec, string pmater, string ID, string pn, string qty,string unit)
        {
            ViewData["canku"] = "成品仓";
            ViewData["mingzi"] = pnumber;
            ViewData["guige"] = pspec;
            ViewData["cailiao"] = pmater;
            ViewData["PackageName"] = pn;
            ViewData["unit"] = unit;
            ViewData["qty"] = qty;
            if (ID != null)
            {
                string fileName = "1.png";
                string ImageUrl = null;
                String savePath = Server.MapPath("~/QRCodeImage") + "/" + fileName;
                Printer pt = new Printer();
                System.Drawing.Image image = pt.CreateQRCodeImage(ID, savePath);
                ImageUrl = "../../QRCodeImage/" + fileName;
                TempData["ptu"] = ImageUrl;

            }
            return View();
        }
        //新增产品信息
        public ActionResult AddMaterial()
        {
            return View();
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
        public ActionResult TtPo(string PojectName, string select, string ProductName, string ProductSpec, string Count, string Units, string selectd, string JieDate, string BreakUp, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
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
                    C_Contract con = ys.C_Contract.Where(c => c.ID == cotId).FirstOrDefault();

                    SqlParameter[] parms = new SqlParameter[27];
                    parms[0] = new SqlParameter("@ContractID", cotId);
                    parms[1] = new SqlParameter("@ProductDetailID", pId);
                    parms[2] = new SqlParameter("@ProductType", select);
                    parms[3] = new SqlParameter("@ProductName", ProductName);
                    parms[4] = new SqlParameter("@Count", Count);
                    parms[5] = new SqlParameter("@ProductSpect", ProductSpec);
                    parms[6] = new SqlParameter("@Units", Units);
                    if (!string.IsNullOrEmpty(con.TaskLevel))
                    {
                        parms[7] = new SqlParameter("@TaskFile1", con.TaskLevel);
                    }
                    //if (file1 != null)
                    //{
                    //    var fileName1 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file1.FileName));
                    //    file1.SaveAs(fileName1);
                    //    string fl1 = "~/Upload/" + file1.FileName;
                    //    parms[7] = new SqlParameter("@TaskFile1", fl1);
                    //}
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
                    parms[26] = new SqlParameter("@BreakUp", BreakUp);
                    ys.Database.ExecuteSqlCommand("exec ProductDetail_Order_Confirm  @ContractID,@ProductDetailID,@ProductType,@ProductName,@Count,@ProductSpect,@Units,@TaskFile1,@TaskFile2," +
                      "@TaskFile3,@P_Speed,@DueDay,@P_CarryingCapacity,@P_ElectricalRequirements,@P_EquipmentWorkplace,@P_Main_materialofworkpiece,@P_ChildPartSpecRange,@P_ChildPartWeight,@P_WorkpieceFeedingMode," +
                      "@P_RollerDiameter,@P_RollerMaterial,@P_RollerSurface,@P_RollerTransferMode,@P_MainBeaMaterial,@ProductTypeID,@ifDrive,@BreakUp", parms);
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
                if (CNumber == null)
                {
                    CNumber = "";
                }
                if (strattime == null)
                {
                    strattime = "";
                }
                if (endtime == null)
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
        public ActionResult WorkManagement(string taskid, string cancle)
        {
            //Session["tide"] = taskid;
            Session["cancle"] = cancle;
            return View();
        }
        public ActionResult WorkManagement2(string taskid, string cancle)
        {            
            return View();
        }
        //显示工单信息
        public ActionResult CheckWorkManagement(string CName, string RepairOrder, int page, int limit)
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
        //获取PDF路径
        public ActionResult CheckPdfPath(string src,string Materid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                //  var list =
                //from w in ys.PM_Product
                //join r in ys.PM_Route on w.RouteID equals r.ID
                //select r.ProcessDocument;
                //datatest(list.ToString());

                datatest(src, Materid);
                return Content("/pdf2/ExceltoPdf5.pdf");

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

        #region 打印工艺卡

        public ActionResult AddProcess(string marid, string id, string PartId, string gid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int pid = int.Parse(marid);
                ViewData["id"] = id;
                ViewData["PartId"] = PartId;
                string fileName = "2.png";
                string ImageUrl = null;
                String savePath = Server.MapPath("~/QRCodeImage") + "/" + fileName;
                Printer pt = new Printer();
                System.Drawing.Image image = pt.CreateQRCodeImage(gid, savePath);
                ImageUrl = "../../QRCodeImage/" + fileName;
                TempData["gid"] = ImageUrl;
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ParId", pid);
                var list = ys.Database.SqlQuery<CheckRouteName_Result>("exec CheckRouteName @ParId", parms).FirstOrDefault();
                ViewData["Route"] = list.RouteName;
            }
            return View();
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
        public string AddDetai(Dictionary<string, string> data, string id, string ids)
        {
            StringBuilder sb = new StringBuilder();
            string Type = "add";
            if (ids != "0")
            {
                id = ids;
                Type = "up";
            }
            SqlParameter[] prams = new SqlParameter[data.Count()];
            Dictionary<string, string> Applier = new Dictionary<string, string>();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int j = 0;
                sb.Append("exec DetailAdd @CreatedBy='" + Session["name"] + "', @type='" + Type + "'," + "@ContractID=" + id + ",");
                foreach (var app in data)
                {
                    prams[j] = new SqlParameter("@" + app.Key + "1", app.Value);


                    if ((j + 1) == data.Count())
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
        [HttpPost]
        public string AddApplierList(List<string> ApplierName, List<string> ApplierNameValue, string id, string file, string ContractNumber)
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
                sb.Append("exec SP_ContractEdit @StatusID='销售部新建销售合同', @CreatedBy='" + Session["name"] + "', @type='" + Type + "'," + "@ID=" + id.ToString() + "," + "@TaskLevel='" + file + "'," + "@ContractNumber='" + ContractNumber + "',");
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
                ys.Database.ExecuteSqlCommand("exec DetailAdd @type=@type,@id=@id,@ContractID=@ContractID", prams);
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
                var list = ys.PM_WorkOrder.Where(p => p.WorkorderNO.Contains(WorkOrderName) && p.Status == "进行中").Select(s => new { s.WorkorderNO }).Distinct().ToList();
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
                int i = ys.Database.ExecuteSqlCommand("exec DeletePM_WorkDetailHistorys @id", parms);
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
        public ActionResult WorkStationDetail(string line, string dan)
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
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                //SqlParameter[] parms = new SqlParameter[2];
                //parms[0] = new SqlParameter("@line", line);
                //parms[1] = new SqlParameter("@WorkorderNO", dan);
                //var list = ys.Database.SqlQuery<CheckWorkStation_Result>("exec CheckWorkStation  @line,@WorkorderNO", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                //hasmap.Add("code", 0);
                //hasmap.Add("msg", "");
                //hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }

        }
        //工位类型页面
        public ActionResult StationDetails()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
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
        public ActionResult AddStationDetails(string Station, string responsible, string RecievedPCS)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string line = Session["line"].ToString();
                    string dan = Session["dan"].ToString();
                    int rpcs = int.Parse(RecievedPCS);
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@Station", Station);
                    parms[1] = new SqlParameter("@WorkOrderName", dan);
                    parms[2] = new SqlParameter("@Name", responsible);
                    parms[3] = new SqlParameter("@Line", line);
                    parms[4] = new SqlParameter("@RecievedPCS", rpcs);
                    parms[5] = new SqlParameter("@type", "add");
                    parms[6] = new SqlParameter("@id", 66);
                    ys.Database.ExecuteSqlCommand("exec StationDetails @Station,@WorkOrderName,@Name,@Line,@RecievedPCS,@type,@id", parms);
                    return Content("true");
                }
            }
            catch (Exception ex)
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
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);
                    PM_WorkStationDetail WorkStationDetail = ys.PM_WorkStationDetail.Where(c => c.ID == i).FirstOrDefault();
                    ys.PM_WorkStationDetail.Remove(WorkStationDetail);
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
        //修改工位信息
        public ActionResult EditStationDetail(string id, string pcs, string res)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        #endregion

        //PDA扫描记录
        public ActionResult Scanrecord()
        {
            return View();
        }
        //区域
        public JsonResult ScanrecordName()
        {
            List<string> list = new List<string>();
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand Mycommand = null;
            SqlDataReader dr = null;
            conn.Open();
            string sqlStr = "SELECT distinct operation FROM [YLMES_new].[dbo].[PM_Scanrecord]";
            Mycommand = new SqlCommand(sqlStr, conn);
            dr = Mycommand.ExecuteReader();
            list.Add("全部");
            while (dr.Read())
            {
                list.Add(dr[0].ToString());
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Scanrecordlist(string operation, string Scantrim, string Endtime)
        {
            if (operation.Equals("全部"))
            {
                operation = "";
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "PDA_Scanlist");
                parms[1] = new SqlParameter("@operation", operation);
                parms[2] = new SqlParameter("@Scantrim", Scantrim);
                parms[3] = new SqlParameter("@Endtime", Endtime);
                var list = ys.Database.SqlQuery<PDA_Scanrecord_Result>("exec PDA_Scanrecord @Type,@operation,@Scantrim,@Endtime", parms).ToList();
                Dictionary<string, object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }



        #region Excel转.PDF
        public ActionResult datatest(string path,string value)
        {
            button2_Click(path, value);
            MergePdfFilesWithBookMark("0", "10", "25");
            return Content("False2");
        }

        private void button2_Click(string path,string value)
        {
            //创建一个Wordbook类对象，并加载需要转换的Excel文档
            Workbook workbook = new Workbook();
            //string save = Server.MapPath("~/Process") + "/" + path;
            string save = Server.MapPath(path);
            workbook.LoadFromFile(save);//加载excel路径4
            var fileName1 = Path.Combine(Request.MapPath("~/pdf2"), Path.GetFileName("ExceltoPdf.pdf"));//相对路径
            //将Excel文档保存为PDF，并打开转换后的PDF文档
            workbook.ConverterSetting.SheetFitToPage = true;
            workbook.SaveToFile(fileName1, Spire.Xls.FileFormat.PDF);

            var fileName2 = Path.Combine(Request.MapPath("~/pdf2"), Path.GetFileName("ExceltoPdf2.pdf"));//相对路径
            // //新建一个PDF文档对象并加载要添加印章的文档。
            //PdfDocument doc = new PdfDocument();
            //doc.LoadFromFile(fileName1);
            ////获取文档的第一页。
            //PdfPageBase page = doc.Pages[0];
            ////获取PDF页数
            //int number = doc.Pages.Count;
            ////删除最后一页
            //doc.Pages.RemoveAt(number - 1);
            var fileName3 = Path.Combine(Request.MapPath("~/QRCodeImage"), Path.GetFileName("1.png"));//相对路径
            //System.Drawing.Image img = System.Drawing.Image.FromFile(fileName3);
            string fileName = "1.png";
            string ImageUrl = null;
            String savePath = Server.MapPath("~/QRCodeImage") + "/" + fileName;
            Printer pt = new Printer();
            System.Drawing.Image img = pt.CreateQRCodeImage(value, savePath);
            String pdfS = (fileName1);//待加图像的PDF文件


            String pdfD = (fileName2);//已加图像的PDF文件


            String strQRcodeContent = "";//添加文字


            //System.Drawing.Image qrImage = GenQRBarcodeImg(strQRcodeContent, 120, 120, "BarCodeType");
            //// 保存文档。
            //doc.SaveToFile(fileName1);
            AddBarcodeIMGtoPDF(pdfS, pdfD, img, strQRcodeContent);
            ImageUrl = "../../QRCodeImage/" + fileName;
            TempData["ptu"] = ImageUrl;
        }
        //显示成品仓库存信息
        public ActionResult GoodsInventory()
        {
            return View();
        }



        public void AddBarcodeIMGtoPDF(String PDFFileFullName, String PDFFileFullNameAddedIMG, System.Drawing.Image qrImage, String strQRcodeContent)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(PDFFileFullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                // 创建一个PdfReader对象

                PdfReader reader = new PdfReader(fs);


                // 获得文档页数


                int n = reader.NumberOfPages;


                // 获得第一页的大小


                iTextSharp.text.Rectangle psize = reader.GetPageSize(1);


                float width = psize.Width;


                float height = psize.Height;


                // 创建一个文档变量
                Document document = new Document(psize,50, 50, 50, 50);//50页面空白边距


                // 创建该文档


                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(PDFFileFullNameAddedIMG, FileMode.Create));


                document.Open(); // 打开文档


                for (int iPageIndex = 1; iPageIndex <= n; iPageIndex++)

                {   // 添加内容

                    PdfContentByte cb = writer.DirectContent;

                    PdfImportedPage page11 = writer.GetImportedPage(reader, iPageIndex);

                    BaseFont bfbf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Image imglift = iTextSharp.text.Image.GetInstance((System.Drawing.Image)qrImage, System.Drawing.Imaging.ImageFormat.Bmp);

                    cb.AddTemplate(page11, 1.0f, 0, 0, 1.0f, 0, 0);
                    //所添加图像的比例，位置20140626


                    cb.AddImage(imglift, imglift.Width * 1 / 3, 0, 0, imglift.Height * 1 / 3, width - 120, height - 95);
                    document.NewPage();
                }
                document.Close();//关闭文档            
            }
            catch (Exception de)
            {
                Console.Error.WriteLine(de.Message);
                Console.Error.WriteLine(de.StackTrace);
            }
        }
        #endregion
        //扫描工单显示图纸
        public ActionResult RepairOrderDrawing()
        {
            return View();
        }
        public void MergePdfFilesWithBookMark(string q, string a, string c)
        {
            //创建一个Wordbook类对象，并加载需要转换的Excel文档
            //Workbook workbook = new Workbook();
            //workbook.LoadFromFile(@"C:\Users\Public\Nwt\cache\recv\陆炳曦\6012-1000 无动力滚筒线组装.xls");//加载excel路径
            var fileName1 = Path.Combine(Request.MapPath("~/pdf2"), Path.GetFileName("ExceltoPdf2.pdf"));//相对路径
            //                                                                                               // 将Excel文档保存为PDF，并打开转换后的PDF文档
            //workbook.SaveToFile(fileName1, Spire.Xls.FileFormat.PDF);

            //审明文件集合只能放jpg和pdf
            List<Hashtable> datas = new List<Hashtable>();
            Hashtable ht2 = new Hashtable();
            ht2.Add("title", "1"); ht2.Add("paths", fileName1);
            datas.Add(ht2);//添加pdf文件集合

            /*生成书签*/
            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();//新建一个pdf文档对象
            pdf.Pages.Add();//添加空页面
            int index = 0;
            int totalPages = 0;

            for (int i = 0; i < datas.Count; i++)
            {
                Hashtable ht = datas[i];
                Spire.Pdf.PdfDocument pdfs = new Spire.Pdf.PdfDocument();//新建一个pdf文档对象
                index = pdf.Pages.Count;//获取一个pdf文档对象的总页数
                int b = ht["paths"].ToString().LastIndexOf("\\") + 1;
                int e = ht["paths"].ToString().LastIndexOf(".");
                string marks = ht["paths"].ToString().Substring(b, e - b);//截取文件名
                string types = ht["paths"].ToString().Substring(e + 1);//截取文件后缀名
                if (types != "jpg")//类型为pdf的文件
                {
                    pdfs.LoadFromFile(ht["paths"].ToString());//加载pdf文件
                    pdfs.Bookmarks.Clear();//去除书签
                    pdf.AppendPage(pdfs);//添加无书签pdf文档对象
                }
                else//jpg类型文件
                {
                    PdfPageBase page = pdf.Pages.Add();//获取添加空页面
                    Spire.Pdf.Graphics.PdfImage image = Spire.Pdf.Graphics.PdfImage.FromFile(ht["paths"].ToString()); //加载.jpg文件  
                    Spire.Pdf.Graphics.PdfTemplate template = page.CreateTemplate();//创建一个pdf模板
                    template.Graphics.DrawImage(image, 0, 0);//在pdf模板里绘制图片
                    template.Draw(page, new PointF(0, 0));//printf（)函数是格式化输出函数， 一般用于向标准输出设备按规定格式输出信息

                }
                totalPages = pdf.Pages.Count;//获取一个pdf文档对象的总页数
                PdfBookmarkCollection markcoll = pdf.Bookmarks;//获取pdf文档对象的书签对象
                int markCount = markcoll.Count;//获取pdf文档对象的书签总数
                bool ishavetitle = false;
                for (int k = 0; k < markCount; k++)
                {
                    PdfBookmark pdfmark = markcoll[k];//获取pdf文档对象的书签对象的单个书签
                    if (pdfmark.Title == ht["title"].ToString())
                    {
                        PdfBookmark childBookmark = pdfmark.Insert(pdfmark.Count, marks);
                        childBookmark.Destination = new Spire.Pdf.General.PdfDestination(pdf.Pages[index]);
                        childBookmark.Destination.Location = new PointF(0, 0); ishavetitle = true; break;
                    }
                }
                if (!ishavetitle)
                {
                    PdfBookmark bookmark = pdf.Bookmarks.Add(ht["title"].ToString());
                    bookmark.Destination = new Spire.Pdf.General.PdfDestination(pdf.Pages[index]);
                    bookmark.Destination.Location = new PointF(0, 0);
                    PdfBookmark childBookmark = bookmark.Insert(bookmark.Count, marks);
                    childBookmark.Destination = new Spire.Pdf.General.PdfDestination(pdf.Pages[index]);
                    childBookmark.Destination.Location = new PointF(0, 0);
                }
                var fileName2 = Path.Combine(Request.MapPath("~/pdf2"), Path.GetFileName("ExceltoPdf4.pdf"));//相对路径
                var fileName3 = Path.Combine(Request.MapPath("~/pdf2"), Path.GetFileName("ExceltoPdf5.pdf"));//相对路径
                int u = 0;
                int p = pdf.Pages.Count;
                Spire.Pdf.PdfDocument pdf3 = new Spire.Pdf.PdfDocument();//新建一个pdf文档对象

                //遍历文档pdf1中的所有页面     
                foreach (PdfPageBase page in pdf.Pages)
                {

                    if (u == p)
                    {
                        break;
                    }
                    u++;
                    //指定A2大小的页面和页边距，并添加到文档pdf2
                    PdfPageBase newPage = pdf3.Pages.Add(PdfPageSize.A4, new PdfMargins(int.Parse(a), int.Parse(c)));

                    //将原pdf1中内容写入新页面              
                    page.CreateTemplate().Draw(newPage, new PointF(0, 0));

                }
                //删除第二页
                pdf3.Pages.RemoveAt(int.Parse(q));
                //保存新的PDF文档
                pdf3.SaveToFile(fileName2, Spire.Pdf.FileFormat.PDF);

                //pdf.SaveToFile(fileName2);
                MergePdfFilesWithBookMark(new string[1] { fileName2 }, fileName3);
            }

        }

        private void MergePdfFilesWithBookMark(string[] sourcePdfs, string outputPdf)
        {

            PdfReader reader = null;
            Document document = new Document();
            PdfImportedPage page = null;
            PdfCopy pdfCpy = null;
            int n = 0;
            int totalPages = 0;
            int page_offset = 0;
            List<Dictionary<string, object>> bookmarks = new List<Dictionary<string, object>>();
            IList<Dictionary<string, object>> tempBookmarks;
            for (int i = 0; i <= sourcePdfs.GetUpperBound(0); i++)
            {
                reader = new PdfReader(sourcePdfs[i]);
                reader.ConsolidateNamedDestinations();
                n = reader.NumberOfPages;
                tempBookmarks = SimpleBookmark.GetBookmark(reader);
                if (i == 0)
                {
                    document = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
                    pdfCpy = new PdfCopy(document, new FileStream(outputPdf, FileMode.Create));
                    document.Open();
                    SimpleBookmark.ShiftPageNumbers(tempBookmarks, page_offset - 1, null);
                    page_offset += n;
                    if (tempBookmarks != null)
                        bookmarks.AddRange(tempBookmarks);
                    totalPages = n;
                }
                else
                {
                    SimpleBookmark.ShiftPageNumbers(tempBookmarks, page_offset - 1, null);
                    if (tempBookmarks != null) bookmarks.AddRange(tempBookmarks);
                    page_offset += n;
                    totalPages += n;
                }
                for (int j = 2; j <= n; j++)
                {
                    page = pdfCpy.GetImportedPage(reader, j);
                    pdfCpy.AddPage(page);
                }
                reader.Close();
            }
            pdfCpy.Outlines = bookmarks;
            document.Close();

        }
    }
            }