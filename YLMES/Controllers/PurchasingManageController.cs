
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YLMES.Models;


namespace YLMES.Controllers
{
    public class PurchasingManageController : Controller
    {
        // GET: PurchasingManage
        public ActionResult Index()
        {
            return View();
        }
        #region 采购
        //采购合同页面
        //设备
        public ActionResult P_Contract(List<string> value, string key, string cuid)
        {
            ViewData["value"] = value;
            ViewData["key"] = key;
            ViewData["cuid"] = cuid;
            return View();
        }
        //外协
        public ActionResult O_Contract(List<string> value, string key, string cuid)
        {
            ViewData["value"] = value;
            ViewData["key"] = key;
            ViewData["cuid"] = cuid;
            return View();
        }
        //钢材
        public ActionResult S_Contract(List<string> value, string key, string cuid)
        {
            ViewData["value"] = value;
            ViewData["key"] = key;
            ViewData["cuid"] = cuid;
            return View();
        }
        //常规
        public ActionResult E_Contract(List<string> value, string key, string cuid)
        {
            ViewData["value"] = value;
            ViewData["key"] = key;
            ViewData["cuid"] = cuid;
            return View();
        }
        //零星
        public ActionResult F_Contract(List<string> value, string key, string cuid)
        {
            ViewData["value"] = value;
            ViewData["key"] = key;
            ViewData["cuid"] = cuid;
            return View();
        }
        //确认合同状态
        public string OkStut(string ids, string Money, string Price, string Appid,string Sum,int indexStut) {
            StringBuilder PurchaseKey = new StringBuilder();
            foreach (var key in Session["PurchaseKey"] as List<string>)
            {
                PurchaseKey.Append(key);
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[9];
                parms[0] = new SqlParameter("@mid", ids);
                parms[1] = new SqlParameter("@type", "stut");
                parms[2] = new SqlParameter("@money", Money);
                parms[3] = new SqlParameter("@createBy", Session["name"]);
                parms[4] = new SqlParameter("@price", Price);
                parms[5] = new SqlParameter("@appid", Appid);
                parms[6] = new SqlParameter("@Sum", Sum);
                parms[7] = new SqlParameter("@indexStut", indexStut);
                parms[8] = new SqlParameter("@taid", PurchaseKey.ToString());
                ys.Database.ExecuteSqlCommand("exec UpMaterialDefaultStu @mid=@mid,@type=@type,@money=@money,@createBy=@createBy,@price=@price,@appid=@appid,@Sum=@Sum,@indexStut=@indexStut,@taid=@taid", parms);
            }
            return "true";
        }
        //合同管理
        public ActionResult PurchaseCou() {

            return View();
        }
        //撤销合同
        public string UpPONO(List<string> mid) {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var i = 1;
                StringBuilder PurchaseValue = new StringBuilder();
                foreach (var key in mid)
                {
                    if (i == mid.Count) {
                        PurchaseValue.Append(key);
                    }
                    else { 
                    PurchaseValue.Append(key+",");
                        i++;
                            }
                }
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@mid", PurchaseValue.ToString());
                 ys.Database.ExecuteSqlCommand("exec PurchaseCo @mid=@mid", parameter);
            }
                return "true";
        }
        //查询采购合同
        public ActionResult chaeckCo(string appid,string cid) {
            if (appid==null) {
                appid = "";
            }
            if (cid == null)
            {
                cid = "";
            }
            List<PurchaseCo_Result> list;
            List<PurchaseAll_Result> purchaseAlllist = new List<PurchaseAll_Result>();
            Dictionary<string, object> hasmap = new Dictionary<string, Object>();
            using (YLMES_newEntities ys =new YLMES_newEntities()) {
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter("@ApplierId", appid);
                parameter[1] = new SqlParameter("@ContractTypeId", cid);
                list = ys.Database.SqlQuery<PurchaseCo_Result>("exec PurchaseCo @ContractTypeId,@ApplierId", parameter).ToList();
                foreach (var i in list)
                {
                    PurchaseAll_Result purchaseAll = new PurchaseAll_Result();
                    var listi = ys.PM_Material.Where(p => p.ID == i.MaterialID).ToList();
                    var listj = ys.PM_PurchaseMaterialList.Where(p => p.MaterialID == i.MaterialID).ToList();
                    purchaseAll.PartNumber = listi[0].PartNumber;
                    purchaseAll.PartSpec = listi[0].PartSpec;
                    purchaseAll.MaterialID = i.MaterialID;
                    purchaseAll.ActPurchaseQTY = i.数量;
                    purchaseAll.ApplierId = listi[0].ApplierId;
                    purchaseAll.ContractTypeId = listi[0].ContractTypeId;
                    purchaseAll.UnitPrice = listj[0].UnitPrice;
                    purchaseAll.TotalPrice = (i.数量*listj[0].UnitPrice);
                    purchaseAlllist.Add(purchaseAll);
                }
            }
            hasmap.Add("code", 0);
            hasmap.Add("msg", "");
            hasmap.Add("data", purchaseAlllist);
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //更改默认状态
        public void UpDefaultStu(List<string> mid,string appid,string cuid) {
            using (YLMES_newEntities ys =new YLMES_newEntities()) {
                StringBuilder PurchaseValue = new StringBuilder();
                foreach (var key in mid)
                {
                    PurchaseValue.Append(key);
                }
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@mid", PurchaseValue.ToString());
                parms[1] = new SqlParameter("@appid", appid);
                parms[2] = new SqlParameter("@cuid", cuid);
                ys.Database.ExecuteSqlCommand("exec UpMaterialDefaultStu @mid,@appid,@cuid", parms);
            }
        }
        //查询供应商最新Id
        public int checkApplierId()
        {
            List<PM_ApplierList> list = new List<PM_ApplierList>();
            int id = 0;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                list = ys.PM_ApplierList.ToList();
                id = list.Max(p => p.ID);
            }
            return id;
        }
        //根据id查询供应商信息
        public ActionResult ByIdfindApplier(int id)
        {
            List<PM_ApplierList> list = new List<PM_ApplierList>();

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                list = ys.PM_ApplierList.Where(p => p.ID == id).ToList();

            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //根据名称查询供应商信息
        public ActionResult ByNamefindApplier(string ApplierName)
        {
            List<PM_ApplierList> list = new List<PM_ApplierList>();

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                list = ys.PM_ApplierList.Where(p => p.Name == ApplierName).ToList();

            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //汇总页面
        public ActionResult SummaryMat(List<string> PurchaseKey, List<string> PurchaseValue)
        {
            Session["PurchaseKey"] = PurchaseKey;
            Session["PurchaseValue"] = PurchaseValue;
            return View();
        }
        //汇总信息
        public ActionResult checkSummary(List<string> ids)
        {

            StringBuilder PurchaseKey = new StringBuilder();
            StringBuilder PurchaseValue = new StringBuilder();
            foreach (var key in Session["PurchaseKey"] as List<string>)
            {
                PurchaseKey.Append(key);
            }
            if (ids != null)
            {
                foreach (var Value in ids)
                {
                    PurchaseValue.Append(Value);
                }
            }
            else
            {
                foreach (var Value in Session["PurchaseValue"] as List<string>)
                {
                    PurchaseValue.Append(Value);
                }
            }
            List<Summary_Result> list;
            List<PurchaseAll_Result> purchaseAlllist = new List<PurchaseAll_Result>();
            Dictionary<string, object> hasmap = new Dictionary<string, Object>();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@SummaryTid", PurchaseKey.ToString());
                parms[1] = new SqlParameter("@SummaryMid", PurchaseValue.ToString());
                list = ys.Database.SqlQuery<Summary_Result>("exec Summary @SummaryTid,@SummaryMid", parms).ToList();
                foreach (var i in list)
                {
                    PurchaseAll_Result purchaseAll = new PurchaseAll_Result();
                    var listi = ys.PM_Material.Where(p => p.ID == i.MaterialID).ToList();
                    var listj = ys.PM_PurchaseMaterialList.Where(p => p.MaterialID == i.MaterialID).ToList();
                    purchaseAll.PartNumber = listi[0].PartNumber;
                    purchaseAll.PartSpec = listi[0].PartSpec;
                    purchaseAll.MaterialID = i.MaterialID;
                    purchaseAll.ActPurchaseQTY = i.申请数量;
                    purchaseAll.ApplierId = listi[0].ApplierId;
                    purchaseAll.ContractTypeId = listi[0].ContractTypeId;
                    purchaseAll.UnitPrice = listj[0].UnitPrice;
                    purchaseAll.TotalPrice = (i.申请数量 * listj[0].UnitPrice);
                    purchaseAlllist.Add(purchaseAll);

                }
            }
            hasmap.Add("code", 0);
            hasmap.Add("msg", "");
            hasmap.Add("data", purchaseAlllist);
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //显示采购信息
        //public ActionResult CheckSupplierInformation()
        //{

        //}
        #endregion
    }
}