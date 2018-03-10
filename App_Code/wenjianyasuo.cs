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
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Net;

/// <summary>
/// wenjianyasuo 的摘要说明
/// </summary>
public class wenjianyasuo
{
    #region 压缩
    /// <summary>
    ///压缩文件
    /// </summary>
    /// <param name="filePath">需要被压缩文件的路径</param>
    public string FileCompression(string filePath)
    {
       string fileName = System.IO.Path.GetFileName(HttpContext.Current.Server.MapPath(filePath));
        string fileExtension = System.IO.Path.GetExtension(fileName);
        string yasuoming = filePath.Replace(fileExtension, ".zip");
        //HttpContext.Current.Response.Write(yasuoming);
        //HttpContext.Current.Response.End();
        //压缩  
        //创建读取流  
        using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(filePath), FileMode.Open, FileAccess.Read))
        {
            //创建写入流  
            using (FileStream save = new FileStream(HttpContext.Current.Server.MapPath(yasuoming), FileMode.Create, FileAccess.Write))
            {
                //创建包含写入流的压缩流  
                using (GZipStream gs = new GZipStream(save, CompressionMode.Compress))
                {
                    //创建byte[]数组中转数据  
                    byte[] b = new byte[1024 * 1024];
                    int count = 0;
                    //循环将读取流中数据写入到byte[]数组中  
                    while ((count = fs.Read(b, 0, b.Length)) > 0)
                    {
                        //将byte[]数组中数据写入到压缩流  
                        gs.Write(b, 0, b.Length);
                    }
                }
            }
        }
        //  
        try
        {
           File.Delete(HttpContext.Current.Server.MapPath(filePath));
        }
        catch { }
        return yasuoming;
    }
    #endregion
    #region 解压
    public string fileDeCompression(string filePath)
    {
        string fileName = System.IO.Path.GetFileName(HttpContext.Current.Server.MapPath(filePath));
        string fileExtension = System.IO.Path.GetExtension(fileName);
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        string jieyaming = "/upfile/linshi/"+ tt1 + "/";
        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(jieyaming));
        jieyaming = jieyaming + fileName.Replace(fileExtension, ".wav");
        using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(filePath), FileMode.Open, FileAccess.Read))
        {
            //目录文件写入流  
            using (FileStream save = new FileStream(HttpContext.Current.Server.MapPath(jieyaming), FileMode.Create, FileAccess.Write))
            {
                //创建包含压缩文件流的GZipStream流  
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Decompress, true))
                {
                    //创建byte[]数组中转数据  
                    byte[] buf = new byte[1024];
                    int len;
                    //循环将解压流中数据写入到byte[]数组中  
                    while ((len = zipStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        //向目标文件流写入byte[]中转数组  
                        save.Write(buf, 0, len);
                    }
                }
            }
        }
        return jieyaming;
    }
    #endregion
}