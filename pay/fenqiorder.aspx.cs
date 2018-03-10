using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Com.Alipay;

public partial class fenqiorder : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_html my_h = new my_html();
    string type = "";
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {//用户状态
            my_b.user_sta("user_name");
            //end
            string type = "";
            try
            {
                type = Request["type"].ToString();
            }
            catch { }
            
           
            if (type == "pay")
            {
                string tablename = my_b.c_string(Request["tablename"].ToString());
                string zhifufangshi = my_b.c_string(Request["zhifufangshi"].ToString());
                my_c.genxin("update sl_" + tablename + " set zhifufangshi='" + zhifufangshi + "'where dingdanhao='" + my_b.c_string(Request["dingdanhao"].ToString()) + "' and yonghuming='" + my_b.k_cookie("user_name") + "' ");
                DataTable dt = my_c.GetTable("select * from sl_" + tablename + " where dingdanhao ='" + Request["dingdanhao"].ToString() + "'  and yonghuming='" + my_b.k_cookie("user_name") + "' and zhuangtai<>'已还'");
                if (dt.Rows.Count == 0)
                {
                    Response.Redirect("/err.aspx?err=订单支付成功！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                }
                string dingdanhao = dt.Rows[0]["dingdanhao"].ToString();
                string dingdanname = dt.Rows[0]["yonghuming"].ToString() +"还"+ dt.Rows[0]["qishu"].ToString()+"款";
                string dingdanjiage = dt.Rows[0]["jine"].ToString();
               
          
                if (zhifufangshi == "支付宝")
                {
                    ////////////////////////////////////////////请求参数////////////////////////////////////////////

                    //支付类型
                    string payment_type = "1";
                    //必填，不能修改
                    //服务器异步通知页面路径
                    string notify_url = my_b.get_Domain() + "inc/alipay/notify_url.aspx?tablename=" + tablename + "";
                    //需http://格式的完整路径，不能加?id=123这类自定义参数

                    //页面跳转同步通知页面路径
                    string return_url = my_b.get_Domain() + "inc/alipay/return_url.aspx?tablename=" + tablename + "";
                    //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

                    //卖家支付宝帐户
                    string seller_email = my_b.get_pay("支付宝", "zhanghao");
                    //必填

                    //商户订单号
                    string out_trade_no = dingdanhao;
                    //商户网站订单系统中唯一订单号，必填

                    //订单名称
                    string subject = dingdanname;
                    //必填

                    //付款金额
                    string total_fee = dingdanjiage;
                    //必填
                    //Response.Write(dingdanjiage + "||" + dingdanname + "||" + dingdanhao + "||" + seller_email + "||" + return_url + "||" + notify_url);
                    //Response.End();
                    //订单描述

                    string body = "";
                    //商品展示地址
                    string show_url = "";
                    //需以http://开头的完整路径，例如：http://www.xxx.com/myorder.html

                    //防钓鱼时间戳
                    string anti_phishing_key = "";
                    //若要使用请调用类文件submit中的query_timestamp函数

                    //客户端的IP地址
                    string exter_invoke_ip = "";
                    //非局域网的外网IP地址，如：221.0.0.1


                    ////////////////////////////////////////////////////////////////////////////////////////////////

                    //把请求参数打包成数组
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", alipayConfig.Partner);
                    sParaTemp.Add("_input_charset", alipayConfig.Input_charset.ToLower());
                    sParaTemp.Add("service", "create_direct_pay_by_user");
                    sParaTemp.Add("payment_type", payment_type);
                    sParaTemp.Add("notify_url", notify_url);
                    sParaTemp.Add("return_url", return_url);
                    sParaTemp.Add("seller_email", seller_email);
                    sParaTemp.Add("out_trade_no", out_trade_no);
                    sParaTemp.Add("subject", subject);
                    sParaTemp.Add("total_fee", total_fee);
                    sParaTemp.Add("body", body);
                    sParaTemp.Add("show_url", show_url);
                    sParaTemp.Add("anti_phishing_key", anti_phishing_key);
                    sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

                    //建立请求
                    string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                    Response.Write(sHtmlText);
                }
                else if (zhifufangshi == "京东支付")
                {
                    Response.Redirect("/inc/chinabank/Send.aspx?v_oid=" + dingdanhao + "&tablename=" + tablename + "&v_amount=" + dingdanjiage + "");
                }
                else if (zhifufangshi == "快钱")
                {
                    float kuaiqian = float.Parse(dingdanjiage) * 100;
                    Response.Redirect("/inc/kuaiqian/send.aspx?v_oid=" + dingdanhao + "&tablename=" + tablename + "&v_amount=" + kuaiqian + "");
                }
                else if (zhifufangshi == "微信支付")
                {
                    float weixinzhifu = float.Parse(dingdanjiage) * 100;
                    if (my_b.set_fangwen() == 0)
                    {
                        Response.Redirect("/inc/weixinpay/demo/native_dynamic_qrcode.php?dingdanhao=" + dingdanhao + "&bodystr=" + dingdanhao + "&tablename=" + tablename + "&jine=" + weixinzhifu + "");
                    }
                    else
                    {
                        Response.Redirect("/inc/weixinpay/demo/js_api_call.php?dingdanhao=" + dingdanhao + "&tablename=" + tablename + "&bodystr=网上购物（" + dingdanhao + "）&jine=" + weixinzhifu + "");
                    }


                }
                else if (zhifufangshi == "货到付款")
                {

                    Response.Redirect("/");
                }
            }





        }
    }
}