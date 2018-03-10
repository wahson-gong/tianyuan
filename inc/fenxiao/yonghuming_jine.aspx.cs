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
    float jine = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable sl_order = my_c.GetTable("select * from sl_order where zhuangtai='订单完成' and yonghuming='" + my_b.c_string(Request["yonghuming"]) + "'");
            for (int i = 0; i < sl_order.Rows.Count; i++)
            {
                DataTable sl_caiwu = my_c.GetTable("select * from sl_caiwu where zhuangtai='已付款' and dingdanhao='" + sl_order.Rows[i]["dingdanhao"].ToString() + "'");
                if (sl_caiwu.Rows.Count > 0)
                {
                    jine = jine + float.Parse(sl_caiwu.Rows[0]["jine"].ToString());
                }
            }

            Response.Write(jine);



        }
    }
}
