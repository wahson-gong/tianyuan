using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Net;

public partial class inc_paypal_AutoReceive : System.Web.UI.Page
{
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    protected void Page_Load(object sender, EventArgs e)
    {
        string tablename = "";
        try
        {
            tablename = Request.QueryString["tablename"].ToString();
        }
        catch { }
        //成功后返回的页面
        string strFormValues;
        string strResponse;
        string authToken;
        string txToken;
        string query;
        //定义您的身份标记,这里改成您的身份标记
        authToken = my_b.get_pay("PayPal", "zhifukeyma");
        //获取PayPal 交易流水号
        txToken = Request["item_number"];
        // Set the 'Method' property of the 'Webrequest' to 'POST'.
        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");
        //HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.paypal.com/cgi-bin/webscr");
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //设置请求参数
        query = "cmd=_notify-synch&tx=" + txToken + "&at=" + authToken;
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] byte1 = encoding.GetBytes(query);
        strFormValues = Encoding.ASCII.GetString(byte1);
        myHttpWebRequest.ContentLength = strFormValues.Length;
        //发送请求
        StreamWriter stOut = new StreamWriter(myHttpWebRequest.GetRequestStream(), System.Text.Encoding.ASCII);
        stOut.Write(strFormValues);
        stOut.Close();
        //接受返回信息
        StreamReader stIn = new StreamReader(myHttpWebRequest.GetResponse().GetResponseStream());
        strResponse = stIn.ReadToEnd();
        stIn.Close();
        //取前面七个字符

  
       
        //显示返回的字符串，
        Response.Write(strResponse);
        //此处需要判断网站订单是否已经处理

        if (my_c.GetTable("select id from sl_system where u1='" + my_b.k_cookie("user_name").ToString() + "' and u2 like '" + txToken + "' and u4='会员订单'").Rows.Count == 0)
        {

            my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + txToken + "'");

            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name").ToString() + "','" + txToken + "','" + Request.UserHostAddress.ToString() + "','会员订单')");

            Response.Redirect("/err.aspx?err=支付成功！&errurl=" + my_b.tihuan("/single.aspx?m=user-myorder", "&", "fzw123") + "");
        }
        else
        {
            Response.Redirect("/err.aspx?err=已经充值！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
        }
        //end
       
    }
}