using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Web.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Web.Caching;
/// <summary>
/// my_from 的摘要说明
/// </summary>
public class my_from
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string set_peizhi(string id)
    {
        if (id != "0")
        {
            string t1 = "";
            DataTable dt = my_c.GetTable("select * from sl_peizhi where classid=" + id + " order by id");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["u2"].ToString() == "单选按钮组")
                {
                    t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\">";
                    string[] aa = Regex.Split(dt.Rows[i]["u3"].ToString(), "\r\n");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        t1 = t1 + "<input type=\"radio\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\" value=\"" + aa[j] + "\" />" + aa[j] + "";
                    }
                    t1 = t1 + "</td></tr>";
                }
                else if (dt.Rows[i]["u2"].ToString() == "下拉列表框")
                {
                    t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><select name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\">";
                    string[] aa = Regex.Split(dt.Rows[i]["u3"].ToString(), "\r\n");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        t1 = t1 + "<option value=\"" + aa[j] + "\">" + aa[j] + "</option>";
                    }
                    t1 = t1 + "</select></td></tr>";
                }
                else if (dt.Rows[i]["u2"].ToString() == "多选按钮组")
                {
                    t1 = t1 + "<tr><td class=\"tRight\" width=\"80px\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\">";
                    string[] aa = Regex.Split(dt.Rows[i]["u3"].ToString(), "\r\n");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        t1 = t1 + "<input type=\"checkbox\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  value=\"" + aa[j] + "\" />" + aa[j] + "&nbsp;&nbsp;";
                    }
                    t1 = t1 + "</td></tr>";
                }
                else if (dt.Rows[i]["u2"].ToString() == "上传框")
                {
                    t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><input type=\"text\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  width=\"200px\" value=\"" + dt.Rows[i]["u3"].ToString() + "\"/>&nbsp; &nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"" + dt.Rows[i]["u1"].ToString() + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=from_peizhi" + i.ToString() + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/></td></tr>";
                }
                else if (dt.Rows[i]["u2"].ToString() == "文本域")
                {
                    t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><textarea name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\" cols=\"80\" rows=\"6\">" + dt.Rows[i]["u3"].ToString() + "</textarea></td></tr>";
                }
                else
                {
                    t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><input type=\"text\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  width=\"400px\" size=\"100\" value=\"" + dt.Rows[i]["u3"].ToString() + "\"/></td></tr>";
                }
            }

            return "<table class=\"cTable_2 table\">" + t1 + "</table>";
        }
        else
        {
            return "<table class=\"cTable_2 table\"><tr><td>没有内容</td></tr></table>";
        }

    }

    public string get_peizhi(string id, string g2)
    {
        int i1 = 1;
        string[] peizhi = Regex.Split(g2, "next");

        string t1 = "";
        DataTable dt = my_c.GetTable("select * from sl_peizhi where classid=" + id + " order by id");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["u2"].ToString() == "单选按钮组")
            {
                t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\">";
                string[] aa = Regex.Split(dt.Rows[i]["u3"].ToString(), "\r\n");
                for (int j = 0; j < aa.Length; j++)
                {
                    if (aa[j] == peizhi[i1].ToString())
                    {
                        t1 = t1 + "<input type=\"radio\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\" value=\"" + aa[j] + "\"  checked=\"checked\"/>" + aa[j] + "";
                    }
                    else
                    {
                        t1 = t1 + "<input type=\"radio\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\" value=\"" + aa[j] + "\" />" + aa[j] + "";
                    }
                }
                t1 = t1 + "</td></tr>";
            }
            else if (dt.Rows[i]["u2"].ToString() == "下拉列表框")
            {
                t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><select name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\">";
                string[] aa = Regex.Split(dt.Rows[i]["u3"].ToString(), "\r\n");
                for (int j = 0; j < aa.Length; j++)
                {
                    if (aa[j] == peizhi[i1].ToString())
                    {
                        t1 = t1 + "<option value=\"" + aa[j] + "\" selected=\"selected\">" + aa[j] + "</option>";
                    }
                    else
                    {
                        t1 = t1 + "<option value=\"" + aa[j] + "\">" + aa[j] + "</option>";
                    }
                }
                t1 = t1 + "</select></td></tr>";
            }
            else if (dt.Rows[i]["u2"].ToString() == "多选按钮组")
            {
                t1 = t1 + "<tr><td class=\"tRight\" width=\"80px\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\">";
                string[] aa = Regex.Split(dt.Rows[i]["u3"].ToString(), "\r\n");
                string[] bb = Regex.Split(peizhi[i1].ToString(), ",");
                for (int j = 0; j < aa.Length; j++)
                {
                    //try
                    //{
                    if (aa[j] == bb[j])
                    {
                        t1 = t1 + "<input type=\"checkbox\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  value=\"" + aa[j] + "\" checked=\"checked\"/>" + aa[j] + "&nbsp;&nbsp;";
                    }
                    else
                    {
                        t1 = t1 + "<input type=\"checkbox\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  value=\"" + aa[j] + "\" />" + aa[j] + "&nbsp;&nbsp;";
                    }
                    //}
                    //catch
                    //{ }

                }
                t1 = t1 + "</td></tr>";
            }
            else if (dt.Rows[i]["u2"].ToString() == "上传框")
            {
                t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><input type=\"text\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  width=\"200px\" value=\"" + peizhi[i1].ToString() + "\"/>&nbsp; &nbsp;<input id=\"Button4\" class=\"button\" type=\"button\" value=\"" + dt.Rows[i]["u1"].ToString() + "\"  onClick=\"window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=from_peizhi" + i.ToString() + "','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');\"/></td></tr>";
            }
            else if (dt.Rows[i]["u2"].ToString() == "文本域")
            {
                t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><textarea name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\" cols=\"80\" rows=\"6\">" + peizhi[i1].ToString() + "</textarea></td></tr>";
            }
            else
            {

                t1 = t1 + "<tr><td class=\"tRight\">" + dt.Rows[i]["u1"].ToString() + "：</td><td colspan=\"2\"><input type=\"text\" name=\"from_peizhi" + i.ToString() + "\" id=\"from_peizhi" + i.ToString() + "\"  width=\"400px\"  size=\"100\" value=\"" + peizhi[i1].ToString() + "\"/></td></tr>";
            }

            i1++;
        }

        return "<table class=\"cTable_2 table\">" + t1 + "</table>";
    }

    public string get_peizhi_value(string g1, string g2, string g3)
    {
        //HttpContext.Current.Response.Write(g1);
        //HttpContext.Current.Response.Write(g2);
        //HttpContext.Current.Response.Write(g3);
        //HttpContext.Current.Response.End();
        int i = 0;
        string t1 = "";
        DataTable dt = my_c.GetTable("select u1 from sl_peizhi where classid=" + g1 + " order by id");
        for (i = 0; i < dt.Rows.Count; i++)
        {
            t1 = t1 + "next" + dt.Rows[i]["u1"].ToString();
        }
        string[] aa = Regex.Split(t1, "next");
        string[] bb = Regex.Split(g2, "next");
        int j = 0;
        for (i = 0; i < aa.Length; i++)
        {
            if (aa[i].ToString() == g3)
            {
                j = i;
            }
        }
        return bb[j].ToString();
    }

    public string get_peizhi_all(string g1, string g2)
    {
        string t2 = "";
        int i = 0;
        string t1 = "";
        DataTable dt = my_c.GetTable("select u1 from sl_peizhi where classid=" + g1 + " order by id");
        for (i = 0; i < dt.Rows.Count; i++)
        {
            t1 = t1 + "next" + dt.Rows[i]["u1"].ToString();
        }
        string[] aa = Regex.Split(t1, "next");
        string[] bb = Regex.Split(g2, "next");
        for (i = 1; i < aa.Length; i++)
        {
            t2 = t2 + "<tr><td style=\"width: 100px; background-color:#ebe1cf\">&nbsp;<span style=\"font-weight:700\">" + aa[i].ToString() + "</span></td><td style=\"background-color:#ebe1cf\">&nbsp;" + bb[i].ToString() + "</td></tr>";
        }
        return "<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" style=\"width: 100%; line-height:30px; background-color:#a47d40\">" + t2 + "</table>";
    }
}
