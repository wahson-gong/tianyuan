<%@ Control Language="C#" AutoEventWireup="true" CodeFile="bianjiqi.ascx.cs" Inherits="admin_ascx_bianjiqi" %>
<!-- 配置文件 -->
    <script type="text/javascript" src="ueditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="ueditor/ueditor.all.js"></script>
    <!-- 实例化编辑器 -->
    
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>

<script type="text/javascript">
    var ue = UE.getEditor('<%=TextBox1.ClientID%>');
    </script>
