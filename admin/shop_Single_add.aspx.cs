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
public partial class admin_shop_Single_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    DataTable dt = new DataTable();
    my_html my_h = new my_html();
    int i = 0;
    public void set_html(string u7,string u6,string id)
    {
        
        File.WriteAllText(HttpContext.Current.Request.PhysicalApplicationPath+u7, my_h.set_Single(File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString() + "/" + u6, Encoding.UTF8), "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single", id), Encoding.UTF8);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateTime dy = DateTime.Now;

            string[] file_arr = Directory.GetFiles(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString());
            for (i = 0; i < file_arr.Length; i++)
            {
                DropDownList1.Items.Insert(0, file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1));
                DropDownList1.Items[0].Value = file_arr[i].ToString().Replace(HttpContext.Current.Request.PhysicalApplicationPath+"/template/" + ConfigurationSettings.AppSettings["template"].ToString(), "").Substring(1);
            }
            DropDownList1.Items.Insert(0, "—请选择模板页面—");
            DropDownList1.Items[0].Value = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {

                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                this.TextBox2.Text = dt.Rows[0]["u2"].ToString();
                this.TextBox3.Text = dt.Rows[0]["u3"].ToString();
                this.TextBox4.Text = dt.Rows[0]["u4"].ToString();
                this.FreeTextBox1.Text = dt.Rows[0]["u5"].ToString();
                for (i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == dt.Rows[0]["u6"].ToString())
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
                this.TextBox5.Text = dt.Rows[0]["u7"].ToString();
            }
            else if (type == "sc")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single where id  in (" + Request.QueryString["id"].ToString() + ")");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string file_path = HttpContext.Current.Request.PhysicalApplicationPath+dt.Rows[i]["u7"].ToString().Substring(0, dt.Rows[i]["u7"].ToString().LastIndexOf("/")+1);
                    if (type != "")
                    {
                        Directory.CreateDirectory(file_path);
                    }
                    set_html(dt.Rows[i]["u7"].ToString(), dt.Rows[i]["u6"].ToString(), dt.Rows[i]["id"].ToString());
                    
                }
                my_b.tiaozhuan("生成单页面成功，正在跳转单页面管理页面！", "shop_Single.aspx");
             
            }
            else if (type == "del")
            {
                dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single where id in ( " + Request.QueryString["id"].ToString() + ")");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    my_b.del_article_pic(dt.Rows[i]["u5"].ToString());
                    my_b.del_pic(dt.Rows[i]["u7"].ToString());
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single where id=" + dt.Rows[i]["id"].ToString());
                }
                my_b.tiaozhuan("删除单页面成功，正在跳转单页面管理页面！", "shop_Single.aspx");
            }
            else
            {

            }
        }
    }




    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = my_b.c_string(this.TextBox3.Text);
        string u4 = my_b.c_string(this.TextBox4.Text);
        string u5 = my_b.c_string(this.FreeTextBox1.Text);
        string u6 = DropDownList1.SelectedItem.Value;
        string u7 = my_b.c_string(this.TextBox5.Text);
        if (u7.IndexOf("/") != 0)
        {
            u7 = "/" + u7;
        }
        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }
        if (type == "edit")
        {
            string file_path = u7.Substring(0, u7.LastIndexOf("/"));
            if (type != "")
            {
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + file_path);
            }
            if (my_b.set_mode() == "静态网站")
            {
                set_html(u7, u6, Request.QueryString["id"].ToString());
            }
            //Response.Write("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "',u7='" + u7 + "' where id=" + Request.QueryString["id"].ToString());
            //Response.End();
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',u5='" + u5 + "',u6='" + u6 + "',u7='" + u7 + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改单页面成功！马上跳转到单页面管理页面！&errurl=" + my_b.tihuan("shop_Single.aspx", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single (u1,u2,u3,u4,u5,u6,u7) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + u5 + "','" + u6 + "','" + u7 + "')");

            dt = my_c.GetTable("select top 1 * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Single order by id desc");
            for (i = 0; i < dt.Rows.Count; i++)
            {
                string file_path = dt.Rows[i]["u7"].ToString().Substring(0, dt.Rows[i]["u7"].ToString().LastIndexOf("/"));

                try
                {
                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + file_path);
                }
                catch
                { }
                if (my_b.set_mode() == "静态网站")
                {
                    set_html(dt.Rows[i]["u7"].ToString(), dt.Rows[i]["u6"].ToString(), dt.Rows[i]["id"].ToString());
                }
            }

            Response.Redirect("err.aspx?err=增加单页面成功！马上跳转到单页面管理页面！&errurl=" + my_b.tihuan("shop_Single.aspx", "&", "fzw123") + "");
        }
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        if (u1 == "首页")
        {
            this.TextBox5.Text = "/index.html";
        }
        else
        {
            this.TextBox5.Text = "/" + my_b.hanzi(u1, "_") + ".html";
        }
    }
}
