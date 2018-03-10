<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dao_xls.aspx.cs" Inherits="admin_K_Table_add"  ValidateRequest="false"%>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>导入XLS</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .zuo1{ width:50%; text-align:right}
    .you1{ width:50%}
    .yincang { display:none}
    </style>
             <script src="js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
		<script src="js/jquery.SuperSlide.2.1.1.js" type="text/javascript" charset="utf-8"></script>
		<script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
        		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
        <span class="split ftSplit" ></span>
	<div id="guide" class="light_orange">您现在的位置：扩展功能 >>  导入XLS</div>
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
            <asp:TextBox ID="TextBox1" runat="server" Text="/upfile/Upload/2017324/2017324141457646.xls"></asp:TextBox>&nbsp;<input id="Button4" class="button" type="button" value="上传xls"  onClick="window.open('/inc/webuploader/FileUploader.aspx?type=soft&editname=TextBox1','','status=no,scrollbars=no,top=20,left=110,width=450,height=400');"/>
            <asp:Button ID="Button2" runat="server" CssClass="button" Text="获取字段" 
                onclick="Button2_Click"/>
            </td>
	</tr>
	  	      </table>
    <asp:Panel ID="Panel1" runat="server">
     <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="50%" style=" text-align:right" OnClick="aa();">【<asp:Literal ID="Literal2" runat="server"></asp:Literal>】表格字段</td>
		<td>模型管理：
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                onselectedindexchanged="DropDownList1_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </td>
	</tr>
    </table>
               <asp:Table ID="Table1" runat="server" CssClass="cTable_2 table">
        </asp:Table>
    </asp:Panel>
    <asp:TextBox ID="TextBox2" runat="server" CssClass="yincang"></asp:TextBox>
    <div id="cEndToolbar" class="toolbarBg">
      <div class="caButCase">
     
          <asp:Button ID="Button1" runat="server" CssClass="button" Text="确认操作" OnClick="Button1_Click1" OnClientClick="return aa();" />
		    
	        <input type="reset" name="Submit2" value="清空重填" class="button">
      </div>
    </div>
<script type="text/javascript">
    function aa() {
        var g = document.getElementsByTagName("select"); //设置变量接收所查找到的select控件 
        var t1="";
        for (i = 0; i < g.length; i++) //遍历循环赋值 
        {
            if (g[i].id != "DropDownList1") {
                if (document.getElementById(g[i].id).value != "") {
                    if (t1 == "") {
                        t1 = g[i].id + "{next}" + document.getElementById(g[i].id).value;
                    }
                    else {
                        t1 = t1 + "{page}" + g[i].id + "{next}" + document.getElementById(g[i].id).value;
                    }
                }
            }
            
        }
      
document.getElementById("TextBox2").value = t1;
        return true;
    }
</script>
    </form>
</body>
</html>


