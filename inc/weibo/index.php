<?php
session_start();

include_once( 'config.php' );
include_once( 'saetv2.ex.class.php' );

$o = new SaeTOAuthV2( WB_AKEY , WB_SKEY );

$code_url = $o->getAuthorizeURL( WB_CALLBACK_URL );

?>


<?php  
$url = $code_url;  
echo "<script language='javascript' 
type='text/javascript'>";  
echo "window.location.href='$url'";  
echo "</script>";  
?> 