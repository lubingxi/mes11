using System;
using PrintLib.Printers.Zebra;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using YLMES.Models;
using System.Reflection;

namespace YlMES.Controllers
{
    public class RmsController : Controller
    {
        // GET: Rms
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult bianma(string cc, string c2, string c3, string hw)
        {
            ViewData["ck"] = cc;
            ViewData["kq"] = c2;
            ViewData["hq"] = c3;
            ViewData["hw"] = hw;          
            return PartialView("bianma");
        }

        #region 新增物料信息
        //新增页面
        public ActionResult AddMaterial()
        {
            MaterialRegistration();
           
            return View();
        }
        //新增数据
        public ActionResult AddJsonMaterial(string c1, string c2, string fn, string pn, string ps, string pm,string listType)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[9];
                    parms[0] = new SqlParameter("@type", "新增");
                    parms[1] = new SqlParameter("@id","");
                    parms[2] = new SqlParameter("@Category1ID", c1);
                    parms[3] = new SqlParameter("@Category2ID", c2);
                    parms[4] = new SqlParameter("@figureNumber", fn);
                    parms[5] = new SqlParameter("@PartNumber", pn);
                    parms[6] = new SqlParameter("@PartSpec", ps);
                    parms[7] = new SqlParameter("@PartMaterial", pm);
                    parms[8] = new SqlParameter("@ListType", listType);
                    ys.Database.ExecuteSqlCommand("exec UpdatePmc  @type,@id,@Category1ID,@Category2ID,@figureNumber,@PartNumber,@PartSpec,@PartMaterial,@ListType", parms);
                }
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content("false");
            }
           
        }
        #endregion

        #region 打印二维码
        public ContentResult txt(string txt,string cangku,string kuqu,string kuwei,string huowei)
        {            
            try
            {
               
                TempData["cangkuCache"] = cangku;
                TempData["kuquCache"] = kuqu;
                TempData["kuweiCache"] = kuwei;
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    SqlParameter[] parms = new SqlParameter[3];
                    parms[0] = new SqlParameter("@WHID", cangku);
                    parms[1] = new SqlParameter("@WHAreaID", kuqu);
                    parms[2] = new SqlParameter("@WHStorageLocationID", kuwei);
                    var list = ys.Database.SqlQuery<CheckDropKwList_Result>("exec CheckDropKwList  @WHID,@WHAreaID,@WHStorageLocationID", parms).ToList();

                    TempData["huoweiCache"] = list;
                }
             
                if (txt != null)
                {
                    string fileName =  "1.png";
                    string ImageUrl = null;
                    String savePath = Server.MapPath("~/QRCodeImage") + "/" + fileName;
                    Printer pt = new Printer();
                    Image image = pt.CreateQRCodeImage(txt, savePath);
                    ImageUrl = "../../QRCodeImage/" + fileName;
                    TempData["ptu"] = ImageUrl;

                    return Content("true");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Content("false");
        }
        #endregion

        #region 插入和显示打印信息
        public ActionResult JinCang(string ck, string kq, string kw, string hw,string hww,string tuhao, string cailiao, string guige, string named, string shuliang)
        {
            ViewData["canku"] = ck;
            ViewData["kuqu"] = kq;
            ViewData["huoqu"] = kw;
            ViewData["huowei"] = hw;
            ViewData["tuhao"] = tuhao;
            ViewData["cailiao"] = cailiao;
            ViewData["guige"] = guige;
            ViewData["mingzi"] = named;
            TempData["ptu"].ToString();
            int wuliaoId = 0
                
                ;
            int sl = int.Parse(shuliang);
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                var wid = ys.PM_Material.Where(p => p.PartNumber==named).FirstOrDefault();
                wuliaoId = wid.ID;
            }
            using (YLMES_newEntities yd = new YLMES_newEntities())
            {
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@MaterialID",wuliaoId);
                parms[1] = new SqlParameter("@InQTY", sl);
                parms[2] = new SqlParameter("@Location", hww);
                parms[3] = new SqlParameter("@createdBy", name);
                yd.Database.ExecuteSqlCommand("exec AddPM_MaterialList @MaterialID,@InQTY,@Location,@createdBy", parms);
            }
            return View();
        }
        #endregion

        #region 分布视图仓库
        public ActionResult kqd(string ck)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[4];
                parms[0] = new SqlParameter("@TYPE", "WH");
                parms[1] = new SqlParameter("@WHID", ck);
                parms[2] = new SqlParameter("@WHAreaID", "");
                parms[3] = new SqlParameter("@WHStorageLocationID", "");
                var list = ys.Database.SqlQuery<CheckDropWhList_Result>("exec CheckDropWhList  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID", parms).ToList();
                ViewBag.Kq = list;
            }
            return PartialView("kqd");
        }
        public ActionResult kwd(string kw, string ck)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@WHID", ck);
                parms[1] = new SqlParameter("@WHAreaID", kw);
                var list = ys.Database.SqlQuery<CheckDropAreaList_Result>("exec CheckDropAreaList  @WHID,@WHAreaID", parms).ToList();
                ViewBag.Kw = list;
            }
            return PartialView("kwd");
        }
        public ActionResult hwd(string kq, string cq, string hq)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@WHID", cq);
                parms[1] = new SqlParameter("@WHAreaID", kq);
                parms[2] = new SqlParameter("@WHStorageLocationID", hq);
                var list = ys.Database.SqlQuery<CheckDropKwList_Result>("exec CheckDropKwList  @WHID,@WHAreaID,@WHStorageLocationID", parms).ToList();
                ViewBag.Hw = list;
            }
            return PartialView("hwd");
        }
        #endregion

        #region 初始库区值
        public ActionResult Pbc(string figureNumber, string PartNumber, string PartSpec, string PartMaterial)
        {
            ViewData["fn"] = figureNumber;
            ViewData["pn"] = PartNumber;
            ViewData["ps"] = PartSpec;
            ViewData["pm"] = PartMaterial;
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {                         
                      ViewBag.ck = FindPM_WH(ys);                
                      ViewBag.Kq = FindPM_WHArea(ys);           
                      ViewBag.Kw = FindPM_WHStorageLocation(ys);
                      ViewBag.Hw = FindPM_WHGoodsAllocation(ys);                               
            }
            return View();
        }
        #endregion

        #region 仓库删除

        public ActionResult DelCk(string ck)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(ck);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "DeleteWH");
                parms[1] = new SqlParameter("@WHID", i);
                parms[2] = new SqlParameter("@WHAreaID", 100);
                parms[3] = new SqlParameter("@WHStorageLocationID", 100);
                parms[4] = new SqlParameter("@WHGoodsAllocationID", 100);
                int w = ys.Database.ExecuteSqlCommand("exec PM_WH_Delete  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }
        public ActionResult DelKq(string Kq)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(Kq);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "DeleteWHArea");
                parms[1] = new SqlParameter("@WHID", 100);
                parms[2] = new SqlParameter("@WHAreaID", i);
                parms[3] = new SqlParameter("@WHStorageLocationID", 100);
                parms[4] = new SqlParameter("@WHGoodsAllocationID", 100);
                int w = ys.Database.ExecuteSqlCommand("exec PM_WH_Delete  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }
        public ActionResult DelKw(string Kw)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(Kw);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "DeleteWHStorageLocation");
                parms[1] = new SqlParameter("@WHID", 100);
                parms[2] = new SqlParameter("@WHAreaID", 100);
                parms[3] = new SqlParameter("@WHStorageLocationID", i);
                parms[4] = new SqlParameter("@WHGoodsAllocationID", 100);
                int w = ys.Database.ExecuteSqlCommand("exec PM_WH_Delete  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }
        public ActionResult DelHw(string Hw)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int i = int.Parse(Hw);
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@TYPE", "DeleteWHGoodsAllocation");
                parms[1] = new SqlParameter("@WHID", 100);
                parms[2] = new SqlParameter("@WHAreaID", 100);
                parms[3] = new SqlParameter("@WHStorageLocationID", 100);
                parms[4] = new SqlParameter("@WHGoodsAllocationID", i);
                int w = ys.Database.ExecuteSqlCommand("exec PM_WH_Delete  @TYPE,@WHID,@WHAreaID,@WHStorageLocationID,@WHGoodsAllocationID", parms);
                if (w > 0)
                {
                    return Content("true");
                }
            }
            return Content("false");
        }
        #endregion

        #region 仓库增加
        public ActionResult CkAdd()
        {

            return View();
        }
        public ActionResult ckwadd()
        {
            try
            {
                string ck = Request["ck"].ToString();
                PM_WH c1 = new PM_WH();
                c1.Name = ck;
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.PM_WH.Add(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }

        }
        public ActionResult kqwAdd()
        {
            try
            {
                string kq = Request["kq"].ToString();
                PM_WHArea c1 = new PM_WHArea();
                c1.WHArea = kq;
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.PM_WHArea.Add(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        public ActionResult KqAdd()
        {
            return View();
        }
        public ActionResult kwwAdd()
        {
            try
            {
                string kw = Request["kw"].ToString();
                PM_WHStorageLocation c1 = new PM_WHStorageLocation();
                c1.Name = kw;
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.PM_WHStorageLocation.Add(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        public ActionResult KwAdd()
        {
            return View();
        }
        public ActionResult hwwAdd()
        {
            try
            {
                string hw = Request["hw"].ToString();
                PM_WHGoodsAllocation c1 = new PM_WHGoodsAllocation();
                c1.Name = hw;
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.PM_WHGoodsAllocation.Add(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        public ActionResult HwAdd()
        {
            return View();
        }
        #endregion

        #region 初始大小类型
        public ActionResult MaterialRegistration()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<Category1> list = ys.Category1.ToList();
                ViewBag.c1 = list;
                List<Category2> listd = ys.Category2.ToList();
                ViewBag.c2 = listd;
            }
            return View();
        }
        #endregion

        #region 分部视图小类型
        public ActionResult ca1(string c1)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Da", c1);
                var list = ys.Database.SqlQuery<CheckDaTypeChangeXiao_Result>("exec CheckDaTypeChangeXiao  @Da", parms).ToList();
                ViewBag.c2 = list;


            }
            return PartialView("ca1");
        }
        #endregion

        #region 显示大小物料名称
        public ActionResult CateCHeck(string Da, string Xiao, int page, int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                List<PM_Material> list = ys.PM_Material.ToList();
                if (Da != "")
                {
                    int i = int.Parse(Da);
                    list = ys.PM_Material.Where(p => p.Category1ID==i).ToList();
                }
                if (Xiao != "")
                {
                    int x = int.Parse(Xiao);
                    list = list.Where(p => p.Category2ID==x).ToList();
                }
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<PM_Material> pageList = new PageList<PM_Material>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);

            }

        }
        #endregion

        #region 增加大小类别界面
        public ActionResult DaAdd()
        {

            return View();
        }
        public ActionResult XiaoAdd()
        {
            return View();
        }

        #endregion

        #region 添加大数据
        public ActionResult Dadd()
        {
            try
            {
                string da = Request["Da"].ToString();
                Category1 c1 = new Category1();
                c1.CategoryName = da;
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.Category1.Add(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        public ActionResult Xadd()
        {
            try
            {
                string X = Request["X"].ToString();
                Category2 c1 = new Category2();
                c1.CategoryName = X;
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.Category2.Add(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        #endregion

        #region 删除大小类型

        public ActionResult DelD(string delD)
        {
            try
            {
                YLMES_newEntities ys = new YLMES_newEntities();

                int i = int.Parse(delD);
                Category1 c1 = new Category1() { id = i };
                ys.Category1.Attach(c1);
                ys.Category1.Remove(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }
        public ActionResult DelD2(string delD)
        {
            try
            {
                int i = int.Parse(delD);
                Category2 c1 = new Category2() { id = i };
                YLMES_newEntities ys = new YLMES_newEntities();
                ys.Category2.Attach(c1);
                ys.Category2.Remove(c1);
                ys.SaveChanges();
                return Content("true");
            }
            catch (Exception ex)
            {
                return Content("false");
            }
        }

        #endregion

        #region 缓存的显示库区值
        public List<PM_WH> FindPM_WH(YLMES_newEntities ys)
        {
            var cache = CacheHelper.GetCache("PM_WH");//先读取
             
            if (cache == null)//如果没有该缓存
            {
                List<PM_WH> wh = ys.PM_WH.ToList();
                CacheHelper.SetCache("PM_WH", wh);//添加缓存
                return wh;
            }
                var result = (List<PM_WH>)cache;//有就直接返回该缓存
                return result;
                             
        }
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

        #region 显示物料修改信息

        public ActionResult PbcUpdate(string PartNumber,string PartSpec)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                MaterialRegistration();

                //展示大类型
                var list = ys.PM_Material.Where(p => p.PartNumber==PartNumber && p.PartSpec==PartSpec).FirstOrDefault();
                var c1 = ys.Category1.Where(p => p.id==list.Category1ID).FirstOrDefault();
                ViewData["cc"] = c1.id;


                //展示小类型
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "c2");
                parms[1] = new SqlParameter("@PartNumber", PartNumber);
                parms[2] = new SqlParameter("@PartSpec", PartSpec);
                var listd = ys.Database.SqlQuery<CheckdCategory_Result>("exec CheckdCategory  @Type,@PartNumber,@PartSpec", parms).FirstOrDefault();
                ViewData["ccc"] = listd.id;

                var can = ys.PM_Material.Where(p => p.PartNumber==PartNumber && p.PartSpec==PartSpec).FirstOrDefault();
                Session["pmId"] = can.ID;
                ViewData["figureNumber"] = can.figureNumber;
                ViewData["PartNumber"] = can.PartNumber;
                ViewData["PartSpec"] = can.PartSpec;
                ViewData["PartMaterial"] = can.PartMaterial;
                ViewData["ListType"] = can.ListType;
            }

            return View();
        }

        #endregion

        #region 修改物料

        public ActionResult EditMachine(string c1,string c2,string fn,string pn, string ps, string pm,string listType)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                   string pmid=Session["pmId"].ToString();
                    SqlParameter[] parms = new SqlParameter[9];
                    parms[0] = new SqlParameter("@type", "更新");
                    parms[1] = new SqlParameter("@id",pmid );
                    parms[2] = new SqlParameter("@Category1ID", c1);
                    parms[3] = new SqlParameter("@Category2ID", c2);
                    parms[4] = new SqlParameter("@figureNumber", fn);
                    parms[5] = new SqlParameter("@PartNumber", pn);
                    parms[6] = new SqlParameter("@PartSpec", ps);
                    parms[7] = new SqlParameter("@PartMaterial",pm);
                    parms[8] = new SqlParameter("@ListType", listType);
                    ys.Database.ExecuteSqlCommand("exec UpdatePmc  @type,@id,@Category1ID,@Category2ID,@figureNumber,@PartNumber,@PartSpec,@PartMaterial,@ListType", parms);
                }
                return Content("true");
            }
            catch(Exception ex)
            {
                return Content("false");
            }
                      
        }


        #endregion

        #region 删除物料

        public ActionResult DeleteMaterial(string ID)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    int i = int.Parse(ID);
                    var Material = (from m in ys.PM_Material
                                    where m.ID == i
                                    select m).Single();
                    ys.PM_Material.Remove(Material);
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

        #region 大小类型加模糊查询

        public JsonResult TypeLikeSearch(string Da,string Xiao,string pn,int page,int limit)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[13];
                parms[0] = new SqlParameter("@Type", "查找物料");
                parms[1] = new SqlParameter("@PONO", "");
                parms[2] = new SqlParameter("@QTY", "");
                parms[3] = new SqlParameter("@MaterialID", "");
                parms[4] = new SqlParameter("@CreatedBy", "");
                parms[5] = new SqlParameter("@Status", "");
                parms[6] = new SqlParameter("@Desc", "");
                parms[7] = new SqlParameter("@Location", "");
                parms[8] = new SqlParameter("@ProjectName", "");
                parms[9] = new SqlParameter("@MaterialType", "");
                parms[10] = new SqlParameter("@PartNumber", pn);
                parms[11] = new SqlParameter("@Category1ID", Da);
                parms[12] = new SqlParameter("@Category2ID", Xiao);
                var list = ys.Database.SqlQuery<SP_PM_Material_Result>("exec SP_PM_Material  @Type,@PONO,@QTY,@MaterialID,@CreatedBy,@Status,@Desc,@Location,@ProjectName,@MaterialType,@PartNumber,@Category1ID,@Category2ID", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                PageList<SP_PM_Material_Result> pageList = new PageList<SP_PM_Material_Result>(list, page, limit);
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);

            }
        }

        #endregion

        #region 判断是否有物料

        public ActionResult config(string ifhw)
        {
            try
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var text = ys.PM_MaterialList_new.Where(p => p.Location == ifhw).FirstOrDefault();
                    string txt = text.ToString();
                    if (!string.IsNullOrEmpty(txt))
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
                return Content("error");
            }
        }

        #endregion

        #region 原材料查询
        public ActionResult MaterialsStock()
        {
            return View();
        }
        public JsonResult GetmaterialsStock(string ProjectName, string PartNumber, string PartSpec, string CreatedTimeEnd, string CreatedTime, string CreatedBy)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[6];
                parms[0] = new SqlParameter("@ProjectName", ProjectName);
                parms[1] = new SqlParameter("@PartNumber", PartNumber);
                parms[2] = new SqlParameter("@PartSpec", PartSpec);
                parms[3] = new SqlParameter("@CreatedTimeEnd", CreatedTimeEnd);
                parms[4] = new SqlParameter("@CreatedTime", CreatedTime);
                parms[5] = new SqlParameter("@CreatedBy", CreatedBy);
                var list = ys.Database.SqlQuery<Raw_MaterialStock_Result>("exec Raw_MaterialStock @ProjectName,@PartNumber,@PartSpec,@CreatedTimeEnd,@CreatedTime,@CreatedBy", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("data", list);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //模糊查询
        public JsonResult VagueCustomerName(string CustomerName)
        {
            if (CustomerName != "")
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var ss1 = ys.C_Contract.Where(p => p.CustomerName.Contains(CustomerName)).ToList();
                    //var list = ys.Database.SqlQuery<PM_WorkOrder>(" SELECT *  FROM PM_WorkOrder ").ToList();
                    return Json(ss1, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        public JsonResult VagueMaterialName(string MaterialName)
        {
            if (MaterialName != "")
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var ss1 = ys.PM_Material.Where(p => p.PartNumber.Contains(MaterialName)).ToList();
                    //var list = ys.Database.SqlQuery<PM_WorkOrder>(" SELECT *  FROM PM_WorkOrder ").ToList();
                    return Json(ss1, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        public JsonResult VagueUserName(string UserName)
        {
            if (UserName != "")
            {
                using (YLMES_newEntities ys = new YLMES_newEntities())
                {
                    var ss1 = ys.PM_MaterialList.Where(p => p.CreatedBy.Contains(UserName)).ToList();
                    //var list = ys.Database.SqlQuery<PM_WorkOrder>(" SELECT *  FROM PM_WorkOrder ").ToList();
                    return Json(ss1, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        #endregion
    }
}