using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Diagnostics;
using System.IO;


namespace web.App_Code
{
    public class operateMethod
    {
        public operateMethod()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        //获取转换工具路径
        public static string ffmpegtool = ConfigurationManager.AppSettings["ffmpeg"];
        //获取视频的文件夹名
        
        //文件图片大小
        public static string sizeOfImg = ConfigurationManager.AppSettings["imgSize"];
        //文件大小
        public static string widthOfFile = ConfigurationManager.AppSettings["widthSize"];
        public static string heightOfFile = ConfigurationManager.AppSettings["heightSize"];


   
        // 显示视频
        public static string GetFlashText(string url)
        {
            url = "player.swf?fileName=" + url;
            string str = "<object classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' width='452' height='360'  id='index' name='index'><param name='allowScriptAccess' value='always' /><param name='movie' value='" +
                url + "'><embed src='" +
                url + "' id='index1' name='index1' type='application/x-shockwave-flash' swLiveConnect=true allowScriptAccess='always' width='452' height='360'></embed></object>";
            return str;
        }


        //截取字符串
        public static string interceptStr(string str, int len)
        {
            if (str.Length > len)
            {
                str = str.Substring(0, len) + "...";
            }
            return str;
        }

        /// <summary>
        /// 过滤HTML字符
        /// </summary>
        /// <param name="str">传入需要过滤的字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string filtrateHtml(string str)
        {
            str = str.Trim();
            str = str.Replace("'", "&quot;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        /// 回复HTML字符
        /// </summary>
        /// <param name="str">传入需要回复的字符串</param>
        /// <returns>返回回复后的字符串</returns>
        public static string resumeHtml(string str)
        {
            str = str.Trim();
            str = str.Replace("&quot;", "'");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("<br>", "\n");
            return str;
        }



        /// <summary>
        /// 对视频进行图片截取
        /// </summary>
        /// <param name="fileName">需要截取图片的视频路径</param>
        /// <param name="imgFile">截取图片后保存的图片路径</param>

        public static void catchImg(string fileName, string imgFile)
        {
            //获取截图工具路径
            string ffmpeg = System.Web.HttpContext.Current.Server.MapPath("/") + ffmpegtool;
            //获取截图后保存的路径
            string flv_img = imgFile;
            //获取截取图片的大小
            string FlvImgSize = sizeOfImg;
            Process pss = new Process();
            //设置启动程序的路径
            pss.StartInfo.FileName = ffmpeg;
          
            pss.StartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 50 -vframes 1  -s   " + FlvImgSize + "   " + flv_img;
            //启动进程
            pss.Start();

        }
    }
}
