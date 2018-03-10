<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_table.aspx.cs" Inherits="admin_Order_table" %>
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
        <style type="text/css">
		.hq_box{
			width: 100%;
			position: relative;
			
		}
		.tree {
    min-height:20px;
    padding:19px;
    margin-bottom:20px;
    background-color:#fbfbfb;
    border:1px solid #999;
    -webkit-border-radius:4px;
    -moz-border-radius:4px;
    border-radius:4px;
    -webkit-box-shadow:inset 0 1px 1px rgba(0, 0, 0, 0.05);
    -moz-box-shadow:inset 0 1px 1px rgba(0, 0, 0, 0.05);
    box-shadow:inset 0 1px 1px rgba(0, 0, 0, 0.05);
    padding-left: 0;
}
.tree>ul{
	padding-left: 0;
}
.tree li {
    list-style-type:none;
    margin:0;
    padding:10px 5px 0 5px;
    position:relative
}
.tree li::before, .tree li::after {
    content:'';
    left:-20px;
    position:absolute;
    right:auto
}
.tree li::before {
    border-left:1px solid #999;
    bottom:50px;
    height:100%;
    top:0;
    width:1px
}
.tree li::after {
    border-top:1px solid #999;
    height:20px;
    top:25px;
    width:25px
}
.tree li span {
    -moz-border-radius:5px;
    -webkit-border-radius:5px;
    border:1px solid #60afe7;
    border-radius:3px;
    display:inline-block;
    padding:3px 8px;
    text-decoration:none;
    min-width: 130px;
}
.tree li.parent_li>span {
    cursor:pointer
}
.tree>ul>li::before, .tree>ul>li::after {
    border:0
}
.tree li:last-child::before {
    height:30px
}
.tree li.parent_li>span:hover, .tree li.parent_li>span:hover+ul li span {
    background:#ddd;
    border:1px solid #94a0b4;
    color:#000
}
.tree li li{
	/*display: none;*/
}
[class^="icon-"],
[class*=" icon-"] {
	display: inline-block;
	width: 14px;
	height: 14px;
	margin-top: 3px;
	*margin-right: .3em;
	line-height: 14px;
	vertical-align: text-top;
	background-image: url("img/glyphicons-halflings.png");
	background-position: 16px 14px;
	background-repeat: no-repeat
}
.icon-plus-sign {
	background-position: -407px -96px
}

.icon-minus-sign {
	background-position: -432px -96px
}
.icon-leaf {
	background-position: -48px -120px
}
.icon-folder-open {
	width: 16px;
	background-position: -384px -143px;
}
.tree>ul>.parent_li>span{
	background-color: #1c85d1;
	color: #fff;
	border: 1px solid #60afe7;
}
.parent_li>span{
	background-color: #60afe7;
	color: #fff;
}
@media only screen and (max-width: 640px) {
	.tree li span{
		min-width: 100px;
	}
}
.tree ul{
    padding-left:40px
}
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
			    &nbsp;>&nbsp;内容管理 &nbsp;>&nbsp;<asp:Literal ID="Literal4" runat="server"></asp:Literal><asp:Literal ID="Literal3" runat="server"></asp:Literal>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
			<br />
			<!--筛选条件-->
			<div class="filter">
				
				<div class="filter-left">
					<div class="form-control">
					时间：从<asp:TextBox ID="TextBox1" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>至<asp:TextBox ID="TextBox2" runat="server" CssClass="dinput" onClick="WdatePicker()"></asp:TextBox>
				    </div>
				    <div class="form-control">
					    状态：
					    <div class="select-container">  
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container"></asp:DropDownList>
	 					</div>  
					    
					    &nbsp;&nbsp;
				    </div>
                    <div class="form-control">
					    激活：
					    <div class="select-container">  
                            <asp:DropDownList ID="DropDownList2" runat="server" CssClass="select-container">
                                <asp:ListItem>全部</asp:ListItem>
                                <asp:ListItem>是</asp:ListItem>
                                <asp:ListItem>否</asp:ListItem>
                            </asp:DropDownList>
	 					</div>  
					    
					    &nbsp;&nbsp;
				    </div>
			
				</div>
				
				<div class="filter-right">
					<div class="form-control">
                        <asp:TextBox  ID="TextBox3" runat="server" class="input" placeholder="搜索条件"></asp:TextBox>
				    	
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-blue" OnClick="Button1_Click1"
                Text="搜索" />
                        <a href="user_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>" class="btn btn-green"> <em class="ficon ficon-tianjia"></em> 添加</a>
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
	                    <input type="checkbox" id="check-all" onclick="selcheck('content')">
	                    <label for="check-all">全选</label>
	                 </div>
	                 <div class="fl ft14">
	                 	&nbsp;&nbsp;
	                     <a href="javascript:chk1()" title="删除" class="operation gray"><span class="ficon ficon-qingkong"></span> 删除</a>
           &nbsp;&nbsp; <asp:Literal ID="Literal5" runat="server"></asp:Literal>
                         
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
            <div class="hq_box">
			

		</div>
			
			 <uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		
              <script src="/inc/layer/layer.js"></script>
		
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
 
      var tiao_url ="user_table_add.aspx?Model_id=<%=Request.QueryString["Model_id"].ToString() %>&type=del&id="+t1;
    setajax(tiao_url, "yes");
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

