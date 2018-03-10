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

public partial class AutoReceive : System.Web.UI.Page
{
    protected string v_oid; //订单号
    protected string v_pstatus; //支付状态码
    //20（支付成功，对使用实时银行卡进行扣款的订单）；
    //30（支付失败，对使用实时银行卡进行扣款的订单）；

    protected string v_pstring; //支付状态描述
    protected string v_pmode; //支付银行
    protected string v_md5str; //MD5校验码
    protected string v_amount; //支付金额
    protected string v_moneytype; //币种		
    protected string remark1;//备注1
    protected string remark2;//备注1

    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        string tablename = "";
        try
        {
            tablename = Request.QueryString["tablename"].ToString();
        }
        catch { }
        			// MD5密钥要跟订单提交页相同，如Send.asp里的 key = "test" ,修改""号内 test 为您的密钥
        string key = my_b.get_pay("网银在线", "zhifukeyma");	// 如果您还没有设置MD5密钥请登陆我们为您提供商户后台，地址：https://merchant3.chinabank.com.cn/
						// 登陆后在上面的导航栏里可能找到“资料管理”，在资料管理的二级导航栏里有“MD5密钥设置”
						// 建议您设置一个16位以上的密钥或更高，密钥最多64位，但设置16位已经足够了			
			
			v_oid     = Request["v_oid"];
			v_pstatus = Request["v_pstatus"];
			v_pstring = Request["v_pstring"];
			v_pmode   = Request["v_pmode"];
			v_md5str  = Request["v_md5str"];
			v_amount  = Request["v_amount"];
			v_moneytype = Request["v_moneytype"];
			remark1 = Request["remark1"];
			remark2 = Request["remark2"];

			string str = v_oid+v_pstatus+v_amount+v_moneytype+key;
            str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToUpper();
            //v_oid = "2014031022826103160417";
            //v_amount = "10";
            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('fzw','" + v_oid + "|" + v_md5str + "|" + v_pstatus + "|" + v_amount + "','" + Request.UserHostAddress.ToString() + "','支付返回')");
            if(str==v_md5str)
            {
				Response.Write("ok"); //通知网银服务器验证通过，停止发送

                if(v_pstatus.Equals("20")) 
                {

                    //支付成功
                    //在这里商户可以写上自己的业务逻辑
                    if (my_c.GetTable("select id from sl_system where u1='" + my_b.k_cookie("user_name").ToString() + "' and u2 like '" + v_oid + "' and u4='会员订单'").Rows.Count == 0)
                    {
                        my_c.genxin("update sl_" + tablename + " set zhuangtai='已付款' where dingdanhao='" + v_oid + "'");

                        my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + my_b.k_cookie("user_name").ToString() + "','" + v_oid + "','" + Request.UserHostAddress.ToString() + "','会员订单')");

                        string leixingbiaoti = "";
                        if (tablename == "hotel_order")
                        {
                            leixingbiaoti = "酒店付款成功";
                        }
                        else if (tablename == "zuche_order")
                        {
                            leixingbiaoti = "租车付款成功";
                        }
                        else if (tablename == "qianzheng_order")
                        {
                            leixingbiaoti = "签证付款成功";
                        }
                        else
                        {
                            leixingbiaoti = "线路付款成功";
                        }
                        my_b.post_duan(v_oid, "sl_" + tablename, leixingbiaoti);

                        Response.Redirect("/err.aspx?err=支付成功！&errurl=" + my_b.tihuan("/single.aspx?m=orderor&dingdanhao=" + v_oid + "&tablename=" + tablename + "", "&", "fzw123") + "");
                    }
                    else
                    {
                        Response.Redirect("/err.aspx?err=已经充值！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                    }
                    //end

                }
            }
            else
            {
                Response.Write("error");  //验证失败，请求重发
            }
    }
}
