<%@ Page Language="C#" AutoEventWireup="true" CodeFile="caijian.aspx.cs" Inherits="admin_pic_Cut" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>裁剪图片</title>
 	<LINK href="css/jquery.Jcrop.css" type="text/css" rel="Stylesheet" media="screen">
		<script type="text/javascript" src="js/jquery-1.3.2.min.js"></script>
		<script type="text/javascript" src="js/jquery.Jcrop.min.js"></script>
		<script type="text/javascript" src="js/jQuery.UtrialAvatarCutter.js"></script>
		<style>
			div{float:left;margin-right:10px;}
			input{clear:both;float:left;margin-top:20px;}
		</style>
</head>
<body>
    <form id="form1" runat="server">
   <!--
			原始图
		-->
		<div id="picture_original">
            <asp:Image ID="Image1" runat="server" />
		</div>
		<!---
			缩略图
		-->
		<div id="picture_200"></div>
		<div id="picture_50"></div>
		<div id="picture_30"></div>
		<br>

		<input type="button" value="确定" id="save_btn"/>
		<SCRIPT type="text/javascript">
			var cutter = new jQuery.UtrialAvatarCutter(
				{
					//主图片所在容器ID
					content : "picture_original",
					
					//缩略图配置,ID:所在容器ID;width,height:缩略图大小
					purviews : [{id:"picture_200",width:200,height:200},{id:"picture_50",width:50,height:50},{id:"picture_30",width:30,height:30}],
					
					//选择器默认大小
					selector : {width:200,height:200}
				}
			);
	
			$(window).load(function(){
				cutter.init();

				//确定按钮动作
				$("#save_btn").click(
					function(){
						var data = cutter.submit();
						alert("x="+data.x+"\ny="+data.y+"\nw="+data.w+"\nh="+data.h+"\ns="+data.s);
					}
				);
			});
			
	
		</SCRIPT>
    </form>
</body>
</html>
