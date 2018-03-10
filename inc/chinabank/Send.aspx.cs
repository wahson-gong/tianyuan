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

public partial class Send : System.Web.UI.Page
{
    //必要的交易信息
    protected string v_amount;       // 订单金额
    protected string v_moneytype;    // 币种
    protected string v_md5info;      // 对拼凑串MD5私钥加密后的值
    protected string v_mid;		 // 商户号
    protected string v_url;		 // 返回页地址
    protected string v_oid;		 // 推荐订单号构成格式为 年月日-商户号-小时分钟秒

    //收货信息
    protected string v_rcvname;      // 收货人
    protected string v_rcvaddr;      // 收货地址
    protected string v_rcvtel;       // 收货人电话
    protected string v_rcvpost;      // 收货人邮编
    protected string v_rcvemail;     // 收货人邮件
    protected string v_rcvmobile;    // 收货人手机号

    //订货人信息
    protected string v_ordername;    // 订货人姓名
    protected string v_orderaddr;    // 订货人地址
    protected string v_ordertel;     // 订货人电话
    protected string v_orderpost;    // 订货人邮编
    protected string v_orderemail;   // 订货人邮件
    protected string v_ordermobile;  // 订货人手机号

    //两个备注
    protected string remark1;
    protected string remark2;


    //引用
    my_conn my_c = new my_conn();
    my_basic my_b=new my_basic();
    //
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
            v_mid = my_b.get_pay("网银在线", "zhanghao");				 // 商户号，这里为测试商户号20000400，替换为自己的商户号即可
            v_url = my_b.get_Domain() + "pay/chinabank/Receive.aspx?tablename=" + tablename + ""; // 商户自定义返回接收支付结果的页面
            //remark2 = "http://" + Request.ServerVariables["SERVER_NAME"] + "/chinabank/AutoReceive.aspx";//自动对帐

            
            // MD5密钥要跟订单提交页相同，如Send.asp里的 key = "test" ,修改""号内 test 为您的密钥
            string key = my_b.get_pay("网银在线", "zhifukeyma");				 // 如果您还没有设置MD5密钥请登陆我们为您提供商户后台，地址：https://merchant3.chinabank.com.cn/
            // 登陆后在上面的导航栏里可能找到“资料管理”，在资料管理的二级导航栏里有“MD5密钥设置”
            // 建议您设置一个16位以上的密钥或更高，密钥最多64位，但设置16位已经足够了
            v_oid = Request["v_oid"];

            if (v_oid == null || v_oid.Equals(""))
            {
                DateTime dt = DateTime.Now;
                string v_ymd = dt.ToString("yyyyMMdd"); // yyyyMMdd
                string timeStr = dt.ToString("HHmmss"); // HHmmss
                v_oid = v_ymd + v_mid + timeStr;
            }
            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name").ToString() + "','" + v_oid + "|" + Request["v_amount"] + "','" + Request.UserHostAddress.ToString() + "','会员订单')");

            v_amount = Request["v_amount"];

            v_moneytype = "CNY";

            string text = v_amount + v_moneytype + v_oid + v_mid + v_url + key; // 拼凑加密串

            v_md5info = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text, "md5").ToUpper();

            //收货信息
            v_rcvname = Request["v_rcvname"];
            v_rcvaddr = Request["v_rcvaddr"];
            v_rcvtel = Request["v_rcvtel"];
            v_rcvpost = Request["v_rcvpost"];
            v_rcvemail = Request["v_rcvemail"];
            v_rcvmobile = Request["v_rcvmobile"];

            //订货人信息
            v_ordername = Request["v_ordername"];
            v_orderaddr = Request["v_orderaddr"];
            v_ordertel = Request["v_ordertel"];
            v_orderpost = Request["v_orderpost"];
            v_orderemail = Request["v_orderemail"];
            v_ordermobile = Request["v_ordermobile"];

            remark1 = Request["remark1"];
            remark2 = Request["remark2"];
        }
    }
}
