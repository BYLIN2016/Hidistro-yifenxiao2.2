<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="FriendlyLinks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.FriendlyLinks" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>友情链接管理</h1>
    <span>管理店铺的所有友情链接，您可以添加、修改或删除友情链接</span></div>
    <!-- 添加按钮-->

    <!--结束-->
    <div class="datalist">
		<div class="searcharea clearfix br_search">
			<ul><li><a href="AddFriendlyLink.aspx" class="submit_jia">添加友情链接</a></li></ul>
		</div>
     <UI:Grid ID="grdGroupList" runat="server" AutoGenerateColumns="false"  ShowHeader="true" DataKeyNames="LinkId" GridLines="None" HeaderStyle-CssClass="table_title" Width="100%">
                        <Columns>                                                        
                            <asp:TemplateField HeaderText="友情链接Logo" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>&nbsp;
                                     <Hi:HiImage ID="HiImage1"  runat="server" DataField="ImageUrl" CssClass="Img100_30" />
                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="网站名称" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>
		                            <asp:Literal ID="liblTitle" Text='<%#Eval("Title") %>' runat="server"></asp:Literal>
		                            <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="网站地址" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>
                                   <div><asp:Literal ID="liblLinkUrl" Text='<%#Eval("LinkUrl") %>' runat="server"></asp:Literal></div>
                               </ItemTemplate>
                            </asp:TemplateField>                            
                            <UI:SortImageColumn  HeaderText="排列顺序" ItemStyle-Width="10%" ReadOnly="true" HeaderStyle-CssClass="td_right td_left" />                            
                            <UI:YesNoImageColumn DataField="Visible"  ItemStyle-Width="8%" HeaderText="是否显示" HeaderStyle-CssClass="td_right td_left" />
                             <asp:TemplateField HeaderText="操作" ItemStyle-Width="17%" HeaderStyle-CssClass="td_left td_right_fff">
                                 <ItemTemplate>
                                 <span class="submit_bianji"><a href='<%# "EditFriendlyLink.aspx?linkId="+Eval("LinkId") %>''>编辑</a></span>			                       
			                        <span class="submit_shanchu"><Hi:ImageLinkButton  ID="lkDelete"  IsShow="true"  Text="删除" CommandName="Delete" runat="server" /></span>
                                 </ItemTemplate>
                             </asp:TemplateField>
                        </Columns>
                </UI:Grid>
    </div>    
  </div>          
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>