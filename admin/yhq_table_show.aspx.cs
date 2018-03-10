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

public partial class yhq_table_show : System.Web.UI.Page
{
    my_conn my_c = new my_conn();
    my_basic my_b = new my_basic();
    public void set_kj_edit(TableCell c, string type, string id, string txt_value, string user_value)
    {
        if (type == "文件框")
        {
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = user_value + "&nbsp;&nbsp;<a href='" + user_value + "' target='_blank'>下载文件</a>";
            //添加控件到容器 
            c.Controls.Add(txt);
        }
        else if (type == "缩略图" || type == "头像")
        {
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = "<a href='showpic.aspx?url=" + user_value + "' target=_blank><img onerror=\"this.src='images/nopic.jpg';\"  src='" + user_value + "' height='160px'></a>";
            //添加控件到容器 
            c.Controls.Add(txt);
        }
        else
        {
            Literal txt;
            //创建TextBox 
            txt = new Literal();
            txt.Text = user_value;
            //添加控件到容器 
            c.Controls.Add(txt);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
            DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString());
            string url = my_b.get_Domain() + "inc/erweima.aspx?id=" + Request.QueryString["id"].ToString() + "";
            Image1.ImageUrl = "http://qr.liantu.com/api.php?text=" + url + "";
            Literal1.Text = url;
            int numrows = 1;
            int numcells = 2;
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                for (int j = 0; j < numrows; j++)
                {
                    TableRow r = new TableRow();
                    for (int i = 0; i < numcells; i++)
                    {
                        TableCell c = new TableCell();
                        if (i == 0)
                        {
                            c.Controls.Add(new LiteralControl(Model_dt.Rows[d1]["u2"].ToString()));
                            c.CssClass = "tRight";
                        }
                        else if (i == 1)
                        {
                            set_kj_edit(c, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), Model_dt.Rows[d1]["u8"].ToString(), dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString());
                        }
                        else
                        {

                        }


                        r.Cells.Add(c);

                    }
                    Table1.Rows.Add(r);
                }
            }




        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
    
        DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
        //Response.Write("update " + Model_dt.Rows[0]["u1"].ToString() + " set shouchuzhuangtai='已售' where  id=" + my_b.c_string(Request.QueryString["id"].ToString()));
        //Response.End();
        my_c.genxin("update " + Model_dt.Rows[0]["u1"].ToString() + " set shouchuzhuangtai='已售' where  id=" + my_b.c_string(Request.QueryString["id"].ToString()));

        Response.Write("<script>window.opener.show_yhq('此兑换卡已改成已已售！');window.close();</script>");
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
}