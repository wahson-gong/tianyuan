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
using System.Drawing;
using System.Drawing.Imaging;
public partial class admin_file_manage_main : System.Web.UI.Page
{
    public int i = 0;
    public string file_path = "";
    public string histroy_file_path = "";
    public int file_int = 1;
    public string getfile(string g1)
    {
        return Directory.GetCreationTime(g1).ToString();

    }
    public string GetDirectorytyp(string g1)
    {
        try
        {
            string[] getfile = Directory.GetFiles(g1);
            return "<img src='images/dir.gif' style='float:left'>" + "<a href='file_manage_main.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
        }
        catch
        {
            string file_name = "";
            try
            {
                file_name = g1.Substring(g1.LastIndexOf(".")).ToLower();
            }
            catch
            {

            }
            if (file_name == ".jpg" || file_name == ".jepg")
            {
                return "<img src='images/jpg.gif' style='float:left'>" + "<a href='" + g1 + "' target='_blank'>" + g1 + "</a>";
            }
            if (file_name == ".bmp" || file_name == ".png" || file_name == ".pwd")
            {
                return "<img src='images/image.gif' style='float:left'>" + "<a href='" + g1 + "' target='_blank'>" + g1 + "</a>";
            }

            else if (file_name == ".gif")
            {
                return "<img src='images/gif.gif' style='float:left'>" + "<a href='" + g1 + "' target='_blank'>" + g1 + "</a>";
            }
            else if (file_name == ".txt")
            {
                return "<img src='images/txt.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".php")
            {
                return "<img src='images/php.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".css")
            {
                return "<img src='images/css.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".js")
            {
                return "<img src='images/js.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".cs")
            {
                return "<img src='images/cs.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".aspx")
            {
                return "<img src='images/aspx.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".ascx")
            {
                return "<img src='images/ascx.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".asax")
            {
                return "<img src='images/asax.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".dll" || file_name == ".db")
            {
                return "<img src='images/dll.gif' style='float:left'>" + g1;
            }
            else if (file_name == ".config")
            {
                return "<img src='images/Config.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".exe")
            {
                return "<img src='images/exe.gif' style='float:left'>" + "<a href='file_manage_main.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".swf")
            {
                return "<img src='images/file_swf.gif' style='float:left'>" + "<a href='file_manage_main.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".xml")
            {
                return "<img src='images/xml.gif' style='float:left'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".zip")
            {
                return "<img src='images/zip.gif' style='float:left'>" + "<a href='" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".rar")
            {
                return "<img src='images/rar.gif' style='float:left'>" + "<a href='" + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".avi" || file_name == ".wmv" || file_name == ".flv" || file_name == ".mp3" || file_name == ".mp4")
            {
                return "<img src='images/wmv.gif' style='float:left'>" + "<a href=' " + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".rm" || file_name == ".rmvb")
            {
                return "<img src='images/rm.gif' style='float:left'>" + "<a href=' " + g1 + "'>" + g1 + "</a>";
            }
            else if (file_name == ".htm" || file_name == ".html" || file_name == ".shtml" || file_name == ".xhtml")
            {
                return "<img src='images/htm.gif' style='float:left; height:40px'>" + "<a href='file_manage_view.aspx?file_path=" + g1 + "'>" + g1 + "</a>";
            }
            else
            {
                return "<img src='images/m.gif' style='float:left'>" + g1;
            }

        }
    }
    public static long GetDirectoryLength(string dirPath)
    {
        try
        {
            string[] getfile = Directory.GetFiles(dirPath);
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        catch
        {
            //System.IO.FileInfo f = new FileInfo("c:\\123.txt");
            //MessageBox.Show(f.Length.ToString());
            FileInfo f = new FileInfo(dirPath);
            return long.Parse(f.Length.ToString());
        }



    }

    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
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
            {

            }
            if (type == "del")
            {
                try
                {
                    string[] getfile = Directory.GetFiles(Request.QueryString["file_path"].ToString());
                    Response.Redirect("err.aspx?err=不可以直接删除文件夹，请先删除文件夹中的文件！&errurl=" + my_b.tihuan("file_manage_main.aspx", "&", "fzw123") + "");
                }
                catch
                {
                    File.Delete(Request.QueryString["file_path"].ToString());
                    Response.Redirect("err.aspx?err=删除文件成功！&errurl=" + my_b.tihuan("file_manage_main.aspx", "&", "fzw123") + "");
                }

            }
            else if (type == "yasuo")
            {
                try
                {
                    string[] getfile = Directory.GetFiles(Request.QueryString["file_path"].ToString());
                    Response.Redirect("err.aspx?err=不可以直接删除文件夹，请先删除文件夹中的文件！&errurl=" + my_b.tihuan("file_manage_main.aspx", "&", "fzw123") + "");
                }
                catch
                {
                    string g1 = Request.QueryString["file_path"].ToString();
                    string file_name = g1.Substring(g1.LastIndexOf("."));
                    if (file_name == ".jpg" || file_name == ".jepg" || file_name == ".gif" || file_name == ".png" || file_name == ".bmp")
                    {
                        if (File.Exists(Server.MapPath("aa.jpg")))
                        {
                            File.Delete(Server.MapPath("aa.jpg"));
                        }
                        if (File.Exists(Server.MapPath("aa.gif")))
                        {
                            File.Delete(Server.MapPath("aa.gif"));
                        }
                        File.Move(g1, Server.MapPath("aa.jpg"));

                        Bitmap bim = new Bitmap(Server.MapPath("aa.jpg"));
                        bim.Save(Server.MapPath("aa.gif"), ImageFormat.Gif);

                        //File.Delete(g1);
                        Bitmap bim1 = new Bitmap(Server.MapPath("aa.gif"));

                        bim1.Save(g1, ImageFormat.Jpeg);
                        //File.Delete(g1);
                        Response.Clear();
                    }
                    else
                    {
                        Response.Redirect("err.aspx?err=压缩图片格式不正确！只可以压缩jpg jpeg gif png bmp格式图片&errurl=" + my_b.tihuan("file_manage_main.aspx", "&", "fzw123") + "");
                    }
                    Response.Redirect("err.aspx?err=压缩图片成功，本操作不可以恢复！&errurl=" + my_b.tihuan(Request.Path.ToString(), "&", "fzw123") + "");
                }
            }
            else
            {
                //获取目录下所有文件
                // string[] aa = Directory.GetFiles(Server.MapPath("../"));
                //获取目录下所有文件夹
                // string[] aa = Directory.GetDirectories(Server.MapPath("../"));
                file_path = Request.PhysicalApplicationPath.ToString();
                try
                {
                    file_path = Request.QueryString["file_path"].ToString();
                }
                catch
                {

                }
                if (file_path.ToLower() + @"\" == Request.PhysicalApplicationPath.ToLower())
                {
                    histroy_file_path = file_path;
                }
                else
                {
                    histroy_file_path = file_path.Substring(0, file_path.LastIndexOf(@"\"));
                }
                //Request.PhysicalApplicationPath
                string[] getpath = Directory.GetDirectories(file_path);
                string[] getfile = Directory.GetFiles(file_path);

                //for (int i = 0; i < getfile.Length; i++)
                //{
                //    try
                //    {
                //        getpath[i] = getfile[i].ToString();
                //    }
                //    catch
                //    { }
                //}
                //string t1 = "";
                //for (int i = 0; i < getpath.Length; i++)
                //{
                //    if (t1 == "")
                //    {
                //        t1 = "@\"" + getpath[i].ToString() + "\"";
                //    }
                //    else
                //    {
                //        t1 = t1 + "," + "@\"" + getpath[i].ToString() + "\"";
                //    }
                //   // Response.Write(getpath[i].ToString() + "<br>");
                //}
                ArrayList myArrList = new ArrayList();//不用指出数组的大小,而且每个元素可以是任意数据类型;

                for (int i = 0; i < getfile.Length; i++)
                {

                    myArrList.Insert(0, getfile[i].ToString());

                }
                for (int i = 0; i < getpath.Length; i++)
                {

                    myArrList.Insert(0, getpath[i].ToString());

                }

                //for (int i = 0; i < getfile.Length; i++)
                //{
                //    if (t1 == "")
                //    {
                //        t1 = "" + getfile.[i].ToString() + "";
                //    }
                //    else
                //    {
                //        t1 = t1 + "," + "" + getfile.[i].ToString() + "";
                //    }
                //}
                // Response.Write(t1);
                // string[] aa = new string[] { t1 };
                Repeater1.DataSource = myArrList;
                Repeater1.DataBind();
                // Repeater1.Items[].DataItem="dfgdf";
            }

        }
    }
}
