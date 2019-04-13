using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;
using YlMES;
using YLMES;
using System.Data.SqlClient;
using PrintLib.Printers.Zebra;
using System.Drawing;

namespace YLMES.Controllers
{
    public class FinishProductHouseController : Controller
    {
        // GET: FinishProductHouse
        public ActionResult FinishedProductRegistration()
        {
            return View();
        }
        //显示成品仓位置信息
        public JsonResult SeeFinishedProductWarehouse(string Reservoir,string CargoArea,string Goods,int page,int limit)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Reservoir == null)
                {
                    Reservoir = "";
                }
                if (CargoArea == null)
                {
                    CargoArea = "";
                }
                if (Goods == null)
                {
                    Goods = "";
                }
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Reservoir", "");
                parms[1] = new SqlParameter("@CargoArea", "");
                parms[2] = new SqlParameter("@Goods", "");
                parms[3] = new SqlParameter("@Type", "check");
                var list = ys.Database.SqlQuery<CheckWarehouseLocation_Result>("exec CheckWarehouseLocation @Reservoir,@CargoArea,@Goods,@Type", parms).ToList();
                Dictionary<string, object> hasmap = new Dictionary<string, object>();
                PageList<CheckWarehouseLocation_Result> pageList = new PageList<CheckWarehouseLocation_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);

            }
        }
        //新增成品仓位置信息页面
        public ActionResult AddFinishLocation()
        {
            return View();
        }
        //新增成品仓位置信息
        public ActionResult AddFinishProLocation(string Reservoir,string CargoArea,string Goods)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@Reservoir", Reservoir);
                parms[1] = new SqlParameter("@CargoArea", CargoArea);
                parms[2] = new SqlParameter("@Goods", Goods);
                parms[3] = new SqlParameter("@Type", "Add");
              int i=ys.Database.ExecuteSqlCommand("exec CheckWarehouseLocation @Reservoir,@CargoArea,@Goods,@Type", parms);
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
        //删除成品仓位置信息
        public ActionResult DeleteFinishProLocation(string id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {               
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Reservoir", "");
                parms[1] = new SqlParameter("@CargoArea", "");
                parms[2] = new SqlParameter("@Goods", "");
                parms[3] = new SqlParameter("@Type", "delete");
                parms[4] = new SqlParameter("@id", id);
                int i = ys.Database.ExecuteSqlCommand("exec CheckWarehouseLocation @Reservoir,@CargoArea,@Goods,@Type,@id", parms);
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
        //添加引用产品页面
        public ActionResult CheckPackagingProducts(string id)
        {
            ViewData["id"] = id;
            return View();
        }
        //修改成品仓货
     public ActionResult CheckFinishedProductWarehouse(string id)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];                
                parms[0] = new SqlParameter("@id", id);
                var list = ys.Database.SqlQuery<CheckWarehouseLocation2_Result>("exec CheckWarehouseLocation2 @id", parms).ToList();
                Dictionary<string, object> hasmap = new Dictionary<string, object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        
        }
        //添加项目名称页面
        public ActionResult AddProjectName(string id)
        {
            ViewData["id"] = id;
            return View();
        }
        //显示项目编号信息
        public ActionResult getCNumber()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var list = ys.C_Contract.ToList();
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("data", list);
                return Json(map, JsonRequestBehavior.AllowGet);
            }
        }
        //添加项目编号
        public ActionResult AddProjectNumber(string id,string CNumber)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(id);
                WarehouseLocation wl = ys.WarehouseLocation.Where(c => c.id == i).FirstOrDefault();
                wl.ContractNumber = CNumber;
                ys.SaveChanges();
                return Content("true");
            }          
        }
        //打印成品仓位置页面
        public ActionResult txt(string id,string Reservoir,string CargoArea,string Goods)
        {
            string fileName = "1.png";
            string ImageUrl = null;
            String savePath = Server.MapPath("~/QRCodeImage") + "/" + fileName;
            Printer pt = new Printer();
            Image image = pt.CreateQRCodeImage(id, savePath);
            ImageUrl = "../../QRCodeImage/" + fileName;
            TempData["ptu"] = ImageUrl;
            ViewData["Reservoir"] = Reservoir;
            ViewData["CargoArea"] = CargoArea;
            ViewData["Goods"] = Goods;
            return View();
        }
        //显示成品仓库存查询信息页面
        public ActionResult FinishedGoods()
        {
            return View();
        }
    }
}