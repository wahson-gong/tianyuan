<%@ WebHandler Language="C#" Class="UploadHandler" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public class UploadHandler : IHttpHandler
{
    public string kuangao = "";
    my_basic my_b = new my_basic();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        context.Response.AddHeader("Access-Control-Allow-Headers", "content-type");
        context.Response.AddHeader("Access-Control-Max-Age", "30");
        if (context.Request["REQUEST_METHOD"] == "OPTIONS")
        {
            context.Response.End();
        }
        SaveFile();
    }
    private void SaveFile()
    {
        try
        {
            kuangao = my_b.k_cookie("kuangao");
        }
        catch
        {

        }
        string basePath = "./NewFolder1/";
        string name;
        basePath = System.Web.HttpContext.Current.Server.MapPath(basePath);
        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        //if (!System.IO.Directory.Exists(basePath))
        //{
        //    System.IO.Directory.CreateDirectory(basePath);
        //}
        var suffix = files[0].ContentType.Split('/');
        //获取文件格式
        // var _suffix = suffix[1].Equals("jpeg", StringComparison.CurrentCultureIgnoreCase) ? "" : suffix[1];
        var _suffix = suffix[1];

        var fileName = files[0].FileName.ToString();

        //如果不修改文件名，则创建随机文件名
        //if (!string.IsNullOrEmpty(_temp))
        //{
        //    name = _temp;
        //}
        //else
        //{
        //    Random rand = new Random(24 * (int)DateTime.Now.Ticks);
        //    name = rand.Next() + "." + _suffix;
        //}
        //文件保存
        //Random rand = new Random(24 * (int)DateTime.Now.Ticks);
        //name = rand.Next() + "." + _suffix;
        //var full = basePath + name;
        //files[0].SaveAs(full);
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
        string url = "/upfile/Upload/" + tt1 + "/";
        Directory.CreateDirectory(System.Web.HttpContext.Current.Request.MapPath(url));
        string fileExtension = "";
        if (fileName != "")
        {

            fileExtension = System.IO.Path.GetExtension(fileName);//获取扩展名

            if (fileExtension== "")
            {
                fileName = d1 + Num1.ToString() + ".jpg";
            }
            else
            {
                fileName = d1 + Num1.ToString() + fileExtension;
            }
  


            //注意：可能要修改你的文件夹的匿名写入权限。
            files[0].SaveAs(System.Web.HttpContext.Current.Request.MapPath(url) + fileName);
        }
        string imgurl = url + fileName;
        if (kuangao != "")
        {
            imgurl = my_b.set_onepic_size(url + fileName, kuangao);
        }
        name = "{\"url\":\"" + imgurl + "\",\"filename\":\"" + fileName + "\"}";
        System.Web.HttpContext.Current.Response.Write(name);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}