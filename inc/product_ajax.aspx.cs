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
public partial class admin_page_ajax : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable peizhi_all_dt = new DataTable();
    DataTable peizhi_id_dt = new DataTable();
    DataTable product_dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string peizhiid = HttpUtility.UrlDecode(Request.QueryString["peizhiid"].ToString());
            string id = HttpUtility.UrlDecode(Request.QueryString["id"].ToString());
            string value = HttpUtility.UrlDecode(Request.QueryString["value"].ToString());
            string product_id = HttpUtility.UrlDecode(Request.QueryString["product_id"].ToString());
            string beizhu = HttpUtility.UrlDecode(Request.QueryString["beizhu"].ToString());

           
            if (id != "")
            {
                peizhi_id_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "peizhi where id=" + id + " and leixing='规格'");

                string peizhi_id_str = "";
                string peizhi_id_title = "";
                if (peizhi_id_dt.Rows.Count > 0)
                {
                    peizhi_id_str = peizhi_id_dt.Rows[0]["u1"].ToString() + "：" + value;
                    peizhi_id_title = peizhi_id_dt.Rows[0]["u1"].ToString() + "：";

                    if (beizhu.IndexOf(peizhi_id_dt.Rows[0]["u1"].ToString()) == -1)
                    {
                        if (beizhu == "")
                        {
                            beizhu = peizhi_id_str;

                        }
                        else
                        {
                            if (my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "product where id=" + product_id + " and  beizhu like '%" + beizhu + "{zu}" + peizhi_id_str + "%'").Rows.Count > 0)
                            {
                                beizhu = beizhu + "{zu}" + peizhi_id_str;
                            }
                            else
                            {
                                beizhu = peizhi_id_str + "{zu}" + beizhu;
                            }

                        }
                    }
                    else
                    {
                        //Response.Write(peizhi_id_dt.Rows[0]["u1"].ToString());
                        //Response.End();
                        //  beizhu = Regex.Replace(beizhu, peizhi_id_title + ".*?{zu}|{zu}.*?" + peizhi_id_title, peizhi_id_str, RegexOptions.Singleline);
                        if (Regex.Match(beizhu, peizhi_id_title + ".*?{zu}", RegexOptions.Singleline).ToString() != "")
                        {
                            beizhu = Regex.Replace(beizhu, peizhi_id_title + ".*?{zu}", peizhi_id_str + "{zu}", RegexOptions.Singleline);
                        }
                        else if (Regex.Match(beizhu, "{zu}" + peizhi_id_title + ".*", RegexOptions.Singleline).ToString() != "")
                        {
                            beizhu = Regex.Replace(beizhu, "{zu}" + peizhi_id_title + ".*", "{zu}" + peizhi_id_str, RegexOptions.Singleline);

                        }
                        else
                        {

                            beizhu = Regex.Replace(beizhu, peizhi_id_title + ".*", peizhi_id_str, RegexOptions.Singleline);

                        }

                    }
                }
            }

            //如果没有ID
            if (peizhiid != "0")
            {
                peizhi_all_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "peizhi where classid=" + peizhiid + " and leixing='规格' order by id asc");
                float jiage = 0;
                float kucun = 0;
                float dijiage = 1;
                string[] aa = Regex.Split(beizhu, "{zu}");
                product_dt = my_c.GetTable("select * from " + ConfigurationSettings.AppSettings["Prefix"].ToString() + "product where id=" + product_id + "");
                if (peizhi_all_dt.Rows.Count == aa.Length)
                {

                    if (product_dt.Rows.Count > 0)
                    {
                        string[] pz_neirong = Regex.Split(product_dt.Rows[0]["beizhu"].ToString(), "{next}");

                        for (int i = 0; i < pz_neirong.Length; i++)
                        {
                            if (pz_neirong[i].IndexOf(beizhu) > -1)
                            {
                                //HttpContext.Current.Response.Write(pz_neirong[i]);
                                //Response.End();
                                string[] pz_list = Regex.Split(pz_neirong[i], "{zu}");
                                //计算出具体价格
                                if (pz_list[0].ToString() != "")
                                {
                                    jiage = float.Parse(pz_list[0].ToString());
                                    dijiage = 0;
                                }
                                if (pz_list[1].ToString() != "")
                                {
                                    kucun = float.Parse(pz_list[1].ToString());
                                }
                            }

                        }
                    }
                }
                else
                {


                    if (product_dt.Rows.Count > 0)
                    {
                        string[] pz_neirong = Regex.Split(product_dt.Rows[0]["beizhu"].ToString(), "{next}");

                        for (int i = 0; i < pz_neirong.Length; i++)
                        {
                            string[] pz_list = Regex.Split(pz_neirong[i], "{zu}");
                            if (pz_list[0].ToString() != "")
                            {
                                if (jiage < float.Parse(pz_list[0].ToString()))
                                {
                                    jiage = float.Parse(pz_list[0].ToString());
                                }
                                else
                                {
                                    dijiage = float.Parse(pz_list[0].ToString());
                                }
                            }
                            if (pz_list[1].ToString() != "")
                            {
                                if (kucun < float.Parse(pz_list[1].ToString()))
                                {
                                    kucun = float.Parse(pz_list[1].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        dijiage = 0;
                    }

                    //算出价格与库存
                }
                if (jiage == 0)
                {
                    jiage = float.Parse(product_dt.Rows[0]["jiage"].ToString());
                    dijiage = 0;
                }
                Response.Write(beizhu);
                Response.Write("|" + my_b.get_jiage(dijiage));
                Response.Write("|" + my_b.get_jiage(jiage));

                Response.Write("|" + kucun);
            }


            //end
        }
    }
}
