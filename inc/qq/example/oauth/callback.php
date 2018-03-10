<?php
require_once("../../API/qqConnectAPI.php");
$qc = new QC();
$qc->qq_callback();
$qc->get_openid();
header("Location: http://new.huayujy.com/inc/qq/example/user/get_user_info.php"); 
