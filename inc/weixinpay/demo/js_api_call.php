<?php
setCookie("dingdanhao",$_GET['dingdanhao'],time()+3600);
setCookie("jine",$_GET['jine'],time()+3600);
setCookie("tablename",$_GET['tablename'],time()+3600);

//echo "订单号1：".$_COOKIE["dingdanhao"];
//echo "表名：".$_COOKIE["jine"];
//echo "金额：".$_COOKIE["tablename"]."元";


//echo "订单号：".$_GET['dingdanhao'];
//echo "表名：".$_GET['tablename'];
//echo "金额：".$_GET['jine']."元";
//exit();
/**
 * JS_API支付demo
 * ====================================================
 * 在微信浏览器里面打开H5网页中执行JS调起支付。接口输入输出数据格式为JSON。
 * 成功调起支付需要三个步骤：
 * 步骤1：网页授权获取用户openid
 * 步骤2：使用统一支付接口，获取prepay_id
 * 步骤3：使用jsapi调起支付
*/
	include_once("../WxPayPubHelper/WxPayPubHelper.php");
	
	//使用jsapi接口
	$jsApi = new JsApi_pub();

	//=========步骤1：网页授权获取用户openid============
	//通过code获得openid
	if (!isset($_GET['code']))
	{
		//触发微信返回code码
		$url = $jsApi->createOauthUrlForCode(WxPayConf_pub::JS_API_CALL_URL);
		Header("Location: $url"); 
	}else
	{
		//获取code码，以获取openid
	    $code = $_GET['code'];
		$jsApi->setCode($code);
		$openid = $jsApi->getOpenId();
	}

	//=========步骤2：使用统一支付接口，获取prepay_id============
	//使用统一支付接口
	$unifiedOrder = new UnifiedOrder_pub();
	
	//设置统一支付接口参数
	//设置必填参数
	//appid已填,商户无需重复填写
	//mch_id已填,商户无需重复填写
	//noncestr已填,商户无需重复填写
	//spbill_create_ip已填,商户无需重复填写
	//sign已填,商户无需重复填写


	$unifiedOrder->setParameter("openid","$openid");//商品描述
	$unifiedOrder->setParameter("body","美图商贸，订单号".$_COOKIE["dingdanhao"]);//商品描述

	//自定义订单号，此处仅作举例
	$timeStamp = time();
	$out_trade_no = WxPayConf_pub::APPID."$timeStamp";
	//$unifiedOrder->setParameter("out_trade_no",$_COOKIE["dingdanhao"]);//商户订单号 
	$unifiedOrder->setParameter("out_trade_no",$_COOKIE["dingdanhao"]);//商户订单号 
	$unifiedOrder->setParameter("total_fee",$_COOKIE["jine"]);//总金额
	$unifiedOrder->setParameter("notify_url",WxPayConf_pub::NOTIFY_URL);//通知地址 
		
	$unifiedOrder->setParameter("trade_type","JSAPI");//交易类型
	//非必填参数，商户可根据实际情况选填
	//$unifiedOrder->setParameter("sub_mch_id","XXXX");//子商户号  
	//$unifiedOrder->setParameter("device_info","XXXX");//设备号 
	//$unifiedOrder->setParameter("attach","XXXX");//附加数据 
	//$unifiedOrder->setParameter("time_start","XXXX");//交易起始时间
	//$unifiedOrder->setParameter("time_expire","XXXX");//交易结束时间 
	//$unifiedOrder->setParameter("goods_tag","XXXX");//商品标记 
	//$unifiedOrder->setParameter("openid","XXXX");//用户标识
	//$unifiedOrder->setParameter("product_id","XXXX");//商品ID

	$prepay_id = $unifiedOrder->getPrepayId();
	logger("prepay_id ".$prepay_id);
	//=========步骤3：使用jsapi调起支付============
	$jsApi->setPrepayId($prepay_id);

	$jsApiParameters = $jsApi->getParameters();
	logger("jsApiParameters ".$jsApiParameters);
	//echo $jsApiParameters;
	//echo $jsApiParameters;
	
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

<!DOCTYPE html>
<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content=" width=device-width,height=device-height,initial-scale=1,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title>微信安全支付</title>
	<script type="text/javascript" src="/inc/jquery1.42.min.js"></script>
	<script type="text/javascript">

		//调用微信JS api 支付
		function jsApiCall()
		{
			WeixinJSBridge.invoke(
				'getBrandWCPayRequest',
				<?php echo $jsApiParameters; ?>,
				function(res){
					WeixinJSBridge.log(res.err_msg);
					//alert(res.err_code+res.err_desc+res.err_msg);
				}
			);
		}
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

		function callpay()
		{
			if (typeof WeixinJSBridge == "undefined"){
			    if( document.addEventListener ){
			        document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
			    }else if (document.attachEvent){
			        document.attachEvent('WeixinJSBridgeReady', jsApiCall); 
			        document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
			    }
			}else{
			//	alert("ok");
				
				
			    jsApiCall();
			}
			
			
		}
		callpay();
	</script>
</head>
<body>

 
</body>
</html>