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

public partial class _Default : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch
            { }
            if (type == "qq")
            {
                #region QQ
                string nickname = Server.HtmlDecode(Request.QueryString["nickname"].ToString());
                string figureurl = Server.HtmlDecode(Request.QueryString["figureurl"].ToString());
                string xingbie = Server.HtmlDecode(Request.QueryString["xingbie"].ToString());
                string suozaidi = Server.HtmlDecode(Request.QueryString["suozaidi"].ToString());
           
                string yonghuming = "";
                if (figureurl != "")
                {
                    try
                    {
                        yonghuming = figureurl.Substring(0, figureurl.LastIndexOf("/"));
                        yonghuming = yonghuming.Substring(yonghuming.LastIndexOf("/") + 1);
                    }
                    catch { }
                }
              
                
                DataTable dt = new DataTable();
                if (my_b.get_count("sl_user", "where qqid='" + yonghuming + "'") > 0)
                {
                    dt = my_c.GetTable("select * from sl_user where qqid='" + yonghuming + "'");
                    my_b.c_cookie(dt.Rows[0]["yonghuming"].ToString(), "user_name");
                    my_b.c_cookie(dt.Rows[0]["mima"].ToString(), "user_pwd");
                    Response.Redirect("/");
                }
                else
                {
                    //string sql = "insert into sl_user(yonghuming,mima,xingming,touxiang,suozaidi,xingbie,qqid) values('" + yonghuming + "','','" + nickname + "','" + figureurl + "','" + suozaidi + "','" + xingbie + "','" + yonghuming + "')";


                    //my_c.genxin(sql);

                    //my_c.genxin("insert into sl_system (u1,u2,u3,u4) values('" + yonghuming + "','此会员（" + yonghuming + "）注册成功!操作页面" + Request.Url.ToString() + "','" + Request.UserHostAddress.ToString() + "','QQ会员')");

                   // my_b.c_cookie(yonghuming, "yonghuming");
                    my_b.c_cookie(nickname, "xingming");
                    my_b.c_cookie(figureurl, "touxiang");
                    my_b.c_cookie(suozaidi, "suozaidi");
                    my_b.c_cookie(xingbie, "xingbie");
                    my_b.c_cookie(yonghuming, "qqid");
                }
               

                Response.Redirect("/single.aspx?m=login");
                #endregion
            }

            if (type == "weixin")
            {
                #region 微信
                string nickname = Server.HtmlDecode(Request.QueryString["nickname"].ToString());
                string figureurl = Server.HtmlDecode(Request.QueryString["figureurl"].ToString());
                string xingbie = Server.HtmlDecode(Request.QueryString["xingbie"].ToString());
                string suozaidi = Server.HtmlDecode(Request.QueryString["suozaidi"].ToString());
                string yonghuming = Server.HtmlDecode(Request.QueryString["yonghuming"].ToString());

              
                DataTable dt = new DataTable();
                if (my_b.get_count("sl_user", "where openid='" + yonghuming + "'") > 0)
                {
                    dt = my_c.GetTable("select * from sl_user where openid='" + yonghuming + "'");
                    my_b.c_cookie(dt.Rows[0]["yonghuming"].ToString(), "user_name");
                    my_b.c_cookie(dt.Rows[0]["mima"].ToString(), "user_pwd");
                    Response.Redirect("/");
                }
                else
                {
                    // my_b.c_cookie(yonghuming, "yonghuming");
                    my_b.c_cookie(nickname, "xingming");
                    my_b.c_cookie(figureurl, "touxiang");
                    my_b.c_cookie(suozaidi, "suozaidi");
                    my_b.c_cookie(xingbie, "xingbie");
                    my_b.c_cookie(yonghuming, "openid");
                }

                
                    Response.Redirect("/single.aspx?m=login");
                    #endregion
                

            }

            if (type == "weibo")
            {
                #region 微博
                string nickname = Server.HtmlDecode(Request.QueryString["nickname"].ToString());
                string figureurl = Server.HtmlDecode(Request.QueryString["figureurl"].ToString());
                string xingbie = Server.HtmlDecode(Request.QueryString["xingbie"].ToString());
                string suozaidi = Server.HtmlDecode(Request.QueryString["suozaidi"].ToString());
                string yonghuming = Server.HtmlDecode(Request.QueryString["id"].ToString());
                if (xingbie == "m")
                {
                    xingbie = "男";
                }
                else
                {
                    xingbie = "女";
                }
             

                DataTable dt = new DataTable();
                if (my_b.get_count("sl_user", "where weiboid='" + yonghuming + "'") > 0)
                {
                    dt = my_c.GetTable("select * from sl_user where weiboid='" + yonghuming + "'");
                    my_b.c_cookie(dt.Rows[0]["yonghuming"].ToString(), "user_name");
                    my_b.c_cookie(dt.Rows[0]["mima"].ToString(), "user_pwd");
                    Response.Redirect("/");
                }
                else
                {
                   // my_b.c_cookie(yonghuming, "yonghuming");
                    my_b.c_cookie(nickname, "xingming");
                    my_b.c_cookie(figureurl, "touxiang");
                    my_b.c_cookie(suozaidi, "suozaidi");
                    my_b.c_cookie(xingbie, "xingbie");
                    my_b.c_cookie(yonghuming, "weiboid");
                }


                Response.Redirect("/single.aspx?m=login");
                #endregion
            }

        }
    }

}
