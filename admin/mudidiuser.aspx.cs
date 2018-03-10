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

public partial class Default2 : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    public string editname ="";
   public string listid = "";
   public string classid_list = "";
    public string set_dr(string id)
    {
        
        if (listid != "")
        {
            string[] aa = listid.Split(',');
            for (int i = 0; i < aa.Length; i++)
            {
                if (aa[i] == id)
                {
                    if (classid_list == "")
                    {
                        classid_list = aa[i];
                    }
                    else
                    {
                        classid_list =classid_list+","+ aa[i];
                    }
                    return " selected=\"selected\"";
                }
            }
        }
        return "";
    }
    //设置dr
    string mudidi_string = "";
    public void insert_dr(string classid)
    {
        int i = 0;
        DataTable dt = new DataTable();
        if (classid_list == "")
        {
            dt = my_c.GetTable("select * from sl_Parameter where classid in (" + classid + ") and id not in (197) order by u3 desc,id asc");
        }
        else
        {
            dt = my_c.GetTable("select * from sl_Parameter where classid in (" + classid_list + ") and id not in (197) order by u3 desc,id asc");
            classid_list = "";
        }
        if (dt.Rows.Count > 0)
        {
            j++;
          
            if (j == 1)
            {
                mudidi_string = mudidi_string+"<select multiple data-am-selected id=\"main\" class=\"end\">";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    mudidi_string = mudidi_string + " <option " + set_dr(dt.Rows[i]["id"].ToString()) + " value=\"" + dt.Rows[i]["id"].ToString() + "\">" + dt.Rows[i]["u1"].ToString() + "</option>";
                }
                mudidi_string = mudidi_string + "</select>";
            }
            else
            {
                mudidi_string = mudidi_string+"<select multiple data-am-selected id=\"rr" + j + "\" class=\"end\">";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    mudidi_string = mudidi_string + " <option " + set_dr(dt.Rows[i]["id"].ToString()) + " value=\"" + dt.Rows[i]["id"].ToString() + "\">" + dt.Rows[i]["u1"].ToString() + "</option>";
                }
                mudidi_string = mudidi_string + "</select>";
            }
            if (classid_list != "")
            {
                insert_dr(classid_list);
            }
        }

        Literal1.Text = mudidi_string;
    }
    //end
    public int j = 0;
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
            if (type == "view")
            {
                string classid = Request.QueryString["classid"].ToString();
                DataTable dt = my_c.GetTable("select * from sl_Parameter where classid in (" + classid + ") order by u3 desc,id asc");
                string view_string = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    view_string = view_string+"<option value=\"" + dt.Rows[i]["id"].ToString() + "\">" + dt.Rows[i]["u1"].ToString() + "</option>";
                }
                Response.Write(view_string + "{view}");
            }
            else
            {
                try
                {
                    listid = Request.QueryString["listid"].ToString();
                }
                catch { }
                editname = Request.QueryString["editname"].ToString();
                string classid = "196";
                try
                {
                    classid = Request.QueryString["classid"].ToString();
                }
                catch { }
                insert_dr(classid);

              
            }

        
        }
    }
}