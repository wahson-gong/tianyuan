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
using System.Text;
public partial class user_daoru : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    my_paiwei my_p = new my_paiwei();
    public void chengxu(int page,int jishu,int dt_count,string type)
    {
        int kaishi = 0;
        if (page > 1)
        {
            kaishi = (page - 1) * jishu;
        }

        int shuliang = page * jishu;
        if (dt_count <= jishu)
        {
            shuliang = dt_count;
        }
        for (int i = kaishi; i < shuliang; i++)
        {
            #region 批量导入会员
            string yonghuming = "ym" + my_b.get_bianhao();
            string mima = my_b.md5("123456");
            string xingming = "测试员" + i.ToString();
            string jieshaoren = "";
            if (i > 0)
            {
                jieshaoren = my_c.GetTable("select top 1 yonghuming from sl_user order by id desc").Rows[0]["yonghuming"].ToString();
            }
            string leixing = "普通会员";
            string dailiren = my_p.get_dailiren();
            //Response.Write(dailiren);
            //Response.End();
            //if (dailiren == "")
            //{
            //    dailiren = yonghuming;
            //}
            string anquanmima = my_b.md5("123456");
            string jihuo = "是";
            string dingdanhao = my_b.get_bianhao();

            my_c.genxin("insert into sl_user(yonghuming,mima,xingming,jieshaoren,leixing,dailiren,anquanmima,jihuo) values('" + yonghuming + "','" + mima + "','" + xingming + "','" + jieshaoren + "','" + leixing + "','" + dailiren + "','" + anquanmima + "','" + jihuo + "')");
          

            #region 开始计算返利
            my_p.jianglizhidu("投资金额", yonghuming, dingdanhao);//投资金额
            if (jieshaoren != "")
            {
                my_p.jianglizhidu("直推奖励", jieshaoren, dingdanhao);//直推奖励
                my_p.jianglizhidu("广告费", jieshaoren, dingdanhao);//广告费
            }
            #endregion
            if (dailiren != "")
            {
                my_p.jianglizhidu("邮费补贴", dailiren, "");//邮费补贴

                my_p.set_paiwei(dailiren); //排位处理
            }
            //end
            #endregion
        }
        int xinpage = page + 1;
        if (type == "all")
        {
            Response.End();
            Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("user_table.aspx?Model_id=23", "&", "fzw123") + "");
        }
        else
        {
            Response.Redirect("err.aspx?err=合计：" + dt_count + "条，已生成：" + shuliang + "条！&errurl=" + my_b.tihuan("user_daoru.aspx?page=" + xinpage + "&dt_count=" + dt_count, "&", "fzw123") + "");
        }
       
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int page = 1;
            int dt_count = 0;
            int jishu = 100;
            try
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }
            catch
            { }
            try
            {
                dt_count = int.Parse(Request.QueryString["dt_count"].ToString());
            }
            catch
            { }
            if (dt_count > 0)
            {
                if (dt_count<=jishu)
                {
                    #region 未生成
                    chengxu(page, jishu, dt_count,"all");
                    #endregion
                }
                else if (dt_count - (page * jishu) >= 0)
                {
                    #region 未生成
                    chengxu(page, jishu, dt_count,"");
                    #endregion
                }
                else
                {
                    #region 已生成完
                    Response.Redirect("err.aspx?err=增加信息成功！马上跳转到信息列表页面！&errurl=" + my_b.tihuan("user_table.aspx?Model_id=23", "&", "fzw123") + "");
                    #endregion
                }
            }


        }
    }



    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        int shuliang = int.Parse(this.TextBox1.Text);
        //if (shuliang > 0)
        //{
        //    for (int i = 0; i < shuliang; i++)
        //    {
        //        #region 批量导入会员
        //        string yonghuming = "ym" + my_b.get_bianhao();
        //        string mima = my_b.md5("123456");
        //        string xingming = "测试员" + i.ToString();
        //        string jieshaoren = "";
        //        if (i > 0)
        //        {
        //            jieshaoren = my_c.GetTable("select top 1 yonghuming from sl_user order by id desc").Rows[0]["yonghuming"].ToString();
        //        }
        //        string leixing = "普通会员";
        //        string dailiren = my_p.get_dailiren();
        //        //if (dailiren == "")
        //        //{
        //        //    dailiren = yonghuming;
        //        //}
        //        string anquanmima = my_b.md5("123456");
        //        string jihuo = "是";
        //        string dingdanhao = my_b.get_bianhao();

        //        my_c.genxin("insert into sl_user(yonghuming,mima,xingming,jieshaoren,leixing,dailiren,anquanmima,jihuo) values('" + yonghuming + "','" + mima + "','" + xingming + "','" + jieshaoren + "','" + leixing + "','" + dailiren + "','" + anquanmima + "','" + jihuo + "')");

        //        #region 开始计算返利
        //        my_p.jianglizhidu("投资金额", yonghuming, dingdanhao);//投资金额
        //        if (jieshaoren != "")
        //        {
        //            my_p.jianglizhidu("直推奖励", jieshaoren, dingdanhao);//直推奖励
        //            my_p.jianglizhidu("广告费", jieshaoren, dingdanhao);//广告费
        //        }
        //        #endregion
        //        if (dailiren != "")
        //        {
        //            my_p.jianglizhidu("邮费补贴", dailiren, "");//邮费补贴

        //            my_p.set_paiwei(dailiren); //排位处理
        //        }
        //        //end
        //            #endregion
        //    }
        //}
        Response.Redirect("err.aspx?err=开始生成！&errurl=" + my_b.tihuan("user_daoru.aspx?dt_count="+ shuliang, "&", "fzw123") + "");
        //end
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

}
