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
public partial class admin_Model_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    int i = 0;
    public void set_field(string Model_type)
    {
        
            DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=53 order by id desc");
        string field_list = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (field_list == "")
            {
                field_list = dt.Rows[i]["u1"].ToString();
            }
            else
            {
                field_list = field_list + "\r\n" + dt.Rows[i]["u1"].ToString();
            }
        }
        string Model_id = my_c.GetTable("select top 1 id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model order by id desc").Rows[0]["id"].ToString();
        if (Model_type == "文章模型" || Model_type == "产品模型" || Model_type == "新闻模型")
        {
            #region 文章模型
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('prope','属性','','否','否','多选按钮组','','" + field_list + "',5,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('classid','分类ID','','否','否','分类','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('paixu','排序','','否','否','数字','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('dtime','更新时间','','否','否','时间框','','当前时间',100,'否','10%'," + Model_id + ")");
            #endregion
        }
        else if(Model_type == "商品模型")
        {
            #region 商品模型
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('prope','属性','','否','否','多选按钮组','','" + field_list + "',5,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('classid','分类ID','','否','否','分类','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('paixu','排序','','否','否','数字','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('dtime','更新时间','','否','否','时间框','','当前时间',100,'否','10%'," + Model_id + ")");
            #endregion
        }
        else if (Model_type == "分类模型")
        {
            #region 分类模型
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('classid','分类ID','','否','否','上级','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('paixu','排序','','否','否','数字','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('dtime','更新时间','','否','否','时间框','','当前时间',100,'否','10%'," + Model_id + ")");
            #endregion
        }
        else
        {
            #region 其它
//            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('paixu','排序','','否','否','数字','','0',100,'否','10%'," + Model_id + ")");
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field(u1,u2,u3,u4,u5,u6,u7,u8,u9,u10,u11,Model_id) values('dtime','更新时间','','否','否','时间框','','当前时间',100,'否','10%'," + Model_id + ")");
            #endregion
        }

    }
    public void crea_table(string table_name,string Model_type)
    {
        if (Model_type == "文章模型"|| Model_type == "产品模型"|| Model_type == "新闻模型")
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + table_name + " (id int IDENTITY (1,1) PRIMARY KEY,classid int default 0,Filepath VARCHAR (250),dtime datetime default getdate(),paixu int default 0,prope VARCHAR (250))");
            }
            else
            {
                my_c.genxin("create table " + table_name + " (id autoincrement(1,1),classid int default 0,Filepath varchar,[dtime] datetime default now(),paixu int default 0,prope varchar)");
            }
            set_field(Model_type);
        }
        else if (Model_type == "商品模型")
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + table_name + " (id int IDENTITY (1,1) PRIMARY KEY,classid int default 0,Filepath VARCHAR (250),dtime datetime default getdate(),paixu int default 0,prope VARCHAR (250))");
            }
            else
            {
                my_c.genxin("create table " + table_name + " (id autoincrement(1,1),classid int default 0,Filepath varchar,[dtime] datetime default now(),paixu int default 0,prope varchar)");
            }
            set_field(Model_type);
        }
        else if(Model_type=="表单模型")
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + table_name + " (id int IDENTITY (1,1) PRIMARY KEY,dtime datetime default getdate())");
            }
            else
            {
                my_c.genxin("create table " + table_name + " (id autoincrement(1,1),[dtime] datetime default now())");
            }
            set_field(Model_type);
        }
        else if (Model_type == "分类模型")
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + table_name + " (id int IDENTITY (1,1) PRIMARY KEY,classid int default 0,paixu int default 0,dtime datetime default getdate())");
            }
            else
            {
                my_c.genxin("create table " + table_name + " (id autoincrement(1,1),classid int default 0,paixu int default 0,[dtime] datetime default now())");
            }
            set_field(Model_type);
        }
        else
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                my_c.genxin("create table " + table_name + " (id int IDENTITY (1,1) PRIMARY KEY,dtime datetime default getdate())");
            }
            else
            {
                my_c.genxin("create table " + table_name + " (id autoincrement(1,1),[dtime] datetime default now())");
            }
            set_field(Model_type);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        { 


            DropDownList1.DataSource = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where classid=11");
            DropDownList1.DataTextField = "u1";
            DropDownList1.DataValueField = "u1";
            DropDownList1.DataBind();

            string[] file_arr = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString());
            for (i = 0; i < file_arr.Length; i++)
            {

                DropDownList2.Items.Insert(0, file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString(),"").Substring(1));
                DropDownList2.Items[0].Value = file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1);
                DropDownList3.Items.Insert(0, file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1));
                DropDownList3.Items[0].Value = file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1);
            }
            DropDownList2.Items.Insert(0, "选择模板");
            DropDownList2.Items[0].Value = "";
            DropDownList3.Items.Insert(0, "选择模板");
            DropDownList3.Items[0].Value = "0";

            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            Label1.Text = "*例：" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "你设置的表名";

            if (type == "edit")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString().Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "");
                this.TextBox1.Enabled = false;
                this.TextBox2.Text = dt.Rows[0]["u2"].ToString();
                for (int i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["u3"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                for (int i = 0; i < DropDownList2.Items.Count; i++)
                {
                    if (DropDownList2.Items[i].Value == dt.Rows[0]["u4"].ToString())
                    {
                        DropDownList2.Items[i].Selected = true;
                    }
                }
                for (int i = 0; i < DropDownList3.Items.Count; i++)
                {
                    if (DropDownList3.Items[i].Value == dt.Rows[0]["u5"].ToString())
                    {
                        DropDownList3.Items[i].Selected = true;
                    }
                }
                this.TextBox3.Text = dt.Rows[0]["u6"].ToString();
                this.TextBox4.Text = dt.Rows[0]["u7"].ToString();
                this.TextBox5.Text = dt.Rows[0]["u8"].ToString();
                this.TextBox6.Text = dt.Rows[0]["u9"].ToString();
                this.TextBox7.Text = dt.Rows[0]["fangwen"].ToString();
                this.TextBox8.Text = dt.Rows[0]["fangwen_type"].ToString();
                this.TextBox9.Text = dt.Rows[0]["qianzhi"].ToString();
                if (dt.Rows[0]["xianshi"].ToString() == "是")
                {
                    CheckBox1.Checked = true;
                }
            }
            else if (type == "del")
            {
                if (Request.QueryString["id"].ToString() == "")
                {
                    Response.Redirect("err.aspx?err=请选择要删除的信息！&errurl=" + my_b.tihuan(Request.UrlReferrer.ToString(), "&", "fzw123") + "");
                }
                try
                {
                    if (int.Parse(my_c.GetTable("select count(id) as count_id from " + my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["id"].ToString() + "").Rows[0]["u1"].ToString() + "").Rows[0]["count_id"].ToString()) > 0)
                    {
                        Response.Redirect("err.aspx?err=删除模型失败！请确认所属模型表中数据是不否为空！&errurl=" + my_b.tihuan("articles.aspx?Model_id=" + Request.QueryString["id"].ToString() + "", "&", "fzw123") + "");
                    }
                    else
                    {
                        my_c.genxin("Drop Table " + my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["id"].ToString()).Rows[0]["u1"].ToString());
                        my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["id"].ToString());
                        my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["id"].ToString());
                        Response.Redirect("err.aspx?err=删除模型成功！马上跳转到模型管理页面！&errurl=" + my_b.tihuan("Model.aspx", "&", "fzw123") + "");
                    }
                }
                catch
                {
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Request.QueryString["id"].ToString());
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Field where Model_id=" + Request.QueryString["id"].ToString());
                    Response.Redirect("err.aspx?err=删除模型成功！马上跳转到模型管理页面！&errurl=" + my_b.tihuan("Model.aspx", "&", "fzw123") + "");
                }
               
            }
            else
            {
                
            }
           
        }
    }


    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        if (my_b.get_cn(my_b.c_string(this.TextBox1.Text)) == 1)
        {
            this.TextBox1.Text = "";
            Label1.Text = Label1.Text + "&nbsp;不能使用汉字";
        }
        else
        {
            Label1.Text = Label1.Text.Replace("&nbsp;不能使用汉字", "");
            this.TextBox1.Text = my_b.c_string(this.TextBox1.Text.Replace(ConfigurationSettings.AppSettings["Prefix"].ToString(), "").ToLower());
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = ConfigurationSettings.AppSettings["Prefix"].ToString() + my_b.c_string(this.TextBox1.Text).ToLower();
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = DropDownList1.SelectedItem.Value;
        string u4 = DropDownList2.SelectedItem.Value;
        string u5 = DropDownList3.SelectedItem.Value;
        string u6 = my_b.c_string(this.TextBox3.Text);
        string u7 = my_b.c_string(this.TextBox4.Text);
        string u8 = my_b.c_string(this.TextBox5.Text);
        string u9 = my_b.c_string(this.TextBox6.Text);
        string fangwen = my_b.c_string(this.TextBox7.Text);
        string fangwen_type = my_b.c_string(this.TextBox8.Text);
        string qianzhi = my_b.c_string(this.TextBox9.Text);
        string xianshi = "否";
        if (CheckBox1.Checked)
        {
            xianshi = "是";
        }
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "',u7='" + u7 + "',u8='" + u8 + "',u9='" + u9 + "',fangwen='" + fangwen + "',fangwen_type='" + fangwen_type + "',qianzhi='" + qianzhi + "',xianshi='" + xianshi + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改模型成功！马上跳转到模型管理页面！&errurl=" + my_b.tihuan("Model.aspx", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model(u1,u2,u3,u4,u5,u6,u7,u8,u9,fangwen,fangwen_type,qianzhi,xianshi) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + u5 + "','" + u6 + "','" + u7 + "','" + u8 + "','" + u9 + "','" + fangwen + "','" + fangwen_type + "','" + qianzhi + "','" + xianshi + "')");
            crea_table(u1, u3);

            Response.Redirect("err.aspx?err=添加模型成功！马上跳转到模型管理页面！&errurl=" + my_b.tihuan("Model.aspx", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {
        this.TextBox1.Text = my_b.pinyin(my_b.c_string(this.TextBox2.Text), 50);
        
    }
}
