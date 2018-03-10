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
using System.IO;
using System.Text;
public partial class admin_articles_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string type = "";
    static DataTable Model_dt = new DataTable();
    biaodan db = new biaodan();
    my_html my_h = new my_html();
    //end listbox
    public void set_html(string g1, string g2, string g3, string g4, string filepath)
    {

        if (my_b.set_mode() == "静态网站")
        {

            try
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where id in (" + g3 + ")");
                string file_path = g2;
                if (my_b.c_string(dt.Rows[0]["u6"].ToString()) != "")
                {
                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + g2);
                    try
                    {
                        Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + filepath);
                    }
                    catch
                    { }
                }

                my_h.set_content(File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + dt.Rows[0]["u6"].ToString(), Encoding.UTF8), g4, g1, file_path);
            }
            catch
            { }
        }


    }
    
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

            string url = my_b.get_Domain() + "err.aspx?errurl=single.aspx?m=daodianfzw123id=" + Request.QueryString["id"].ToString() + "";
            Image1.ImageUrl = "http://qr.liantu.com/api.php?text=" + Server.HtmlEncode(url) + "";
            Literal1.Text = url;

            url = my_b.get_Domain() + "err.aspx?errurl=single.aspx?m=yuyue_orderfzw123id=" + Request.QueryString["id"].ToString() + "";
            Image2.ImageUrl = "http://qr.liantu.com/api.php?text=" + Server.HtmlEncode(url) + "";
            Literal2.Text = url;


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
                            db.set_kj_edit(c, dt.Rows[0][Model_dt.Rows[d1]["u1"].ToString()].ToString(), Model_dt,d1);
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

            Response.Redirect("err.aspx?err=此条信息已设置成【已审核】！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("dianpu_add.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
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
                catch {
                    Response.Redirect("err.aspx?err=无此功能！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
              

                //  审核通过发短信

                string yonghuming = dt.Rows[0]["yonghuming"].ToString();
                string youxiang = "";
                if (yonghuming != "")
                {
                    youxiang = my_c.GetTable("select youxiang  from sl_user where yonghuming='" + yonghuming + "'").Rows[0]["youxiang"].ToString();
                }

                // 发送邮件
                if (youxiang != "")
                {
                    string leixingbiaoti = "文章审核未通过";
                    string leixing = "邮件";
                    string content = my_b.get_value("youjianneirong", "sl_fasong", "where leixing='" + leixing + "' and leixingbiaoti='" + leixingbiaoti + "'");
                    if (content != "")
                    {
                        string biaoti = my_b.get_value("biaoti", "sl_fasong", "where leixing='" + leixing + "' and leixingbiaoti='" + leixingbiaoti + "'");
                        //Response.Write(youxiang + "<br>");
                        //Response.Write(my_b.set_neirong(sl_wenzhang, biaoti) + "<br>");
                        //Response.Write(my_b.set_neirong(sl_wenzhang, content) + "<br>");
                        my_b.WebMailTo(youxiang, my_b.set_neirong(dt, biaoti), my_b.set_neirong(dt, content));
                    }
                }
                //  Response.End();
            }
            Response.Redirect("err.aspx?err=此条信息已设置成【未通过】！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("dianpu_add.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        else if (type == "del")
        {
            string table_name = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString());
            DataTable dt = my_c.GetTable("select * from " + table_name + " where id in (" + Request.QueryString["id"].ToString() + ")");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

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
                    if (dt_Field.Rows[j]["u6"].ToString() == "组图")
                    {
                        my_b.del_zutu(dt.Rows[i][dt_Field.Rows[j]["u1"].ToString()].ToString());
                    }

                }

                my_b.del_pic(dt.Rows[i]["Filepath"].ToString() + dt.Rows[i]["id"].ToString() + ".html");
                my_c.genxin("delete from " + table_name + " where id=" + dt.Rows[i]["id"].ToString());
            }

            Response.Redirect("err.aspx?err=删除信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("dianpu.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
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


        button = (Button)form1.FindControl("tqlable");
        try
        {
            button.Command += new CommandEventHandler(this.On_lable);
        }
        catch
        { }


    }
    //标签按钮
    protected void On_lable(Object sender, CommandEventArgs e)
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
                if (Model_dt.Rows[d1]["u6"].ToString() == "标签")
                {
                    TextBox txt = new TextBox();
                    txt = (TextBox)form1.FindControl(Model_dt.Rows[d1]["u1"].ToString());
                    txt.Text = my_b.get_Keywords(t1);

                }
            }
        }

    }
    //缩略图按钮
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


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            string sql = "update " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " set ";
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
            string filepath = my_c.GetTable("select filepath from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " where id=" + Request.QueryString["id"].ToString() + "").Rows[0]["filepath"].ToString();



            my_c.genxin(sql);
            set_html(Request.QueryString["id"].ToString(), filepath, Request.QueryString["classid"].ToString(), my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + ""), filepath);
            Response.Redirect("err.aspx?err=修改信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("dianpu.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        else
        {
            string sql = "insert into " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " ";
            string filepath = "";
            try
            {
                DropDownList dr = new DropDownList();
                dr = (DropDownList)form1.FindControl("classid");
                string classid = dr.SelectedValue;
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
                sql = sql + ",Filepath) values (";
                for (int d1 = 0; d1 < Model_dt.Rows.Count; d1++)
                {
                    if (d1 == 0)
                    {
                        sql = sql + db.get_kj(Table1, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());
                    }
                    else
                    {
                        sql = sql + "," + db.get_kj(Table1, Model_dt.Rows[d1]["u6"].ToString(), Model_dt.Rows[d1]["u1"].ToString());
                    }
                }
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + Request.QueryString["classid"].ToString() + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "')";
                my_c.genxin(sql);

            }
            catch
            {
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
                sql = sql + ",Filepath,classid) values (";
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
                DateTime dy = DateTime.Now;
                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                filepath = my_b.get_value("u7", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort", "where id=" + Request.QueryString["classid"].ToString() + "") + my_b.chuli_lujing();
                sql = sql + ",'" + filepath + "'," + Request.QueryString["classid"].ToString() + ")";
                my_c.genxin(sql);

            }






            string over_id = my_c.GetTable("select max(id) as id from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + "").Rows[0]["id"].ToString();
            set_html(over_id, filepath, Request.QueryString["classid"].ToString(), my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + ""), filepath);

            //审核记录
            try
            {
                string article_new_id = my_c.GetTable("select top 1 id from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " order by id desc").Rows[0]["id"].ToString();
                string article_log = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + "{fenge}" + article_new_id + "{fenge}" + Request.QueryString["Model_id"].ToString();
                my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("admin_id") + "','" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
            }
            catch
            { }

            //审核记录End

            Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("dianpu.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
