using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

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
                return File(filePath, "text/plain", file1);
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
        public ActionResult UpdateIssueCompleted(string Taskid)
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
                ViewData["ProductName"] = list.产品名称;
                ViewData["ProductNamed"] = list.产品名称;
                ViewData["TaskName"] = list.任务名称;
                ViewData["ProductSpec"] = list.产品规格;
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
            int ins = 0;
            try
            {
                var fileName1 = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
                file.SaveAs(fileName1);
                string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
                string fnames = fname.Substring(0, fname.LastIndexOf('.'));
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(taskid);
                    SqlParameter[] parm = new SqlParameter[2];
                    parm[0] = new SqlParameter("@TaskID", i);
                    parm[1] = new SqlParameter("@PartNumber", fnames);
                    var list = ys.Database.SqlQuery<checkBOMS_Result>("exec checkBOMS  @TaskID,@PartNumber", parm).FirstOrDefault();
                    if (string.IsNullOrEmpty(list.图号))
                    {
                        return Content("<script>alter('该部件没有图号不能上传')</script>");
                    }
                    else
                    {
                        string name = Session["name"].ToString();
                        SqlParameter[] parms = new SqlParameter[5];
                        parms[0] = new SqlParameter("@Type", "Uploaddrawings");
                        parms[1] = new SqlParameter("@FigureNumber", list.图号);
                        parms[2] = new SqlParameter("@FolderName", "Upload");
                        parms[3] = new SqlParameter("@FileName", fname);
                        parms[4] = new SqlParameter("@CreatedBy", name);
                        ys.Database.ExecuteSqlCommand("exec UploadTheDrawings  @Type,@FigureNumber,@FolderName,@FileName,@CreatedBy", parms);
                        ins = 0;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ins = 1;
                Dictionary<string, Object> hasmaps = new Dictionary<string, Object>();
                hasmaps.Add("code", ins);
                return Json(hasmaps, JsonRequestBehavior.AllowGet);
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

        #region 编辑机械和电气任务分配

        public ActionResult EditMachine(string PriorityCode, string TaskName, string select, string ProductName, string ProductSpec, string JieDate, string taskdesc, string MachineDesignOwner, string MachineDesignBaseTime, string MachineDesignBaseScore, string QualifiedOwner, string QualifiedBaseTime, string QualifiedBaseScore, string IssueOwner, string IssueBaseTime, string IssueBaseScore, string fileDownA, string fileDownB, string fileDownC)
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
                    SqlParameter[] parms = new SqlParameter[23];
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
                    ys.Database.ExecuteSqlCommand("exec updatelevel2Task  @TaskID,@PriorityCode,@TaskName,@ProductName,@ProductSpec,@Owner,@TaskScore,@DueDay,@TaskDesc,@MachineDesignOwner,@MachineDesignBaseTime," +
                        "@MachineDesignBaseScore,@QualifiedOwner,@QualifiedBaseTime,@QualifiedBaseScore,@IssueOwner,@IssueBaseTime,@IssueBaseScore,@Note,@electricalOwner,@electricalDesignBaseTime,@electricalDesignBaseScore,@TaskType", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region 导入EXECL文件到数据库
        public ActionResult StationImport(HttpPostedFileBase files)
        {
            try
            {
                if (files == null)
                {
                    return Content("<script>alert('图号不能为空');history.go(-1);</script>");
                }
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var fileName1 = Path.Combine(Request.MapPath("~/BOM"), Path.GetFileName(files.FileName));                 
                    files.SaveAs(fileName1);
                    string strConn;
                    string did = Session["Delete"].ToString();
                    int di = int.Parse(did);
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID", di);
                    string deleteAll = ys.Database.ExecuteSqlCommand("exec DeleteAll @TaskID", parms).ToString();
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + fileName1 + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
                    DataSet ds = new DataSet();
                    myCommand.Fill(ds, "ExcelInfo");
                    DataTable tab = ds.Tables["ExcelInfo"].DefaultView.ToTable();
                    for (int i = 1; i < tab.Rows.Count; i++)
                    {
                        SqlParameter[] parm = new SqlParameter[10];
                        parm[0] = new SqlParameter("@Level", tab.Rows[i][0].ToString());
                        parm[1] = new SqlParameter("@FigureNumber", tab.Rows[i][1].ToString());
                        parm[2] = new SqlParameter("@PartNumber", tab.Rows[i][2].ToString());
                        parm[3] = new SqlParameter("@PartSpec", tab.Rows[i][3].ToString());
                        parm[4] = new SqlParameter("@PartMaterial", tab.Rows[i][4].ToString());
                        parm[5] = new SqlParameter("@QTY", tab.Rows[i][5].ToString());
                        parm[6] = new SqlParameter("@Note", tab.Rows[i][6].ToString());
                        parm[7] = new SqlParameter("@Type", tab.Rows[i][7].ToString());
                        parm[8] = new SqlParameter("@ListType", "");
                        parm[9] = new SqlParameter("@TaskID", di);
                        ys.Database.ExecuteSqlCommand("exec AddMerial  @Level,@FigureNumber,@PartNumber,@PartSpec,@PartMaterial,@QTY,@Note,@Type,@ListType,@TaskID", parm);
                    }
                    string name = Session["name"].ToString();
                    SqlParameter[] parmd = new SqlParameter[2];
                    parmd[0] = new SqlParameter("@TaskID", di);
                    parmd[1] = new SqlParameter("@createdBy", name);
                    ys.Database.ExecuteSqlCommand("exec CreateBOM  @TaskID,@createdBy", parmd);
                    conn.Close();
                    System.IO.File.Delete(fileName1);
                    ViewData["dao"] = "1";
                }
                return View("FinishMyDesign");
            }
            catch (Exception ex)
            {
                var fileName1 = Path.Combine(Request.MapPath("~/BOM"), Path.GetFileName(files.FileName));
                string filePath = Server.MapPath(fileName1);
                System.IO.File.Delete(filePath);
                return Content("<script>alert('BOM清单格式不对');history.go(-1);</script>");
            }
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

        public ActionResult TaskMapPart(string TaskID, string ProductName, string ProductSpec)
        {
            Session["quetask"] = TaskID;
            //第1个页面
            Session["task1"] = TaskID;
            Session["ProductName1"] = ProductName;
            Session["ProductSpec1"] = ProductSpec;

            //第2个页面
            Session["task2"] = TaskID;
            Session["ProductName2"] = ProductName;
            Session["ProductSpec2"] = ProductSpec;


            ViewData["taskid"] = TaskID;
            ViewData["ProductName"] = ProductName;
            ViewData["ProductNamesd"] = ProductName;
            ViewData["ProductSpec"] = ProductSpec;
            return View();
        }
        public ActionResult TaskMapPart2(string TaskID, string ProductName, string ProductSpec, string zi)
        {
            Session["quetask2"] = TaskID;
            Session["task1"].ToString();
            Session["ProductName1"].ToString();
            Session["ProductSpec1"].ToString();
            Session["zi"] = zi;
            //第2个页面
            Session["task2"] = TaskID;
            Session["ProductName2"] = ProductName;
            Session["ProductSpec2"] = ProductSpec;



            return View();
        }

        //表格显示BOM
        public ActionResult CheckBOM(string PName)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ProjectName", PName);
                var list = ys.Database.SqlQuery<TaskMapingPartCheck_Result>("exec TaskMapingPartCheck @ProjectName", parms).ToList();
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
                    SqlParameter[] parms = new SqlParameter[3];
                    parms[0] = new SqlParameter("@id", i);
                    parms[1] = new SqlParameter("@dosage", dosage);
                    parms[2] = new SqlParameter("@unit", unit);
                    ys.Database.ExecuteSqlCommand("exec UpdateBOM  @id,@dosage,@unit", parms);
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
        public ActionResult CheckBOM2(string PName)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ProjectName", PName);
                var list = ys.Database.SqlQuery<TaskMapingPartCheck_Result>("exec TaskMapingPartCheck @ProjectName", parms).ToList();
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

        public ActionResult CompleteProcessSetting(string tastid)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int id = int.Parse(tastid);
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID", id);
                    ys.Database.ExecuteSqlCommand("exec UpdatePEcompletedTask  @TaskID", parms);
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

        #region 工艺流程
        //视图
        public ActionResult EditingProcess(string PartNumber, string ProductSpec, string ProductName, string PartId, string ifs)
        {
            ViewData["ifs"] = ifs;
            ViewData["PartNumber"] = PartNumber;
            ViewData["PartId"] = PartId;
            ViewData["ProductSpec"] = ProductSpec;
            ViewData["ProductName"] = ProductName;

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@PartNumber", PartNumber);
                ViewBag.rn = ys.Database.SqlQuery<CheckRouteName_Result>("exec CheckRouteName @PartNumber", param).ToList();


            }
            return View();
        }
        //显示流程
        [ValidateInput(false)]
        public ActionResult CheckProcess(string ProductName, string ProductSpec, string type, string PartNumber, string id, string RouteName)
        {
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
            ViewData["ProductName"] = ProductName;
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
            ViewData["TaskID2"] = id;
            ViewData["pjname"] = ProjectName;
            ViewData["pcName"] = pcName;
            ViewData["pcNamed"] = pcName;
            ViewData["spec"] = spec;
            Session["specs"] = spec;
            ViewData["TaskType"] = TaskType;
            ViewData["Choose"] = Choose;
            return View();
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
                hasmap.Add("data", pageList);
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
        public ActionResult TurnPwo(string pcName, string PartNumber, string TotalPCS, string TotalQTY, string QTYofOneSet, string TaskID, string ji, string spec)
        {
            try
            {
                int pcs = int.Parse(TotalPCS);
                int qty = int.Parse(TotalQTY);
                int set = int.Parse(QTYofOneSet);
                int tid = int.Parse(TaskID);
                string cSpec = Session["specs"].ToString();
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[11];
                parms[0] = new SqlParameter("@PartNumber", pcName);
                parms[1] = new SqlParameter("@TotalPCS", pcs);
                parms[2] = new SqlParameter("@QTYofOneSet", set);
                parms[3] = new SqlParameter("@TotalQTY", qty);
                parms[4] = new SqlParameter("@TaskID", tid);
                parms[5] = new SqlParameter("@ParentPartNumber", PartNumber);
                parms[6] = new SqlParameter("@DueDay", "");
                parms[7] = new SqlParameter("@CreatedEmployee", name);
                parms[8] = new SqlParameter("@Spec", spec);
                parms[9] = new SqlParameter("@Ji", ji);
                parms[10] = new SqlParameter("@cSpec", cSpec);
                // parms[8] = new SqlParameter("@MSG", "");
                //SqlParameter ptal = new SqlParameter("@MSG", "");
                //ptal.Direction = ParameterDirection.Output;
                //,@MSG out
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec AddWork @PartNumber,@TotalPCS,@QTYofOneSet,@TotalQTY,@TaskID,@ParentPartNumber,@DueDay,@CreatedEmployee,@Spec,@Ji,@cSpec", parms);
                    return Content("true");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //采购申请单
        public ActionResult Purchasing(string Taskid)
        {
            Session["purcha"] = Taskid;
            ViewData["purchas"] = Taskid;
            ViewData["purcha"] = Taskid;
            Session["purcha"] = Taskid;
            return View();
        }
        //显示申购
        public JsonResult CheckPurchase(string change, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(Session["purcha"].ToString());
                if (change == "全部")
                {
                    change = "";
                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@TaskID", tid);
                parms[1] = new SqlParameter("@ListType", change);
                var list = ys.Database.SqlQuery<PurchaseQTYcheck_Result>("exec PurchaseQTYcheck @TaskID,@ListType", parms).ToList();
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
        //修改申购
        public ActionResult EditPurchase(string taskid, string matrterID, string ApplayPcs, string PcsNumber, string unit, string note)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[7];
                int pcNumber = int.Parse(PcsNumber);
                int id = int.Parse(taskid);
                int tct = int.Parse(ApplayPcs);
                int mid = int.Parse(matrterID);
                parms[0] = new SqlParameter("@QTYofPCS", pcNumber);
                parms[1] = new SqlParameter("@MaterialID", mid);
                parms[2] = new SqlParameter("@ApplyPurchasePCS", tct);
                parms[3] = new SqlParameter("@TaskID", id);
                parms[4] = new SqlParameter("@Units", unit);
                parms[5] = new SqlParameter("@Note", note);
                parms[6] = new SqlParameter("@type", "edit");
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
        //编辑申购
        public ActionResult EditPurchased(string taskid, string shenPcs, string pNumber, string ActSpc, string Unit, string note, string Price)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[10];
                int pcNumber = int.Parse(pNumber);
                int id = int.Parse(taskid);
                int tct = int.Parse(shenPcs);
                int pices = int.Parse(Price);
                int spcs = int.Parse(ActSpc);
                parms[0] = new SqlParameter("@QTYofPCS", pcNumber);
                parms[1] = new SqlParameter("@MaterialID", 666);
                parms[2] = new SqlParameter("@ApplyPurchasePCS", tct);
                parms[3] = new SqlParameter("@TaskID", 666);
                parms[4] = new SqlParameter("@Units", Unit);
                parms[5] = new SqlParameter("@Note", note);
                parms[6] = new SqlParameter("@type", "update");
                parms[7] = new SqlParameter("@ActPCS", spcs);
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
        //新增采购信息
        public ActionResult AddPurlist(string Material, string PartSpec, string PartMaterial, string ApplyPCS, string qtyofPCS, string type, string Taskid, string unit, string desc)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                try
                {
                    int tid = int.Parse(Taskid);
                    int pcs = int.Parse(ApplyPCS);
                    int qtypcs = int.Parse(qtyofPCS);
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[10];
                    parms[0] = new SqlParameter("@TaskID", tid);
                    parms[1] = new SqlParameter("@Material", Material);
                    parms[2] = new SqlParameter("@PartSpec", PartSpec);
                    parms[3] = new SqlParameter("@PartMaterial", PartMaterial);
                    parms[4] = new SqlParameter("@ApplyPCS", pcs);
                    parms[5] = new SqlParameter("@qtyofPCS", qtypcs);
                    parms[6] = new SqlParameter("@ListType", type);
                    parms[7] = new SqlParameter("@Units", unit);
                    parms[8] = new SqlParameter("@Note", desc);
                    parms[9] = new SqlParameter("@UserName", name);
                    ys.Database.ExecuteSqlCommand("exec PM_AddPurlist  @TaskID,@Material,@PartSpec,@PartMaterial,@ApplyPCS,@qtyofPCS,@ListType,@Units,@Note,@UserName", parms);
                    return Content("true");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Content("false");
                }
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

        #region 上传或更新或查看图纸

        public ActionResult UploadTheDrawings(string hao)
        {
            ViewData["hao"] = hao;
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
        public JsonResult Departmentlist(string Name, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Name == null)
                {
                    Name = "";
                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@owner", Name);
                parms[1] = new SqlParameter("@Depart", "技术部");
                var list = ys.Database.SqlQuery<StatisticsDepartmentlist_Result>(" exec [StatisticsDepartmentlist]@owner,@Depart", parms).ToList();
                PageList<StatisticsDepartmentlist_Result> pageList = new PageList<StatisticsDepartmentlist_Result>(list, page, limit);
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

        #region 显示收款信息

        public ActionResult Get_Datas(string CName, string CNumber,string strattime, string endtime, string rs, int page, int limit)
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
    }
}