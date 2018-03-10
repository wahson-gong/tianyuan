<%@ Page Language="C#" AutoEventWireup="true" CodeFile="file_manage_view.aspx.cs" Inherits="admin_file_manage_view"  ValidateRequest="false"%>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>文件修改</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
        <span class="split ftSplit" ></span>
	<div id="guide" class="light_orange">您现在的位置：扩展功能 >> <a href="file_manage_main.aspx">WEBFT</a> >> 文件修改</div>
	<div class="pageInfo right light_gray"></div>
    </div>
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>基本信息</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">工作目录：</td>
		<td colspan="2">
        <asp:TextBox ID="TextBox1" runat="server" CssClass="txtwidth" Enabled="False"></asp:TextBox>&nbsp; （空白表示根目录 ，不允许用 “..” 形式的路径）</td>
	</tr>
<tr>
		<td class="tRight">文件名称：</td>
		<td colspan="2">
		<asp:TextBox ID="TextBox2" runat="server" CssClass="txtwidth" Enabled="False"></asp:TextBox>&nbsp; （不允许用 “..” 形式的路径）
         </td>
	</tr>
	<tr>
	<td colspan="3">
	<asp:TextBox ID="TextBox3" runat="server" Height="400px" TextMode="MultiLine" Width="95%"></asp:TextBox>
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


