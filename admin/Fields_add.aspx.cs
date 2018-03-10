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
public partial class admin_Fields_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    int i = 0;
    //处理约束
    public void yueshu(string tablename, string ziduan)
    {
        string sql = "select * from sysobjects where parent_obj in(select id from sysobjects where name='" + tablename + "') and name like '%" + ziduan + "%'";
        DataTable dt = new DataTable();
        try
        {
            dt = my_c.GetTable(sql);
        }
        catch { }
        if (dt.Rows.Count > 0)
        {
            try
            {
                my_c.genxin("alter table " + tablename + " drop constraint " + dt.Rows[0]["name"].ToString() + "");
            }
            catch { }
        }
    }
    //end
    #region 规格处理
    public void guigechuli()
    {
        string guigefenlei = ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigefenlei";

        string guigejiage = ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigejiage";

        string guigecanshu = ConfigurationSettings.AppSettings["Prefix"].ToString() + "guigecanshu";


        if (my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where u1='" + guigefenlei + "'").Rows.Count == 0)
        {
            #region 规格分类
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + guigefenlei + " (id int IDENTITY (1,1) PRIMARY KEY,fenlei  int default 0,laiyuanbianhao int default 0,canshu VARCHAR (250),Model_id int default 0,dtime datetime default getdate())");
            }
            else
            {
                my_c.genxin("create table " + guigefenlei + " (id int IDENTITY (1,1) PRIMARY KEY,fenlei  int default 0,laiyuanbianhao int default 0,canshu VARCHAR (250),Model_id int default 0,dtime datetime default now())");

            }
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model(u1,u2,u3,u4,u5,u6,u7,u8,u9) values('" + guigefenlei + "','规格分类表','表单模型','','','','','','')");

            string Model_id = my_c.GetTable("select top 1 id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model order by id desc").Rows[0]["id"].ToString();

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('fenlei','分类','是','是','是','数字','','',5,'是','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('laiyuanbianhao','来源编号','是','是','是','数字','','',5,'是','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('canshu','参数','是','是','是','文本框','','',5,'是','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('Model_id','模型编号','是','是','是','数字','','',5,'否','10%'," + Model_id + ")");
            #endregion
            #region 规格参数
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + guigecanshu + " (id int IDENTITY (1,1) PRIMARY KEY,classid  int default 0,laiyuanbianhao int default 0,biaoti VARCHAR (250),paixu int default 0,dtime datetime default getdate())");
            }
            else
            {
                my_c.genxin("create table " + guigecanshu + " (id int IDENTITY (1,1) PRIMARY KEY,classid  int default 0,laiyuanbianhao int default 0,biaoti VARCHAR (250),paixu int default 0,dtime datetime default now())");

            }
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model(u1,u2,u3,u4,u5,u6,u7,u8,u9) values('" + guigecanshu + "','规格参数表','分类模型','','','','','','')");

             Model_id = my_c.GetTable("select top 1 id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model order by id desc").Rows[0]["id"].ToString();

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('classid','分类','','是','是','上级','','',3,'是','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('laiyuanbianhao','来源编号','','是','是','数字','','',2,'是','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('biaoti','标题','','是','是','文本框','','',1,'是','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('paixu','排序','','是','是','数字','','',5,'否','10%'," + Model_id + ")");
            #endregion
            #region 规格价格
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + guigejiage + " (id int IDENTITY (1,1) PRIMARY KEY,canshu VARCHAR (250),fenlei VARCHAR (250),jiage float default 0,laiyuanbianhao int default 0,Model_id int default 0,dtime datetime default getdate())");
            }
            else
            {
                my_c.genxin("create table " + guigejiage + " (id int IDENTITY (1,1) PRIMARY KEY,canshu VARCHAR (250),fenlei VARCHAR (250),jiage float default 0,laiyuanbianhao int default 0,Model_id int default 0,dtime datetime default now())");
            }
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model(u1,u2,u3,u4,u5,u6,u7,u8,u9) values('" + guigejiage + "','规格价格表','表单模型','','','','','','')");

            Model_id = my_c.GetTable("select top 1 id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model order by id desc").Rows[0]["id"].ToString();

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('laiyuanbianhao','来源编号','是','是','是','数字','','',5,'否','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('Model_id','模型编号','是','是','是','数字','','',5,'否','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('jiage','价格','是','是','是','货币','','',5,'否','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('fenlei','分类','','是','是','文本框','','',5,'否','10%'," + Model_id + ")");

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('canshu','参数','','是','是','文本框','','',5,'是','10%'," + Model_id + ")");
            #endregion

            //end
        }
    }
    #endregion 规格处理
    public void get_filed(string u6, string Model_id, string u1, string type, string user_value)
    {
        if (type == "add")
        {
            #region 增加字段
            if (u6 == "数字")
            {
                Regex reg = new Regex("random.*", RegexOptions.Singleline);
                Match ma = reg.Match(user_value);
                if (ma.ToString() != "")
                {
                    user_value = "0";
                }

                my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " int default " + int.Parse(user_value) + "");
            }
            else if (u6 == "联动" || u6 == "分类" || u6 == "上级")
            {
                Regex reg = new Regex("random.*", RegexOptions.Singleline);
                Match ma = reg.Match(user_value);
                if (ma.ToString() != "")
                {
                    user_value = "0";
                }

                my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " int default 0");
            }
            else if (u6 == "规格")
            {

                guigechuli();

                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " VARCHAR (250) NULL");
                }
                catch { }
            }
            else if (u6 == "货币")
            {
                Regex reg = new Regex("random.*", RegexOptions.Singleline);
                Match ma = reg.Match(user_value);
                if (ma.ToString() != "")
                {
                    user_value = "0";
                }

                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " float default " + int.Parse(user_value) + "");
                }
                catch { }
            }
            else if (u6 == "时间框")
            {
                if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "0")
                {
                    #region 增加时间框的处理ACCESS
                    if (user_value == "当前时间")
                    {
                        try
                        {
                            my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " datetime default now()");
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " datetime");
                        }
                        catch { }
                    }
                    #endregion
                }
                else
                {
                    #region 增加时间框的处理SQL
                    if (user_value == "当前时间")
                    {
                        try
                        {
                            my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " datetime default getdate()");
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " datetime ");
                        }
                        catch { }
                    }
                    #endregion

                }
            }
            else if (u6 == "编辑器" || u6 == "文本域" || u6 == "文件框" || u6 == "子编辑器" || u6 == "组图")
            {
                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " TEXT NULL");
                }
                catch { }
            }
            else
            {
                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ADD " + u1 + " VARCHAR (250) NULL");
                }
                catch { }
            }
            #endregion 增加字段
        }
        else
        {
            #region 编辑字段
            yueshu(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id = " + Model_id), u1);
            if (u6 == "数字")
            {
                Regex reg = new Regex("random.*", RegexOptions.Singleline);
                Match ma = reg.Match(user_value);
                if (ma.ToString() != "")
                {
                    user_value = "0";
                }

                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " int default " + int.Parse(user_value) + "");
                }
                catch
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " int default " + int.Parse(user_value) + "");
                }
            }
            else if (u6 == "联动" || u6 == "分类" || u6 == "上级")
            {
                Regex reg = new Regex("random.*", RegexOptions.Singleline);
                Match ma = reg.Match(user_value);
                if (ma.ToString() != "")
                {
                    user_value = "0";
                }

                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " int default 0");
                }
                catch
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " int default 0");
                }
            }
            else if (u6 == "规格")
            {

                guigechuli();

                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " int default " + int.Parse(user_value) + "");
                }
                catch
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " int default " + int.Parse(user_value) + "");
                }
            }
            else if (u6 == "货币")
            {
                Regex reg = new Regex("random.*", RegexOptions.Singleline);
                Match ma = reg.Match(user_value);
                if (ma.ToString() != "")
                {
                    user_value = "0";
                }

                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " float ");
                }
                catch { }
            }
            else if (u6 == "时间框")
            {
                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " datetime default getdate()");
                }
                catch { }
            }
            else if (u6 == "编辑器" || u6 == "文本域" || u6 == "文件框" || u6 == "子编辑器")
            {
                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " TEXT NULL");
                }
                catch { }
            }
            else
            {
                try
                {
                    my_c.genxin("ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + Model_id) + " ALTER COLUMN " + u1 + " VARCHAR (250) NULL");
                }
                catch { }
            }
            #endregion 编辑字段
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            DropDownList1.DataSource = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=2");
            DropDownList1.DataTextField = "u1";
            DropDownList1.DataValueField = "u1";
            DropDownList1.DataBind();
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            if (type == "edit")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                this.TextBox1.Enabled = false;
                this.TextBox2.Text = dt.Rows[0]["u2"].ToString();
                this.TextBox3.Text = dt.Rows[0]["u3"].ToString();
                this.TextBox4.Text = dt.Rows[0]["u7"].ToString();
                this.TextBox5.Text = dt.Rows[0]["u8"].ToString();
                this.TextBox7.Text = dt.Rows[0]["u9"].ToString();
                this.TextBox6.Text = dt.Rows[0]["u11"].ToString();
                this.TextBox8.Text = dt.Rows[0]["u12"].ToString();
                this.TextBox9.Text = dt.Rows[0]["shijian"].ToString();
                if (dt.Rows[0]["u4"].ToString() == "是")
                {
                    CheckBox1.Checked = true;
                }
                if (dt.Rows[0]["u5"].ToString() == "是")
                {
                    CheckBox2.Checked = true;
                }
                if (dt.Rows[0]["u10"].ToString() == "是")
                {
                    CheckBox3.Checked = true;
                }
                if (dt.Rows[0]["u13"].ToString() == "是")
                {
                    CheckBox4.Checked = true;
                }
                if (dt.Rows[0]["jiami"].ToString() == "是")
                {
                    CheckBox5.Checked = true;
                }
                if (dt.Rows[0]["liebiaocaozuo"].ToString() == "是")
                {
                    CheckBox6.Checked = true;
                }
                if (dt.Rows[0]["daoru"].ToString() == "是")
                {
                    CheckBox7.Checked = true;
                }

                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["u6"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                        break;
                    }

                }


            }
            else if (type == "del")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + Request.QueryString["id"].ToString());

                yueshu(my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[0]["Model_id"].ToString()), dt.Rows[0]["u1"].ToString());

                try
                {
                    my_c.genxin(" ALTER TABLE " + my_b.get_value("u1", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + dt.Rows[0]["Model_id"].ToString()) + " DROP COLUMN " + dt.Rows[0]["u1"].ToString() + "");
                }
                catch { }

                my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where id=" + Request.QueryString["id"].ToString());
                Response.Redirect("err.aspx?err=删除字段成功！马上跳转到字段管理页面！&errurl=" + my_b.tihuan("Fields.aspx?Model_id=" + Request.QueryString["Model_id"].ToString(), "&", "fzw123") + "");
            }
            else
            {
                DataTable sl_Model = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["Model_id"].ToString() + "");
                if (sl_Model.Rows[0]["u3"].ToString() == "文章模型" || sl_Model.Rows[0]["u3"].ToString() == "新闻模型" || sl_Model.Rows[0]["u3"].ToString() == "产品模型")
                {
                    if (int.Parse(my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString()).Rows[0]["count_id"].ToString()) > 4)
                    {
                        this.TextBox7.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString()).Rows[0]["count_id"].ToString();
                    }
                }
                else
                {
                    this.TextBox7.Text = my_c.GetTable("select count(id) as count_id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["Model_id"].ToString()).Rows[0]["count_id"].ToString();
                }




            }
        }
    }

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        if (my_b.get_cn(my_b.c_string(this.TextBox1.Text)) == 1)
        {
            this.TextBox1.Text = "";

        }
        else
        {
            if (my_b.get_count(ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field", "where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u1='" + my_b.c_string(this.TextBox1.Text) + "'") > 0)
            {
                this.TextBox1.Text = "";
                Label1.Text = "此字段名数据库表中已存在";
            }
            else
            {
                Label1.Text = "*这里的名称跟表中字段一样";
            }
        }
    }
    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox2.Text);
        if (u1 != "")
        {
            if (my_b.c_string(this.TextBox1.Text) == "")
            {
                this.TextBox1.Text = my_b.hanzi(u1, "_");
                if (my_b.get_count(ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field", "where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u1='" + my_b.c_string(this.TextBox1.Text) + "'") > 0)
                {
                    this.TextBox1.Text = "";
                    Label1.Text = "此字段名数据库表中已存在";
                }
                else
                {
                    Label1.Text = "*这里的名称跟表中字段一样";
                }
            }
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue == "数字" || DropDownList1.SelectedValue == "货币")
        {
            this.TextBox5.Text = "0";
        }
        else if (DropDownList1.SelectedValue == "标题")
        {

            if (my_b.get_count(ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field", "where Model_id=" + Request.QueryString["Model_id"].ToString() + " and u6 like '标题'") > 0)
            {
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    DropDownList1.Items[i].Selected = false;
                }
            }
              
        }
        else if (DropDownList1.SelectedValue == "时间框")
        {
            this.TextBox5.Text = "当前时间";
        }
        else if (DropDownList1.SelectedValue == "规格")
        {
            this.TextBox1.Text = "guige";
            this.TextBox2.Text = "规格";
            Label3.Text = "默认值请输入参数表的Classid";
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text).ToLower();
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = my_b.c_string(this.TextBox3.Text);
        string u4 = "否";
        if (CheckBox1.Checked)
        {
            u4 = "是";
        }
        string u5 = "否";
        if (CheckBox2.Checked)
        {
            u5 = "是";
        }
        string u6 = DropDownList1.SelectedItem.Value;
        string u7 = my_b.c_string(this.TextBox4.Text);
        string u8 = my_b.c_string1(this.TextBox5.Text);
        string u9 = my_b.c_string(this.TextBox7.Text);
        string Model_id = Request.QueryString["Model_id"].ToString();
        string u10 = "否";
        if (CheckBox3.Checked)
        {
            u10 = "是";
        }
        string u11 = my_b.c_string(this.TextBox6.Text);
        string u12 = my_b.c_string(this.TextBox8.Text);
        string shijian = my_b.c_string(this.TextBox9.Text);
        string u13 = "否";
        if (CheckBox4.Checked)
        {
            u13 = "是";
        }
        string jiami = "否";
        if (CheckBox5.Checked)
        {
            jiami = "是";
        }
        string liebiaocaozuo = "否";
        if (CheckBox6.Checked)
        {
            liebiaocaozuo = "是";
        }
        string daoru = "否";
        if (CheckBox7.Checked)
        {
            daoru = "是";
        }
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            try
            {
                get_filed(u6, Request.QueryString["Model_id"].ToString(), u1, "edit", u8);
            }
            catch { }
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "',u7='" + u7 + "',u8='" + u8 + "',u9=" + u9 + ",u10='" + u10 + "',u11='" + u11 + "',u12='" + u12 + "',u13='" + u13 + "',jiami='" + jiami + "',liebiaocaozuo='" + liebiaocaozuo + "',daoru='" + daoru + "',shijian='" + shijian + "' where id=" + Request.QueryString["id"].ToString());

            Response.Redirect("err.aspx?err=修改字段成功！马上跳转到字段管理页面！&errurl=" + my_b.tihuan("Fields.aspx?Model_id=" + Request.QueryString["Model_id"].ToString(), "&", "fzw123") + "");
        }
        else
        {
            //try
            //{
            get_filed(u6, Model_id, u1, "add", u8);
            //}
            //catch
            //{
            //    Response.Redirect("err.aspx?err=增加数据库字段时出错，是否字段名重复！&errurl=" + my_b.tihuan("Fields_add.aspx?id=" + Request.QueryString["id"].ToString() + "", "&", "fzw123") + "");
            //}

            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,u12,u13,Model_id,jiami,liebiaocaozuo,shijian,daoru) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + u5 + "','" + u6 + "','" + u7 + "','" + u8 + "'," + u9 + ",'" + u10 + "','" + u11 + "','" + u12 + "','" + u13 + "'," + Model_id + ",'" + jiami + "','" + liebiaocaozuo + "','" + shijian + "','" + daoru + "')");


            Response.Redirect("err.aspx?err=添加字段成功！马上跳转到字段管理页面！&errurl=" + my_b.tihuan("Fields.aspx?Model_id=" + Request.QueryString["Model_id"].ToString(), "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }
}
