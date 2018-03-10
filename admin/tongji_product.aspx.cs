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

public partial class tongji_product : System.Web.UI.Page
{
    public my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string key = "";
    string shang_time = " 00:00:01";
    string xia_time = " 23:59:59";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable sl_product= my_c.GetTable("select id,biaoti from sl_product  order by id desc");
            DropDownList1.DataSource = sl_product;
            DropDownList1.DataTextField = "biaoti";
            DropDownList1.DataValueField = "id";
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, "全部");
            DropDownList1.Items[0].Value = "";

            #region 搜索参数
            string search_sql = "";
            string page = "1";
            string shijian1 = "";
            try
            {
                shijian1 = Request.QueryString["shijian1"].ToString();
                this.TextBox1.Text = shijian1;
            }
            catch
            { }
            string shijian2 = "";
            try
            {
                shijian2 = Request.QueryString["shijian2"].ToString();
                this.TextBox2.Text = shijian2;
            }
            catch
            { }
            try
            {
                page = Request.QueryString["page"].ToString();
            }
            catch
            { }
            try
            {
                search_sql = Request.QueryString["search_sql"].ToString();
            }
            catch
            { }
            try
            {
                key = Request.QueryString["key"].ToString();
                this.TextBox3.Text = key;
            }
            catch
            { }
            string laiyuanbianhao = "";
            try
            {
                laiyuanbianhao = Request.QueryString["laiyuanbianhao"].ToString();
            }
            catch
            { }
            if (laiyuanbianhao != "")
            {
                search_sql = "laiyuanbianhao=" + laiyuanbianhao + "";
                for (int i = 0; i < DropDownList1.Items.Count; i++)
                {
                    if (DropDownList1.Items[i].Value == laiyuanbianhao)
                    {
                        DropDownList1.Items[i].Selected = true;
                    }
                }
            }
            if (shijian1 != "" && shijian2 != "")
            {
                if (search_sql == "")
                {
                    search_sql = "dtime between '" + shijian1.ToString() + shang_time + "' and '" + shijian2.ToString() + xia_time + "'";
                }
                else
                {
                    search_sql = search_sql + " and dtime between '" + shijian1.ToString() + shang_time + "' and '" + shijian2.ToString() + xia_time + "'";
                }

            }
            if (key != "")
            {
                //if (search_sql == "")
                //{
                //    search_sql = "(u1 like '%" + key + "%' or u2 like '%" + key + "%' or u3 like '%" + key + "%')";
                //}
                //else
                //{
                //    search_sql = search_sql + " and (u1 like '%" + key + "%' or u2 like '%" + key + "%' or u3 like '%" + key + "%')";
                //}
                this.TextBox1.Text = key;
            }
            if (search_sql == "")
            {
                search_sql = "id>0";
            }

            DataTable dt = my_c.GetTable("select * from sl_xiaoshou where " + search_sql + " order by id desc");

            DataTable newdt = new DataTable();//可以给表创建一个名字，tb  
            newdt.Columns.Add("xiaoji", typeof(System.String)); //加字段
            newdt.Columns.Add("jine", typeof(System.String)); //加字段
            newdt.Columns.Add("shuliang", typeof(System.String)); //加字段
            newdt.Columns.Add("laiyuanbianhao", typeof(System.String)); //加字段
            for (int i = 0; i < sl_product.Rows.Count; i++)
            {
                DataRow[] rows = dt.Select("laiyuanbianhao=" + sl_product.Rows[i]["id"].ToString());
                if (rows.Length >0)
                {
                    float xiaoji = 0;
                    float jine = 0;
                    float shuliang = 0;
                     laiyuanbianhao = "";
                    foreach (DataRow row in rows)
                    {

                        xiaoji = xiaoji + float.Parse(row["jine"].ToString());
                        jine = float.Parse(row["jine"].ToString());
                        shuliang = shuliang + float.Parse(row["shuliang"].ToString());
                        laiyuanbianhao = row["laiyuanbianhao"].ToString();
                    }
                    DataRow row1 = newdt.NewRow();//生成新行
                    row1["xiaoji"] = xiaoji;//加数据
                    row1["jine"] = jine;
                    row1["shuliang"] = shuliang;
                    row1["laiyuanbianhao"] = laiyuanbianhao;
                    newdt.Rows.Add(row1);//这样就可以添加了  
                }
               
            }
          
           

            Repeater1.DataSource = newdt;
            Repeater1.DataBind();
            #endregion


        }
    }


    protected void Button1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("tongji_product.aspx?key=" + this.TextBox3.Text + "&laiyuanbianhao=" + DropDownList1.SelectedValue + "&shijian1=" + this.TextBox1.Text + "&shijian2=" + this.TextBox2.Text + "");
    }
}
