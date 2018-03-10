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
using System.Text.RegularExpressions;

public partial class inc_WebUpload_FileUploader : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    public string type = "";
    public string editname = "";
    public string typefile = "";
    public string newTypeFile = "";
    string file_type = "";
    public string fangwen = "";
    public string filesize = "1";
    public string kuangao = "";
    public string crop = "false";
    public string towidth = "0";
    public string toheight = "0";
    public string anniu_str = "";
    public string tip_str = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            fangwen = my_b.set_fangwen().ToString();
            dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");

            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            {

            }

            try
            {
                kuangao = Request.QueryString["kuangao"].ToString();
            }
            catch
            {

            }
          
            if (kuangao != "")
            {
                my_b.c_cookie(kuangao, "kuangao");

                crop = "true";
                towidth = kuangao.Substring(0, kuangao.IndexOf("*"));
                toheight = kuangao.Substring(kuangao.IndexOf("*") + 1);
            }
            if (type == "soft")
            {
                filesize = "1000";
            }
            else
            {
                filesize = "2";
            }

            try
            {
                editname = Request.QueryString["editname"].ToString();
            }
            catch
            { }

            if (type == "video")
            {
                typefile = dt.Rows[0]["u7"].ToString();
                newTypeFile = "video/*";
                anniu_str = "点击选择视频";
              
            }
            else if (type == "mp3")
            {
                typefile = dt.Rows[0]["u8"].ToString();
                newTypeFile = "mp3/*";
                anniu_str = "点击选择音频";
            }
            else if (type == "soft")
            {
                typefile = dt.Rows[0]["u9"].ToString();
                newTypeFile = "soft";
                anniu_str = "点击选择文件";
            }
            else if (type == "other")
            {
                typefile = dt.Rows[0]["u10"].ToString();
                newTypeFile = "other/*";
                anniu_str = "点击选择文件";
            }
            else
            {
                typefile = dt.Rows[0]["u6"].ToString();
                newTypeFile = "image/*";
                anniu_str = "点击选择图片";
            }

            typefile = typefile.Replace("|", ",").Replace(".", "");

            try
            {
                editname = Request.QueryString["editname"].ToString();
            }
            catch
            { }
        }
    }
}