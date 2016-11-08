<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyProductPromotions.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyProductPromotions" Title="无标题页" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
            <div class="dataarea mainwidth databody">
  <div class="title td_bottom m_none">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>商品促销</h1>
  <span>针对部分商品的打折优惠赠送等促销，您可以添加新的促销活动或管理当前的促销活动</span></div>
  
		<!-- 添加按钮-->
		  <div class="btn">
          <a href="AddMyProductPromotions.aspx" class="submit_jia">添加商品促销活动</a>
	    </div>
	<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
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
                                 <span class="submit_chakan"><a href='<%# "EditMyProductPromotions.aspx?ActivityId="+Eval("ActivityId") %>' >编辑</a></span>
                                <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" runat="server"  IsShow="true" CommandName="Delete" Text="删除" /></span>      
                                <span class="submit_chakan"><a href= '<%# "SetMyPromotionProducts.aspx?ActivityId="+Eval("ActivityId")%>' >促销商品</a></span>                         
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>                    
                </UI:Grid >
  
  </div>
	</div>
</asp:Content>
