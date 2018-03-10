<%@ Page Language="C#" AutoEventWireup="true" CodeFile="err.aspx.cs" Inherits="fzw_admin_err"  ValidateRequest="false"%>

<!DOCTYPE html>
<html>
	<head id="Head1" runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<meta name="renderer" content="webkit|ie-comp|ie-stand">
		<meta name="viewport" content="width=device-width,initial-scale=1">
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<link rel="stylesheet" type="text/css" href="css/animate.min.css" />
		<link rel="stylesheet" type="text/css" href="fonts/css/fontello.css"/>
		<link rel="stylesheet" type="text/css" href="css/common.css"/>
		<!--[if lt IE 8]>
			<link rel="stylesheet" href="fonts/css/fontello-ie7.css">
		<![endif]-->
		<script src="js/jquery-1.11.2.min.js" type="text/javascript"></script>
		<!--[if lt IE 9]>
		      <script src="js/html5shiv.min.js"></script>
		      <script src="js/respond.min.js"></script>
		    <![endif]-->
		
		<title>网站后台管理系统</title>
		
		  <script type="text/javascript">
 var jj=<%=count_s %>;
var timer;
 function intopwd()
  {
   
      jj=jj-1;
     document.getElementById("d1").innerHTML=jj+"秒过后返回！可以点击马上返回！";
	 if(jj==1)
	 {
	  window.location="<%=errurl %>";
	   clearTimeout(timer);
	 }
	
	 
  }
function aa()
{
timer=setInterval("intopwd()",1000);
}
    </script>
        <style>
            body, html {
                height:100%;
            }
            .errbox{
                width:80%;
                max-width:500px;
                margin:0 auto;
                padding-top:15%;
                font-size:16px;
            }
        </style>


	</head>
	<body class="body-color">
        <form id="form1" runat="server">
		<!--主体部分-->
			
		<!--数据分析-->
			<section class="errbox">
				<div class="home-modular">
				    <div class="home-modular-header" style="font-size:20px">
				        <span class="ficon ficon-shujutongji"></span>&nbsp;操作提示
				    </div>
				    <div class="home-modular-contet">
				    	<div class="data-analysis">
				    	    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br><a href="<%=errurl %>" style="color:#0094ff"><div id="d1"></div></a>
				    	</div>
				      
				        <p class="clear"></p>
				    </div>
				</div>
			</section>
			<!--数据分析 end -->
			
		
            <script type="text/javascript">
aa();
</script>
            </form>
	</body>
</html>
