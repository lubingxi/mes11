using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YlMES.Controllers
{
    public class ProductionPlanningController : Controller
    {
        // GET: ProductionPlanning
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PPCreaty()
        {
            return View();
        }
        //订单明细
        public ActionResult notice(string id)
        {
            TempData["notice"] = id;
            Session["TdId"] = id;
            Session["ZdId"] = id;
            ViewData["ids"] = id;
            ViewData["idw"] = id;
            return View();
        }

        public ActionResult Contract_Check(string id)
        {
            //显示合同信息
            XianShiContract(id);
            ViewData["ContractDetialsCheckId"] = id;
            return View();
        }

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
        public ActionResult PPEdit(string Cid, string pid)
        {
            Session["PDId"] = pid;
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

        public ActionResult EdPo(string PojectName, string Status, string select, string ProductName, string ProductSpec, string Count, string Units, string selectd, string JieDate,string fileDownA, HttpPostedFileBase file1,string fileDownB, HttpPostedFileBase file2,string fileDownC, HttpPostedFileBase file3)
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
                            parms[7] = new SqlParameter("@TaskFile1","");
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

        public FileResult FileADownload(string file1)
        {
            string filePath = Server.MapPath(file1);
            return File(filePath, "text/plain", file1.Substring(file1.LastIndexOf('/') + 1));
        }
        public FileResult FileBDownload(string file2)
        {
            string filePath = Server.MapPath(file2);
            return File(filePath, "text/plain", file2.Substring(file2.LastIndexOf('/') + 1));
        }
        public FileResult FileCDownload(string file3)
        {
            string filePath = Server.MapPath(file3);
            return File(filePath, "text/plain", file3.Substring(file3.LastIndexOf('/') + 1));
        }
        #endregion

        #region 转生产订单
        [HttpPost]
        public ActionResult TtPo(string PojectName, string select, string ProductName, string P_Speed, string ElectricalRequirements, string materialofworkpiece, string ChildPartWeight, string RollerDiameter, string RollerSurface, string MainBeaMaterial, string ProductSpec, string CarryingCapacity, string EquipmentWorkplace, string ChildPartSpecRange, string WorkpieceFeedingMode, string RollerMaterial, string RollerTransferMode, string Count, string Units, string selectd, string JieDate, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            try
            {
                string ContractID = Session["ZdId"].ToString();
                string PdId = Session["PDId"].ToString();
                int cotId = int.Parse(ContractID);
                int pId = int.Parse(PdId);
                string PtID = Session["ProductTypeID"].ToString();
                int PTiD = int.Parse(PtID);

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
                    parms[10] = new SqlParameter("@P_Speed", P_Speed);
                    parms[11] = new SqlParameter("@DueDay", JieDate);
                    parms[12] = new SqlParameter("@P_CarryingCapacity", CarryingCapacity);
                    parms[13] = new SqlParameter("@P_ElectricalRequirements", ElectricalRequirements);
                    parms[14] = new SqlParameter("@P_EquipmentWorkplace", EquipmentWorkplace);
                    parms[15] = new SqlParameter("@P_Main_materialofworkpiece", materialofworkpiece);
                    parms[16] = new SqlParameter("@P_ChildPartSpecRange", ChildPartSpecRange);
                    parms[17] = new SqlParameter("@P_ChildPartWeight", ChildPartWeight);
                    parms[18] = new SqlParameter("@P_WorkpieceFeedingMode", WorkpieceFeedingMode);
                    parms[19] = new SqlParameter("@P_RollerDiameter", RollerDiameter);
                    parms[20] = new SqlParameter("@P_RollerMaterial", RollerMaterial);
                    parms[21] = new SqlParameter("@P_RollerSurface", RollerSurface);
                    parms[22] = new SqlParameter("@P_RollerTransferMode", RollerTransferMode);
                    parms[23] = new SqlParameter("@P_MainBeaMaterial", MainBeaMaterial);
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

        #region 显示生产订单

        public JsonResult Get_PP(string CName, string CNumber, string Status, string Statusd, string strattime, string endtime, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];

                if (Status == "全部")
                {
                    Status = "";
                }
                if (Status == "已完成")
                {
                    Status = "已完成";
                }
                if (Status == "收款审核通过")
                {
                    Status = "收款审核通过";
                }
                if (Status == "进行中")
                {
                    Status = "进行中";
                }
                if (Statusd == "全部")
                {
                    Statusd = "全部";
                }
                if (Statusd == "已转生产订单")
                {
                    Statusd = "已转生产订单";
                }
                if (Statusd == "未转生产订单")
                {
                    Statusd = "未转生产订单";
                }
                parms[0] = new SqlParameter("@CreatedTimeStart", strattime);
                parms[1] = new SqlParameter("@CreatedTimeEnd", endtime);
                parms[2] = new SqlParameter("@StatusID", Status);
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
                string i = "0";
                if (list.合同状态 == "未开始")
                {
                    i = "0";
                }
                else if (list.合同状态.Equals("进行中"))
                {
                    i = "1";
                }
                else
                {
                    i = "3";
                }
                ViewData["id"] = list.id;
                ViewData["select"] = i;
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


        #endregion

        #region 工单管理
        //工单页面
        public ActionResult WorkManagement()
        {
            return View();
        }
        //显示工单信息
        public ActionResult CheckWorkManagement(string CName, string CNumber, string RepairOrder, string strattime, string endtime, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@CustomerName", CName);
                parms[1] = new SqlParameter("@ContractNumber", CNumber);
                parms[2] = new SqlParameter("@WorkorderNO", RepairOrder);
                parms[3] = new SqlParameter("@CreateTime", strattime);
                parms[4] = new SqlParameter("@CreateTimeTo", endtime);
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
            TempData["fanhui"] = id;
            return View();
        }
        //显示工单详细信息
        public ActionResult CheckRoDetail()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string returns = TempData["fanhui"].ToString();
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@WorkorderNO", returns);
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
            if (WorkorderNO != "")
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
            return null;
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
    }
}