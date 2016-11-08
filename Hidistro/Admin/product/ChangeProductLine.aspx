<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ChangeProductLine" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title"> 
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>产品线批量替换</h1>
            <span>将某一产品线的商品批量替换到另一个产品线中</span></div>
        <div class="formitem">
        <table style="text-align:center;font-size:14px;">
            <tr><td align="right"><em >*</em>需要替换的产品线：</td><td> <Hi:ProductLineDropDownList ID="dropProductLineFrom" NullToDisplay="--请选择--"  runat="server"   /></td></tr>
            <tr><td width="1" colspan="2"> </td></tr>
            <tr><td align="right"><em >*</em>替换至：</td><td> <Hi:ProductLineDropDownList ID="dropProductLineFromTo" NullToDisplay="--请选择--"  runat="server"  /></td></tr>
            <tr><td colspan="2"><asp:Button ID="btnSaveCategory" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding" /></td></tr>
        </table>
       </div>  
  </div>
</div>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
