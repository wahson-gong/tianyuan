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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using WFsoft.wfLibrary;

public partial class text : System.Web.UI.Page
{
   
    my_basic my_b = new my_basic();
    protected void Page_Load(object sender, EventArgs e)
    {
      
        wfVerifyImage m_wfVerifyImage = new wfVerifyImage();

        //允许出现阿拉伯数字 
        m_wfVerifyImage.wfAllowNumber = true;

        //允许出现小写字母 
        m_wfVerifyImage.wfAllowLowercaseLetter = false;

        //允许出现大写字母 
        m_wfVerifyImage.wfAllowUppercaseLetter = false;

        //允许出现汉字 
        m_wfVerifyImage.wfAllowCharacter = false;

        //验证码图片宽度 
        m_wfVerifyImage.wfWidth = 100;

        //验证码图片高度 
        m_wfVerifyImage.wfHeight = 40;

        //验证码图片背景颜色 
        m_wfVerifyImage.wfBackgroundColor = Color.White;

        //验证码图片将要生成的字符个数 
        m_wfVerifyImage.wfMaxVerifyCharacterNumber = 4;

        //验证码图片干扰点个数 
        m_wfVerifyImage.wfDisturbPointNumber = 50;

        //验证码图片干扰点颜色 
        m_wfVerifyImage.wfDisturbPointColor = Color.Black;

        //验证码图片干扰线数量 
        m_wfVerifyImage.wfDisturbLineNumber = 3;

        //验证码图片干扰线颜色 
        m_wfVerifyImage.wfDisturbLineColor = Color.Black;

        //验证码图片验证文字的渐变颜色 
        m_wfVerifyImage.wfVerifyCodeBeginColor = Color.AliceBlue;
        m_wfVerifyImage.wfVerifyCodeEndColor = Color.Purple;

        //验证码图片验证文字的字体名称 
        m_wfVerifyImage.wfVerifyCodeFontName = "宋体";

        //验证码图片验证文字的字体大小 
        m_wfVerifyImage.wfVerifyCodeFontSize = 16;


        //生成验证码图片到流 
        m_wfVerifyImage.wfGenerate(Response.OutputStream, ImageFormat.Gif);

        //获取本次生成的验证码 
        //Session["g_strVerifyCode"] = m_wfVerifyImage.wfVerifyCode;
        my_b.c_cookie(m_wfVerifyImage.wfVerifyCode, "yzm");
    }
}

  

