using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class WarehouseController : Controller
    {
        // GET: Warehouse
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Stock()
        {
            return View();
        }
        #region 成品仓库存
        public JsonResult Getproduct(string CName, string storagetime, string storagetimeend, string outgoingtime, string outgoingtimeend)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@CustomerName", CName);
                parms[1] = new SqlParameter("@CreatedTime", storagetime);
                parms[2] = new SqlParameter("@CreatedTimeEnd", storagetimeend);
                parms[3] = new SqlParameter("@OutgoingTime", outgoingtime);
                parms[4] = new SqlParameter("@OutgoingTimeEnd", outgoingtimeend);
                var list = ys.Database.SqlQuery<Finished_stock_Result>("exec Finished_stock @CustomerName,@CreatedTime,@CreatedTimeEnd,@OutgoingTime,@OutgoingTimeEnd", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 仓库设置
        public ActionResult Warehousesetup()
        {
            return View();
        }
        public JsonResult Warehouselist(string WarehouseName, string CreatedTimestart, string CreatedTimeEnd, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (WarehouseName == null)
                {
                    WarehouseName = "";
                }
                if(CreatedTimestart==null)
                {
                    CreatedTimestart = "";
                }
                if (CreatedTimeEnd == null)
                {
                    CreatedTimeEnd = "";
                }
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "Warehouselist");
                parms[1] = new SqlParameter("@WarehouseName", WarehouseName);
                parms[2] = new SqlParameter("@CreatedTimestart", CreatedTimestart);
                parms[3] = new SqlParameter("@CreatedTimeEnd", CreatedTimeEnd);
                var list = ys.Database.SqlQuery<SP_Warehousesetup_Result>("exec SP_Warehousesetup @Type,@WarehouseName,@CreatedTimestart,@CreatedTimeEnd", parms).ToList();
                PageList<SP_Warehousesetup_Result> pageList = new PageList<SP_Warehousesetup_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public string Warehousedel(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "Warehousedel");
                parms[1] = new SqlParameter("@id", id);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup] @Type,'','','',@id", parms);
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
        public string Warehouseupdata(string id, string Name, string Description, string CreatedBy, string StatusID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Type", "Warehouseupdata");
                parms[1] = new SqlParameter("@WarehouseName", Name);
                parms[2] = new SqlParameter("@id", id);
                parms[3] = new SqlParameter("@Description", Description);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                parms[5] = new SqlParameter("@StatusID", StatusID);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup] @Type,@WarehouseName,'','',@id,@Description,@CreatedBy,@StatusID", parms);
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
        public ActionResult Warehouseadd()
        {
            return View();
        }
        public string Warehouseadditions(string Name, string Description)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Type", "WarehouseAdd");
                parms[1] = new SqlParameter("@WarehouseName", Name);
                parms[2] = new SqlParameter("@Description", Description);
                parms[3] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup] @Type,@WarehouseName,'','','',@Description,@CreatedBy", parms);
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
        public ActionResult WarehouseArea()
        {
            return View();
        }
        public string AreaAdd(string WHArea, string Description, string WHName)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Type", "areaAdd");
                parms[1] = new SqlParameter("@WHArea", WHArea);
                parms[2] = new SqlParameter("@Description", Description);
                parms[3] = new SqlParameter("@WHName", WHName);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_area] @Type,'',@WHName,@WHArea,@Description,@CreatedBy", parms);
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
        #region 根据仓库查库区
        public ActionResult Warehousesel()
        {
            return View();
        }
        public JsonResult Arealist(string WHID, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "area");
                parms[1] = new SqlParameter("@WHID", WHID);
                var list = ys.Database.SqlQuery<SP_Warehousesetup_area_Result>("exec SP_Warehousesetup_area @Type,@WHID", parms).ToList();
                PageList<SP_Warehousesetup_area_Result> pageList = new PageList<SP_Warehousesetup_area_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public string Areadel(string WHID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "areadel");
                parms[1] = new SqlParameter("@WHID", WHID);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_area] @Type,@WHID", parms);
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
        public string Areaupdata(string WHAreaID, string WHArea, string Description, string CreatedBy, string StatusID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Type", "areaupdata");
                parms[1] = new SqlParameter("@WHID", WHAreaID);
                parms[2] = new SqlParameter("@WHArea", WHArea);
                parms[3] = new SqlParameter("@Description", Description);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                parms[5] = new SqlParameter("@StatusID", StatusID);
                int i = ys.Database.ExecuteSqlCommand("exec[SP_Warehousesetup_area] @Type,@WHID,'',@WHArea,@Description,@CreatedBy,@StatusID", parms);
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
        public ActionResult Storage()
        {
            return View();
        }
        public string StorageAdd(string Name, string Description, string WHArea)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Type", "WHStorageAdd");
                parms[1] = new SqlParameter("@Name", Name);
                parms[2] = new SqlParameter("@Description", Description);
                parms[3] = new SqlParameter("@WHAreaName", WHArea);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_WHStorage] @Type,'',@Name,@Description,@WHAreaName,@CreatedBy", parms);
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
        #region 根据仓库跟库区名查货区
        public ActionResult WHStorage()
        {
            return View();
        }
        public JsonResult WHStoragelist(string WHAreaID, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "WHStoragelist");
                parms[1] = new SqlParameter("@WHAreaID", WHAreaID);
                var list = ys.Database.SqlQuery<SP_Warehousesetup_WHStorage_Result>("exec SP_Warehousesetup_WHStorage @Type,@WHAreaID", parms).ToList();
                PageList<SP_Warehousesetup_WHStorage_Result> pageList = new PageList<SP_Warehousesetup_WHStorage_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public string WHStoragedel(string StorageLocationID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "WHStoragedel");
                parms[1] = new SqlParameter("@WHAreaID", StorageLocationID);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_WHStorage] @Type,@WHAreaID", parms);
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
        public string WHStorageUpdata(string StorageLocationID, string Name, string Description, string CreatedBy, string StatusID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Type", "WHStorageupdata");
                parms[1] = new SqlParameter("@WHAreaID", StorageLocationID);
                parms[2] = new SqlParameter("@Name", Name);
                parms[3] = new SqlParameter("@Description", Description);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                parms[5] = new SqlParameter("@StatusID", StatusID);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_WHStorage] @Type,@WHAreaID,@Name,@Description,'',@CreatedBy,@StatusID", parms);
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
        public ActionResult WHGoodsAlAdd()
        {
            return View();
        }
        public string GoodsAlAdd(string StorageName, string Name, string Description)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Type", "WHGoodsAlAdd");
                parms[1] = new SqlParameter("@Name", Name);
                parms[2] = new SqlParameter("@Description", Description);
                parms[3] = new SqlParameter("@StorageName", StorageName);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_WHGoodsAl] @Type,'',@StorageName,@Name,@Description,@CreatedBy", parms);
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
        #region 根据货区查询货位
        public ActionResult WHGoodsAl()
        {
            return View();
        }
        public JsonResult WHGoodsAllist(string StorageLocationID, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "WHGoodsAllist");
                parms[1] = new SqlParameter("@StorageLocationID", StorageLocationID);
                var list = ys.Database.SqlQuery<SP_Warehousesetup_WHGoodsAl_Result>("exec SP_Warehousesetup_WHGoodsAl @Type,@StorageLocationID", parms).ToList();
                PageList<SP_Warehousesetup_WHGoodsAl_Result> pageList = new PageList<SP_Warehousesetup_WHGoodsAl_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        public string WHGoodsAldel(string GoodsAlid)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "WHGoodsAldel");
                parms[1] = new SqlParameter("@StorageLocationID", GoodsAlid);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_WHGoodsAl] @Type,@StorageLocationID", parms);
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
        public string WHGoodsAlupdata(string GoodsAlid, string Name, string Description, string CreatedBy, string StatusID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Type", "WHGoodsAlupdata");
                parms[1] = new SqlParameter("@StorageLocationID", GoodsAlid);
                parms[2] = new SqlParameter("@Name", Name);
                parms[3] = new SqlParameter("@Description", Description);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                parms[5] = new SqlParameter("@StatusID", StatusID);
                int i = ys.Database.ExecuteSqlCommand("exec [SP_Warehousesetup_WHGoodsAl] @Type,@StorageLocationID,'',@Name,@Description,@CreatedBy,@StatusID", parms);
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

        #region 批量增加
        public ActionResult WHbatch()
        {
            return View();

        }
        public JsonResult WHName()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.PM_WH.ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult coun(string WHName)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@WHName", WHName);
                var list = ys.Database.SqlQuery<WHNameMAX_Result>("exec WHNameMAX @WHName", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }


        }
        public string WHNamebatch(string WHName, string WHArea, string WHStorage, string WHGoodsAl)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@WHName", WHName);
                parms[1] = new SqlParameter("@WHArea", Convert.ToInt32(WHArea));
                parms[2] = new SqlParameter("@WHStorage", Convert.ToInt32(WHStorage));
                parms[3] = new SqlParameter("@WHGoodsAl", Convert.ToInt32(WHGoodsAl));
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec [WHNamebatch] @WHName,@WHArea,@WHStorage,@WHGoodsAl,@CreatedBy", parms);
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
        #endregion
    }
}