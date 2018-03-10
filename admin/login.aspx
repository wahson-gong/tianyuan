<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="admin_login" %>
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
		
		<title>后台登陆</title>
		<style type="text/css">
			html{
				height: 100%;
			}
		</style>
		
	</head>
	<body class="login-bg" onload="document.getElementById('TextBox1').focus();">
		
		<div class="login">
		    <form id="form1" runat="server">
		    <div class="login-content">
		    	<p><a href="login.aspx"><img src="images/login-logo.png"/></a></p>
		    <br />
		        <div class="login-form fl">
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="login-input" OnChange="get_touxiang()" placeholder="用户名" onkeydown="onKeyDown()"></asp:TextBox>
		            <span class="ficon ficon-user"></span>
		        </div>
		        <span class="dot fl">&hellip;</span>
		        
		        <div class="login-form fr">
	                    <asp:TextBox ID="TextBox2" runat="server" CssClass="login-input" TextMode="Password" placeholder="密码" onkeydown="onKeyDown()"></asp:TextBox>
		            <span class="ficon ficon-mima"></span>
		        </div>
		        <span class="dot fr">&hellip;</span>
		        <img src="images/one.png" class="login-touxiang" id="touxiang"  onerror='this.src="images/one.png"'/>
		        <p>
		        	<br />
            
		        	<a href="#" class="btn btn-default" onclick="enter();"><em class="ficon ficon-shuaxin"></em> 登录</a>
		        </p>
		        <div class="progress_bar">
			        <div class="progress"></div>
			    </div>
		    </div>
                </form>
		</div>
		 
		
		<script src="js/common.js" type="text/javascript"></script>
		<script src="js/wow.min.js" type="text/javascript"></script>
		<!--wow 初始化-->
		<script>
			if (!(/msie [6|7|8|9]/i.test(navigator.userAgent))) {
				new WOW().init();
			};
		</script>
		<style type="text/css">
		
		.animated {
  -webkit-animation-duration: 1s;
          animation-duration: 1s;
  -webkit-animation-fill-mode: both;
          animation-fill-mode: both;
}
.animated-delay{
  	animation-delay:.5s;
	-webkit-animation-delay:.5s; 
  }


@-webkit-keyframes fadeLeft {
  0% {
    opacity: 1;
  }
  
  100% {
    opacity: 0;
    -webkit-transform: translate3d(-30px, 0, 0);
            transform: translate3d(-30px, 0, 0);
  }
}

@keyframes fadeLeft {
  0% {
    opacity: 1;
  }

  100% {
    opacity: 0;
    -webkit-transform: translate3d(-30px, 0, 0);
            transform: translate3d(-30px, 0, 0);
  }
}

.fadeLeft {
  -webkit-animation-name: fadeLeft;
          animation-name: fadeLeft;
}
@-webkit-keyframes fadeRight {
  0% {
    opacity: 1;
  }

  100% {
    opacity: 0;
    -webkit-transform: translate3d(30px, 0, 0);
            transform: translate3d(30px, 0, 0);
  }
}

@keyframes fadeRight {
  0% {
    opacity: 1;
  }

  100% {
    opacity: 0;
    -webkit-transform: translate3d(30px, 0, 0);
            transform: translate3d(30px, 0, 0);
  }
}

.fadeRight {
  -webkit-animation-name: fadeRight;
          animation-name: fadeRight;
}

@-webkit-keyframes fadeUp {
  0% {
    opacity: 1;
  }

  100% {
    opacity: 0;
    -webkit-transform: translate3d(0, -30px, 0);
            transform: translate3d(0, -30px, 0);
  }
}

@keyframes fadeUp {
  0% {
    opacity: 1;
  }

  100% {
    opacity: 0;
    -webkit-transform: translate3d(0, -30px, 0);
            transform: translate3d(0, -30px, 0);
  }
}

.fadeUp {
  -webkit-animation-name: fadeUp;
          animation-name: fadeUp;
}


@-webkit-keyframes fadeUpIn {
  0% {
    opacity: 0;
  }

  100% {
    opacity: 1;
    -webkit-transform: translate3d(0, -30px, 0);
            transform: translate3d(0, -30px, 0);
  }
}

@keyframes fadeUpIn {
  0% {
    opacity: 0;
  }

  100% {
    opacity: 1;
    -webkit-transform: translate3d(0, -30px, 0);
            transform: translate3d(0, -30px, 0);
  }
}

.fadeUpIn {
  -webkit-animation-name: fadeUpIn;
          animation-name: fadeUpIn;
}


		</style>
		<script type="text/javascript">
		    function onKeyDown() {
		        var code = event.keyCode;
		        if (code == 13) {
		            enter();
		            return false;
		        }

		    }
		    function get_touxiang()
		    {
		        $.ajax({
		            type: 'get',
		            url: 'single.aspx?m=temp/touxiang&yonghuming=' + $("#TextBox1").val(),
		            success: function (data) {
		                if (data != "")
		                {
		                    document.getElementById("touxiang").src = data;
		                }
		                
		            }
		        })
		    }
			function enter(){
				$(".login-form.fl").addClass("animated fadeRight");
				$(".login-form.fr").addClass("animated fadeLeft");
				$(".btn-default").addClass("animated fadeUp");
				$(".login-touxiang").addClass("animated animated-delay fadeUp");
				$(".progress_bar").addClass("animated animated-delay fadeUpIn");
				$(".progress").delay(300).animate({ "width": "100%" }, 1000);
				var formData = $("#form1").serialize();
				$.ajax({
				    url: 'loginajax.aspx?type=login',
				    data: formData,
				    contentType: "application/x-www-form-urlencoded; charset=utf-8",
				    success: function (data) {
				       // alert(data);
				        var t2 = window.setTimeout("aj('" + data + "')", 1000);
				        

				    }
				})
				

			}
			function aj(data)
			{
			    if (data == "ok") {
			        window.location = "default.aspx";
			    }
			    else if (data == "err") {
			        window.location = "err.aspx?err=登陆失败，帐号或密码错误，请重新登陆！&errurl=login.aspx";
			    }
			    else {
			        window.location = "err.aspx?err=操作不正确！&errurl=/";
			    }
			}
		</script>
           
	</body>
</html>
