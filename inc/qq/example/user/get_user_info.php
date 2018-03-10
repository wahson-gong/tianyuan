<?php

/*
 *调用接口代码
 *
 **/
require_once("../../API/qqConnectAPI.php");
$qc = new QC();
$arr = $qc->get_user_info();


echo '<meta charset="UTF-8">';



?>
<script type="text/javascript" charset="gb2312">
  var url = "/inc/set_login.aspx?nickname=" + escape('<?php echo $arr['nickname']?>') + "&figureurl=" + escape('<?php echo $arr['figureurl_2']?>') + "&xingbie=" + escape('<?php echo $arr['gender']?>') + "&suozaidi=" + escape('<?php echo $arr['province']?>-<?php echo $arr['city']?>') + "&type=qq&sid=" + Math.random();
  window.location.href = url;
  window.event.returnValue = false;
</script>