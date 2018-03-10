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
using System.Text.RegularExpressions;
public partial class admin_file_manage_view : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    //public void a1(string g1, string g2, string g3)
    //{
    //    //StreamWriter filew1 = new StreamWriter("", false, System.Text.Encoding.UTF8);
    //    //filew1.Write(getWebresourceFile2(ConfigurationSettings.AppSettings["sql_url"] + g2 + ".aspx"));
    //    //filew1.Close();
    //    //Response.Write(g3 + "生成成功<br>");
    //    File.ReadAllText(

    //}
    public string get_string_UTF8(string g1)
    {


        byte[] Res = Encoding.UTF8.GetBytes(g1);
        byte[] Tag = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("Gb2312"), Res);
        return Encoding.GetEncoding("Gb2312").GetString(Tag);


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string file_path = Request.QueryString["file_path"].ToString();
            string g1 = file_path;
            TextBox1.Text = g1.Substring(0, g1.LastIndexOf(@"\") + 1);
            TextBox2.Text = g1.Substring(g1.LastIndexOf(@"\") + 1).ToLower();
            this.TextBox3.Text = File.ReadAllText(file_path,Encoding.UTF8);

        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
  string file_path = Request.QueryString["file_path"].ToString();
        string file_content = this.TextBox3.Text;
        StreamWriter filew = new StreamWriter(file_path, false, System.Text.Encoding.UTF8);
        filew.Write(file_content);
        filew.Close();
        // File.WriteAllText(file_path, file_content);
        File.Move(file_path, this.TextBox1.Text + TextBox2.Text);
        string dir_path = "";
        try
        {
            dir_path = Request.QueryString["dir_path"].ToString();
        }
        catch
        { }
        if (dir_path == "")
        {
            Response.Redirect("err.aspx?err=修改" + file_path + "成功，正在跳转到WEBFTP页面！&errurl=" + my_b.tihuan("file_manage_main.aspx", "&", "fzw123") + "");
        }
        else
        {
            Response.Redirect("err.aspx?err=修改" + file_path + "成功，正在跳转到WEBFTP页面！&errurl=" + my_b.tihuan("file_manage_main.aspx?file_path=" + Request.QueryString["dir_path"].ToString(), "&", "fzw123") + "");
        }
      
        
    }
}
