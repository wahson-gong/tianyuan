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
using System.Text;
public partial class Default2 : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    public string set_fangjianbianhao(string fangjianbianhao)
    {
        return fangjianbianhao.Replace("[", "[[]");
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = "";
            try
            {
                type = Request.QueryString["type"].ToString();
            }
            catch { }
            string renqun = "";
            try
            {
                renqun = Request.QueryString["renqun"].ToString();
            }
            catch { }

            string laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            DateTime dy = DateTime.Now;
            string dy1 = "";
            if (type == "xianlu")
            {
                string shijian = "";
                try
                {
                    shijian = Request.QueryString["shijian"].ToString();
                    dy = DateTime.Parse(shijian);
                }
                catch { }

                dy1 = dy.Year.ToString() + "-" + dy.Month.ToString() + "-" + dy.Day.ToString();
                //线路价格
                DataTable sl_lvyou_jiage = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + dy1.ToString() + "' and jieshushijian>='" + dy1.ToString() + "' order by id desc");
                if (sl_lvyou_jiage.Rows.Count > 0)
                {
                    Response.Write(sl_lvyou_jiage.Rows[0][renqun].ToString());
                }
                else
                {
                    dy = dy.AddDays(15);
                    string dy2 = dy.Year.ToString() + "-" + dy.Month.ToString() + "-" + dy.Day.ToString();
                    DataTable sl_lvyou_jiage_ = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian>'" + dy1.ToString() + shang_time + "' and kaishishijian<='" + dy2.ToString() + xia_time + "' order by id desc");
                    if (sl_lvyou_jiage_.Rows.Count > 0)
                    {
                        Response.Write(sl_lvyou_jiage_.Rows[0][renqun].ToString());
                    }
                    else
                    {
                        Response.Write("暂无");
                    }

                }
                //end

            }
            if (type == "orderxianlu")
            {
                string shijian = "";
                try
                {
                    shijian = Request.QueryString["shijian"].ToString();
                    dy = DateTime.Parse(shijian);
                }
                catch { }

                dy1 = dy.Year.ToString() + "-" + dy.Month.ToString() + "-" + dy.Day.ToString();
                //线路价格
                DataTable sl_lvyou_jiage = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + dy1.ToString() + "' and jieshushijian>='" + dy1.ToString() + "' order by id desc");

                if (sl_lvyou_jiage.Rows.Count > 0)
                {
                    if (sl_lvyou_jiage.Rows[0][renqun].ToString() == "0")
                    {
                        Response.Write("暂无");
                    }
                    else
                    {
                        Response.Write(sl_lvyou_jiage.Rows[0][renqun].ToString());
                    }
                }
                else
                {
                    //DataTable sl_lvyou_jiage_ = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " order by id desc");
                    //if (sl_lvyou_jiage_.Rows.Count > 0)
                    //{
                    //    Response.Write(sl_lvyou_jiage_.Rows[0][renqun].ToString());
                    //}
                    Response.Write("暂无");
                }
                //end

            }
            else if (type == "orderly")
            {
                string shijian = "";
                try
                {
                    shijian = Request.QueryString["shijian"].ToString();
                    dy = DateTime.Parse(shijian);
                }
                catch { }

                dy1 = dy.Year.ToString() + "-" + dy.Month.ToString() + "-" + dy.Day.ToString();
                //线路价格
                DataTable sl_lvyou_jiage = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian between '" + dy1.ToString() + shang_time + "' and '" + dy1.ToString() + xia_time + "' order by id desc");


                if (sl_lvyou_jiage.Rows.Count > 0)
                {
                    if (int.Parse(sl_lvyou_jiage.Rows[0]["kucun"].ToString()) == 0)
                    {
                        Response.Write("1");
                    }
                    if (sl_lvyou_jiage.Rows[0]["shifoudianxun"].ToString() == "是")
                    {
                        Response.Write("2");
                    }
                }

                //end

            }
            else if (type == "qianzheng")
            {
                //线路价格
                DataTable sl_qianzheng_jiage = my_c.GetTable("select top 1 * from sl_qianzheng_jiage where laiyuanbianhao=" + laiyuanbianhao + " order by jiage asc,id desc");
                if (sl_qianzheng_jiage.Rows.Count > 0)
                {
                    Response.Write(sl_qianzheng_jiage.Rows[0][renqun].ToString());
                }
                else
                {
                    Response.Write("0");

                }
                //end

            }
            else if (type == "jiudian")
            {
                //酒店价格
                string shijian = "";
                try
                {
                    shijian = Request.QueryString["shijian"].ToString();
                    dy = DateTime.Parse(shijian);
                }
                catch
                {
                    dy = DateTime.Now;
                }

                dy1 = dy.Year.ToString() + "-" + dy.Month.ToString() + "-" + dy.Day.ToString();

                DataTable sl_hotel_jiage = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + "  and kaishishijian between '" + dy1.ToString() + shang_time + "' and kaishishijian between '" + dy1.ToString() + shang_time + "' and '" + dy1.ToString() + xia_time + "' order by jiage asc,id desc");

                if (sl_hotel_jiage.Rows.Count > 0)
                {
                    Response.Write(sl_hotel_jiage.Rows[0][renqun].ToString());
                }
                else
                {
                    DataTable sl_hotel_jiage_ = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and jieshushijian>='" + dy1.ToString() + "' order by jiage asc,id desc");
                    Response.Write("暂无");
                    if (sl_hotel_jiage_.Rows.Count > 0)
                    {
                        // Response.Write(sl_hotel_jiage_.Rows[0][renqun].ToString());
                    }

                }
                //end
            }
            else if (type == "jiudian_view")
            {
                try
                {

                    if (Request.QueryString["shijian"].ToString() == "")
                    {
                        dy1 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                    }
                    else
                    {
                        dy1 = Request.QueryString["shijian"].ToString();
                    }
                }
                catch
                {
                    dy1 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                }

                string fangjianbianhao = set_fangjianbianhao(Request.QueryString["fangjianbianhao"].ToString());
                //酒店详细价格

                DataTable sl_hotel_jiage = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and   kaishishijian<='" + dy1.ToString() + shang_time + "' and jieshushijian>='" + dy1.ToString() + xia_time + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");
                //Response.Write("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and   kaishishijian<='" + dy1.ToString() + shang_time + "' and jieshushijian>='" + dy1.ToString() + xia_time + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");
                //Response.End();
                if (sl_hotel_jiage.Rows.Count > 0)
                {
                    Response.Write(sl_hotel_jiage.Rows[0][renqun].ToString());
                }
                else
                {
                    Response.Write("暂无");
                    //DataTable sl_hotel_jiage_ = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and jieshushijian>='" + dy1.ToString() + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");

                    //if (sl_hotel_jiage_.Rows.Count > 0)
                    //{
                    //    Response.Write(sl_hotel_jiage_.Rows[0][renqun].ToString());
                    //}
                    //else
                    //{
                    //    DataTable sl_hotel_jiage1_ = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and  fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");

                    //    if (sl_hotel_jiage1_.Rows.Count > 0)
                    //    {
                    //        Response.Write(sl_hotel_jiage1_.Rows[0][renqun].ToString());
                    //    }
                    //}

                }
                //end
            }
            else if (type == "jiudian_order")
            {

                try
                {

                    if (Request.QueryString["ruzhushijian"].ToString() == "")
                    {
                        dy1 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                    }
                    else
                    {
                        dy1 = Request.QueryString["ruzhushijian"].ToString();
                    }
                }
                catch
                {
                    dy1 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                }
                string dy2 = "";
                try
                {

                    if (Request.QueryString["lidianshijian"].ToString() == "")
                    {
                        dy2 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                    }
                    else
                    {
                        dy2 = Request.QueryString["lidianshijian"].ToString();
                    }
                }
                catch
                {
                    dy2 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                }

                string fangjianbianhao = set_fangjianbianhao(Request.QueryString["fangjianbianhao"].ToString());
                //酒店详细价格
                //Response.Write("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + dy1.ToString() + "' and jieshushijian>='" + dy1.ToString() + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");
                //Response.End();

                DateTime kaishishijian = DateTime.Parse(dy1);
                DateTime jieshushijian = DateTime.Parse(dy2);
                TimeSpan span = jieshushijian - kaishishijian;

                float zongjia = 0;
                for (int i = 0; i < span.Days; i++)
                {
                    string shijian = kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString();

                    DataTable sl_hotel_jiage = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian.ToString() + shang_time + "' and jieshushijian>='" + shijian.ToString() + xia_time + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");
                    //  Response.Write("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian.ToString() + shang_time + "' and jieshushijian>='" + shijian.ToString() + xia_time + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc【"+ sl_hotel_jiage.Rows[0][renqun].ToString() + "】<br><br><br>");
                    if (sl_hotel_jiage.Rows.Count > 0)
                    {
                        zongjia = zongjia + float.Parse(sl_hotel_jiage.Rows[0][renqun].ToString());
                    }

                }

                Response.Write(zongjia);
                //end
            }
            else if (type == "orderjd")
            {
                try
                {

                    if (Request.QueryString["shijian"].ToString() == "")
                    {
                        dy1 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                    }
                    else
                    {
                        dy1 = Request.QueryString["shijian"].ToString();
                    }
                }
                catch
                {
                    dy1 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                }
                string dy2 = "";
                try
                {

                    if (Request.QueryString["shijian1"].ToString() == "")
                    {
                        dy2 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                    }
                    else
                    {
                        dy2 = Request.QueryString["shijian1"].ToString();
                    }
                }
                catch
                {
                    dy2 = DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().LastIndexOf(" "));
                }

                string fangjianbianhao = set_fangjianbianhao(Request.QueryString["fangjianbianhao"].ToString());
                //酒店详细价格


                DateTime kaishishijian = DateTime.Parse(dy1);
                DateTime jieshushijian = DateTime.Parse(dy2);
                TimeSpan span = jieshushijian - kaishishijian;
                //  Response.Write(span.Days);
                float jiage = 0;
                for (int i = 0; i < span.Days; i++)
                {
                    string shijian = kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString();

                    DataTable sl_hotel_jiage = my_c.GetTable("select top 1 * from sl_hotel_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian.ToString() + shang_time + "' and jieshushijian>='" + shijian.ToString() + xia_time + "' and fangjianbianhao like '%" + fangjianbianhao + "%' order by jiage asc,id desc");


                    if (sl_hotel_jiage.Rows.Count > 0)
                    {
                        if (int.Parse(sl_hotel_jiage.Rows[0]["kucun"].ToString()) == 0)
                        {
                            Response.Write("" + kaishishijian.AddDays(i).Month.ToString() + "月" + kaishishijian.AddDays(i).Day.ToString() + "日已满，请重新选择日期");
                            Response.End();
                        }
                        else
                        {
                            jiage = float.Parse(sl_hotel_jiage.Rows[0][renqun].ToString()) + jiage;
                            if (i == span.Days - 1)
                            {
                                Response.Write(jiage.ToString());
                                Response.End();
                            }
                        }
                    }
                    else
                    {
                        Response.Write("" + kaishishijian.AddDays(i).Month.ToString() + "月" + kaishishijian.AddDays(i).Day.ToString() + "日暂无报价，请重新选择");
                        Response.End();
                    }

                }




                //end

            }
            else if (type == "lvyouview")
            {
                //线路详细页  下拉框日期
                DateTime kaishishijian = DateTime.Now;
                DateTime jieshushijian = kaishishijian.AddDays(500);
                TimeSpan span1 = kaishishijian - dy;

                if (span1.Days <= 0)
                {
                    int tiqiantianshu = 1;
                    try
                    {
                        tiqiantianshu = int.Parse(my_c.GetTable("select tiqiantianshu from sl_lvyou where id=" + laiyuanbianhao + "").Rows[0]["tiqiantianshu"].ToString());
                    }
                    catch { }
                    kaishishijian = dy.AddDays(tiqiantianshu);
                }
                TimeSpan span = jieshushijian - kaishishijian;
                int jj = 1;
                for (int i = 0; i < span.Days; i++)
                {

                    string shijian = kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString();
                    DataTable sl_lvyou_jiage = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian.ToString() + "' and jieshushijian>='" + shijian.ToString() + "' order by id desc");
                    //Response.Write("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian.ToString() + "' and jieshushijian>='" + shijian.ToString() + "' order by id desc");
                    //Response.End();
                    if (sl_lvyou_jiage.Rows.Count > 0)
                    {
                        if (jj <= 5)
                        {
                            string shijian_ = kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString();
                            if (my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian_.ToString() + " 23:59:59" + "' and jieshushijian>='" + shijian_.ToString() + " 00:00:01" + "' order by id desc").Rows[0]["shifoudianxun"].ToString() == "是")
                            {
                                Response.Write("<option value=\"\">" + shijian + "(" + my_b.get_xingqi(DateTime.Parse(shijian)) + ") 电询</option>");
                            }
                            else if (int.Parse(sl_lvyou_jiage.Rows[0]["kucun"].ToString()) == 0)
                            {
                                Response.Write("<option value=\"\">" + shijian + "(" + my_b.get_xingqi(DateTime.Parse(shijian)) + ") 已满</option>");
                            }
                            else
                            {
                                Response.Write("<option value=\"" + shijian + "\">" + shijian + "(" + my_b.get_xingqi(DateTime.Parse(shijian)) + ")" + sl_lvyou_jiage.Rows[0]["chengren"].ToString() + "/人</option>");
                            }
                            jj++;
                        }
                        else
                        {
                            Response.End();
                        }
                    }
                    else
                    {
                        //DataTable sl_lvyou_jiage_ = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " order by id desc");
                        //if (sl_lvyou_jiage_.Rows.Count > 0)
                        //{

                        //    Response.Write("<option value=\"" + shijian + "\">" + shijian + "(" + my_b.get_xingqi(DateTime.Parse(shijian)) + ")" + sl_lvyou_jiage_.Rows[0]["chengren"].ToString() + "/人</option>");
                        //}

                        //Response.Write("<option value=\"\">" + shijian + "(" + my_b.get_xingqi(DateTime.Parse(shijian)) + ")" +"暂无价格</option>");


                    }
                }
            }
            else if (type == "lvyourili")
            {
                //旅游日历

                DateTime kaishishijian = DateTime.Parse(Request.QueryString["kaishishijian"].ToString());

                DateTime jieshushijian = DateTime.Parse(Request.QueryString["jieshushijian"].ToString());
                TimeSpan span1 = kaishishijian - dy;

                if (span1.Days <= 0)
                {
                    int tiqiantianshu = 1;
                    try
                    {
                        tiqiantianshu = int.Parse(my_c.GetTable("select tiqiantianshu from sl_lvyou where id=" + laiyuanbianhao + "").Rows[0]["tiqiantianshu"].ToString());
                    }
                    catch { }
                    kaishishijian = dy.AddDays(tiqiantianshu);
                }
                TimeSpan span = jieshushijian - kaishishijian;
                string lvyourili_str = "";

                for (int i = 0; i <= span.Days; i++)
                {
                    string shijian = kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString();

                    DataTable sl_lvyou_jiage = sl_lvyou_jiage = my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian.ToString() + "' and jieshushijian>='" + shijian.ToString() + "' order by id desc");


                    if (sl_lvyou_jiage.Rows.Count > 0)
                    {
                        string shijian_ = kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString();
                        //是否电询
                        if (my_c.GetTable("select top 1 * from sl_lvyou_jiage where laiyuanbianhao=" + laiyuanbianhao + " and kaishishijian<='" + shijian_.ToString() + " 23:59:59" + "' and jieshushijian>='" + shijian_.ToString() + " 00:00:01" + "' order by id desc").Rows[0]["shifoudianxun"].ToString() == "是")
                        {
                            if (lvyourili_str == "")
                            {
                                lvyourili_str = "{title: '电询',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD'),className:'tag tag_tel'}";
                            }
                            else
                            {
                                lvyourili_str = lvyourili_str + ",{title: '电询',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD'),className:'tag tag_tel'}";
                            }

                            //是否电询  end
                        }
                        else
                        {
                            //其它开始
                            if (int.Parse(sl_lvyou_jiage.Rows[0]["kucun"].ToString()) == 0)
                            {
                                if (lvyourili_str == "")
                                {
                                    lvyourili_str = "{title: '已满',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD'),className:'tag tag_full'}";
                                }
                                else
                                {
                                    lvyourili_str = lvyourili_str + ",{title: '已满',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD'),className:'tag tag_full'}";
                                }
                            }
                            else
                            {
                                //正常
                                if (lvyourili_str == "")
                                {
                                    lvyourili_str = "{title: '成人价：" + sl_lvyou_jiage.Rows[0]["chengren"].ToString() + "',start:'" + kaishishijian.AddDays(i).Year.ToString() + "-" + kaishishijian.AddDays(i).Month.ToString() + "-" + kaishishijian.AddDays(i).Day.ToString() + "'}";
                                }
                                else
                                {
                                    lvyourili_str = lvyourili_str + ",{title: '成人价：" + sl_lvyou_jiage.Rows[0]["chengren"].ToString() + "',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD')}";
                                }

                                if (lvyourili_str == "")
                                {
                                    lvyourili_str = "{title: '儿童价：" + sl_lvyou_jiage.Rows[0]["ertong"].ToString() + "',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD')}";
                                }
                                else
                                {
                                    lvyourili_str = lvyourili_str + ",{title: '儿童价：" + sl_lvyou_jiage.Rows[0]["ertong"].ToString() + "',start:moment('" + kaishishijian.AddDays(i).Year.ToString() + " " + kaishishijian.AddDays(i).Month.ToString() + " " + kaishishijian.AddDays(i).Day.ToString() + "','YYYY MM DD')}";
                                }
                            }
                        }
                    }
                    else
                    {

                    }

                    //DateTime.DaysInMonth(kaishishijian.Year, kaishishijian.Month)
                }
                Response.Write(lvyourili_str);
                Response.End();
                //end
            }

        }
    }
}