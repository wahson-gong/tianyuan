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
    string type = "";
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

        string classid = "";
        try
        {
            classid = Request.QueryString["classid"].ToString();
          
        }
        catch
        {
            Response.Redirect("Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString());
        }
        //设置当前位置
        try
        {
            Literal4.Text = my_b.set_weizhi(classid, "Class_Table.aspx?Model_id=" + Request.QueryString["Model_id"].ToString() + "&classid=$classid$", "sl_sort");
        }
        catch
        {
            Literal4.Text = "内容搜索";
        }
        //end
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

            Response.Redirect("err.aspx?err=此条信息已设置成【已审核】！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("news_table.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
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
            Response.Redirect("err.aspx?err=此条信息已设置成【未通过】！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("news_table.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
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
                #region 删除搜索汇总表记录
                DataTable sl_Model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='sl_search'");
                if (sl_Model.Rows.Count > 0)
                {
                    my_c.genxin("delete from sl_search where classid=" + dt.Rows[i]["classid"].ToString() + " and laiyuanbianhao=" + dt.Rows[i]["id"].ToString() + "");
                }
                #endregion
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
            my_b.tiaozhuan("删除信息成功！马上跳转到信息列表页面！", "articles.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "");
     
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
        string id = "";
        string filepath = "";
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            #region 修改
            id = Request.QueryString["id"].ToString(); //修改获取ID
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
            filepath = my_c.GetTable("select filepath from " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " where id=" + Request.QueryString["id"].ToString() + "").Rows[0]["filepath"].ToString();

            db.write_log(Request.QueryString["Model_id"].ToString(), Request.QueryString["id"].ToString(), Model_dt, Table1);

            my_c.genxin(sql);
            set_html(Request.QueryString["id"].ToString(), filepath, Request.QueryString["classid"].ToString(), my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + ""), filepath);

            #endregion
        }
        else
        {
            #region 增加
            string sql = "insert into " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + " ";
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
                sql = sql + ",'" + filepath + "')";


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
            }
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                id = my_c.GetTable(sql + " select @@IDENTITY as id").Rows[0]["id"].ToString();
            }
            else
            {
                my_c.genxin(sql);
            }

            #endregion
        }
        #region 生成、增加审核信息、插入搜索信息
        if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
        {
            set_html(id, filepath, Request.QueryString["classid"].ToString(), my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + ""), filepath);

            //审核记录
            string article_log = my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Request.QueryString["Model_id"].ToString() + "") + "{fenge}" + id + "{fenge}" + Request.QueryString["Model_id"].ToString();

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + my_b.k_cookie("admin_id") + "','" + article_log + "','" + Request.UserHostAddress.ToString() + "','文章审核')");
            //审核记录End
            db.charu_search(Request.QueryString["Model_id"].ToString(), id);

        }
        #endregion
        if (type == "edit")
        {
            Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("articles.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        else
        {
            Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("articles.aspx?classid=" + Request.QueryString["classid"].ToString() + "&Model_id=" + Request.QueryString["Model_id"].ToString() + "", "&", "fzw123") + "");
        }
        //end
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
