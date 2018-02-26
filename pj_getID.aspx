<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="System.Xml" %>
 
<Script Language="c#" Runat="Server">
    protected void Page_Load(object sender, EventArgs e)
    {
        //StringBuilder sb = new StringBuilder();
        //sb.Append("test");
        //Response.Write(sb.ToString());
         
        //初始化一个xml对象
        XmlDocument xml = new XmlDocument();
        //加载xml文件
        xml.Load(Server.MapPath("/App_Data/Const.xml"));
        //读取指定的节点   xml.SelectSingleNode("/Config/SystemName");
        XmlNode xmlNode = xml.SelectSingleNode("/All/TicketAdmin/IDs");
        Response.Write(xmlNode.Attributes["Text"].Value);
        
        
    }
</script>

 