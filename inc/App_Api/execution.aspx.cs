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
using Newtonsoft.Json;
using LitJson;
using System.Runtime.Serialization;

public partial class orderauto : System.Web.UI.Page
{
    my_basic my_b = new my_basic();
    my_conn my_c = new my_conn();
    Common_ghy my_com = new Common_ghy();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            my_com.OutJson("select * from sl_admin", "sl_admin");



        }

    }
}
