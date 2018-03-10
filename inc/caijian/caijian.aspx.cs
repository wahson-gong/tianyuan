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
public partial class admin_pic_Cut : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string cc1 = "10";
    public string cc2 = "10";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string pic_name = "http://files.jb51.net/demoimg/201003/203dda9bbc2fab3b6e068c1a.jpg";
            try
            {
                pic_name=Request.QueryString["pic_name"].ToString();
            }
            catch { }
            Image1.ImageUrl = pic_name;

            string cc = "200*200";
            try
            {
                cc = Request.QueryString["cc"].ToString();
            }
            catch { }
            try
            {
                cc1 = cc.Split('*')[0].ToString();
            }
            catch
            { }
            try
            {
                cc2 = cc.Split('*')[1].ToString();
            }
            catch
            { }
        }
    }
    protected void submit_ServerClick(object sender, EventArgs e)
    {
        string cc = "200*200";
        try
        {
            cc = Request.QueryString["cc"].ToString();
        }
        catch { }
        try
        {
            cc1 = cc.Split('*')[0].ToString();
        }
        catch
        { }
        try
        {
            cc2 = cc.Split('*')[1].ToString();
        }
        catch
        { }

        string s_pic = HttpContext.Current.Request.PhysicalApplicationPath+my_b.Downloads(this.Image1.ImageUrl);
        string[] a_img_pos = Request["img_pos"].Split(',');
        string[] a_cut_pos = Request["cut_pos"].Split(',');
        int imageWidth = Int32.Parse(a_img_pos[0]);
        int imageHeight = Int32.Parse(a_img_pos[1]);

        int cutTop = Int32.Parse(a_cut_pos[1]) - 5;
        int cutLeft = Int32.Parse(a_cut_pos[0]);

        int dropWidth = int.Parse(cc1);
        int dropHeight = int.Parse(cc2);
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        string d1 = dy.ToString().Replace(" ", "");
        d1 = d1.Replace(":", "");
        d1 = d1.Replace("-", "");
        Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath+"/upfile/Upload/pic" + tt1 + "/");
        string file_path = my_b.get_ApplicationPath() + "/upfile/Upload/pic" + tt1 + "/" + d1 + Num1.ToString() + ".jpg";
        ZoomImage.SaveCutPic(s_pic, HttpContext.Current.Request.PhysicalApplicationPath+file_path, 0, 0, dropWidth, dropHeight, cutLeft, cutTop, imageWidth,imageHeight);
       // Response.Redirect(file_path);
       Response.Write("<script language='javascript'>window.opener.document.all('" + Request.QueryString["editname"].ToString() + "').value='" + file_path + "';alert('裁剪成功');window.close();</script>");
    }
}
