<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Shopadmin/ShopAdmin.Master" 
CodeBehind="DistributorPaymentNotify_url.aspx.cs" Inherits="Hidistro.UI.Web.Pay.DistributorPaymentNotify_url" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">	  
	  <div class="columnright">
		  <h2 class="spanR"></h2>
		  <div class="blank18 clearfix"></div>		  
		  <div class="border_backg">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="10%" valign="middle"></td>
              <td><span class="spanY"><asp:Literal ID="litMessage" runat="server"></asp:Literal></span></td>
            </tr>
          </table>
        </div>
	  </div>
	  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
