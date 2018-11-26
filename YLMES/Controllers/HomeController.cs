using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using YLMES.Models;

namespace YlMES.Controllers
{
    public class HomeController : Controller
    {


        string connString = "Data Source=192.168.1.251;Initial Catalog=KQXT;User ID=admin;Password=admin123";
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Top()
        {
            return View();
        }
        public ActionResult CheckInfo()
        {
            return View();
        }
        public ActionResult ContractAdd()
        {
            SeTableStatus();
            return View();
        }
        public ActionResult notice(string id)
        {
            TempData["notice"] = id;
            ViewData["ids"] = id;
            ViewData["idw"] = id;
            return View();
        }

        //查询状态
        public void SeTableStatus() {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<TableStatus> list = ys.TableStatus.ToList();
                ViewBag.ck = list;

            }
        }
        public ActionResult VarCheckInfoAdd()
        {
        
            return View();
        }

        public ActionResult left()
        {
            return View();
        }
        public ActionResult Menu()
        {

            return View();
        }

        public ActionResult HDetailsAdd()
        {
            return View();
        }
        public ActionResult HDetailsAddd()
        {
            return View();
        }
        public ActionResult SalesCreation()
        {
            SeTableStatus();
            return View();
        }
        public ActionResult DetialsEdit(string id, string idws)
        {
            //合同详细信息显示
            DetialsXianShi(id, idws);      
            return View();
        }
        //销售合同收款
        public ActionResult PM_ContractReceivablesMain()
        {
            SeTableStatus();
            return View();
        }


        //合同查询
        public ActionResult PM_Contract_Check()
        {
            SeTableStatus();
            return View();
        }
        //收款
        public ActionResult Gathering(string id, string CustomerName, string ContractNumber)
        {
            ViewData["Receivables"] = id;
            ViewData["CNames"] = CustomerName;
            ViewData["CNumbers"] = ContractNumber;
            return View();
        }
        public ActionResult Contract_Check(string id)
        {
            
          
            ViewData["ContractDetialsCheckId"] = id;
            XianShiContract(id);
            return View();
        }
        public ActionResult HDetials(string id)
        {
            TempData["d"] = id;
            ViewData["ids"] = id;
            Session["Cid"] = id;
            return View();


        }
        

        int Did = 0;
        public ActionResult CDetialsd()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Did = Convert.ToInt32(TempData["d"]);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@type", "ProductDetail_check");
                parms[1] = new SqlParameter("@ContractID", Did);
                parms[2] = new SqlParameter("@ProductDetailID", 2);
                var list = ys.Database.SqlQuery<SP_Contract_ProductDetails_Result>("exec SP_Contract_ProductDetails  @type,@ContractID,@ProductDetailID", parms).ToList();

                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                foreach (var isdu in list)
                {
                    Session["BianHao"] = isdu.合同编号;
                }
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Contract_Checks(string id)
        {
            XianShiContract(id);
            return View();
        }
        #region 发送通知

      
        #endregion


        #region 提交合同状态

        public ActionResult SubmitMethod(string id)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);
                    var con = ys.C_Contract.Where(c => c.ID == i).FirstOrDefault();
                    con.StatusID = "销售部提交合同至财务";
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

        #endregion

        #region 申请提交

        public ActionResult Apply(string id)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);
                    var con = ys.C_Contract.Where(c => c.ID == i).FirstOrDefault();
                    con.AuditThrough = "申请修改";
                    ys.SaveChanges();
                }
                return Content("true");
            }
            catch(Exception ex){
                return Content("false");
            }
        }

        #endregion

        #region 申请审核

        public ActionResult Audit(string id)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(id);
                    string name = Session["name"].ToString();
                    if (name == "11")
                    {
                        var con = ys.C_Contract.Where(c => c.ID == i).FirstOrDefault();
                        con.AuditThrough = "审核通过";
                        ys.SaveChanges();
                        return Content("true");
                    }
                    else
                    {
                        return Content("er");
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

        #region 显示合同信息

        public ActionResult Contract_Check_Main(string id)
        {

            XianShiContract(id);
            ViewData["ContractDetialsCheckId"] = id;
            return View();
        }

        #endregion

        #region 显示和修改合同信息
        public ActionResult EditContract(string id)
        {
           
               
                XianShiContract(id);
                ViewData["ContractDetialsCheckId"] = id;
            return View();
        }
       
        public ActionResult EditContractd(string id,string CuName, string CuNumber, string Money, string IfInstall, string IfIncludeTax,string DateOfSign,string AmountCollected,string IfTransport,string select,string Pay,string SendDate,string weiyuetiaojian,string CZongJie,string CreatedTime)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {                   
                    double money = double.Parse(Money);
                    SqlParameter[] parms = new SqlParameter[20];
                    parms[0] = new SqlParameter("@type", "update");
                    parms[1] = new SqlParameter("@ID", id);
                    parms[2] = new SqlParameter("@CustomerName", CuName);
                    parms[3] = new SqlParameter("@ContractNumber", CuNumber);
                    parms[4] = new SqlParameter("@DateOfSign", DateOfSign);
                    parms[5] = new SqlParameter("@Money", money);
                    parms[6] = new SqlParameter("@PaymentMethod",Pay);
                    parms[7] = new SqlParameter("@IfInstall", IfInstall);
                    parms[8] = new SqlParameter("@IfTransport", IfTransport);
                    parms[9] = new SqlParameter("@IfIncludeTax", IfIncludeTax);
                    parms[10] = new SqlParameter("@DeliveryTime", SendDate);
                    parms[11] = new SqlParameter("@ConditionsOfbreachOfContract", weiyuetiaojian);
                    parms[12] = new SqlParameter("@Summary", CZongJie);
                    parms[13] = new SqlParameter("@CreatedBy", "");
                    parms[14] = new SqlParameter("@CreatedTime",CreatedTime);
                    parms[15] = new SqlParameter("@StatusID",select);
                    parms[16] = new SqlParameter("@CreatedTimeStart", "");
                    parms[17] = new SqlParameter("@CreatedTimeEnd", "");
                    parms[18] = new SqlParameter("@AmountCollected", 927335.85);
                    parms[19] = new SqlParameter("@ProductOrderStatus", "");
                    ys.Database.ExecuteSqlCommand("exec SP_ContractEdit @type,@ID,@CustomerName,@ContractNumber,@DateOfSign,@Money,@PaymentMethod,@IfInstall,@IfTransport,@IfIncludeTax,@DeliveryTime," +
                   "@ConditionsOfbreachOfContract,@Summary,@CreatedBy,@CreatedTime,@StatusID,@CreatedTimeStart,@CreatedTimeEnd,@AmountCollected,@ProductOrderStatus", parms);
                    int i = int.Parse(id);
                    var st = ys.C_Contract.Where(c => c.ID == i).First();
                    st.AuditThrough = "";
                    ys.SaveChanges();
                }
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content("false");
            }
           
        }
        #endregion

        #region 将合同变成审核通过

        public ActionResult EditStatus(string bianhao)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities()){
                    C_Contract contract = ys.C_Contract.Where(p => p.ContractNumber == bianhao).FirstOrDefault();
                    contract.StatusID = "财务部合同审核通过";
                    ys.SaveChanges();
                }
               
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region  分页显示加查询
        public ActionResult Get_Data(string CName, string CNumber, string Status, string strattime, string endtime,string rs, int page, int limit)
        {
            if (Status.Equals("全部")) {

                Status = "";
            }
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "check");
                parms[1] = new SqlParameter("@CustomerName", CName);
                parms[2] = new SqlParameter("@ContractNumber", CNumber);
                parms[3] = new SqlParameter("@StatusID", Status);
                parms[4] = new SqlParameter("@CreatedTimeStart", strattime);
                parms[5] = new SqlParameter("@CreatedTimeEnd", endtime);
                parms[6] = new SqlParameter("@ReviewStatus", rs);
                var list = ys.Database.SqlQuery<SP_ContractEdit_Result>("exec SP_ContractEdit @type,'',@CustomerName,@ContractNumber,'',1.00,'','','','','','','','','',@StatusID,@CreatedTimeStart,@CreatedTimeEnd,'8.88','',@ReviewStatus", parms).ToList();
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

        #region 验证登陆
        public ActionResult VaLogin()
        {
            string boold = "false";
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                string uName = Request["username"].ToString();
                string uPwd = Request["password"].ToString();
                string sqlStr = "select * from  [Employee]   where [username]='" + uName + "' and [PWD] ='" + uPwd + "' ";
                SqlCommand Mycommand = new SqlCommand(sqlStr, conn);
                SqlDataReader dr = Mycommand.ExecuteReader();
                if (dr.Read())
                {
                    Session["name"] = uName;
                    boold = "True";
                }


            }
            catch (Exception ex)
            {
                boold = "false";
            }
            finally
            {
                conn.Close();
            }
            return Content(boold);
        }
        #endregion

        #region 循环删除

        public ActionResult DeleteList(string id)
        {
            string[] strid = id.Split(',');
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = 0;
                foreach (var item in strid)
                {
                    SqlParameter[] parms = new SqlParameter[20];
                    parms[0] = new SqlParameter("@type", "delete");
                    parms[1] = new SqlParameter("@ID", item);
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
                    i = ys.Database.ExecuteSqlCommand("exec SP_ContractEdit @type,@ID,@CustomerName,@ContractNumber,@DateOfSign,@Money,@PaymentMethod,@IfInstall,@IfTransport,@IfIncludeTax,@DeliveryTime," +
                   "@ConditionsOfbreachOfContract,@Summary,@CreatedBy,@CreatedTime,@StatusID,@CreatedTimeStart,@CreatedTimeEnd,@AmountCollected,@ProductOrderStatus", parms);
                }
                if (i > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }


        #endregion

        #region 删除合同

        public ActionResult DeleteContractd(string id)
        {
            int i = 0;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@type", "delete");
                parms[1] = new SqlParameter("@ID", id);
                i = ys.Database.ExecuteSqlCommand("exec SP_ContractEdit @type,@ID", parms);
            }
            if (i > 0)
            {
                return Content("true");
            }
            return Content("false");
        }




        #endregion

        #region 删除货物详细信息

        public ActionResult DeleteContractdws(string id, string idws)
        {
            int i = 0;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int Cid = int.Parse(id);
                int wid = int.Parse(idws);
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@type", "delete");
                parms[1] = new SqlParameter("@ContractID", wid);
                parms[2] = new SqlParameter("@ProductDetailID", Cid);
                i = ys.Database.ExecuteSqlCommand("exec SP_Contract_ProductDetails  @type,@ContractID,@ProductDetailID", parms);
            }
            if (i > 0)
            {
                return Content("true");
            }
            return Content("false");
        }




        #endregion

        #region 添加合同
        public ActionResult CwAdd()
        {
            int i = 0;
            try
            {
                string bg = "false";
                string cuname = Request["CuName"].ToString();
                string cunumber = Request["CuNumber"].ToString();
                string Money = Request["Money"].ToString();
                decimal mon = Convert.ToDecimal(Money);
                string IfInstall = Request["IfInstall"].ToString();
                string IfTransportd = Request["IfTransport"].ToString();
                string IfIncludeTaxd = Request["IfIncludeTax"].ToString();
                string DateOfSign = Request["DateOfSign"].ToString();
                string Pay = Request["Pay"].ToString();
                string SendDate = Request["SendDate"].ToString();
                string weiyuetiaojian = Request["weiyuetiaojian"].ToString();
                string CZongJie = Request["CZongJie"].ToString();
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[20];
                    parms[0] = new SqlParameter("@type", "add");
                    parms[1] = new SqlParameter("@ID", 1);
                    parms[2] = new SqlParameter("@CustomerName", cuname);
                    parms[3] = new SqlParameter("@ContractNumber", cunumber);
                    parms[4] = new SqlParameter("@DateOfSign", DateOfSign);
                    parms[5] = new SqlParameter("@Money", mon);
                    parms[6] = new SqlParameter("@PaymentMethod", Pay);
                    parms[7] = new SqlParameter("@IfInstall", IfInstall);
                    parms[8] = new SqlParameter("@IfTransport", IfTransportd);
                    parms[9] = new SqlParameter("@IfIncludeTax", IfIncludeTaxd);
                    parms[10] = new SqlParameter("@DeliveryTime", SendDate);
                    parms[11] = new SqlParameter("@ConditionsOfbreachOfContract", weiyuetiaojian);
                    parms[12] = new SqlParameter("@Summary", CZongJie);
                    parms[13] = new SqlParameter("@CreatedBy", "");
                    parms[14] = new SqlParameter("@CreatedTime", "");
                    parms[15] = new SqlParameter("@StatusID", "");
                    parms[16] = new SqlParameter("@CreatedTimeStart", "");
                    parms[17] = new SqlParameter("@CreatedTimeEnd", "");
                    parms[18] = new SqlParameter("@AmountCollected", 927335.85);
                    parms[19] = new SqlParameter("@ProductOrderStatus", "");
                    i = ys.Database.ExecuteSqlCommand("exec SP_ContractEdit @type,@ID,@CustomerName,@ContractNumber,@DateOfSign,@Money,@PaymentMethod,@IfInstall,@IfTransport,@IfIncludeTax,@DeliveryTime," +
                   "@ConditionsOfbreachOfContract,@Summary,@CreatedBy,@CreatedTime,@StatusID,@CreatedTimeStart,@CreatedTimeEnd,@AmountCollected,@ProductOrderStatus", parms);
                }
                if (i > 0)
                {
                    return Content("true");
                }
            }
            catch (Exception ex)
            {
                return Content("false");
            }

            return Content("false");
        }

        #endregion

        #region 添加货物明细

        public ActionResult HDAdd()
        {
            int i = 0;
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string ContractID = Session["Cid"].ToString();
                    string ProductName = Request["ProductName"];
                    string ifDrive = Request["Select"];
                    string ProductSpec = Request["ProductSpec"];
                    string P_Speed = Request["P_Speed"];
                    string P_CarryingCapacity = Request["P_CarryingCapacity"];
                    string P_MainBeaMaterial = Request["P_MainBeaMaterial"];
                    string P_ElectricalRequirements = Request["P_ElectricalRequirements"];
                    string P_EquipmentWorkplace = Request["P_EquipmentWorkplace"];
                    string P_Main_materialofworkpiece = Request["P_Main_materialofworkpiece"];
                    string P_ChildPartSpecRange = Request["P_ChildPartSpecRange"];
                    string P_ChildPartWeight = Request["P_ChildPartWeight"];
                    string P_WorkpieceFeedingMode = Request["P_WorkpieceFeedingMode"];
                    string P_RollerDiameter = Request["P_RollerDiameter"];
                    string P_RollerMaterial = Request["P_RollerMaterial"];
                    string P_RollerSurface = Request["P_RollerSurface"];
                    string P_RollerTransferMode = Request["P_RollerTransferMode"];
                    string Units = Request["Units"];
                    string Count = Request["Count"];
                    string DueDay = Request["DueDay"];
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[24];
                    parms[0] = new SqlParameter("@ContractID", ContractID);
                    parms[1] = new SqlParameter("@ProductName", ProductName);
                    parms[2] = new SqlParameter("@Customer_ProductName", ProductName);
                    parms[3] = new SqlParameter("@ProductTypeID", "");
                    parms[4] = new SqlParameter("@ifDrive", ifDrive);
                    parms[5] = new SqlParameter("@ProductSpec", ProductSpec);
                    parms[6] = new SqlParameter("@P_Speed", P_Speed);
                    parms[7] = new SqlParameter("@P_CarryingCapacity", P_CarryingCapacity);
                    parms[8] = new SqlParameter("@P_ElectricalRequirements", P_ElectricalRequirements);
                    parms[9] = new SqlParameter("@P_EquipmentWorkplace", P_EquipmentWorkplace);
                    parms[10] = new SqlParameter("@P_Main_materialofworkpiece", P_Main_materialofworkpiece);
                    parms[11] = new SqlParameter("@P_MainBeaMaterial", P_MainBeaMaterial);
                    parms[12] = new SqlParameter("@P_ChildPartSpecRange", P_ChildPartSpecRange);
                    parms[13] = new SqlParameter("@P_ChildPartWeight", P_ChildPartWeight);
                    parms[14] = new SqlParameter("@P_WorkpieceFeedingMode", P_WorkpieceFeedingMode);
                    parms[15] = new SqlParameter("@P_RollerDiameter", P_RollerDiameter);
                    parms[16] = new SqlParameter("@P_RollerMaterial", P_RollerMaterial);
                    parms[17] = new SqlParameter("@P_RollerSurface", P_RollerSurface);
                    parms[18] = new SqlParameter("@P_RollerTransferMode", P_RollerTransferMode);
                    parms[19] = new SqlParameter("@Units", Units);
                    parms[20] = new SqlParameter("@Count", Count);
                    parms[21] = new SqlParameter("@DueDay", DueDay);
                    parms[22] = new SqlParameter("@Acceptance", "");
                    parms[23] = new SqlParameter("@CreatedBy", name);
                    i = ys.Database.ExecuteSqlCommand("exec  SP_Contract_ProductDetail_add  @ContractID,@ProductName,@Customer_ProductName," +
                     "@ProductTypeID,@ifDrive,@ProductSpec,@P_Speed,@P_CarryingCapacity,@P_ElectricalRequirements,@P_EquipmentWorkplace,@P_Main_materialofworkpiece,@P_MainBeaMaterial," +
                      "@P_ChildPartSpecRange,@P_ChildPartWeight,@P_WorkpieceFeedingMode,@P_RollerDiameter,@P_RollerMaterial,@P_RollerSurface,@P_RollerTransferMode,@Units,@Count,@DueDay,@Acceptance,@CreatedBy", parms);
                    if (i > 0)
                    {
                        return Content("true");
                    }
                }

            }
            catch (Exception ex)
            {
                return Content("false");
            }


            return Content("false");
        }


        #endregion

        #region 修改货物明细

        public ActionResult HDEdit(string ProductName, string ifDrive, string ProductSpec, string P_Speed, string P_CarryingCapacity, string P_ElectricalRequirements, string P_EquipmentWorkplace, string P_Main_materialofworkpiece, string P_MainBeaMaterial,
            string P_ChildPartSpecRange, string P_ChildPartWeight, string P_WorkpieceFeedingMode, string P_RollerDiameter, string P_RollerMaterial, string P_RollerSurface, string P_RollerTransferMode, string Units, string Count,string DueDay)
        {
                int i = 0;
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string id = Session["Eid"].ToString();
                    string idw = Session["Eidwd"].ToString();                   
                    SqlParameter[] parms = new SqlParameter[23];
                    parms[0] = new SqlParameter("@ProductDetailID", id);
                    parms[1] = new SqlParameter("@ContractID", idw);
                    parms[2] = new SqlParameter("@ProductName", ProductName);
                    parms[3] = new SqlParameter("@Customer_ProductName", ProductName);
                    parms[4] = new SqlParameter("@ProductTypeID", "");
                    parms[5] = new SqlParameter("@ifDrive", ifDrive);
                    parms[6] = new SqlParameter("@ProductSpec", ProductSpec);
                    parms[7] = new SqlParameter("@P_Speed", P_Speed);
                    parms[8] = new SqlParameter("@P_CarryingCapacity", P_CarryingCapacity);
                    parms[9] = new SqlParameter("@P_ElectricalRequirements", P_ElectricalRequirements);
                    parms[10] = new SqlParameter("@P_EquipmentWorkplace", P_EquipmentWorkplace);
                    parms[11] = new SqlParameter("@P_Main_materialofworkpiece", P_Main_materialofworkpiece);
                    parms[12] = new SqlParameter("@P_MainBeaMaterial", P_MainBeaMaterial);
                    parms[13] = new SqlParameter("@P_ChildPartSpecRange", P_ChildPartSpecRange);
                    parms[14] = new SqlParameter("@P_ChildPartWeight", P_ChildPartWeight);
                    parms[15] = new SqlParameter("@P_WorkpieceFeedingMode", P_WorkpieceFeedingMode);
                    parms[16] = new SqlParameter("@P_RollerDiameter", P_RollerDiameter);
                    parms[17] = new SqlParameter("@P_RollerMaterial", P_RollerMaterial);
                    parms[18] = new SqlParameter("@P_RollerSurface", P_RollerSurface);
                    parms[19] = new SqlParameter("@P_RollerTransferMode", P_RollerTransferMode);
                    parms[20] = new SqlParameter("@Units", Units);
                    parms[21] = new SqlParameter("@Count", Count);
                    parms[22] = new SqlParameter("@DueDay", DueDay);
                    i = ys.Database.ExecuteSqlCommand("exec  SP_Contract_ProductDetail_Update @ProductDetailID,@ContractID,@ProductName,@Customer_ProductName," +
                     "@ProductTypeID,@ifDrive,@ProductSpec,@P_Speed,@P_CarryingCapacity,@P_ElectricalRequirements,@P_EquipmentWorkplace,@P_Main_materialofworkpiece,@P_MainBeaMaterial," +
                      "@P_ChildPartSpecRange,@P_ChildPartWeight,@P_WorkpieceFeedingMode,@P_RollerDiameter,@P_RollerMaterial,@P_RollerSurface,@P_RollerTransferMode,@Units,@Count,@DueDay", parms);
                    if (i > 0)
                    {
                        return Content("true");
                    }
                }
            return Content("false");
        }


        #endregion

        #region 显示通知
        public JsonResult NoticeCheck()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string Did = TempData["notice"].ToString();
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ContractID", Did);
                var list = ys.Database.SqlQuery<SP_Contract_ProductDetailNotice_Check_Result>("exec SP_Contract_ProductDetailNotice_Check  @ContractID", parms).ToList();

                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 编辑发货数量

        public ActionResult SendNotice(string id, string qty, string ContractID, string CNumber, string YNumber)
        {
            int i = 0;
            int cn = int.Parse(CNumber);
            int yn = int.Parse(YNumber);
            int qt = int.Parse(qty);
            if (yn + qt > cn)
            {
                return Content("da");
            }
            using (YLMES_newEntities ysd = new YLMES_newEntities())
            {
                int ids = int.Parse(ContractID);
                var ifNotice = ysd.PM_ShipNotice.Where(w => w.StatusID.Equals("已确认发货") && w.ProjectID.Equals(ids)).FirstOrDefault();
                if (ifNotice != null)
                {
                    return Content("yiqueren");
                }
            }         
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    if (qty == null)
                    {
                        qty = "";
                    }
                    SqlParameter[] parms = new SqlParameter[3];
                    parms[0] = new SqlParameter("@ContractID", ContractID);
                    parms[1] = new SqlParameter("@ProductDetailID", id);
                    parms[2] = new SqlParameter("@QTY", qty);
                    i = ys.Database.ExecuteSqlCommand("exec SP_Contract_ProductDetailNotice_SendNotice  @ContractID,@ProductDetailID,@QTY", parms);
                }          
            if (i > 0)
            {
                return Content("true");
            }
            return Content("false");
        }
        #endregion

        #region 是否发送货物通知

        public ActionResult ifSendNotice(string id, string ContractID)
        {
            string[] strid = id.Split(',');
            int i = 0;
            foreach (var list in strid)
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[2];
                    parms[0] = new SqlParameter("@ContractID", ContractID);
                    parms[1] = new SqlParameter("@ProductDetailID", list);
                    i = ys.Database.ExecuteSqlCommand("exec SP_Contract_ProductDetailNotice_SendNoticeStatus  @ContractID,@ProductDetailID", parms);
                }
            }
            if (i > 0)
            {
                return Content("true");

            }
            return Content("false");
        }



        #endregion

        #region 删除货物通知记录

        public ActionResult DeleteNotice(string id)
        {
            int i = 0;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ContractID", id);
                i = ys.Database.ExecuteSqlCommand("exec SP_Contract_ProductDetailNotice_Delete  @ContractID", parms);
                if (i > 0)
                {
                    return Content("true");
                }
            }

            return Content("false");
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

        #region 货物明细显示

        public void DetialsXianShi(string id, string idws)
        {

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                Session["Eid"] = id;
                Session["Eidwd"] = idws;
                int Cid = int.Parse(id);
                int wid = int.Parse(idws);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ContractID", wid);
                parms[1] = new SqlParameter("@ProductDetailID", Cid);
                var list = ys.Database.SqlQuery<SP_Contract_ProductDetail_Check_Result>("exec SP_Contract_ProductDetail_Check  @ContractID,@ProductDetailID", parms).FirstOrDefault();
                ViewData["ProductSpec"] = list.产品规格;
                ViewData["P_Speed"] = list.速度;
                ViewData["P_ElectricalRequirements"] = list.电气要求;
                ViewData["p_main_materialofworkpiece"] = list.工件主要材质;
                ViewData["P_ChildPartWeight"] = list.工件重量;
                ViewData["P_RollerDiameter"] = list.滚筒直径;
                ViewData["P_RollerSurface"] = list.滚筒表面处理形式;
                ViewData["Count"] = list.合同数量;
                string i = "";
                if (list.是否动力 == "")
                {
                    i = "0";
                }
                else if (list.是否动力 == "动力")
                {
                    i = "1";
                }
                else
                {
                    i = "2";
                }
                ViewData["select"] = i;
                ViewData["ProductName"] = list.产品名称;
                ViewData["P_MainBeaMaterial"] = list.主梁材质;
                ViewData["P_CarryingCapacity"] = list.载重;
                ViewData["P_EquipmentWorkplace"] = list.设备工作场所;
                ViewData["P_ChildPartSpecRange"] = list.工件最大最小外形尺寸;
                ViewData["P_WorkpieceFeedingMode"] = list.工件来料输送方式;
                ViewData["P_RollerMaterial"] = list.滚筒材质;
                ViewData["P_RollerTransferMode"] = list.滚筒输送形式;
                ViewData["Units"] = list.单位;
                ViewData["P_ChildPartWeight"] = list.工件重量;
                ViewData["DueDay"] = list.截止日期;
            }
        }

        #endregion

        #region 查看合同细节

        public JsonResult checkDetail(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ID", id);
                var list = ys.Database.SqlQuery<SP_ContractEdit_checkContractDetail_Result>("exec SP_ContractEdit_checkContractDetail @ID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region  显示收款情况
        public JsonResult Get_Datad(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "check");
                parms[1] = new SqlParameter("@ContractID", i);
                parms[2] = new SqlParameter("@CreatedBy", "");
                parms[3] = new SqlParameter("@newAmountCollected", "");
                parms[4] = new SqlParameter("@DateOfReceipt", "");
                parms[5] = new SqlParameter("@TicketOpeningAndDate", "");
                parms[6] = new SqlParameter("@ReceivablesID", 1);
                var list = ys.Database.SqlQuery<SP_Contract_Receivables_Result>("exec SP_Contract_Receivables  @type,@ContractID,@CreatedBy,@newAmountCollected,@DateOfReceipt,@TicketOpeningAndDate,@ReceivablesID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  显示收款历史记录
        public JsonResult Get_ReceivablesHistory(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ContractID", i);
                var list = ys.Database.SqlQuery<ReceivablesHistorycheck_Result>("exec ReceivablesHistorycheck  @ContractID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 显示新增收款记录

        public ActionResult ShouKuangAdd(string CNumbers)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var id = ys.C_Contract.Where(cs => cs.ContractNumber.Equals(CNumbers)).FirstOrDefault();
                ViewData["HeBianHao"] = id.ID;
                ViewData["jine"] = id.Money;
                ViewData["createBy"] = id.CreatedBy;
            }
            return View();
        }

        #endregion


        #region 新增第一个收款记录

        public ActionResult ShouXinAdd(string hebianhao, string newAmountCollected, string DateOfReceipt, string Ticket)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(hebianhao);
                 var value = ys.C_Contract_Receivables.Where(p=>p.ContractID==i).FirstOrDefault();
                 string vl = Convert.ToString(value);
                 if (!string.IsNullOrEmpty(vl))
                 {
                     return Content("you");
                 }
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "Add");
                parms[1] = new SqlParameter("@ContractID", i);
                parms[2] = new SqlParameter("@CreatedBy", name);
                parms[3] = new SqlParameter("@newAmountCollected", newAmountCollected);
                parms[4] = new SqlParameter("@DateOfReceipt", DateOfReceipt);
                parms[5] = new SqlParameter("@TicketOpeningAndDate", Ticket);
                parms[6] = new SqlParameter("@ReceivablesID", 1);
                int w = ys.Database.ExecuteSqlCommand("exec SP_Contract_Receivables  @type,@ContractID,@CreatedBy,@newAmountCollected,@DateOfReceipt,@TicketOpeningAndDate,@ReceivablesID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }


        #endregion

        #region  新增收款记录

        public ActionResult AddShouJiLu(string id, string NewAmountCollected, string DateOfReceipt, string TicketOpeningAndDate)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "update");
                parms[1] = new SqlParameter("@ContractID", i);
                parms[2] = new SqlParameter("@CreatedBy", name);
                parms[3] = new SqlParameter("@newAmountCollected", NewAmountCollected);
                parms[4] = new SqlParameter("@DateOfReceipt", DateOfReceipt);
                parms[5] = new SqlParameter("@TicketOpeningAndDate", TicketOpeningAndDate);
                parms[6] = new SqlParameter("@ReceivablesID", 1);
                int w = ys.Database.ExecuteSqlCommand("exec SP_Contract_Receivables  @type,@ContractID,@CreatedBy,@newAmountCollected,@DateOfReceipt,@TicketOpeningAndDate,@ReceivablesID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }

        #endregion


        #region 修改收款记录

        public ActionResult XiuShouJilu()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string id = Request["cid"].ToString();
                int e = int.Parse(id);
                string cid = Request["ContractID"].ToString();
                int Cid = int.Parse(cid);
                string NewA = Request["NewA"].ToString();
                string Dtr = Request["Dtr"].ToString();
                string Ticket = Request["Ticket"].ToString();
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "ReceivablesHistoryUpdate");
                parms[1] = new SqlParameter("@ContractID", Cid);
                parms[2] = new SqlParameter("@CreatedBy", name);
                parms[3] = new SqlParameter("@newAmountCollected", NewA);
                parms[4] = new SqlParameter("@DateOfReceipt",Dtr);
                parms[5] = new SqlParameter("@TicketOpeningAndDate",Ticket);
                parms[6] = new SqlParameter("@ReceivablesID",e);
                int w = ys.Database.ExecuteSqlCommand("exec SP_Contract_Receivables  @type,@ContractID,@CreatedBy,@newAmountCollected,@DateOfReceipt,@TicketOpeningAndDate,@ReceivablesID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }

        #endregion

        #region 删除收款记录

        public ActionResult DeleteShouJiLu(string id, string ContractID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                int cid = int.Parse(ContractID);
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@type", "ReceivablesHistorydelete");
                parms[1] = new SqlParameter("@ContractID", cid);
                parms[2] = new SqlParameter("@CreatedBy", "");
                parms[3] = new SqlParameter("@newAmountCollected", "");
                parms[4] = new SqlParameter("@DateOfReceipt", "");
                parms[5] = new SqlParameter("@TicketOpeningAndDate", "");
                parms[6] = new SqlParameter("@ReceivablesID", i);
                int w = ys.Database.ExecuteSqlCommand("exec SP_Contract_Receivables  @type,@ContractID,@CreatedBy,@newAmountCollected,@DateOfReceipt,@TicketOpeningAndDate,@ReceivablesID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }

        #endregion

        #region 合同打印

        public ActionResult ContractPrint(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                ViewData["ContractDetialsCheckId"] = id;
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@type", "checkByContractID");
                parms[1] = new SqlParameter("@ID", id);

                var list = ys.Database.SqlQuery<SP_ContractEdit_Result>("exec SP_ContractEdit @type,@ID", parms).FirstOrDefault();

                ViewData["CuName"] = list.CustomerName;
                ViewData["CuNumber"] = list.ContractNumber;
                ViewData["Money"] = list.合同金额;
                ViewData["id"] = list.id;
                ViewData["AmountCollected"] = list.收款金额;
                ViewData["DateOfSign"] = list.合同签订日期;
                if (list.是否安装.Equals("是"))
                {
                    ViewData["IfInstall"] = 1;
                }
                else
                {
                    ViewData["IfInstall"] = 0;
                }
                if (list.是否含税.Equals("是"))
                {
                    ViewData["IfIncludeTax"] = 1;
                }
                else
                {
                    ViewData["IfIncludeTax"] = 0;
                }
                if (list.是否运输.Equals("是"))
                {
                    ViewData["IfTransport"] = 1;
                }
                else
                {
                    ViewData["IfTransport"] = 0;
                }

                ViewData["Pay"] = list.收款方式;
                ViewData["SendDate"] = list.交期;
                ViewData["weiyuetiaojian"] = list.违约条件;
                ViewData["CZongJie"] = list.合同总结;


            }
            return View();
        }

        public JsonResult checkDetail(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@ID", id);
                var list = ys.Database.SqlQuery<SP_ContractEdit_checkContractDetail_Result>("exec SP_ContractEdit_checkContractDetail @ID", parms).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }


}