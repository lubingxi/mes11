using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using YLMES.Models;
using System.Data.SqlClient;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
namespace YLMES
{
    public  class Sms
    {
        public static int InsertSmsInfos(string Dept,string Number,string CreateBy)
        {
            using(YLMES_newEntities ys = new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[3];
                parms[0] = new SqlParameter("@Dept", Dept);
                parms[1] = new SqlParameter("@Number", Number);
                parms[2] = new SqlParameter("@CreatedBy", CreateBy);        
                ys.Database.ExecuteSqlCommand("exec InsertSmsInfos  @Dept,@Number,@CreatedBy", parms);
                return 1;
            }         
        }
        public static int CheckSms(string Dept, string number)
        {
            using(YLMES_newEntities ys =new YLMES_newEntities())
            {
                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@Dept", Dept);
                var list = ys.Database.SqlQuery<SendSms_Result>("exec SendSms @Dept", parms).FirstOrDefault();
                int i = SendSms(list.UserName, number, list.Tel);
                if (i > 0)
                {
                    SqlParameter[] parmd = new SqlParameter[1];
                    parmd[0] = new SqlParameter("@Dept", Dept);
                    ys.Database.ExecuteSqlCommand("exec UpdateSmsInfo @Dept");
                    return 1;
                }
                else
                {
                    return 0;
                }
            }            
        }
        public static int SendSms(string name,string number,string phone)
        {
            string product = "Dysmsapi";//短信API产品名称
            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            string accessId = "LTAI4ZbZoliOWDL1";
            string accessSecret = "0zz6ytZvbm4LHG2xMPHB9wUM7pzTS7";
            string regionIdForPop = "cn-hangzhou";
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {              
                //request.SignName = "上云预发测试";//"管理控制台中配置的短信签名（状态必须是验证通过）"
                //request.TemplateCode = "SMS_71130001";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
                //request.RecNum = "13567939485";//"接收号码，多个号码可以逗号分隔"
                //request.ParamString = "{\"name\":\"123\"}";//短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
                //SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
                request.PhoneNumbers = phone;
                request.SignName = "友力智能";
                request.TemplateCode = "SMS_159771433";
                request.TemplateParam = "{\"name\":\"" + name + "\",\"number\":\"" + number + "\"}";
               // request.OutId = "";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                System.Console.WriteLine(sendSmsResponse.Message);

            }
            catch (ServerException e)
            {
                return 0;
            }
            catch (ClientException e)
            {
                return 0;
            }          
                return 1;            
        }
    }
}