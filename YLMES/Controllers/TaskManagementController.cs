using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
            TempData["cn"] = customerName;
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
        public FileResult FileADownload(string file1)
        {
            string filePath = Server.MapPath(file1);
            return File(filePath, "text/plain", file1);
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
                    ys.Database.ExecuteSqlCommand("exec UpdateMachineDesignConfirm @TaskID,@Type", parms);
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

        public ActionResult FinishMyDesign(string ifDrive, string name)
        {
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
        public ActionResult IwantMoreTask(int page,int limit)
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
        public ActionResult UpdateTaskForChangeOwner(string task)
        {
            try
            {
                string name = Session["name"].ToString();
                string[] id = task.Split(',');
                List<string> listd = new List<string>();
                foreach (var list in id)
                {
                    listd.Add(list);
                }
                for (int w = 0; w < listd.Count; w++)
                {
                    string bianhao = listd[w].ToString();
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        SqlParameter[] parms = new SqlParameter[2];
                        parms[0] = new SqlParameter("@Owner", name);
                        parms[1] = new SqlParameter("@TaskID", bianhao);
                        ys.Database.ExecuteSqlCommand("exec UpdateTaskForChangeOwner  @Owner,@TaskID", parms);
                    }
                }                            
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //插入审核任务
        public ActionResult updateTFCQO(string task)
        {
            try
            {
                string name = Session["name"].ToString();
                string[] id = task.Split(',');
                List<string> listd = new List<string>();
                foreach (var list in id)
                {
                    listd.Add(list);
                }
                for (int w = 0; w < listd.Count; w++)
                {
                    string bianhao = listd[w].ToString();
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        SqlParameter[] parms = new SqlParameter[2];
                        parms[0] = new SqlParameter("@Owner", name);
                        parms[1] = new SqlParameter("@TaskID", bianhao);
                        ys.Database.ExecuteSqlCommand("exec TaskForChangeQualifiedOwner  @Owner,@TaskID", parms);
                    }
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //插入图纸发行任务
        public ActionResult UpdateTFCIO(string task)
        {
            try
            {
                string name = Session["name"].ToString();
                string[] id = task.Split(',');
                List<string> listd = new List<string>();
                foreach (var list in id)
                {
                    listd.Add(list);
                }
                for (int w = 0; w < listd.Count; w++)
                {
                    string bianhao = listd[w].ToString();
                    using (YLMES_newEntities ys = new YLMES_newEntities())
                    {
                        SqlParameter[] parms = new SqlParameter[2];
                        parms[0] = new SqlParameter("@Owner", name);
                        parms[1] = new SqlParameter("@TaskID", bianhao);
                        ys.Database.ExecuteSqlCommand("exec UpdateTaskForChangeIssueOwner  @Owner,@TaskID", parms);
                    }
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
                ViewData["QualifiedOwner"] = list.审核人;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["MachineDesignBaseScore"] = list.机械设计分值;
                ViewData["MachineDesignBaseTime"] = list.机械设计基准时间;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
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
                ViewData["MachineDesignOwner"] = list.电气设计人;
                ViewData["QualifiedOwner"] = list.审核人;
                ViewData["IssueOwner"] = list.发行人;
                ViewData["MachineDesignBaseScore"] = list.电气设计分值;
                ViewData["MachineDesignBaseTime"] = list.电气设计基准时间;
                ViewData["QualifiedBaseScore"] = list.审核分值;
                ViewData["QualifiedBaseTime"] = list.审核基准时间;
                ViewData["IssueBaseScore"] = list.发行分值;
                ViewData["IssueBaseTime"] = list.发行基准时间;
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

        #region 显示已完成任务信息

        public JsonResult CompletedTasksJson(string StartTime, string endtime, string ProjectName, string TaskName, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Owner", "");
                parms[1] = new SqlParameter("@Status", "");
                DateTime dts = Convert.ToDateTime(StartTime);
                parms[2] = new SqlParameter("@StartTime", dts);
                DateTime dte = Convert.ToDateTime(endtime);
                parms[3] = new SqlParameter("@EndTime", dte);
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

        public JsonResult TpA()
        {
            string cn = TempData["cn"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ProjectName", cn);
                var list = ys.Database.SqlQuery<PM_ProjectCheckTask2_Result>("exec PM_ProjectCheckTask2  @ProjectName", parms).ToList();

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

        #region 分页显示任务分配

        public JsonResult TaskJson(string RwapStatusID, string CreatedTimeStart, string SalesOrderd, string ProjectName, string Status, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@RwapStatusID", RwapStatusID);
                parms[1] = new SqlParameter("@CreatedTimeStart", CreatedTimeStart);
                parms[2] = new SqlParameter("@SalesOrder", SalesOrderd);
                parms[3] = new SqlParameter("@ProjectName", ProjectName);
                if (Status == "全部")
                {
                    Status = "";
                }
                if (Status == "已完成")
                {
                    Status = "已完成";
                }
                if (Status == "未开始")
                {
                    Status = "未开始";
                }
                if (Status == "正在进行中")
                {
                    Status = "进行中";
                }
                parms[4] = new SqlParameter("@StatusID", Status);
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

        public ActionResult EditMachine(string PriorityCode, string TaskName, string ProductName, string ProductSpec, string JieDate, string taskdesc, string MachineDesignOwner, string MachineDesignBaseTime, string MachineDesignBaseScore, string QualifiedOwner, string QualifiedBaseTime, string QualifiedBaseScore, string IssueOwner, string IssueBaseTime, string IssueBaseScore, string fileDownA, string fileDownB, string fileDownC)
        {
            try
            {
                string takid = Session["taskid"].ToString();
                int tk = int.Parse(takid);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[22];
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
                    ys.Database.ExecuteSqlCommand("exec updatelevel2Task  @TaskID,@PriorityCode,@TaskName,@ProductName,@ProductSpec,@Owner,@TaskScore,@DueDay,@TaskDesc,@MachineDesignOwner,@MachineDesignBaseTime," +
                        "@MachineDesignBaseScore,@QualifiedOwner,@QualifiedBaseTime,@QualifiedBaseScore,@IssueOwner,@IssueBaseTime,@IssueBaseScore,@Note,@electricalOwner,@electricalDesignBaseTime,@electricalDesignBaseScore", parms);
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
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + fileName1 + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
                    DataSet ds = new DataSet();
                    myCommand.Fill(ds, "ExcelInfo");
                    DataTable tab = ds.Tables["ExcelInfo"].DefaultView.ToTable();
                    for (int i = 0; i < tab.Rows.Count; i++)
                    {
                        SqlParameter[] parm = new SqlParameter[9];
                        parm[0] = new SqlParameter("@Level", tab.Rows[i][0].ToString());
                        parm[1] = new SqlParameter("@FigureNumber", tab.Rows[i][1].ToString());
                        parm[2] = new SqlParameter("@PartNumber", tab.Rows[i][2].ToString());
                        parm[3] = new SqlParameter("@PartSpec", tab.Rows[i][3].ToString());
                        parm[4] = new SqlParameter("@PartMaterial", tab.Rows[i][4].ToString());
                        parm[5] = new SqlParameter("@QTY", tab.Rows[i][5].ToString());
                        parm[6] = new SqlParameter("@Note", tab.Rows[i][6].ToString());
                        parm[7] = new SqlParameter("@ListType", "");
                        parm[8] = new SqlParameter("@TaskID", di);
                        ys.Database.ExecuteSqlCommand("exec AddMerial  @Level,@FigureNumber,@PartNumber,@PartSpec,@PartMaterial,@QTY,@Note,@ListType,@TaskID", parm);
                    }
                    string name = Session["name"].ToString();
                    SqlParameter[] parmd = new SqlParameter[2];
                    parmd[0] = new SqlParameter("@TaskID", di);
                    parmd[1] = new SqlParameter("@createdBy", name);
                    ys.Database.ExecuteSqlCommand("exec CreateBOM  @TaskID,@createdBy", parmd);
                    conn.Close();
                    System.IO.File.Delete(fileName1);
                }
                return View("FinishMyDesign");
            }
            catch (Exception ex)
            {
                return View("FinishMyDesign");
            }


        }

        #endregion

        #region 显示BOM信息

        public JsonResult CheckBOMD()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string id = Session["Check"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@TaskID",i);
                var list = ys.Database.SqlQuery<checkBOM_Result>("exec checkBOM  @TaskID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 上传或更新图纸
        //视图
        public ActionResult UploadPicture(string hao)
        {
            ViewData["tuhao"] = hao;
         
            return View();
        }
        //插入图纸
        public ActionResult UploadTheDrawings(string TuHao, HttpPostedFileBase file)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string name = Session["name"].ToString();
                    var fileName1 = Path.Combine(Request.MapPath("~/Picture"), Path.GetFileName(file.FileName));
                    file.SaveAs(fileName1);
                    string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@TYPE", "update");
                    parms[1] = new SqlParameter("@ID", 10000);
                    parms[2] = new SqlParameter("@FigureNumber",TuHao);
                    parms[3] = new SqlParameter("@FolderName", "Picture");
                    parms[4] = new SqlParameter("@FileName", fname);
                    parms[5] = new SqlParameter("@CreatedBy", name);
                    parms[6] = new SqlParameter("@Status","");
                    ys.Database.ExecuteSqlCommand("exec SP_PM_Figure  @TYPE,@ID,@FigureNumber,@FolderName,@FileName,@CreatedBy,@Status", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }          
        }
        //查看图纸
        public ActionResult UpdatePicture(string hao)
        {
            ViewData["cTuhao"] = hao;
            using(YLMES_newEntities ys = new YLMES_newEntities()){
                var file = ys.PM_Figure.Where(p => p.FigureNumber==hao).FirstOrDefault();
                ViewData["file"] = file.FileName;
                Session["filed"] = file.FileName;
                string fileds = "/Picture/" + file.FileName;
                TempData["tupian"] = fileds;
            }
           
            return View();
        }
        //更新图纸 
        public ActionResult UpdateTheDrawing(string TuHao, HttpPostedFileBase file)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string filed = Session["filed"].ToString();
                    if (!string.IsNullOrEmpty(filed))
                    {
                        string fileds = "~/Picture/" + filed;
                        string filePath = Server.MapPath(fileds);
                        System.IO.File.Delete(filePath);
                    }
                    var fileName1 = Path.Combine(Request.MapPath("~/Picture"), Path.GetFileName(file.FileName));
                    file.SaveAs(fileName1);
                    string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@TYPE","updated");
                    parms[1] = new SqlParameter("@ID", 10000);
                    parms[2] = new SqlParameter("@FigureNumber", TuHao);
                    parms[3] = new SqlParameter("@FolderName", "");
                    parms[4] = new SqlParameter("@FileName", fname);
                    parms[5] = new SqlParameter("@CreatedBy", "");
                    parms[6] = new SqlParameter("@Status", "");
                    ys.Database.ExecuteSqlCommand("exec SP_PM_Figure  @TYPE,@ID,@FigureNumber,@FolderName,@FileName,@CreatedBy,@Status", parms);
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

        #region 确认完成我的设计

        public ActionResult ConfirmTheDesign()
        {
            try
            {
            string type=Session["type"].ToString();
            string tid = Session["Finish"].ToString();
            int id = int.Parse(tid);
            if (type == "标准")
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                  
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID",id);
                    ys.Database.ExecuteSqlCommand("exec updateIssueCompleted  @TaskID",parms);
                }
            }
            else
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@TaskID", id);
                    ys.Database.ExecuteSqlCommand("exec UpdateCompletedMachineDesign  @TaskID",parms);
                }
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
                parms[0] = new SqlParameter("@Taskid",i);
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
                    parms[1] = new SqlParameter("@TaskId",tId);
                    parms[2] = new SqlParameter("@ProgressItem",Item);
                    parms[3] = new SqlParameter("@TimeFrom",From);
                    parms[4] = new SqlParameter("@TimeTO",TO);
                    parms[5] = new SqlParameter("@ChildTaskDescription",ChildTaskDescription);
                    parms[6] = new SqlParameter("@statusDescription",StatusDescription);
                    parms[7] = new SqlParameter("@UserName",name);
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
                int id =int.Parse(tid);
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
                    parms[4] = new SqlParameter("@Content",ChildTaskDescription);
                    parms[5] = new SqlParameter("@CompletionDescription",StatusDescription);
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
        public ActionResult TaskMapPart2(string TaskID, string ProductName, string ProductSpec,string zi)
        {
            Session["task1"].ToString();
            Session["ProductName1"].ToString();
            Session["ProductSpec1"].ToString();
            Session["zi"]=zi;
            //第2个页面
            Session["task2"] = TaskID;
            Session["ProductName2"] = ProductName;
            Session["ProductSpec2"] = ProductSpec;


          
            return View();
        }

        //表格显示BOM
        public ActionResult CheckBOM()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ProjectName", "YW7615-C6型轨道游移万向推车dd");
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
        public ActionResult UpdateBOM(string part, string child, string dosage, string unit)
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
                    parms[1] = new SqlParameter("@ID",pId);
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
        public ActionResult EditingProcess(string PartNumber)
        {
            ViewData["pn"] = PartNumber;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                ViewBag.rn = ys.Database.SqlQuery<CheckRouteName_Result>("exec CheckRouteName").ToList();
            }
            return View();
        }
        //显示流程
        public ActionResult CheckProcess(string pn)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                //string name = Session["name"].ToString();
                parms[0] = new SqlParameter("@PartNumber", pn);
                var list = ys.Database.SqlQuery<CheckProcess_Result>("exec CheckProcess @PartNumber", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();          
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //删除流程
        public ActionResult DeleteProcess(string id)
        {
            try
            {
                int i = int.Parse(id);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[1];
                    parms[0] = new SqlParameter("@ID", i);
                    ys.Database.ExecuteSqlCommand("exec DeleteProcess  @ID", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
           
        }
        //修改流程
        public ActionResult UpdateProcess(string xuhao, string type, string dantao, string fuzhu, string yaoqiu,string id)
        {
            try
            {
                int i = int.Parse(id);
                int hao = int.Parse(xuhao);
                string name = Session["name"].ToString();
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[7];
                    parms[0] = new SqlParameter("@StationType", type);
                    parms[1] = new SqlParameter("@SortID", hao);
                    parms[2] = new SqlParameter("@WorkHours", dantao);
                    parms[3] = new SqlParameter("@WorkHours2", fuzhu);
                    parms[4] = new SqlParameter("@require", yaoqiu);
                    parms[5] = new SqlParameter("@ID", i);
                    parms[6] = new SqlParameter("@CreatedBy", name);
                    ys.Database.ExecuteSqlCommand("exec UpdateProcess  @StationType,@SortID,@WorkHours,@WorkHours2,@require,@ID,@CreatedBy", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }

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
        //新增流程站点
        public ActionResult AddRoute(string id)
        {
            try
            {
                int i = int.Parse(id);
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Route", i);
                parms[1] = new SqlParameter("@CreatedBy", name);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec AddRoute @Route,@CreatedBy", parms);
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
        //完成计划
        public ActionResult PMCSettingWO(string id, string ProjectName, string pcName, string spec, string TaskType, string Choose)
        {
            ViewData["taskid"] = id;
            ViewData["pjname"] = ProjectName;
            ViewData["pcName"] = pcName;
            ViewData["pcNamed"] = pcName;
            ViewData["spec"] = spec;
            ViewData["TaskType"] = TaskType;
            ViewData["Choose"] = Choose;
            return View();
        }
        //显示工单信息
        public ActionResult TaskMapingWOCheck(string taskid, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(taskid);
                Session["tide"] = taskid;
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
        public ActionResult TurnPwo(string pcName, string PartNumber, string TotalPCS, string TotalQTY, string QTYofOneSet, string TaskID)
        {
            try
            {
                int pcs = int.Parse(TotalPCS);
                int qty = int.Parse(TotalQTY);
                int set = int.Parse(QTYofOneSet);
                int tid = int.Parse(TaskID);
                string name = Session["name"].ToString();
                string msg = "";
                SqlParameter[] parms = new SqlParameter[8];
                parms[0] = new SqlParameter("@PartNumber", PartNumber);
                parms[1] = new SqlParameter("@TotalPCS", pcs);
                parms[2] = new SqlParameter("@QTYofOneSet", set);
                parms[3] = new SqlParameter("@TotalQTY", qty);
                parms[4] = new SqlParameter("@TaskID", tid);
                parms[5] = new SqlParameter("@ParentPartNumber", pcName);
                parms[6] = new SqlParameter("@DueDay", "");
                parms[7] = new SqlParameter("@CreatedEmployee", name);
               // parms[8] = new SqlParameter("@MSG", "");
                //SqlParameter ptal = new SqlParameter("@MSG", "");
                //ptal.Direction = ParameterDirection.Output;
                //,@MSG out
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec AddWork @PartNumber,@TotalPCS,@QTYofOneSet,@TotalQTY,@TaskID,@ParentPartNumber,@DueDay,@CreatedEmployee", parms);
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
        public ActionResult Purchasing(string ifDriver, string Taskid)
        {
            Session["taskid"] = Taskid;
            return View();
        }
        //显示申购
        public ActionResult CheckPurchase(string change, string Taskid,int page,int limit)
        {
         
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int tid = int.Parse(Taskid);
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
        public ActionResult EditPurchase(string afps, string QRequested, string unit, string desic,string tid)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[5];
                int fps = int.Parse(afps);
                int count = int.Parse(QRequested);
                int id = int.Parse(tid);
                parms[0] = new SqlParameter("@ActPurchasePCS",fps);
                parms[1] = new SqlParameter("@Purchasepcs", count);
                parms[2] = new SqlParameter("@TaskID", id);
                parms[3] = new SqlParameter("@Units", unit);
                parms[4] = new SqlParameter("@Note", desic);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec UpdatePurchaseQTY  @ActPurchasePCS,@Purchasepcs,@TaskID,@Units,@Note", parms);
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
        public ActionResult CompletePurchase(string type, string id, string pjname)
        {
            ViewData["types"] = type;
            ViewData["tid"] = id;
            ViewData["pjname"] = pjname;
            int tid = int.Parse(id);
            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@TaskID", tid);
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
                           
                           
                //var list = ys.Database.SqlQuery<McomingPO_Result>("exec ImcomingPOcheck @TaskID", parms).ToList();
                ViewBag.pn = list;
            }
            return View();
        }
        //显示采购合同
        //public ActionResult CheckpurChaseOrder(string PONOID,int page,int limit)
        //{
        //    int pid = int.Parse(PONOID);
        //    using (YLMES_newEntities ys = new YLMES_newEntities())
        //    {             
        //        SqlParameter[] parms = new SqlParameter[1];
        //        parms[0] = new SqlParameter("@PONOID", pid);
        //        var list = ys.Database.SqlQuery<POcheck_Result>("exec POcheck @PONOID", parms).ToList();
        //        Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
        //        PageList<POcheck_Result> pageList = new PageList<POcheck_Result>(list, page, limit);
        //        int count = list.Count();
        //        hasmap.Add("code", 0);
        //        hasmap.Add("msg", "");
        //        hasmap.Add("count", count);
        //        hasmap.Add("data", pageList);
        //        return Json(hasmap, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //循环生成合同号
        public ActionResult ContractNo(string hao)
        {
            try
            {
                string[] haoma = hao.Split(',');
                List<string> listd = new List<string>();              
                    foreach (var list in haoma)
                    {
                        listd.Add(list);
                    }
                    for (int w = 0; w < listd.Count; w++)
                    {
                        string xh = listd[w].ToString();
                        using (YLMES_newEntities ys = new YLMES_newEntities())
                        {
                            SqlParameter[] parms = new SqlParameter[3];
                            parms[0] = new SqlParameter("@TaskID", 999);
                            parms[1] = new SqlParameter("@ListType", "");
                            parms[2] = new SqlParameter("@IDs", xh);
                            ys.Database.ExecuteSqlCommand("exec GeneratPO  @TaskID,@ListType,@IDs", parms);
                        }
                    }
                
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //发送到货通知
        public ActionResult SendNotice(string hao)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@PONO", hao);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec ImcommingPONotice  @PONO", parms);
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

  
    }
}