<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CnzzStatisticsSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CnzzStatisticsSet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>访问统计</h1>
      <span>访问统计内置了中国站长联盟（CNZZ）站长统计功能，您只需要点击开启统计功能即可开始使用专业强大的站长统计</span>
    </div>
   <div class="Tempimg" id="div_pan1" runat="server">
      <table width="98%" border="0" cellspacing="0">
        <tr>
          <td width="25%" rowspan="4"></td>
          <td width="2%" rowspan="4">&nbsp;</td>
          <td width="19%">
           <asp:LinkButton ID="hlinkCreate" CssClass="submit_jia" Text="点击创建统计账号" runat="server"></asp:LinkButton>
          </td>
          <td width="54%" rowspan="4" align="right"></td>
        </tr>
        <tr>
          <td><span class="colorG"><strong><asp:Literal ID="litThemeName" runat="server" /></strong></span></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
        </tr>
      </table>
    </div>
    <div class="blank12 clearfix"></div>
    <div class="datafrom" id="div_pan2" runat="server">
        <div class="Template">
        <h1>开启或关闭统计</h1>
        <p style="margin-left:50px;">您的帐号已经创建，可以通过点击开启或关闭按钮对统计功能进行开启和关闭，如果开启动就会在前台进行统计，可以通过查看统计的链接查看统计结果。</p>
        <p style="margin-left:50px;">如果您是第一次创建统计账号，请点击开启统计功能，这样统计功能即可开始使用。</p>
          <p style="width:400px; margin-left:50px; margin-top:10px;"> <asp:LinkButton ID="hplinkSet" CssClass="submit_jia" Text="开启统计功能" runat="server"></asp:LinkButton></p>
         </div>
     </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
