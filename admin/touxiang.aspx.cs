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
public partial class admin_shangchuan : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    string file_type = "";
    public string editname = "";
    public string pic = "";
    public string typefile = "";
    public string newTypeFile = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");

            try
            {
                file_type = Request.QueryString["Extension"].ToString();
            }
            catch
            {
                file_type = "图片";
            }

            try
            {
                editname = Request.QueryString["editname"].ToString();
            }
            catch
            { }

            try
            {
                pic = Request.QueryString["pic"].ToString();
            }
            catch
            { }

            if (file_type == "视频")
            {
                file_type = dt.Rows[0]["u7"].ToString();
            }
            else if (file_type == "音频")
            {
                file_type = dt.Rows[0]["u8"].ToString();
            }
            else if (file_type == "软件")
            {
                file_type = dt.Rows[0]["u9"].ToString();
            }
            else if (file_type == "其它")
            {
                file_type = dt.Rows[0]["u10"].ToString();
            }
            else
            {
                file_type = dt.Rows[0]["u6"].ToString();
            }
            string[] aa = file_type.Split('|');
            string fa_1 = "";
            string fa_2 = "";
            string fa_3 = "";
            typefile = @" ($fa_1$)"",""$fa_2$""";
            newTypeFile = @" ($fa_1$)"",""$fa_2$"",""$fa_3$""";
            string file_typ1 = "";
            try
            {
                file_typ1 = Request.QueryString["Extension"].ToString();
            }
            catch
            {
                file_typ1 = "图片";
            }
            file_typ1 = @""""+file_typ1;
            typefile = file_typ1 + typefile;
            newTypeFile = file_typ1 + newTypeFile;
            for (int j = 0; j < aa.Length; j++)
            {
                if (fa_1 != "")
                {
                    fa_1 = fa_1  +"," + "*" + aa[j].ToString();
                }
                else
                {
                    fa_1 =  "*" + aa[j].ToString();
                }
                fa_2 = fa_2  + "*" + aa[j].ToString() + ";";
                if (fa_3 != "")
                {
                    fa_3 = fa_3 + ";." + aa[j].ToString().Replace(".", "").ToUpper(); ;
                }
                else
                {
                    fa_3 = "." + aa[j].ToString().Replace(".", "").ToUpper(); ;
                }

            }
            typefile = typefile.Replace("$fa_1$", fa_1).Replace("$fa_2$", fa_2).Replace("$fa_3$", fa_3);
            newTypeFile = newTypeFile.Replace("$fa_1$", fa_1).Replace("$fa_2$", fa_2).Replace("$fa_3$", fa_3);
 
        }
    }
 
}
