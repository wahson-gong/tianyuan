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
    public void get_user_count(string yonghuming,int hang)
    {

        DataTable dt = my_c.GetTable("select yonghuming from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where jieshaoren='" + yonghuming + "' and yonghuming<>'"+ yonghuming + "' order by id desc");
     
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (hang == 1)
                {
                    user_count1 = user_count1 + 1;
                }
                if (hang == 2)
                {
                    user_count2 = user_count2 + 1;
                }
                if (hang == 3)
                {
                    user_count3 = user_count3 + 1;
                }
                if (hang > 3)
                {
                    break;
                }

                jiaoyi(dt.Rows[i]["yonghuming"].ToString(),hang);
                int tt1 = hang + 1;
               
                get_user_count(dt.Rows[i]["yonghuming"].ToString(), tt1);
            }
        }
     
    }
    //我的客户数 end
    //交易 start
    public void jiaoyi(string yonghuming,int hang)
    {
        if (hang == 1)
        {
            order_count1 = order_count1 + int.Parse(my_c.GetTable("select count(id) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());
            float jine = 0;
            try
            {
                jine = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());
            }
            catch { }
            order_sum1 = order_sum1 + jine;
        }

        if (hang == 2)
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

        if (hang == 3)
        {
            order_count3 = order_count3 + int.Parse(my_c.GetTable("select count(id) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());
            float jine = 0;
            try
            {
                jine = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_order where yonghuming='" + yonghuming + "' and zhuangtai='订单完成'").Rows[0]["count_id"].ToString());
            }
            catch { }
            order_sum3 = order_sum3 + jine;
        }

    }
    //交易 end
    public string yonghuming = "";
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
            yonghuming = Request.QueryString["yonghuming"].ToString();
           

            get_user_count(yonghuming,1);
            user_count = user_count1 + user_count2 + user_count3;
            order_sum = order_sum1 + order_sum2 + order_sum3;
            order_count = order_count1 + order_count2 + order_count3;
            Response.Write(user_count + "|"+ order_sum+ "|"+ order_count + "|" + user_count1 + "|" + order_sum1 + "|" + order_count1 + "|" + user_count2 + "|" + order_sum2 + "|" + order_count2 + "|" + user_count3 + "|" + order_sum3 + "|" + order_count3);
            Response.End();



        }
    }
}
