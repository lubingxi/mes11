using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YlMES.Controllers
{
    public class ContractTwoController : Controller
    {
        string connString = "Data Source=192.168.1.251;Initial Catalog=KQXT;User ID=admin;Password=admin123";


        // GET: ContractTwo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginContract()
        {
            return View();
        }

        #region 验证登陆
        public ActionResult VaLogin()
        {        
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                string uName = Request.Form["username"].ToString();
                string uPwd = Request.Form["password"].ToString();
                string sqlStr = "select top 1 * from  [Employee]   where [username]='" + uName + "' and [PWD] ='" + uPwd + "' ";
                SqlCommand Mycommand = new SqlCommand(sqlStr, conn);
                SqlDataReader dr = Mycommand.ExecuteReader();
                if (dr.Read())
                {
                    Session["name"] = uName;

                }
                while (dr.Read())
                {
                    if (dr[0].ToString() == "")
                    {
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return View("LoginContract");
        }
        #endregion

    }
}