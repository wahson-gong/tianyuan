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
using System.Text.RegularExpressions;

public partial class list_edit : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_paiwei my_p = new my_paiwei();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string id = my_b.c_string(Request.QueryString["id"].ToString());
            string Fields = my_b.c_string(Request.QueryString["Fields"].ToString());
            string zhi = my_b.c_string(Request.QueryString["zhi"].ToString());
            string Model_id = my_b.c_string(Request.QueryString["Model_id"].ToString());

            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
            string biaoming = Model_dt.Rows[0]["u1"].ToString();
            DataTable Field_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Model_id + " and U10='是'");
            //Response.Write("update " + biaoming + " set " + Fields + "='" + zhi + "' where id=" + id);
            //Response.End();
            #region 关于会员激活的后续操作
            if (biaoming == "sl_user")
            {
                if (Fields == "jihuo"&&zhi=="是")
                {
                    string dingdanhao = my_b.get_bianhao();
                    DataTable sl_user = my_c.GetTable("select * from sl_user where id=" + id);
                    if (sl_user.Rows.Count > 0)
                    {
                        my_p.jianglizhidu("投资金额", sl_user.Rows[0]["yonghuming"].ToString(), dingdanhao);//投资金额

                        string jieshaoren = sl_user.Rows[0]["jieshaoren"].ToString();
                        if (jieshaoren != "")
                        {
                            #region 开始计算返利
                            my_p.jianglizhidu("直推奖励", sl_user.Rows[0]["jieshaoren"].ToString(), dingdanhao);//直推奖励
                            my_p.jianglizhidu("广告费", sl_user.Rows[0]["jieshaoren"].ToString(), dingdanhao);//广告费
                                                                                                              //end
                            #endregion


                        }
                    }
                }
            }
            #endregion
            my_c.genxin("update " + biaoming + " set "+ Fields + "='" + zhi + "' where id=" + id);
            Response.Write("ok");

        }
    }
}
