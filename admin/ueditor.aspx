<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ueditor.aspx.cs" Inherits="admin_ueditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <script type="text/javascript" src="ueditor/ueditor.config.js"></script>
        <script type="text/javascript" src="ueditor/ueditor.all.min.js"> </script>
        <script type="text/javascript" src="ueditor/ueditor.all.min.js"> </script>
        <script type="text/javascript"src="ueditor/lang/zh-cn/zh-cn.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <script id="container" name="content" type="text/plain">
    这里写你的初始化内容
</script>
<script type="text/javascript">
    var editor = UE.getEditor('container')
</script>
    </form>
</body>
</html>
