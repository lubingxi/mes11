using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class AfterManagementController : Controller
    {
        // GET: AfterManagement
        public ActionResult Index()
        {
            return View();
        }
        #region 售后管理

        //售后安装
        public ActionResult Installation()
        {
            return View();
        }
        //显示售后安装
        public ActionResult CheckInstallation(string CName, string CNumber, string strattime, string endtime,int page,int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@ID", "");
                parms[1] = new SqlParameter("@CustomerName", CName);
                parms[2] = new SqlParameter("@ContractNumber", CNumber);
                parms[3] = new SqlParameter("@CreatedTimeStart", strattime);
                parms[4] = new SqlParameter("@CreatedTimeEnd", endtime);
                parms[5] = new SqlParameter("@StatusID", "");
                var list = ys.Database.SqlQuery<CheckSales_Result>("exec CheckSales @ID,@CustomerName,@ContractNumber,@CreatedTimeStart,@CreatedTimeEnd,@StatusID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<CheckSales_Result> pageList = new PageList<CheckSales_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //查看合同编号
        public ActionResult Contract_Check(string id)
        {
            //显示合同信息
            XianShiContract(id);
            ViewData["ContractDetialsCheckId"] = id;
            return View();
        }
        //安装情况页面
        public ActionResult ContractInstallation(string CName, string CNumber,string id)
        {
            ViewData["CName"] = CName;
            ViewData["CNumber"] = CNumber;
            Session["CName2"] = CName;
            Session["CNumber2"] = CNumber;
            ViewData["id"] = id;
            return View();
        }
        //显示安装详细信息
        public ActionResult CheckContractInstallation(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ContractID", i);
                var list = ys.Database.SqlQuery<Install_check_Result>("exec Install_check  @ContractID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //修改安装详细信息
        public ActionResult EditContractInstallation(string sn, string yan, string jan, string fish, string ifshou, string shoutime, string pid, string cid,string cn)
        {
            try
            {
                int YianNumber = int.Parse(yan);
                int JianNumber = int.Parse(jan);
                int HeNumber = int.Parse(cn);
                int SendNumber = int.Parse(sn);
                int poid = int.Parse(pid);
                int coid = int.Parse(cid);
                if (YianNumber + JianNumber > HeNumber)
               {
                    return Content("da");
                }
                else
                {
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@ContractID", coid);
                    parms[1] = new SqlParameter("@ProductDetailID", poid);
                    parms[2] = new SqlParameter("@newInstalledQuantity", jan);
                    parms[3] = new SqlParameter("@CompletionDate", fish);
                    parms[4] = new SqlParameter("@AcceptanceDate", shoutime);
                    parms[5] = new SqlParameter("@Acceptance", ifshou);
                    parms[6] = new SqlParameter("@CreatedBy", name);
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        ys.Database.ExecuteSqlCommand("exec  Install_update  @ContractID,@ProductDetailID,@newInstalledQuantity,@CompletionDate,@AcceptanceDate,@Acceptance,@CreatedBy", parms);
                    }
                    return Content("true");
               }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //安装历史记录页面
        public ActionResult InstallHistory(string tid, string cid, string cn, string cnb)
        {
            ViewData["cn"] = cn;
            ViewData["cnb"] = cnb;
            ViewData["tid"] = tid;
            ViewData["cid"] = cid;
            return View();
        }
        //显示安装历史记录
        public ActionResult CheckInstallHistory(string tid,string cid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int toid = int.Parse(tid);
                int coid = int.Parse(cid);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ContractID",toid);
                parms[1] = new SqlParameter("@ProductDetailID",coid);
                var list = ys.Database.SqlQuery<InstallHistory_check_Result>("exec InstallHistory_check  @ContractID,@ProductDetailID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //修改安装历史记录
        public ActionResult EditInstallHistory(string ifacce, string ifd, string cid, string pid, string hid, string fish, string sn, string yan, string jan)
        {
            try
            {
                int coid = int.Parse(cid);
                int poid = int.Parse(pid);
                int hoid = int.Parse(hid);
                int YianNumber = int.Parse(yan);
                int JianNumber = int.Parse(jan);
                int SendNumber = int.Parse(sn);
                if ((YianNumber + JianNumber) > SendNumber)
                {
                    return Content("<script>layer.msg('安装数量不能大于发货数量');</script>");
                }
                else
                {
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@newInstalledQuantity", jan);
                    parms[1] = new SqlParameter("@CompletionDate", fish);
                    parms[2] = new SqlParameter("@Acceptance", ifacce);
                    parms[3] = new SqlParameter("@AcceptanceDate", ifd);
                    parms[4] = new SqlParameter("@ContractID", coid);
                    parms[5] = new SqlParameter("@ProductDetailID", poid);
                    parms[6] = new SqlParameter("@historyID", hoid);
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        ys.Database.ExecuteSqlCommand("exec InstallHistory_Update  @newInstalledQuantity,@CompletionDate,@Acceptance,@AcceptanceDate,@ContractID,@ProductDetailID,@historyID", parms);
                    }
                    return Content("true");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //删除安装历史记录
        public ActionResult DeleteInstallHistory(string pid, string cid, string hid)
        {
            try
            {
                int coid = int.Parse(cid);
                int poid = int.Parse(pid);
                int hoid = int.Parse(hid);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@ContractID", coid);
                parms[1] = new SqlParameter("@ProductDetailID", poid);
                parms[2] = new SqlParameter("@historyID", hoid);
                using(YLMES_newEntities ys = new YLMES_newEntities()){
                    ys.Database.ExecuteSqlCommand("exec InstallHistory_delete  @ContractID,@ProductDetailID,@historyID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
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
    }
}