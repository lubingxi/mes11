
using PrintLib.Printers.Zebra;
using Spire.Xls;
using Spire.Xls.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YLMES;
using YLMES.Models;
using static System.Net.Mime.MediaTypeNames;

namespace YlMES.Controllers
{
    public class TaskManagementController : Controller
    {
        // GET: TaskManagement
        public ActionResult Index()
        {
            return View();
        }
        //任务分配视图
        public ActionResult TaskAllocation()
        {

            return View();
        }
        public ActionResult Contract_Check(string id)
        {
            //显示合同信息
            XianShiContract(id);
            ViewData["ContractDetialsCheckId"] = id;
            return View();
        }
        //技术部分配任务
        public ActionResult TechnicalPartAssignment(string customerName)
        {
            Session["cn"] = customerName;
            Session["pndd"] = customerName;
            return View();
        }
        //我的任务
        public ActionResult MyTask()
        {

            return View();
        }
        //已完成的任务视图
        public ActionResult CompletedTasks()
        {
            return View();
        }

        #region 下载模版
        public ActionResult FileADownload(string file1)
        {
            if (string.IsNullOrEmpty(file1))
            {
                return Content("<script>alert('没有文件哦!');history.go(-1);</script>");
            }
            else
            {
                string filePath = Server.MapPath(file1);
                return File(filePath, "~/Upload", file1);
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
                return File(filePath, "text/plain", file2);
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
                return File(filePath, "text/plain", file3);
            }

        }
        public ActionResult DownloadProcess(string RouteName)
        {
            string rn = RouteName + ".xls";
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (RouteName=="0" || RouteName=="")
                {
                    return Content("<script>alert('请选择流程!');history.go(-1);</script>");
                }                        
                else
                {
                    PM_Route pr = ys.PM_Route.Where(r => r.RouteName == RouteName).FirstOrDefault();
                    string prodo = pr.ProcessDocument;
                    if (string.IsNullOrEmpty(prodo))
                    {
                        return Content("<script>alert('没有文件哦!');history.go(-1);</script>");
                    }
                    else
                    {
                        string filePath = Server.MapPath(prodo);
                        return File(filePath, "application/ms-excel",rn);
                    }                    
                }
            }        
        }
        #endregion

        #region 设置状态完成

        //设计部确认机械设计分配
        public ActionResult UpdateMachineDesignConfirm(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "Machine");
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);

                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        //设计部确认电气设计分配
        public ActionResult UpdateElectricalDesignConfirm(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "Electrical");
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //审核分配
        public ActionResult UpdateQualifiedConfirm(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "Qualified");
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //确认电气审核分配
        public ActionResult UpdateElectricalAudit(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "Elec");
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //图纸发行分配
        public ActionResult UpdateIssueConfirm(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "Issue");
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //图纸发行
        public ActionResult UpdateIssueCompleted(string Taskid,string cnumber)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "IssueCompleted");
                    int i = ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
                    if (i == 0)
                    {
                        return Content("null");
                    }else if (i > 0)
                    {
                        string name = Session["name"].ToString();
                        Sms.InsertSmsInfos("梁树银",cnumber, name);
                    }
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //图纸接收
        public ActionResult UpdateAcceptDesignFile(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Type", "AcceptDesignFile");
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //设计部完成电气设计
        public ActionResult FinishElectricalDesign(string taskid,string PartNumber,string ProductSpec)
        {
            ViewData["taskid"] = taskid;
            ViewData["Pnumber"] = PartNumber;
            ViewData["PSpec"] = ProductSpec;
            return View();
        }

        #endregion

        #region 显示插入评分

        public ActionResult TaskScoring(string ifDrive)
        {
            return View();
        }
        public ActionResult ApprovalRatings(string Score, string SuggestedPoints)
        {
            try
            {
                int s = int.Parse(Score);
                if (s < 0 || s > 100)
                {
                    return Content("da");
                }
                string TaskId = Session["taid"].ToString();
                string type = Session["type"].ToString();
                int i = int.Parse(TaskId);
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@TaskID", i);
                if (type == "机械设计")
                {
                    parms[1] = new SqlParameter("@MachineDesignScore", Score);
                }
                else
                {
                    parms[1] = new SqlParameter("@MachineDesignScore", "");
                }
                if (type == "电气设计")
                {
                    parms[2] = new SqlParameter("@electricalDesignScore", Score);
                }
                else
                {
                    parms[2] = new SqlParameter("@electricalDesignScore", "");
                }
                parms[3] = new SqlParameter("@SuggestedPoints", SuggestedPoints);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec updateQualifiedCompleted  @TaskID,@MachineDesignScore,@electricalDesignScore,@SuggestedPoints", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("fales");
            }
        }

        #endregion

        #region 复制订单
        public ContentResult AddPoDetials(string id, string cid, string pname)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int pid = int.Parse(id);
                int coid = int.Parse(cid);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@ContractID", coid);
                parms[1] = new SqlParameter("@Pid", pid);
                parms[2] = new SqlParameter("@PojectNames", pname);
                int i = ys.Database.ExecuteSqlCommand("exec AddPoDetials @ContractID,@Pid,@PojectNames", parms);
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
        #endregion

        #region 删除订单

        public ContentResult DeletePoDetials(string id, string pid)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int ci = int.Parse(id);
                    int pi = int.Parse(pid);
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@cid", ci);
                    parms[1] = new SqlParameter("@pid", pi);
                    int i = ys.Database.ExecuteSqlCommand("exec DeletePoDetials @cid,@pid", parms);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        #endregion

        #region 显示完成我的设计

        public ActionResult FinishMyDesign(string ifDrive, string name, string ProjectName, string taskid)
        {
            ViewData["taskid"] = taskid;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                var list = ys.Database.SqlQuery<checkBOM_Result>("exec checkBOM  @TaskID", parms).ToList();
                if (list.Count == 0)
                {
                    ViewData["dao"] = "0";
                }
                else
                {
                    ViewData["dao"] = "2";
                }
            }
            Session["ifD"] = ifDrive;
            Session["ifB"] = ifDrive;
            Session["nam"] = name;
            Session["type"] = name;
            return View();
        }

        #endregion

        #region 显示任务名称
        public ActionResult NameTask(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Session["id"] = id;
                Session["taid"] = id;
                Session["Delete"] = id;
                Session["Check"] = id;
                Session["Finish"] = id;
                Session["Projess"] = id;
                Session["UTP"] = id;
                ViewData["ProcessFlow"] = id;
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", i);
                var list = ys.Database.SqlQuery<TaskCheck_Result>("exec TaskCheck @TaskID", parms).FirstOrDefault();
                ViewData["Status"] = list.任务状态;
                ViewData["TaskID"] = list.任务编号;
                ViewData["TaskID2"] = list.任务编号;
                Session["TaskID3"] = list.任务编号;
                ViewData["CustomerName"] = list.客户名称;
                ViewData["CustomerNames"] = list.客户名称;
                ViewData["CustomerNamesv"] = list.客户名称;
                ViewData["ContractNumber"] = list.项目名称;
                ViewData["ContractNumbers"] = list.项目名称;
                ViewData["ProductName"] = list.产品名称;
                Session["ProductNamed"] = list.产品名称;
                ViewData["TaskName"] = list.任务名称;
                ViewData["ProductSpec"] = list.产品规格;
                Session["ProductSpeces"] = list.产品规格;
                ViewData["ProductSpecd"] = list.产品规格;
                ViewData["tasktype"] = list.设计类型;
                var Status = ys.PM_TaskRoute.Where(p => p.TaskCurrentStatus.Equals(list.任务状态)).FirstOrDefault();
                ViewData["ifDrive"] = Status.TaskCurrentContent;
                ViewData["ifDrives"] = Status.TaskCurrentContent;
                ViewData["name"] = list.任务类型;
                ViewData["names"] = list.任务类型;
                ViewData["SendXing"] = list.任务类型;
                Session["leixing"] = list.任务类型;
                ViewData["PriorityCode"] = list.紧急程度;
                ViewData["taskdesc"] = list.任务描述;
                ViewData["pcs"] = list.数量;
                ViewData["file1"] = list.任务附件1;
                ViewData["file2"] = list.任务附件2;
                ViewData["file3"] = list.任务附件3;
                ViewData["JieDate"] = list.截止日期;
                Session["type"] = list.设计类型;
                Session["type2"] = list.设计类型;
                if (list.设计类型 == "电气设计")
                {
                    ViewData["Owner"] = list.电气设计人;
                    ViewData["BaseScore"] = list.电气设计分值;
                    ViewData["BaseTime"] = list.电气设计基准时间;
                    ViewData["ConfirmTime"] = list.电气设计确认时间;
                    ViewData["CompletedTime"] = list.电气设计完成时间;
                    ViewData["OverDue"] = list.电气设计是否逾期;
                }
                else
                {
                    ViewData["Owner"] = list.机械设计人;
                    ViewData["BaseScore"] = list.机械设计分值;
                    ViewData["BaseTime"] = list.机械设计基准时间;
                    ViewData["ConfirmTime"] = list.机械设计确认时间;
                    ViewData["CompletedTime"] = list.机械设计完成时间;
                    ViewData["OverDue"] = list.机械设计是否逾期;
                }
                ViewData["QualifiedOwner"] = list.审核人;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["QualifiedConfirmTime"] = list.审核确认时间;
                ViewData["QualifiedCompletedTime"] = list.审核完成时间;
                ViewData["QualifiedOverDue"] = list.审核是否逾期;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
                ViewData["IssueConfirmTime"] = list.发行确认时间;
                ViewData["IssueCompletedTime"] = list.发行完成时间;
                ViewData["IssueOverDue"] = list.发行是否逾期;
                ViewData["CarryingCapacity"] = list.载重;
                ViewData["PEOwner"] = list.工艺负责人;
                ViewData["PEBaseTime"] = list.工艺基准时间;
                ViewData["PEConfirmTime"] = list.工艺确认时间;
                ViewData["PECompletedTime"] = list.工艺完成时间;
                ViewData["PurchaseOwner"] = list.采购负责人;
                ViewData["PurchaseBaseTime"] = list.采购基准时间;
                ViewData["PurchaseConfirmTime"] = list.采购确认时间;
                ViewData["PurchaseCompletedTime"] = list.采购完成时间;
                ViewData["PMCOwner"] = list.计划负责人;
                ViewData["PMCBaseTime"] = list.计划基准时间;
                ViewData["PMCConfirmTime"] = list.计划确认时间;
                ViewData["PMCCompletedTime"] = list.计划完成时间;
                ViewData["ProductOwner"] = list.采购负责人;
                ViewData["ProductBaseTime"] = list.采购基准时间;
                ViewData["ProductConfirmTime"] = list.采购确认时间;
                ViewData["ProductCompletedTime"] = list.采购完成时间;
                ViewData["WHOwner"] = list.仓库负责人;
                ViewData["WHDeliveryBasetime"] = list.仓库基准时间;
                ViewData["WHConfirmTime"] = list.仓库确认时间;
                ViewData["WHCompletedTime"] = list.仓库完成时间;
                ViewData["InstallOwner"] = list.售后负责人;
                ViewData["InstallBasetime"] = list.售后基准时间;
                ViewData["InstallConfirmTime"] = list.售后确认时间;
                ViewData["InstallCompletedTime"] = list.售后完成时间;
            }
            return View();
        }
        public ActionResult NameTasks(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", i);
                var list = ys.Database.SqlQuery<TaskCheck_Result>("exec TaskCheck @TaskID", parms).FirstOrDefault();
                ViewData["Status"] = list.任务状态;
                ViewData["TaskID"] = list.任务编号;
                ViewData["TaskID2"] = list.任务编号;
                Session["TaskID3"] = list.任务编号;
                ViewData["CustomerName"] = list.客户名称;
                ViewData["CustomerNames"] = list.客户名称;
                ViewData["CustomerNamesv"] = list.客户名称;
                ViewData["ContractNumber"] = list.项目名称;
                ViewData["ProductName"] = list.产品名称;
                ViewData["ProductNamed"] = list.产品名称;
                ViewData["TaskName"] = list.任务名称;
                ViewData["ProductSpec"] = list.产品规格;
                ViewData["ProductSpecd"] = list.产品规格;
                var Status = ys.PM_TaskRoute.Where(p => p.TaskCurrentStatus.Equals(list.任务状态)).FirstOrDefault();
                ViewData["ifDrive"] = Status.TaskCurrentContent;
                ViewData["ifDrives"] = Status.TaskCurrentContent;
                ViewData["name"] = list.任务类型;
                ViewData["names"] = list.任务类型;
                ViewData["SendXing"] = list.任务类型;
                Session["leixing"] = list.任务类型;
                ViewData["PriorityCode"] = list.紧急程度;
                ViewData["taskdesc"] = list.任务描述;
                ViewData["file1"] = list.任务附件1;
                ViewData["file2"] = list.任务附件2;
                ViewData["file3"] = list.任务附件3;
                ViewData["JieDate"] = list.截止日期;
                Session["type"] = list.设计类型;
                Session["type2"] = list.设计类型;
                if (list.设计类型 == "电气设计")
                {
                    ViewData["Owner"] = list.电气设计人;
                    ViewData["BaseScore"] = list.电气设计分值;
                    ViewData["BaseTime"] = list.电气设计基准时间;
                    ViewData["ConfirmTime"] = list.电气设计确认时间;
                    ViewData["CompletedTime"] = list.电气设计完成时间;
                    ViewData["OverDue"] = list.电气设计是否逾期;
                }
                else
                {
                    ViewData["Owner"] = list.机械设计人;
                    ViewData["BaseScore"] = list.机械设计分值;
                    ViewData["BaseTime"] = list.机械设计基准时间;
                    ViewData["ConfirmTime"] = list.机械设计确认时间;
                    ViewData["CompletedTime"] = list.机械设计完成时间;
                    ViewData["OverDue"] = list.机械设计是否逾期;
                }
                ViewData["QualifiedOwner"] = list.审核人;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["QualifiedConfirmTime"] = list.审核确认时间;
                ViewData["QualifiedCompletedTime"] = list.审核完成时间;
                ViewData["QualifiedOverDue"] = list.审核是否逾期;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
                ViewData["IssueConfirmTime"] = list.发行确认时间;
                ViewData["IssueCompletedTime"] = list.发行完成时间;
                ViewData["IssueOverDue"] = list.发行是否逾期;
                ViewData["CarryingCapacity"] = list.载重;
                ViewData["PEOwner"] = list.工艺负责人;
                ViewData["PEBaseTime"] = list.工艺基准时间;
                ViewData["PEConfirmTime"] = list.工艺确认时间;
                ViewData["PECompletedTime"] = list.工艺完成时间;
                ViewData["PurchaseOwner"] = list.采购负责人;
                ViewData["PurchaseBaseTime"] = list.采购基准时间;
                ViewData["PurchaseConfirmTime"] = list.采购确认时间;
                ViewData["PurchaseCompletedTime"] = list.采购完成时间;
                ViewData["PMCOwner"] = list.计划负责人;
                ViewData["PMCBaseTime"] = list.计划基准时间;
                ViewData["PMCConfirmTime"] = list.计划确认时间;
                ViewData["PMCCompletedTime"] = list.计划完成时间;
                ViewData["ProductOwner"] = list.采购负责人;
                ViewData["ProductBaseTime"] = list.采购基准时间;
                ViewData["ProductConfirmTime"] = list.采购确认时间;
                ViewData["ProductCompletedTime"] = list.采购完成时间;
                ViewData["WHOwner"] = list.仓库负责人;
                ViewData["WHDeliveryBasetime"] = list.仓库基准时间;
                ViewData["WHConfirmTime"] = list.仓库确认时间;
                ViewData["WHCompletedTime"] = list.仓库完成时间;
                ViewData["InstallOwner"] = list.售后负责人;
                ViewData["InstallBasetime"] = list.售后基准时间;
                ViewData["InstallConfirmTime"] = list.售后确认时间;
                ViewData["InstallCompletedTime"] = list.售后完成时间;
            }
            return View();
        }
        public ActionResult NameTask2(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Session["id"] = id;
                Session["taid"] = id;
                Session["Delete"] = id;
                Session["Check"] = id;
                Session["Finish"] = id;
                Session["Projess"] = id;
                Session["UTP"] = id;
                ViewData["ProcessFlow"] = id;
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", i);
                var list = ys.Database.SqlQuery<TaskCheck_Result>("exec TaskCheck @TaskID", parms).FirstOrDefault();
                ViewData["Status"] = list.任务状态;
                ViewData["TaskID"] = list.任务编号;
                ViewData["TaskID2"] = list.任务编号;
                Session["TaskID3"] = list.任务编号;
                ViewData["CustomerName"] = list.客户名称;
                ViewData["CustomerNames"] = list.客户名称;
                ViewData["CustomerNamesv"] = list.客户名称;
                ViewData["ContractNumber"] = list.项目名称;
                ViewData["ProductName"] = list.产品名称;
                ViewData["ProductNamed"] = list.产品名称;
                ViewData["TaskName"] = list.任务名称;
                ViewData["ProductSpec"] = list.产品规格;
                ViewData["ProductSpecd"] = list.产品规格;
                var Status = ys.PM_TaskRoute.Where(p => p.TaskCurrentStatus.Equals(list.任务状态)).FirstOrDefault();
                ViewData["ifDrive"] = Status.TaskCurrentContent;
                ViewData["ifDrives"] = Status.TaskCurrentContent;
                ViewData["name"] = list.任务类型;
                ViewData["names"] = list.任务类型;
                ViewData["SendXing"] = list.任务类型;
                Session["leixing"] = list.任务类型;
                ViewData["PriorityCode"] = list.紧急程度;
                ViewData["taskdesc"] = list.任务描述;
                ViewData["file1"] = list.任务附件1;
                ViewData["file2"] = list.任务附件2;
                ViewData["file3"] = list.任务附件3;
                ViewData["JieDate"] = list.截止日期;
                Session["type"] = list.设计类型;
                Session["type2"] = list.设计类型;
                if (list.设计类型 == "电气设计")
                {
                    ViewData["Owner"] = list.电气设计人;
                    ViewData["BaseScore"] = list.电气设计分值;
                    ViewData["BaseTime"] = list.电气设计基准时间;
                    ViewData["ConfirmTime"] = list.电气设计确认时间;
                    ViewData["CompletedTime"] = list.电气设计完成时间;
                    ViewData["OverDue"] = list.电气设计是否逾期;
                }
                else
                {
                    ViewData["Owner"] = list.机械设计人;
                    ViewData["BaseScore"] = list.机械设计分值;
                    ViewData["BaseTime"] = list.机械设计基准时间;
                    ViewData["ConfirmTime"] = list.机械设计确认时间;
                    ViewData["CompletedTime"] = list.机械设计完成时间;
                    ViewData["OverDue"] = list.机械设计是否逾期;
                }
                ViewData["QualifiedOwner"] = list.审核人;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["QualifiedConfirmTime"] = list.审核确认时间;
                ViewData["QualifiedCompletedTime"] = list.审核完成时间;
                ViewData["QualifiedOverDue"] = list.审核是否逾期;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
                ViewData["IssueConfirmTime"] = list.发行确认时间;
                ViewData["IssueCompletedTime"] = list.发行完成时间;
                ViewData["IssueOverDue"] = list.发行是否逾期;
                ViewData["CarryingCapacity"] = list.载重;
                ViewData["PEOwner"] = list.工艺负责人;
                ViewData["PEBaseTime"] = list.工艺基准时间;
                ViewData["PEConfirmTime"] = list.工艺确认时间;
                ViewData["PECompletedTime"] = list.工艺完成时间;
                ViewData["PurchaseOwner"] = list.采购负责人;
                ViewData["PurchaseBaseTime"] = list.采购基准时间;
                ViewData["PurchaseConfirmTime"] = list.采购确认时间;
                ViewData["PurchaseCompletedTime"] = list.采购完成时间;
                ViewData["PMCOwner"] = list.计划负责人;
                ViewData["PMCBaseTime"] = list.计划基准时间;
                ViewData["PMCConfirmTime"] = list.计划确认时间;
                ViewData["PMCCompletedTime"] = list.计划完成时间;
                ViewData["ProductOwner"] = list.采购负责人;
                ViewData["ProductBaseTime"] = list.采购基准时间;
                ViewData["ProductConfirmTime"] = list.采购确认时间;
                ViewData["ProductCompletedTime"] = list.采购完成时间;
                ViewData["WHOwner"] = list.仓库负责人;
                ViewData["WHDeliveryBasetime"] = list.仓库基准时间;
                ViewData["WHConfirmTime"] = list.仓库确认时间;
                ViewData["WHCompletedTime"] = list.仓库完成时间;
                ViewData["InstallOwner"] = list.售后负责人;
                ViewData["InstallBasetime"] = list.售后基准时间;
                ViewData["InstallConfirmTime"] = list.售后确认时间;
                ViewData["InstallCompletedTime"] = list.售后完成时间;
            }
            return View();
        }
        #endregion

        #region 显示我的任务信息

        public JsonResult TaskDisplay(string Status, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                string name = Session["name"].ToString();
                parms[0] = new SqlParameter("@Owner", name);
                parms[1] = new SqlParameter("@Status", Status);
                var list = ys.Database.SqlQuery<MyTaskCheck_Result>("exec MyTaskCheck @Owner,@Status", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<MyTaskCheck_Result> pageList = new PageList<MyTaskCheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 我要更多任务
        //更多任务页面
        public ActionResult MoreTask()
        {
            return View();
        }
        //显示更多任务
        public ActionResult IwantMoreTask(int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.Database.SqlQuery<IwantMoreTask_Result>("exec IwantMoreTask").ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<IwantMoreTask_Result> pageList = new PageList<IwantMoreTask_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //显示采购更多任务
        public ActionResult ProMoreTask(int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.Database.SqlQuery<ProcurementMoreTask_Result>("exec ProcurementMoreTask").ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<ProcurementMoreTask_Result> pageList = new PageList<ProcurementMoreTask_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //插入设计任务
        public ActionResult UpdateTaskForChangeOwner(string task, string stus)
        {
            bool check = stus.Contains("设计");
            if (check)
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@Owner", name);
                    parms[1] = new SqlParameter("@TaskID", task);
                    ys.Database.ExecuteSqlCommand("exec UpdateTaskForChangeOwner  @Owner,@TaskID", parms);
                    return Content("true");
                }
            }
            else
            {
                return Content("pei");
            }
        }
        //插入采购任务分配
        public ActionResult UpdateTaskForChangeOwners(string task)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Owner", name);
                parms[1] = new SqlParameter("@TaskID", task);
                ys.Database.ExecuteSqlCommand("exec UpdateTaskForChangeOwners  @Owner,@TaskID", parms);
                return Content("true");
            }
        }
        //插入审核任务
        public ActionResult updateTFCQO(string task, string stus)
        {
            string name = Session["name"].ToString();
            bool check = stus.Contains("审核");
            if (check)
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@Owner", name);
                    parms[1] = new SqlParameter("@TaskID", task);
                    ys.Database.ExecuteSqlCommand("exec TaskForChangeQualifiedOwner  @Owner,@TaskID", parms);
                    return Content("true");
                }
            }
            else
            {
                return Content("pei");
            }
        }
        //插入图纸发行任务
        public ActionResult UpdateTFCIO(string task, string stus)
        {
            string name = Session["name"].ToString();
            bool check = stus.Contains("图纸");
            if (check)
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@Owner", name);
                    parms[1] = new SqlParameter("@TaskID", task);
                    ys.Database.ExecuteSqlCommand("exec UpdateTaskForChangeIssueOwner  @Owner,@TaskID", parms);
                    return Content("true");
                }
            }
            else
            {
                return Content("pei");
            }
        }

        #endregion

        #region 显示机械任务

        public ActionResult EditingMechanicalTasks(string TaskID)
        {
            int Tid = int.Parse(TaskID);
            Session["taskid"] = TaskID;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", Tid);
                var list = ys.Database.SqlQuery<TaskCheck_Result>("exec TaskCheck  @TaskID", parms).FirstOrDefault();
                ViewData["PojectName"] = list.项目名称;
                ViewData["Status"] = list.任务状态;
                ViewData["ifDrive"] = list.设计类型;
                ViewData["select"] = list.任务类型;
                ViewData["TaskName"] = list.任务名称;
                ViewData["PriorityCode"] = list.紧急程度;
                ViewData["ProductSpec"] = list.产品规格;
                ViewData["ProductName"] = list.产品名称;
                ViewData["taskdesc"] = list.任务描述;
                ViewData["JieDate"] = list.截止日期;
                ViewData["file1"] = list.任务附件1;
                ViewData["file2"] = list.任务附件2;
                ViewData["file3"] = list.任务附件3;
                ViewData["MachineDesignOwner"] = list.机械设计人;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["MachineDesignBaseScore"] = list.机械设计分值;
                ViewData["MachineDesignBaseTime"] = list.机械设计基准时间;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
                ViewData["Count"] = list.数量;
                ViewData["Units"] = list.单位;
                ViewData["BracketFoot"] = list.共用支架脚;
                List<UserName_Result> list1 = ys.Database.SqlQuery<UserName_Result>("exec UserName").ToList();
                List<SelectListItem> it = new List<SelectListItem>();
                foreach (var t in list1)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.UserName, Text = t.UserName };
                    if (t.UserName == list.审核人)
                    {
                        itemselectLanguage.Selected = true;
                        ViewData["shenhe"] = itemselectLanguage.Value;
                    }
                    it.Add(itemselectLanguage);
                }
                if (ViewData["shenhe"] == "")
                {
                    ViewData["shenhe"] = list1[0].UserName;
                }
                ViewData["selects"] = it;

                List<UserName_Result> list2 = ys.Database.SqlQuery<UserName_Result>("exec UserName").ToList();
                List<SelectListItem> its = new List<SelectListItem>();
                foreach (var t in list2)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.UserName, Text = t.UserName };
                    if (t.UserName == list.机械设计人)
                    {
                        itemselectLanguage.Selected = true;
                        ViewData["jixie"] = list.机械设计人;
                    }
                    its.Add(itemselectLanguage);
                }
                if (ViewData["jixie"] == "")
                {
                    ViewData["jixie"] = list2[0].UserName;
                }
                ViewData["selectd"] = its;  
            }
            return View();
        }


        #endregion

        #region 显示电气任务
        public ActionResult EditingElectricalTasks(string TaskID)
        {
            int Tid = int.Parse(TaskID);
            Session["taskid"] = TaskID;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", Tid);
                var list = ys.Database.SqlQuery<TaskCheck_Result>("exec TaskCheck  @TaskID", parms).FirstOrDefault();
                ViewData["PojectName"] = list.项目名称;
                ViewData["Status"] = list.任务状态;
                ViewData["ifDrive"] = list.设计类型;
                ViewData["select"] = list.任务类型;
                ViewData["TaskName"] = list.任务名称;
                ViewData["PriorityCode"] = list.紧急程度;
                ViewData["ProductSpec"] = list.产品规格;
                ViewData["ProductName"] = list.产品名称;
                ViewData["taskdesc"] = list.任务描述;
                ViewData["JieDate"] = list.截止日期;
                ViewData["file1"] = list.任务附件1;
                ViewData["file2"] = list.任务附件2;
                ViewData["file3"] = list.任务附件3;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["MachineDesignBaseScore"] = list.电气设计分值;
                ViewData["MachineDesignBaseTime"] = list.电气设计基准时间;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
                ViewData["Count"] = list.数量;
                ViewData["Units"] = list.单位;

                List<UserName_Result> list1 = ys.Database.SqlQuery<UserName_Result>("exec UserName").ToList();
                List<SelectListItem> it = new List<SelectListItem>();
                foreach (var t in list1)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.UserName, Text = t.UserName };
                    if (t.UserName == list.审核人)
                    {
                        itemselectLanguage.Selected = true;
                        ViewData["shenhe"] = itemselectLanguage.Value;
                    }
                    it.Add(itemselectLanguage);
                }
                if (ViewData["shenhe"] == "")
                {
                    ViewData["shenhe"] = list1[0].UserName;
                }
                ViewData["selects"] = it;

                List<UserName_Result> list2 = ys.Database.SqlQuery<UserName_Result>("exec UserName").ToList();
                List<SelectListItem> its = new List<SelectListItem>();
                foreach (var t in list2)
                {
                    var itemselectLanguage = new SelectListItem { Value = t.UserName, Text = t.UserName };
                    if (t.UserName == list.电气设计人)
                    {
                        itemselectLanguage.Selected = true;
                        ViewData["dianqi"] = list.电气设计人;
                    }
                    its.Add(itemselectLanguage);
                }
                if (ViewData["dianqi"] == "")
                {
                    ViewData["dianqi"] = list2[0].UserName;
                }
                ViewData["selectd"] = its;
            }
            return View();
        }
        #endregion

        #region 下载文件

        public FileResult Filed(string file1)
        {
            string filePath = Server.MapPath(file1);
            return File(filePath, "text/plain", file1.Substring(file1.LastIndexOf('/') + 1));
        }

        #endregion

        #region 批量上传

        //显示页面
        public ActionResult BatchUpload(string taskid)
        {
            ViewData["taskid"] = taskid;
            return View();
        }
        //上传操作
        public ActionResult PerformBatchUpload(string taskid, HttpPostedFileBase file)
        {
            string FigureNumber = "";
            Nullable<int> Materid = 0;
            try
            {
                using (YLMES_newEntities yd = new YLMES_newEntities())
                {
                    string fileName2 = file.FileName;
                    string fnamew = fileName2.Substring(fileName2.LastIndexOf('_') + 1);
                    string fnamed = fnamew.Substring(0, fnamew.LastIndexOf('.'));
                    SqlParameter[] parmd = new SqlParameter[2];
                    parmd[0] = new SqlParameter("@PartNumber", fnamed);
                    parmd[1] = new SqlParameter("@taskid", taskid);
                    var list = yd.Database.SqlQuery<PM_PartSettingCheck_Result>("exec PM_PartSettingCheck @PartNumber,@taskid", parmd).FirstOrDefault();
                    FigureNumber = list.FigureNumber;
                    Materid = list.MatertID;
                    //查出PM_PartSetting_temp表字段MatertID和FigureNumber
                }
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var fileName1 = Path.Combine(Request.MapPath("~/pdf"), Path.GetFileName(file.FileName));
                    file.SaveAs(fileName1);
                    //上传到pdf文件夹                 

                    int i = int.Parse(taskid);
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[6];
                    parms[0] = new SqlParameter("@Type", "Uploaddrawings");
                    parms[1] = new SqlParameter("@FigureNumber", FigureNumber);
                    parms[2] = new SqlParameter("@FolderName", "Upload");
                    parms[3] = new SqlParameter("@FileName", file.FileName);
                    parms[4] = new SqlParameter("@CreatedBy", name);
                    parms[5] = new SqlParameter("@MarterID", Materid);
                    ys.Database.ExecuteSqlCommand("exec UploadTheDrawings  @Type,@FigureNumber,@FolderName,@FileName,@CreatedBy,@MarterID", parms);
                    //上传UploadTheDrawings存储过程
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Dictionary<string, Object> hasmas = new Dictionary<string, Object>();
                hasmas.Add("code", 1);
                return Json(hasmas, JsonRequestBehavior.AllowGet);
            }
            Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
            hasmap.Add("code", 0);
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 完成收款财务页面

        public ActionResult FinishTask(string name)
        {
            return View();
        }

        public ActionResult Get_Datasd(int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "checksdd");
                parms[1] = new SqlParameter("@CustomerName", "");
                parms[2] = new SqlParameter("@ContractNumber", "");
                parms[3] = new SqlParameter("@StatusID", "");
                parms[4] = new SqlParameter("@CreatedTimeStart", "");
                parms[5] = new SqlParameter("@CreatedTimeEnd", "");
                parms[6] = new SqlParameter("@ReviewStatus", "");
                var list = ys.Database.SqlQuery<SP_ContractEdit_Result>("exec SP_ContractEdit @type,'',@CustomerName,@ContractNumber,'',1.00,'','','','','','','','',@StatusID,@CreatedTimeStart,@CreatedTimeEnd,'8.88','',@ReviewStatus", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_ContractEdit_Result> pageList = new PageList<SP_ContractEdit_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 显示已完成任务信息

        public JsonResult CompletedTasksJson(string StartTime, string endtime, string ProjectName, string TaskName, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Owner", name);
                parms[1] = new SqlParameter("@Status", "");
                parms[2] = new SqlParameter("@StartTime", StartTime);
                parms[3] = new SqlParameter("@EndTime", endtime);
                parms[4] = new SqlParameter("@ProjectName", ProjectName);
                parms[5] = new SqlParameter("@TaskName", TaskName);
                var list = ys.Database.SqlQuery<MyCompletedTaskCheck_Result>("exec MyCompletedTaskCheck @Owner,@Status,@StartTime,@EndTime,@ProjectName,@TaskName", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<MyCompletedTaskCheck_Result> pageList = new PageList<MyCompletedTaskCheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CompletedTasksJsons(string StartTime, string endtime, string ProjectName, string TaskName, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Owner", name);
                parms[1] = new SqlParameter("@Status", "");
                parms[2] = new SqlParameter("@StartTime", StartTime);
                parms[3] = new SqlParameter("@EndTime", endtime);
                parms[4] = new SqlParameter("@ProjectName", ProjectName);
                parms[5] = new SqlParameter("@TaskName", TaskName);
                var list = ys.Database.SqlQuery<ProCompletedTaskCheck_Result>("exec ProCompletedTaskCheck @Owner,@Status,@StartTime,@EndTime,@ProjectName,@TaskName", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<ProCompletedTaskCheck_Result> pageList = new PageList<ProCompletedTaskCheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 显示分配任务信息

        public JsonResult TpA(string DistriBution)
        {
            string cn = Session["cn"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", DistriBution);
                parms[1] = new SqlParameter("@ProjectName", cn);
                var list = ys.Database.SqlQuery<PM_ProjectCheckTask2_Result>("exec PM_ProjectCheckTask2  @Type,@ProjectName", parms).ToList();
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

        #region 分页显示任务分配

        public JsonResult TaskJson(string RwapStatusID, string CreatedTimeStart, string SalesOrderd, string ProjectName, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@RwapStatusID", RwapStatusID);
                parms[1] = new SqlParameter("@CreatedTimeStart", CreatedTimeStart);
                parms[2] = new SqlParameter("@SalesOrder", SalesOrderd);
                parms[3] = new SqlParameter("@ProjectName", ProjectName);
                parms[4] = new SqlParameter("@StatusID", "");
                var list = ys.Database.SqlQuery<PM_AssignTasksCheck_Result>("exec PM_AssignTasksCheck @RwapStatusID,@CreatedTimeStart,@SalesOrder,@ProjectName,@StatusID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<PM_AssignTasksCheck_Result> pageList = new PageList<PM_AssignTasksCheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 提交计划部
        public ActionResult SubmitPlans(string taskid)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int di = int.Parse(taskid);
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter("@TaskID", di); 
                ys.Database.ExecuteSqlCommand("exec AddPlanMater  @TaskID", parm);
                return Content("true");
            }
        }

        #endregion


        #region 编辑机械和电气任务分配

        public ActionResult EditMachine(string PojectName, string PriorityCode, string TaskName, string select, string ProductName, string ProductSpec, string JieDate, string taskdesc, string MachineDesignOwner, string MachineDesignBaseTime, string MachineDesignBaseScore, string QualifiedOwner, string QualifiedBaseTime, string QualifiedBaseScore, string IssueOwner, string IssueBaseTime, string IssueBaseScore, string fileDownA, string fileDownB, string fileDownC, string Count, string Units, string BracketFoot)
        {
            try
            {
                string takid = Session["taskid"].ToString();
                int tk = int.Parse(takid);
                if (select == null)
                {
                    select = "";
                }
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[26];
                    parms[0] = new SqlParameter("@TaskID", tk);
                    parms[1] = new SqlParameter("@PriorityCode", PriorityCode);
                    parms[2] = new SqlParameter("@TaskName", TaskName);
                    parms[3] = new SqlParameter("@ProductName", ProductName);
                    parms[4] = new SqlParameter("@ProductSpec", ProductSpec);
                    parms[5] = new SqlParameter("@Owner", "");
                    parms[6] = new SqlParameter("@TaskScore", 10.00);
                    parms[7] = new SqlParameter("@DueDay", JieDate);
                    parms[8] = new SqlParameter("@TaskDesc", taskdesc);
                    parms[9] = new SqlParameter("@MachineDesignOwner", MachineDesignOwner);
                    parms[10] = new SqlParameter("@MachineDesignBaseTime", MachineDesignBaseTime);
                    parms[11] = new SqlParameter("@MachineDesignBaseScore", MachineDesignBaseScore);
                    parms[12] = new SqlParameter("@QualifiedOwner", QualifiedOwner);
                    parms[13] = new SqlParameter("@QualifiedBaseTime", QualifiedBaseTime);
                    parms[14] = new SqlParameter("@QualifiedBaseScore", QualifiedBaseScore);
                    parms[15] = new SqlParameter("@IssueOwner", IssueOwner);
                    parms[16] = new SqlParameter("@IssueBaseTime", IssueBaseTime);
                    parms[17] = new SqlParameter("@IssueBaseScore", IssueBaseScore);
                    parms[18] = new SqlParameter("@Note", "");
                    parms[19] = new SqlParameter("@electricalOwner", MachineDesignOwner);
                    parms[20] = new SqlParameter("@electricalDesignBaseTime", MachineDesignBaseTime);
                    parms[21] = new SqlParameter("@electricalDesignBaseScore", MachineDesignBaseScore);
                    parms[22] = new SqlParameter("@TaskType", select);
                    parms[23] = new SqlParameter("@PCS", Count);
                    parms[24] = new SqlParameter("@Units", Units);
                    parms[25] = new SqlParameter("@BracketFoot", BracketFoot);
                    int i=ys.Database.ExecuteSqlCommand("exec updatelevel2Task  @TaskID,@PriorityCode,@TaskName,@ProductName,@ProductSpec,@Owner,@TaskScore,@DueDay,@TaskDesc,@MachineDesignOwner,@MachineDesignBaseTime," +
                        "@MachineDesignBaseScore,@QualifiedOwner,@QualifiedBaseTime,@QualifiedBaseScore,@IssueOwner,@IssueBaseTime,@IssueBaseScore,@Note,@electricalOwner,@electricalDesignBaseTime,@electricalDesignBaseScore,@TaskType,@PCS,@Units,@BracketFoot", parms);
                    //if (i > 0)
                    //{
                        return Content("true");
                        //string name = Session["name"].ToString();
                        //if ((MachineDesignOwner != null || MachineDesignOwner != "") && (QualifiedOwner!=null || QualifiedOwner !=""))
                        //{                           
                        //    Sms.InsertSmsInfos(QualifiedOwner, PojectName, name);
                        //}
                        //else if(MachineDesignOwner != null || MachineDesignOwner != "")
                        //{
                        //    Sms.InsertSmsInfos(MachineDesignOwner, PojectName, name);
                        //}
                        //if(IssueOwner !=null || IssueOwner != "")
                        //{
                        //    Sms.InsertSmsInfos(IssueOwner, PojectName, name);
                        //}
                   // }
                    //string name = Session["name"].ToString();                  
                    //SqlParameter[] parmd = new SqlParameter[2];
                    //parmd[0] = new SqlParameter("@TaskId", tk);
                    //parmd[1] = new SqlParameter("@CreateBy", name);
                    //ys.Database.ExecuteSqlCommand("exec TechTaskReCe  @TaskId,@CreateBy", parmd);

                }
              
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region 导入机械EXECL文件到数据库
        public ActionResult StationImport(List<Dictionary<string, string>> datas)
        {
            //try
            //{
                if (datas == null)
                {
                    return Content("<script>alert('图号不能为空');history.go(-1);</script>");
                }
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
            
                    string did = Session["Delete"].ToString();
                    int di = int.Parse(did);
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID", di);
                    string deleteAll = ys.Database.ExecuteSqlCommand("exec DeleteAll @TaskID", parms).ToString();
                    foreach (var data in datas)
                    {                      
                        data.Add("ListType", "");
                        data.Add("TaskID", did);
                        Tools<object>.SqlComm("exec AddMerial ",data);
                    }
                    //for (int i = 1; i < tab.Rows.Count; i++)
                    //{
                    //    SqlParameter[] parm = new SqlParameter[10];
                    //    parm[0] = new SqlParameter("@Level", tab.Rows[i][0].ToString());
                    //    parm[1] = new SqlParameter("@FigureNumber", tab.Rows[i][1].ToString());
                    //    parm[2] = new SqlParameter("@PartNumber", tab.Rows[i][2].ToString());
                    //    parm[3] = new SqlParameter("@PartSpec", tab.Rows[i][3].ToString());
                    //    parm[4] = new SqlParameter("@PartMaterial", tab.Rows[i][4].ToString());
                    //    parm[5] = new SqlParameter("@QTY", tab.Rows[i][5].ToString());
                    //    parm[6] = new SqlParameter("@Note", tab.Rows[i][6].ToString());
                    //    parm[7] = new SqlParameter("@Type", tab.Rows[i][7].ToString());
                    //    parm[8] = new SqlParameter("@ListType", "");
                    //    parm[9] = new SqlParameter("@TaskID", di);
                    //    ys.Database.ExecuteSqlCommand("exec AddMerial  @Level,@FigureNumber,@PartNumber,@PartSpec,@PartMaterial,@QTY,@Note,@Type,@ListType,@TaskID", parm);
                    //}
                    string name = Session["name"].ToString();
                    SqlParameter[] parmd = new SqlParameter[2];
                    parmd[0] = new SqlParameter("@TaskID", di);
                    parmd[1] = new SqlParameter("@createdBy", name);
                    int s=ys.Database.ExecuteSqlCommand("exec CreateBOM  @TaskID,@createdBy", parmd);                                     
                return Content("true");
            }
       
            //     }
            //catch (Exception ex)
            //{            
            //    return Content("<script>alert('BOM清单格式不对');history.go(-1);</script>");
            //}
        }
        #endregion

        #region 导入电气Excel文件

        public ActionResult StationsImport(HttpPostedFileBase files)
        {

            return View();
        }


        #endregion



        #region 显示BOM信息

        public JsonResult CheckBOMD(string w)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string id = Session["Check"].ToString();
                int i = 0;
                if (w != null)
                {
                    i = int.Parse(w);
                }
                else
                {
                    i = int.Parse(id);
                }

                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", i);
                var list = ys.Database.SqlQuery<checkBOM_Result>("exec checkBOM  @TaskID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 检查BOM图纸是否全部导入

        public ActionResult CheckBomAll(string Taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", i);
                var list = ys.Database.SqlQuery<checkBOM_Result>("exec checkBOM  @TaskID", parms).ToList();
                list = list.Where(c => c.类型 == "焊接件" || c.类型 == "装配体").ToList();
                foreach (var w in list)
                {
                    if (w.上传图片 == "上传")
                    {
                        return Content("false");
                    }
                }
            }
            return Content("true");
        }
        public ActionResult UploadDrawing(string id)
        {
            ViewData["taskid"] = id;
            return View();
        }
        #endregion

        #region 上传或更新图纸

        //视图
        //public ActionResult UploadPicture(string hao)
        //{
        //    ViewData["tuhao"] = hao;

        //    return View();
        //}
        ////插入图纸
        //public ActionResult UploadTheDrawings(string TuHao, HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        using (YLMES_newEntities ys = new YLMES_newEntities())
        //        {
        //            string name = Session["name"].ToString();
        //            var fileName1 = Path.Combine(Request.MapPath("~/Picture"), Path.GetFileName(file.FileName));
        //            file.SaveAs(fileName1);
        //            string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
        //            SqlParameter[] parms = new SqlParameter[7];
        //            parms[0] = new SqlParameter("@TYPE", "update");
        //            parms[1] = new SqlParameter("@ID", 10000);
        //            parms[2] = new SqlParameter("@FigureNumber",TuHao);
        //            parms[3] = new SqlParameter("@FolderName", "Picture");
        //            parms[4] = new SqlParameter("@FileName", fname);
        //            parms[5] = new SqlParameter("@CreatedBy", name);
        //            parms[6] = new SqlParameter("@Status","");
        //            ys.Database.ExecuteSqlCommand("exec SP_PM_Figure  @TYPE,@ID,@FigureNumber,@FolderName,@FileName,@CreatedBy,@Status", parms);
        //        }
        //        return Content("true");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return Content("false");
        //    }          
        //}
        ////查看图纸
        //public ActionResult UpdatePicture(string hao)
        //{
        //    ViewData["cTuhao"] = hao;
        //    using(YLMES_newEntities ys = new YLMES_newEntities()){
        //        var file = ys.PM_Figure.Where(p => p.FigureNumber==hao).FirstOrDefault();
        //        ViewData["file"] = file.FileName;
        //        Session["filed"] = file.FileName;
        //        string fileds = "/Picture/" + file.FileName;
        //        TempData["tupian"] = fileds;
        //    }

        //    return View();
        //}
        ////更新图纸 
        //public ActionResult UpdateTheDrawing(string TuHao, HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        using (YLMES_newEntities ys = new YLMES_newEntities())
        //        {
        //            string filed = Session["filed"].ToString();
        //            if (!string.IsNullOrEmpty(filed))
        //            {
        //                string fileds = "~/Picture/" + filed;
        //                string filePath = Server.MapPath(fileds);
        //                System.IO.File.Delete(filePath);
        //            }
        //            var fileName1 = Path.Combine(Request.MapPath("~/Picture"), Path.GetFileName(file.FileName));
        //            file.SaveAs(fileName1);
        //            string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
        //            SqlParameter[] parms = new SqlParameter[7];
        //            parms[0] = new SqlParameter("@TYPE","updated");
        //            parms[1] = new SqlParameter("@ID", 10000);
        //            parms[2] = new SqlParameter("@FigureNumber", TuHao);
        //            parms[3] = new SqlParameter("@FolderName", "");
        //            parms[4] = new SqlParameter("@FileName", fname);
        //            parms[5] = new SqlParameter("@CreatedBy", "");
        //            parms[6] = new SqlParameter("@Status", "");
        //            ys.Database.ExecuteSqlCommand("exec SP_PM_Figure  @TYPE,@ID,@FigureNumber,@FolderName,@FileName,@CreatedBy,@Status", parms);
        //        }
        //        return Content("true");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return Content("false");
        //    }
        //}
        #endregion

        #region 确认完成我的设计

        public ActionResult ConfirmTheDesign()
        {
            try
            {
                string type = Session["type"].ToString();
                string tid = Session["Finish"].ToString();
                int id = int.Parse(tid);
                if (type == "标准")
                {
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {

                        SqlParameter[] parms = new SqlParameter[1];
                        parms[0] = new SqlParameter("@TaskID", id);
                        ys.Database.ExecuteSqlCommand("exec updateIssueCompleted  @TaskID", parms);                    
                        return Content("true");
                    }
                }
                else
                {
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {

                        SqlParameter[] parms = new SqlParameter[1];
                        parms[0] = new SqlParameter("@TaskID", id);
                        int i = ys.Database.ExecuteSqlCommand("exec UpdateCompletedMachineDesign  @TaskID", parms);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }

        #endregion

        #region 更新进度
        //任务进度详细
        public ActionResult UpdateProgress()
        {
            return View();
        }
        //显示任务进度
        public JsonResult CheckProgress()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string id = Session["Projess"].ToString();
                TempData["up"] = id;
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Taskid", i);
                var list = ys.Database.SqlQuery<CheckTaskProgress_Result>("exec CheckTaskProgress  @Taskid", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //显示更新任务进度
        public ActionResult UpdateTaskProgress()
        {
            Session["UTP"].ToString();
            return View();
        }
        //更新任务进度
        public ActionResult UpdateProgressd(string TaskID, string Item, string From, string TO, string ChildTaskDescription, string StatusDescription)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string name = Session["name"].ToString();
                    int tId = int.Parse(TaskID);
                    SqlParameter[] parms = new SqlParameter[8];
                    parms[0] = new SqlParameter("@Type", "Add");
                    parms[1] = new SqlParameter("@TaskId", tId);
                    parms[2] = new SqlParameter("@ProgressItem", Item);
                    parms[3] = new SqlParameter("@TimeFrom", From);
                    parms[4] = new SqlParameter("@TimeTO", TO);
                    parms[5] = new SqlParameter("@ChildTaskDescription", ChildTaskDescription);
                    parms[6] = new SqlParameter("@statusDescription", StatusDescription);
                    parms[7] = new SqlParameter("@UserName", name);
                    ys.Database.ExecuteSqlCommand("exec  PM_TaskProgressEdit  @Type,@TaskId,@ProgressItem,@TimeFrom,@TimeTO,@ChildTaskDescription,@statusDescription,@UserName", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //显示修改任务进度
        public ActionResult EditShowProgress(string tid, string item)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int id = int.Parse(tid);
                int it = int.Parse(item);
                var TaskProgressd = ys.PM_TaskProgress.Where(p => p.TaskID == id && p.Item == it).FirstOrDefault();
                ViewData["tid"] = tid;
                ViewData["item"] = item;
                ViewData["from"] = TaskProgressd.From;
                ViewData["to"] = TaskProgressd.TO;
                ViewData["ctd"] = TaskProgressd.Content;
                ViewData["cd"] = TaskProgressd.CompletionDescription;
            }
            return View();
        }
        //修改任务进度
        public ActionResult EidtProgressDu(string TaskID, string Item, string From, string TO, string ChildTaskDescription, string StatusDescription)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int id = int.Parse(TaskID);
                    int it = int.Parse(Item);
                    SqlParameter[] parms = new SqlParameter[6];
                    parms[0] = new SqlParameter("@TaskID", id);
                    parms[1] = new SqlParameter("@Item", it);
                    parms[2] = new SqlParameter("@From", From);
                    parms[3] = new SqlParameter("@TO", TO);
                    parms[4] = new SqlParameter("@Content", ChildTaskDescription);
                    parms[5] = new SqlParameter("@CompletionDescription", StatusDescription);
                    ys.Database.ExecuteSqlCommand("exec UpdateTaskProgress  @TaskID,@Item,@From,@TO,@Content,@CompletionDescription", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //删除任务进度
        public ActionResult DeleteProgress(string TaskID, string Item)
        {
            try
            {
                int id = int.Parse(TaskID);
                int it = int.Parse(Item);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@TaskID", id);
                    parms[1] = new SqlParameter("@Item", it);
                    ys.Database.ExecuteSqlCommand("exec DeleteTaskProgress  @TaskID,@Item", parms);
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

        #region 工艺BOM
        public ActionResult TaskMapPart(string TaskID, string ProductName, string ProductSpec,string ContractNumbers)
        {
            Session["quetask"] = TaskID;
            //第1个页面
            Session["task1"] = TaskID;
          //  Session["ProductName1"] = ;
            Session["ProductSpec1"] = ProductSpec;
            Session["ContractNumbers"] = ContractNumbers;

            //第2个页面
            Session["task2"] = TaskID;
           // Session["ProductName2"] = ProductName;
            Session["ProductSpec2"] = ProductSpec;


            ViewData["taskid"] = TaskID;
            ViewData["ProductName"] = ProductName;
            ViewData["ProductNamesd"] = ProductName;
            ViewData["ProductSpec"] = ProductSpec;
            return View();
        }
        public ActionResult TaskMapPart2(string TaskID, string PartNumber, string ProductSpec)
        {
            Session["task2"] = TaskID;
            Session["quetask2"] = TaskID;
            Session["ProductName1"] = PartNumber;
            Session["ProductSpec1"] = ProductSpec;
            return View();
        }

        //表格显示BOM
        public ActionResult CheckBOM(string PSpec,string taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
               string PName = Session["ProductNamed"].ToString();
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@ProjectName", PName);
                parms[1] = new SqlParameter("@PartSpec", PSpec);
                parms[2] = new SqlParameter("@Type", "机械设计");
                var list = ys.Database.SqlQuery<TaskMapingPartCheck_Result>("exec TaskMapingPartCheck @ProjectName,@PartSpec,@Type", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //修改BOM
        public ActionResult UpdateBOM(string id, string dosage, string unit)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);
                    PM_Material pm = ys.PM_Material.Where(c => c.ID == i).FirstOrDefault();
                    pm.materialQTY = dosage;
                    pm.MaterialUnits = unit;
                    ys.SaveChanges();
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        //删除BOM
        public ActionResult DeleteBOM(string id)
        {
            try
            {
                int pId = int.Parse(id);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@PartNumber", "");
                    parms[1] = new SqlParameter("@ID", pId);
                    ys.Database.ExecuteSqlCommand("exec DeleteBOM  @PartNumber,@ID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }

        //表格显示BOM2
        public ActionResult CheckBOM2(string PName,string SPec,string taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (taskid == null)
                {
                    taskid = "";
                }
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@ProjectName", PName);
                parms[1] = new SqlParameter("@PartSpec", SPec);
                parms[2] = new SqlParameter("@Taskid", taskid);
                var list = ys.Database.SqlQuery<TaskMapingPartCheck_Result>("exec TaskMapingPartCheck @ProjectName,@PartSpec,@Taskid", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //修改BOM2
        public ActionResult UpdateBOM2(string part, string child, string dosage, string unit)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[4];
                    parms[0] = new SqlParameter("@part", part);
                    parms[1] = new SqlParameter("@child", child);
                    parms[2] = new SqlParameter("@dosage", dosage);
                    parms[3] = new SqlParameter("@unit", unit);
                    ys.Database.ExecuteSqlCommand("exec UpdateBOM  @part,@child,@dosage,@unit", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        //删除BOM2
        public ActionResult DeleteBOM2(string id)
        {
            try
            {
                int pId = int.Parse(id);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@PartNumber", "");
                    parms[1] = new SqlParameter("@ID", pId);
                    ys.Database.ExecuteSqlCommand("exec DeleteBOM  @PartNumber,@ID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //确认提交到生产部
        public ActionResult SubmitPlan(string taskid, string partid)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int tkid = int.Parse(taskid);
                    int paid = int.Parse(partid);
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[3];
                    parms[0] = new SqlParameter("@PartID", paid);
                    parms[1] = new SqlParameter("@TaskID", tkid);
                    parms[2] = new SqlParameter("@CreatedBy", name);
                    ys.Database.ExecuteSqlCommand("exec QueMapingPart  @PartID,@TaskID,@CreatedBy", parms);
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

        #region 新建子件

        public ActionResult NewChildThing()
        {
            return View();
        }

        #endregion

        #region 工艺卡

        public ActionResult ProcessCard()
        {

            return View();
        }

        #endregion

        #region 完成工艺设置

        public ActionResult CompleteProcessSetting(string tastid,string ContractNumbers)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int id = int.Parse(tastid);
                    SqlParameter[] parmw = new SqlParameter[1];
                    parmw[0] = new SqlParameter("@TaskID", id);
                   var list=ys.Database.SqlQuery<CheckFinshProcess_Result>("exec CheckFinshProcess @TaskID", parmw).ToList();
                    foreach(var sd in list)
                    {
                        if (sd.工艺流程 == "添加流程")
                        {
                            return Content("false");
                        }
                    }
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID", id);
                    string name = Session["name"].ToString();
                    ys.Database.ExecuteSqlCommand("exec UpdatePEcompletedTask  @TaskID", parms);
                    int i = Sms.InsertSmsInfos("邓德力",ContractNumbers, name);
                    if (i > 0)
                    {
                        return Content("true");
                    }
                    else
                    {
                        return Content("false");
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

        #endregion

        #region 工艺流程
        //视图
        public ActionResult EditingProcess(string PartNumber, string ProductSpec, string ProductName, string PartId, string ifs)
        {
            ViewData["ifs"] = ifs;
            ViewData["PartNumber"] = PartNumber;
            ViewData["PartId"] = PartId;
            Session["ProductName"] = ProductName;
            ViewData["ProductSpec"] = ProductSpec;
            ViewData["ProductName"] = ProductName;

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] param = new SqlParameter[1];
                if (PartId == null)
                {
                    PartId = "";
                }
                param[0] = new SqlParameter("@ParId", PartId);
                ViewBag.rn = ys.Database.SqlQuery<CheckRouteName_Result>("exec CheckRouteName @ParId", param).ToList();
            }
            return View();
        }
        //显示流程
        public ActionResult CheckProcess(string type, string PartNumber,string id, string RouteName)
        {

            string ProductName = "";
            string ProductSpec = "";
            ProductName = Session["ProductNamed"].ToString();
            ProductSpec= Session["ProductSpeces"].ToString();
            if (ProductSpec == null)
            {
                ProductSpec = "";
            }
            int i = 3;
            if (PartNumber != null)
            {
                i = 4;
                if (id == "Partid")
                {
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        var list = ys.PM_Route.Where(q => q.RouteName == RouteName).ToList();
                        int id1 = list[0].ID;
                        var list1 = ys.PM_Product.Where(p => p.RouteID == id1).ToList();
                        id = list1[0].PartID.ToString();
                    }
                }
                if (id != "" && id != "null")
                {
                    i = 5;
                }
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[i];
                if (i == 4)
                {
                    parms[3] = new SqlParameter("@PartNumber", PartNumber);

                }
                if (i == 5)
                {
                    parms[3] = new SqlParameter("@PartNumber", PartNumber);
                    parms[4] = new SqlParameter("@id", id);
                    type = "'chickid'";
                }
                //string name = Session["name"].ToString();
                parms[0] = new SqlParameter("@ProductName", ProductName);
                parms[1] = new SqlParameter("@ProductSpec", ProductSpec);
                parms[2] = new SqlParameter("@type", type);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                if (i == 4)
                {
                    var list = ys.Database.SqlQuery<CheckTask_Result>("exec CheckTask @type,@ProductName,@ProductSpec,@PartNumber", parms).FirstOrDefault();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else if (i == 5)
                {

                    var list = ys.Database.SqlQuery<CheckTask_Result>("exec CheckTask @type,@ProductName,@ProductSpec,@PartNumber,@id", parms).FirstOrDefault();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = ys.Database.SqlQuery<CheckProcess_Result>("exec CheckProcess @type,@ProductName,@ProductSpec", parms).ToList();
                    int count = list.Count();
                    hasmap.Add("code", 0);
                    hasmap.Add("msg", "");
                    hasmap.Add("count", count);
                    hasmap.Add("data", list);
                    return Json(hasmap, JsonRequestBehavior.AllowGet);
                }

            }
        }
        //上传Excel文件页面
        public ActionResult UploadProcess(string Route)
        {
            ViewData["RouteNames"] = Route;
            //string path = Server.MapPath("~/Process/6012-2000 无动力滚筒线组装.xls");
          
            //Workbook workbook = new Workbook();

            //workbook.LoadFromFile(path);
            //Worksheet sheet = workbook.Worksheets[0];
            //sheet.ViewMode = ViewMode.Preview;
            //sheet.ZoomScaleNormal = 80;
            //workbook.SaveToFile(path, ExcelVersion.Version2010);

            //   System.Diagnostics.Process.Start("Excel.EXE",path);
            return View();
        }
        //上传工艺文件
        public ActionResult UploadProces(string rn,HttpPostedFileBase files)
        {
            StreamReader sr;
            if (files == null)
            {
                return Content("<script>alert('请选择文件');history.go(-1);</script>");
            }
            else
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parmsw = new SqlParameter[4];
                    parmsw[0] = new SqlParameter("@type", "delete");
                    parmsw[1] = new SqlParameter("@Route", rn);
                    parmsw[2] = new SqlParameter("@CreatedBy", "");
                    parmsw[3] = new SqlParameter("@Require", "");
                    ys.Database.ExecuteSqlCommand("exec PM_RouteDetials  @type,@Route,@CreatedBy,@Require", parmsw);
                    string name = Session["name"].ToString();
                    var fileNames = Path.Combine(Request.MapPath("~/Process"), Path.GetFileName(files.FileName));
                    files.SaveAs(fileNames);
                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(fileNames);
                    Worksheet sheet = workbook.Worksheets[0];
                    var fileName1 = Path.Combine(Request.MapPath("~/txt"), Path.GetFileName("wwe.txt"));
                    FileStream fs = new FileStream(fileName1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);                    
                    sheet.SaveToFile(fileName1, " ", Encoding.UTF8);
                    sr = new StreamReader(fs, System.Text.Encoding.Default);
                    int i = 0;//文件行数  
                    int lineIdx = 10;//指定行数
                    string st = "";
                    string s2 = "";
                    List<string> delList = new List<string>();
                    while ((st = sr.ReadLine()) != null)
                    {
                        i++;
                        if (i >= lineIdx)
                        {//获取指定行数
                            if (st != null)
                            {
                                if (st.Trim() == "")
                                {
                                    delList.Add(s2); 
                                }
                                if (st.Trim() != "")
                                {
                                    string str1 = (st.Replace("\"", "")).Substring(0, 1);
                                    string str = new System.Text.RegularExpressions.Regex("[\\s]+").Replace((st.Replace("\"", "")).Trim(), " ");//替换多个空额为一个空格
                                    if (str1.Trim() == "" || i == 10)
                                    {
                                        s2 += str.Trim().Replace(" ", "-") + "-";//替换空
                                    }
                                    else
                                    {
                                        delList.Add(s2);
                                        s2 = "";
                                        s2 = str.Trim().Replace(" ", "-");//替换空
                                    }
                                }
                                else
                                {                                   
                                    break;
                                }
                            }
                        }
                        st = "";
                    }
                    foreach (var sb in delList)
                    {
                        SqlParameter[] parms = new SqlParameter[4];
                        parms[0] = new SqlParameter("@type", "update");
                        parms[1] = new SqlParameter("@Route", rn);
                        parms[2] = new SqlParameter("@CreatedBy", name);
                        parms[3] = new SqlParameter("@Require",sb.ToString());                       
                        ys.Database.ExecuteSqlCommand("exec PM_RouteDetials  @type,@Route,@CreatedBy,@Require", parms);                        
                    }
                    return Content("<script>alert('更新成功!');history.go(-1);</script>");
                }              
            }     
        }
        //上传新工艺文件页面
        public ActionResult AddUploadProcess(string PartNumber,string ProductName,string ProductSpec,string PartId,string ProcessName)
        {
            ViewData["PartNumber"] = PartNumber;
            ViewData["ProductName"] = ProductName;
            ViewData["ProductSpec"] = ProductSpec;
            ViewData["PartId"] = PartId;
            ViewData["ProcessName"] = ProcessName;
            return View();
        }
        public ActionResult UploadProcesd(string PartId,string ProcessName, HttpPostedFileBase files)
        {
            StreamReader sr;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string pname = "~/Process/" + ProcessName + ".xls";
                string name = Session["name"].ToString();
                string file = "~/Process/" + files.FileName;
                if (pname != file)
                {
                    return Content("<script>alert('文件名字必须与流程名字一样');history.go(-1);</script>");
                }
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@PartId", PartId);
                parms[1] = new SqlParameter("@ProcessName", ProcessName);
                parms[2] = new SqlParameter("@File", file);
                parms[3] = new SqlParameter("@CreateBy", name);
                int q=ys.Database.ExecuteSqlCommand("exec AddProcessCar  @PartId,@ProcessName,@File,@CreateBy", parms);
                if (q > 0)
                {
                    var fileNames = Path.Combine(Request.MapPath("~/Process"), Path.GetFileName(files.FileName));                    
                    files.SaveAs(fileNames);
                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(fileNames);
                    Worksheet sheet = workbook.Worksheets[0];
                    var fileName1 = Path.Combine(Request.MapPath("~/txt"), Path.GetFileName("wwe.txt"));
                    FileStream fs = new FileStream(fileName1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    sheet.SaveToFile(fileName1, " ", Encoding.UTF8);
                    sr = new StreamReader(fs, System.Text.Encoding.Default);
                    int i = 0;//文件行数  
                    int lineIdx = 10;//指定行数
                    string st = "";
                    string s2 = "";
                    List<string> delList = new List<string>();
                    while ((st = sr.ReadLine()) != null)
                    {
                        i++;
                        if (i >= lineIdx)
                        {//获取指定行数
                            if (st != null)
                            {
                                if (st.Trim() == "")
                                {
                                    delList.Add(s2);
                                }
                                if (st.Trim() != "")
                                {
                                    string str1 = (st.Replace("\"", "")).Substring(0, 1);
                                    string str = new System.Text.RegularExpressions.Regex("[\\s]+").Replace((st.Replace("\"", "")).Trim(), " ");//替换多个空额为一个空格
                                    if (str1.Trim() == "" || i == 10)
                                    {
                                        s2 += str.Trim().Replace(" ", "-") + "-";//替换空
                                    }
                                    else
                                    {
                                        delList.Add(s2);
                                        s2 = "";
                                        s2 = str.Trim().Replace(" ", "-");//替换空
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        st = "";
                    }
                    foreach (var sb in delList)
                    {
                        SqlParameter[] parmd = new SqlParameter[4];
                        parmd[0] = new SqlParameter("@type", "update");
                        parmd[1] = new SqlParameter("@Route", ProcessName);
                        parmd[2] = new SqlParameter("@CreatedBy", name);
                        parmd[3] = new SqlParameter("@Require", sb.ToString());
                        ys.Database.ExecuteSqlCommand("exec PM_RouteDetials  @type,@Route,@CreatedBy,@Require", parmd);
                    }
                    return Content("<script>alert('上传成功!');history.go(-2);</script>");
                }
                else
                {
                    return Content("<script>alert('请联系维修人员查看错误!');history.back(-1);</script>");
                }
            }
           
        }
        //引用流程页面
        public ActionResult ReferProcess(string Partid)
        {
            ViewData["Partid"] = Partid;
          
            return View();
        }
        //显示流程信息
        public ActionResult getRoute()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_Route.ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }

        }
        //新增引用流程
        public ActionResult ReferProcessAdd(string mid,string rt)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Materid", mid);
                parms[1] = new SqlParameter("@Route", rt);
                parms[2] = new SqlParameter("@CreateBy", name);
                ys.Database.ExecuteSqlCommand("exec PM_ReferProcess  @Materid,@Route,@CreateBy", parms);
                return Content("true");
            }
        }
        //删除流程
        public ActionResult DeleteProcess(string id, string Rotueid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Rotueid == null)
                {
                    Rotueid = "";

                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ID", id);
                parms[1] = new SqlParameter("@Rotueid", Rotueid);
                ys.Database.ExecuteSqlCommand("exec DeleteProcess  @ID,@Rotueid", parms);
            }
            return Content("true");


        }
        //修改流程
        public ActionResult UpdateProcess(string RouteName, string WorkSecondPerPCS, string WorkSecond2PerPCS, string require, string StationType, string id, string index)
        {

            int i = int.Parse(id);

            string name = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[9];
                parms[0] = new SqlParameter("@StationType", StationType);
                parms[1] = new SqlParameter("@SortID", "1");
                parms[2] = new SqlParameter("@WorkHours", WorkSecondPerPCS);
                parms[3] = new SqlParameter("@WorkHours2", WorkSecond2PerPCS);
                parms[4] = new SqlParameter("@require", require);
                parms[5] = new SqlParameter("@ID", id);
                parms[6] = new SqlParameter("@CreatedBy", name);
                parms[7] = new SqlParameter("@RouteName", RouteName);
                parms[8] = new SqlParameter("@index", index);
                ys.Database.ExecuteSqlCommand("exec UpdateProcess  @StationType,@SortID,@WorkHours,@WorkHours2,@require,@ID,@CreatedBy,@RouteName,@index", parms);
            }
            return Content("true");

        }
        //另存流程
        public ActionResult SaveRoute(string saveName, string Routevalue)
        {
            try
            {
                string name = Session["name"].ToString();
                int rv = int.Parse(Routevalue);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@RouteSaveAS", saveName);
                parms[1] = new SqlParameter("@CreatedBy", name);
                parms[2] = new SqlParameter("@ID", rv);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec SaveRoute  @RouteSaveAS,@CreatedBy,@ID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        //我的任务收款
        public ActionResult PM_ContractReceivablesMain()
        {
            return View();
        }
        //新增流程站点
        [ValidateInput(false)]
        public ActionResult AddProcess(string PartNumber, string ProductSpec, string ProductName, string ProcessName, string PartId, string Route, string id)
        {
            ViewData["Route"] = Route;
            ViewData["id"] = id;
            ViewData["PartNumber"] = PartNumber;
            ViewData["ProductSpec"] = ProductSpec;
            TempData["ProductName"] = ProductName;
            ViewData["ProcessName"] = ProcessName;
            ViewData["PartId"] = PartId;
            return View();
        }
        //查询工位类型
        public ActionResult StationTypeNumber()
        {
            List<PM_ProductStationType> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                list = ys.PM_ProductStationType.ToList();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //查询可用流程
        public ActionResult CheckProductRouting(string RouteName, string code)
        {

            List<CheckProcess_Result> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                list = ys.Database.SqlQuery<CheckProcess_Result>("exec CheckProcess @type='sel',@RouteName='" + RouteName + "'").ToList();
            }
            if (code != null)
            {
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<CheckProcess_Result> pageList = new PageList<CheckProcess_Result>(list, 1, 10);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //保存流程
        [ValidateInput(false)]
        public string AddPoc(List<string> proValue, List<string> proName, string RouteName, string PartId1)
        {

            StringBuilder sb = new StringBuilder();
            string Type = "add";
            SqlParameter[] prams = new SqlParameter[proName.Count];
            Dictionary<string, string> Applier = new Dictionary<string, string>();

            for (int index = 0; index < proName.Count; index++)
            {

                Applier.Add(proName[index].ToString(), proValue[index].ToString());

            }
            int PartId = int.Parse(PartId1);
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int j = 0;
                sb.Append("exec CheckProcess @PartId=" + PartId + ",@RouteName='" + RouteName + "',@CreatedBy='" + Session["name"] + "',@type='" + Type + "',");
                foreach (var app in Applier)
                {
                    prams[j] = new SqlParameter("@" + app.Key + "1", app.Value);


                    if ((j + 1) == proName.Count)
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
        //查询大物料

        public ActionResult findTask()
        {

            List<CheckMateial_Result> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                list = ys.Database.SqlQuery<CheckMateial_Result>("exec CheckMateial ''").ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //查询小物料
        public ActionResult findTaskMi(string ProductName, string ProductSpec)
        {

            List<CheckMateial_Result> list = null;
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@ProductName", ProductName);
            parms[1] = new SqlParameter("@ProductSpec", ProductSpec);
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                list = ys.Database.SqlQuery<CheckMateial_Result>("exec CheckMateial @ProductName,@ProductSpec", parms).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 计划部
        //计划部确认计划分配
        public ActionResult UpdatePMCAssignConfirm(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec PMCAssignConfirm  @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //计划部完成计划
        public ActionResult UpdatePMCCompleted(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec PMCCompleted  @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //转生产工单
        public ActionResult PMCSettingWO(string id, string ProjectName, string pcName, string spec, string TaskType, string Choose)
        {
            ViewData["taskid"] = id;
            ViewData["quetaskid"] = id;
            ViewData["TaskID2"] = id;
            ViewData["TaskID3"] = id;
            ViewData["pjname"] = ProjectName;
            ViewData["pcName"] = pcName;
            ViewData["pcNamed"] = pcName;
            ViewData["spec"] = spec;
            Session["specs"] = spec;
            ViewData["TaskType"] = TaskType;
            ViewData["Choose"] = Choose;
            return View();
        }
        //显示是否转工单页面
        public ActionResult TransferWork(string Parid,string OrderCount,string pnum,string taskid,string ProductName,string ProductSpec)
        {
            ViewData["partid"] = Parid;
            ViewData["partids"] = Parid;
            ViewData["count"] = OrderCount;
            ViewData["count2"] = OrderCount;
            ViewData["partname"] = pnum;
            ViewData["taskid"] = taskid;
            ViewData["taskid2"] = taskid;
            ViewData["ProductName"] = ProductName;
            ViewData["ProductSpec"] = ProductSpec;
            return View();
        }
        //显示是否转工单信息
        public JsonResult TurnRepair(string pid, string tid,string count)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(pid);
                int ti = int.Parse(tid);
                int con = int.Parse(count);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@PartID",i);
                parms[1] = new SqlParameter("@TaskID", ti);
                parms[2] = new SqlParameter("@count", con);
                var list = ys.Database.SqlQuery<CheckInventory_Result>(" exec [CheckInventory] @PartID,@TaskID,@count", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }           
        }
        //修改物料是否采购
        public ActionResult UpdateApplyMater(string Materid)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int mid = int.Parse(Materid);
                PM_Material lid = ys.PM_Material.Where(p => p.ID == mid).FirstOrDefault();
                lid.InventoryQuantity = "申请中";
                ys.SaveChanges();
            }            
            return Content("true");
        }
        //申请采购信息
        public ActionResult ApplyPurchasingd(string type,string Materid,string PCount,string Unit,string Taskid,string Spec)
        {
            if (Session["name"].ToString() == "")
            {
                return Content("<script>window.top.location = '/Home/Login';</script>");
            }
            else
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string name = Session["name"].ToString();
                    int tid = int.Parse(Taskid);
                    int cid = int.Parse(Materid);
                    int count = int.Parse(PCount);
                    SqlParameter[] parmd = new SqlParameter[7];
                    parmd[0] = new SqlParameter("@TaskID", tid);
                    parmd[1] = new SqlParameter("@MaterialID", cid);
                    parmd[2] = new SqlParameter("@Count", count);
                    parmd[3] = new SqlParameter("@UserName", name);
                    parmd[4] = new SqlParameter("@Unit", Unit);
                    parmd[5] = new SqlParameter("@type", type);
                    parmd[6] = new SqlParameter("@Spec", Spec);
                    ys.Database.ExecuteSqlCommand("exec PM_AddPurlist  @TaskID,@MaterialID,@Count,@UserName,@Unit,@type,'',@Spec", parmd);
                    //if (i > 0)
                    //{

                    //}
                }                
            }
            return Content("true");
        }
        //检查是否全部工单转换成功
        public ActionResult TaskMapings(string taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                var list = ys.Database.SqlQuery<TaskMapingWOCheck_Result>("exec TaskMapingWOCheck @TaskID", parms).ToList();
                foreach (var wn in list)
                {
                    if (string.IsNullOrEmpty(wn.工单名称))
                    {
                        return Content("false");
                    }
                }
            }
            return Content("true");
        }
        //显示工单信息
        public ActionResult TaskMapingWOCheck(string taskid, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = 0;
                Session["tide"] = taskid;
                if (string.IsNullOrEmpty(taskid))
                {
                    string workid = Session["worktaskid"].ToString();
                    tid = int.Parse(workid);
                }
                else
                {
                    tid = int.Parse(taskid);
                }
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                var list = ys.Database.SqlQuery<TaskMapingWOCheck_Result>("exec TaskMapingWOCheck @TaskID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<TaskMapingWOCheck_Result> pageList = new PageList<TaskMapingWOCheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //检查部件是否数量完成
        public ContentResult CheckParts(string taskid,string count)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(taskid);
                int number = int.Parse(count);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                var list = ys.Database.SqlQuery<CheckParts_Result>("exec CheckParts @TaskID", parms).ToList();
                foreach(var li in list){
                    if (number != li.部件数量)
                    {
                        return Content("false");
                    }
                }
                return Content("true");
            }           
        }
        //显示成品的部件库存信息
        public JsonResult CheckPartd(string taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                var list = ys.Database.SqlQuery<CheckParts_Result>("exec CheckParts @TaskID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //完成建工单
        public ActionResult CompletionOfWorkOrder()
        {
            try
            {
                string tid = Session["tide"].ToString();
                int id = int.Parse(tid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", id);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec UpdatePMCCompleted  @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //转生产工单
        public ActionResult TurnPwo(string  PartId,string  ProductName,string ProductSpec,string Taskid,string Count)
        {
            try
            {
                string PartNumber = "";
                string spec = "";
                string ji = "";
                using (YLMES_newEntities yd = new YLMES_newEntities())
                {
                    int pid = int.Parse(PartId);
                    SqlParameter[] parmd = new SqlParameter[1];
                    parmd[0] = new SqlParameter("@Partid", pid);
                    var lisd = yd.Database.SqlQuery<CheckMater_Result>("exec CheckMater @Partid", parmd).FirstOrDefault();
                    PartNumber = lisd.PartNumber;
                    spec = lisd.PartSpec;
                    ji = lisd.PartMaterial;
                    int paid = int.Parse(PartId);
                    int onumber = int.Parse(Count);
                    int tid = int.Parse(Taskid);
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@PartID", paid);
                    var list = yd.Database.SqlQuery<CheckInventory_Result>(" exec [CheckInventory] @PartID", parms).ToList();
                    int? cid = 0;
                    foreach (var num in list)
                    {
                        int? s = 0;
                        if (num.StockQTY == null)
                        {
                            s = 0;
                        }
                        else
                        {
                            s = num.StockQTY;
                        }
                        cid = num.ChildPartID;
                        if (s < (onumber * num.ChildPartQTY))
                        {
                            return Content("cc");
                        }
                       
                    }
                }
                // parms[8] = new SqlParameter("@MSG", "");
                //SqlParameter ptal = new SqlParameter("@MSG", "");
                //ptal.Direction = ParameterDirection.Output;
                //,@MSG out
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string name = Session["name"].ToString();
                    int tid = int.Parse(Taskid);
                    SqlParameter[] parms = new SqlParameter[9];
                    parms[0] = new SqlParameter("@PartNumber", ProductName);
                    parms[1] = new SqlParameter("@TaskID", tid);
                    parms[2] = new SqlParameter("@ParentPartNumber", PartNumber);
                    parms[3] = new SqlParameter("@DueDay", "");
                    parms[4] = new SqlParameter("@CreatedEmployee", name);
                    parms[5] = new SqlParameter("@Spec", spec);
                    parms[6] = new SqlParameter("@Ji", ji);
                    parms[7] = new SqlParameter("@cSpec", ProductSpec);
                    parms[8] = new SqlParameter("@count", Count);
                    ys.Database.ExecuteSqlCommand("exec AddWork @PartNumber,@TaskID,@ParentPartNumber,@DueDay,@CreatedEmployee,@Spec,@Ji,@cSpec,@count", parms);
                    return Content("true");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //产品转工单
        public ActionResult TrunWorderOrder(string taskid,string PartNumber,string PartSpec,string Parent,string Spec,string Ji)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                string name = Session["name"].ToString();
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[8];
                parms[0] = new SqlParameter("@PartNumber", PartNumber);
                parms[1] = new SqlParameter("@TaskID", tid);
                parms[2] = new SqlParameter("@ParentPartNumber", PartNumber);
                parms[3] = new SqlParameter("@DueDay", "");
                parms[4] = new SqlParameter("@CreatedEmployee", name);
                parms[5] = new SqlParameter("@Spec", Spec);
                parms[6] = new SqlParameter("@Ji", Ji);
                parms[7] = new SqlParameter("@cSpec", PartSpec);
                ys.Database.ExecuteSqlCommand("exec AddWork @PartNumber,@TaskID,@ParentPartNumber,@DueDay,@CreatedEmployee,@Spec,@Ji,@cSpec", parms);
                return Content("true");
            }
        }
        //采购申请单
        public ActionResult Purchasing(string Taskid, string PartID)
        {
            Session["purcha"] = Taskid;
            Session["partsid"] = PartID;
            ViewData["purchas"] = Taskid;
            ViewData["purcha"] = Taskid;
            Session["purcha"] = Taskid;
            ViewData["pid"] = PartID;
            ViewData["pid2"] = PartID;
            return View();
        }
        //显示申购
        public JsonResult CheckPurchase(string change, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(Session["purcha"].ToString());
                int pid = int.Parse(Session["partsid"].ToString());
                if (change == "全部")
                {
                    change = "";
                }
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@TaskID", tid);
                parms[1] = new SqlParameter("@ListType", change);
                parms[2] = new SqlParameter("@PartID", pid);
                var list = ys.Database.SqlQuery<PurchaseQTYcheck_Result>("exec PurchaseQTYcheck @TaskID,@ListType,@PartID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<PurchaseQTYcheck_Result> pageList = new PageList<PurchaseQTYcheck_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //显示成品的部件的库存
        public ActionResult CheckProductInvent(string Taskid)
        {
            ViewData["taskid"] = Taskid;
            return View();
        }
        //检测是否已经申购
        public ActionResult CheckPurchaseList(string Materid,string Taskid)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int mid = int.Parse(Materid);
                int tid = int.Parse(Taskid);
                PM_PurchaseMaterialList pm = ys.PM_PurchaseMaterialList.Where(c => c.MaterialID == mid && c.TaskID == tid).FirstOrDefault();
                if (pm==null)
                {
                    return Content("true");
                }
                else
                {
                    return Content(pm.ActPurchaseQTY.ToString());
                }
                
            }
           
        }
        //修改申购
        public ActionResult EditPurchase(string taskid, string matrterID, string ApplayQty)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[4];
                int id = int.Parse(taskid);
                int aqt = int.Parse(ApplayQty);
                int mid = int.Parse(matrterID);
                parms[0] = new SqlParameter("@QTYofPCS", aqt);
                parms[1] = new SqlParameter("@MaterialID", mid);
                parms[2] = new SqlParameter("@TaskID", id);
                parms[3] = new SqlParameter("type", "edit");
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec UpdatePurchaseQTY  @QTYofPCS,@MaterialID,'',@TaskID,'','',@type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //编辑申购
        public ActionResult EditPurchased(string taskid, string count, string Price)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[10];
                int id = int.Parse(taskid);
                int pices = int.Parse(Price);
                int counts = int.Parse(count);
                parms[0] = new SqlParameter("@QTYofPCS", 666);
                parms[1] = new SqlParameter("@MaterialID", 666);
                parms[2] = new SqlParameter("@ApplyPurchasePCS", counts);
                parms[3] = new SqlParameter("@TaskID", 666);
                parms[4] = new SqlParameter("@Units", "无");
                parms[5] = new SqlParameter("@Note", "无");
                parms[6] = new SqlParameter("@type", "update");
                parms[7] = new SqlParameter("@ActPCS", 666);
                parms[8] = new SqlParameter("@price", pices);
                parms[9] = new SqlParameter("@id", id);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec UpdatePurchaseQTY  @QTYofPCS,@MaterialID,@ApplyPurchasePCS,@TaskID,@Units,@Note,@type,@ActPCS,@price,@id", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //删除申请
        public ActionResult DeletePurchase(string taskid, string marterid)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[7];
                int id = int.Parse(taskid);
                int mid = int.Parse(marterid);
                parms[0] = new SqlParameter("@QTYofPCS", 666);
                parms[1] = new SqlParameter("@MaterialID", mid);
                parms[2] = new SqlParameter("@ApplyPurchasePCS", 666);
                parms[3] = new SqlParameter("@TaskID", id);
                parms[4] = new SqlParameter("@Units", "");
                parms[5] = new SqlParameter("@Note", "");
                parms[6] = new SqlParameter("@type", "delete");
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec UpdatePurchaseQTY  @QTYofPCS,@MaterialID,@ApplyPurchasePCS,@TaskID,@Units,@Note,@type", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //提交到采购清单
        public ActionResult AddPurchaseMater(string id,string count)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[3];
                int counts = int.Parse(count);
                string name = Session["name"].ToString();
                parms[0] = new SqlParameter("@id", i);
                parms[1] = new SqlParameter("@count", counts);
                parms[2] = new SqlParameter("@username", name);                         
                ys.Database.ExecuteSqlCommand("exec pm_AddElectricalList  @id,@count,@username", parms);                              
                return Content("true");
            }
        }
        //确认提交采购申请
        public ActionResult EQProcurement(string taskid)
        {
            try
            {
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec PMCAskPurchase  @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //排产计划
        public ActionResult Scheduling(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec Scheduling  @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //新增采购申请页面
        public ActionResult AddPur(string Taskid)
        {
            ViewData["purcha"] = Taskid;
            return View();
        }
        //显示采购申请物料名称
        public JsonResult CheckGetMaterial()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = (from l in ys.PM_Material
                            group l by new { l.PartNumber } into g
                            select new { g.Key.PartNumber }).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);

            }
        }
        //显示采购申请物料规格
        public JsonResult CheckGetSpec(string PartNumber)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = (from l in ys.PM_Material
                            where l.PartNumber == PartNumber
                            group l by new { l.PartSpec } into g
                            select new { g.Key.PartSpec }).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        //显示采购申请物料级别
        public JsonResult CheckGetPartMaterial(string PartSpec)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = (from l in ys.PM_Material
                            where l.PartSpec == PartSpec
                            group l by new { l.PartMaterial } into g
                            select new { g.Key.PartMaterial }).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //判断是否有采购信息
        public ActionResult CheckPurlist(string partid, string Taskid)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    if (partid == "null")
                    {
                        partid = "0";
                    }
                    int tid = int.Parse(Taskid);
                    int pid = int.Parse(partid);
                    var values = ys.PM_TemporaryPurchaseMaterialList.Where(c => c.TaskID == tid && c.MaterialID == pid).FirstOrDefault();
                    if (values == null)
                    {
                        return Content("true");
                    }
                }
                return Content("false");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }

        }



        #endregion

        #region 采购部
        //确认采购分配
        public ActionResult CPurchase(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec PurchaseApplyConfirm @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //采购部确认视图
        public ActionResult CompletePurchase(string Taskid)
        {
            ViewData["taskidc"] = Taskid;
            ViewData["taskids"] = Taskid;
            int tid = int.Parse(Taskid);
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = (from p in ys.PM_PurchaseMaterialList
                            join s in ys.PM_PONO
                           on p.POID equals s.ID
                            where p.TaskID == tid
                            select new
                            {
                                s.ID,
                                s.PONO
                            }).Distinct().ToList();

                ViewBag.pn = list;
            }
            return View();
        }
        //显示采购合同
        public ActionResult CheckpurChaseOrder(string taskid, int page, int limit)
        {
            int tid = int.Parse(taskid);
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Taskid", tid);
                var list = ys.Database.SqlQuery<PoNoCheck_Result>("exec PoNoCheck @Taskid", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //生成采购订单
        public ActionResult ShenPurchase(string id)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@IDs", id);
                    ys.Database.ExecuteSqlCommand("exec GeneratPO @IDs", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }



        //循环生成合同号
        //public ActionResult ContractNo(string hao)
        //{
        //    try
        //    {
        //        string[] haoma = hao.Split(',');
        //        List<string> listd = new List<string>();
        //        foreach (var list in haoma)
        //        {
        //            listd.Add(list);
        //        }
        //        for (int w = 0; w < listd.Count; w++)
        //        {
        //            string xh = listd[w].ToString();
        //            using (YLMES_newEntities ys = new YLMES_newEntities())
        //            {
        //                SqlParameter[] parms = new SqlParameter[3];
        //                parms[0] = new SqlParameter("@TaskID", 999);
        //                parms[1] = new SqlParameter("@ListType", "");
        //                parms[2] = new SqlParameter("@IDs", xh);
        //                ys.Database.ExecuteSqlCommand("exec GeneratPO  @TaskID,@ListType,@IDs", parms);
        //            }
        //        }

        //        return Content("true");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return Content("false");
        //    }
        //}
        //发送到货通知
        public ActionResult SendNotice(string hao)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "notice");
                parms[1] = new SqlParameter("@PONO", hao);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec ImcommingPONotice   @Type,@PONO", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //发送审核通过
        public ActionResult SendAudit(string hao)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "audit");
                parms[1] = new SqlParameter("@PONO", hao);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec ImcommingPONotice  @Type,@PONO", parms);
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

        #region 生产部
        //确认生产和开始生产
        public ActionResult ProductStartConfirm(string Taskid)
        {
            try
            {
                int tid = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", tid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec ProductStartConfirm @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //完成生产任务
        public ActionResult ProductMS()
        {
            return View();
        }

        #endregion

        #region 订单处理中心确认收到订单

        public ActionResult SubmitOrder(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);

                var list = ys.C_Contract.Where(c => c.ID == i).FirstOrDefault();
                if (list.StatusID == "计划部订单处理中心确认收到生产订单")
                {
                    return Content("two");
                }
                else
                {
                    var con = ys.C_ContractProductDetail.Where(c => c.ContractID == i).ToList();
                    foreach (var s in con)
                    {
                        s.Status = "计划部订单处理中心确认收到生产订单";
                        ys.SaveChanges();
                    }
                    list.StatusID = "计划部订单处理中心确认收到生产订单";
                    ys.SaveChanges();
                    return Content("true");
                }

            }

        }

        #endregion

        #region 获取图纸名称

        public ActionResult CheckFigure(string pnum,string pesc,string pmater)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@PartNumber", pnum);
                parms[1] = new SqlParameter("@PartSpec", pesc);
                parms[2] = new SqlParameter("@PartMaterial", pmater);
                var list = ys.Database.SqlQuery<PM_CheckMaterID_Result>(" exec PM_CheckMaterID @PartNumber,@PartSpec,@PartMaterial", parms).FirstOrDefault();
                return Content(list.FileName);
            }
           
        }

        #endregion

        #region 上传或更新或查看图纸

        public ActionResult UploadTheDrawings(string hao, string name, string spec, string partMet)
        {
            ViewData["hao"] = hao;
            Session["names"] = name;
            Session["spec"] = spec;
            Session["partMet"] = partMet;
            return View();
        }
        public JsonResult FigureNumberlike(string FigureNumber)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_Figure_history.Where(p => p.FigureNumber.Contains(FigureNumber)).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 订单处理中心我的任务

        public ActionResult PTask()
        {
            return View();
        }

        #endregion

        #region 查询机械分配页面
        public ActionResult DesignDistribution(string ProjectName)
        {
            ViewData["pndd2"] = ProjectName;
            return View();
        }
        //显示机械分配信息
        public ActionResult DesignDistriCheck(string ProjectName)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ProjectName", ProjectName);
                var list = ys.Database.SqlQuery<DesignDistribution_Result>("exec DesignDistribution @ProjectName", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //显示工作查询页面
        public ActionResult WorkCheck()
        {
            return View();
        }
        //public JsonResult Departmentlist(string Name, int page, int limit)
        //{
        //    using (YLMES_newEntities ys = new YLMES_newEntities())
        //    {
        //        if (Name == null)
        //        {
        //            Name = "";
        //        }
        //        SqlParameter[] parms = new SqlParameter[2];
        //        parms[0] = new SqlParameter("@owner", Name);
        //        parms[1] = new SqlParameter("@Depart", "技术部");
        //        var list = ys.Database.SqlQuery<StatisticsDepartmentlist_Result>(" exec [StatisticsDepartmentlist]@owner,@Depart", parms).ToList();
        //        PageList<StatisticsDepartmentlist_Result> pageList = new PageList<StatisticsDepartmentlist_Result>(list, page, limit);
        //        Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
        //        int count = list.Count();
        //        hasmap.Add("code", 0);
        //        hasmap.Add("msg", "");
        //        hasmap.Add("count", count);
        //        hasmap.Add("data", pageList);
        //        return Json(hasmap, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion

        #region 显示收款信息

        public ActionResult Get_Datas(string CName, string CNumber, string strattime, string endtime, string rs, int page, int limit)
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
            if (rs == null)
            {
                rs = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "checksd");
                parms[1] = new SqlParameter("@CustomerName", CName);
                parms[2] = new SqlParameter("@ContractNumber", CNumber);
                parms[3] = new SqlParameter("@StatusID", "");
                parms[4] = new SqlParameter("@CreatedTimeStart", strattime);
                parms[5] = new SqlParameter("@CreatedTimeEnd", endtime);
                parms[6] = new SqlParameter("@ReviewStatus", rs);
                var list = ys.Database.SqlQuery<SP_ContractEdit_Result>("exec SP_ContractEdit @type,'',@CustomerName,@ContractNumber,'',1.00,'','','','','','','','',@StatusID,@CreatedTimeStart,@CreatedTimeEnd,'8.88','',@ReviewStatus", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_ContractEdit_Result> pageList = new PageList<SP_ContractEdit_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 显示订单处理中心完成页面

        public ActionResult FinishPTask()
        {
            return View();
        }

        #endregion

        #region 检测是否有库存

        public ActionResult CheckInventory(string Parid, string OrderCount, string taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Parid == "null")
                {
                    Parid = "0";
                }
                int pid = int.Parse(Parid);
                int onumber = int.Parse(OrderCount);
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@PartID", pid);
                var list = ys.Database.SqlQuery<CheckInventory_Result>(" exec [CheckInventory] @PartID", parms).ToList();
                int i = 0;
                int cid = 0;
                foreach (var num in list)
                {
                    int s=0;
                    if (num.StockQTY == null)
                    {
                        s = 0;
                    }
                    //cid = num.ChildPartID;
                    //if (s < (onumber*num.ChildPartQTY))
                    //{
                    //    string name = Session["name"].ToString();
                    //    SqlParameter[] parmd = new SqlParameter[5];
                    //    parmd[0] = new SqlParameter("@TaskID", tid);
                    //    parmd[1] = new SqlParameter("@MaterialID", cid);
                    //    parmd[2] = new SqlParameter("@Count", (onumber * num.ChildPartQTY));
                    //    parmd[3] = new SqlParameter("@UserName", name);
                    //    parmd[4] = new SqlParameter("@Parid", pid);
                    //    ys.Database.ExecuteSqlCommand("exec PM_AddPurlist  @TaskID,@MaterialID,@Count,@UserName,@Parid", parmd);
                    //    //SqlParameter[] parmsd = new SqlParameter[2];
                    //    //parmsd[0] = new SqlParameter("@TaskID", tid);
                    //    //parmsd[1] = new SqlParameter("@MaterialID", cid);
                    //    //ys.Database.ExecuteSqlCommand("exec PMCAskPurchase  @TaskID,@MaterialID", parmsd);
                    //    i = 1;
                    //}
                    //else
                    //{
                    //    PM_Material pm = ys.PM_Material.Where(c => c.ID == cid).FirstOrDefault();
                    //    pm.StockQTY = pm.StockQTY - onumber;
                    //    ys.SaveChanges();
                    //}
                }
                if (i == 0)
                {
                    return Content("true");
                }
                else
                {
                    PM_Material lid = ys.PM_Material.Where(p => p.ID == pid).FirstOrDefault();
                    lid.InventoryQuantity = "申请中";
                    ys.SaveChanges();
                    return Content("false");
                }
            }
        }
        #endregion

        #region 查看库存信息
        public ActionResult CheckInvent(string Parid)
        {
            ViewData["part"] = Parid;
            return View();
        }
        public ActionResult CheckInventers(string Parid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                int pid = int.Parse(Parid);
                parms[0] = new SqlParameter("@PartID", pid);
                var list = ys.Database.SqlQuery<CheckInventory_Result>(" exec [CheckInventory] @PartID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region 采购任务页面

        public ActionResult ProcurementTask()
        {
            return View();
        }
        //选择采购任务
        public ActionResult ProcurementMoreTask()
        {
            return View();
        }
        //完成采购任务
        public ActionResult CompleteTheProcurementTask()
        {
            return View();
        }
        //电气申请采购页面
        public ActionResult ApplyForPurchasing(string id)
        {
            return View();
        }
        #endregion

        #region 上传电气Excel

        public ActionResult UploadElectrical(List<Dictionary<string, string>> datas, string taskid,string pnumber,string pspec)
        {
            foreach (var data in datas)
            {
                data.Add("TaskID", taskid);
                data.Add("CreateBy", Session["name"].ToString());
                data.Add("pnumber", pnumber);
                data.Add("pspec", pspec);
                Tools<object>.SqlComm("exec AddElectriMerial ", data);
            }
            return Content("true");
        }
        //完成电气设计
        public ActionResult ConfirmTheElectDesign()
        {
            string tid = Session["Finish"].ToString();
            int id = int.Parse(tid);
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID", id);
                    ys.Database.ExecuteSqlCommand("exec UpdateCompletedMachineDesign  @TaskID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return Content("false");
            }

        }
        //完成电气审核
        public ActionResult UpdateEcectConfirm(string Taskid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int id = int.Parse(Taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID", id);
                ys.Database.ExecuteSqlCommand("exec updateIssueCompleted  @TaskID", parms);
                return Content("true");
            }
        }

        #endregion

        #region 计划部显示电气清单任务

        public ActionResult PlannedElectricalList(string id,string ProjectName,string pcName,string spec,string TaskType)
        {
            ViewData["taskid"] = id;
            ViewData["taskids"] = id;
            ViewData["pjname"] = ProjectName;
            ViewData["pcName"] = pcName;
            ViewData["spec"] = spec;
            ViewData["TaskType"] = TaskType;
            return View();
        }
        //计划部完成申请采购任务
        public ActionResult TaskElectricalCheck(string taskid,string count)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(taskid);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@taskid", tid);
                var list = ys.Database.SqlQuery<PM_CheckElectricalInv_Result>("exec PM_CheckElectricalInv  @taskid", parms).ToList();
                Dictionary<string, Object> hasmap= new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 查看特殊符号

        public ActionResult CheckSpecialSymbols()
        {
            return View();
        }

        #endregion

        #region 查询成品仓库存
        //显示成品仓库存页面
        public ActionResult GoodsInventory()
        {
            return View();
        }
        public JsonResult CheckInventorys(string Reservoir)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                if (Reservoir=="" || Reservoir == null)
                {
                    Dictionary<string, object> map = new Dictionary<string, object>()
                {
                    {"Location",ys.WarehouseLocation.ToList() }
                };
                    return Json(map, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Dictionary<string, object> map = new Dictionary<string, object>()
                {
                    {"Location",ys.WarehouseLocation.Where(c=>c.Reservoir==Reservoir).ToList() }
                };
                    return Json(map, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //联动显示信息
        public JsonResult LinkageShows()
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                Dictionary<string, object> map = new Dictionary<string, object>()
                {
                    {"Reservoir",ys.WarehouseLocation.ToList() }
                };
                return Json(map, JsonRequestBehavior.AllowGet);
            }            
        }    
        #endregion
    }
}