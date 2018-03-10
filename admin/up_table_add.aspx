<%@ Page Language="C#" AutoEventWireup="true" CodeFile="up_table_add.aspx.cs" Inherits="admin_up_table_add" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>添加更新</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
        <span class="split ftSplit" ></span>
	<div id="guide" class="light_orange">您现在的位置：扩展功能 >> <a href="up_table.aspx"  style="color:#000">在线更新</a> >>添加更新</div>
	<div class="pageInfo right light_gray"></div>
    </div>
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>基本信息</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">文件路径：</td>
		<td colspan="2">
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:Label ID="Label1" runat="server"></asp:Label></td>
	</tr>
<tr>
		<td class="tRight">更新路径：</td>
		<td colspan="2"><asp:TextBox ID="TextBox1" runat="server" CssClass="txtwidth"></asp:TextBox>
            /bin/ &nbsp; /admin/</td>
	</tr>
	<tr>
		<td class="tRight">功能说明：</td>
		<td colspan="2">
		<asp:TextBox ID="TextBox2" runat="server" Width="450px" Height="120px" TextMode="MultiLine"></asp:TextBox>
            </td>
	</tr>
	
	  	      </table>
    <div id="cEndToolbar" class="toolbarBg">
      <div class="caButCase">
          <asp:Button ID="Button1" runat="server" CssClass="button" Text="确认操作" OnClick="Button1_Click1" />
		    
	        <input type="reset" name="Submit2" value="清空重填" class="button" />
      </div>
    </div>

    </form>
</body>
</html>


