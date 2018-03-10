<%@ Page Language="C#" AutoEventWireup="true" CodeFile="send.aspx.cs" Inherits="inc_paypal_send" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onLoad="javascript:document.myForm.submit()">

<%--<form id="myForm" name="myForm" action="https://www.paypal.com/cgi-bin/webscr" method="post">--%>
<form id="Form1" name="myForm" action="https://www.sandbox.paypal.com/cgi-bin/webscr" method="post">
<input type="hidden" name="cmd" value="_xclick" /><br />
<input type="hidden" name="business" value="<%=business %>" /><br />
商品名称：
<input type="text" name="item_name" value="<%=item_name %>"/><br />
商品编号：
<input type="text" name="item_number"  value="<%=item_number %>"/><br />
<%--使用贝宝
<input type="hidden" name="currency_code" value="CNY" />--%>
使用国际paypal

<input type="hidden" name="currency" value="USD" />
<input type="hidden" name="no_shippin" value="1" />
    <br />
<input type="hidden" name="custom" value="qml" /><br />
<input type="Hidden" name="notify_url"  value="<%=notify_url %>" />
<input type="hidden" name="return"  value="<%=return_url %>" /><br />
<input type="hidden" name="charset" value="gb2312" />
    <br />
金额：
<input type="text" name="amount" value="<%=amount %>"/>
    <br />
<input type="submit" value="确定"/>
</form>
</body>
</html>
