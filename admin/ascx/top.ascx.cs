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
using System.Text.RegularExpressions;
public partial class ascx_top : System.Web.UI.UserControl
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    int i = 0;
    public string yemian = "";
    public string touxiang = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string Model_id = "";
            try
            {
                Model_id = Request.QueryString["Model_id"].ToString();
            }
            catch { }
            if (Model_id != "")
            {
                string u2 = my_b.get_value("u2", "" + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Model", "where id=" + HttpContext.Current.Request.QueryString["Model_id"].ToString());
                Page.Title = u2;
            }

            DataTable dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admin where u1='" + my_b.k_cookie("admin_id") + "'");
            touxiang = dt.Rows[0]["touxiang"].ToString();
            string huiyuanzu = dt.Rows[0]["u3"].ToString();
            string u3 = "";
            try
            {
                u3 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "admingroup where zuming='" + huiyuanzu + "'").Rows[0]["quanxian"].ToString().Replace("&amp;", "&");
            }
            catch { }
            //登陆时生成菜单
            string t1 = "";
            string t3 = "";
            DataTable dt1 = new DataTable();
            dt1 = my_c.GetTable("select  * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=0 and u4='显示' order by paixu asc, id asc");

            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (u3 == "")
                {
                 
                    //当权限值为空时 start
                    t1 = t1 + "<li class=\"nLi\"><h3><a href=\"#\"><em class=\"ficon " + dt1.Rows[i]["u5"].ToString() + "\"></em> " + dt1.Rows[i]["u1"].ToString() + "</a></h3><ul class=\"sub\">";
                    DataTable dt2 = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "Columns where classid=" + dt1.Rows[i]["id"].ToString() + " and u4='显示' order by paixu asc, id asc");
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
                                #region 处理路径
                                //string url_str = Request.Url.ToString().Substring(Request.Url.ToString().LastIndexOf("/") + 1);
                                //if (yemian != "")
                                //{

                                //    url_str = yemian;
                                //}
                                //if (url_str.IndexOf("Class_Table.aspx?Model_id") > -1)
                                //{
                                //    url_str = url_str.Replace("Class_Table.aspx?Model_id", "articles.aspx?Model_id");
                                //}
                                //else
                                //{
                                //    url_str = url_str.Replace("articles.aspx?Model_id", "Class_Table.aspx?Model_id");
                                //}



                                //if (dt2.Rows.Count - 1 == j)
                                //{
                                //    if (url_str.IndexOf(dt2.Rows[j]["u3"].ToString()) > -1 || dt2.Rows[j]["u3"].ToString().IndexOf(url_str) > -1)
                                //    {
                                //        t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\" class=\"on\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                //    }
                                //    else
                                //    {
                                //        t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                //    }

                                //}
                                //else
                                //{

                                //    if (url_str.IndexOf(dt2.Rows[j]["u3"].ToString()) > -1 || dt2.Rows[j]["u3"].ToString().IndexOf(url_str) > -1)
                                //    {
                                //        t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\" class=\"on\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                //    }
                                //    else
                                //    {
                                //        t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";
                                //    }

                                //}
                                #endregion
                                #region 不处理路径
                                if (dt2.Rows.Count - 1 == j)
                                {
                                    t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";

                                }
                                else
                                {
                                    t2 = t2 + "<li><a href=\"" + dt2.Rows[j]["u3"].ToString() + "\">" + dt2.Rows[j]["u1"].ToString() + "</a></li>";

                                }
                                #endregion
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
        }
    }


}
