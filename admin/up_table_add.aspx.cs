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
public partial class admin_up_table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {
                dt = my_c.GetTable("select * from up_table where id=" + Request.QueryString["id"].ToString());
                this.TextBox1.Text = dt.Rows[0]["up_path"].ToString();
                this.TextBox2.Text = dt.Rows[0]["contents"].ToString();

                if (dt.Rows[0]["file_path"].ToString() != "")
                {
                    Label1.Text = "<a href='" + dt.Rows[0]["file_path"].ToString() + "' target=_blank>文件已上传</a>";
                }


            }
            else if (type == "del")
            {
                DataTable dt = my_c.GetTable("select * from up_table where id in (" + Request.QueryString["id"].ToString() + ")");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    my_c.genxin("delete from up_table where id in(" + dt.Rows[i]["id"].ToString() + ")");
                    my_b.del_pic(dt.Rows[i]["file_path"].ToString());
                }

                Response.Redirect("err.aspx?err=删除在线更新成功，正在在线更新管理页面！&errurl=" + my_b.tihuan("up_table.aspx", "&", "fzw123") + "");
            }
            else
            {
                
            }
        }
    }
    public string shangchuang(FileUpload File1, string file_Extension)
    {
        string file_name = "";
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        string g1 = Path.GetFileName(File1.PostedFile.FileName).ToString();
        if (g1 == "")
        {
            return "-1";
        }
        file_name = g1.Substring(g1.LastIndexOf("."));
        file_Extension = file_Extension.ToLower();
        file_name = file_name.ToLower();
        if (File1.PostedFile.ContentLength > 0 && File1.PostedFile.ContentLength < (1024 * 1024 * 20))
        {
            if (file_Extension.IndexOf(file_name) > -1 || file_Extension == "*")
            {

                DateTime dy = DateTime.Now;
                string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                file_name = d1 + Num1.ToString() + file_name;
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile/up_data/");
                string filepath = HttpContext.Current.Request.PhysicalApplicationPath + "upfile//up_data//" + file_name + ".rar";
                File1.PostedFile.SaveAs(filepath);
                return my_b.get_ApplicationPath() + "/upfile/up_data/" + file_name + ".rar";
            }
            else
            {
                return "1";
                //文件类型不正确
            }
        }
        else
        {
            return "2";
            //文件太大或者不存在
        }
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        string up_path = my_b.c_string(this.TextBox1.Text);
        string contents = my_b.c_string(this.TextBox2.Text);
        string file_path = shangchuang(FileUpload1, "*");

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            my_c.genxin("update up_table set file_path='" + file_path + "',up_path='" + up_path + "',contents='" + contents + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改在线更新成功，正在在线更新管理页面！&errurl=" + my_b.tihuan("up_table.aspx", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into up_table (contents,up_path,file_path) values('" + contents + "','" + up_path + "','" + file_path + "')");
            Response.Redirect("err.aspx?err=增加在线更新成功，正在在线更新管理页面！&errurl=" + my_b.tihuan("up_table.aspx", "&", "fzw123") + "");
        }
    }
}
