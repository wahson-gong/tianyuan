<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Office.aspx.cs" Inherits="inc_office_Office" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script language="javascript" type="text/javascript">
        <!--

        function show_word() {
            document.all.MyOffice.Open("<%=my_b.get_Domain()%><%=Request.QueryString["url"].ToString()%>", false, "Word.Document");
        }

        // -->
        </script>
</head>
  <body onload="show_word();">
    <form id="form1" runat="server">
    <div>
      <OBJECT id="MyOffice" codeBase="dsoframer.ocx#version=2,3,0,0'" height="100%" width="99%" classid="clsid:00460182-9E5E-11D5-B7C8-B8269041DD57">
                            <PARAM NAME="_ExtentX" VALUE="16960" />
                            <PARAM NAME="_ExtentY" VALUE="13600" />
                            <PARAM NAME="BorderColor" VALUE="-2147483632" />
                            <PARAM NAME="BackColor" VALUE="-2147483643" />
                            <PARAM NAME="ForeColor" VALUE="-2147483640" />
                            <PARAM NAME="TitlebarColor" VALUE="-2147483635" />
                            <PARAM NAME="TitlebarTextColor" VALUE="-2147483634" />
                            <PARAM NAME="BorderStyle" VALUE="1" />
                            <PARAM NAME="Titlebar" VALUE="0" />
                            <PARAM NAME="Toolbars" VALUE="1" />
                            <PARAM NAME="Menubar" VALUE="1" />
                        </OBJECT>

    </div>
    </form>
</body>
</html>
