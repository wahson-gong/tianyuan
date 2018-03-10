<%@ Page Language="C#" AutoEventWireup="true" CodeFile="show_order_table.aspx.cs" Inherits="admin_show_auto_table" %>

<%@ Register Src="yanz.ascx" TagName="yanz" TagPrefix="uc1" %>
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
		
		<title>查看信息</title>
		<style type="text/css">
			td{
                border: 1px solid #eee;
			}
		    #Table1 {
                background:#fff;
		    }
		    .body-content {
                margin:0
		    }
            #Table1 tr td:nth-child(2) {
                padding-left:12px;
		    }
		</style>
		
	</head>
	<body class="body-color">
        <form id="form1" runat="server">
	

		
		<!--主体部分-->
		<section class="body-content">
			<!--当前位置-->
			<div class="site-location">
				&nbsp;&nbsp;
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="default.aspx">用户首页</a>
			    &nbsp;>&nbsp;内容管理 &nbsp;>&nbsp;查看信息
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			
			  <asp:Table ID="Table1" runat="server" CssClass="table table-info cs-table">
        </asp:Table>
		    <table class="table table-info cs-table">
                  <tr>

		<td width="18%"><strong>服务产品</strong></td>
		<td  align="left">&nbsp;</td>
		<td width="16%"><strong>价格</strong></td>

		<td width="10%"><strong>数量</strong></td>

		<td width="10%"><strong>小结</strong></td>

	  </tr>
	    <asp:Repeater ID="Repeater1" runat="server">
	  <ItemTemplate>
	  <tr>

		<td><img src="<%#my_b.get_value("suoluetu", "sl_product", "where id=" + DataBinder.Eval(Container.DataItem, "laiyuanbianhao") + "")%>" width="61" height="69" /></td>
		<td align="left">
        <%#DataBinder.Eval(Container.DataItem, "biaoti")%>&nbsp;
            <a href="/page.aspx?id=<%#DataBinder.Eval(Container.DataItem, "laiyuanbianhao")%>&classid=<%#my_b.get_value("classid", "sl_product", "where id=" + DataBinder.Eval(Container.DataItem, "laiyuanbianhao") + "")%>" target="_blank" class="btn btn-green">查看</a>
            <br />
            <%#DataBinder.Eval(Container.DataItem, "beizhu")%>
        </td>
		<td>￥<%#my_b.get_jiage(float.Parse(DataBinder.Eval(Container.DataItem, "danjia").ToString()))%></td>

        <td><%#DataBinder.Eval(Container.DataItem, "shuliang")%></td>

		<td>￥<%#my_b.get_jiage(float.Parse(DataBinder.Eval(Container.DataItem, "xiaoji").ToString()))%></td>
	   
	  </tr>
                 </ItemTemplate>
	  </asp:Repeater>
                </table>
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
