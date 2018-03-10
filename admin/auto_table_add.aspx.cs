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
public partial class admin_auto_table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    static DataTable Model_dt = new DataTable();
    biaodan db = new biaodan();
    my_order my_o = new my_order();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {


            Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
            DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " where id=" + Request.QueryString["id"].ToString());
    
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
                            db.set_kj_edit(c, dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString(), Model_dt, d1);

                        }
                        else
                        {

                        }


                        r.Cells.Add(c);

                    }
                    Table1.Rows.Add(r);
                }
            }
            #region 读操作记录
            db.read_log(Request.QueryString["Model_id"].ToString(), Request.QueryString["id"].ToString(), Table4);
            #endregion

        }
        else if (type == "shenhe")
        {
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    my_c.genxin("update " + table_name + " set shenhe='已审核' where id=" + dt.Rows[i]["id"].ToString() + "");
                }
                catch
                {
                    Response.Redirect("err.aspx?err=无此功能！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
            }
            Response.Redirect("err.aspx?err=此条信息已设置成【已审核】！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        else if (type == "shenhe1")
        {
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    my_c.genxin("update " + table_name + " set shenhe='未通过',shenheshuoming='" + my_b.c_string(Request.QueryString["shenheshuoming"].ToString()) + "' where id=" + dt.Rows[i]["id"].ToString() + "");
                }
                catch
                {
                    Response.Redirect("err.aspx?err=无此功能！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
            }
            Response.Redirect("err.aspx?err=此条信息已设置成【未通过】！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        else if (type == "del")
        {
            if (Request.QueryString["id"].ToString() == "")
            {
                Response.Redirect("err.aspx?err=请选择要删除的信息！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
            }
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                my_c.genxin("delete from " + table_name + " where id=" + dt.Rows[i]["id"].ToString());
                DataTable dt_Field = my_c.GetTable("select u6,u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString());
                for (int j = 0; j < dt_Field.Rows.Count; j++)
                {
                    if (dt_Field.Rows[j]["u6"].ToString() == "编辑器")
                    {
                        my_b.del_article_pic(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                    }
                    if (dt_Field.Rows[j]["u6"].ToString() == "缩略图")
                    {
                        my_b.del_pic(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                    }
                    if (dt_Field.Rows[j]["u6"].ToString() == "文件框")
                    {
                        my_b.del_pic(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                    }
                }
            }
            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            }
            catch
            { }
            if (laiyuanbianhao == "")
            {
                my_b.tiaozhuan("删除信息成功！马上跳转到信息列表页面！", "auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "");
            }
            else
            {
                my_b.tiaozhuan("删除信息成功！马上跳转到信息列表页面！", "auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + laiyuanbianhao + "");
            }
        }
        else
        {
            Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
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
                            db.set_kj(c, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString(), Model_dt.Rows[d1]["u8"].ToString(), Model_dt.Rows[d1]["u3"].ToString());
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
        //设置按钮事件
        Button button = new Button();
        button = (Button)form1.FindControl("tqtp");
        try
        {
            button.Command += new CommandEventHandler(this.On_Button);
        }
        catch
        { }

        //设置来源编号
        try
        {
            TextBox txt = new TextBox();
            txt = (TextBox)form1.FindControl("laiyuanbianhao");
            txt.Text = Request.QueryString["laiyuanbianhao"].ToString();
            txt.Enabled = false;
        }
        catch { }
    }
    protected void On_Button(Object sender, CommandEventArgs e)
    {
        Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString() + " order by u9,id");
        DataTable dt = my_c.GetTable("select * from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString()) + " ");
        string t1 = "";
        int d1 = 0;
        for (d1 = 0; d1 < Model_dt.Rows.Count; d1++)
        {
            if (Model_dt.Rows[d1]["u6"].ToString() == "编辑器")
            {
                TextBox txt = new TextBox();
                txt = (TextBox)form1.FindControl(Model_dt.Rows[d1]["u1"].ToString());
                t1 = my_b.c_string(txt.Text.ToString());

            }
        }
        if (t1 != "")
        {
            for (d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (Model_dt.Rows[d1]["u6"].ToString() == "缩略图")
                {
                    TextBox txt = new TextBox();
                    txt = (TextBox)form1.FindControl(Model_dt.Rows[d1]["u1"].ToString());
                    txt.Text = my_b.get_images(t1, 1);

                }
            }
        }

    }



    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string laiyuanbianhao = "";
        try
        {
            laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
        }
        catch
        { }

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id = " + Request.QueryString["Model_id"].ToString() + "");
            string sql = "update " + table_name + " set ";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString() + "=" + db.get_kj(Table1, Model_dt, d1) + "";
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString() + "=" + db.get_kj(Table1, Model_dt, d1) + "";
                }
            }

            sql = sql + " where id=" + Request.QueryString["id"].ToString();
            //Response.Write(sql);
            //Response.End();


            db.write_log(Request.QueryString["Model_id"].ToString(), Request.QueryString["id"].ToString(), Model_dt, Table1);
            my_c.genxin(sql);
           
            //end
            if (laiyuanbianhao == "")
            {
                Response.Redirect("err.aspx?err=修改信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
            }
            else
            {
                Response.Redirect("err.aspx?err=修改信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + laiyuanbianhao + "", "&", "fzw123") + "");
            }
        }
        else
        {
            string sql = "insert into " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " ";
            sql = sql + "(";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + Model_dt.Rows[d1]["u1"].ToString();
                }
                else
                {
                    sql = sql + "," + Model_dt.Rows[d1]["u1"].ToString();
                }
            }
            sql = sql + ") values (";
            for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
            {
                if (d1 == 0)
                {
                    sql = sql + db.get_kj(Table1, Model_dt, d1);
                }
                else
                {
                    sql = sql + "," + db.get_kj(Table1, Model_dt, d1);
                }
            }
            sql = sql + ")";

            
            my_c.genxin(sql);
            if (laiyuanbianhao == "")
            {
                Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
            }
            else
            {
                Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("auto_table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&laiyuanbianhao=" + laiyuanbianhao + "", "&", "fzw123") + "");
            }
        }
    }
}
