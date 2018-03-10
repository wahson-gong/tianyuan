<%@ Page Language="C#" AutoEventWireup="true" CodeFile="get_password.aspx.cs" Inherits="admin_get_password" %>
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
		
		<title>修改密码</title>
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
			    &nbsp;>&nbsp;<a href="admin_table.aspx">管理员管理</a>  &nbsp;>&nbsp;管理员添加
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<table class="add-table">
				<tr>
					<th><em class="ficon ficon-yonghuming"></em></th>
					<td>
						<div class="pos-relative">
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">后台管理名</p>
						</div>
					</td>
				</tr>
                <tr>
					<th><em class="ficon ficon-mima"></em></th>
					<td>
						<div class="pos-relative">
							<asp:TextBox ID="TextBox2" runat="server" TextMode="Password" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">后台旧密码</p>
						</div>
					</td>
				</tr>
                <tr>
					<th><em class="ficon ficon-mima"></em></th>
					<td>
						<div class="pos-relative">
							<asp:TextBox ID="TextBox3" runat="server" TextMode="Password" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">后台新密码</p>
						</div>
					</td>
				</tr>
				
				
				<tr>
					<th></th>
					<td>
						<asp:LinkButton ID="LinkButton1" CssClass="btn btn-blue marb-20" runat="server" OnClick="LinkButton1_Click" OnClientClick="return admin_js()" ><em class="ficon ficon-queren"></em> 确认操作</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton2" CssClass="btn btn-green marb-20" runat="server" OnClick="LinkButton2_Click"><em class="ficon ficon-shanchu"></em> 清空重填</asp:LinkButton>
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