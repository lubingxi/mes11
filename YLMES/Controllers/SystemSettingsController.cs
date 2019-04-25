using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class SystemSettingsController : Controller
    {
        // GET: SystemSettings
        public ActionResult Index()
        {
            return View();
        }

        #region 权限设置
        public ActionResult Permissions()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<PM_FunctionList> list = ys.PM_FunctionList.ToList();
                ViewBag.ck = list;
                
            }
            return View();
        }
        public ActionResult ProductLine()
        {

            return View();
        }
        public ActionResult ProductStation()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<PM_ProductLine> list = ys.PM_ProductLine.ToList();
                List<PM_ProductStationType> list1 = ys.PM_ProductStationType.ToList();
                ViewBag.ce = list1;        
                ViewBag.ck = list;        
                return View();
            }

        }
        //添加工位
        public string ADDProductStation()
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "ADD");
                parms[1] = new SqlParameter("@CreatedBy", CreatedBy.ToString());
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductStation @Type,'','','','','','','','',@CreatedBy", parms);
            }
            return "true";
        }
        //删除工位类型
        public string DlProductStation(string StationID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "delete");
                parms[1] = new SqlParameter("@StationID", Convert.ToInt32(StationID));
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductStation @Type,@StationID", parms);
            }
            return "true";
        }
        //修改工位类型
        public string UpProductStation(string Line, string StationID, string Station, string StationType)
        {
            string CreatedBy = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@Type", "Update");
                parms[1] = new SqlParameter("@StationID", Convert.ToInt32(StationID));
                parms[2] = new SqlParameter("@Station", Station);
                parms[3] = new SqlParameter("@Line", Line);
                parms[4] = new SqlParameter("@StationType", StationType);
                parms[5] = new SqlParameter("@CreatedBy", CreatedBy.ToString());
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductStation @Type,@StationID,@Station,'',@Line,'','','',@StationType,'',@CreatedBy", parms);


            }
            return "true";
        }
        //搜索工位类型
        public ActionResult SeProductStation(string Name, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Name == null)
                {
                    Name = "";
                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@Station", Name);
                var list = ys.Database.SqlQuery<SP_PM_ProductStation_Result>("exec SP_PM_ProductStation @Type,'',@Station", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_ProductStation_Result> pageList = new PageList<SP_PM_ProductStation_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //添加生产线
        public string ADDProduct()
        {
            string CreatedBy = Session["name"].ToString();

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "ADD");
                parms[1] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @Type,'','','',@CreatedBy", parms);
            }
            return "true";
        }
        //删除生产线
        public string DlProduct(string lineID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "delete");
                parms[1] = new SqlParameter("@lineID", Convert.ToInt32(lineID));
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @Type,@lineID", parms);

            }
            return "true";
        }
        //修改生产线信息
        public string UpProduct(string line, string lineID,string lengsd)
        {
            string CreatedBy = Session["name"].ToString();

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Type", "Update");
                parms[1] = new SqlParameter("@line", line);
                parms[2] = new SqlParameter("@lineID", Int32.Parse(lineID));
                parms[3] = new SqlParameter("@LineLength", lengsd);
                parms[4] = new SqlParameter("@CreatedBy", CreatedBy);
                int i = ys.Database.ExecuteSqlCommand("exec SP_PM_ProductLine @Type,@lineID,@line,@LineLength,@CreatedBy", parms);
            }
            return "true";
        }
        //搜索生产线
        public ActionResult SeProductLine(string Name, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "check");
                parms[1] = new SqlParameter("@line", Name);
                var list = ys.Database.SqlQuery<SP_PM_ProductLine_Result>("exec SP_PM_ProductLine @Type,'',@line", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_ProductLine_Result> pageList = new PageList<SP_PM_ProductLine_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //查询已有权限
        public ActionResult ByExistsRoot(string UserName, string FunctionName, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@type", "Type");
                parms[1] = new SqlParameter("@UserName", UserName);
                parms[2] = new SqlParameter("@FunctionName", FunctionName);
                parms[3] = new SqlParameter("@typeid", int.Parse("1"));
                var list = ys.Database.SqlQuery<PM_EditAccessSetup_Result>("exec PM_EditAccessSetup @type,@UserName,@FunctionName,'',@typeid", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<PM_EditAccessSetup_Result> pageList = new PageList<PM_EditAccessSetup_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
        //查询未有权限
        public ActionResult ByNoExistsRoot(string UserName, string FunctionName, int page, int limit)
        {
            Dictionary<string, Object> hasmap;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@type", "Type");
                parms[1] = new SqlParameter("@UserName", UserName);
                parms[2] = new SqlParameter("@FunctionName", FunctionName);
                parms[3] = new SqlParameter("@typeid", int.Parse("0"));
                var list = ys.Database.SqlQuery<PM_EditAccessSetup_Result>("exec PM_EditAccessSetup @type,@UserName,@FunctionName,'',@typeid", parms).ToList();
                hasmap = new Dictionary<string, Object>();
                PageList<PM_EditAccessSetup_Result> pageList = new PageList<PM_EditAccessSetup_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);

            }
            return Json(hasmap, JsonRequestBehavior.AllowGet);


        }
        //权限修改
        public string UpRole(string FunctionName, int Status, string UserName)
        {

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {

                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@type", "EDIT");
                parms[1] = new SqlParameter("@UserName", UserName);
                parms[2] = new SqlParameter("@FunctionName", FunctionName);
                parms[3] = new SqlParameter("@Status", Status);
                int i = ys.Database.ExecuteSqlCommand("exec PM_EditAccessSetup @type,@UserName,@FunctionName,@Status,''", parms);



            }
            return "true";
        }

        #endregion

        #region 系统权限工艺BOM
        public ActionResult Process()
        {
            return View();
        }
        //显示工艺BOM信息
        public ActionResult CheckProcessBOM(string Name, string PartSpec, string Material,int page,int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Name == null)
                {
                    Name = "";
                }
                if (PartSpec == null)
                {
                    PartSpec = "";
                }
                if(Material==null)
                {
                    Material = "";
                }
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@PartNumber", Name);
                parms[1] = new SqlParameter("@PartSpec", PartSpec);
                parms[2] = new SqlParameter("@PartMaterial", Material);
                var list = ys.Database.SqlQuery<ProcessBOM_Result>("exec ProcessBOM @PartNumber,@PartSpec,@PartMaterial", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<ProcessBOM_Result> pageList = new PageList<ProcessBOM_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //导入EXCEL
        //public ActionResult StationImport(HttpPostedFileBase files)
        //{
        //    try
        //    {

        //        return Content("true");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return Content("false");
        //    }

        //}
        //删除BOM
        public ActionResult DeleteProcessBOM(string id)
        {
            try
            {
                int tid = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@ID", tid);
                parms[1] = new SqlParameter("@PartNumber", "");
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec  DeleteProcessBOM  @ID,@PartNumber", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //修改BOM
        public ActionResult EditProcessBOM(string id, string pc, string ph,string mqty,string spec,string unit)
        {
            try
            {
                int tid = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@ID", tid);
                parms[1] = new SqlParameter("@PartNumber", "");
                parms[2] = new SqlParameter("@ChildPartQTY", pc);
                parms[3] = new SqlParameter("@ChildPartNumber", ph);
                parms[4] = new SqlParameter("@MQty", mqty);
                parms[5] = new SqlParameter("@Unit", unit);
                parms[6] = new SqlParameter("@Spec", spec);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec  EditProcessBOM  @ID,@PartNumber,@ChildPartQTY,@ChildPartNumber,@MQty,@Unit,@Spec", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //添加子件
        public ActionResult AddProcessBOM(string name, string PartSpec)
        {
            try
            {
                string username = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@PartNumber", name);
                parms[1] = new SqlParameter("@PartSpec", PartSpec);
                parms[2] = new SqlParameter("@CreatedBy", username);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec  AddProcessBOM  @PartNumber,@PartSpec,@CreatedBy", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //显示子件
        public ActionResult BOM2(string name)
        {
            ViewData["name"] = name;
            return View();
        }
        #endregion

        #region 库区

        public ActionResult Reservoir()
        {
            return View();
        }
        //显示库区信息
        public ActionResult CheckReservoir(string select, string Reservoir, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "wh");
                parms[1] = new SqlParameter("@WHArea", Reservoir);
                parms[2] = new SqlParameter("@WH", select);
                 var list = ys.Database.SqlQuery<CheckWarehouse_Result>("exec CheckWarehouse @Type,@WHArea,@WH", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<CheckWarehouse_Result> pageList = new PageList<CheckWarehouse_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //删除库区信息
        public ActionResult DeleteReservoir(string WHArea)
        {
            try
            {
                int wha = int.Parse(WHArea);
                List<SqlParameter> paramArray = new List<SqlParameter>();
                paramArray.Add(new SqlParameter("@Type", "DeleteWHArea"));
                paramArray.Add(new SqlParameter("@WHAreaID", wha));
                paramArray.Add(new SqlParameter("@WHStorageLocationID", 999));
                paramArray.Add(new SqlParameter("@WHGoodsAllocationID", 999));
                SqlParameter param = new SqlParameter("@MSG", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                paramArray.Add(param);            
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec DeleteWH  @Type,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID,@MSG out", paramArray.ToArray());
                }
                int result = (int)paramArray[4].Value;
                if (result == 1)
                {
                    return Content("Error");
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("true");
            }
        }
        //修改库区信息
        public ActionResult EditReservoir(string id, string wh, string ms, string ck)
        {
            try
            {
                string name = Session["name"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[9];
                parms[0] = new SqlParameter("@TYPE", "UpdateWHArea");
                parms[1] = new SqlParameter("@ID", i);
                parms[2] = new SqlParameter("@WHID",666);
                parms[3] = new SqlParameter("@WH", ck);
                parms[4] = new SqlParameter("@WHArea", wh);
                parms[5] = new SqlParameter("@CreatedBy", name);
                parms[6] = new SqlParameter("@desc", ms);
                parms[7] = new SqlParameter("@WHStorageLocation", "");
                parms[8] = new SqlParameter("@WHGoodsAllocation", "");
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec EditWH  @TYPE,@ID,@WHID,@WH,@WHArea,@CreatedBy,@desc,@WHStorageLocation,@WHGoodsAllocation", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //新增库区
        public ActionResult AddReservoir(string id)
        {
            try
            {
                string name = Session["name"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "ADDWHArea");
                parms[1] = new SqlParameter("@WHID", i);
                parms[2] = new SqlParameter("@WHAreaID", 666);
                parms[3] = new SqlParameter("@WHStorageLocationID", 999);
                parms[4] = new SqlParameter("@CreatedBy", name);           
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec AddWH  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@CreatedBy", parms);
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

        #region 库位

        public ActionResult Location()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                ViewBag.Kq = FindPM_WHArea(ys);
            }
            return View();
        }
        public ActionResult kqd(string ck)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int id;
                if (ck == "成品仓")
                {
                    id = 4;
                }
                else if (ck == "五金仓")
                {
                    id = 2;
                }
                else
                {
                    id = 1;
                }
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@TYPE", "WH");
                parms[1] = new SqlParameter("@WHID", id);
                parms[2] = new SqlParameter("@WHAreaID", "");
                parms[3] = new SqlParameter("@WHStorageLocationID", "");
                var list = ys.Database.SqlQuery<CheckDropWhList_Result>("exec CheckDropWhList  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID", parms).ToList();
                ViewBag.Kq = list;
            }
            return PartialView("kqd");
        }
        //显示库位信息
        public ActionResult CheckLocation(string select, string Reservoir, string location, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@WHArea", Reservoir);
                parms[1] = new SqlParameter("@WH", select);
                parms[2] = new SqlParameter("@WHStorageLocation", location);
                var list = ys.Database.SqlQuery<CheckStorage_Result>("exec CheckStorage @WHArea,@WH,@WHStorageLocation", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<CheckStorage_Result> pageList = new PageList<CheckStorage_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //删除库位
        public ActionResult DeleteLocation(string id)
        {
            try
            {
                int lid = int.Parse(id);
                List<SqlParameter> paramArray = new List<SqlParameter>();
                paramArray.Add(new SqlParameter("@Type", "DeleteWHStorageLocation"));
                paramArray.Add(new SqlParameter("@WHAreaID", 999));
                paramArray.Add(new SqlParameter("@WHStorageLocationID", lid));
                paramArray.Add(new SqlParameter("@WHGoodsAllocationID", 999));
                SqlParameter param = new SqlParameter("@MSG", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                paramArray.Add(param);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec DeleteWH  @Type,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID,@MSG out", paramArray.ToArray());
                }
                int result = (int)paramArray[4].Value;
                if (result == 1)
                {
                    return Content("Error");
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("true");
            }
        }
        //修改库位
        public ActionResult EditLocation(string id,string ms,string Slocation)
        {
            try
            {
                string name = Session["name"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[9];
                parms[0] = new SqlParameter("@TYPE", "UpdateWHStorageLocation");
                parms[1] = new SqlParameter("@ID", i);
                parms[2] = new SqlParameter("@WHID", 666);
                parms[3] = new SqlParameter("@WH", "");
                parms[4] = new SqlParameter("@WHArea", "");
                parms[5] = new SqlParameter("@CreatedBy", name);
                parms[6] = new SqlParameter("@desc", ms);
                parms[7] = new SqlParameter("@WHStorageLocation", Slocation);
                parms[8] = new SqlParameter("@WHGoodsAllocation", "");
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec EditWH  @TYPE,@ID,@WHID,@WH,@WHArea,@CreatedBy,@desc,@WHStorageLocation,@WHGoodsAllocation", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //新增库位
        public ActionResult AddLocation(string id)
        {
            try
            {
                string name = Session["name"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "ADDWHStorageLocation");
                parms[1] = new SqlParameter("@WHID", 666);
                parms[2] = new SqlParameter("@WHAreaID",999);
                parms[3] = new SqlParameter("@WHStorageLocationID", i);
                parms[4] = new SqlParameter("@CreatedBy", name);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec AddWH  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@CreatedBy", parms);
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

        #region 货位

        public ActionResult Goods()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                ViewBag.Kq = FindPM_WHArea(ys);
                ViewBag.Kw = FindPM_WHStorageLocation(ys); 
            }
         
            return View();
        }
        //显示货位信息
        public ActionResult CheckGoods(string select, string Reservoir, string location, string Goods, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@WHArea", Reservoir);
                parms[1] = new SqlParameter("@WH", select);
                parms[2] = new SqlParameter("@WHStorageLocation", location);
                parms[3] = new SqlParameter("@WHGoodsAllocation", Goods);
                var list = ys.Database.SqlQuery<CheckGoods_Result>("exec CheckGoods @WHArea,@WH,@WHStorageLocation,@WHGoodsAllocation", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<CheckGoods_Result> pageList = new PageList<CheckGoods_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //删除货位
        public ActionResult DeleteGoods(string id)
        {
            try
            {
                int lid = int.Parse(id);
                List<SqlParameter> paramArray = new List<SqlParameter>();
                paramArray.Add(new SqlParameter("@Type", "DeleteWHGoodsAllocation"));
                paramArray.Add(new SqlParameter("@WHAreaID", 999));
                paramArray.Add(new SqlParameter("@WHStorageLocationID", 999));
                paramArray.Add(new SqlParameter("@WHGoodsAllocationID", lid));
                SqlParameter param = new SqlParameter("@MSG", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                paramArray.Add(param);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec DeleteWH  @Type,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID,@MSG out", paramArray.ToArray());
                }
                int result = (int)paramArray[4].Value;
                if (result == 1)
                {
                    return Content("Error");
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("true");
            }
        }
        //修改货位
        public ActionResult EditGoods(string id, string ms, string Glocation)
        {
            try
            {
                string name = Session["name"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[9];
                parms[0] = new SqlParameter("@TYPE", "UpdateWHGoodsAllocation");
                parms[1] = new SqlParameter("@ID", i);
                parms[2] = new SqlParameter("@WHID", 666);
                parms[3] = new SqlParameter("@WH", "");
                parms[4] = new SqlParameter("@WHArea", "");
                parms[5] = new SqlParameter("@CreatedBy", name);
                parms[6] = new SqlParameter("@desc", ms);
                parms[7] = new SqlParameter("@WHStorageLocation", "");
                parms[8] = new SqlParameter("@WHGoodsAllocation", Glocation);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec EditWH  @TYPE,@ID,@WHID,@WH,@WHArea,@CreatedBy,@desc,@WHStorageLocation,@WHGoodsAllocation", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //新增货位
        public ActionResult AddGoods(string id)
        {
            try
            {
                string name = Session["name"].ToString();
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "ADDWHGoodsAllocation");
                parms[1] = new SqlParameter("@WHID", 666);
                parms[2] = new SqlParameter("@WHAreaID", 999);
                parms[3] = new SqlParameter("@WHStorageLocationID", i);
                parms[4] = new SqlParameter("@CreatedBy", name);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec AddWH  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@CreatedBy", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }

        public ActionResult kwd(string kw, string ck)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int id;
                if (ck == "成品仓")
                {
                    id = 4;
                }
                else if (ck == "五金仓")
                {
                    id = 2;
                }
                else
                {
                    id = 1;
                }
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@WHID", id);
                parms[1] = new SqlParameter("@WHAreaID", kw);
                var list = ys.Database.SqlQuery<CheckDropAreaList_Result>("exec CheckDropAreaList  @WHID,@WHAreaID", parms).ToList();
                ViewBag.Kw = list;
            }
            return PartialView("kwd");
        }    
        #endregion

        #region 显示下拉信息
        public List<PM_WHArea> FindPM_WHArea(YLMES_newEntities ys)
        {
            var cache = CacheHelper.GetCache("PM_WHArea");//先读取
            if (cache == null)//如果没有该缓存
            {
                List<PM_WHArea> wh = ys.PM_WHArea.ToList();
                CacheHelper.SetCache("PM_WHArea", wh);//添加缓存
                return wh;
            }
            var result = (List<PM_WHArea>)cache;//有就直接返回该缓存
            return result;
        }
        public List<PM_WHStorageLocation> FindPM_WHStorageLocation(YLMES_newEntities ys)
        {
            var cache = CacheHelper.GetCache("PM_WHStorageLocation");//先读取
            if (cache == null)//如果没有该缓存
            {
                List<PM_WHStorageLocation> wh = ys.PM_WHStorageLocation.ToList();
                CacheHelper.SetCache("PM_WHStorageLocation", wh);//添加缓存
                return wh;
            }
            var result = (List<PM_WHStorageLocation>)cache;//有就直接返回该缓存
            return result;
        }
        public List<PM_WHGoodsAllocation> FindPM_WHGoodsAllocation(YLMES_newEntities ys)
        {
            var cache = CacheHelper.GetCache("PM_WHGoodsAllocation");//先读取
            if (cache == null)//如果没有该缓存
            {
                List<PM_WHGoodsAllocation> wh = ys.PM_WHGoodsAllocation.ToList();
                CacheHelper.SetCache("PM_WHGoodsAllocation", wh);//添加缓存
                return wh;
            }
            var result = (List<PM_WHGoodsAllocation>)cache;//有就直接返回该缓存
            return result;
        }
        #endregion

        #region 生产工位类型
        //工位信息页面
        public ActionResult LocationType()
        {
            return View();
        }
        //显示工位信息
        public ActionResult CheckLocationType(string Type,int page,int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@StationType", Type);
                parms[1] = new SqlParameter("@Type", "check");
                var list = ys.Database.SqlQuery<CheckStationType_Result>("exec CheckStationType @StationType,@Type", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<CheckStationType_Result> pageList = new PageList<CheckStationType_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //删除工位类型
        public ActionResult DeleteLocationType(string id)
        {
            try
            {
                int i = int.Parse(id);
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "delete");
                parms[1] = new SqlParameter("@id", i);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec CheckStationType  '',@Type,@id", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //修改工位类型
        public ActionResult EditLocationType(string id, string Station)
        {
            try
            {
                int i = int.Parse(id);
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@StationType", Station);
                parms[1] = new SqlParameter("@Type", "update");
                parms[2] = new SqlParameter("@id", i);
                parms[3] = new SqlParameter("@CreatedBy", name);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec CheckStationType  @StationType,@Type,@id,@CreatedBy", parms);
                }
                return Content("true");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }
        }
        //添加工位类型
        public ActionResult AddLocationType()
        {
            try
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@StationType", "");
                parms[1] = new SqlParameter("@Type", "add");
                parms[2] = new SqlParameter("@id", 666);
                parms[3] = new SqlParameter("@CreatedBy", name);
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    ys.Database.ExecuteSqlCommand("exec CheckStationType  @StationType,@Type,@id,@CreatedBy", parms);
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
    }
}