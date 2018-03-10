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
<?php
//setCookie("dingdanhao",$_GET['dingdanhao']);
//setCookie("jine",$_GET['jine']);
//setCookie("bodystr",$_GET['bodystr']);
//setCookie("tablename",$_GET['tablename']);
//echo $_GET['dingdanhao'];
//echo "金额：".$_COOKIE["jine"]."元";
//echo $_COOKIE["bodystr"];
//echo $_GET['tablename'];
//echo urlencode('http://'.$_SERVER['HTTP_HOST'].$_SERVER['PHP_SELF'].'?'.$_SERVER['QUERY_STRING']);
//exit;
/**
 * Native（原生）支付-模式二-demo
 * ====================================================
 * 商户生成订单，先调用统一支付接口获取到code_url，
 * 此URL直接生成二维码，用户扫码后调起支付。
 * 
*/
	include_once("../WxPayPubHelper/WxPayPubHelper.php");

	//使用统一支付接口
	$unifiedOrder = new UnifiedOrder_pub();
	
	//设置统一支付接口参数
	//设置必填参数
	//appid已填,商户无需重复填写
	//mch_id已填,商户无需重复填写
	//noncestr已填,商户无需重复填写
	//spbill_create_ip已填,商户无需重复填写
	//sign已填,商户无需重复填写
	$unifiedOrder->setParameter("body",$_GET['bodystr']);//商品描述
	//自定义订单号，此处仅作举例
	$timeStamp = time();
	$out_trade_no = WxPayConf_pub::APPID."$timeStamp";
	$unifiedOrder->setParameter("out_trade_no",$_GET['dingdanhao']);//商户订单号 
	$unifiedOrder->setParameter("total_fee",$_GET['jine']);//总金额
	$unifiedOrder->setParameter("notify_url",WxPayConf_pub::NOTIFY_URL);//通知地址 
	$unifiedOrder->setParameter("trade_type","NATIVE");//交易类型
	//非必填参数，商户可根据实际情况选填
	//$unifiedOrder->setParameter("sub_mch_id","XXXX");//子商户号  
	//$unifiedOrder->setParameter("device_info","XXXX");//设备号 
	//$unifiedOrder->setParameter("attach","XXXX");//附加数据 
	//$unifiedOrder->setParameter("time_start","XXXX");//交易起始时间
	//$unifiedOrder->setParameter("time_expire","XXXX");//交易结束时间 
	//$unifiedOrder->setParameter("goods_tag","XXXX");//商品标记 
	//$unifiedOrder->setParameter("openid","XXXX");//用户标识
	//$unifiedOrder->setParameter("product_id","XXXX");//商品ID
	
	//获取统一支付接口结果
	$unifiedOrderResult = $unifiedOrder->getResult();
	logger("unifiedOrderResult ".$unifiedOrderResult);
	
	
	//商户根据实际情况设置相应的处理流程
	if ($unifiedOrderResult["return_code"] == "FAIL") 
	{
		//商户自行增加处理流程
		echo "通信出错：".$unifiedOrderResult['return_msg']."<br>";
	}
	elseif($unifiedOrderResult["result_code"] == "FAIL")
	{
		//商户自行增加处理流程
		echo "错误代码：".$unifiedOrderResult['err_code']."<br>";
		echo "错误代码描述：".$unifiedOrderResult['err_code_des']."<br>";
	}
	elseif($unifiedOrderResult["code_url"] != NULL)
	{
		//从统一支付接口获取到code_url
		$code_url = $unifiedOrderResult["code_url"];
		
		logger("code_url ".$code_url);
		//商户自行增加处理流程
		//......
	}
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
        function str_insert2($str,$i,$substr){
            if(strlen($str) == 4){
                $i = 2;
            }
            elseif(strlen($str) == 5){
                $i = 3;
            }
            $start=substr($str,0,$i);
            $end=substr($str,$i);
            $str = ($start . $substr . $end);
            return $str;
        }
?>
	<div align="center" id="qrcode">
        <div id="content">
<!--        <p>订单号：--><?php //echo $_GET['dingdanhao']; ?><!--</p>-->
        </div>
	</div>
	
</body>
	<script src="./qrcode.js"></script>
	<script>
		if(<?php echo $unifiedOrderResult["code_url"] != NULL; ?>)
		{
			var url = "<?php echo $code_url;?>";
			//参数1表示图像大小，取值范围1-10；参数2表示质量，取值范围'L','M','Q','H'
			var qr = qrcode(10, 'M');
			qr.addData(url);
			qr.make();
			var wording=document.createElement('p');
			wording.innerHTML = "总金额为"+<?php echo str_insert2($_GET['jine'],'','.');?>;
			var code=document.createElement('DIV');
			code.innerHTML = qr.createImgTag();
			var element=document.getElementById("qrcode");
			element.appendChild(code);
//            element.appendChild(wording);
		}

		function chadan(){
			$.ajax({
					type: "GET",
					url: "/pay/web_wx.aspx?&tablename=<?php echo $_GET['tablename']; ?>&dingdanhao=<?php echo $_GET['dingdanhao']; ?>&url=<?php  echo urlencode('http://'.$_SERVER['HTTP_HOST'].$_SERVER['PHP_SELF'].'?'.$_SERVER['QUERY_STRING'])?>&suijishu="+ Math.random(),
					processData:true,
					success: function(data){
				//	alert(data);
						if (data=="已付款"){
							clearInterval(chd);
							window.location.href="/search.aspx?m=user_order";
						}
					}
				});
		}
		var chd=setInterval('chadan()',1000);
	</script>
</html>