<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>
<%@ Register src="ascx/top.ascx" tagname="top" tagprefix="uc1" %>
<%@ Register src="ascx/botton.ascx" tagname="botton" tagprefix="uc2" %>
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
		<script src="http://cdn.hcharts.cn/highcharts/highcharts.js"></script>
<script src="http://cdn.hcharts.cn/highcharts/modules/exporting.js"></script>

<script src="/inc/layer/layer.js"></script>

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
			
			<!--管理模块-->
			<section>
			<ul class="menu-list">
				<asp:Repeater ID="Repeater1" runat="server">
                 <ItemTemplate>
				<li class="<%#DataBinder.Eval(Container.DataItem, "u6").ToString()%>">
					<a href="<%#DataBinder.Eval(Container.DataItem, "u3").ToString()%>">
						<span class="ficon <%#DataBinder.Eval(Container.DataItem, "u5").ToString()%>"></span>
						<br /><%#DataBinder.Eval(Container.DataItem, "u1").ToString()%>
					</a>
				</li>
                </ItemTemplate>
                 </asp:Repeater>
			</ul>
			</section>
			<!--管理模块 end-->
			
			<!--数据分析-->
			<section>
				<div class="home-modular">
				    <div class="home-modular-header">
				        <span class="ficon ficon-shujutongji"></span>&nbsp;数据分析
				    </div>
				    <div class="home-modular-contet">
				    	<div class="data-analysis" id="container">
				    	    
				    	</div>
				        <div class="data-type">
                            <ul style="line-height:29px">
                                <li>IP地址：<asp:Literal ID="Literal1" runat="server"></asp:Literal></li>
                                <li>域名：<asp:Literal ID="Literal2" runat="server"></asp:Literal></li>
                                <li>端口：<asp:Literal ID="Literal3" runat="server"></asp:Literal></li>
                                <li>本文件所在路径：<asp:Literal ID="Literal4" runat="server"></asp:Literal></li>
                                <li>操作系统：<asp:Literal ID="Literal5" runat="server"></asp:Literal></li>
                                <li>.NET Framework 版本：<asp:Literal ID="Literal6" runat="server"></asp:Literal></li>
                                <li>脚本超时时间：<asp:Literal ID="Literal7" runat="server"></asp:Literal> 秒</li>
                                <li>启动到现在已运行： <asp:Literal ID="Literal8" runat="server"></asp:Literal> 分钟</li>
                                <li>CPU 数量： <asp:Literal ID="Literal9" runat="server"></asp:Literal></li>
                                <li>CPU类型： <asp:Literal ID="Literal10" runat="server"></asp:Literal></li>
                                <li>服务器名称： <asp:Literal ID="Literal11" runat="server"></asp:Literal> </li>
                                <li>数据库版本： <asp:Literal ID="Literal12" runat="server"></asp:Literal> </li>
                            </ul>
				           
				        </div>
				        <p class="clear"></p><br />
                         <ul class="data-type-list" style="border-top:1px solid #ddd; padding-top:10px; display:none">
				            	
				            	<li>
				            		<a href="user_table.aspx?Model_id=23&u1=&leixing=会员&shijian1=&shijian2=">
				            			<span class="ficon ficon-zoomout"></span><br />
				            			<span class="data-num">
                                            <asp:Literal ID="Literal13" runat="server"></asp:Literal></span><br />
				            			普通会员数
				            		</a>
				            	</li>
				            	<li>
				            		<a href="user_table.aspx?Model_id=23&u1=&leixing=老师&shijian1=&shijian2=">
				            			<span class="ficon ficon-user"></span><br />
				            			<span class="data-num"><asp:Literal ID="Literal14" runat="server"></asp:Literal></span><br />
				            			老师数
				            		</a>
				            	</li>
				            	<li>
				            		<a href="user_table.aspx?Model_id=23&u1=&leixing=代理&shijian1=&shijian2=">
				            			<span class="ficon ficon-dingdan"></span><br />
				            			<span class="data-num"><asp:Literal ID="Literal15" runat="server"></asp:Literal></span><br />
				            			代理商数
				            		</a>
				            	</li>
				            	<li>
				            		<a href="articles.aspx?Model_id=82&classid=37">
				            			<span class="ficon ficon-shop"></span><br />
				            			<span class="data-num"><asp:Literal ID="Literal16" runat="server"></asp:Literal></span><br />
				            			课程数
				            		</a>
				            	</li>
				            	<li>
				            		<a href="articles.aspx?Model_id=88&classid=55">
				            			<span class="ficon ficon-diqiu"></span><br />
				            			<span class="data-num"><asp:Literal ID="Literal17" runat="server"></asp:Literal></span><br />
				            			课件数
				            		</a>
				            	</li>
				            	<li>
				            		<a href="order_table.aspx?Model_id=86">
				            			<span class="ficon ficon-wait"></span><br />
				            			<span class="data-num"><asp:Literal ID="Literal18" runat="server"></asp:Literal></span><br />
				            			课程订单数
				            		</a>
				            	</li>
				            </ul>
                         <p class="clear"></p>
				    </div>
				</div>
			</section>
			<!--数据分析 end -->
			<!--数据块-->
			<section class="hmc-small">
				<!--运营状态-->
				
				<div class="home-modular">
					<div class="home-modular-border">
					
				    <div class="home-modular-header">
				        <span class="ficon ficon-yunying"></span> 运营状态
				    </div>
				    <div class="home-modular-contet">
				        <a href="#" class="btn btn-blue small fr">续 费</a>
				        <p>域名：<span class="word-break"><%=ConfigurationSettings.AppSettings["web_url"].ToString()%></span></p>
				        <p>产品运营时间</p>
				        <div class="progressbar_container progressbar-num-right" data-show="hidden">
				        	<span class="num">86.52%</span>
				            <div class="progress_bar">
			        		    <div class="progress" data-width="86.5"></div>
			        		</div>
			        		<p>2015-04-05 / 2016-04-05</p>
				        </div>
				    </div>
				
					</div>
				</div>
				<!--运营状态 end-->
				<!--空间状况-->
				<div class="home-modular">
					<div class="home-modular-border">
				    <div class="home-modular-header">
				        <span class="ficon ficon-withdraw"></span> 空间状况
				    </div>
				    <div class="home-modular-contet">
				        <a href="#" class="btn btn-blue small fr">扩 充</a>
				        <p>空间总容量：<span class="word-break">500MB</span></p>
				        <p>网站空间</p>
				        <div class="progressbar_container progressbar-num-right" data-show="hidden">
				        	<span class="num">81%</span>
				            <div class="progress_bar">
			        		    <div class="progress" data-width="81"></div>
			        		</div>
			        		<p>388.00 / 500 MB</p>
				        </div>
				    </div>
				    </div>
				</div>
				<!--空间状况 end-->
				<!--短信使用情况-->
				<div class="home-modular">
					<div class="home-modular-border">
				    <div class="home-modular-header">
				        <span class="ficon ficon-duanxin"></span> 短信使用情况
				    </div>
				    <div class="home-modular-contet">
				        <a href="#" class="btn btn-blue small fr">充 值</a>
				        <p>总短信数：<span class="word-break">500条</span></p>
				        <p>短信情况</p>
				        <div class="progressbar_container progressbar-num-right" data-show="hidden">
				        	<span class="num">32%</span>
				            <div class="progress_bar">
			        		    <div class="progress" data-width="32"></div>
			        		</div>
			        		<p>160 / 500 条</p>
				        </div>
				    </div>
				    </div>
				</div>
				<!--空间状况 end-->
				<!--联系我们-->
				<div class="home-modular ">
					<div class="home-modular-border">
				    <div class="home-modular-header">
				        <span class="ficon ficon-lianxi"></span> 联系我们
				    </div>
				    <div class="home-modular-contet">
				        <div class="kefu-img">
				            <img src="images/kefu.jpg"/>
				            <p>专属客服：李瑶</p>
				        </div>
				        <div class="contact-info">
				            <p><span class="ficon ficon-dianhua"></span> 028-85530019 </p>
				            <p><span class="ficon ficon-iconfont-qq"></span> <a href="http://wpa.qq.com/msgrd?v=3&uin=9024570&site=qq&menu=yes" target="_blank">QQ交流</a> </p>
				            <p><span class="ficon ficon-iconfont-baidu"></span> <a href="http://p.qiao.baidu.com//im/index?siteid=437192&ucid=2646382" target="_blank">在线交流</a> </p>
				        </div>
				    </div>
				    </div>
				</div>
				<!--联系我们 end-->
			</section>
			<!--数据块  end -->
			
			
			<uc2:botton ID="botton1" runat="server" />
		</section>
		<p class="clear"></p>
		<script type="text/javascript">
         var admin_id = "<%=my_b.k_cookie("admin_id")%>";
		    var admin_pwd = "<%=my_b.k_cookie("admin_pwd")%>";
		    if (admin_id == "cdsile" && admin_pwd == "01AC3D95A020811609CEEF9ED8336E2E")
		    {
		        layer.confirm('您的密码太简单了，请重新设置您的密码。', {
		            btn: ['确定', '取消'] //按钮
		        }, function () {
		            window.location.href = "get_password.aspx";
		        }, function () {
		            
		        });
		        //alert("您的密码太简单了，请重新设置您的密码。");
		    }
            //上面

		    $(function () {
		       
		        $('#container').highcharts({
		            
		            chart: {
		                type: 'area'
		            },
		            title: {
		                text: '最近10天发布的数据'
		            },
		            subtitle: {
		                text: ''
		            },
		            xAxis: {
		                categories: [<%=categories%>]
		            },
		            yAxis: {
		                title: {
		                    text: '数据统计 (条)'
		                },
		                min: 0
		            },
		            tooltip: {
		                valueSuffix: '条'
		            },
		            series: [<%=model%>]
		        });

		        });
		   
		</script>

		
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
