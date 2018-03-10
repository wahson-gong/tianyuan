<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeFile="user_table_all_add.aspx.cs" Inherits="admin_auto_table_add" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>
<%@ Register Src="ascx/FreeTextBox.ascx" TagName="FreeTextBox" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>批量增加会员</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
   
<script type="text/javascript">
    gg_load_err = 0;
    try {
        google.load("language", "1");
    }
    catch (e) {
        gg_load_err = 1;
    }

    function gg_translate(g1, g2) {
        try {
            google.language.translate(document.getElementById(g1).value, "zh", "en", function (result) {
                if (!result.error) {
                    var file_path = document.getElementById(g2).value + "/" + result.translation + "/";
                    document.getElementById(g2).value = file_path.replace("//", "/");
                }
            });
        }
        catch (e) {
            alert("failed to translate");
        }
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg">
    <div style="float:left"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a>&nbsp;
        <span class="split ftSplit" ></span></div>
	<div id="guide" class="light_orange">您现在的位置：<a href="default.aspx">用户管理</a> >> <a href="/admin/auto_table.aspx?Model_id=10">会员管理</a> >> 批量增加会员</div>
	<div class="pageInfo right light_gray"></div>
    </div>
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>批量增加会员</div></td>
		<td colspan="2"></td>
	</tr>
	<tr>
		<td class="tRight">会员号区间：</td>
		<td colspan="2">
        <asp:TextBox ID="TextBox5" runat="server"   CssClass="pwdwidth" Text="1"></asp:TextBox>到<asp:TextBox ID="TextBox6" runat="server"   CssClass="pwdwidth" Text="100"></asp:TextBox>
            </td>
	</tr>
	<tr>
	  <td class="tRight">数量：</td>
	  <td colspan="2"><asp:TextBox ID="TextBox1" runat="server"   CssClass="pwdwidth" Text="1"></asp:TextBox></td>
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


