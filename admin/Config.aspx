<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Config.aspx.cs" Inherits="admin_Config" ValidateRequest="false" %>
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
		
		<title>添加参数</title>
		<style type="text/css">
			
		</style>
		<script src="color/color.js"></script>

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
            <div class="site-location">
				&nbsp;&nbsp;
			    <span class="ficon ficon-weizhi"></span>&nbsp;<a href="#">首页</a>
			    &nbsp;>&nbsp;<a href="#">文章管理</a>
			</div>
			<p class="silo_zw"></p>
			<!--当前位置 end-->
		
			<div class="peizhi-box">
			    <table class="peizhi-header">
			    	<tr>
			        <td class="on">网站参数</td>
			        <td>运费设置</td>
			        <td>页面参数</td>
			        <td>内容参数</td>
			        <td>邮件参数</td>
			        <td>短信参数</td>
			        <td>水印/缩放</td>
			        <td>添加数据</td>
			        <td>商城设置</td>
			        </tr>
			    </table>
			    <div class="peizhi-header-wap">
			        <div class="select-container" style="width: 280px;">  
				            <span class="selectOption gray">网站参数</span>  
				            <ul class="selectMenu">  
				                <li>网站参数</li>  
				                <li>运费设置</li>  
				                <li>页面参数</li>  
				                <li>内容参数</li>  
				                <li>邮件参数</li>  
				                <li>短信参数</li>  
				                <li>水印/缩放</li>  
				                <li>添加数据</li>  
				                <li>商城设置</li>  
				            </ul>  
				            <span class="shows ficon ficon-down-open"></span>  
	 					</div> 
			    </div>
				
				<div class="peizhi-content">
				    <table class="table table-info cs-table">
						<tr>
							<th width="10%">站点名称</th>
							<td>
								<asp:TextBox ID="TextBox1" runat="server" CssClass="input" autocomplete="off"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">子标题</th>
							<td>
								<asp:TextBox ID="TextBox2" runat="server" CssClass="input"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">网站关键字</th>
							<td>
								<asp:TextBox ID="TextBox3" runat="server" CssClass="txtinput"  TextMode="MultiLine"></asp:TextBox>
							</td>
						</tr>
                        <tr>
							<th width="10%">网站描述</th>
							<td>
								<asp:TextBox ID="TextBox4" runat="server" CssClass="txtinput"  TextMode="MultiLine"></asp:TextBox>
							</td>
						</tr>
						</table>
					<table class="table table-info cs-table">
						<tr>
							<th width="10%">运费</th>
							<td>
								<asp:TextBox ID="TextBox30" runat="server" CssClass="dinput"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">免运费设置</th>
							<td>
								<asp:TextBox ID="TextBox31" runat="server" CssClass="dinput"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
						</table>
					<table class="table table-info cs-table">
						<tr>
							<th width="10%">页面模式</th>
							<td>
								<asp:RadioButtonList ID="RadioButtonList4" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True">动态网站</asp:ListItem>
                <asp:ListItem>静态网站</asp:ListItem>
                <asp:ListItem>伪静态</asp:ListItem>
            </asp:RadioButtonList>
							</td>
						</tr>
                        <tr>
							<th width="10%">默认首页</th>
							<td>
								<asp:TextBox ID="TextBox29" runat="server" CssClass="dinput">/index.html</asp:TextBox>
							</td>
						</tr>
                          <tr>
							<th width="10%">详细页路径</th>
							<td>
								<asp:TextBox ID="TextBox32" runat="server" CssClass="dinput"></asp:TextBox><font color="red">* yyyyMMddHHmmss，分别代表年月日时分秒</font>
							</td>
						</tr>
                        </table>
                        <table class="table table-info cs-table">
                        <tr>
							<th width="10%">每次搜索间隔</th>
							<td>
								<asp:TextBox ID="TextBox5" runat="server" CssClass="dinput"  ></asp:TextBox>
							</td>
						</tr>
                        <tr>
							<th width="10%">文件上传设置（图片）</th>
							<td>
								<asp:TextBox ID="TextBox6" runat="server" CssClass="input" ></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">文件上传设置（视频）</th>
							<td>
								<asp:TextBox ID="TextBox7" runat="server" CssClass="input" ></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">文件上传设置（音频）</th>
							<td>
								<asp:TextBox ID="TextBox8" runat="server" CssClass="input"></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">文件上传设置（软件）</th>
							<td>
								<asp:TextBox ID="TextBox9" runat="server" CssClass="input" ></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">文件上传设置（其它）</th>
							<td>
								<asp:TextBox ID="TextBox10" runat="server" CssClass="input" ></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">评论过虑</th>
							<td>
								<asp:TextBox ID="TextBox11" runat="server"  CssClass="txtinput"  TextMode="MultiLine"></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">内容过虑</th>
							<td>
								<asp:TextBox ID="TextBox12" runat="server" CssClass="txtinput"  TextMode="MultiLine"></asp:TextBox><font color="red">* （用“|”间隔）</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">图片大小</th>
							<td>
								<asp:TextBox ID="TextBox13" runat="server" CssClass="dinput" ></asp:TextBox>
							</td>
						</tr>
                        <tr>
							<th width="10%">视频大小</th>
							<td>
								<asp:TextBox ID="TextBox14" runat="server" CssClass="dinput"></asp:TextBox>
							</td>
						</tr>
                        <tr>
							<th width="10%">软件大小</th>
							<td>
								<asp:TextBox ID="TextBox15" runat="server" CssClass="dinput"></asp:TextBox>
							</td>
						</tr>
						</table>	
                    <table class="table table-info cs-table">
						<tr>
							<th width="10%">邮件服务器</th>
							<td>
								<asp:TextBox ID="TextBox16" runat="server" CssClass="dinput" ></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">邮箱名</th>
							<td>
								<asp:TextBox ID="TextBox17" runat="server" CssClass="dinput"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">邮箱密码</th>
							<td>
								<asp:TextBox ID="TextBox18" runat="server" CssClass="dinput"></asp:TextBox><asp:Label ID="Label1" runat="server"></asp:Label><font color="red">* </font>
							</td>
						</tr>
                        <tr>
							<th width="10%">显示人</th>
							<td>
								<asp:TextBox ID="TextBox19" runat="server" CssClass="input"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
						</table>
                    <table class="table table-info cs-table">
						<tr>
							<th width="10%">短信帐号</th>
							<td>
								<asp:TextBox ID="TextBox20" runat="server" CssClass="dinput" ></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">短信密码</th>
							<td>
								<asp:TextBox ID="TextBox21" runat="server" CssClass="dinput"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                        <tr>
							<th width="10%">短信签名</th>
							<td>
								<asp:TextBox ID="TextBox22" runat="server" CssClass="dinput"></asp:TextBox><font color="red">*</font>
							</td>
						</tr>
                     
						</table>
                    <table class="table table-info cs-table">
						<tr>
							<th width="10%">水印类型</th>
							<td>
								<asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem>文字水印</asp:ListItem>
                <asp:ListItem>图片水印</asp:ListItem>
                                        <asp:ListItem>无水印</asp:ListItem>
                            </asp:RadioButtonList>
							</td>
						</tr>
                        <tr>
							<th width="10%">文字水印设置</th>
							<td>
								<table border="0" cellpadding="0" cellspacing="0" style="width: 100%; line-height:24px">
                                <tr>
                                    <td style="width: 120px">
                                    水印的文字内容为：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox25" runat="server" MaxLength="200" CssClass="input"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">
                                    水印文字的大小为：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox26" runat="server" MaxLength="200"  CssClass="dinput"></asp:TextBox>PX</td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">
                                    水印文字的字体为：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="select-container1">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">
                                    水印文字的颜色为：
                                    </td>
                                    <td> <asp:TextBox ID="TextBox33" runat="server" MaxLength="200"  CssClass="dinput" onClick="Jcolor(this).color();"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">
                                    粗体：
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="粗体" /></td>
                                </tr>
                            </table>
							</td>
						</tr>
                        <tr>
							<th width="10%">图片水印设置</th>
							<td>
								<table border="0" cellpadding="0" cellspacing="0" style="width: 100%; line-height:24px">
                                <tr>
                                    <td style="width: 140px">
                                    用作水印的图片路径为：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox27" runat="server" MaxLength="200" CssClass="dinput"></asp:TextBox>&nbsp;
                                        <a href="javascript:window.open('/inc/webuploader/FileUploader.aspx?type=img&editname=TextBox27','','status=no,scrollbars=no,top=20,left=110,width=435,height=400');" class="btn btn-green"> <em class="ficon ficon-photo"></em> 我要上传图片</a>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 140px">
                                    水印图片透明度：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox28" runat="server" MaxLength="200"  CssClass="dinput"></asp:TextBox>&nbsp;0.1~1.0</td>
                                </tr>
                                <tr>
                                    <td style="width: 140px">
                                    水印位置：
                                    </td>
                                    <td><asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>左上</asp:ListItem>
                                        <asp:ListItem>中上</asp:ListItem>
                                        <asp:ListItem>右上</asp:ListItem>
                                        <asp:ListItem>中左</asp:ListItem>
                                        <asp:ListItem>居中</asp:ListItem>
                                        <asp:ListItem>中右</asp:ListItem>
                                        <asp:ListItem>左下</asp:ListItem>
                                        <asp:ListItem>下中</asp:ListItem>
                                        <asp:ListItem>右下</asp:ListItem>
                                    </asp:RadioButtonList></td>
                                </tr>

                            </table>
							</td>
						</tr>
                     
						</table>
                    <table class="table table-info cs-table">
						<tr>
							<th width="10%">编辑器</th>
							<td>
								<asp:CheckBoxList ID="CheckBoxList1" runat="server" 
                RepeatDirection="Horizontal">
                <asp:ListItem>远程抓图</asp:ListItem>
                <asp:ListItem>自动改变图片大小</asp:ListItem>
                <asp:ListItem>设置第一张为缩略图</asp:ListItem>
            </asp:CheckBoxList>
							</td>
						</tr>
                     
                     
						</table>
                    <table class="table table-info cs-table">
						<tr>
							<th width="10%">积分比例</th>
							<td>
								<asp:TextBox ID="TextBox23" runat="server" MaxLength="200" CssClass="dinput"></asp:TextBox>分等于一元。 例：100分换1元，就输入100。
							</td>
						</tr>
                     <tr>
							<th width="10%">通知邮箱</th>
							<td>
								<asp:TextBox ID="TextBox24" runat="server" MaxLength="200" CssClass="dinput"></asp:TextBox>
							</td>
						</tr>
                     
						</table>

                    <br />
						  <div class="center">
                              <asp:LinkButton ID="LinkButton1" CssClass="btn btn-blue" runat="server" OnClick="LinkButton1_Click"><em class="ficon ficon-queren"></em> 确认操作</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="LinkButton2" CssClass="btn btn-green" runat="server" OnClick="LinkButton2_Click"><em class="ficon ficon-shanchu"></em> 清空重填</asp:LinkButton>
						  </div>
				</div>
			</div>
			
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
        <script>
            $(".cs-table").hide().eq(0).show();
            $(".peizhi-header td,.selectMenu li").click(function () {
                var i = $(this).index();
                $(this).addClass("on").siblings().removeClass("on");
                $(".selectOption").html($(this).html());
                $(".cs-table").eq(i).fadeIn().siblings(".cs-table").hide();
            })
        </script>
	</body>
</html>
