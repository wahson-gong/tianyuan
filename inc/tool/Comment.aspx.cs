using System;
using System.Collections;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using web.App_Code;

public partial class Comment : System.Web.UI.Page
{
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //try
            //{
            //    string fileurl = Request.QueryString["fileurl"].ToString();
            //    string fileExtension = fileurl.Substring(fileurl.LastIndexOf("."));

            //    System.Drawing.Image img = FrameGrabber.GetFrameFromVideo(Server.MapPath(fileurl), 0.2d);

            //    string picname = Server.MapPath(fileurl.Replace(fileExtension, ".jpg"));

            //    img.Save(picname);
            //    Response.Write(fileurl.Replace(fileExtension, ".jpg"));
            //}
            //catch { }
            string fileurl = Request.QueryString["fileurl"].ToString();
            string fileExtension = fileurl.Substring(fileurl.LastIndexOf("."));

            string picname = fileurl.Replace(fileExtension, ".jpg");
           
            //调用公共类中的catchImg方法截取视频图片
            operateMethod.catchImg(Server.MapPath(fileurl), Server.MapPath(picname));

            ////调用自定义insertVideoInfo方法将视频的信息保存到数据库中
            //insertVideoInfo(playFile, imgFile);
           Response.Write(picname);


        }
    }
}
