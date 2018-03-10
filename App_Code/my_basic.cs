using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using System.Web.SessionState;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.Exceptions;
using System.Drawing.Drawing2D;
/// <summary>
///my_basic 的摘要说明
/// </summary>
public class my_basic
{
    #region 发送内容参数替换
    public string fasong_neirong_url(string content)
    {
        Regex reg = new Regex("{.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(content);

        foreach (Match match in matches)
        {
            string t1 = match.ToString().Replace("{", "").Replace("}", "");
            if (t1 == "u1")
            {


            }
            else
            {
                #region 默认参数名
                try
                {
                    content = content.Replace(match.ToString(), HttpContext.Current.Request.QueryString[t1].ToString());
                }
                catch { }
                #endregion
            }

        }

        return content;
    }
    #endregion
    #region 给文件加地址
    public string set_neirong_url(string g1)
    {
        string neirong = g1;
        Regex reg = new Regex(@"href="".*?""|src="".*?""|background-image:url[\s\S]*?\)|background="".*?""|background=.*?|background:url[\s\S]*?\)", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {
            string url_string = match.ToString();
            url_string = url_string.Replace("href=", "").Replace("src=", "").Replace("\"", "").Replace("background-image:url(", "").Replace(")", "").Replace("background=", "").Replace("background:url(", "");
            if (url_string.IndexOf("http://") == -1 && url_string.IndexOf("ftp://") == -1 && url_string.IndexOf("https://") == -1 && url_string.IndexOf("javascript:") == -1 && url_string.IndexOf("tel:") == -1 && url_string.IndexOf("mailto:") == -1 && url_string.IndexOf("mqqwpa:") == -1 && url_string.IndexOf("tencent://") == -1 && url_string.IndexOf("sms:") == -1)
            {
                neirong = neirong.Replace(match.ToString(), match.ToString().Replace(url_string, get_Domain() + url_string));
            }
        }
        return neirong;
    }
    #endregion
    #region 参数字段验证
    public void canshu_ziduan(string ziduan,string type)
    {
        string sta = "";
        if (type == "数字")
        {
            try
            {
                int tt = int.Parse(ziduan);
            }
            catch{
                sta = "yes";
            }
        }
        else
        {

        }
        if (sta != "")
        {
            HttpContext.Current.Response.Write("参数有误");
            HttpContext.Current.Response.End();
        }
       
    }
    #endregion
    #region 判断手机号
    public string get_shouji(string neirong)
    {
        string shouji = "";
        Regex reg2 = new Regex(@"[1]+\d{10}", RegexOptions.Singleline);
        MatchCollection matches = reg2.Matches(neirong);
        foreach (Match match in matches)
        {

            if (shouji == "")
            {
                shouji = match.ToString();
            }
            else
            {
                shouji = shouji+"," +match.ToString();
            }
        }
       return shouji;
    }
    #endregion
    #region 获取用户名
    public string get_yonghuming()
    {
        
        string yonghuming = "";
        try
        {
            yonghuming = k_cookie("user_name");
        }
        catch { }
        if (yonghuming == "")
        {
            try
            {
                yonghuming = c_string(HttpContext.Current.Request["yonghuming"].ToString());
            }
            catch { }
        }

        if (yonghuming != "")
        {
            DataTable sl_yuangong = my_c.GetTable("select * from sl_yuangong where (yonghuming='" + yonghuming + "' )");

            if (sl_yuangong.Rows.Count == 0)
            {
                sl_yuangong = my_c.GetTable("select * from sl_yuangong where shoujihaoma ='" + yonghuming + "'");
            }
            if (sl_yuangong.Rows.Count > 0)
            {
                yonghuming = sl_yuangong.Rows[0]["id"].ToString();
            }
            else
            {
                yonghuming = "0";
            }
        }
        else
        {
            yonghuming = "0";
        }
        return yonghuming;
    }
    #endregion
    #region 生成默认头像
    /// <summary>
    /// 生成文字图片
    /// </summary>
    /// <param name="text"></param>
    /// <param name="isBold"></param>
    /// <param name="fontSize"></param>
    public string CreateImage(string text, bool isBold, int fontSize)
    {
        Regex regex = new Regex("^[\u4E00-\u9FA5]+$");
      
        if (regex.Match(text).ToString() == "")
        {
            int zichang = text.Length;
            if (zichang > 5)
            {
                text = text.Substring(0, 5);
            }
        }
        else
        {
            if (text.Length > 2)
            {
                text = text.Substring(text.Length - 2, 2);
            }
        }
        //HttpContext.Current.Response.Write(text);
        //HttpContext.Current.Response.End();
      
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
        Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile/touxiang/" + tt1 + "/");
        string filepath = "/upfile/touxiang/" + tt1 + "/" + d1 + Num1.ToString() + ".jpg";
        string newilt = HttpContext.Current.Request.PhysicalApplicationPath + filepath;

        int wid = 50;
        int high = 50;
        Font font;
        if (isBold)
        {
            font = new Font("黑体", fontSize, FontStyle.Bold);

        }
        else
        {
            font = new Font("黑体", fontSize, FontStyle.Regular);

        }
        //绘笔颜色
        SolidBrush brush = new SolidBrush(Color.White);
        StringFormat format = new StringFormat(StringFormatFlags.NoClip);
        Bitmap image = new Bitmap(wid, high);
        Graphics g = Graphics.FromImage(image);
        SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高
        int width = (int)(sizef.Width + 1);
        int height = (int)(sizef.Height + 1);
        image.Dispose();
        image = new Bitmap(wid, high);
        g = Graphics.FromImage(image);
        #region 随机颜色
         Num1 = r.Next(10);
        if (Num1 == 1)
        {
            g.Clear(Color.Peru);//颜色
        }
        else if (Num1 == 2)
        {
            g.Clear(Color.SlateBlue);//颜色
        }
        else if (Num1 == 3)
        {
            g.Clear(Color.SteelBlue);//颜色
        }
        else if (Num1 == 4)
        {
            g.Clear(Color.Tomato);//颜色
        }
        else if (Num1 == 5)
        {
            g.Clear(Color.Maroon);//颜色
        }
        else if (Num1 == 6)
        {
            g.Clear(Color.RoyalBlue);//颜色
        }
        else if (Num1 == 7)
        {
            g.Clear(Color.DimGray);//颜色
        }
        else if (Num1 == 8)
        {
            g.Clear(Color.YellowGreen);//颜色
        }
        else if (Num1 == 9)
        {
            g.Clear(Color.HotPink);//颜色
        }
        else
        {
            g.Clear(Color.MediumPurple);//颜色
        }

        #endregion
        float X = (wid - width) / 2;
        float Y = ((high - height) / 2) + 3;
        RectangleF rect = new RectangleF(X, Y, width, height);
        //绘制图片
        g.DrawString(text, font, brush, rect);
        //释放对象
        g.Dispose();

        image.Save(newilt, ImageFormat.Jpeg);
        return filepath;
    }
    #endregion
    #region 记录日志
    public void logadd()
    {
        string get_str = HttpContext.Current.Request.QueryString.ToString();
        string post_str = HttpContext.Current.Request.Form.ToString();
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        if (File.Exists(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt")))
        {
            //File.WriteAllText(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt"), File.ReadAllText(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt")) + "\r\n\r\n\r\n" + dy.ToString() + "：" + "GET："+get_str + "\r\n" + "POST：" + post_str);
        }
        else
        {
           // File.WriteAllText(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt"),  dy.ToString() + "：GET：" + get_str + "\r\nPOST：" + post_str);
        }
       
    }
    public void sqllogadd(string sql)
    {

        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        if (File.Exists(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt")))
        {
           // File.WriteAllText(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt"), File.ReadAllText(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt")) + "\r\n\r\n\r\n" + dy.ToString() + "：" + "SQL：" + sql );
        }
        else
        {
           // File.WriteAllText(HttpContext.Current.Server.MapPath("/log/" + tt1 + ".txt"), dy.ToString() + "：SQL" + sql);
        }

    }
    #endregion
    #region 关于生成数字的问题
    public string set_shuzi(string txt_value)
    {
        string[] type = txt_value.Split('|');
        string leixing = "";

        if (type.Length > 0)
        {
            leixing = type[0].ToString();
        }

        if (leixing == "会员卡")
        {
            return get_huiyuanka();
        }
        else if (leixing == "兑换卡")
        {
            return get_duihuanka();
        }
        else if (leixing == "随机卡")
        {
            return suijishu(int.Parse(type[1].ToString()));
        }
        else
        {
            return get_bianhao();
        }

    }
    #region 生成获取员工工号
    public string get_gonghao()
    {
        int count_id = 1;
        try
        {
            count_id = count_id + int.Parse(my_c.GetTable(" SELECT count(id) as count_id FROM sl_yuangong").Rows[0]["count_id"].ToString());
        }
        catch
        { }
        string bianhao = count_id.ToString("d6");

        if (bianhao == "")
        {
            bianhao = "000001";
        }
       
        return bianhao;
    }
    #endregion
    #region 生成获取订单编号
    public string get_bianhao()
    {
        DateTime dy = DateTime.Now;
        string tt1 = dy.Month.ToString() + dy.Day.ToString();
        int count_id = int.Parse(my_c.GetTable(" SELECT count(id) as count_id FROM sl_bianhao").Rows[0]["count_id"].ToString());
        string bianhao = count_id.ToString("d5");

        if (bianhao == "")
        {
            bianhao = "00001";
        }
        bianhao = tt1 + bianhao;

        if (my_c.GetTable("select id from sl_bianhao where bianhao='" + bianhao + "'").Rows.Count > 0)
        {
            get_bianhao();
        }
        else
        {

            my_c.genxin("insert into sl_bianhao(bianhao) values('" + bianhao + "')");
            return bianhao;
        }
        return "";
    }
    #endregion
    #region 生成会员卡
    public string get_huiyuanka()
    {
        DataTable dt = my_c.GetTable("select * from sl_Parameter where id=313");
        int qian_ = 1;
        int hou_ = 100;
        if (dt.Rows.Count > 0)
        {
            qian_ = int.Parse(dt.Rows[0]["u2"].ToString().Split('-')[0].ToString());
            hou_ = int.Parse(dt.Rows[0]["u2"].ToString().Split('-')[1].ToString());
        }

        int Num1 = r.Next(qian_, hou_);
        string num_string = Num1.ToString();

        for (int i = num_string.Length; i < 6; i++)
        {
            num_string = "0" + num_string;
        }

        if (my_c.GetTable("select id from sl_bianhao where bianhao='" + num_string + "'").Rows.Count > 0)
        {
            get_huiyuanka();
        }
        else
        {

            return num_string;
        }
        return "";
    }
    #endregion
    #region 获取兑换卡
    Random r = new Random();
    public string get_duihuanka()
    {
        DateTime dy = DateTime.Now;

        int Num1 = r.Next(100000, 999999);
        int Num2 = r.Next(100000, 999999);
        //string num_string = Num1.ToString() + dy.Year.ToString().Substring(2,2) + Num2.ToString() + dy.Day.ToString() ;


        string num_string = Num1.ToString() + Num2.ToString();
        //HttpContext.Current.Response.Write(num_string);
        //HttpContext.Current.Response.End();
        // my_c.genxin("insert into sl_bianhao(bianhao) values('" + num_string + "')");
        if (my_c.GetTable("select id from sl_bianhao where bianhao='" + num_string + "'").Rows.Count > 0)
        {
            get_duihuanka();
        }
        else
        {
            my_c.genxin("insert into sl_bianhao (bianhao) values('" + num_string + "')");
            return num_string;
        }
        get_duihuanka();
        return "";
    }
    public string get_suiji(int weishu)
    {
        string suijishu = "";
        for (int i = 1; i <= weishu; i++)
        {
            int Num1 = r.Next(0, 9);
            suijishu = suijishu + Num1.ToString();
        }
        return suijishu;
    }
    #endregion
    #region 生成一个字母和数字组成的随机卡
    private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    public string suijishu(int strLength)
    {
        System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
        Random rd = new Random();
        for (int i = 0; i < strLength; i++)
        {
            newRandom.Append(constant[rd.Next(62)]);
        }

        return newRandom.ToString();
    }
    #endregion
    #endregion

    #region 设置图片水印
    public string shuiyin_list(string g1)
    {

        DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
        if (xml_dt.Rows[0]["u26"].ToString() != "无水印")
        {
            Regex g = new Regex(@"<img.*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            MatchCollection m = g.Matches(g1);

            foreach (Match math in m)
            {
                Regex reg = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Match tupian = reg.Match(math.ToString());
                string imgUrl = tupian.ToString();
                imgUrl = imgUrl.Replace("src", "");
                imgUrl = imgUrl.Replace("\"", "");
                imgUrl = imgUrl.Replace("'", "");
                imgUrl = imgUrl.Replace("=", "");
                imgUrl = imgUrl.Trim();

                string g2 = "";
                if (imgUrl.ToLower().IndexOf("_shuiyin_") == -1)
                {
                    try
                    {
                        DateTime dy = new DateTime();

                        Random r = new Random();
                        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                        string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                        string rDstImgPath = imgUrl.Substring(0, imgUrl.LastIndexOf(".")) + "_shuiyin_" + imgUrl.Substring(imgUrl.LastIndexOf("."));

                        get_shuiyin(imgUrl, HttpContext.Current.Server.MapPath(rDstImgPath));
                        if (File.Exists(HttpContext.Current.Server.MapPath(rDstImgPath)))
                        {
                            g2 = rDstImgPath;
                        }
                        else
                        {
                            g2 = imgUrl;
                        }

                        g1 = g1.Replace(imgUrl, g2);
                        del_pic(imgUrl);
                    }
                    catch
                    {

                    }
                }

            }
        }


        return g1;
    }
    public void get_shuiyin(string rSrcImgPath, string rDstImgPath)
    {
        DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
        if (xml_dt.Rows[0]["u26"].ToString() == "图片水印")
        {
            BuildWatermark(HttpContext.Current.Server.MapPath(rSrcImgPath), HttpContext.Current.Server.MapPath(xml_dt.Rows[0]["u32"].ToString()), "", rDstImgPath, xml_dt);
        }
        else if (xml_dt.Rows[0]["u26"].ToString() == "文字水印")
        {
            AddTextToImg(rSrcImgPath, xml_dt.Rows[0]["u27"].ToString(), rDstImgPath, xml_dt);
        }

    }
    /// <summary>  
    /// Creating a Watermarked Photograph with GDI+ for .NET  
    /// </summary>  
    /// <param name="rSrcImgPath">原始图片的物理路径</param>  
    /// <param name="rMarkImgPath">水印图片的物理路径</param>  
    /// <param name="rMarkText">水印文字（不显示水印文字设为空串）</param>  
    /// <param name="rDstImgPath">输出合成后的图片的物理路径</param>  
    /// @整理: anyrock@mending.cn  
    public void BuildWatermark(string rSrcImgPath, string rMarkImgPath, string rMarkText, string rDstImgPath, DataTable xml_dt)
    {

        //以下（代码）从一个指定文件创建了一个Image 对象，然后为它的 Width 和 Height定义变量。  
        //这些长度待会被用来建立一个以24 bits 每像素的格式作为颜色数据的Bitmap对象。  

        System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(rSrcImgPath);
        int phWidth = imgPhoto.Width;
        int phHeight = imgPhoto.Height;
        Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(72, 72);
        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //这个代码载入水印图片，水印图片已经被保存为一个BMP文件，以绿色(A=0,R=0,G=255,B=0)作为背景颜色。  
        //再一次，会为它的Width 和Height定义一个变量。  
        System.Drawing.Image imgWatermark = new Bitmap(rMarkImgPath);
        int wmWidth = imgWatermark.Width;
        int wmHeight = imgWatermark.Height;
        //这个代码以100%它的原始大小绘制imgPhoto 到Graphics 对象的（x=0,y=0）位置。  
        //以后所有的绘图都将发生在原来照片的顶部。  
        //原图
        grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
        grPhoto.DrawImage(
             imgPhoto,
             new Rectangle(0, 0, phWidth, phHeight),
             0,
             0,
             phWidth,
             phHeight,
             GraphicsUnit.Pixel);
        //为了最大化版权信息的大小，我们将测试7种不同的字体大小来决定我们能为我们的照片宽度使用的可能的最大大小。  
        //为了有效地完成这个，我们将定义一个整型数组，接着遍历这些整型值测量不同大小的版权字符串。  
        //一旦我们决定了可能的最大大小，我们就退出循环，绘制文本  
        int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
        Font crFont = null;
        SizeF crSize = new SizeF();
        for (int i = 0; i < 7; i++)
        {
            crFont = new Font("arial", sizes[i],
                  FontStyle.Bold);
            crSize = grPhoto.MeasureString(rMarkText,
                  crFont);
            if ((ushort)crSize.Width < (ushort)phWidth)
                break;
        }
        //因为所有的照片都有各种各样的高度，所以就决定了从图象底部开始的5%的位置开始。  
        //使用rMarkText字符串的高度来决定绘制字符串合适的Y坐标轴。  
        //通过计算图像的中心来决定X轴，然后定义一个StringFormat 对象，设置StringAlignment 为Center。  
        int yPixlesFromBottom = (int)(phHeight * .05);
        float yPosFromBottom = ((phHeight -
             yPixlesFromBottom) - (crSize.Height / 2));
        float xCenterOfImg = (phWidth / 2);
        StringFormat StrFormat = new StringFormat();
        StrFormat.Alignment = StringAlignment.Center;
        //现在我们已经有了所有所需的位置坐标来使用60%黑色的一个Color(alpha值153)创建一个SolidBrush 。  
        //在偏离右边1像素，底部1像素的合适位置绘制版权字符串。  
        //这段偏离将用来创建阴影效果。使用Brush重复这样一个过程，在前一个绘制的文本顶部绘制同样的文本。  
        SolidBrush semiTransBrush2 =
             new SolidBrush(Color.FromArgb(153, 0, 0, 0));
        grPhoto.DrawString(rMarkText,
             crFont,
             semiTransBrush2,
             new PointF(xCenterOfImg + 1, yPosFromBottom + 1),
             StrFormat);
        SolidBrush semiTransBrush = new SolidBrush(
             Color.FromArgb(153, 255, 255, 255));
        grPhoto.DrawString(rMarkText,
             crFont,
             semiTransBrush,
             new PointF(xCenterOfImg, yPosFromBottom),
             StrFormat);
        //根据前面修改后的照片创建一个Bitmap。把这个Bitmap载入到一个新的Graphic对象。  
        Bitmap bmWatermark = new Bitmap(bmPhoto);
        bmWatermark.SetResolution(
             imgPhoto.HorizontalResolution,
             imgPhoto.VerticalResolution);
        Graphics grWatermark =
             Graphics.FromImage(bmWatermark);
        //通过定义一个ImageAttributes 对象并设置它的两个属性，我们就是实现了两个颜色的处理，以达到半透明的水印效果。  
        //处理水印图象的第一步是把背景图案变为透明的(Alpha=0, R=0, G=0, B=0)。我们使用一个Colormap 和定义一个RemapTable来做这个。  
        //就像前面展示的，我的水印被定义为100%绿色背景，我们将搜到这个颜色，然后取代为透明。  
        ImageAttributes imageAttributes =
             new ImageAttributes();
        ColorMap colorMap = new ColorMap();
        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
        ColorMap[] remapTable = { colorMap };
        //第二个颜色处理用来改变水印的不透明性。  
        //通过应用包含提供了坐标的RGBA空间的5x5矩阵来做这个。  
        //通过设定第四行、第四列为1.0f我们就达到了一个不透明的水平。结果是水印会轻微地显示在图象底下一些。  
        imageAttributes.SetRemapTable(remapTable,
             ColorAdjustType.Bitmap);
        float zuobiao = float.Parse(xml_dt.Rows[0]["u33"].ToString());
        float[][] colorMatrixElements = {
                                                     new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                     new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                     new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                     new float[] {0.0f,  0.0f,  0.0f, zuobiao, 0.0f},//这里
                                                     new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                                };
        ColorMatrix wmColorMatrix = new
             ColorMatrix(colorMatrixElements);
        imageAttributes.SetColorMatrix(wmColorMatrix,
             ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);
        //随着两个颜色处理加入到imageAttributes 对象，我们现在就能在照片右手边上绘制水印了。  
        //我们会偏离10像素到底部，10像素到左边。  
        int markWidth;
        int markHeight;
        //mark比原来的图宽  
        if (phWidth <= wmWidth)
        {
            markWidth = phWidth - 10;
            markHeight = (markWidth * wmHeight) / wmWidth;
        }
        else if (phHeight <= wmHeight)
        {
            markHeight = phHeight - 10;
            markWidth = (markHeight * wmWidth) / wmHeight;
        }
        else
        {
            markWidth = wmWidth;
            markHeight = wmHeight;
        }
        #region 方位
        int yuantu_kuan = phWidth;
        int yuantu_gao = phHeight;
        int shuiyin_kuan = markWidth;
        int shuiyin_gao = wmHeight;
        string fangwei = xml_dt.Rows[0]["u34"].ToString();
        int xPosOfWm = 0;
        int yPosOfWm = 0;
        //Response.Write(phWidth+"||"+ wmWidth);
        //Response.End();

        if (fangwei == "左上")
        {
            xPosOfWm = 0;
            yPosOfWm = 0;
        }
        else if (fangwei == "中上")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan) / 2;
            yPosOfWm = 0;
        }
        else if (fangwei == "右上")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan);
            yPosOfWm = 0;
        }
        else if (fangwei == "中左")
        {
            xPosOfWm = 0;
            yPosOfWm = (yuantu_gao - shuiyin_gao) / 2;
        }
        else if (fangwei == "居中")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan) / 2;
            yPosOfWm = (yuantu_gao - shuiyin_gao) / 2;
        }
        else if (fangwei == "中右")
        {
            xPosOfWm = yuantu_kuan - shuiyin_kuan;
            yPosOfWm = (yuantu_gao - shuiyin_gao) / 2;
        }
        else if (fangwei == "左下")
        {
            xPosOfWm = 0;
            yPosOfWm = yuantu_gao - shuiyin_gao;
        }
        else if (fangwei == "下中")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan) / 2;
            yPosOfWm = yuantu_gao - shuiyin_gao;
        }
        else
        {
            xPosOfWm = yuantu_kuan - shuiyin_kuan;
            yPosOfWm = yuantu_gao - shuiyin_gao;
        }
        #endregion
        grWatermark.DrawImage(imgWatermark,
             new Rectangle(xPosOfWm, yPosOfWm, markWidth,
             markHeight),
             0,
             0,
             wmWidth,
             wmHeight,
             GraphicsUnit.Pixel,
             imageAttributes);
        //最后的步骤将是使用新的Bitmap取代原来的Image。 销毁两个Graphic对象，然后把Image 保存到文件系统。  
        imgPhoto = bmWatermark;
        grPhoto.Dispose();
        grWatermark.Dispose();
        imgPhoto.Save(rDstImgPath, ImageFormat.Jpeg);
        imgPhoto.Dispose();
        imgWatermark.Dispose();
    }
    //using System.Drawing;
    //using System.IO;
    //using System.Drawing.Imaging;

    private void AddTextToImg(string fileName, string text, string rDstImgPath, DataTable xml_dt)
    {
        if (!File.Exists(HttpContext.Current.Server.MapPath(fileName)))
        {
            throw new FileNotFoundException("The file don't exist!");
        }

        if (text == string.Empty)
        {
            return;
        }
        //还需要判断文件类型是否为图像类型，这里就不赘述了

        System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(fileName));
        Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
        Graphics g = Graphics.FromImage(bitmap);

        float fontSize = float.Parse(xml_dt.Rows[0]["u28"].ToString());    //字体大小
        float textWidth = text.Length * fontSize;  //文本的长度
                                                   //下面定义一个矩形区域，以后在这个矩形里画上白底黑字

        float rectWidth = text.Length * (fontSize + fontSize);
        float rectHeight = fontSize + fontSize;
        #region 方位
        int yuantu_kuan = image.Width;
        int yuantu_gao = image.Height;
        int shuiyin_kuan = int.Parse(rectWidth.ToString());
        int shuiyin_gao = int.Parse(rectHeight.ToString());
        string fangwei = xml_dt.Rows[0]["u34"].ToString();
        int xPosOfWm = 0;
        int yPosOfWm = 0;
        //Response.Write(phWidth+"||"+ wmWidth);
        //Response.End();

        if (fangwei == "左上")
        {
            xPosOfWm = 0;
            yPosOfWm = 0;
        }
        else if (fangwei == "中上")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan) / 2;
            yPosOfWm = 0;
        }
        else if (fangwei == "右上")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan);
            yPosOfWm = 0;
        }
        else if (fangwei == "中左")
        {
            xPosOfWm = 0;
            yPosOfWm = (yuantu_gao - shuiyin_gao) / 2;
        }
        else if (fangwei == "居中")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan) / 2;
            yPosOfWm = (yuantu_gao - shuiyin_gao) / 2;
        }
        else if (fangwei == "中右")
        {
            xPosOfWm = yuantu_kuan - shuiyin_kuan;
            yPosOfWm = (yuantu_gao - shuiyin_gao) / 2;
        }
        else if (fangwei == "左下")
        {
            xPosOfWm = 0;
            yPosOfWm = yuantu_gao - shuiyin_gao;
        }
        else if (fangwei == "下中")
        {
            xPosOfWm = (yuantu_kuan - shuiyin_kuan) / 2;
            yPosOfWm = yuantu_gao - shuiyin_gao;
        }
        else
        {
            xPosOfWm = yuantu_kuan - shuiyin_kuan;
            yPosOfWm = yuantu_gao - shuiyin_gao;
        }
        #endregion
        //声明矩形域
        RectangleF textArea = new RectangleF(xPosOfWm, yPosOfWm, rectWidth, rectHeight);

        Font font = new Font(xml_dt.Rows[0]["u29"].ToString(), fontSize);   //定义字体
        Brush whiteBrush = new SolidBrush(Color.White);   //白笔刷，画文字用
        Brush blackBrush = new SolidBrush(Color.Black);   //黑笔刷，画背景用

        g.FillRectangle(blackBrush, yPosOfWm, yPosOfWm, rectWidth, rectHeight);

        g.DrawString(text, font, whiteBrush, textArea);
        MemoryStream ms = new MemoryStream();
        //保存为Jpg类型
        bitmap.Save(rDstImgPath, ImageFormat.Jpeg);

        //输出处理后的图像，这里为了演示方便，我将图片显示在页面中了
        HttpContext.Current.Response.Clear();
        //Response.ContentType = "image/jpeg";
        //Response.BinaryWrite(ms.ToArray());
        g.Dispose();
        bitmap.Dispose();
        image.Dispose();
    }
    #endregion
    #region 处理路径
    public string chuli_lujing()
    {
        DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
        string lujing = xml_dt.Rows[0]["u38"].ToString();
        if (lujing == "")
        {
            return lujing;
        }
        else
        {
            lujing = DateTime.Now.ToString(lujing) + "/";
            return lujing;
        }

    }
    #endregion
    #region 后台跳转返回值 
    public void tiaozhuan(string err, string errurl)
    {
        string tiaozhuan = "";
        try
        {
            tiaozhuan = HttpContext.Current.Request["tiaozhuan"].ToString();
        }
        catch { }
        if (tiaozhuan == "ajax")
        {
            HttpContext.Current.Response.Write(err);
            HttpContext.Current.Response.End();
        }
        else
        {
            HttpContext.Current.Response.Redirect("err.aspx?err=" + err + "&errurl=" + tihuan(errurl, "&", "fzw123") + "");
        }
    }
    #endregion
    //调用js sdk
    public void set_jssdk()
    {
        //string signature = "";
        ////try
        ////{
        ////    signature = k_cookie("signature").ToString();
        ////}
        ////catch { }
        ////if (signature == "")
        ////{

        ////}
        //string timestamp = string.Empty;
        //string nonceStr = string.Empty;
        //string appID = ConfigurationSettings.AppSettings["appid"];
        //string appSecret = ConfigurationSettings.AppSettings["AppSecret"];
        //if (appID != "" || appSecret != "")
        //{
        //    string ticket = string.Empty;
        //    timestamp = JSSDKHelper.GetTimestamp();
        //    nonceStr = JSSDKHelper.GetNoncestr();
        //    JSSDKHelper jssdkhelper = new JSSDKHelper();
        //    ticket = JsApiTicketContainer.TryGetTicket(appID, appSecret);
        //    signature = jssdkhelper.GetSignature(ticket, nonceStr, timestamp, HttpContext.Current.Request.Url.AbsoluteUri.ToString());


        //    c_cookie(appID, "appID");
        //    c_cookie(timestamp, "timestamp");
        //    c_cookie(nonceStr, "nonceStr");
        //    c_cookie(signature, "signature");
        //}


        ////try
        ////{



        ////}
        ////catch (ErrorJsonResultException ex)
        ////{
        ////    HttpContext.Current.Response.Write("errorcode:" + ex.JsonResult.errcode.ToString() + "   errmsg:" + ex.JsonResult.errmsg);
        ////}
    }
    //获取微商城配置
    public string mall_config(string yonghuming, string Fields)
    {
        //DataTable sl_mall_config = my_c.GetTable("select "+ Fields + " from sl_mall_config where yonghuming='"+ yonghuming + "'");
        //if (sl_mall_config.Rows.Count > 0)
        //{
        //    return sl_mall_config.Rows[0][Fields].ToString();
        //}
        return "";
    }
    //处理分销链接 start
    public void fenxiaolink()
    {
        if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("micromessenger") > -1)
        {
            string fenxiao = "";
            try
            {
                fenxiao = ConfigurationSettings.AppSettings["fenxiao"].ToString();
            }
            catch { }
            if (fenxiao == "yes")
            {
                //处理介绍人
                if (HttpContext.Current.Request.Url.ToString().IndexOf("&fenxiao_openid") == -1 && HttpContext.Current.Request.Url.ToString().IndexOf("?fenxiao_openid") == -1)
                {

                    //介绍人等于空
                    string yonghuming = "";
                    string yonghuming_ = "";
                    try
                    {
                        yonghuming_ = k_cookie("openid").ToString();
                    }
                    catch { }

                    string jieshaoren = "";
                    try
                    {
                        jieshaoren = k_cookie("jieshaoren").ToString();
                    }
                    catch { }
                    try
                    {
                        yonghuming = HttpContext.Current.Request.QueryString["fenxiao_openid"].ToString();
                    }
                    catch
                    {

                        if (jieshaoren == "")
                        {
                            //无
                            if (HttpContext.Current.Request.QueryString.ToString() == "")
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString() + "?fenxiao_openid=" + yonghuming_);
                            }
                            else
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString() + "&fenxiao_openid=" + yonghuming_);
                            }
                            //end
                        }
                        else
                        {
                            //有
                            if (HttpContext.Current.Request.QueryString.ToString() == "")
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString() + "?fenxiao_openid=" + jieshaoren);
                            }
                            else
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString() + "&fenxiao_openid=" + jieshaoren);
                            }
                            //end
                        }


                    }


                    //Response.Write(my_b.k_cookie("jieshaoren"));
                    //Response.End();

                    //空
                }
                else
                {


                    string yonghuming = "";
                    try
                    {
                        yonghuming = k_cookie("user_name").ToString();
                    }
                    catch { }
                    string openid = "";
                    try
                    {
                        openid = k_cookie("openid").ToString();
                    }
                    catch { }

                    if (yonghuming == "")
                    {

                        c_cookie(HttpContext.Current.Request.QueryString["fenxiao_openid"].ToString(), "jieshaoren");
                        //给临时表加数据
                        if (openid != "")
                        {
                            //openid不空
                            DataTable sl_user = my_c.GetTable("select * from sl_user where 	openid='" + openid + "'");
                            //HttpContext.Current.Response.Write(sl_user.Rows.Count);
                            //HttpContext.Current.Response.End();
                            if (sl_user.Rows.Count == 0)
                            {
                                string jieshaoren = "";
                                try
                                {
                                    jieshaoren = k_cookie("jieshaoren").ToString();
                                }
                                catch { }

                                // my_c.genxin("insert into sl_user (openid,jieshaoren) values('" + k_cookie("openid") + "','" + jieshaoren + "')");
                                if (jieshaoren != "" || jieshaoren != openid)
                                {
                                    if (my_c.GetTable("select * from sl_user where openid='" + openid + "'").Rows.Count == 0)
                                    {
                                        my_c.genxin("insert into sl_user(yonghuming,mima,openid,touxiang,xingbie,xingming,guanzhu,jieshaoren,leixing) values('','" + md5("123456") + "','" + openid + "','" + k_cookie("touxiang") + "','" + k_cookie("xingbie") + "','" + k_cookie("xingming") + "','" + k_cookie("subscribe") + "','" + jieshaoren + "','普通用户')");

                                    }
                                }

                                //未关注的处理
                                string subscribe = k_cookie("subscribe");
                                if (subscribe == "否")
                                {
                                    string guanzhu = "";
                                    try
                                    {
                                        guanzhu = ConfigurationSettings.AppSettings["guanzhu"].ToString();
                                    }
                                    catch { }
                                    if (guanzhu != "")
                                    {
                                        HttpContext.Current.Response.Redirect(guanzhu);
                                    }
                                }
                                //end
                            }
                            //openid end
                        }

                        //end
                    }

                    //HttpContext.Current.Response.Write(k_cookie("jieshaoren"));
                    //HttpContext.Current.Response.End();

                }


                //处理介绍人
            }

        }
    }
    //处理分销链接 end

    //获取手机号归属地
    public string get_shouji_guishudi(string mobile)
    {
        string mobilecontent = getWebFile("http://www.ip138.com:8080/search.asp?action=mobile&mobile=" + mobile);
        Regex reg = new Regex("卡号归属地.*?</TR>", RegexOptions.Singleline);
        Match matches = reg.Match(mobilecontent);
        mobilecontent = NoHTML(matches.ToString()).Replace("卡号归属地", "").Replace(" ", "");
        return mobilecontent;
    }
    public string set_img(string imgurl)
    {

        if (imgurl != "")
        {
            return "<img src='" + imgurl + "' width='100px'>";
        }
        else
        {
            return "";
        }
    }
    //手机号码隐藏
    public string ReturnPhoneNO(string phoneNo)
    {
        Regex re = new Regex("(\\d{3})(\\d{4})(\\d{4})", RegexOptions.None);
        phoneNo = re.Replace(phoneNo, "$1****$3");
        return phoneNo;
    }
    //加分
    public void set_jifen(string fenshuleixing, string yonghuming)
    {

        DataTable sl_jiafen = my_c.GetTable("select * from sl_jiafen where fenshuleixing='" + fenshuleixing + "'");
        DataTable sl_jifen = new DataTable();

        if (sl_jiafen.Rows.Count > 0)
        {
            string fenshu = sl_jiafen.Rows[0]["fenshu"].ToString();
            string bianhua_ = my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + yonghuming + "' and zhuangtai='已处理'").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            int zuidi = int.Parse(bianhua_);
            int bianhua = int.Parse(bianhua_) + int.Parse(fenshu);
            if (sl_jiafen.Rows[0]["duoci"].ToString() == "否")
            {
                sl_jifen = my_c.GetTable("select * from sl_jifen where leixing='" + fenshuleixing + "' and yonghuming='" + yonghuming + "'");
                if (sl_jifen.Rows.Count == 0)
                {
                    if (int.Parse(sl_jiafen.Rows[0]["zuidi"].ToString()) <= zuidi)
                    {
                        my_c.genxin("insert into sl_jifen (leixing,fenshu,shijian,zhuangtai,yonghuming) values('签到送积分'," + fenshu + ",'" + fenshuleixing + "加" + fenshu + "分，只能增加一次！','已处理','" + yonghuming + "')");
                    }
                }
            }
            else
            {
                if (int.Parse(sl_jiafen.Rows[0]["zuidi"].ToString()) <= zuidi)
                {
                    my_c.genxin("insert into sl_jifen (leixing,fenshu,shijian,zhuangtai,yonghuming) values('签到送积分'," + fenshu + ",'" + fenshuleixing + "加" + fenshu + "分，可以增加多次！','已处理','" + yonghuming + "')");


                }
            }


        }



    }

    //zutulist
    public string get_zutu(string imgurl)
    {
        string t1 = "";
        string[] aa = Regex.Split(imgurl, "{next}");
        t1 = "";
        if (aa.Length > 0)
        {
            string[] b = Regex.Split(aa[0], "{title}");
            t1 = b[0].ToString();
        }
        return t1;
    }
    //统计单个会员积分
    public string getjifen(string yonghuming)
    {
        string jifen = "0";
        DataTable dt = my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + yonghuming + "'");
        if (dt.Rows.Count > 0)
        {
            jifen = dt.Rows[0]["count_id"].ToString();
        }
        if (jifen == "")
        {
            return "0";
        }
        else
        {
            return jifen;
        }
    }
    //根据积分设置会员等级
    public void set_uset_jibie(string yonghuming)
    {
        string jifen = "0";
        DataTable dt = my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + yonghuming + "' and zhuangtai='已处理' and (leixing='消费送积分' or leixing='签到送积分' or leixing='完善资料送积分') ");
        if (dt.Rows.Count > 0)
        {
            jifen = dt.Rows[0]["count_id"].ToString();
        }
        if (jifen == "")
        {
            jifen = "0";
        }
        DataTable sl_Parameter = my_c.GetTable("select * from sl_Parameter where classid=244 order by id asc");
        for (int i = 0; i < sl_Parameter.Rows.Count; i++)
        {
            string u2 = sl_Parameter.Rows[i]["u2"].ToString();
            Regex reg = new Regex("-", RegexOptions.Singleline);
            string[] aa = reg.Split(u2);
            int jifen_ = int.Parse(jifen);
            if (jifen_ >= int.Parse(aa[0].ToString()) && jifen_ <= int.Parse(aa[1].ToString()))
            {
                string jibie = sl_Parameter.Rows[i]["u1"].ToString();
                my_c.genxin("update sl_user set jibie='" + jibie + "' where yonghuming='" + yonghuming + "'");
                //HttpContext.Current.Response.Write(jibei);
                //HttpContext.Current.Response.End();
                break;
            }
        }


    }
    //加钱
    public void set_caiwu(string leixing, string miaoshu, float jine)
    {
        if (leixing == "消费")
        {
            string bianhua_ = my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + k_cookie("user_name") + "' ").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            float bianhua = float.Parse(bianhua_) - jine;
            my_c.genxin("insert into sl_caiwu (jine,leixing,miaoshu,bianhua,yonghuming) values(-" + jine.ToString() + ",'" + leixing + "','" + miaoshu + "'," + bianhua.ToString() + ",'" + k_cookie("user_name") + "')");
        }
        else
        {
            string bianhua_ = my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + k_cookie("user_name") + "' ").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            float bianhua = float.Parse(bianhua_) + jine;
            my_c.genxin("insert into sl_caiwu (jine,leixing,miaoshu,bianhua,yonghuming) values(" + jine.ToString() + ",'" + leixing + "','" + miaoshu + "'," + bianhua.ToString() + ",'" + k_cookie("user_name") + "')");
        }

    }
    //框架链接设置
    public string kuangjiaurl(string txt_value)
    {
        if (txt_value.IndexOf("?") > -1)
        {
            return txt_value + "&";
        }
        else
        {
            return txt_value + "?";
        }
    }
    //根据终端输入模板
    public string set_moban(string pctemp, string waptemp, string filename)
    {
        //string yonghuming = "";
        //try
        //{
        //    yonghuming = HttpContext.Current.Request.QueryString["yonghuming"].ToString();
        //}
        //catch { }
        string moban = "";
        //if (yonghuming != "")
        //{
        //    moban = mall_config(yonghuming, "moban");
        //}
        pctemp = pctemp + filename;
        waptemp = waptemp + filename;
        if (moban != "")
        {
            string malltemp = moban + filename;
            //微商城
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + malltemp))
            {
                return malltemp;
            }
            //end
        }
        else
        {
            //其它 start
            if (set_fangwen() == 0)
            {
                return pctemp;
            }
            else
            {
                if (waptemp == "")
                {
                    return pctemp;
                }
                else
                {
                    if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "/template/" + waptemp))
                    {
                        return waptemp;
                    }
                    else
                    {
                        return pctemp;
                    }

                }

            }
            //其它 end
        }
        return "";
        //end
    }
    //判断访问端
    public int set_fangwen()
    {
        int str = 0;
        string fangwen = HttpContext.Current.Request.UserAgent.ToLower();
        if (fangwen.IndexOf("iphone") > -1 || fangwen.IndexOf("ipad") > -1)
        {
            str = 1;
        }
        if (fangwen.IndexOf("android") > -1)
        {
            str = 1;
        }
        return str;
    }
    //设置openid

    public void set_openid()
    {
        if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("micromessenger") > -1)
        {
            #region 是在微信端
            set_jssdk();
            string openid = "";

            try
            {
                openid = k_cookie("openid");
            }
            catch { }

            if (openid != "")
            {

                #region cookie openid存在
                DataTable dt = my_c.GetTable("select * from sl_user where openid='" + openid + "' and (yonghuming<>'' or yonghuming is not null)");
                if (dt.Rows.Count > 0)
                {
                    c_cookie(dt.Rows[0]["yonghuming"].ToString(), "user_name");
                }
                else
                {
                    //微信自动登录 start
                    string login = "";
                    try
                    {
                        login = ConfigurationSettings.AppSettings["login"].ToString();
                    }
                    catch { }

                    if (login == "weixin")
                    {
                        my_c.genxin("insert into sl_user(yonghuming,mima,openid,touxiang,xingbie,xingming,guanzhu) values('" + openid + "','" + md5("123456") + "','" + openid + "','" + k_cookie("touxiang") + "','" + k_cookie("xingbie") + "','" + k_cookie("xingming") + "','" + k_cookie("subscribe") + "')");
                        c_cookie(openid, "user_name");
                    }

                    //微信自动登录 end
                }
                #endregion
            }
            else
            {
                #region openid为空或不存在 appid值存在
                string appid = "";
                try
                {
                    appid = ConfigurationSettings.AppSettings["appid"].ToString();
                }
                catch { }
                if (appid != "")
                {

                    HttpContext.Current.Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + HttpContext.Current.Server.UrlEncode(get_Domain() + "inc/user2.aspx?url=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "") + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect");
                }
                #endregion
            }
            #endregion
        }
    }
    //配置邮件及短信内容
    public string set_neirong(DataTable dt, string content)
    {
        Regex reg = new Regex(@"{fzw:database:.*?}", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(content);
        foreach (Match match in matches)
        {
            string ziduan = match.ToString().Replace("{fzw:database:", "");
            ziduan = ziduan.Replace("/", "");
            ziduan = ziduan.Replace("}", "");
            ziduan = ziduan.Trim();
            if (ziduan == "openid")
            {
                string openid = my_c.GetTable("select openid from sl_user where yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "'").Rows[0]["openid"].ToString();
                content = content.Replace(match.ToString(), openid);
            }
            else if (ziduan == "youhuiquanbianhao_mianzhi")
            {
                string mianzhi = my_c.GetTable("select mianzhi from sl_youhuiquan where youhuiquanbianhao='" + dt.Rows[0]["youhuiquanbianhao"].ToString() + "'").Rows[0]["mianzhi"].ToString();
                content = content.Replace(match.ToString(), mianzhi);
            }
            else if (ziduan == "jine_count")
            {
                string count_id = my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "' and leixing <>'领取' and zhuangtai='已付款'").Rows[0]["count_id"].ToString();
                try
                {
                    content = content.Replace(match.ToString(), get_jiage(float.Parse(count_id)));
                }
                catch { }
            }
            else if (ziduan == "yonghuming")
            {
                string xingming = my_c.GetTable("select xingming from sl_user where yonghuming='" + dt.Rows[0]["yonghuming"].ToString() + "'").Rows[0]["xingming"].ToString();
                content = content.Replace(match.ToString(), xingming);
            }
            else if (ziduan == "sl_cart")
            {
                string cart_str = "";
                DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanbianhao='" + dt.Rows[0]["dingdanhao"].ToString() + "'");
                for (int i = 0; i < sl_cart.Rows.Count; i++)
                {
                    if (cart_str == "")
                    {
                        // cart_str = sl_cart.Rows[i]["biaoti"].ToString() + "[" + sl_cart.Rows[i]["danjia"].ToString() + "×" + sl_cart.Rows[i]["shuliang"].ToString() + "=" + sl_cart.Rows[i]["xiaoji"].ToString() + "]";
                        cart_str = sl_cart.Rows[i]["biaoti"].ToString();
                    }
                    else
                    {
                        // cart_str = cart_str + "  " + sl_cart.Rows[i]["biaoti"].ToString() + "[" + sl_cart.Rows[i]["danjia"].ToString() + "×" + sl_cart.Rows[i]["shuliang"].ToString() + "=" + sl_cart.Rows[i]["xiaoji"].ToString() + "]";
                        cart_str = cart_str + "  " + sl_cart.Rows[i]["biaoti"].ToString();
                    }
                }
                content = content.Replace(match.ToString(), cart_str);
            }
            else
            {
                try
                {
                    content = content.Replace(match.ToString(), dt.Rows[0][ziduan].ToString());
                }
                catch
                {
                    HttpContext.Current.Response.Write(ziduan);
                    HttpContext.Current.Response.End();
                }
            }

        }
        return content;
    }
    //加分//在订单状态改变后，设置订单积分
    public void set_jifen(string leixing, string miaoshu, float jifen)
    {
        if (leixing == "兑换")
        {
            string bianhua_ = my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + k_cookie("user_name") + "' ").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            float bianhua = float.Parse(bianhua_) - jifen;
            my_c.genxin("insert into sl_jifen (fenshu,leixing,shijian,bianhua,yonghuming) values(-" + jifen.ToString() + ",'" + leixing + "','" + miaoshu + "'," + bianhua.ToString() + ",'" + k_cookie("user_name") + "')");
        }
        else
        {
            string bianhua_ = my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + k_cookie("user_name") + "' ").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            float bianhua = float.Parse(bianhua_) + jifen;
            my_c.genxin("insert into sl_jifen (fenshu,leixing,shijian,bianhua,yonghuming) values(" + jifen.ToString() + ",'" + leixing + "','" + miaoshu + "'," + bianhua.ToString() + ",'" + k_cookie("user_name") + "')");
        }



    }
    public void set_jifen1(string leixing, string miaoshu, float jifen, string yonghuming)
    {
        if (leixing == "兑换")
        {
            string bianhua_ = my_c.GetTable("select sum(fenshu) as count_id from sl_jifen where yonghuming='" + yonghuming + "' ").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            float bianhua = float.Parse(bianhua_) - jifen;
            my_c.genxin("insert into sl_jifen (fenshu,leixing,shijian,bianhua,yonghuming,zhuangtai) values(-" + jifen.ToString() + ",'" + leixing + "','" + miaoshu + "'," + bianhua.ToString() + ",'" + yonghuming + "','已处理')");
        }
        else
        {
            string bianhua_ = my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + yonghuming + "' ").Rows[0]["count_id"].ToString();
            if (bianhua_ == "")
            {
                bianhua_ = "0";
            }
            float bianhua = float.Parse(bianhua_) + jifen;
            my_c.genxin("insert into sl_jifen (fenshu,leixing,shijian,bianhua,yonghuming,zhuangtai) values(" + jifen.ToString() + ",'" + leixing + "','" + miaoshu + "'," + bianhua.ToString() + ",'" + yonghuming + "','已处理')");
        }



    }
    //设置参数
    public string set_canshu(string laiyuanbianhao, string Model_id)
    {
        DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
        string canshu_id = Model_dt.Rows[0]["u9"].ToString();
        if (canshu_id != "")
        {
            return "<a href='auto_table.aspx?Model_id=" + canshu_id + "&laiyuanbianhao=" + laiyuanbianhao + "' target=_blank>参数</a> | ";
        }
        else
        {
            return "";
        }
    }
    public string set_canshu1(string laiyuanbianhao, string Model_id, string classid)
    {
        //DataTable Model_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model where id=" + Model_id);
        //DataTable dt = my_c.GetTable("select * from " + Model_dt.Rows[0]["u1"].ToString() + " where id=" + laiyuanbianhao + "");
        //if (dt.Rows[0]["shenhe"].ToString() == "未审核")
        //{
        //    //处理会员
        //    DataTable sl_user = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "user where yonghuming='" + k_cookie("admin_id") + "'");
        //    if (sl_user.Rows[0]["huiyuanzu"].ToString() != "管理员")
        //    {

        //       return "";
        //    }
        //    else
        //    {
        //        return "<a href='news_table_add.aspx?classid=" + classid + "&Model_id=" + Model_id + "&type=shenhe&id=" + laiyuanbianhao + "'>审核通过</a> | ";
        //    }
        //    //end

        //}
        //else
        //{
        //    return "";
        //}
        return "";
    }
    //时间格式处理
    public string set_time(string t1, string t2)
    {
        try
        {
            DateTime b = DateTime.Parse(t1);
            string b_Year = b.Year.ToString();
            string b_Month = b.Month.ToString();
            if (int.Parse(b_Month) < 10)
            {
                b_Month = "0" + b_Month;
            }
            string b_Day = b.Day.ToString();
            if (int.Parse(b_Day) < 10)
            {
                b_Day = "0" + b_Day;
            }
            string b_Hour = b.Hour.ToString();
            if (int.Parse(b_Hour) < 10)
            {
                b_Hour = "0" + b_Hour;
            }
            string b_Minute = b.Minute.ToString();
            if (int.Parse(b_Minute) < 10)
            {
                b_Minute = "0" + b_Minute;
            }
            string b_Second = b.Second.ToString();
            if (int.Parse(b_Second) < 10)
            {
                b_Second = "0" + b_Second;
            }

            t1 = t2.Replace("yyyy", b_Year).Replace("MM", b_Month).Replace("dd", b_Day).Replace("hh", b_Hour).Replace("mm", b_Minute).Replace("ss", b_Second);
            return t1;
        }
        catch
        {
            return t1;
        }
    }
    //发送短信内容
    public void post_duan(string dingdanhao, string tablename, string leixingbiaoti)
    {
        DataTable dt = my_c.GetTable("select top 1 * from " + tablename + " where dingdanhao='" + dingdanhao + "' order by id desc");
        string shoujihaoma = dt.Rows[0]["shoujihaoma"].ToString();
        string beizhu = dt.Rows[0]["beizhu"].ToString();
        string zhifufangshi = dt.Rows[0]["zhifufangshi"].ToString();
        string shoujianrenxingming = dt.Rows[0]["shoujianrenxingming"].ToString();
        string suozaidiqu = dt.Rows[0]["suozaidiqu"].ToString();
        string jiedaodizhi = dt.Rows[0]["jiedaodizhi"].ToString();

        string yonghuming = "";
        try
        {
            yonghuming = k_cookie("user_name");
        }
        catch
        {
            yonghuming = dt.Rows[0]["yonghuming"].ToString();
        }

        DataTable xml_dt = my_c.read_xml("upfile/data/web_config.xml", "web_config");
        //（逗号隔开多个号码） DestMobile = "18180229615"
        //Content 内容
        string shangjiashoujihao = xml_dt.Rows[0]["u24"].ToString();





        string fanhui = duanxing(shoujihaoma, get_value("duanxinneirong", "sl_fasong", "where leixing='短信' and leixingbiaoti='" + leixingbiaoti + "'").Replace("[shoujihaoma]", shoujihaoma).Replace("[beizhu]", beizhu).Replace("[zhifufangshi]", zhifufangshi).Replace("[shoujianrenxingming]", shoujianrenxingming).Replace("[suozaidiqu]", suozaidiqu).Replace("[jiedaodizhi]", jiedaodizhi).Replace("[dingdanhao]", dingdanhao).Replace("[yonghuming]", yonghuming), get_value("duanxinmobanid", "sl_fasong", "where leixing='短信' and leixingbiaoti='" + leixingbiaoti + "'"));
    }
    #region 发送短信
    public string duanxing(string DestMobile, string ContentStr1, string duanxinmobanid)
    {
        duanxin dx = new duanxin();
        DataTable dt = my_c.GetTable("select top 1 * from sl_duanxin where qiyong='是' order by id desc");
        string err = "";
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["jiekoufangan"].ToString() == "阿里云")
            {
                dx.aliyun(dt.Rows[0]["zhanghao"].ToString(), dt.Rows[0]["mima"].ToString(), DestMobile, ContentStr1, dt.Rows[0]["qianming"].ToString(), duanxinmobanid);
            }
            if (dt.Rows[0]["jiekoufangan"].ToString() == "阿里大鱼")
            {
                dx.alidayu(dt.Rows[0]["zhanghao"].ToString(), dt.Rows[0]["mima"].ToString(), DestMobile, ContentStr1, dt.Rows[0]["qianming"].ToString(), duanxinmobanid);
            }
            if (dt.Rows[0]["jiekoufangan"].ToString() == "创瑞")
            {
                dx.supermore(dt.Rows[0]["zhanghao"].ToString(), dt.Rows[0]["mima"].ToString(), DestMobile, ContentStr1, dt.Rows[0]["qianming"].ToString(), duanxinmobanid);
            }
            if (dt.Rows[0]["jiekoufangan"].ToString() == "凌凯")
            {
              string fanhui= dx.mb345(dt.Rows[0]["zhanghao"].ToString(), dt.Rows[0]["mima"].ToString(), DestMobile, ContentStr1, dt.Rows[0]["qianming"].ToString(), duanxinmobanid);
              if (int.Parse(fanhui) != 1)
              {
                  HttpContext.Current.Response.Write(fanhui);
                  HttpContext.Current.Response.End();
              }
              else
              {
                  return fanhui;
                  //HttpContext.Current.Response.Write(fanhui);
                  //HttpContext.Current.Response.End();
              }
               
            }
        }
        else
        {
            err = "没有配置短信";
        }
        return err;
    }
    #endregion
    //获取周几
    public string get_xingqi(DateTime dy)
    {
        string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        string week = Day[Convert.ToInt32(dy.DayOfWeek.ToString("d"))].ToString();

        return week;

    }
    //会员生日或店庆送双倍积分
    public int get_user_order_jifen(string yonghuming, int jine)
    {
        //DataTable sl_user = my_c.GetTable("select * from sl_user where yonghuming='" + yonghuming + "'");
        //DateTime dy = DateTime.Now;

        //if (dy.Day == 10)
        //{
        //    return jine * 2;
        //}
        //if (sl_user.Rows.Count > 0)
        //{
        //    if (sl_user.Rows[0]["shengri"].ToString() != "")
        //    {
        //        DateTime dy1 = DateTime.Parse(sl_user.Rows[0]["shengri"].ToString());
        //        if (dy.Month == dy1.Month && dy.Day == dy1.Day)
        //        {
        //            return jine * 2;
        //        }
        //    }
        //}
        return jine;
    }
    //在订单状态改变后，设置返利
    public void set_order_fanli(string dingdanhao, string tablename)
    {

        DataTable dt = my_c.GetTable("select * from sl_" + tablename.Replace("sl_", "") + " where dingdanhao='" + dingdanhao + "'");
        string songfen = "";
        try
        {
            songfen = dt.Rows[0]["songfen"].ToString();
        }
        catch { }
        if (dt.Rows[0]["songfen"].ToString() != "是")
        {
            string yonghuming = dt.Rows[0]["yonghuming"].ToString();
            float jine = 0;

            if (dt.Rows[0]["zhuangtai"].ToString() == "订单完成")
            {
                DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanbianhao='" + dingdanhao + "' and yonghuming='" + yonghuming + "'");

                for (int i = 0; i < sl_cart.Rows.Count; i++)
                {
                    string jieshaoren = sl_cart.Rows[i]["jieshaoren"].ToString();
                    if (jieshaoren != "")
                    {

                        jine = float.Parse(sl_cart.Rows[i]["xiaoji"].ToString()) * float.Parse(my_c.GetTable("select top 1 u2 from sl_Parameter where classid=253").Rows[0]["u2"].ToString());

                        //加钱
                        float bianhua = 0;
                        try
                        {
                            bianhua = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + jieshaoren + "'").Rows[0]["count_id"].ToString());
                        }
                        catch { }
                        bianhua = bianhua + jine;
                        my_c.genxin("insert into sl_caiwu(yonghuming,leixing,jine,miaoshu,bianhua) values('" + jieshaoren + "','返利'," + jine.ToString() + ",'用户" + yonghuming + "购买" + get_dingdanhao_biaoti(dingdanhao) + "后返利'," + bianhua.ToString() + ")");


                    }
                }



                //end

            }
        }




    }
    public string get_dingdanhao_biaoti(string dingdanhao)
    {
        DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanbianhao='" + dingdanhao + "' and leixing='产品'");
        if (sl_cart.Rows.Count > 0)
        {
            return c_string("<a href='/page.aspx?id=" + sl_cart.Rows[0]["laiyuanbianhao"].ToString() + "&classid=1' target='_blank'>" + sl_cart.Rows[0]["biaoti"].ToString() + "</a>，订单号：" + dingdanhao);
        }
        return "";
    }
    //在订单状态改变后，设置设计师返利
    public void set_order_shejishi(string dingdanhao, string tablename)
    {

        DataTable dt = my_c.GetTable("select * from sl_" + tablename.Replace("sl_", "") + " where dingdanhao='" + dingdanhao + "'");
        string songfen = "";
        try
        {
            songfen = dt.Rows[0]["songfen"].ToString();
        }
        catch { }
        if (dt.Rows[0]["songfen"].ToString() != "是")
        {
            string yonghuming = dt.Rows[0]["yonghuming"].ToString();
            float jine = 0;

            if (dt.Rows[0]["zhuangtai"].ToString() == "订单完成")
            {
                DataTable sl_cart = my_c.GetTable("select * from sl_cart where dingdanbianhao='" + dingdanhao + "' and yonghuming='" + yonghuming + "'");

                for (int i = 0; i < sl_cart.Rows.Count; i++)
                {
                    string jieshaoren = sl_cart.Rows[i]["shejishi"].ToString();
                    if (jieshaoren != "")
                    {

                        jine = float.Parse(sl_cart.Rows[i]["xiaoji"].ToString()) * float.Parse(my_c.GetTable("select top 1 u2 from sl_Parameter where classid=253").Rows[0]["u2"].ToString());

                        //加钱
                        float bianhua = 0;
                        try
                        {
                            bianhua = float.Parse(my_c.GetTable("select sum(jine) as count_id from sl_caiwu where yonghuming='" + jieshaoren + "'").Rows[0]["count_id"].ToString());
                        }
                        catch { }
                        bianhua = bianhua + jine;

                        my_c.genxin("insert into sl_caiwu(yonghuming,leixing,jine,miaoshu,bianhua) values('" + jieshaoren + "','返利'," + jine.ToString() + ",'用户" + yonghuming + "购买" + get_dingdanhao_biaoti(dingdanhao) + "后返利'," + bianhua.ToString() + ")");


                    }
                }



                //end

            }
        }




    }

    //获取编号
    //获取网站域名
    public string get_Domain()
    {
        string Domain = "";
        if (HttpContext.Current.Request.ServerVariables["Server_Port"].ToString() != "80")
        {
            Domain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + ":" + HttpContext.Current.Request.ServerVariables["Server_Port"].ToString() + "/";
        }
        else
        {
            Domain = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + "/";
        }
        return Domain;
    }

    //设置等级分数
    public void setdengji(string yonghuming, string leixing, float fenshu)
    {
        System.TimeSpan st = new TimeSpan();
        DateTime dy1 = DateTime.Now;
        DateTime dy2 = DateTime.Now.AddDays(-1);

        DataTable dt = my_c.GetTable("select top 1 * from sl_system where u1='" + k_cookie("user_name").ToString() + "' and u4='会员签到' order by dtime desc");
        if (dt.Rows.Count > 0)
        {
            dy2 = DateTime.Parse(dt.Rows[0]["dtime"].ToString());
        }
        //    HttpContext.Current.Response.Write(st.Days.ToString());
        if (st.Days >= 1)
        {

            st = dy1.Subtract(dy2);


            my_c.genxin("insert into sl_dengji (yonghuming,leixing,fenshu) values('" + k_cookie("user_name").ToString() + "','" + leixing + "'," + fenshu + ")");
            my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + k_cookie("user_name").ToString() + "','此会员（" + k_cookie("user_name").ToString() + "）签到成功!操作页面" + HttpContext.Current.Request.Url.ToString() + "','" + HttpContext.Current.Request.UserHostAddress.ToString() + "','会员签到')");

            // HttpContext.Current.Response.Write("ok");

        }
        //HttpContext.Current.Response.End();
    }
    //获取我的分数
    public float get_fenshu(string leixing)
    {
        float zongfen = 0;
        DataTable dt = new DataTable();
        dt = my_c.GetTable("select sum(fenshu) as count_id from sl_dengji where yonghuming='" + k_cookie("user_name").ToString() + "' and leixing='" + leixing + "'");

        if (dt.Rows.Count > 0)
        {
            zongfen = float.Parse(dt.Rows[0]["count_id"].ToString());
        }


        return zongfen;
    }
    //end
    //获取html页面提交的图片
    public string wenjian(string ziduan)
    {

        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        HttpFileCollection files = HttpContext.Current.Request.Files;
        HttpPostedFile postedFile = files[ziduan];

        string fileName, fileExtension;
        fileName = System.IO.Path.GetFileName(postedFile.FileName);
        fileExtension = System.IO.Path.GetExtension(fileName);
        //strMsg.Append("上传的文件类型：" + postedFile.ContentType.ToString() + "<br>");
        //strMsg.Append("客户端文件地址：" + postedFile.FileName + "<br>");
        //strMsg.Append("上传文件的文件名：" + fileName + "<br>");
        //strMsg.Append("上传文件的扩展名：" + fileExtension + "<br><hr>");
        ///'可根据扩展名字的不同保存到不同的文件夹
        ///注意：可能要修改你的文件夹的匿名写入权限。
        ///postedFile.ContentLength  大小
        DataTable dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");
        string file_Extension = dt.Rows[0]["u6"].ToString();
        if (postedFile.ContentLength > 0 && postedFile.ContentLength < (1024 * 1024 * 10))
        {
            if (file_Extension.IndexOf(fileExtension) > -1)
            {

                DateTime dy = DateTime.Now;
                string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                fileName = d1 + Num1.ToString() + fileExtension;
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile//Upload//" + tt1 + "");
                string filepath = HttpContext.Current.Request.PhysicalApplicationPath + "upfile//Upload//" + tt1 + "//" + fileExtension;


                postedFile.SaveAs(HttpContext.Current.Request.PhysicalApplicationPath + "/upfile/Upload/" + tt1 + "/" + fileName + "");
                return "/upfile/Upload/" + tt1 + "/" + fileName + "";
            }
            else
            {
                return "";
                //文件类型不正确
            }
        }
        else
        {
            return "";
            //文件太大或者不存在
        }
    }

    //获取支付参数
    public string get_pay(string type, string Fields)
    {
        DataTable dt = my_c.GetTable("select * from sl_pay where fangshimingcheng='" + type + "' ");
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][Fields].ToString();
        }
        return "";
    }
    //end
    //强行处理字段串
    public string set_url_css(string file_value)
    {
        return HttpContext.Current.Server.HtmlDecode(t_string(HttpContext.Current.Server.HtmlEncode(file_value)));
    }
    //获取获取分数
    public string get_fanli(float fenshu, string type)
    {
        float bili = 0;
        DataTable dt = new DataTable();
        if (type == "消费")
        {
            dt = my_c.GetTable("select * from sl_Parameter where classid=72");
        }
        else
        {
            dt = my_c.GetTable("select * from sl_Parameter where classid=66");
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string u1 = dt.Rows[i]["u1"].ToString();
            float value1 = float.Parse(u1.Split('-')[0].ToString());
            float value2 = float.Parse(u1.Split('-')[1].ToString());
            if (fenshu >= value1 && fenshu <= value2)
            {
                bili = float.Parse(dt.Rows[i]["u2"].ToString());
            }
        }

        fenshu = fenshu * bili;
        return fenshu.ToString();
    }
    //end

    //空格变成问号的怪问题
    public string set_htmldecode(string HtmlStr)
    {

        byte[] space = new byte[] { 0xc2, 0xa0 };
        string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
        HtmlStr = HtmlStr.Replace(UTFSpace, "&nbsp;");
        return HtmlStr;
    }
    //把价格处理下
    public string get_jiage(float danjia)
    {
        try
        {
            return danjia.ToString("F2");
        }
        catch
        {
            return danjia.ToString();
        }
    }
    //获取省市县三级联动
    public string set_liandong(string g1, int count_i)
    {
        string t1 = "";
        string[] aa = g1.Split('-');
        for (int i = 0; i < count_i; i++)
        {
            try
            {
                t1 = t1 + " " + aa[i].ToString();
            }
            catch { }
        }
        return t1;
    }
    //删除组图内图片
    public void del_zutu(string g1)
    {
        Regex reg = new Regex("{next}", RegexOptions.Singleline);
        string[] aa = reg.Split(g1);
        for (int i = 0; i < aa.Length; i++)
        {
            Regex reg1 = new Regex("{title}", RegexOptions.Singleline);
            string[] bb = reg1.Split(aa[i].ToString());
            string imgurl = bb[0].ToString();
            if (imgurl.ToString().IndexOf("http://") == -1)
            {
                del_pic((imgurl));
            }
        }
    }

    //设置评论数
    public void set_pinglun(string tablename, string id, string type)
    {
        string pinglun = my_c.GetTable("select count(id) as count_id from sl_comments where pinglunleixing='" + type + "' and pinglunid=" + id + "").Rows[0]["count_id"].ToString();
        my_c.genxin("update " + tablename + " set pinglun=" + pinglun + " where id=" + id + "");

        string pinglun1 = my_c.GetTable("select count(id) as count_id from sl_comments where yonghuming='" + k_cookie("user_name") + "'").Rows[0]["count_id"].ToString();
        string pinglun2 = my_c.GetTable("select count(id) as count_id from sl_tiezi where yonghuming='" + k_cookie("user_name") + "'").Rows[0]["count_id"].ToString();
        string pinglun3 = my_c.GetTable("select count(id) as count_id from sl_article where yonghuming='" + k_cookie("user_name") + "'").Rows[0]["count_id"].ToString();
        my_c.genxin("update sl_user set huifu=" + pinglun1 + ",fatie=" + pinglun2 + ",baike=" + pinglun3 + " where youxiang='" + k_cookie("user_name") + "'");


    }
    //几小时前
    public string DateStringFromNow(DateTime dt)
    {

        TimeSpan span = DateTime.Now - dt;

        if (span.TotalDays > 60)
        {

            return dt.ToShortDateString();

        }

        else
        {

            if (span.TotalDays > 30)
            {

                return

                "1个月前";

            }

            else
            {

                if (span.TotalDays > 14)
                {

                    return

                    "2周前";

                }

                else
                {

                    if (span.TotalDays > 7)
                    {

                        return

                        "1周前";

                    }

                    else
                    {

                        if (span.TotalDays > 1)
                        {

                            return

                            string.Format("{0}天前", (int)Math.Floor(span.TotalDays));

                        }

                        else
                        {

                            if (span.TotalHours > 1)
                            {

                                return

                                string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));

                            }

                            else
                            {

                                if (span.TotalMinutes > 1)
                                {

                                    return

                                    string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));

                                }

                                else
                                {

                                    if (span.TotalSeconds >= 1)
                                    {

                                        return

                                        string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));

                                    }

                                    else
                                    {

                                        return

                                        "1秒前";

                                    }

                                }

                            }

                        }

                    }

                }

            }

        }

    }



    //C#中使用TimeSpan计算两个时间的差值

    //可以反加两个日期之间任何一个时间单位。

    private string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {

        string dateDiff = null;

        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);

        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);

        TimeSpan ts = ts1.Subtract(ts2).Duration();

        dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";

        return dateDiff;

    }





    //说明：

    /**/

    /*1.DateTime值类型代表了一个从公元0001年1月1日0点0分0秒到公元9999年12月31日23点59分59秒之间的具体日期时刻。因此，你可以用DateTime值类型来描述任何在想象范围之内的时间。一个DateTime值代表了一个具体的时刻

    2.TimeSpan值包含了许多属性与方法，用于访问或处理一个TimeSpan值

    下面的列表涵盖了其中的一部分：

    Add：与另一个TimeSpan值相加。 

    Days:返回用天数计算的TimeSpan值。 

    Duration:获取TimeSpan的绝对值。 

    Hours:返回用小时计算的TimeSpan值 

    Milliseconds:返回用毫秒计算的TimeSpan值。 

    Minutes:返回用分钟计算的TimeSpan值。 

    Negate:返回当前实例的相反数。 

    Seconds:返回用秒计算的TimeSpan值。 

    Subtract:从中减去另一个TimeSpan值。 

    Ticks:返回TimeSpan值的tick数。 

    TotalDays:返回TimeSpan值表示的天数。 

    TotalHours:返回TimeSpan值表示的小时数。 

    TotalMilliseconds:返回TimeSpan值表示的毫秒数。 

    TotalMinutes:返回TimeSpan值表示的分钟数。 

    TotalSeconds:返回TimeSpan值表示的秒数。

    */



    /**/

    /// <summary>

    /// 日期比较

    /// </summary>

    /// <param name="today">当前日期</param>

    /// <param name="writeDate">输入日期</param>

    /// <param name="n">比较天数</param>

    /// <returns>大于天数返回true，小于返回false</returns>

    private bool CompareDate(string today, string writeDate, int n)
    {

        DateTime Today = Convert.ToDateTime(today);

        DateTime WriteDate = Convert.ToDateTime(writeDate);

        WriteDate = WriteDate.AddDays(n);

        if (Today >= WriteDate)

            return false;

        else

            return true;

    }

    //设置标签
    public string set_lable(string g1)
    {
        string[] aa = g1.Split('|');
        string t3 = "";
        for (int x = 0; x < aa.Length; x++)
        {
            string lableurl = "/search.aspx?key=" + aa[x].ToLower() + "";
            t3 = t3 + "<a href='" + lableurl + "' class='dh1'>" + aa[x] + "</a>&nbsp;&nbsp;&nbsp;&nbsp;";
        }


        return t3;
    }
    //处理价格
    public string set_jiage(string jiage)
    {
        float jiage1 = float.Parse(jiage);
        if (jiage1 == 0)
        {
            return "现场估值";
        }
        else
        {
            return "￥" + jiage1.ToString("F2");
        }
    }
    //生成优惠券
    public string get_yhq()
    {
        DateTime dy = DateTime.Now;
        string tt1 = dy.Day.ToString() + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(90000000)) + 10000000;
        if (my_c.GetTable("select id from sl_youhuiquan where bianhao='" + Num1.ToString() + tt1 + "'").Rows.Count > 0)
        {
            get_yhq();
        }
        else
        {
            return Num1.ToString() + tt1;
        }
        return "";
    }
    //end
    //查看会员登录状态
    public void user_sta(string usercookie)
    {

        try
        {
            if (k_cookie(usercookie) == "")
            {
                HttpContext.Current.Response.Redirect("/err.aspx?err=请登录后访问！&errurl=" + tihuan("/?login.html", "&", "fzw123") + "");
            }

        }
        catch
        {
            HttpContext.Current.Response.Redirect("/err.aspx?err=请登录后访问！&errurl=" + tihuan("/?login.html", "&", "fzw123") + "");
        }



    }
    //查看会员登录状态
    public string user_login()
    {

        try
        {
            if (k_cookie("user_name") == "")
            {

                return "no";
            }
            else
            {
                return "yes";
            }

        }
        catch
        {
            return "no";
        }



    }
    //通过时间知道年龄
    public string c_time(string g1)
    {

        try
        {
            DateTime b = DateTime.Parse(g1);
            int jisui = DateTime.Now.Date.Subtract(b).Days / 365;
            return jisui.ToString();
        }
        catch
        {
            return "";
        }

    }
    //end
    //get ad
    public string get_ad(string g1)
    {
        DataTable dt = my_c.GetTable("select * from sl_ad where u1='" + g1 + "'");
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["u6"].ToString();
        }
        return "";
    }
    //end
    //设置遍历控件
    public void setform(DataTable fromdt, Panel p1)
    {
        foreach (Control ct in p1.Controls)//循环查询表单里面的子控件
        {

            //textbox
            if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
            {
                TextBox txt = (TextBox)ct;
                if (txt.ValidationGroup == "")
                {
                    if (txt.TextMode.ToString().ToString() != "Password")
                    {
                        txt.Text = fromdt.Rows[0][txt.ID].ToString();
                    }
                }
            }

            //end

            //RadioButtonList

            if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.RadioButtonList"))
            {
                RadioButtonList ra = (RadioButtonList)ct;
                if (ra.ValidationGroup == "")
                {
                    ra.SelectedValue = fromdt.Rows[0][ra.ID].ToString();
                }
            }
            //end

            //DropDownList

            if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
            {
                DropDownList dr = (DropDownList)ct;
                if (dr.ValidationGroup == "")
                {
                    dr.SelectedValue = fromdt.Rows[0][dr.ID].ToString();
                }
            }
            //end

            //CheckBoxList

            if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBoxList"))
            {
                CheckBoxList ch = (CheckBoxList)ct;
                if (ch.ValidationGroup == "")
                {
                    ch.SelectedValue = fromdt.Rows[0][ch.ID].ToString();

                    for (int j = 0; j < ch.Items.Count; j++)
                    {
                        if (fromdt.Rows[0][ch.ID].ToString().IndexOf(ch.Items[j].Text) > -1)
                        {
                            ch.Items[j].Selected = true;
                        }
                    }
                }
            }
            //CheckBoxList


        }
    }
    //end
    //给DropDownList设值
    public void set_list(DropDownList g1, int id)
    {

        g1.DataSource = my_c.GetTable("select u1,id from sl_Parameter where classid=" + id.ToString() + "");
        g1.DataTextField = "u1";
        g1.DataValueField = "u1";
        g1.DataBind();
        g1.Items.Insert(0, "请选择");
        g1.Items[0].Value = "";
    }
    //end
    //获取遍历控件
    public string insertform(string type, string fromname, string values, Panel p1)
    {
        if (type == "add")
        {
            string sql = "insert into " + fromname + " (#file#) values(#value#)";
            string sql1 = "";
            string sql2 = "";
            foreach (Control ct in p1.Controls)//循环查询表单里面的子控件
            {

                //textbox
                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                {
                    TextBox txt = (TextBox)ct;
                    if (txt.ValidationGroup == "")
                    {

                        if (sql1 == "")
                        {
                            sql1 = txt.ID;
                        }
                        else
                        {
                            sql1 = sql1 + "," + txt.ID;
                        }

                        if (sql2 == "")
                        {
                            if (txt.TextMode.ToString().ToString() == "Password")
                            {
                                sql2 = "'" + md5(txt.Text) + "'";
                            }
                            else
                            {
                                sql2 = "'" + txt.Text + "'";

                            }
                        }
                        else
                        {
                            if (txt.TextMode.ToString() == "Password")
                            {
                                sql2 = sql2 + "," + "'" + md5(txt.Text) + "'";
                            }
                            else
                            {
                                sql2 = sql2 + "," + "'" + txt.Text + "'";

                            }

                        }

                    }
                }

                //end

                //RadioButtonList

                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.RadioButtonList"))
                {
                    RadioButtonList ra = (RadioButtonList)ct;
                    if (ra.ValidationGroup == "")
                    {
                        if (sql1 == "")
                        {
                            sql1 = ra.ID;
                        }
                        else
                        {
                            sql1 = sql1 + "," + ra.ID;
                        }

                        if (sql2 == "")
                        {
                            sql2 = "'" + ra.SelectedValue + "'";
                        }
                        else
                        {
                            sql2 = sql2 + "," + "'" + ra.SelectedValue + "'";
                        }
                    }
                }
                //end

                //DropDownList

                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                {
                    DropDownList dr = (DropDownList)ct;
                    if (dr.ValidationGroup == "")
                    {
                        if (sql1 == "")
                        {
                            sql1 = dr.ID;
                        }
                        else
                        {
                            sql1 = sql1 + "," + dr.ID;
                        }

                        if (sql2 == "")
                        {
                            sql2 = "'" + dr.SelectedValue + "'";
                        }
                        else
                        {
                            sql2 = sql2 + "," + "'" + dr.SelectedValue + "'";
                        }
                    }
                }
                //end

                //CheckBoxList

                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBoxList"))
                {
                    CheckBoxList ch = (CheckBoxList)ct;
                    if (ch.ValidationGroup == "")
                    {
                        if (sql1 == "")
                        {
                            sql1 = ch.ID;
                        }
                        else
                        {
                            sql1 = sql1 + "," + ch.ID;
                        }
                        string ch_str = "";
                        for (int j = 0; j < ch.Items.Count; j++)
                        {
                            if (ch.Items[j].Selected == true)
                            {
                                if (ch_str == "")
                                {
                                    ch_str = ch.Items[j].Text;
                                }
                                else
                                {
                                    ch_str = ch_str + "|" + ch.Items[j].Text;
                                }
                            }
                        }
                        if (sql2 == "")
                        {
                            sql2 = "'" + ch_str + "'";
                        }
                        else
                        {
                            sql2 = sql2 + "," + "'" + ch_str + "'";
                        }
                    }
                }
                //end


            }

            sql = sql.Replace("#file#", sql1).Replace("#value#", sql2);
            return sql;
        }
        //add end
        if (type == "edit")
        {
            string sql = "update  " + fromname + " set #value# " + values;
            string sql1 = "";
            foreach (Control ct in p1.Controls)//循环查询表单里面的子控件
            {

                //textbox
                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                {
                    TextBox txt = (TextBox)ct;
                    if (txt.ValidationGroup == "")
                    {

                        if (sql1 == "")
                        {
                            if (txt.TextMode.ToString() == "Password")
                            {
                                if (txt.Text != "")
                                {
                                    sql1 = txt.ID + "='" + md5(txt.Text) + "'";
                                }
                            }
                            else
                            {
                                sql1 = txt.ID + "='" + txt.Text + "'";

                            }

                        }
                        else
                        {
                            if (txt.TextMode.ToString() == "Password")
                            {
                                sql1 = sql1 + "," + txt.ID + "='" + md5(txt.Text) + "'";
                            }
                            else
                            {
                                sql1 = sql1 + "," + txt.ID + "='" + txt.Text + "'";

                            }

                        }

                    }
                }

                //end

                //RadioButtonList

                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.RadioButtonList"))
                {

                    RadioButtonList ra = (RadioButtonList)ct;
                    if (ra.ValidationGroup == "")
                    {
                        if (sql1 == "")
                        {
                            sql1 = ra.ID + "='" + ra.Text + "'";
                        }
                        else
                        {
                            sql1 = sql1 + "," + ra.ID + "='" + ra.SelectedValue + "'";
                        }
                    }
                }
                //end

                //DropDownList

                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                {
                    DropDownList dr = (DropDownList)ct;
                    if (dr.ValidationGroup == "")
                    {
                        if (sql1 == "")
                        {
                            sql1 = dr.ID + "='" + dr.Text + "'";
                        }
                        else
                        {
                            sql1 = sql1 + "," + dr.ID + "='" + dr.SelectedValue + "'";
                        }
                    }
                }
                //end
                //CheckBoxList

                if (ct.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBoxList"))
                {
                    CheckBoxList ch = (CheckBoxList)ct;
                    if (ch.ValidationGroup == "")
                    {
                        string ch_str = "";
                        for (int j = 0; j < ch.Items.Count; j++)
                        {
                            if (ch.Items[j].Selected == true)
                            {
                                if (ch_str == "")
                                {
                                    ch_str = ch.Items[j].Text;
                                }
                                else
                                {
                                    ch_str = ch_str + "|" + ch.Items[j].Text;
                                }
                            }
                        }
                        if (sql1 == "")
                        {
                            sql1 = ch.ID + "='" + ch_str + "'";
                        }
                        else
                        {
                            sql1 = sql1 + "," + ch.ID + "='" + ch_str + "'";
                        }
                    }
                }
                //end


            }

            sql = sql.Replace("#value#", sql1);
            return sql;
        }
        return type;
    }
    //end
    //汉字长度
    public int z_chang(string g1)
    {
        byte[] sarr = System.Text.Encoding.UTF8.GetBytes(g1);
        return sarr.Length;
    }

    /// <summary>
    /// 首选编码的代码页名称
    /// </summary>
    /// <param name="srcName">原编码格式</param>
    /// <param name="convToName">要转换成的编码格式</param>
    /// <param name="value">需要转换的字符串</param>
    /// <returns>返回转换后的字符串</returns>
    public string zhuanma(string srcName, string convToName, string value)
    {
        System.Text.Encoding srcEncode = System.Text.Encoding.GetEncoding(srcName);
        System.Text.Encoding convToEncode = System.Text.Encoding.GetEncoding(convToName);
        byte[] bytes = srcEncode.GetBytes(value);
        //HttpContext.Current.Response.Write(bytes.Length.ToString());
        //HttpContext.Current.Response.End();
        System.Text.Encoding.Convert(srcEncode, convToEncode, bytes, 0, bytes.Length);
        return convToEncode.GetString(bytes);
    }
    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <param name="DFilePath">需要压缩的文件夹或者单个文件</param>
    /// <param name="DRARName">生成压缩文件的文件名</param>
    /// <param name="DRARPath">生成压缩文件保存路径</param>
    /// <returns></returns>
    /// 
    public void set_admin_url()
    {

        string d_page = "";
        try
        {
            d_page = HttpContext.Current.Request.Url.ToString().Substring(HttpContext.Current.Request.Url.ToString().LastIndexOf("/") + 1);
        }
        catch
        { }
        string admin_id = k_cookie("admin_id").ToString();
        DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + admin_id + "'");
        string huiyuanzu = "";
        if (dt.Rows.Count > 0)
        {
            huiyuanzu = dt.Rows[0]["u3"].ToString();
        }
        else
        {

            HttpContext.Current.Response.Redirect("err.aspx?err=对不起你还没有登陆,请重新登陆！&errurl=login.aspx");
        }




        string u3 = "";
        try
        {
            u3 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where zuming='" + huiyuanzu + "'").Rows[0]["quanxian"].ToString().Replace("&amp;", "&");
        }
        catch { }
        if (u3 != "" && d_page != "")
        {
            if (u3.IndexOf(d_page) > -1)
            {
                string page_sta = "";
                Regex reg = new Regex("{fzw:dui}", RegexOptions.Singleline);
                string[] aa = reg.Split(u3);
                for (int i = 0; i < aa.Length; i++)
                {
                    if (aa[i].ToString().IndexOf(d_page) > -1)
                    {
                        Regex reg1 = new Regex("{fzw:zu}", RegexOptions.Singleline);
                        string[] bb = reg1.Split(aa[i].ToString());

                        Regex reg2 = new Regex(",", RegexOptions.Singleline);
                        string[] cc = reg2.Split(bb[1].ToString());
                        if (cc[0] == "查看")
                        {
                            page_sta = "yes";
                        }

                    }

                }

                if (HttpContext.Current.Request.Url.ToString().IndexOf("main.aspx") == -1)
                {

                    if (page_sta == "")
                    {
                        HttpContext.Current.Response.Redirect("err.aspx?err=你没有此页面的操作权限，正在跳转后台管理乎页！&errurl=" + tihuan("default.aspx", "&", "fzw123") + "");
                    }
                }


            }
        }

    }
    public string get_ApplicationPath()
    {
        if (HttpContext.Current.Request.ApplicationPath.ToString() == "/")
        {
            return "";
        }
        else
        {
            return HttpContext.Current.Request.ApplicationPath.ToString();
        }
    }
    public string set_ApplicationPath(string g1)
    {
        if (g1.IndexOf(HttpContext.Current.Request.ApplicationPath.ToString()) > -1)
        {
            return g1;
        }
        else
        {
            if (HttpContext.Current.Request.ApplicationPath.ToString() == "/")
            {
                return "" + g1;
            }
            else
            {
                return HttpContext.Current.Request.ApplicationPath.ToString() + g1;
            }
        }
    }
    protected bool RAR(string DFilePath, string DRARName, string DRARPath)
    {
        String the_rar;
        RegistryKey the_Reg;
        Object the_Obj;
        String the_Info;
        ProcessStartInfo the_StartInfo;
        Process the_Process;
        try
        {
            the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRAR.exe\Shell\Open\Command");
            the_Obj = the_Reg.GetValue("");
            the_rar = the_Obj.ToString();
            the_Reg.Close();
            the_rar = the_rar.Substring(1, the_rar.Length - 7);
            the_Info = " a    " + " " + DRARName + "  " + DFilePath + " -ep1"; //命令 + 压缩后文件名 + 被压缩的文件或者路径
            the_StartInfo = new ProcessStartInfo();
            the_StartInfo.FileName = the_rar;
            the_StartInfo.Arguments = the_Info;
            the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            the_StartInfo.WorkingDirectory = DRARPath; //RaR文件的存放目录。
            the_Process = new Process();
            the_Process.StartInfo = the_StartInfo;
            the_Process.Start();
            the_Process.WaitForExit();
            the_Process.Close();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    /// <summary>
    /// 解压缩到指定文件夹 
    /// </summary>
    /// <param name="RARFilePath">压缩文件存在的目录 </param>
    /// <param name="RARFileName">压缩文件名称 </param>
    /// <param name="UnRARFilePath">解压到文件夹</param>
    /// <returns></returns>
    protected bool UnRAR(string RARFilePath, string RARFileName, string UnRARFilePath)
    {
        //解压缩
        String the_rar;
        RegistryKey the_Reg;
        Object the_Obj;
        String the_Info;
        ProcessStartInfo the_StartInfo;
        Process the_Process;
        try
        {
            the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRar.exe\Shell\Open\Command");
            the_Obj = the_Reg.GetValue("");
            the_rar = the_Obj.ToString();
            the_Reg.Close();
            the_rar = the_rar.Substring(1, the_rar.Length - 7);
            the_Info = @" X " + " " + RARFilePath + RARFileName + " " + UnRARFilePath;
            the_StartInfo = new ProcessStartInfo();
            the_StartInfo.FileName = the_rar;
            the_StartInfo.Arguments = the_Info;
            the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            the_Process = new Process();
            the_Process.StartInfo = the_StartInfo;
            the_Process.Start();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }
    //提交文件地址
    public string get_reg(string g1, string type)
    {
        g1 = NoHTML(g1);

        string t1 = "";
        int i = 0;
        string[] aa = type.Split('|');
        for (i = 0; i < aa.Length; i++)
        {
            if (t1 == "")
            {
                t1 = "http://.*?" + aa[i].ToString() + "|ftp://.*?" + aa[i].ToString() + "";
            }
            else
            {
                t1 = t1 + "|" + "http://.*?" + aa[i].ToString() + "|ftp://.*?" + aa[i].ToString() + "";
            }
        }
        string t2 = "";
        Regex reg = new Regex(t1, RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(g1);
        foreach (Match match in matches)
        {
            if (t2 == "")
            {
                t2 = match.ToString();
            }
            else
            {
                t2 = t2 + "|" + match.ToString();
            }
        }

        return t2;
    }
    //引用连接数据库的类文件
    my_conn my_c = new my_conn();
    //发邮件
    // email_to 接收人fzw9@qq.com  //标题  //内容
    public int WebMailTo(string email_to, string email_subject, string email_messageText)
    {
        DataTable dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");
        string email_server = dt.Rows[0]["u16"].ToString(); //smtp.qq.com
        string email_user = dt.Rows[0]["u17"].ToString(); //372895791@qq.com
        string email_password = dt.Rows[0]["u18"].ToString(); //qq password
        string email_from = dt.Rows[0]["u19"].ToString() + "<" + email_user + ">";  //网事如风
        //HttpContext.Current.Response.Write(email_to + "||" + email_messageText + "||" + email_subject);
        //HttpContext.Current.Response.End();
        MailMessage mm = new MailMessage();
        mm.BodyFormat = MailFormat.Html;
        mm.To = email_to;
        mm.From = email_from;
        mm.Subject = email_subject;
        mm.Body = email_messageText;
        //设置支持服务器验证
        mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
        //设置用户名
        mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", email_user);
        //设置用户密码
        mm.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", email_password);
        SmtpMail.SmtpServer = email_server;

        int mailok;
        try
        {
            SmtpMail.Send(mm);
            mailok = 1;
        }
        catch
        {
            mailok = 0;
        }
        return mailok;
    }
    //判断字符串安全
    public bool IsPass(string str)
    {
        if (str.Trim() == "" || str == null)
        {
            return true;
        }
        else
        {
            Regex re = new Regex(@"\s");
            str = re.Replace(str.Replace("%20", " "), " ");
            string pattern = @"select |insert |delete from |count\(|drop table|update |truncate \(|mid\(|char\(|xp_cmdshell|exec master|net localgroup administrators|:|net user|""|\'| or ";
            if (Regex.IsMatch(str, pattern))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    //特殊防注入处理字符串
    public string t_string(string g1)
    {
        g1 = g1.Replace("'", "''").Replace("*", "×");
        if (ProcessSqlStr(g1))
        {

            return g1.Trim();
        }
        else
        {
            return "";
        }
        return "";

    }
    //处理字符串
    public string c_string(string g1)
    {
        g1 = HttpContext.Current.Server.HtmlDecode(g1);
        g1 = g1.Replace("''", "'");
        g1 = g1.Replace("'", "''");
        return set_htmldecode(g1.Trim());

    }
    //处理字符串
    public string c_string1(string g1)
    {
        g1 = HttpContext.Current.Server.HtmlDecode(g1);
        g1 = g1.Replace("'", "''");
        // g1 = g1.Replace("'", "&apos;");
        return set_htmldecode(g1.Trim());

    }
    //防注入
    public bool ProcessSqlStr(string inputString)
    {
        string SqlStr = @"or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
        try
        {
            if ((inputString != null) && (inputString != String.Empty))
            {
                string str_Regex = @"\b(" + SqlStr + @")\b";

                Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                //string s = Regex.Match(inputString).Value; 
                if (true == Regex.IsMatch(inputString))
                    return false;

            }
        }
        catch
        {
            return false;
        }
        return true;
    }
    //分页类
    public PagedDataSource fenye(int g1, DataTable g2)
    {
        PagedDataSource fy = new PagedDataSource();
        fy.AllowPaging = true;
        fy.PageSize = g1;
        fy.CurrentPageIndex = 0;
        fy.DataSource = g2.DefaultView;
        return fy;
    }
    //删除文章内图片
    public void del_article_pic(string g1)
    {
        Regex g = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        foreach (Match math in m)
        {
            string imgUrl = math.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();

            if (imgUrl.ToString().IndexOf("http://") == -1)
            {
                del_pic((imgUrl));
            }
        }
    }
    //截取（两种）
    public string jiequ(string type, string aOrgStr, int aLength)
    {
        int intLen = aOrgStr.Length;
        int start = 0;
        int end = intLen;
        int single = 0;
        char[] chars = aOrgStr.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (System.Convert.ToInt32(chars[i]) > 255)
            {
                start += 2;
            }
            else
            {
                start += 1;
                single++;
            }
            if (start >= aLength)
            {

                if (end % 2 == 0)
                {
                    if (single % 2 == 0)
                    {
                        end = i + 1;
                    }
                    else
                    {
                        end = i;
                    }
                }
                else
                {
                    end = i + 1;
                }
                break;
            }
        }
        string temp = aOrgStr.Substring(0, end);
        string temp2 = aOrgStr.Remove(0, end);
        if (type == "yes")
        {
            if (temp2 != "")
            {
                temp = temp + "..";
            }
        }
        return temp;

    }

    ////删除单个图片
    public void del_pic(string g1)
    {
        if (g1 != "")
        {
            try
            {
                if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + g1))
                {
                    File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + g1);
                }
            }
            catch
            { }
        }
    }
    //去除HTML
    public string NoHTML(string Htmlstring)
    {

        //删除脚本   
        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        //删除HTML   
        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "mdash;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "amp;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "rdquo;", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "ldquo;", "\"", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "mdash;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, "hellip;", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&.*?;", "", RegexOptions.IgnoreCase);
        Htmlstring = System.Web.HttpContext.Current.Server.HtmlEncode(Htmlstring.Trim());
        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = Htmlstring.Replace("&amp;", "");

        return Htmlstring;
    }
    //获取字符串字节数
    public int string_b(string string_s)
    {
        return System.Text.Encoding.UTF8.GetByteCount(string_s);
    }
    //md5加密
    public string md5(string g1)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(g1, "md5");
    }


    public string tihuan(string g1, string g2, string g3)
    {
        return g1.Replace(g2, g3);
    }
    //生成cookie
    public void c_cookie(string cookie_value, string cookie_name)
    {
        admin_o_cookie(cookie_name);
        HttpContext.Current.Session[cookie_name] = HttpUtility.UrlEncode(cookie_value); ;
        ////HttpUtility.UrlEncode
        HttpCookie ck = new HttpCookie(ConfigurationSettings.AppSettings["web_url"].ToString() + cookie_name);
        ck.Value = HttpUtility.UrlEncode(cookie_value);
        //    ck.Expires = DateTime.Now.AddSeconds(60 * 60 * 24 * 30);//60秒*60分*2小时
        HttpContext.Current.Response.Cookies.Add(ck);


    }
    //生成cookie
    public void c_cookie1(string cookie_value, string cookie_name)
    {
        HttpContext.Current.Session[cookie_name] = HttpUtility.UrlEncode(cookie_value);
        //HttpUtility.UrlEncode
        HttpCookie ck = new HttpCookie(ConfigurationSettings.AppSettings["web_url"].ToString() + cookie_name);
        ck.Value = HttpUtility.UrlEncode(cookie_value);
        ck.Expires = DateTime.Now.AddSeconds(60 * 10);//60秒*60分*2小时
        HttpContext.Current.Response.Cookies.Add(ck);

    }
    public void c_cookie_dtime(string cookie_value, string cookie_name, int shijian)
    {
        HttpContext.Current.Session[cookie_name] = HttpUtility.UrlEncode(cookie_value);
        //HttpUtility.UrlEncode
        HttpCookie ck = new HttpCookie(ConfigurationSettings.AppSettings["web_url"].ToString() + cookie_name);
        ck.Value = HttpUtility.UrlEncode(cookie_value);
        ck.Expires = DateTime.Now.AddSeconds(shijian);//60秒*60分*2小时
        HttpContext.Current.Response.Cookies.Add(ck);

    }
    //清除cookie
    public void admin_o_cookie(string cookie_name)
    {
        HttpContext.Current.Session.Clear();

        HttpCookie ck = new HttpCookie(ConfigurationSettings.AppSettings["web_url"].ToString() + cookie_name);
        ck.Value = "";
        ck.Expires = DateTime.Now.AddSeconds(-1);//60秒*60分*2小时
        HttpContext.Current.Response.Cookies.Add(ck);


    }

    //读取cookie
    public string k_cookie(string cookie_name)
    {
        string neirong = "";
        try
        {
            neirong = HttpContext.Current.Session[cookie_name].ToString();
        }
        catch { }
        if (neirong == "")
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[ConfigurationSettings.AppSettings["web_url"].ToString() + cookie_name];
            return HttpUtility.UrlDecode(cookie.Value);
        }
        else
        {
            return HttpUtility.UrlDecode(neirong);
        }


    }



    //批量设置图片显示大小
    public string set_pic_xs(string g1, int kuan)
    {
        Regex g = new Regex(@"<img.*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        string g2 = g1;
        foreach (Match math in m)
        {
            Regex reg = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            Match tupian = reg.Match(math.ToString());
            string imgUrl = tupian.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();
            if (imgUrl.ToString().IndexOf("http://") == -1)
            {
                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));
                if (originalImage.Width > kuan)
                {
                    g2 = g2.Replace(math.ToString(), "<a class=\"example-image-link\" href=\"" + imgUrl + "\" data-lightbox=\"example-set\" \"><img src='" + imgUrl + "' width='" + kuan + "px'></a>");
                }
            }
        }
        return g2;
    }
    //正文中不显示图片
    public string set_pic_none(string g1)
    {
        string g2 = g1;
        Regex g = new Regex(@"<img.*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        foreach (Match math in m)
        {
            g2 = g2.Replace(math.ToString(), "");
        }
        return g2;

    }
    //批量设置图片显示大小
    public string set_pic_list(string g1, int kuan)
    {
        Regex g = new Regex(@"<img.*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        string g2 = "";
        foreach (Match math in m)
        {
            Regex reg = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            Match tupian = reg.Match(math.ToString());
            string imgUrl = tupian.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();
            g2 = g2 + "<img class=\"img\" src=\"" + imgUrl + "\" layer-src=\"" + imgUrl + "\" width='" + kuan + "px'/>";
        }

        return "<div class=\"pic_list\">" + g2 + "</div>";
    }
    //下载远程图片
    public string Download_pic(string g1)
    {
        Regex g = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        string g2 = g1;

        foreach (Match math in m)
        {
            string imgUrl = math.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();

            if (imgUrl.ToString().IndexOf("http://") > -1 || imgUrl.ToString().IndexOf("https://") > -1)
            {

                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                DateTime dy = DateTime.Now;
                string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile/Editor/" + tt1 + "/");
                WebClient wc = new WebClient();
                string newilt = HttpContext.Current.Request.PhysicalApplicationPath + "upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + imgUrl.Substring(imgUrl.LastIndexOf("."));

                try
                {
                    wc.DownloadFile(imgUrl, newilt);
                }
                catch
                { }
                g2 = g2.Replace(imgUrl, get_ApplicationPath() + "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + imgUrl.Substring(imgUrl.LastIndexOf(".")));

            }
        }
        return g2;
    }

    //单张图片大小
    /// <param name="g1">内容</param>
    /// <param name="picsize">大小</param>
    public string set_onepic_size(string g1, string picsize)
    {
        string g2 = g1;

        try
        {
            int towidth = int.Parse(picsize.Substring(0, picsize.IndexOf("*")));
            int toheight = int.Parse(picsize.Substring(picsize.IndexOf("*") + 1));

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(g1));
            if (originalImage.Width > towidth)
            {

                Random r = new Random();
                int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                DateTime dy = DateTime.Now;
                string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile/Upload/" + tt1 + "/");

                string file_name = get_ApplicationPath() + "/upfile/Upload/" + tt1 + "/" + d1 + Num1.ToString() + g1.Substring(g1.LastIndexOf("."));

                MakeThumbnail(g1, file_name, towidth, toheight, "Cut");

                string saveimg = get_ApplicationPath() + "/upfile/Upload/" + tt1 + "/" + d1 + Num1.ToString() + g1.Substring(g1.LastIndexOf("."));
                del_pic(g1);
                return file_name;
            }
        }
        catch { }

        return g2;
    }
    //批量改变图片大小
    /// <param name="g1">内容</param>
    /// <param name="picsize">大小</param>

    public string set_pic_size(string g1, string picsize)
    {
        string g2 = g1;

        try
        {
            Regex g = new Regex("<img.*?>|<input.*?>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            MatchCollection m = g.Matches(g1);

            foreach (Match math in m)
            {
                Regex reg = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg|GIF|JPG|PNG|BMP|JPEG)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Match matches = reg.Match(math.ToString());
                if (matches.ToString() != "")
                {
                    string imgUrl = matches.ToString();
                    HttpContext.Current.Response.Write(imgUrl + "<br>");
                    imgUrl = imgUrl.Replace("src", "");
                    imgUrl = imgUrl.Replace("\"", "");
                    imgUrl = imgUrl.Replace("'", "");
                    imgUrl = imgUrl.Replace("=", "");
                    imgUrl = imgUrl.Trim();
                    int towidth = int.Parse(picsize.Substring(0, picsize.IndexOf("*")));
                    int toheight = int.Parse(picsize.Substring(picsize.IndexOf("*") + 1));

                    System.Drawing.Image originalImage = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgUrl));
                    if (originalImage.Width > towidth)
                    {

                        Random r = new Random();
                        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
                        DateTime dy = DateTime.Now;
                        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                        string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                        Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile/Editor/" + tt1 + "/");

                        string file_name = get_ApplicationPath() + "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + imgUrl.Substring(imgUrl.LastIndexOf("."));

                        MakeThumbnail(imgUrl, file_name, towidth, toheight, "W");

                        string saveimg = get_ApplicationPath() + "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + imgUrl.Substring(imgUrl.LastIndexOf("."));
                        g2 = g2.Replace(math.ToString(), "<img src='" + saveimg + "'>");
                        del_pic(imgUrl);
                    }
                }
            }
        }
        catch
        { }

        return g2;
    }
    //设计绝对路径时的url问题
    public string set_http_url(string file_content, string home_url)
    {
        Regex reg = new Regex(@"href="".*?""|src="".*?""|src='.*?'|background-image:url[\s\S]*?\)", RegexOptions.Singleline);
        MatchCollection matches = reg.Matches(file_content);
        foreach (Match match in matches)
        {

            string url_string = match.ToString();
            url_string = url_string.Replace("href=", "").Replace("src=", "").Replace("\"", "").Replace("background-image:url(", "").Replace(")", "").Replace("'", "");
            if (url_string.IndexOf("http://") == -1 && url_string.IndexOf("ftp://") == -1)
            {
                if (url_string.IndexOf("$") != 0)
                {
                    file_content = file_content.Replace(match.ToString().Replace(" ", ""), match.ToString().Replace(url_string, home_url + "/" + url_string).Replace(" ", ""));
                }
                else
                {
                    file_content = file_content.Replace(match.ToString().Replace(" ", ""), match.ToString().Replace(url_string, url_string.Substring(1)).Replace(" ", ""));
                }
            }
            else
            {
                file_content = file_content.Replace(match.ToString(), match.ToString().Replace(" ", ""));
            }

        }

        return file_content;
    }
    //下载单个文件
    public string Downloads(string g1)
    {

        if (g1.IndexOf("http://") > -1 || g1.IndexOf("https://") > -1)
        {

            Random r = new Random();
            int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
            DateTime dy = DateTime.Now;
            string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
            string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "/upfile/Editor/" + tt1 + "/");
            WebClient wc = new WebClient();
            string newilt = HttpContext.Current.Request.PhysicalApplicationPath + "upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + g1.Substring(g1.LastIndexOf("."));
            try
            {
                wc.DownloadFile(g1, newilt);
            }
            catch
            { }
            return get_ApplicationPath() + "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + g1.Substring(g1.LastIndexOf("."));
        }
        return g1;
    }


    //上传软件
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
        if (File1.PostedFile.ContentLength > 0 && File1.PostedFile.ContentLength < (1024 * 1024 * 10))
        {
            if (file_Extension.IndexOf(file_name) > -1)
            {

                DateTime dy = DateTime.Now;
                string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
                string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                file_name = d1 + Num1.ToString() + file_name;
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "upfile//Upload//" + tt1 + "");
                string filepath = HttpContext.Current.Request.PhysicalApplicationPath + "upfile//Upload//" + tt1 + "//" + file_name;
                File1.PostedFile.SaveAs(filepath);
                return get_ApplicationPath() + "/upfile/Upload/" + tt1 + "/" + file_name + "";
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
    //采页面
    public string getWebFile(string url)
    {
        string bianma = ConfigurationSettings.AppSettings["bianma"].ToString();
        if (bianma == "")
        {
            try
            {
                bianma = url.Split('|')[1].ToString();
            }
            catch
            { }
        }
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Split('|')[0].ToString());
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        request.Method = "GET";
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(receiveStream, Encoding.GetEncoding(bianma));
        string SourceCode = readStream.ReadToEnd();
        response.Close();
        readStream.Close();
        return SourceCode;
        return "";
    }
    //采页面
    public string getWebFile1(string url)
    {
        string bianma = "UTF-8";
        if (bianma == "")
        {
            try
            {
                bianma = url.Split('|')[1].ToString();
            }
            catch
            { }
        }
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Split('|')[0].ToString());
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        request.Method = "GET";
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(receiveStream, Encoding.GetEncoding(bianma));
        string SourceCode = readStream.ReadToEnd();
        response.Close();
        readStream.Close();
        return SourceCode;
        return "";
    }
    public void set_xml(string g1, string g2)
    {
        XmlDocument webconfigDoc = new XmlDocument();
        string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\web.config";
        //设置节的xml路径                        
        string xPath = "/configuration/appSettings/add[@key='?']";

        //加载web.config文件
        webconfigDoc.Load(filePath);

        //找到要修改的节点
        XmlNode passkey = webconfigDoc.SelectSingleNode(xPath.Replace("?", g1));

        //设置节点的值
        passkey.Attributes["value"].InnerText = g2;

        //保存设置
        webconfigDoc.Save(filePath);
    }
    //提汉字
    public string hanzi(string u1, string u2)
    {
        string g1 = u1.ToLower();
        string t1 = "";
        int count_string = g1.Length;
        for (int i = 0; i < count_string; i++)
        {
            if (Regex.IsMatch(g1, @"^[\u4e00-\u9fa5]", RegexOptions.IgnoreCase))
            {
                t1 = t1 + Regex.Match(g1, @"^[\u4e00-\u9fa5]", RegexOptions.IgnoreCase);
                g1 = g1.Replace(g1.Substring(0, 1), "");
            }
            else if (Regex.IsMatch(g1, @"^[a-z]", RegexOptions.IgnoreCase))
            {
                t1 = t1 + Regex.Match(g1, @"^[a-z]", RegexOptions.IgnoreCase);
                g1 = g1.Replace(g1.Substring(0, 1), "");
            }
            else if (Regex.IsMatch(g1, @"^[0-9]", RegexOptions.IgnoreCase))
            {
                t1 = t1 + Regex.Match(g1, @"^[0-9]", RegexOptions.IgnoreCase);
                g1 = g1.Replace(g1.Substring(0, 1), "");
            }
            else
            {

                try
                {
                    g1 = g1.Replace(g1.Substring(0, 1), "");
                    t1 = t1 + u2;
                }
                catch
                { }

            }


        }
        return pinyin(t1, 50);
    }
    //转换拼音
    /// <summary>
    /// 定义拼音区编码数组
    /// </summary>

    private int[] pyValue = new int[]
                {
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
                };
    /// <summary>
    /// 定义数组
    /// </summary>
    private static string[] pyName = new string[]
                {
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
                };


    public string pinyin(string hzString, int maxLength)
    {

        if (string.IsNullOrEmpty(hzString))
            return null;
        if (maxLength <= 1)
            maxLength = 10;

        if (hzString.Length > maxLength)
        {
            hzString = hzString.Substring(0, maxLength);
        }
        Regex regex = new Regex(@"([a-zA-Z0-9\._]+)", RegexOptions.IgnoreCase);
        if (regex.IsMatch(hzString))
        {
            if (hzString.Equals(regex.Match(hzString).Groups[1].Value, StringComparison.OrdinalIgnoreCase))
            {
                return hzString;
            }
        }
        // 匹配中文字符
        regex = new Regex("^[\u4e00-\u9fa5]$");
        byte[] array = new byte[2];
        string pyString = "";
        int chrAsc = 0;
        int i1 = 0;
        int i2 = 0;
        char[] noWChar = hzString.ToCharArray();

        for (int j = 0; j < noWChar.Length; j++)
        {

            // 中文字符
            if (regex.IsMatch(noWChar[j].ToString()))
            {

                array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());

                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrAsc = i1 * 256 + i2 - 65536;
                if (chrAsc > 0 && chrAsc < 160)
                {
                    pyString += noWChar[j];
                }
                else
                {
                    // 修正部分文字
                    if (chrAsc == -9254) // 修正“圳”字
                        pyString += "Zhen";
                    else
                    {
                        for (int i = (pyValue.Length - 1); i >= 0; i--)
                        {
                            if (pyValue[i] <= chrAsc)
                            {
                                pyString += pyName[i];
                                break;
                            }
                        }
                    }
                }
            }
            // 非中文字符
            else
            {
                pyString += noWChar[j].ToString();
            }
        }


        return pyString.ToLower();

    }
    //是否有汉字
    public int get_cn(string g1)
    {
        Regex regex = new Regex("^[\u4E00-\u9FA5]+$");
        if (regex.IsMatch(g1))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    //页面分页
    //g1 表名
    //g2 查询条件
    //g3 返回条件
    //g4 排序方式
    //g5 条数
    //g6 当前页面
    //g7 数据控件名
    //g9 页面名
    //g8 Literal控件名
    public void page_list(string g1, string g2, string g3, string g4, int g5, int g6, Repeater g7, string g8, Literal g9)
    {
        int count_id = int.Parse(my_c.GetTable("select count(id) as count_id from " + g1 + " where " + g2 + "").Rows[0]["count_id"].ToString());
        int fenye_count = count_id / g5;
        float fenye_count1 = (float)count_id / (float)g5;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        string l1 = "";
        string sql = "";
        string list1 = "";
        string page_string = "";
        //数据控件的
        int d_page = g6 - 1;
        if (d_page == 0)
        {
            sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE  " + g2 + " " + g4 + "";
        }
        else
        {
            sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE id not in (SELECT TOP " + g5 * d_page + " id FROM " + g1 + " where " + g2 + " " + g4 + ") and  " + g2 + " " + g4 + "";
        }
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        g7.DataSource = my_c.GetTable(sql);
        g7.DataBind();
        //Literal控件的
        if (fenye_count > 10)
        {
            int stapage = 1;
            int overpage = 10;
            if (g6 > 5)
            {
                stapage = g6 - 4;
                if (g6 + 5 < fenye_count)
                {
                    overpage = g6 + 5;
                }
                else
                {
                    overpage = fenye_count;
                    stapage = fenye_count - 9;
                }
            }


            for (int i = stapage; i < overpage; i++)
            {
                if (i == g6)
                {
                    l1 = l1 + " <strong>" + i + "</strong>";
                }
                else
                {
                    page_string = "<A  href=\"" + g8 + "\">" + i + "</A>";
                    l1 = l1 + page_string.Replace("$page$", i.ToString());

                }

            }
        }
        else
        {
            for (int i = 1; i <= fenye_count; i++)
            {
                if (i == g6)
                {
                    l1 = l1 + " <strong>" + i + "</strong>";
                }
                else
                {
                    page_string = "<A  href=\"" + g8 + "\">" + i + "</A>";
                    l1 = l1 + page_string.Replace("$page$", i.ToString());

                }
            }

        }
        list1 = l1;
        if ((g6 - 1) == 0)
        {

        }
        else
        {
            page_string = "<a href=\"" + g8 + "\">上一页</a>";
            int shangyiye = g6 - 1;
            list1 = page_string.Replace("$page$", shangyiye.ToString()) + list1;
            page_string = "<a href=\"" + g8 + "\">首页</a>";
            list1 = page_string.Replace("$page$", "1") + list1;
        }

        if ((g6) < fenye_count)
        {
            page_string = "<a href=\"" + g8 + "\">下一页</a>";
            int xiayeye = g6 + 1;
            list1 = list1 + page_string.Replace("$page$", xiayeye.ToString());
            page_string = "<a href=\"" + g8 + "\">尾页</a>";
            list1 = list1 + page_string.Replace("$page$", fenye_count.ToString());
        }
        else
        {

        }

        list1 = "<strong> " + g5 + " 条/页 共 " + count_id.ToString() + " 条记录</strong>" + list1;
        g9.Text = list1;



    }
    public void page_list1(string g1, string g2, string g3, string g4, int g5, int g6, Repeater g7, string g8, Literal g9)
    {
        int count_id = int.Parse(my_c.GetTable("select count(id) as count_id from " + g1 + " where " + g2 + "").Rows[0]["count_id"].ToString());
        int fenye_count = count_id / g5;
        float fenye_count1 = (float)count_id / (float)g5;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        string l1 = "";
        string sql = "";
        string list1 = "";
        string page_string = "";
        //数据控件的
        int d_page = g6 - 1;
        if (d_page == 0)
        {
            sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE  " + g2 + " " + g4 + "";
        }
        else
        {
            sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE id not in (SELECT TOP " + g5 * d_page + " id FROM " + g1 + " where " + g2 + " " + g4 + ") and  " + g2 + " " + g4 + "";
        }
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        g7.DataSource = my_c.GetTable(sql);
        g7.DataBind();
        //Literal控件的
        if (fenye_count > 10)
        {
            int stapage = 1;
            int overpage = 10;
            if (g6 > 5)
            {
                stapage = g6 - 4;
                if (g6 + 5 < fenye_count)
                {
                    overpage = g6 + 5;
                }
                else
                {
                    overpage = fenye_count;
                    stapage = fenye_count - 9;
                }
            }


            for (int i = stapage; i < overpage; i++)
            {
                if (i == g6)
                {
                    l1 = l1 + " <strong>" + i + "</strong>";
                }
                else
                {
                    page_string = "<A  href=\"" + g8 + "\">" + i + "</A>";
                    l1 = l1 + page_string.Replace("$page$", i.ToString());

                }

            }
        }
        else
        {
            for (int i = 1; i <= fenye_count; i++)
            {
                if (i == g6)
                {
                    l1 = l1 + " <strong>" + i + "</strong>";
                }
                else
                {
                    page_string = "<A  href=\"" + g8 + "\">" + i + "</A>";
                    l1 = l1 + page_string.Replace("$page$", i.ToString());

                }
            }

        }
        list1 = l1;
        page_string = "<a href=\"" + g8 + "\">上一页</a>";
        int shangyiye = g6 - 1;
        list1 = page_string.Replace("$page$", shangyiye.ToString()) + list1;


        page_string = "<a href=\"" + g8 + "\">下一页</a>";
        int xiayeye = g6 + 1;
        list1 = list1 + page_string.Replace("$page$", xiayeye.ToString());



        g9.Text = list1;



    }
    public DataTable art_list(string g1, string g2, string g3, string g4, int g5, int g6, string g8, Literal g9)
    {

        int count_id = 0;
        try
        {
            count_id = int.Parse(my_c.GetTable("select count(id) as count_id from " + g1 + " where " + g2 + "").Rows[0]["count_id"].ToString());
        }
        catch
        {
            HttpContext.Current.Response.Write("select count(id) as count_id from " + g1 + " where " + g2 + "");
            HttpContext.Current.Response.End();
        }


        int fenye_count = count_id / g5;
        float fenye_count1 = (float)count_id / (float)g5;
        if (fenye_count1 > fenye_count)
        {
            fenye_count = fenye_count + 1;
        }

        string l1 = "";
        string sql = "";
        string list1 = "";
        string page_string = "";
        //数据控件的
        int d_page = g6 - 1;
        if (d_page == 0)
        {
            sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE  " + g2 + " " + g4 + "";
        }
        else
        {
            if (ConfigurationSettings.AppSettings["conn_type"].ToString() == "1")
            {
                //  sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE (id < (SELECT min(id) FROM (SELECT TOP " + g5 * d_page + " id FROM " + g1 + " where " + g2 + " " + g4 + ") AS T)) and  " + g2 + " " + g4 + "";
                sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE id not in (SELECT TOP " + g5 * d_page + " id FROM " + g1 + " where " + g2 + " " + g4 + ") and  " + g2 + " " + g4 + "";
            }
            else
            {
                sql = "SELECT TOP " + g5 + " " + g3 + " FROM " + g1 + " WHERE id not in (SELECT TOP " + g5 * d_page + " id FROM " + g1 + " where " + g2 + " " + g4 + ") and  " + g2 + " " + g4 + "";
            }
        }
        //HttpContext.Current.Response.Write(sql);
        //HttpContext.Current.Response.End();
        //g7.DataSource = my_c.GetTable(sql);
        //g7.DataBind();
        //Literal控件的
        if (fenye_count > 10)
        {
            int stapage = 1;
            int overpage = 10;
            if (g6 > 5)
            {
                stapage = g6 - 4;
                if (g6 + 5 < fenye_count)
                {
                    overpage = g6 + 5;
                }
                else
                {
                    overpage = fenye_count;
                    stapage = fenye_count - 9;
                }
            }


            for (int i = stapage; i < overpage; i++)
            {
                if (fenye_count > 1)
                {
                    if (i == g6)
                    {
                        l1 = l1 + " <strong>" + i + "</strong>";
                    }
                    else
                    {
                        page_string = "<A  href=\"" + g8 + "\">" + i + "</A>";
                        l1 = l1 + page_string.Replace("$page$", i.ToString());

                    }
                }


            }
        }
        else
        {
            for (int i = 1; i <= fenye_count; i++)
            {
                if (fenye_count > 1)
                {
                    if (i == g6)
                    {
                        l1 = l1 + " <strong>" + i + "</strong>";
                    }
                    else
                    {
                        page_string = "<A  href=\"" + g8 + "\">" + i + "</A>";
                        l1 = l1 + page_string.Replace("$page$", i.ToString());

                    }
                }

            }

        }
        list1 = l1;
        if ((g6 - 1) == 0)
        {

        }
        else
        {
            page_string = "<a href=\"" + g8 + "\">上一页</a>";
            int shangyiye = g6 - 1;
            list1 = page_string.Replace("$page$", shangyiye.ToString()) + list1;
            page_string = "<a href=\"" + g8 + "\">首页</a>";
            list1 = page_string.Replace("$page$", "1") + list1;
        }

        if ((g6) < fenye_count)
        {
            page_string = "<a href=\"" + g8 + "\">下一页</a>";
            int xiayeye = g6 + 1;
            list1 = list1 + page_string.Replace("$page$", xiayeye.ToString());
            page_string = "<a href=\"" + g8 + "\">尾页</a>";
            list1 = list1 + page_string.Replace("$page$", fenye_count.ToString());
        }
        else
        {

        }

        list1 = "<strong> " + g5 + " 条/页 共 " + count_id.ToString() + " 条记录</strong>" + list1;
        g9.Text = list1;

        DataTable dt2 = my_c.GetTable(sql);
        return dt2;

    }



    //获取本地字体名
    public void get_font(DropDownList g1)
    {
        System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
        foreach (System.Drawing.FontFamily family in fonts.Families)
        {
            g1.Items.Add(family.Name);
        }
    }

    //后台设置当前位置
    public string set_weizhi(string g1, string g2, string g3)
    {
        string g4 = "";
        if (g1 != "0")
        {
            string listid = get_dir(g1, g3);
            if (g1 == "0")
            {
                listid = "" + g1 + "";
            }
            else
            {
                listid = listid + "," + g1 + "";
            }
            DataTable dt = new DataTable();
            dt = my_c.GetTable("select u1,id from " + g3 + " where id in (" + listid + ") order by paixu desc,id asc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                g4 = g4 + " > <a href=\"" + g2.Replace("$classid$", dt.Rows[i]["id"].ToString()) + "\">" + dt.Rows[i]["u1"].ToString() + "</a>";
            }

        }
        return g4;
    }
    //public string set_weizhi(string g1, string g2,string g3)
    //{
    //    string g4 = "";
    //    if (g1 != "0")
    //    {
    //        string listid = get_shang_dir(g1, "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort");
    //        if (listid != "")
    //        {
    //            listid = listid + g1;
    //        }
    //        else
    //        {
    //            listid = g1;
    //        }
    //        //string[] cc = listid.Split(',');
    //        //for (int j = 0; j < cc.Length; j++)
    //        //{
    //        //    DataTable dt = new DataTable();
    //        //    dt = my_c.GetTable("select u1,id from " + g3 + " where id in (" + cc[j] + ")");
    //        //    for (int i = 0; i < dt.Rows.Count; i++)
    //        //    {
    //        //        g4 = g4 + " >> <a href=\"" + g2.Replace("$classid$", dt.Rows[i]["id"].ToString()) + "\">" + dt.Rows[i]["u1"].ToString() + "</a>";
    //        //    }

    //        //}
    //        DataTable dt = new DataTable();
    //        dt = my_c.GetTable("SELECT * FROM "+g3+" WHERE  Id in ("+ listid + ") order by charindex(',' + ltrim(Id) + ',', ',"+ listid + ",')");

    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            g4 = g4 + " &nbsp;>&nbsp;<a href=\"" + g2.Replace("$classid$", dt.Rows[i]["id"].ToString()) + "\">" + dt.Rows[i]["u1"].ToString() + "</a>";
    //        }


    //    }
    //    return g4;
    //}
    static string get_ding_dir_str = "";
    //列出最顶级目录
    public string get_ding_dir(string classid)
    {

        DataTable dt1 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Parameter where id=" + classid + " and classid>0");
        if (dt1.Rows.Count > 0)
        {


            if (dt1.Rows[0]["classid"].ToString() != "196")
            {
                get_ding_dir_str = dt1.Rows[0]["classid"].ToString();
                get_ding_dir(dt1.Rows[0]["classid"].ToString());
            }
        }

        return get_ding_dir_str;
    }
    //列出上级目录
    static string get_shang_dir_str = "";
    public string get_shang_dir(string classid, string tablename)
    {

        DataTable dt1 = my_c.GetTable("select * from " + tablename + " where id=" + classid + " and sort_id>0");
        if (dt1.Rows.Count > 0)
        {
            get_shang_dir_str = dt1.Rows[0]["sort_id"].ToString() + "," + get_shang_dir_str;
            get_shang_dir(dt1.Rows[0]["sort_id"].ToString(), tablename);
        }

        return get_shang_dir_str;
    }

    //列出所有下级目录
    //g1 ID号
    //g2 表名
    int jj = 0;
    public string get_dir(string g1, string tablename)
    {

        DataTable dt = my_c.GetTable("select sort_id from " + tablename + " where id=" + g1 + "");
        string listid = "";
        if (dt.Rows.Count > 0)
        {

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (jj == 0)
                {
                    listid = "" + dt.Rows[j]["sort_id"].ToString() + "";
                }
                else
                {
                    listid = listid + "," + dt.Rows[j]["sort_id"].ToString() + "";
                }
                jj = jj + 1;
                get_dir(dt.Rows[j]["sort_id"].ToString(), tablename);
            }
        }
        return listid;

    }

    //从文本中获取图片
    //g1 文本
    //g2 获取第几张
    public string get_images(string g1, int g2)
    {
        string t1 = "";
        int t2 = 1;
        Regex g = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        foreach (Match math in m)
        {
            string imgUrl = math.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();
            if (t2 == g2)
            {
                t1 = imgUrl;
            }
            t2++;

        }
        return t1;
    }

    //从文本中获取图片
    //g1 文本
    public string get_pic_string(string g1)
    {
        string t1 = "";
        int t2 = 1;
        Regex g = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        foreach (Match math in m)
        {
            string imgUrl = math.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();
            if (t1 == "")
            {
                t1 = imgUrl;
            }
            else
            {
                t1 = "|" + imgUrl;
            }

        }
        return t1;
    }

    //改变图片大小
    //g1 图片源
    //g2 图片大小
    public string set_images(string g1, string g2)
    {
        Random r = new Random();
        int Num1 = Convert.ToInt32(r.Next(9000)) + 1000;
        DateTime dy = DateTime.Now;
        string tt1 = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
        string d1 = tt1 + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
        string file_name = "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + "_m" + g1.Substring(g1.LastIndexOf("."));
        Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "/upfile/Editor/" + tt1);
        if (g1.ToLower().IndexOf("http://") > -1 || g1.ToLower().IndexOf("https://") > -1)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(g1, HttpContext.Current.Request.PhysicalApplicationPath + "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + g1.Substring(g1.LastIndexOf(".")));
            g1 = "/upfile/Editor/" + tt1 + "/" + d1 + Num1.ToString() + g1.Substring(g1.LastIndexOf("."));

        }
        int towidth = int.Parse(g2.Substring(0, g2.IndexOf("*")));
        int toheight = int.Parse(g2.Substring(g2.IndexOf("*") + 1));
        MakeThumbnail(g1, file_name, towidth, toheight, "Cut");
        return get_ApplicationPath() + file_name;


    }
    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="originalImagePath">源图路径（物理路径）</param>
    /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="mode">生成缩略图的方式</param>    
    public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {

        System.Drawing.Image originalImage = System.Drawing.Image.FromFile(HttpContext.Current.Request.PhysicalApplicationPath + originalImagePath);

        int towidth = width;
        int toheight = height;

        int x = 0;
        int y = 0;
        int ow = originalImage.Width;
        int oh = originalImage.Height;

        switch (mode)
        {
            case "HW"://指定高宽缩放（可能变形）                
                break;
            case "W"://指定宽，高按比例                    
                toheight = originalImage.Height * width / originalImage.Width;
                break;
            case "H"://指定高，宽按比例
                towidth = originalImage.Width * height / originalImage.Height;
                break;
            case "Cut"://指定高宽裁减（不变形）                
                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    oh = originalImage.Height;
                    ow = originalImage.Height * towidth / toheight;
                    y = 0;
                    x = (originalImage.Width - ow) / 2;
                }
                else
                {
                    ow = originalImage.Width;
                    oh = originalImage.Width * height / towidth;
                    x = 0;
                    y = (originalImage.Height - oh) / 2;
                }
                break;
            default:
                break;
        }

        //新建一个bmp图片
        System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //新建一个画板
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //设置高质量插值法
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //设置高质量,低速度呈现平滑程度
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //清空画布并以透明背景色填充
        g.Clear(System.Drawing.Color.Transparent);

        //在指定位置并且按指定大小绘制原图片的指定部分
        g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);
        //以jpg格式保存缩略图


        try
        {
            bitmap.Save(HttpContext.Current.Request.PhysicalApplicationPath + thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        catch (System.Exception e)
        {

        }
        finally
        {
            originalImage.Dispose();
            bitmap.Dispose();
            g.Dispose();
        }

    }
    //获取文章中所有图片地址
    //g1 文章内容
    public string get_article_pic(string g1)
    {
        string t1 = "";
        Regex g = new Regex(@"(src=(""|\')\S+\.(gif|jpg|png|bmp|jpeg)(""|\'))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        MatchCollection m = g.Matches(g1);
        foreach (Match math in m)
        {
            string imgUrl = math.ToString();
            imgUrl = imgUrl.Replace("src", "");
            imgUrl = imgUrl.Replace("\"", "");
            imgUrl = imgUrl.Replace("'", "");
            imgUrl = imgUrl.Replace("=", "");
            imgUrl = imgUrl.Trim();
            if (t1 == "")
            {
                t1 = imgUrl;
            }
            else
            {
                t1 = t1 + "|" + imgUrl;
            }

        }
        return t1;
    }
    //获取文中描述
    public string get_description(string g1)
    {
        g1 = c_string(g1);
        return jiequ("no", NoHTML(g1).ToString(), 100);
    }
    //获取文中关键词
    public string get_Keywords(string g1)
    {
        string t1 = "";
        DataTable dt = my_c.GetTable("select u1 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "key where u2 like '%推荐%'");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (g1.IndexOf(dt.Rows[i]["u1"].ToString()) > -1)
            {
                if (t1 == "")
                {
                    t1 = dt.Rows[i]["u1"].ToString();
                }
                else
                {
                    t1 = t1 + "|" + dt.Rows[i]["u1"].ToString();
                }
            }
        }
        return t1;
    }
    //内容中关键词
    public string set_content_key(string g1)
    {
        DataTable dt = my_c.GetTable("select u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "key where u2='推荐'");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["u2"].ToString() == "")
            {
                g1 = g1.Replace(dt.Rows[i]["u1"].ToString(), "<a href=''\''>" + dt.Rows[i]["u1"].ToString() + "</a>");
            }
            else
            {
                g1 = g1.Replace(dt.Rows[i]["u1"].ToString(), "<a href=''" + dt.Rows[i]["u2"].ToString() + "''>" + dt.Rows[i]["u1"].ToString() + "</a>");
            }
        }
        return g1;
    }
    //生成星星
    public string set_cool(string g1)
    {
        string t1 = "";
        for (int i = 1; i <= int.Parse(g1); i++)
        {
            t1 = t1 + "<img src='image/tb9.jpg'>";
        }
        return t1;
    }
    //求总条数
    public int get_count(string g1, string g2)
    {
        try
        {
            if (g2 == "")
            {
                return int.Parse(my_c.GetTable("select count(id) as count_id from " + g1 + "").Rows[0]["count_id"].ToString());
            }
            else
            {

                return int.Parse(my_c.GetTable("select count(id) as count_id from " + g1 + " " + g2 + "").Rows[0]["count_id"].ToString());
            }
        }
        catch
        {
            return 0;
        }
    }
    //求某个字段
    public string get_value(string g1, string g2, string g3)
    {
        //HttpContext.Current.Response.Write("select top 1 " + g1 + " from " + g2 + " " + g3 + " order by id desc");
        //HttpContext.Current.Response.End();
        try
        {
            return my_c.GetTable("select top 1 " + g1 + " from " + g2 + " " + g3 + " order by id desc").Rows[0][g1].ToString();
        }
        catch
        {
            return "";
        }
    }
    //生成图标
    public string get_pic(string g1)
    {
        if (g1 == "原创")
        {
            return "<img src=\"image/tb59.jpg\" />";
        }
        else if (g1 == "转载")
        {
            return "<img src=\"image/tb60.jpg\" />";
        }
        else
        {
            return "<img src=\"image/tb58.jpg\" />";
        }
    }
    //加1
    //g1 点击数
    //g2 字段
    //g3 ID号
    //g4 表名
    public void set_dianji(string g1, string g2, string g3, string g4)
    {
        int dianji = int.Parse(g1) + 1;
        my_c.genxin("update " + g4 + " set " + g2 + "=" + dianji + " where id=" + g3);
    }

    //设置链接
    public string set_url(string g1, string g2)
    {
        if (g1 == "")
        {
            return HttpContext.Current.Server.HtmlEncode(g2);
        }
        else
        {
            return HttpContext.Current.Server.HtmlEncode(g1);
        }
    }
    //如果g2不为空，就显示g2，否则显示g1
    public string set_ad(string g1, string g2)
    {
        if (g2 == "")
        {
            return g1;
        }
        else
        {
            return g2;
        }
    }

    //无限级输出到DropDownList中
    int i = 0;

    public void dr1(string t1, int t2, string Model_id, DropDownList xialai)
    {

        DataTable dt1 = my_c.GetTable("select id,u1,u2 from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "sort where Sort_id=" + t1 + " and Model_id=" + Model_id + "");

        if (dt1.Rows.Count > 0)
        {


            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string bb = "";

                for (int j1 = 0; j1 < t2; j1++)
                {
                    bb = bb + "— ";
                }
                xialai.Items.Insert(jj, bb + dt1.Rows[j]["u1"].ToString());
                xialai.Items[jj].Value = dt1.Rows[j]["id"].ToString();
                jj = jj + 1;
                int tt1 = t2 + 1;
                dr1(dt1.Rows[j]["id"].ToString(), tt1, Model_id, xialai);
            }
        }

    }

    //加点击
    //g1表名
    //g2 ID号
    public void set_read(string tablename, string id)
    {
        int renqi = 1;
        try
        {
            renqi = int.Parse(my_c.GetTable("select renqi from " + tablename + " where id=" + id + "").Rows[0]["renqi"].ToString()) + 1;
        }
        catch { }
        my_c.genxin("update " + tablename + " set renqi=" + renqi + " where id=" + id);
    }

    public string html_encode(string g1)
    {
        return System.Web.HttpUtility.UrlEncode(g1);
    }
    public string html_Decode(string g1)
    {
        return System.Web.HttpUtility.HtmlDecode(g1);
    }

    //登陆检查
    public void set_user(string cookie_name, string page_name, string tip_page)
    {
        try
        {
            if (k_cookie(cookie_name) == "")
            {
                HttpContext.Current.Response.Redirect("" + tip_page + "?err=会员登陆后才可以操作！&errurl=" + tihuan(page_name, "&", "fzw123") + "");
            }
        }
        catch
        {
            HttpContext.Current.Response.Redirect("" + tip_page + "?err=会员登陆后才可以操作！&errurl=" + tihuan(page_name, "&", "fzw123") + "");
        }
    }
    public void Set_FreeTextBox(string type)
    {
        if (type == "admin")
        {
            DateTime dy = DateTime.Now;
            string upfile_path = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
            string mulu = "";

            if (mulu != "")
            {

                mulu = "/upfile/" + mulu + "/";
                c_cookie(mulu, "upfile");
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + mulu);
            }
            else
            {
                mulu = "/upfile/Editor/";
                c_cookie(mulu, "upfile");
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + mulu + upfile_path + "/");

            }



        }
        else
        {
            DateTime dy = DateTime.Now;
            string upfile_path = dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString();
            c_cookie("user_editor/" + upfile_path, "upfile");
            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "/upfile/user_editor/" + upfile_path + "/");
        }
    }


    public string set_mode()
    {
        DataTable xml_dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");
        return xml_dt.Rows[0]["u35"].ToString();
    }

















}
