<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pic_Cut.aspx.cs" Inherits="admin_pic_Cut" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>裁剪图片</title>
    <link href="images/style.css" rel="stylesheet" type="text/css" />
     <LINK rel="stylesheet" type="text/css" href="css/default.css" />
    <script language="javascript" type="text/javascript" src="images/base.drag.js"></script>
<script src="images/image.cut.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <!--开始-->
 <div id="cut_div" style="border:2px solid #888888; width:720px; height:266px; overflow: hidden; position:relative; top:0px; left:0px; margin:4px; cursor:pointer;">
 <table style="border-collapse: collapse; z-index: 10; filter: alpha(opacity=75); position: relative; left: 0px; top: 0px; width: 720px;  height: 266px; opacity: 0.75;" cellspacing="0" cellpadding="0" border="0" unselectable="on">
 <tr>
   <td style="background: #cccccc; height:auto" colspan="3"></td>
 </tr>
 <tr style="height:<%=cc2 %>px"> 
   <td style="background: #cccccc; width: 300px;"></td>
   <td style="border: 1px solid #ffffff; width: <%=cc1 %>px; height: <%=cc2 %>px;"></td>
   <td style="background: #cccccc; width: 305px;"></td>
 </tr>
 <tr>
   <td style="background: #cccccc; height:auto;" colspan="3"></td>
 </tr>
 </table>
<asp:Image ID="cut_img" runat="server" style="position:relative; top:-400px; left:0px" /></div>
<table cellspacing="0" cellpadding="0">
  <tr>
    <td><img style="margin-top: 5px; cursor:pointer;"  src="images/_h.gif" alt="图片缩小" onmouseover="this.src='images/_c.gif'" onmouseout="this.src='images/_h.gif'" onclick="imageresize(false)" /></td>
    <td><img id="img_track" style="width: 700px; height: 18px; margin-top: 5px" src="images/track.gif" /></td>
    <td><img style="margin-top: 5px; cursor:pointer;" src="images/+h.gif" alt="图片放大"  onmouseover="this.src='images/+c.gif'" onmouseout="this.src='images/+h.gif'" onclick="imageresize(true)" /></td>
  </tr>
</table>
<img id="img_grip" style="position:absolute; z-index:100; left:-1000px; top:-1000px; cursor:pointer;" src="images/grip.gif" /> <div style="padding-top:5px; padding-left:300px;">


<input name="img_pos" id="img_pos" value="400,334" style="display:none" />
<input name="cut_pos" id="cut_pos" value=""  style="display:none"/>

<input type="submit" class="button" name="submit" runat="server" id="submit" value=" 确认裁剪并提交 " onserverclick="submit_ServerClick" style="width: 102px" size="" />
&nbsp;&nbsp;&nbsp;&nbsp;<br />
</div>


<!--开始-->  
    </div>
    </form>
</body>
</html>
