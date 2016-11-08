<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderGifts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.OrderGifts"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="Goodsgifts">
    <div class="title title_height"> <em><img src="../images/05.gif" width="32" height="32" /></em>
    <h1 class="title_line"><asp:Literal runat="server" ID="litPageTitle" Text="添加订单礼品" /></h1>
    <span><asp:Literal runat="server" ID="litPageNote" Text="添加赠送给买家的礼品." /></span>
  </div>
    <div class="left">
      <h1>请从下列礼品中选择</h1>
      <ul>
      	<li><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></li>
		<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
      </ul>
      <div class="content">
      <div class="youhuiproductlist">
        <asp:DataList runat="server" ID="dlstGifts" Width="100%" DataKeyField="GiftId" RepeatLayout="Table">
                <ItemTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="conlisttd">
                    
                     <tr>
            <td width="14%" rowspan="2" class="img"><Hi:HiImage ID="HiImage2"  runat="server" DataField="ThumbnailUrl40"/>  </td>
            <td height="27" colspan="4"  class="br_none"><span class="Name"><asp:Literal runat="server" ID="litGiftName" Text='<%# Eval("Name") %>'></asp:Literal></span></td>
          </tr>
          <tr>
            <td width="27%" height="28" valign="top"><span class="colorC">成本价：<Hi:FormatedMoneyLabel ID="lblPrice" runat="server" Money='<%# Eval("CostPrice")%>'  /></span></td>
            <td width="19%" valign="top"> </td>
            <td width="14%" align="left" valign="top" class="a_none"><asp:TextBox ID="txtQuantity"  runat="server" Width="30px" Text="1" ></asp:TextBox></td>
            <td width="15%" valign="top"><span class="submit_tianjia"><asp:LinkButton ID="btnCheck" runat="server" CommandName="check"  Text="添加" /></span></td>
          </tr>
                   </table>
                </ItemTemplate>
            </asp:DataList>         
        <div class="r">
       
         <div class="pagination" style="margin-top:10px;">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
				</div></div>
      </div>
    </div>
    </div>
    <div class="right">
      <h1>已添加的礼品</h1>
      <ul>
		 <li><asp:Button runat="server" ID="btnClear" CssClass="submit_queding" Text="清空列表" /></li>
      </ul>
       <div class="content">
       <div class="youhuiproductlist">
         <asp:DataList runat="server" ID="dlstOrderGifts" Width="100%" DataKeyField="GiftId" RepeatLayout="Table">
                <ItemTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="conlisttd">                
                     <tr>
             <td width="14%"  rowspan="2" class="img" ><Hi:HiImage ID="HiImage1"  runat="server" DataField="ThumbnailsUrl"/>  </td>
             <td height="27" colspan="4"  class="br_none"><span class="Name"><asp:Literal runat="server" ID="litGiftName" Text='<%# Eval("GiftName") %>'></asp:Literal></span></td>
           </tr>
           <tr>
             <td width="27%" height="28" valign="top"><span class="colorC">成本价：<Hi:FormatedMoneyLabel ID="lblPrice" runat="server" Money='<%# Eval("CostPrice")%>'  /></span></td>
             <td width="27%" valign="top">赠送数量：<asp:Label ID="lblQuantity" Text='<%# Eval("Quantity") %>' runat="server" Width="30px" ></asp:Label></td>
             <td width="15%" align="left" valign="top">&nbsp;</td>
             <td width="15%" align="left" valign="top" class="a_none"><span class="submit_shanchu"><asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete"  Text="删除" /></span></td>
           </tr>
           
                   </table>
                </ItemTemplate>
            </asp:DataList>       
         <div class="r">
         <div> &nbsp;</div>
         
         <div class="pagination" style="margin-top:10px;">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pagerOrderGifts" PageIndexFormat="pageindex1" />
				</div>

         </div>         </div>
      </div>
      </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
