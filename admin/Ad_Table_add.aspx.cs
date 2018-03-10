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
public partial class admin_Ad_Table_add : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string type = "";
    DataTable dt = new DataTable();
    public int i1 = 3;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            DateTime dy = DateTime.Now;
            
            int i = 0;
            
 
            this.TextBox4.Text = dy.ToString();
            this.TextBox6.Text = dy.ToString();
            this.TextBox10.Text = dy.ToString();
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "edit")
            {

                if (Request.QueryString["u4"].ToString() == "图文")
                {
                    i1 = 1;
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad where id=" + Request.QueryString["id"].ToString());
                    this.TextBox1.Text = dt.Rows[0]["u1"].ToString();
                    this.TextBox2.Text = dt.Rows[0]["u2"].ToString();
                    this.FreeTextBox1.Text = dt.Rows[0]["u3"].ToString();
                    this.TextBox4.Text = dt.Rows[0]["dtime"].ToString();
                    this.TextBox6.Text = dt.Rows[0]["overtime"].ToString();
                    this.TextBox7.Text = dt.Rows[0]["u5"].ToString();
                }
                else
                {
                    i1 = 2;
                    dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad where id=" + Request.QueryString["id"].ToString());
                    this.TextBox5.Text = dt.Rows[0]["u1"].ToString();
                    this.TextBox8.Text = dt.Rows[0]["u2"].ToString();
                    this.TextBox10.Text = dt.Rows[0]["overtime"].ToString();
                    this.TextBox11.Text = dt.Rows[0]["u5"].ToString();

                    string[] aa = Regex.Split(dt.Rows[0]["u3"].ToString(), "yige");

                    if (dt.Rows[0]["u3"].ToString()!="")
                    {
                        for (i = 0; i < aa.Length; i++)
                        {
                            int ii1 = i + 1;
                            string[] bb = Regex.Split(aa[i].ToString(), "yiban");
                            Literal1.Text = Literal1.Text + "图名：<input type=\"text\" name=\"xiangce_title" + ii1 + "\" id=\"xiangce_title" + ii1 + "\" value=\"" + bb[0] + "\" />&nbsp;链接：<input type=\"text\" name=\"xiangce_name" + ii1 + "\" id=\"xiangce_name" + ii1 + "\" value=\"" + bb[1] + "\" />&nbsp;图片地址：<input type=\"text\" name=\"xiangce_pic" + ii1 + "\" id=\"xiangce_pic" + ii1 + "\" value=\"" + bb[2] + "\" />&nbsp;&nbsp;<input id=\"Button2\" class=\"button\" type=\"button\" value=\"上传图片\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=xiangce_pic" + ii1 + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/><br />";
                        }
                        Label1.Text = aa.Length.ToString();
                    }
                    else
                    {
                        Literal1.Text = "图名：<input type=\"text\" name=\"xiangce_title1\" id=\"xiangce_title1\" value=\"\" />&nbsp;链接：<input type=\"text\" name=\"xiangce_name1\" id=\"xiangce_name1\" value=\"\" />&nbsp;图片地址：<input type=\"text\" name=\"xiangce_pic1\" id=\"xiangce_pic1\" value=\"\" />&nbsp;&nbsp;<input id=\"Button2\" class=\"button\" type=\"button\" value=\"上传图片\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=xiangce_pic1','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/><br />";
                    }
                }

            }
            else if (type == "del")
            {
                DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad where id in (" + Request.QueryString["id"].ToString() + ")");
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    my_c.genxin("delete from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad where id in(" + dt.Rows[i]["id"].ToString() + ")");
                    try
                    {
                        try
                        {
                            File.Delete(HttpContext.Current.Request.PhysicalApplicationPath+"/upfile/ad/" + dt.Rows[i]["u1"].ToString() + ".js");
                        }
                        catch { }
                        my_b.del_article_pic(dt.Rows[i]["u3"].ToString());
                    }
                    catch
                    { }
                }

                Response.Redirect("err.aspx?err=删除广告成功，正在跳转广告管理页面！&errurl=" + my_b.tihuan("Ad_Table.aspx", "&", "fzw123") + "");
            }
            else
            {

                this.TextBox1.Text = "ad_" + dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString() + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                this.TextBox5.Text = "ad_" + dy.Year.ToString() + dy.Month.ToString() + dy.Day.ToString() + dy.Hour.ToString() + dy.Minute.ToString() + dy.Second.ToString();
                string u4 = "";
                try
                {
                    u4 = Request.QueryString["u4"].ToString();
                }
                catch
                { }

                if (u4 == "图文")
                {
                    i1 = 1;
                }
                else if (u4.IndexOf("换图")>-1)
                {
                    i1 = 2;
                    Literal1.Text = "图名：<input type=\"text\" name=\"xiangce_title1\" id=\"xiangce_title1\" value=\"\" />&nbsp;链接：<input type=\"text\" name=\"xiangce_name1\" id=\"xiangce_name1\" value=\"\" />&nbsp;图片地址：<input type=\"text\" name=\"xiangce_pic1\" id=\"xiangce_pic1\" value=\"\" />&nbsp;&nbsp;<input id=\"Button2\" class=\"button\" type=\"button\" value=\"上传图片\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=xiangce_pic1','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/><br />";
                }
                else
                {
                    i1 = 3;
                }
            }
        }
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = my_b.c_string(this.FreeTextBox1.Text);
        string u4 = my_b.c_string(Request.QueryString["u4"].ToString());
        string dtime = my_b.c_string(this.TextBox4.Text);
        string overtime = my_b.c_string(this.TextBox6.Text);
        string u5 = my_b.c_string(this.TextBox7.Text);

        

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',dtime='" + dtime + "',overtime='" + overtime + "',u5='" + u5 + "',u6='" + my_b.c_string(u3) + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改广告成功！马上跳转到广告管理页面！&errurl=" + my_b.tihuan("Ad_Table.aspx", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad (u1,u2,u3,u4,dtime,overtime,u5,u6) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + dtime + "','" + overtime + "','" + u5 + "','" + my_b.c_string(u3) + "')");
            Response.Redirect("err.aspx?err=增加广告成功！马上跳转到广告管理页面！&errurl=" + my_b.tihuan("Ad_Table.aspx", "&", "fzw123") + "");
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        int i = 0;
        string u1 = my_b.c_string(this.TextBox5.Text);
        string u2 = my_b.c_string(this.TextBox8.Text);
        string u3 = my_b.c_string(this.TextBox9.Text);
        string u4 = my_b.c_string(Request.QueryString["u4"].ToString());
        string overtime = my_b.c_string(this.TextBox10.Text);
        string u5 = my_b.c_string(this.TextBox11.Text);
        if (u5 == "")
        {
            u5 = "308*242";
        }
        string i_width = "";
        string i_height = "";
        string[] cc = u5.Split('*');
        try
        {
            i_width = cc[0].ToString();
        }
        catch
        {
            i_width = "308";
        }
        try
        {
            i_height = cc[1].ToString();
        }
        catch
        {
            i_height = "242";
        }

        string[] aa = Regex.Split(u3, "yige");
        string pics = "";
        string links = "";
        string texts = "";
        if (u3 != "")
        {
            for (i = 0; i < aa.Length; i++)
            {
                
                string[] bb = Regex.Split(aa[i].ToString(), "yiban");
                if (pics == "")
                {
                    links = bb[1].ToString();
                    pics = bb[2].ToString();
                    texts = bb[0].ToString();
                }
                else
                {
                    links = links+"|"+bb[1].ToString();
                    pics = pics+"|" + bb[2].ToString();
                    texts = texts + "|" + bb[0].ToString();
                }
            }

        }


       // string tt1 = "<script type='text/javascript'>  var focus_width=" + i_width + ";\r\n var focus_height=" + i_height + ";\r\n var text_height=0;\r\n var swf_height = focus_height+text_height;\r\n  var pics=\"" + pics + "\";\r\n var links=\"" + links + "\";\r\n  var texts=\"" + texts + "\";\r\n document.write('<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" width=\"'+ focus_width +'\" height=\"'+ swf_height +'\">');\r\n document.write('<param name=\"allowScriptAccess\" value=\"sameDomain\"><param name=\"movie\" value=\"/upfile/data/focus.swf\"><param name=\"quality\" value=\"hight\"><param name=\"bgcolor\" value=\"#F0F0F0\">');\r\n document.write('<param name=\"menu\" value=\"false\"><param name=wmode value=\"opaque\">');\r\n document.write('<param name=\"FlashVars\" value=\"pics='+pics+'&links='+links+'&texts='+texts+'&borderwidth='+focus_width+'&borderheight='+focus_height+'&textheight='+text_height+'\">');\r\n document.write('</object>'); </script>";
        string tt1 = "";
        if (u4 == "换图（样式二）")
        {
            tt1 = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile/data/flash_ad1.txt", System.Text.Encoding.UTF8).Replace("$width$", i_width).Replace("$height$", i_height).Replace("$titles$", texts).Replace("$imgs$", pics).Replace("$urls$", links.Replace("&", "%26"));
        }
        else if (u4 == "换图（样式二）")
        {
            tt1 = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile/data/flash_ad.txt", System.Text.Encoding.UTF8).Replace("$width$", i_width).Replace("$height$", i_height).Replace("$titles$", texts).Replace("$imgs$", pics).Replace("$urls$", links.Replace("&", "%26"));
        }
         else
        {
            tt1 = File.ReadAllText(HttpContext.Current.Request.PhysicalApplicationPath + @"upfile/data/flash_ad3.txt", System.Text.Encoding.UTF8).Replace("$width$", i_width).Replace("$height$", i_height).Replace("$titles$", texts).Replace("$imgs$", pics).Replace("$urls$", links.Replace("&", "%26"));
        }

        try
        {
            type = Request.QueryString["type"].ToString();
        }
        catch
        { }

        if (type == "edit")
        {
          
            my_c.genxin("update " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad set u1='" + u1 + "',u2='" + u2 + "',u3='" + u3 + "',u4='" + u4 + "',overtime='" + overtime + "',u5='" + u5 + "',u6='" + my_b.c_string(tt1) + "' where id=" + Request.QueryString["id"].ToString());
            Response.Redirect("err.aspx?err=修改广告成功！马上跳转到广告管理页面！&errurl=" + my_b.tihuan("Ad_Table.aspx", "&", "fzw123") + "");
        }
        else
        {
            my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "ad (u1,u2,u3,u4,overtime,u5,u6) values('" + u1 + "','" + u2 + "','" + u3 + "','" + u4 + "','" + overtime + "','" + u5 + "','" + my_b.c_string(tt1) + "')");
            Response.Redirect("err.aspx?err=增加广告成功！马上跳转到广告管理页面！&errurl=" + my_b.tihuan("Ad_Table.aspx", "&", "fzw123") + "");
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

        Response.Redirect("Ad_Table_add.aspx?u4=" + RadioButtonList1.SelectedItem.Value + "");
    }
}
