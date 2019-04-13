using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class ApplierListController : Controller
    {
        // GET: ApplierList
        public ActionResult Supplier()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                //var list = ys.PM_ApplierProductType.Where(p => p.Type.Contains("货品品类")).ToList();
                var Level = ys.PM_ApplierProductType.Where(p => p.Type.Contains("级别")).ToList();
                var Advantage = ys.PM_ApplierProductType.Where(p => p.Type.Contains("优势分析")).ToList();
                //ViewBag.Category = list;
                ViewBag.Level = Level;
                ViewBag.Advantage = Advantage;
                return View();
            }
        }//查询已有供应商
        public ActionResult checkApplierName() {
            List<PM_ApplierList> list;
            using (YLMES_newEntities ys =new YLMES_newEntities ()) {

                list = ys.PM_ApplierList.ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //查询合同类型
        public ActionResult checkContrType()
        {
            List<ContractType> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
              list = ys.ContractType.ToList();
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        //物料类型查询
        public ActionResult checkPurchase()
        {
            List<MaterTypeNames> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                 list = ys.MaterTypeNames.ToList();

            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //查询物料信息
        public ActionResult checkMaterial(string name,string ContractTypeId,int page ,int limit) {
            Dictionary<string, object> hasmap;
            if (name == null) {
                name = "";
            }
            if (ContractTypeId==null) {
                ContractTypeId = "";
            }
            using (YLMES_newEntities ys=new YLMES_newEntities ()) {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@name",name);
                parms[1] = new SqlParameter("@ContractTypeId", ContractTypeId);
                var list = ys.Database.SqlQuery<checkCoMaterial_Result>("exec checkCoMaterial @name,@ContractTypeId", parms).ToList();
                hasmap = new Dictionary<string, Object>();
            PageList<checkCoMaterial_Result> pageList = new PageList<checkCoMaterial_Result>(list, page, limit);
            int count = list.Count();
            hasmap.Add("code", 0);
            hasmap.Add("msg", "");
            hasmap.Add("count", count);
            hasmap.Add("data", pageList);
            return Json(hasmap,JsonRequestBehavior.AllowGet);
            }
        }
        //修改物料信息
        public ActionResult EditMaterialInformation(string id,string apid,string Applierid, string typeid)
        {
            try
            {
                int ids = int.Parse(id);
                int apsid = int.Parse(apid);
                int appid = int.Parse(Applierid);
                int tid = int.Parse(typeid);
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    PM_Material pm = ys.PM_Material.Where(c => c.ID == ids).First();
                    pm.ApplierProductTypeID = apsid;
                    pm.ApplierId = Applierid;
                    pm.ContractTypeId = typeid;
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
        #region 查询
        public JsonResult GetSupplierlist(string Name, string Status, int page, int limit)
        {
            if (Status == "全部")
            {
                Status = "";
            }
            if (Name == null)
            {
                Name = "";
            }
            if (Status == null)
            {
                Status = "";
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[18];
                parms[0] = new SqlParameter("@Type", "sel");
                parms[1] = new SqlParameter("@ApplierID", 666);
                parms[2] = new SqlParameter("@ApplierName", Name);
                parms[3] = new SqlParameter("@Address", "");
                parms[4] = new SqlParameter("@Contact", "");
                parms[5] = new SqlParameter("@Tel", "");
                parms[6] = new SqlParameter("@Mobile", "");
                parms[7] = new SqlParameter("@Category", "");
                parms[8] = new SqlParameter("@Level", "");
                parms[9] = new SqlParameter("@Advantage", "");
                parms[10] = new SqlParameter("@Note", "");
                parms[11] = new SqlParameter("@Fax", "");
                parms[12] = new SqlParameter("@CreatedBy", "");
                parms[13] = new SqlParameter("@Principal", "");
                parms[14] = new SqlParameter("@Representative", "");
                parms[15] = new SqlParameter("@Account", "");
                parms[16] = new SqlParameter("@Bank", "");
                parms[17] = new SqlParameter("@Status", Status);
                var list = ys.Database.SqlQuery<ApplierList_Supplier_Result>("exec  ApplierList_Supplier  @Type,@ApplierID,@ApplierName,@Address,@Contact,@Tel,@Mobile,@Category,@Level,@Advantage,@Note,@Fax,@CreatedBy,@Principal,@Representative,@Account,@Bank,@Status", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<ApplierList_Supplier_Result> pageList = new PageList<ApplierList_Supplier_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }        
        }
        #endregion
        #region 更新
        public string ApplierListUpdata(string Principal, string Representative, string Account, string Bank, string ApplierID, string ApplierName, string Address, string Contact, string Tel, string Mobile, string Category, string Level, string Advantage)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[17];
                parms[0] = new SqlParameter("@Type", "Updata");
                parms[1] = new SqlParameter("@ApplierID", ApplierID);
                parms[2] = new SqlParameter("@ApplierName", ApplierName);
                parms[3] = new SqlParameter("@Address", Address);
                parms[4] = new SqlParameter("@Contact", Contact);
                parms[5] = new SqlParameter("@Tel", Tel);
                parms[6] = new SqlParameter("@Mobile", Mobile);
                parms[7] = new SqlParameter("@Category", Category);
                parms[8] = new SqlParameter("@Level", Level);
                parms[9] = new SqlParameter("@Advantage", Advantage);
                parms[10] = new SqlParameter("@Note", "");
                parms[11] = new SqlParameter("@Fax", "");
                parms[12] = new SqlParameter("@CreatedBy", "");
                parms[13] = new SqlParameter("@Principal", "");
                parms[14] = new SqlParameter("@Representative", "");
                parms[15] = new SqlParameter("@Account", "");
                parms[16] = new SqlParameter("@Bank", "");
                int i = ys.Database.ExecuteSqlCommand("exec ApplierList_Supplier @Type,@ApplierID,@ApplierName,@Address,@Contact,@Tel,@Mobile,@Category,@Level,@Advantage,'','',''," +
                                                      "@Principal,@Representative,@Account,@Bank", parms);
                return "true";
            }
        }
        //修改地址
        public ActionResult EditDress(string ApplierID,string Dress)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int aid = int.Parse(ApplierID);
                    PM_ApplierList pa = ys.PM_ApplierList.Where(p => p.ID == aid).First();
                    pa.Address = Dress;
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
        #endregion
        #region 新增供应商信息
        public ActionResult ApplierAdd()
        {           
            return View();
        }
        public string ApplierListAdd(List<string> ApplierName, List<string> ApplierNameValue)
        {
            StringBuilder sb = new StringBuilder();
            string Type = "Add";
            SqlParameter[] prams = new SqlParameter[ApplierName.Count];
            Dictionary<string, string> Applier = new Dictionary<string, string>();

            for (int index = 0; index < ApplierName.Count; index++)
            {

                Applier.Add(ApplierName[index].ToString(), ApplierNameValue[index].ToString());

            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int j = 0;
                sb.Append("exec ApplierList_Supplier @CreatedBy='11', @type='" + Type + "'," );
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

        public JsonResult getCategory()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_ApplierProductType.Where(p => p.Type.Contains("货品品类")).ToList();
                var Level = ys.PM_ApplierProductType.Where(p => p.Type.Contains("级别")).ToList();
                var Advantage = ys.PM_ApplierProductType.Where(p => p.Type.Contains("优势分析")).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                hasmap.Add("Level", Level);
                hasmap.Add("Advantage", Advantage);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 删除
        public string ApplierListDel(string ApplierID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "del");
                parms[1] = new SqlParameter("@ApplierID", ApplierID);
                int i = ys.Database.ExecuteSqlCommand("exec [ApplierList_Supplier] @Type,@ApplierID", parms);
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
        #region 审核通过
        public string ApplierListReview(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "ReviewUp");
                parms[1] = new SqlParameter("@ApplierID", id);
                int i = ys.Database.ExecuteSqlCommand("exec [ApplierList_Supplier] @Type,@ApplierID", parms);
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
        #region 显示采购清单
        //查询采购清单
        public ActionResult checkPurchaselist(string id) {
            List<PurchaseAll_Result> list = null;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (id == null)
                {
                    id = "";
                }               
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type","");
                parms[1] = new SqlParameter("@id", id);
                list = ys.Database.SqlQuery<PurchaseAll_Result>(" exec PurchaseAll @Type,@id", parms).ToList();
            }
            if (id != "")
            {
                Dictionary<string, object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Purchaselist(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                ViewData["taskid"] = id;
                var list = ys.PM_ApplierProductType.Where(p => p.Type.Contains("货品品类")).ToList();
                ViewBag.Category = list;

                var GyiS = ys.PM_ApplierList.ToList();
                ViewBag.gy = GyiS;

                //SqlParameter[] parms = new SqlParameter[2];
                //parms[0] = new SqlParameter("@Type", "SupvpliserName");
                //parms[1] = new SqlParameter("@SupplierName", ViewBag.Category.货品品类);
                //var lisst = ys.Database.SqlQuery<ApplierList_getName_Result>("exec ApplierList_getName @Type,@SupplierName", parms).ToList();
                //ViewBag.SupplierName = list;
            }
            return View();
        }
        //物料信息管理
        public ActionResult MaterialManagement()
        {
            return View();
        }
        public JsonResult GetPurchaselist(string PONO, string CreatedTime, string CreatedTimeEnd, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "selMaterialList");
                parms[1] = new SqlParameter("@PONO", PONO);
                parms[2] = new SqlParameter("@CreatedTime", CreatedTime);
                parms[3] = new SqlParameter("@CreatedTimeEnd", CreatedTimeEnd);
                var list = ys.Database.SqlQuery<Purchase_MaterialList_Result>(" exec Purchase_MaterialList @Type,@PONO,@CreatedTime,@CreatedTimeEnd", parms).ToList();
                PageList<Purchase_MaterialList_Result> pageList = new PageList<Purchase_MaterialList_Result>(list, page, limit);
                hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        public JsonResult selMaterialList(string PONO, string CreatedTime, string CreatedTimeEnd, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "sel");
                parms[1] = new SqlParameter("@PONO", PONO);
                parms[2] = new SqlParameter("@CreatedTime", CreatedTime);
                parms[3] = new SqlParameter("@CreatedTimeEnd", CreatedTimeEnd);
                var list = ys.Database.SqlQuery<Purchase_MaterialList_Result>(" exec Purchase_MaterialList @Type,@PONO,@CreatedTime,@CreatedTimeEnd", parms).ToList();
                PageList<Purchase_MaterialList_Result> pageList = new PageList<Purchase_MaterialList_Result>(list, page, limit);
                hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 修改单价和采购数量
        public string UnitPricetUpdata(string PurchaseID, string ApplyPurchaseQTY, string UnitPrice, string MaterialID, string ApplierProductTypeID)
        {
            string CreatedBy = Session["name"].ToString();
            if (UnitPrice == "")
            {
                UnitPrice = "0";
            }
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@Type", "UnitPricetUpdata");
                parms[1] = new SqlParameter("@PurchaseID", PurchaseID);
                parms[2] = new SqlParameter("@ApplyPurchaseQTY", Convert.ToInt32(ApplyPurchaseQTY));
                parms[3] = new SqlParameter("@UnitPrice", Convert.ToDouble(UnitPrice));
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                parms[5] = new SqlParameter("@ApplierProductType", ApplierProductTypeID);
                parms[6] = new SqlParameter("@MaterialID", MaterialID);

                int i = ys.Database.ExecuteSqlCommand(" exec Purchase_MaterialList @Type,'','','',@PurchaseID,@ApplyPurchaseQTY,@UnitPrice,@CreatedBy,@ApplierProductType,@MaterialID", parms);
                if (i > 0)
                {
                    return "true";
                }
                return "false";
            }
        }
        #endregion

        #region 绑定供应商名称
        public JsonResult Supp(string ids)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int id = int.Parse(ids);
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@CategoryID", id);
                var list = ys.Database.SqlQuery<ApplierList_getName_Result>("exec ApplierList_getName @CategoryID", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        //public 
        #endregion
        #region 物料绑定
        public ActionResult Material()
        {
            return View();
        }
        public JsonResult Materiallist(string PartNumber, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "sel");
                parms[1] = new SqlParameter("@PartNumber", PartNumber);
                var list = ys.Database.SqlQuery<Supplier_MaterialBinding_Result>("exec Supplier_MaterialBinding @Type,@PartNumber", parms).ToList();
                PageList<Supplier_MaterialBinding_Result> pageList = new PageList<Supplier_MaterialBinding_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public string Materialbinding(string id, string ApplierID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "Materialbinding");
                parms[1] = new SqlParameter("@id", id);
                parms[2] = new SqlParameter("@ApplierProductType", ApplierID);
                int i = ys.Database.ExecuteSqlCommand("exec Supplier_MaterialBinding @Type,'',@id,@ApplierProductType", parms);
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


        #region 供应商物料信息

      public ActionResult AddMaterInfo(string SupplierId)
        {
            ViewData["SupplierId"] = SupplierId;
            return View();
        }
        //新增供应商物料
        public ActionResult AddMater(string  SupplierId,string id)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int sid = int.Parse(SupplierId);
                int i = int.Parse(id);
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[3];
                 parms[0] = new SqlParameter("@SupplierId", sid);
                 parms[1] = new SqlParameter("@id", i);
                 parms[2] = new SqlParameter("@name", name);
                 ys.Database.ExecuteSqlCommand("exec PM_SupplierMaterials @SupplierId,@id,@name",parms);
                return Content("true");
            }
        }
        public ActionResult CheckMaterTypeNames(int page,int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                var list = ys.MaterTypeNames.ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<MaterTypeNames> pageList = new PageList<MaterTypeNames>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckMaterInfo(string SupplierId)
        {
            ViewData["sid"] = SupplierId;
            return View();
        }
        public ActionResult CheckMaterInfos(string sid, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@SupplierId", sid);
                var list = ys.Database.SqlQuery<PM_CheckSupplierMat_Result>("exec PM_CheckSupplierMat @SupplierId", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<PM_CheckSupplierMat_Result> pageList = new PageList<PM_CheckSupplierMat_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //删除物料类型
        public ActionResult DeleteMaterInfo(string id)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                SupplierMaterials sm = ys.SupplierMaterials.Where(s => s.Id == i).FirstOrDefault();
                ys.SupplierMaterials.Remove(sm);
               int isd=ys.SaveChanges();
                if (isd > 0)
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

    }
}