<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Generate.aspx.cs" Inherits="admin_Generate" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1">
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<!--<link rel="stylesheet" type="text/css" href="css/bootstrap.min.css"/>-->
		<link rel="stylesheet" type="text/css" href="css/animate.min.css" />
		<link rel="stylesheet" type="text/css" href="fonts/css/fontello.css"/>
		<link rel="stylesheet" type="text/css" href="css/components.css"/>
		<link rel="stylesheet" type="text/css" href="css/common.css"/>
		<!--[if lt IE 8]>
			<link rel="stylesheet" href="fonts/css/fontello-ie7.css">
		<![endif]-->
		<script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
		<!--[if lt IE 9]>
		      <script src="js/html5shiv.min.js"></script>
		      <script src="js/respond.min.js"></script>
		    <![endif]-->
		
		<title>生成静态页面</title>
		<style type="text/css">
			
		</style>
		
	</head>
	<body class="body-color">
        <form id="form1" runat="server">
		<uc1:top ID="top1" runat="server" />
		<!--侧边菜单栏-->
		<aside class="side-nav">
			<div class="pos-relative">
			<ul class="side-nav-ul ">
				<%=Cache["date"] %>
			</ul>
			<em class="sj"></em>
			</div>
		</aside>
		<!--侧边菜单栏 end-->
		
		<!--主体部分-->
		<section class="body-content">
			<!--当前位置-->
			<div class="site-location">
				&nbsp;&nbsp;
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="default.aspx">用户首页</a>
			    &nbsp;>&nbsp;生成管理 &nbsp;>&nbsp;<a href="Generate.aspx">生成静态页面</a>
		  </div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="table table-info cs-table">
				<tr>
					<th  style="width:60px">数据模型</th>
					<td>
						<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" CssClass="select-container1">
            </asp:DropDownList>
					</td>
				</tr>
				<tr>
					<th>栏目</th>
					<td>
						<asp:ListBox ID="ListBox1" runat="server" Rows="10" CssClass="select-container2" Width="250px"></asp:ListBox>
					</td>
				</tr>
             <tr>
					<th>生成类型</th>
					<td>
						<asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged"  >
                <asp:ListItem Selected="True">列表页</asp:ListItem>
                <asp:ListItem>内容页</asp:ListItem>
                <asp:ListItem>主栏目</asp:ListItem>
            </asp:RadioButtonList>
					</td>
				</tr>
                 <asp:Panel ID="Panel1" runat="server" Height="50px" Width="125px">
                <tr>
					<th>排序类型</th>
					<td>
						<asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem>添加时间</asp:ListItem>
                <asp:ListItem Selected="True">文章ID</asp:ListItem>
            </asp:RadioButtonList>
					</td>
				</tr>
                <tr>
					<th>排序方式</th>
					<td>
                   <asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem>正序</asp:ListItem>
                <asp:ListItem Selected="True">倒序</asp:ListItem>
            </asp:RadioButtonList>
					</td>
				</tr>
                <tr>
					<th>生成数量</th>
					<td>
						指定生成数量：<asp:TextBox ID="TextBox1" runat="server" Width="60px" CssClass="dinput">1000</asp:TextBox>
            （0：生成全部）
					</td>
				</tr>
       </asp:Panel>
				<tr>
					<th></th>
					<td>
						<asp:LinkButton ID="LinkButton1" CssClass="btn btn-blue" runat="server" OnClick="LinkButton1_Click"><em class="ficon ficon-queren"></em> 确认操作</asp:LinkButton>&nbsp;&nbsp;

                      <asp:LinkButton ID="LinkButton2" CssClass="btn btn-green" runat="server" OnClick="LinkButton2_Click"><em class="ficon ficon-shanchu"></em> 清空重填</asp:LinkButton>
					</td>
				</tr>
			</table>
			
			<uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		
		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
		
		<!--wow 初始化-->
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};
		</script>
		</form>
	</body>
</html>

