<%@ Page Language="C#" AutoEventWireup="true" CodeFile="show_user_table.aspx.cs" Inherits="admin_show_auto_table" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>查看信息</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />

	<div class="pageInfo right light_gray" style></div>
    </div>
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td colspan="3"><span style="font-weight:700" >基本信息</span></td>
	</tr>
	</table>
    <asp:Table ID="Table1" runat="server" CssClass="cTable_2 table">
        </asp:Table>
    <div id="cEndToolbar" class="toolbarBg">
      <div class="caButCase">
      
		    
	        
      </div>
    </div>

    </form>
</body>
</html>


