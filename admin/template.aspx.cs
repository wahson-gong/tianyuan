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

public partial class admin_template : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string Getfile_name(string g1)
    {
        return g1.Substring(g1.LastIndexOf(@"\")+1);
    }
    public string set_border(string g1)
    {
        if (g1 == ConfigurationSettings.AppSettings["template"].ToString())
        {
            return "border:5px solid red";
        }
        else
        {
            return "border:5px solid #e5e4e4";
        }
    }
    public void DeleteFolder(string dir)
    {
        if (Directory.Exists(dir)) //如果存在这个文件夹删除之    
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                    File.Delete(d); //直接删除其中的文件                           
                else
                    DeleteFolder(d); //递归删除子文件夹    
            }
            Directory.Delete(dir, true); //删除已空文件夹                    
        }
    }  

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }

            if (type == "edit")
            {
                my_b.set_xml("template", Getfile_name(Request.QueryString["file_path"].ToString()));

                Response.Redirect("err.aspx?err=重新设置模板成功！&errurl=" + my_b.tihuan("template.aspx", "&", "fzw123") + "");
            }
            else if (type == "del")
            {
                DeleteFolder(Request.QueryString["file_path"].ToString());
                if (ConfigurationSettings.AppSettings["template"].ToString() != "default")
                {
                    my_b.set_xml("template", "default");
                }
                Response.Redirect("err.aspx?err=删除模板成功！&errurl=" + my_b.tihuan("template.aspx", "&", "fzw123") + "");
            }
            else
            {
                string[] getfile = Directory.GetDirectories(HttpContext.Current.Request.PhysicalApplicationPath+"/template/");
                ArrayList myArrList = new ArrayList();//不用指出数组的大小,而且每个元素可以是任意数据类型;
                for (int i = 0; i < getfile.Length; i++)
                {

                    myArrList.Insert(0, getfile[i].ToString());

                }
                Repeater1.DataSource = myArrList;
                Repeater1.DataBind();
            }


        }
    }
  
}
