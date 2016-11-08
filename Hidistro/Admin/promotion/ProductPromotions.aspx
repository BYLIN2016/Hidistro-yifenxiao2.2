<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductPromotions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductPromotions" Title="无标题页" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
        <h1><asp:Literal runat="server" ID="litTite" Text="商品促销活动" /> </h1>
        <span><asp:Literal runat="server" ID="litDec" Text="针对部分商品的打折优惠赠送等促销，您可以添加新的促销活动或管理当前的促销活动" /></span>
    </div>
    <div class="btn">
        <asp:HyperLink runat="server" NavigateUrl="AddProductPromotion.aspx" ID="hlinkAddPromotion" Text="添加新的商品促销" CssClass="submit_jia" />
    </div>
    <div class="datalist">    
        <div>
            <UI:Grid ID="grdPromoteSales" runat="server" AutoGenerateColumns="false"  DataKeyNames="ActivityId" Width="100%" GridLines="None" HeaderStyle-CssClass="table_title" >
                    <Columns>
                       
                        <asp:TemplateField HeaderText="活动名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left">
                        <itemstyle  CssClass="Name" />
                          <ItemTemplate>
                           <asp:Label ID="lblPromteSalesName" Text='<%#Eval("Name") %>' runat="server"></asp:Label>                           
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="活动类型" SortExpression="PromoteType" HeaderStyle-CssClass="td_right td_left">
                          <ItemTemplate>
                            <asp:Label ID="lblPromoteType" runat="server"></asp:Label>
                            <asp:Literal ID="ltrPromotionInfo" runat="server"></asp:Literal>
                          </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="适合的会员等级" SortExpression="Name" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                          <ItemTemplate>
                           <asp:Label ID="lblmemberGrades" Text="" runat="server"></asp:Label>
                          </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="开始时间" DataField="StartDate" HeaderStyle-Width="12%" />
                        <asp:BoundField HeaderText="结束时间" DataField="EndDate" HeaderStyle-Width="12%" />
                        <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                            <itemstyle width="170"  />
                            <itemtemplate>  
                                 <span class="submit_chakan"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/EditProductPromotion.aspx?ActivityId={0}", Eval("ActivityId")))%>' >编辑</a></span>
                                <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" runat="server"  IsShow="true" CommandName="Delete" Text="删除" /></span>      
                                <span class="submit_chakan"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/SetPromotionProducts.aspx?ActivityId={0}&isWholesale={1}", Eval("ActivityId"),isWholesale))%>' >促销商品</a></span>                         
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>                    
                </UI:Grid >
	        </div>
	  </div>  
</div>
</asp:Content>
