using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YLMES.Models;

namespace YLMES.Controllers
{
    public class DiscussController : Controller
    {
        // GET: Discuss
        //问题讨论
        public ActionResult Index()
        {
            return View();
        }
        //显示帖子
        public JsonResult Article(string Columns)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "帖子分类");
                parms[1] = new SqlParameter("@Columns", Columns);
                var list = ys.Database.SqlQuery<Forum_Postings_Result>("exec Forum_Postings @Type,'',@Columns", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //按照未结或已结的帖子查询
        public JsonResult Unfinished(string Status)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                if (Status.Equals("未结"))
                {
                    var lisst = ys.PM_Forum.Where(p => p.Status.Contains("未结")).ToList();
                    return Json(lisst, JsonRequestBehavior.AllowGet);
                }
                else if (Status.Equals("已结"))
                {
                    var list = ys.PM_Forum.Where(p => p.Status.Contains("已结")).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        //最新的帖子
        public JsonResult Newest()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Type", "最新帖子");
                var list = ys.Database.SqlQuery<Forum_Newest_Result>("exec Forum_Newest @Type", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //最热的话题
        public JsonResult Hottest()
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Type", "最热");
                var list = ys.Database.SqlQuery<Forum_Newest_Result>("exec Forum_Newest @Type", parms).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //详情
        public ActionResult Detail()
        {
            return View();
        }
        //显示帖子详情
        public JsonResult Detaillist(int id)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                // var list=ys.PM_Forum.Where(p => p.id==id).ToList();
                SqlParameter[] pa = new SqlParameter[2];
                pa[0] = new SqlParameter("@Type", "详情");
                pa[1] = new SqlParameter("@id", id);
                var list = ys.Database.SqlQuery<Forum_Newest_Result>("exec Forum_Newest @Type,@id", pa).ToList();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", "评论列表");
                parms[1] = new SqlParameter("@id", id);
                var lisst = ys.Database.SqlQuery<Forum_Commentlist_Result>("exec Forum_Commentlist @Type,@id", parms).ToList();
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                hasmap.Add("list", list);
                hasmap.Add("data", lisst);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }
        }
        //编辑帖子
        public string Detailedit(string id, string Substance, string columnist, string title, string Picture)
        {
            string name = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[7];
                parms[0] = new SqlParameter("@Type", "更改帖子");
                parms[1] = new SqlParameter("@id", id);
                parms[2] = new SqlParameter("@Columns", columnist);
                parms[3] = new SqlParameter("@Title", title);
                parms[4] = new SqlParameter("@Substance", Substance);
                parms[5] = new SqlParameter("@Picture", Picture);
                parms[5] = new SqlParameter("@Employee", name);
                int i = ys.Database.ExecuteSqlCommand("exec Forum_Postings @Type,@id,@Columns,@Title,@Substance,@Picture,@Employee", parms);
                if (i <= 0)
                {
                    return "false";
                }
                else if (i > 0)
                {
                    return "true";
                }
            }
            return null;
        }
        //删除评论
        public string Commentdel(string id, string CommentsID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "删除评论");
                parms[1] = new SqlParameter("@id", id);
                parms[2] = new SqlParameter("@CommentsID", CommentsID);
                int i = ys.Database.ExecuteSqlCommand("exec Forum_Commentlist @Type,@id,@CommentsID", parms);
                if (i <= 0)
                {
                    return "false";
                }
                else if (i > 0)
                {
                    return "true";
                }
            }
            return null;
        }
        //评论
        [ValidateInput(false)]
        public string Commentadd(string id, string Comments, string CommentsPicture)
        {
            string name = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[5];
                parms[0] = new SqlParameter("@Type", "评论");
                parms[1] = new SqlParameter("@id", id);
                parms[2] = new SqlParameter("@Comments", Comments);
                parms[3] = new SqlParameter("@CommentsPicture", CommentsPicture);
                parms[4] = new SqlParameter("@Employee", name);
                int i = ys.Database.ExecuteSqlCommand("exec Forum_Commentlist @Type,@id,'',@Comments,@CommentsPicture,@Employee", parms);
                if (i <= 0)
                {
                    return "false";
                }
                else if (i > 0)
                {
                    return "true";
                }
            }
            return null;
        }
        //采纳
        public string Adopt(string id, string CommentsID)
        {
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "采纳");
                parms[1] = new SqlParameter("@id", id);
                parms[2] = new SqlParameter("@CommentsID", CommentsID);
                int i = ys.Database.ExecuteSqlCommand("exec Forum_Commentlist @Type,@id,@CommentsID", parms);
                if (i <= 0)
                {
                    return "false";
                }
                else if (i > 0)
                {
                    return "true";
                }
            }
            return null;
        }
        //删除帖子
        public string Detaildel(string id)
        {
            string name = Session["name"].ToString();
            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Type", "删除帖子");
                parms[1] = new SqlParameter("@id", id);
                parms[2] = new SqlParameter("@Employee", name);
                int i = ys.Database.ExecuteSqlCommand("exec Forum_Postings @Type,@id,'','','','',@Employee", parms);
                if (i <= 0)
                {
                    return "false";
                }
                else if (i > 0)
                {
                    return "true";
                }
            }
            return null;
        }
        //用户
        public ActionResult UserIndex()
        {
            return View();
        }
        //帖子详细
        public ActionResult UserHome()
        {
            return View();
        }
        //发帖
        public ActionResult Posting()
        {
            return View();
        }
        //发帖子
        [ValidateInput(false)]
        public string Postingup(Dictionary<string,string> data,string value)
        {
            StringBuilder builder = new StringBuilder("exec Forum_Postings @Type='新帖子',@Employee="+ Session["name"].ToString()+",");

            using (YLMES_newEntities ys = new YLMES_newEntities())
            {
                int j = 0;
                SqlParameter[] parms = new SqlParameter[data.Count()-1];
                foreach (var da in data)
                {
                    if (da.Key == "Substance")
                    {
                        parms[j] = new SqlParameter("@" + da.Key, value);
                    }
                    else {
                        if (da.Key != "file") {
                            parms[j] = new SqlParameter("@" + da.Key, da.Value);
                            
                             
                        }
                    }
                    if (da.Key != "file")
                    {
                        if (j + 1 == (data.Count() - 1))
                        {
                            builder.Append("@" + da.Key + "=@" + da.Key);
                        }
                        else
                        {
                            builder.Append("@" + da.Key + "=@" + da.Key + ",");
                        }
                        j++;

                    }


                }
                ys.Database.ExecuteSqlCommand(builder.ToString(), parms);

            }
            return "true";

        }
        //显示我的帖子
        public JsonResult CheckMyPost(string Substance,int page,int limit)
        {           
                using(YLMES_newEntities ys =new YLMES_newEntities())
                {
                if (Substance == null)
                {
                    Substance = "";
                }
                string name = Session["name"].ToString();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@UserName", name);
                parms[1] = new SqlParameter("@Substance", Substance);
                var list = ys.Database.SqlQuery<MyPost_Result>("exec MyPost @UserName,@Substance", parms).ToList();
                PageList<MyPost_Result> pageList = new PageList<MyPost_Result>(list, page, limit);
                Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
                int count = list.Count();
                hasmap.Add("code", 0);
                hasmap.Add("msg", "");
                hasmap.Add("count", count);
                hasmap.Add("data", pageList);
                return Json(hasmap, JsonRequestBehavior.AllowGet);
            }        
        }
        //修改密码页面
        public ActionResult ChangPwd()
        {
            return View();
        }
       //修改密码
       public ActionResult Changepwd(string pwd,string names)
        {
            try
            {
                using(YLMES_newEntities ys = new YLMES_newEntities())
                {
                    string name = Session["name"].ToString();
                    SqlParameter[] parms = new SqlParameter[3];
                    parms[0] = new SqlParameter("@UserName", name);
                    parms[1] = new SqlParameter("@NewName", names);
                    parms[2] = new SqlParameter("@PassWord", pwd);                 
                    ys.Database.ExecuteSqlCommand("exec ChangUserPassword @UserName,@NewName,@PassWord", parms);
                }
                return Content("true");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("false");
            }           
        }
        //上传图片
        public ActionResult LayUploadFile(HttpPostedFileBase File)
        {
            var fileName1 = Path.Combine(Request.MapPath("~/QRCodeImage"), Path.GetFileName(File.FileName));
             Dictionary<string, Object> hasmap = new Dictionary<string, Object>();
             Dictionary<string, Object> hasmap2 = new Dictionary<string, Object>();
            File.SaveAs(fileName1);
            string fname = fileName1.Substring(fileName1.LastIndexOf('\\') + 1);
            hasmap.Add("code", 0);
            hasmap.Add("data", hasmap2);
            hasmap2.Add("src", "/QRCodeImage/" + fname);
            return Json(hasmap, JsonRequestBehavior.AllowGet);
        }
    }
}