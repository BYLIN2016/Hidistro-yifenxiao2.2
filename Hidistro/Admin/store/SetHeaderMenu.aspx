<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetHeaderMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetHeaderMenu" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/01.gif" width="32" height="32" /></em>
  <h1>“<a href="ManageThemes.aspx"><asp:Literal runat="server" ID="litThemName"></asp:Literal></a>”模板的页头菜单</h1>
  <span>当前正在使用的模板中，页头菜单由商品分类及列表中的单个菜单组成，你可以控制商品分类的显示数量（不显示则设置数量为0）及增删列表中的单个菜单来控制页头菜单栏目</span>
</div>
		<!--数据列表区域-->
		<div class="datalist">
        <div class="searcharea clearfix br_search">
			<ul>
                <li><asp:HyperLink ID="hlinkAddHeaderMenu" CssClass="submit_jia" Text="添加页头菜单" runat="server" /></li>
                <li>商品分类显示个数 <asp:TextBox ID="txtCategoryNum" runat="server" Width="60px" /> <asp:Button runat="server" ID="btnSave" Text="保存" />（你可以修改商品分类的显示个数及列表中菜单的个数来控制菜单栏目的显示）</li>
                <li><span style="width:95px;height:25px; background:url(../images/icon.gif) no-repeat -100px -87px;  padding-left:15px; line-height:25px;">　
                    <asp:LinkButton ID="lbtnSaveSequence" runat="server">批量保存顺序</asp:LinkButton></span></li>
            </ul>            
		</div>
        
		 <UI:Grid ID="grdHeaderMenu" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="Id" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title" >                                                        
            <Columns>   
                <asp:TemplateField HeaderText="显示顺序" HeaderStyle-CssClass="td_right td_left"> 
                    <ItemTemplate>
                        <asp:TextBox ID="txtDisplaySequence" runat="server" Width="60px" Text='<%# Eval("DisplaySequence")%>' />		                            
                    </ItemTemplate>                                                         
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="菜单名称" HeaderStyle-CssClass="td_right td_left"> 
                    <ItemTemplate>
		                    <%# Eval("Title")%>
                    </ItemTemplate>                                                         
                </asp:TemplateField>          
                <asp:TemplateField HeaderText="菜单类别" HeaderStyle-CssClass="td_right td_left"> 
                    <ItemTemplate>
		                <asp:Literal runat="server" ID="litCategory" Text='<%# Eval("Category")%>' />
                    </ItemTemplate>                                                         
                </asp:TemplateField>          
               <UI:YesNoImageColumn DataField="Visible"  ItemStyle-Width="8%" HeaderText="是否显示" HeaderStyle-CssClass="td_right td_left" />                                             
            <asp:TemplateField HeaderText="操作" ItemStyle-Width="17%" HeaderStyle-CssClass="td_left td_right_fff">
                <ItemTemplate>
                <span class="submit_bianji"><asp:HyperLink ID="lkbEdit" runat="server" Text="编辑" /></span>			                       
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
