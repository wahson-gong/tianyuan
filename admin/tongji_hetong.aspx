<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tongji_hetong.aspx.cs" Inherits="admin_Order_table" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1,user-scalable=no">
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
		
		<title>表单管理</title>
		<style type="text/css">
			
		</style>
         <script src="js/jquery.min.js" type="text/javascript" charset="utf-8"></script>
		<script src="js/jquery.SuperSlide.2.1.1.js" type="text/javascript" charset="utf-8"></script>
		<script type="text/javascript" src="DatePicker/WdatePicker.js"></script>
        		<!--<script src="js/bootstrap.min.js" type="text/javascript"></script>-->
		<script src="js/wow.min.js" type="text/javascript"></script>
		<script src="js/common.js" type="text/javascript"></script>
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
			    &nbsp;>&nbsp;内容管理 &nbsp;>&nbsp;<asp:Literal ID="Literal4" runat="server"></asp:Literal><asp:Literal ID="Literal3" runat="server"></asp:Literal>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<!--筛选条件-->
			<div class="filter">
				
				<div class="filter-left">
					<div class="form-control">
					最近&nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="dinput" Width="60px"></asp:TextBox>&nbsp;天的记录
				    </div>
				    <div class="form-control">
					    状态：
					    <div class="select-container">  
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container1"></asp:DropDownList>
	 					</div>  
					    
					    &nbsp;&nbsp;
				    </div>
			
				</div>
				
				<div class="filter-right">
					<div class="form-control">
                        <asp:TextBox  ID="TextBox3" runat="server" class="input" placeholder="搜索条件"></asp:TextBox>
				    	
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click1"
                Text="搜索" />
                        &nbsp;
				    <a href="hetong_table_add.aspx?&Model_id=<%=Request.QueryString["Model_id"].ToString() %>" class="btn btn-green"> + 添加信息</a>
				    </div>
				
				</div>
				<br />
				<p class="clear-right"></p>
			</div>
			<!--筛选条件 end-->
			
			<!--文章table-->
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
			
			<!--文章table  end-->
			<!--操作部分-->
			<div class="o-hidden padtb-20">
			    
			
				<div class="fl full-sm">
				    <div class="checkbox checkbox-success fl" style="margin-bottom: 0;">
	                 <%=shijian %>天续费金额：<asp:Literal ID="Literal5" runat="server"></asp:Literal>元，续费总金额：<asp:Literal ID="Literal6" runat="server"></asp:Literal>元
	                 </div>
	            
	                 <p class="clear"></p>
				</div>
				<div class="fr full-sm">
				    <div class="pagelist">
				       <asp:Literal ID="Literal1" runat="server"></asp:Literal>
				    </div>
				</div>
			</div>
			<!--操作部分 end-->

			
			 <uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		

		
		<!--wow 初始化-->
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};
		</script>
         <script type="text/javascript">
function chk1()
{
    var boxes = document.getElementsByName("chk"); 
    var t2=0;
    var t1="";
    for (var i = 0; i < boxes.length; i++)   
    {
     if (boxes[i].checked)   
     {
        t2=t2+1;
        if(t2==1)
        {
        t1=t1+boxes[i].value;
        }
        else
        {
        t1=t1+","+boxes[i].value;
        }
     }
    }
    $.ajax({
        type: 'get',
        url: "hetong_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>&type=del&id="+t1,
        success: function (data) {
            alert("删除成功");
            self.location.reload();
        }
    })
   
}
    //注意：下面的代码是放在/Poster/rightgp调用
    $(window.parent.document).find("#main_data").load(function () {
        var main = $(window.parent.document).find("#main_data");
        var mainheight = $(document).height() + 30;
        main.height(mainheight);
    });
        </script> 

		</form>
	</body>
</html>

