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
public partial class admin_login : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    static public string quanjulanmu="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (my_b.get_count(ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin", "") == 0)
                {
                    my_b.set_xml("install", "0");
                    Response.Redirect("err.aspx?err=程序未安装，请安装在进后台操作！&errurl=" + my_b.tihuan("../install/default.aspx", "&", "fzw123") + "");
                   
                }
            }
            catch
            {
                my_b.set_xml("install", "0");
                Response.Redirect("err.aspx?err=程序未安装，请安装在进后台操作！&errurl=" + my_b.tihuan("../install/default.aspx", "&", "fzw123") + "");
                
            }

            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            if (type == "login")
            {
                //登录 start
                string u1 = my_b.c_string(Request["TextBox1"].ToString());
                string u2 = my_b.md5(my_b.c_string(Request["TextBox2"].ToString()));
                if (!my_b.IsPass(u1))
                {
                    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + u1 + "','非法用户在注入攻击!','" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "')");
                    //  Response.Redirect("err.aspx?err=操作不正确！&errurl=" + my_b.tihuan("/", "&", "fzw123") + "");
                    Response.Write("sql");
                    Response.End();
                }

                if (my_c.GetTable("select top 1 id from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + u1 + "' and u2='" + u2 + "' order by id desc").Rows.Count > 0)
                {
                    my_b.c_cookie(u1, "admin_id");
                    my_b.c_cookie(u2, "admin_pwd");
                    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + u1 + "','此会员（" + u1 + "）登陆成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','管理员登陆')");
                    //获取会员组权限
                    DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + u1 + "'");
                    string huiyuanzu = dt.Rows[0]["u3"].ToString();
                    string u3 = "";
                    try
                    {
                        u3 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where zuming='" + huiyuanzu + "'").Rows[0]["quanxian"].ToString().Replace("&amp;", "&");
                    }
                    catch { }
                    //end
                    //登陆时生成菜单
                    string t1 = "";
                    string t3 = "";
                    DataTable dt1 = new DataTable();
                    dt1 = my_c.GetTable("select  * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=0 and u4='显示' order by id");
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (u3 == "")
                        {
                            
                            //当权限值为空时 start
                            t1 = t1 + "<li class=\"nLi\"><h3><a href=\"#\"><em class=\"ficon " + dt1.Rows[i]["u5"].ToString() + "\"></em> " + dt1.Rows[i]["u1"].ToString() + "</a></h3><ul class=\"sub\">";
                            DataTable dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + dt1.Rows[i]["id"].ToString() + " and u4='显示' order by id");
                            string t2 = "";
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                string page_sta = "yes";
                                //根据page_sta值处理
                                if (page_sta == "yes")
                                {
                                    string url_str = Request.Url.ToString().Substring(Request.Url.ToString().LastIndexOf("/") + 1);
                                    if (url_str.IndexOf("Class_Table.aspx?Model_id") > -1)
                                    {
                                        url_str = url_str.Replace("Class_Table.aspx?Model_id", "articles.aspx?Model_id");
                                    }

                                    if (dt2.Rows.Count - 1 == j)
                                    {
                                        if (url_str == dt2.Rows[j]["u3"].ToString())
                                        {
                                            t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\" class='on'>" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                        }
                                        else
                                        {
                                            t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                        }
                                    }
                                    else
                                    {
                                        if (url_str == dt2.Rows[j]["u3"].ToString())
                                        {
                                            t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\" class='on'>" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                        }
                                        else
                                        {
                                            t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                        }

                                    }

                                }

                                //end

                            }
                            if (t2 == "")
                            {
                                t1 = "";
                            }
                            else
                            {
                                t3 = t3 + t1 + t2 + "</ul></li>";
                                t1 = "";
                            }
                            //当权限值为空时 end
                        }
                        else
                        {

                            //当权限值不为空时 start
                            t1 = t1 + "<li class=\"nLi\"><h3><a href=\"#\"><em class=\"ficon " + dt1.Rows[i]["u5"].ToString() + "\"></em> " + dt1.Rows[i]["u1"].ToString() + "</a></h3><ul class=\"sub\">";
                            DataTable dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + dt1.Rows[i]["id"].ToString() + " and u4='显示' order by id");
                            string t2 = "";
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (u3.IndexOf(dt2.Rows[j]["u3"].ToString()) > -1)
                                {
                                    string page_sta = "";
                                    Regex reg = new Regex("{fzw:dui}", RegexOptions.Singleline);
                                    string[] aa = reg.Split(u3);
                                    for (int i1 = 0; i1 < aa.Length; i1++)
                                    {
                                        if (aa[i1].ToString().IndexOf(dt2.Rows[j]["u3"].ToString()) > -1)
                                        {
                                            Regex reg1 = new Regex("{fzw:zu}", RegexOptions.Singleline);
                                            string[] bb = reg1.Split(aa[i1].ToString());

                                            Regex reg2 = new Regex(",", RegexOptions.Singleline);
                                            string[] cc = reg2.Split(bb[1].ToString());
                                            if (cc[0] == "查看")
                                            {
                                                page_sta = "yes";
                                            }

                                        }

                                    }

                                    //根据page_sta值处理
                                    if (page_sta == "yes")
                                    {
                                        string url_str = Request.Url.ToString().Substring(Request.Url.ToString().LastIndexOf("/") + 1);
                                        if (url_str.IndexOf("Class_Table.aspx?Model_id") > -1)
                                        {
                                            url_str = url_str.Replace("Class_Table.aspx?Model_id", "articles.aspx?Model_id");
                                        }


                                        if (dt2.Rows.Count - 1 == j)
                                        {
                                            if (url_str == dt2.Rows[j]["u3"].ToString())
                                            {
                                                t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\" class=\"on\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                            }
                                            else
                                            {
                                                t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                            }

                                        }
                                        else
                                        {
                                            if (url_str == dt2.Rows[j]["u3"].ToString())
                                            {
                                                t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\" class=\"on\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                            }
                                            else
                                            {
                                                t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                            }

                                        }

                                    }

                                    //end
                                }

                            }
                            if (t2 == "")
                            {
                                t1 = "";
                            }
                            else
                            {

                                t3 = t3 + t1 + t2 + "</ul></li>";
                                // Response.Write(t3+ "<br><br><br><br>");
                                t1 = "";
                            }
                            //当权限值不为空时 end
                        }

                    }
                    //Response.Write(t3);
                    //Response.End();
                    t3 = "<li class=\"nLi home\"><h3><a href=\"default.aspx\"><em class=\"ficon ficon-shouye\"></em> 用户首页</a></h3></li>" + t3;
                    Cache["date"] = t3;

                    //登陆时生成菜单 end

                    Response.Write("ok");
                    Response.End();
                }
                else
                {
                    my_c.genxin("insert into " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "system (u1,u2,u3,u4) values('" + u1 + "','此管理员（" + u1 + "）登陆失败，帐号或密码错误!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','管理员登陆')");
                    // Response.Redirect("err.aspx?err=登陆失败，帐号或密码错误，请重新登陆！&errurl=" + my_b.tihuan("login.aspx", "&", "fzw123") + "");
                    Response.Write("err");
                    Response.End();
                }
                //登录 end
            }


        }
    }




 
}
