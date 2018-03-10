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
using System.Xml;
public partial class admin_Config : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    DataTable dt = new DataTable();
    public void set_xml(string g1, string g2)
    {
        XmlDocument webconfigDoc = new XmlDocument();
        string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\web.config";
        //设置节的xml路径                        
        string xPath = "/configuration/appSettings/add[@key='?']";

        //加载web.config文件
        webconfigDoc.Load(filePath);

        //找到要修改的节点
        XmlNode passkey = webconfigDoc.SelectSingleNode(xPath.Replace("?", g1));

        //设置节点的值
        passkey.Attributes["value"].InnerText = g2;

        //保存设置
        webconfigDoc.Save(filePath);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int i = 0;

            my_b.get_font(DropDownList1);
            DataTable xml_dt = my_c.read_xml("/upfile/data/web_config.xml", "web_config");
            this.TextBox1.Text = xml_dt.Rows[0]["u1"].ToString();
            this.TextBox2.Text = xml_dt.Rows[0]["u2"].ToString();
            this.TextBox3.Text = xml_dt.Rows[0]["u3"].ToString();
            this.TextBox4.Text = xml_dt.Rows[0]["u4"].ToString();
            this.TextBox5.Text = xml_dt.Rows[0]["u5"].ToString();
            this.TextBox6.Text = xml_dt.Rows[0]["u6"].ToString();
            this.TextBox7.Text = xml_dt.Rows[0]["u7"].ToString();
            this.TextBox8.Text = xml_dt.Rows[0]["u8"].ToString();
            this.TextBox9.Text = xml_dt.Rows[0]["u9"].ToString();
            this.TextBox10.Text = xml_dt.Rows[0]["u10"].ToString();
            this.TextBox11.Text = xml_dt.Rows[0]["u11"].ToString();
            this.TextBox12.Text = xml_dt.Rows[0]["u12"].ToString();
            this.TextBox13.Text = xml_dt.Rows[0]["u13"].ToString();
            this.TextBox14.Text = xml_dt.Rows[0]["u14"].ToString();
            this.TextBox15.Text = xml_dt.Rows[0]["u15"].ToString();
            this.TextBox16.Text = xml_dt.Rows[0]["u16"].ToString();
            this.TextBox17.Text = xml_dt.Rows[0]["u17"].ToString();
            this.TextBox23.Text = xml_dt.Rows[0]["u23"].ToString();
            this.TextBox24.Text = xml_dt.Rows[0]["u24"].ToString();
            if (xml_dt.Rows[0]["u18"].ToString() != "")
            {
                Label1.Text = "密码已设置，不修改请为空";
            }
            this.TextBox19.Text = xml_dt.Rows[0]["u19"].ToString();
            this.TextBox20.Text = xml_dt.Rows[0]["u20"].ToString();
            this.TextBox21.Text = xml_dt.Rows[0]["u21"].ToString();
            this.TextBox22.Text = xml_dt.Rows[0]["u22"].ToString();

            this.TextBox25.Text = xml_dt.Rows[0]["u27"].ToString();
            this.TextBox26.Text = xml_dt.Rows[0]["u28"].ToString();
            this.TextBox27.Text = xml_dt.Rows[0]["u32"].ToString();
            this.TextBox28.Text = xml_dt.Rows[0]["u33"].ToString();
            this.TextBox29.Text = xml_dt.Rows[0]["u36"].ToString();
            this.TextBox32.Text = xml_dt.Rows[0]["u38"].ToString();

            for (i = 0; i < RadioButtonList2.Items.Count; i++)
            {
                if (RadioButtonList2.Items[i].Value == xml_dt.Rows[0]["u26"].ToString())
                {
                    RadioButtonList2.Items[i].Selected = true;
                }
            }

            for (i = 0; i < RadioButtonList3.Items.Count; i++)
            {
                if (RadioButtonList3.Items[i].Value == xml_dt.Rows[0]["u34"].ToString())
                {
                    RadioButtonList3.Items[i].Selected = true;
                }
            }


            for (i = 0; i < DropDownList1.Items.Count; i++)
            {
                if (DropDownList1.Items[i].Value == xml_dt.Rows[0]["u29"].ToString())
                {
                    DropDownList1.Items[i].Selected = true;
                }
            }
            TextBox33.Text = xml_dt.Rows[0]["u30"].ToString();
           
            for (i = 0; i < RadioButtonList4.Items.Count; i++)
            {
                if (RadioButtonList4.Items[i].Value == xml_dt.Rows[0]["u35"].ToString())
                {
                    RadioButtonList4.Items[i].Selected = true;
                }
            }
            if (xml_dt.Rows[0]["u31"].ToString() == "1")
            {
                CheckBox1.Checked = true;
            }

            for (i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (xml_dt.Rows[0]["u37"].ToString().IndexOf(CheckBoxList1.Items[i].Text) > -1)
                {
                    CheckBoxList1.Items[i].Selected = true;
                }
            }

            //运费
            this.TextBox30.Text = ConfigurationSettings.AppSettings["yunfei"].ToString();
            this.TextBox31.Text = ConfigurationSettings.AppSettings["mianyunfei"].ToString();
            //end
        }
    }



    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.ToString());
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //运费
        set_xml("yunfei", my_b.c_string(this.TextBox30.Text));
        set_xml("mianyunfei", my_b.c_string(this.TextBox31.Text));
        //end
        string u1 = my_b.c_string(this.TextBox1.Text);
        string u2 = my_b.c_string(this.TextBox2.Text);
        string u3 = my_b.c_string(this.TextBox3.Text);
        string u4 = my_b.c_string(this.TextBox4.Text);
        string u5 = my_b.c_string(this.TextBox5.Text);
        string u6 = my_b.c_string(this.TextBox6.Text);
        string u7 = my_b.c_string(this.TextBox7.Text);
        string u8 = my_b.c_string(this.TextBox8.Text);
        string u9 = my_b.c_string(this.TextBox9.Text);
        string u10 = my_b.c_string(this.TextBox10.Text);
        string u11 = my_b.c_string(this.TextBox11.Text);
        string u12 = my_b.c_string(this.TextBox12.Text);
        string u13 = my_b.c_string(this.TextBox13.Text);
        string u14 = my_b.c_string(this.TextBox14.Text);
        string u15 = my_b.c_string(this.TextBox15.Text);
        string u16 = my_b.c_string(this.TextBox16.Text);
        string u17 = my_b.c_string(this.TextBox17.Text);
        string u18 = my_b.c_string(this.TextBox18.Text);
        string u19 = my_b.c_string(this.TextBox19.Text);
        string u20 = my_b.c_string(this.TextBox20.Text);
        string u21 = my_b.c_string(this.TextBox21.Text);
        string u22 = my_b.c_string(this.TextBox22.Text);
        string u23 = my_b.c_string(this.TextBox23.Text);
        string u24 = my_b.c_string(this.TextBox24.Text);

        string u26 = "";
        try
        {
            u26 = RadioButtonList2.SelectedItem.Value;
        }
        catch { }
        string u27 = my_b.c_string(this.TextBox25.Text);
        string u28 = my_b.c_string(this.TextBox26.Text);
        string u29 = "";
        try
        {
            u29 = DropDownList1.SelectedItem.Value;
        }
        catch { }
        string u30 = TextBox33.Text;
        string u31 = "0";
        if (CheckBox1.Checked)
        { u31 = "1"; }
        string u32 = my_b.c_string(this.TextBox27.Text);
        string u33 = my_b.c_string(this.TextBox28.Text);
        string u36 = my_b.c_string(this.TextBox29.Text);
        string u34 = "";
        try
        {
            u34 = RadioButtonList3.SelectedItem.Value;
        }
        catch { }
        string u37 = "";
        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
        {
            if (CheckBoxList1.Items[i].Selected)
            {
                if (u37 == "")
                {
                    u37 = CheckBoxList1.Items[i].Text;
                }
                else
                {
                    u37 = u37 + "|" + CheckBoxList1.Items[i].Text;
                }
            }
        }
        string u38 = my_b.c_string(this.TextBox32.Text);

        string file_path = my_b.get_ApplicationPath() + "/upfile/data/web_config.xml";
        string jidian = "web_config";
        string u35 = "";
        try
        {
            u35 = RadioButtonList4.SelectedItem.Value;
        }
        catch { }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(HttpContext.Current.Server.MapPath(file_path));

        XmlNodeList nodeList = xmlDoc.SelectSingleNode(jidian).ChildNodes;//获取bookstore节点的所有子节点
        foreach (XmlNode xn in nodeList)//遍历所有子节点
        {
            XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
            if (xe.Name == "u1")//如果找到
            {
                xe.InnerText = u1;//则修改

            }
            if (xe.Name == "u2")//如果找到
            {
                xe.InnerText = u2;//则修改

            }
            if (xe.Name == "u3")//如果找到
            {
                xe.InnerText = u3;//则修改

            }
            if (xe.Name == "u4")//如果找到
            {
                xe.InnerText = u4;//则修改

            }
            if (xe.Name == "u5")//如果找到
            {
                xe.InnerText = u5;//则修改

            }
            if (xe.Name == "u6")//如果找到
            {
                xe.InnerText = u6;//则修改

            }
            if (xe.Name == "u7")//如果找到
            {
                xe.InnerText = u7;//则修改

            }
            if (xe.Name == "u8")//如果找到
            {
                xe.InnerText = u8;//则修改

            }
            if (xe.Name == "u9")//如果找到
            {
                xe.InnerText = u9;//则修改

            }
            if (xe.Name == "u10")//如果找到
            {
                xe.InnerText = u10;//则修改

            }
            if (xe.Name == "u11")//如果找到
            {
                xe.InnerText = u11;//则修改

            }
            if (xe.Name == "u12")//如果找到
            {
                xe.InnerText = u12;//则修改

            }
            if (xe.Name == "u13")//如果找到
            {
                xe.InnerText = u13;//则修改

            }
            if (xe.Name == "u14")//如果找到
            {
                xe.InnerText = u14;//则修改
            }
            if (xe.Name == "u15")//如果找到
            {
                xe.InnerText = u15;//则修改

            }
            if (xe.Name == "u16")//如果找到
            {
                xe.InnerText = u16;//则修改

            }
            if (xe.Name == "u17")//如果找到
            {
                xe.InnerText = u17;//则修改

            }
            if (u18 != "")
            {
                if (xe.Name == "u18")//如果找到
                {
                    xe.InnerText = u18;//则修改

                }
            }

            if (xe.Name == "u19")//如果找到
            {
                xe.InnerText = u19;//则修改

            }
            if (xe.Name == "u20")//如果找到
            {
                xe.InnerText = u20;//则修改

            }
            if (xe.Name == "u21")//如果找到
            {
                xe.InnerText = u21;//则修改

            }
            if (xe.Name == "u22")//如果找到
            {
                xe.InnerText = u22;//则修改

            }
            if (xe.Name == "u23")//如果找到
            {
                xe.InnerText = u23;//则修改

            }
            if (xe.Name == "u24")//如果找到
            {
                xe.InnerText = u24;//则修改

            }
            if (xe.Name == "u26")//如果找到
            {
                xe.InnerText = u26;//则修改

            }
            if (xe.Name == "u27")//如果找到
            {
                xe.InnerText = u27;//则修改

            }
            if (xe.Name == "u28")//如果找到
            {
                xe.InnerText = u28;//则修改

            }
            if (xe.Name == "u29")//如果找到
            {
                xe.InnerText = u29;//则修改

            }
            if (xe.Name == "u30")//如果找到
            {
                xe.InnerText = u30;//则修改

            }
            if (xe.Name == "u31")//如果找到
            {
                xe.InnerText = u31;//则修改

            }
            if (xe.Name == "u32")//如果找到
            {
                xe.InnerText = u32;//则修改

            }
            if (xe.Name == "u33")//如果找到
            {
                xe.InnerText = u33;//则修改

            }
            if (xe.Name == "u34")//如果找到
            {
                xe.InnerText = u34;//则修改

            }
            if (xe.Name == "u35")//如果找到
            {
                xe.InnerText = u35;//则修改

            }
            if (xe.Name == "u36")//如果找到
            {
                xe.InnerText = u36;//则修改

            }
            if (xe.Name == "u37")//如果找到
            {
                xe.InnerText = u37;//则修改

            }
            if (xe.Name == "u38")//如果找到
            {
                xe.InnerText = u38;//则修改

            }
        }
        xmlDoc.Save(HttpContext.Current.Server.MapPath(file_path));//保存

        Response.Redirect("err.aspx?err=操作成功！马上跳转到全站参数页面！&errurl=" + my_b.tihuan("Config.aspx", "&", "fzw123") + "");
    }
}
