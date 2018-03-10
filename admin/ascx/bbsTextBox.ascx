<%@ Control Language="C#" AutoEventWireup="true" CodeFile="bbsTextBox.ascx.cs" Inherits="ascx_FreeTextBox" %>
    <script type="text/javascript" src="ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="ueditor/ueditor.all.js"></script>
    
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>

<script type="text/javascript">
        var ue = UE.getEditor('<%=TextBox1.ClientID%>');
    </script>
