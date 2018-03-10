<%@ Page Language="C#" AutoEventWireup="true" CodeFile="products_canshu.aspx.cs" Inherits="admin_Fields_add" ValidateRequest="false" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>设置产品参数</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="ckeditor/ckeditor.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="frame_Toolbar" class="toolbarBg"> 
        <uc1:yanz ID="Yanz1" runat="server" />
        <a href="javascript:self.history.back();" class="icoBtn_ct"><span class="btnIco btnBack"></span><span class="btnTxt">返回</span></a> <a href="javascript:self.location.reload();" class="icoBtn_ct"><span class="btnIco btnReload"></span><span class="btnTxt">刷新</span></a> 
       
        <span class="split ftSplit" ></span>
	<div id="guide" class="light_orange">您现在的位置：内容管理 >> <a href="produts.aspx?classid=<%=my_b.get_value("classid", "sl_product", "where id=" + Request.QueryString["chanpinbianhao"].ToString() + "")%>&Model_id=29">产品管理</a> >> 设置产品参数</div>
	<div class="pageInfo right light_gray"></div>
    </div>
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>产品展示</div></td>
		<td colspan="2"></td>
	</tr>
    <tr>
        <td colspan="3">
             <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
	            <tr>
                    <td width="15%"><strong>缩略图</strong></td>
		            <td><strong>产品名</strong></td>
		            <td width="10%" align="left">积分</td>
		
                    <td width="10%"><strong>库存</strong></td>
		            
		            
	            </tr>
                 <asp:Repeater ID="Repeater3" runat="server">
	  <ItemTemplate>
            <tr>
		            <td width="10%"><img src="<%#DataBinder.Eval(Container.DataItem, "suoluetu")%>" width="120px" /></td>
		            <td><%#DataBinder.Eval(Container.DataItem, "biaoti")%></td>
		            <td width="10%"><%#DataBinder.Eval(Container.DataItem, "jifen")%></td>
		  
		            <td width="10%"><%#DataBinder.Eval(Container.DataItem, "kucun")%></td>
	            </tr>
   </ItemTemplate></asp:Repeater>
	</table>
        </td>
    </tr>
	
	      </table>
    <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>颜色管理</div></td>
		<td colspan="2"></td>
	</tr>
    <tr>
        <td colspan="3">
             <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
	            <tr>
		            <td><strong>颜色名</strong></td>
		            <td width="10%" align="left">类型</td>
		            <td width="15%"><strong>颜色值</strong></td>
		            <td width="15%"><strong>对应图片</strong></td>
		            <td width="20%"><strong>操作</strong></td>
	            </tr>
      <asp:Repeater ID="Repeater1" runat="server">
	  <ItemTemplate>
            <tr>
		            <td><%#DataBinder.Eval(Container.DataItem, "yanseming")%></td>
		            <td width="10%" align="left"><%#DataBinder.Eval(Container.DataItem, "yansezhileixing")%></td>
		            <td width="15%"><%#str_color(DataBinder.Eval(Container.DataItem, "yansezhileixing").ToString(), DataBinder.Eval(Container.DataItem, "yansezhi").ToString())%></td>
		            <td width="15%"><img src="<%#DataBinder.Eval(Container.DataItem, "duiyingtupian")%>" width="120px" /></td>
		            <td width="20%"><a href="products_canshu.aspx?chanpinbianhao=<%=Request.QueryString["chanpinbianhao"].ToString() %>&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&type=yanseedit">编辑</a> | <a href="products_canshu.aspx?chanpinbianhao=<%=Request.QueryString["chanpinbianhao"].ToString() %>&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&type=yansedel" onclick="return confirm('你确认将该信息到回收站?\r\n注意：相关栏目和内容也将一起删除到回收站');">删除</a></td>
	            </tr>
      </ItemTemplate>
      </asp:Repeater>
	</table>
        </td>
    </tr>
	<tr>
		<td class="tRight">颜色名：</td>
		<td colspan="2"><asp:TextBox ID="TextBox1" runat="server" Width="300px" ></asp:TextBox></td>
	</tr>
    <tr>
		<td class="tRight">颜色值类型：</td>
		<td colspan="2">
            <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                RepeatDirection="Horizontal">
                <asp:ListItem Selected="True">代码</asp:ListItem>
                <asp:ListItem>图片</asp:ListItem>
            </asp:RadioButtonList>
        </td>
	</tr>
    <tr>
		<td class="tRight">颜色值：</td>
		<td colspan="2"><asp:TextBox ID="TextBox2" runat="server" Width="300px" ></asp:TextBox>&nbsp;<input id="Button2" class="button" type="button" value="上传图片"  onClick="window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=TextBox2','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');"/></td>
	</tr>
    <tr>
		<td class="tRight">对应图片：</td>
		<td colspan="2"><asp:TextBox ID="TextBox3" runat="server" Width="300px" ></asp:TextBox>&nbsp;<input id="Button3" class="button" type="button" value="上传图片"  onClick="window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=TextBox3','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');"/></td>
	</tr>
    <tr>
        <td>&nbsp;</td>
        <td colspan="2"><asp:Button ID="Button1" runat="server" CssClass="button" Text="确认操作" OnClick="Button1_Click1" /></td>
    </tr>
	      </table>
   <table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>尺码管理</div></td>
		<td colspan="2"></td>
	</tr>
    <tr>
        <td colspan="3">
             <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
	            <tr>
		            <td><strong>尺寸名</strong></td>
                      <td width="20%"><strong>操作</strong></td>
	            </tr>
      <asp:Repeater ID="Repeater2" runat="server">
	  <ItemTemplate>
            <tr>
		            <td><%#DataBinder.Eval(Container.DataItem, "chicunming")%></td>
		        
		            <td width="20%"><a href="products_canshu.aspx?chanpinbianhao=<%=Request.QueryString["chanpinbianhao"].ToString() %>&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&type=chicunedit">编辑</a> | <a href="products_canshu.aspx?chanpinbianhao=<%=Request.QueryString["chanpinbianhao"].ToString() %>&id=<%#DataBinder.Eval(Container.DataItem, "id")%>&type=chicundel" onclick="return confirm('你确认将该信息到回收站?\r\n注意：相关栏目和内容也将一起删除到回收站');">删除</a></td>
	            </tr>
      </ItemTemplate>
      </asp:Repeater>
	</table>
        </td>
    </tr>
	<tr>
		<td class="tRight">尺寸名：</td>
		<td colspan="2"><asp:TextBox ID="TextBox4" runat="server" Width="300px" ></asp:TextBox></td>
	</tr>

    <tr>
        <td>&nbsp;</td>
        <td colspan="2"><asp:Button ID="Button6" runat="server" CssClass="button" 
                Text="确认操作" onclick="Button6_Click"  /></td>
    </tr>
	      </table>
<table class="cTable_2 table">
	<tr class="cTitle toolbarBg">
		<td width="25%"><div>价格管理</div></td>
		<td colspan="2"></td>
	</tr>
    <tr>
        <td colspan="3">
              <asp:Table ID="Table1" runat="server" CssClass="cTable_2 table">
              </asp:Table>
        </td>
    </tr>


    <tr>
        <td>&nbsp;</td>
        <td colspan="2">
            <asp:Button ID="Button4" runat="server" CssClass="button" 
                Text="确认操作" onclick="Button4_Click"  /></td>
    </tr>
	      </table>
    </form>
</body>
</html>
