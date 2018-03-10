<%@ Page Language="C#" AutoEventWireup="true" CodeFile="liandong_table1.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>联动</title>
    <style type="text/css">
select {
    width: 173px;
    height: 38px;
    font-size: 13px;
    font-family: 微软雅黑;
    line-height: 26px;
    margin-bottom: 10px;
    color: rgb(89, 89, 89);
    -webkit-appearance: none;
    outline: none;
    padding: 6px 14px 6px 10px;
    border-width: 1px;
    border-style: solid;
    border-color: rgb(232, 232, 232);
    border-image: initial;
    background: url(/template/default/img/down.png) right center no-repeat scroll transparent;
}
    </style>
   
</head>
<body>
<form id="form1" runat="server">
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" Visible="false" CssClass="select-container2"  onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" Visible="false" CssClass="select-container2" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged" Visible="false" CssClass="select-container2" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList4_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList5" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList5_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList6_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList7" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList7_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList8" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList8_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList9" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList9_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList10" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList10_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList11" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList11_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList12" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList12_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList13" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList13_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList14" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList14_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList15" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList15_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList16" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList16_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList17" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList17_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList18" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList18_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList19" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList19_SelectedIndexChanged" Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
     <asp:DropDownList ID="DropDownList20" runat="server"  Visible="false" onchange="change(this.value)">
    </asp:DropDownList>
    <script type="text/javascript">
        function change(s)
        {
            window.parent.document.getElementById('<%=Request.QueryString["textBox"].ToString()%>').value = s.replace("|","");
        }
    </script>
</form>
</body>
</html>