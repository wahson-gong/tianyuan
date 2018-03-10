<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin_table_add.aspx.cs" Inherits="admin_admin_table_add" Debug="true" %>
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
		
		<title>管理员添加</title>
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
							<p class="add-input-txt">密码</p><asp:Literal ID="Literal1" runat="server" Text="不修改密码请为空"></asp:Literal>
						</div>
					</td>
				</tr>
                <tr>
					<th><em class="ficon ficon-mima"></em></th>
					<td>
						<div class="pos-relative">
							<asp:TextBox ID="TextBox3" runat="server" TextMode="Password" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">确认密码</p>
						</div>
					</td>
				</tr>
				<tr>
					<th><em class="ficon ficon-xingming"></em></th>
					<td>
						<div class="pos-relative">
							 <asp:TextBox ID="xingming" runat="server" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">真实姓名</p>
						</div>
					</td>
				</tr>
				
				<tr>
					<th><em class="ficon ficon-xingbienv"></em></th>
					<td>
						<div class="radio radio-info radio-inline">
                           <input type="radio" id="nan" name="xingbie" checked="checked">
                            <label for="nan"> 男 </label>
                       </div>
                       	&nbsp;&nbsp;
						<div class="radio radio-info radio-inline">
                           <input type="radio" id="nv" name="xingbie">
                            <label for="nv"> 女 </label>
                       </div>
                        <asp:TextBox ID="xingbie" runat="server" CssClass="yincang"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<th><em class="ficon ficon-deng"></em></th>
					<td>
						<div class="pos-relative">
                            <img src="<%=touxiang_show %>" id="touxiang_show" height="100px" onerror='this.src="images/touxiang.jpg"'  onClick="window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=touxiang&kuangao=100*100','','status=no,scrollbars=no,top=20,left=110,width=600,height=530');"/>
							<asp:TextBox ID="touxiang" runat="server" CssClass="add-input yincang"></asp:TextBox>

						
						</div>
					</td>
				</tr>
				<tr>
					<th><em class="ficon ficon-shouji"></em></th>
					<td>
						<div class="pos-relative">
							 <asp:TextBox ID="shouji" runat="server" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">手机</p>
						</div>
					</td>
				</tr>
				<tr>
					<th><em class="ficon ficon-youxiang1"></em></th>
					<td>
						<div class="pos-relative">
							 <asp:TextBox ID="youxiang" runat="server" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">电子邮箱</p>
						</div>
					</td>
				</tr>
				<tr style="display:none">
					<th><em class="ficon ficon-weixin"></em></th>
					<td>
						<div class="pos-relative">
							 <asp:TextBox ID="openid" runat="server" CssClass="add-input"></asp:TextBox>
							<p class="add-input-txt">微信openid</p>
						</div>
					</td>
				</tr>
				<tr>
					<th><em class="ficon ficon-huiyuan" title="会员组"></em></th>
					<td>
						<asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container1"></asp:DropDownList>
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
		<script src="/inc/layer/layer.js"></script>
		<!--wow 初始化-->
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};

			function admin_js()
			{
			    var length = document.getElementById("TextBox1").value.replace(/[^\x00-\xff]/g, "**").length
			    if (length < 5) {
			        layer.alert("用户名：请输入5-25个字符，一个汉字为两个字符，推荐使用中文");
			        document.getElementById("TextBox1").focus();
			        return false;
			    }
			    if (length > 25) {
			        layer.alert("用户名：请输入5-25个字符，一个汉字为两个字符，推荐使用中文");
			        document.getElementById("TextBox1").focus();
			        return false;
			    }

			    if (document.getElementById("TextBox2").value != "")
			    {
			        if (document.getElementById("TextBox2").value.length <= 5) {
			            layer.alert("密码至少输入6个字符！");
			            document.getElementById("TextBox2").focus();
			            return false;
			        }
			        if (document.getElementById("TextBox2").value != document.getElementById("TextBox3").value) {
			            layer.alert("确认密码输入错误");
			            document.getElementById("TextBox3").focus();
			            return false;
			        }

			    }
			   

			    if (document.getElementById("youxiang").value != "") {
			        var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
			        if (!myreg.test(document.getElementById("youxiang").value)) {
			            layer.alert("请输入有效的E_mail！");
			            document.getElementById("youxiang").focus();
			            return false;
			        }
			    }

			    if (document.getElementById("shouji").value != "") {
			        var myreg = /^1+\d{10}$/;
			        if (!myreg.test(document.getElementById("shouji").value)) {
			            layer.alert("请输入有效的手机号！");
			            document.getElementById("shouji").focus();
			            return false;
			        }

			    }

			}
		</script>
           
		</form>
	</body>
</html>