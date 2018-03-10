<?php
setCookie("dingdanhao",$_GET['dingdanhao'],time()+3600);
setCookie("jine",$_GET['jine'],time()+3600);
setCookie("tablename",$_GET['tablename'],time()+3600);
/**
 * Native（原生）支付模式一demo
 * ====================================================
 * 模式一：商户按固定格式生成链接二维码，用户扫码后调微信
 * 会将productid和用户openid发送到商户设置的链接上，商户收到
 * 请求生成订单，调用统一支付接口下单提交到微信，微信会返回
 * 给商户prepayid。
 * 本例程对应的二维码由native_call_qrcode.php生成；
 * 本例程对应的响应服务为native_call.php；
 * 需要两者配合使用。
*/
	include_once("../WxPayPubHelper/WxPayPubHelper.php");

	//设置静态链接
	$nativeLink = new NativeLink_pub();	
	
	//设置静态链接参数
	//设置必填参数
	//appid已填,商户无需重复填写
	//mch_id已填,商户无需重复填写
	//noncestr已填,商户无需重复填写
	//time_stamp已填,商户无需重复填写
	//sign已填,商户无需重复填写
	$product_id = WxPayConf_pub::APPID."static";//自定义商品id
	$nativeLink->setParameter("product_id","$product_id");//商品id
	//获取链接
	$product_url = $nativeLink->getUrl();
	logger("product_url ".$product_url);
	//使用短链接转换接口
	$shortUrl = new ShortUrl_pub();
	//设置必填参数
	//appid已填,商户无需重复填写
	//mch_id已填,商户无需重复填写
	//noncestr已填,商户无需重复填写
	//sign已填,商户无需重复填写
	$shortUrl->setParameter("long_url","$product_url");//URL链接
	//获取短链接
	$codeUrl = $shortUrl->getShortUrl();
	logger("codeUrl ".$codeUrl);
	
	
	function logger($log_content)
	{
		if(isset($_SERVER['HTTP_BAE_ENV_APPID'])){   //BAE
			require_once "BaeLog.class.php";
			$logger = BaeLog::getInstance();
			$logger ->logDebug($log_content);
		}else if (isset($_SERVER['HTTP_APPNAME'])){   //SAE
			sae_set_display_errors(false);
			sae_debug($log_content);
			sae_set_display_errors(true);
		}else {
			$max_size = 100000;
			$log_filename = "log.xml";
			if(file_exists($log_filename) and (abs(filesize($log_filename)) > $max_size)){unlink($log_filename);}
			file_put_contents($log_filename, date('H:i:s')." ".$log_content."\r\n", FILE_APPEND);
		}
	}
	
?>

<!DOCTYPE HTML>
<html>
<head>
	<meta charset="UTF-8">
	<title>微信安全支付</title>
		<script type="text/javascript" src="/inc/jquery1.42.min.js"></script>
		 <style>
            #qrcode{
                width: 760px;
                height: 500px;
                background: url("wechatbg.png") no-repeat;
                position: absolute;
                left: 50%;
                margin-left: -380px;
            }
            #content{
                margin-top: 227px;
            }
        </style>
</head>
<body>
	<div align="center" id="qrcode">
		<p >扫我，扫我</p>
	</div>
	<div align="center">
		<a href="../index.php">返回首页</a>
	</div>
</body>
	<script src="./qrcode.js"></script>
	<script>
		var url = "<?php echo $product_url;?>";
		//参数1表示图像大小，取值范围1-10；参数2表示质量，取值范围'L','M','Q','H'
		var qr = qrcode(10, 'M');
		qr.addData(url);
		qr.make();
		var dom=document.createElement('DIV');
		dom.innerHTML = qr.createImgTag();
		var element=document.getElementById("qrcode");
		element.appendChild(dom);

			function chadan(){
	

			$.ajax({
					type: "GET",
					url: "/single.aspx?m=chadan&danhao=<?php echo $_COOKIE["dingdanhao"]; ?>&tablename=<?php echo $_COOKIE['tablename'];?>",
					processData:true,
					success: function(data){
					//alert(data);
						if (data=="已付款"){
							clearInterval(chd);
							window.location.href="/err.aspx?err=恭喜，已经付款成功-即将进入个人中心。&errurl=/single.aspx?m=user_index";

							
							
						}
					}
				});
		}
		var chd=setInterval('chadan()',1000);
	</script>
</html>