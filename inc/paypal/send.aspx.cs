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

public partial class inc_paypal_send : System.Web.UI.Page
{ 
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    //
    public string business = ""; //PayPal卖家帐号
    public string item_name = ""; //商品名称
    public string item_number = "";//商品编号
    public string notify_url = "";//手动付款后返回地址
    public string return_url = "";//自动付款后返回地址
    public string amount = "";//金额
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string tablename = "";
            try
            {
                tablename = Request.QueryString["tablename"].ToString();
            }
            catch { }
            business = my_b.get_pay("PayPal", "zhanghao");				 // 商户号，这里为测试商户号20000400，替换为自己的商户号即可
            notify_url = my_b.get_Domain() + "inc/paypal/Receive.aspx?tablename=" + tablename + ""; // 商户自定义返回接收支付结果的页面
            return_url = my_b.get_Domain() + "inc/paypal/AutoReceive.aspx?tablename=" + tablename + "";//自动对帐


            // MD5密钥要跟订单提交页相同，如Send.asp里的 key = "test" ,修改""号内 test 为您的密钥
            string Token = my_b.get_pay("PayPal", "zhifukeyma");				 // 
            string dingdanhao = Request["dingdanhao"];
            DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where dingdanhao='" + dingdanhao + "'");
            if (dt.Rows.Count > 0)
            {
                item_name = dt.Rows[0]["yonghuming"].ToString() + "的订单《" + dt.Rows[0]["dingdanhao"].ToString() + "》";
                item_number = dt.Rows[0]["dingdanhao"].ToString();
                amount = dt.Rows[0]["jine"].ToString();
            }
            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name").ToString() + "','" + item_number + "|" + amount + "','" + Request.UserHostAddress.ToString() + "','会员订单')");

        }
    }
}