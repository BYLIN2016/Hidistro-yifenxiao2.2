<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageTemples.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageTemples"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="title m_none td_bottom"> <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1>分销商站点模板管理</h1>
        <span>您可以在此指定该分销商站点可以使用的模板，未选中的模板该分销商站点无法使用</span></div>
	  <div class="datamsg">
        <div class="blank12 clearfix"></div>
	    <span class="fonts">当前分销商：<strong class="colorE"><asp:Literal runat="server" ID="litUserName" /></strong></span> <span  class="fonts">独立域名：<strong class="colorE"><asp:Literal runat="server" ID="litDomain"></asp:Literal></strong> </span>
        <div class="blank12 clearfix"></div>
      </div>
	  <div class="blank12 clearfix"></div>
	  <div class="datafrom">
        <div class="Template">
          <h1>模板列表</h1>
           <asp:DataList runat="server" ID="dtManageThemes" RepeatColumns="3"  RepeatDirection="Horizontal">
                <ItemTemplate>
                <ul><li>
                     <span> <Hi:DisplayThemesImages ID="themeImg" runat="server" Src='<%# Eval("ThemeImgUrl") %>' ThemeName='<%# Eval("ThemeName") %>' /></span>
                       <em>                                                                                       
                        <asp:CheckBox ID="rbCheckThemes" runat="server" Text='<%# Eval("ThemeName") %>'  /></em></li> 
                 </ul>
                </ItemTemplate>
            </asp:DataList>   
        
        </div>
	    <div class="page">
          <div class="bottomPageNumber clearfix">
            
          </div>
        </div>
	    <div class="btntf  Pg_45">
          <asp:Button ID="btnSave" runat="server"  Text="保 存"  CssClass="submit_DAqueding bnit"/>
        </div>
      </div>
</div>
<div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
