<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeFile="user_table_pwd.aspx.cs" Inherits="admin_auto_table_add" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>添加信息</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
 <link rel="stylesheet" href="editor/themes/default/default.css" />
	<link rel="stylesheet" href="editor/plugins/code/prettify.css" />
	<script charset="utf-8" src="editor/kindeditor.js"></script>
	<script charset="utf-8" src="editor/lang/zh_CN.js"></script>
	<script charset="utf-8" src="editor/plugins/code/prettify.js"></script>

     <script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
    </head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 

        <span class="split ftSplit" ></span>
	<div id="guide" class="light_orange">您现在的位置：内容管理 >> <a href="user_table.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>"  style="color:#000">信息管理</a><asp:Literal ID="Literal4" runat="server"></asp:Literal> >> 添加</div>
	<div class="pageInfo right light_gray"></div>
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
     
         
          <asp:Button ID="Button1" runat="server" CssClass="button" Text="确认操作" OnClick="Button1_Click1" OnClientClick="return tijia()" />
		    
	        <input type="reset" name="Submit2" value="清空重填" class="button">
          <asp:Label ID="Label1" runat="server" Text="" style="display:none"></asp:Label></div>
    </div>
<script type="text/javascript">
    function tijia() {
        if (document.getElementById("yonghuming").value == "") {
            document.getElementById("yonghuming").value = document.getElementById("huiyuanka").value;
        }
    }
	function set_lable(g1)
	{
		if(document.getElementById("Label1").innerHTML.indexOf(g1)>-1)
		{
			document.getElementById("Label1").innerHTML="";	
		}
		else
		{
			document.getElementById("Label1").innerHTML=g1;
		}
	}
	function pic_cut(id,cc)
	{
	    window.open("pic_cut.aspx?editname=" + id + "&pic_name=" + document.getElementById(id).value+"&cc="+cc+"","","status=no,scrollbars=no,top=20,left=110,width=740,height=340");
	}
</script>
    </form>
</body>
</html>
