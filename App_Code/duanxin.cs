using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using System.Web.SessionState;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Response;
using Top.Api.Request;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
/// <summary>
///my_basic 的摘要说明
/// </summary>
public class duanxin
{
    my_basic my_b = new my_basic();
    #region 阿里大鱼
    public string alidayu(string UserName, string Password, string DestMobile, string ContentStr1, string SignName, string duanxinmobanid)
    {
        string url = "http://gw.api.taobao.com/router/rest";
        string appkey = UserName;
        string secret = Password;
        ITopClient client = new DefaultTopClient(url, appkey, secret);
        AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
        req.Extend = "";
        req.SmsType = "normal";
        req.SmsFreeSignName = SignName;
        req.SmsParam = ContentStr1;
        req.RecNum = DestMobile;
        req.SmsTemplateCode = duanxinmobanid;
        AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
        return rsp.ToString();
    }
    #endregion
    #region 阿里云
    public string aliyun(string UserName, string Password, string DestMobile, string ContentStr1, string SignName, string duanxinmobanid)
    {
        //IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "" + UserName + "", "" + Password + "");
        //IAcsClient client = new DefaultAcsClient(profile);
        //SingleSendSmsRequest request = new SingleSendSmsRequest();
        //request.SignName = SignName;
        //request.TemplateCode = duanxinmobanid;
        //request.RecNum = DestMobile;
        //request.ParamString = ContentStr1;

        ////HttpContext.Current.Response.Write(string.Format("SignName:{0},TemplateCode:{1},RecNum:{2},ParamString:{3}", SignName, duanxinmobanid, DestMobile, ContentStr1));
        ////HttpContext.Current.Response.End();

        //SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
        //return httpResponse.ToString();


        //产品名称:云通信短信API产品,开发者无需替换
        const String product = "Dysmsapi";
        //产品域名,开发者无需替换
        const String domain = "dysmsapi.aliyuncs.com";
        IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", UserName, Password);
        DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
        IAcsClient acsClient = new DefaultAcsClient(profile);
        SendSmsRequest request = new SendSmsRequest();
        SendSmsResponse response = null;
        try
        {

            //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
            request.PhoneNumbers = DestMobile;
            //必填:短信签名-可在短信控制台中找到
            request.SignName = SignName;
            //必填:短信模板-可在短信控制台中找到
            request.TemplateCode = duanxinmobanid;
            request.TemplateParam = "{\"code\":\"" + ContentStr1 + "\"}";
            
            //请求失败这里会抛ClientException异常
            response = acsClient.GetAcsResponse(request);

        }
        catch (ServerException e)
        {
            HttpContext.Current.Response.Write(e.ErrorCode);
        }
        catch (ClientException e)
        {
            HttpContext.Current.Response.Write(e.ErrorCode);
        }
        return response.ToString(); 


    }
    #endregion
    #region 凌凯
    public string mb345(string UserName, string Password, string DestMobile, string ContentStr1, string SignName, string duanxinmobanid)
    {
        ContentStr1 = System.Web.HttpUtility.UrlEncode(ContentStr1 + "【" + SignName + "】", System.Text.Encoding.GetEncoding("gb2312"));
        //string url = "https://sdk2.028lk.com/sdk2/BatchSend.aspx?CorpID=" + UserName + "&Pwd=" + Password + "&Mobile=" + DestMobile + "&Content=" + ContentStr1 + "";
        string url = "http://mb345.com:999/ws/BatchSend.aspx?CorpID=" + UserName + "&Pwd=" + Password + "&Mobile=" + DestMobile + "&Content=" + ContentStr1 + "";
        HttpWebRequest Rst = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse Rsp = (HttpWebResponse)Rst.GetResponse();
        StreamReader reader = new StreamReader(Rsp.GetResponseStream(), Encoding.GetEncoding("gb2312"));
        string ct = reader.ReadToEnd();
        return ct;

    }
    #endregion
    #region 创瑞
    public string supermore(string UserName, string Password, string DestMobile, string ContentStr1, string SignName, string duanxinmobanid)
    {
        //（逗号隔开多个号码） DestMobile = "18180229615"
        //Content 内容
        string qianming = "【" + SignName + "】";

        string AppendNum = "";//扩展子号
        string SendTime = "";//发送时间

        ContentStr1 = SignName + ContentStr1 + "回复“T”退订短信";//内容
        string MsgType = "1";//消息类型
        string Priority = "1";//优先级别

        string duanxin_url = "http://isms.supermore.com.cn/mobile.asmx/SendMsg?UserName=" + UserName + "&Password=" + Password + "&AppendNum=" + AppendNum + "&SendTime=" + SendTime + "&DestMobile=" + DestMobile + "&Content=" + ContentStr1 + "&MsgType=" + MsgType + "&Priority=" + Priority + "";

        string fanhui = my_b.getWebFile(duanxin_url);
        return fanhui;
    }
    #endregion

}
