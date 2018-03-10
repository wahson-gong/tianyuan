<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_table_down.aspx.cs" Inherits="admin_auto_table" EnableEventValidation="false" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1">
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
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
		
		<title>���ݵ���</title>
         <script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
	</head>
	<body class="body-color">
        <form id="form1" runat="server">
		 <uc1:top ID="top1" runat="server" />
		<!--��߲˵���-->
		<aside class="side-nav">
			<div class="pos-relative">
			<ul class="side-nav-ul ">
				<%=Cache["date"] %>
			</ul>
			<em class="sj"></em>
			</div>
		</aside>
		<!--��߲˵��� end-->
		
		<!--���岿��-->
		<section class="body-content">
			<!--��ǰλ��-->
			<div class="site-location">
				&nbsp;&nbsp;
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="default.aspx">�û���ҳ</a>
			    &nbsp;>&nbsp;���ݹ��� &nbsp;>&nbsp;<a href="yhq_table.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>">������</a>
			</div>
			<p class="silo_zw"></p>
			<!--��ǰλ�� end-->
			<br />
			<!--ɸѡ����-->
			<div class="filter">
				
				<div class="filter-left">
					<div class="form-control">
					<asp:Literal ID="Literal3" runat="server"></asp:Literal>&nbsp;&nbsp;��<asp:TextBox ID="TextBox1" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>��<asp:TextBox ID="TextBox2" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>
				    </div>
				   
				
				<div class="filter-right">
					<div class="form-control">
                        <asp:TextBox  ID="TextBox3" runat="server" class="input" placeholder="��������"></asp:TextBox>
				    	
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click1"
                Text="����" />
				    </div>
				    &nbsp;
                     <asp:Button ID="Button2" runat="server" CssClass="btn btn-green" 
                Text="��������" OnClick="Button2_Click1" />
				   
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--ɸѡ���� end-->
			
			<!--����table-->
			
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-border table-hover">
    </asp:GridView>
			<!--����table  end-->
			

			
			 <uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		
		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
		
		<!--wow ��ʼ��-->
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};
		</script>
		</form>
	</body>
</html>
