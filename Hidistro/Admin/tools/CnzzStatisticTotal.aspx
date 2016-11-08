<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CnzzStatisticTotal.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CnzzStatisticTotal" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

 <%-- <Hi:HiControls ID="HiControls1" LinkURL="http://www.shopefx.com/zengzhi.html" Height="1500" runat="server"/>--%>
<%-- <div  class="areacolumn clearfix" style="width:980px;">--%>

  <div class="dataarea mainwidth databody">
    <div class="title"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>CNZZ站长统计</h1>
      <span>统计内置了中国站长联盟（CNZZ）站长统计功能，您可以使用专业强大的站长统计</span>
    </div>
</div>
<div class="Tempimg">
<iframe style="border:0px;background-color:Transparent; height:1500px;width:100%;" scrolling="no" allowTransparency="true" frameborder="0" id="framcnz" runat="server"></iframe>
</div>
<style>
.Tempimg { width:980px; margin:0 auto; overflow:hidden;}
</style> 

  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
          function gotoWeb(src)
          {
            window.open(src,"_blank")
          }
        </script>
</asp:Content>



