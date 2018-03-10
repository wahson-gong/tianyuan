<?php
session_start();

include_once( 'config.php' );
include_once( 'saetv2.ex.class.php' );

$o = new SaeTOAuthV2( WB_AKEY , WB_SKEY );

if (isset($_REQUEST['code'])) {
	$keys = array();
	$keys['code'] = $_REQUEST['code'];
	$keys['redirect_uri'] = WB_CALLBACK_URL;
	try {
		$token = $o->getAccessToken( 'code', $keys ) ;
	} catch (OAuthException $e) {
	}
}

if ($token) {
	$_SESSION['token'] = $token;
	setcookie( 'weibojs_'.$o->client_id, http_build_query($token) );
?>
<?php


$c = new SaeTClientV2( WB_AKEY , WB_SKEY , $_SESSION['token']['access_token'] );
$ms  = $c->home_timeline(); // done
$uid_get = $c->get_uid();
$uid = $uid_get['uid'];
$user_message = $c->show_user_by_id( $uid);//根据ID获取用户等基本信息


?>
<script type="text/javascript" charset="gb2312">
  var url = "/inc/set_login.aspx?id=" + escape('<?php echo $user_message['id']?>') + "&nickname=" + escape('<?php echo $user_message['screen_name']?>') + "&figureurl=" + escape('<?php echo $user_message['profile_image_url']?>') + "&xingbie=" + escape('<?php echo $user_message['gender']?>') + "&suozaidi=" + escape('<?php echo $user_message['location']?>') + "&type=weibo&sid=" + Math.random();
  window.location.href = url;
  window.event.returnValue = false;
</script>
<?php
} else {
?>
授权失败。
<?php
}
?>
