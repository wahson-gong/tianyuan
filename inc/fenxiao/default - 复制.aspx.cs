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
using System.IO;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    //我的客户数 start
    int user_count = 0;
    float order_sum = 0;
    int order_count = 0;

    int user_count1 = 0;
    float order_sum1 = 0;
    int order_count1 = 0;

    int user_count2 = 0;
    float order_sum2 = 0;
    int order_count2 = 0;

    int user_count3 = 0;
    float order_sum3 = 0;
    int order_count3 = 0;
    public void get_user_count1(string yonghuming)
    {

        DataTable dt = my_c.GetTable("select yonghuming from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where jieshaoren='" + yonghuming + "' and yonghuming<>'"+ yonghuming + "' order by id desc");
      
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                user_count1 = user_count1 + 1;
                jiaoyi1(dt.Rows[i]["yonghuming"].ToString());
             
            }
        }
     
    }
    //我的客户数 end
    //交易 start
    public void jiaoyi1(string yonghuming)
    {
        order_count1 = order_count1 + int.Parse(my_c.GetTable("select count(id) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());

        float jine = 0;
        try
        {
            jine=float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());
        }
        catch { }
        order_sum1 = order_sum1 + jine;

    }
    //交易 end
    public void get_user_count2(string yonghuming)
    {

        DataTable dt = my_c.GetTable("select yonghuming from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where jieshaoren='" + yonghuming + "' and yonghuming<>'" + yonghuming + "' order by id desc");

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dt1 = my_c.GetTable("select yonghuming from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where jieshaoren='" + dt.Rows[i]["yonghuming"].ToString() + "' and yonghuming<>'" + dt.Rows[i]["yonghuming"].ToString() + "' order by id desc");
                if (dt.Rows.Count > 0)
                {
                    for (int i1 = 0; i1 < dt1.Rows.Count; i1++)
                    {
                        user_count2 = user_count2 + 1;
                        jiaoyi2(dt.Rows[i1]["yonghuming"].ToString());
                    }
                }
                   

            }
        }

    }
    //我的客户数 end
    //交易 start
    public void jiaoyi2(string yonghuming)
    {
        order_count2 = order_count2 + int.Parse(my_c.GetTable("select count(id) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());

        float jine = 0;
        try
        {
            jine = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());
        }
        catch { }
        order_sum2 = order_sum2 + jine;

    }
    //交易 end
    public string yonghuming = "";
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
            yonghuming = Request.QueryString["yonghuming"].ToString();
           

            get_user_count(yonghuming);
      
            Response.Write(user_count+"|"+ jyl_sum+"|"+jyl_count);
            Response.End();



        }
    }
}
