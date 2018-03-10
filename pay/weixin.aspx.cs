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
using System.Text.RegularExpressions;
public partial class Single : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_order my_o = new my_order();
    public string get_xml_jiedian(string type,string neirong)
    {
        string t1 = "";
        Regex reg = new Regex("<"+type+ ">?.*?</" + type + ">", RegexOptions.Singleline);
        Match matches = reg.Match(neirong);
        t1 = matches.ToString();
        t1 = t1.Replace("<" + type + ">", "");
        t1 = t1.Replace("</" + type + ">", "");
        t1 = t1.Replace("<![CDATA[", "");
        t1 = t1.Replace("]]>", "");
        return t1;
    }
    float jine = 0;
    public int log_sta(string dingdanhao)
    {
        int log_str = -1;
        string log_string = File.ReadAllText(Server.MapPath("/inc/weixinpay/demo/log.xml"));
        Regex reg = new Regex("<xml>?.*?</xml>", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(log_string);
        foreach (Match match in matches)
        {
            string result_code = get_xml_jiedian("result_code", match.ToString());
            string total_fee = get_xml_jiedian("total_fee", match.ToString());
            string out_trade_no = get_xml_jiedian("out_trade_no", match.ToString());
            string openid = get_xml_jiedian("openid", match.ToString());

            if (out_trade_no != "")
            {
                if (out_trade_no == dingdanhao)
                {
                    jine = float.Parse(total_fee);
                    log_str = 1;

                }
            }
          //  Response.Write(result_code + "|" + total_fee + "|" + out_trade_no + "|" + openid + "<br><br><br>");
        }
     //   Response.End();
            return log_str;
    }
    public void dingdan_chuli()
    {
        string dingdanhao = Request.QueryString["dingdanhao"].ToString();
        string tablename = "";
        File.WriteAllText(Server.MapPath("/bb.txt"), tablename + dingdanhao);

        File.WriteAllText(Server.MapPath("/bb.txt"), "select * from sl_daodian where zhuangtai='未付款' and dingdanhao='" + dingdanhao + "'" + "||" + tablename + "||" + Request.QueryString["dingdanhao"].ToString());
        #region 支付成功后的订单状态的处理
        my_o.orderok_chuli(dingdanhao, tablename,jine);
        #endregion


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            //if (type == "search")
            //{
            //    string content = File.ReadAllText(Server.MapPath("/inc/weixinpay/demo/notify_url.log"));
            //    Regex reg2 = new Regex("【支付成功" + Request.QueryString["dingdanhao"].ToString() + "?】", RegexOptions.Singleline);
            //    Match matches = reg2.Match(content);
            //    if (matches.ToString() != "")
            //    {

            //        dingdan_chuli();
            //    }

            //}
            //else
            //{

            //}

            if (log_sta(Request.QueryString["dingdanhao"].ToString()) > -1)
            {
                dingdan_chuli();

            }




        }
    }
}
