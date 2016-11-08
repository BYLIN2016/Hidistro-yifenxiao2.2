<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyHelpCategories.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyHelpCategories" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
            <li class="optionstar"><a href="MyHelpList.aspx" class="optionnext"><span>帮助管理</span></a></li>
            <li class="menucurrent"><a href="MyHelpCategories.aspx" class="optioncurrentend"><span class="optioncenter">帮助分类管理</span></a></li>
		</ul>
	</div>
	<!--选项卡-->

	<div class="dataarea mainwidth">
		<!--搜索-->
		
		 <div class="Pa_15">
             <a href="AddMyHelpCategory.aspx" class="submit_jia">添加帮助分类</a>
			
		</div>

		
		<!--数据列表区域-->
	  <div class="datalist">
     <UI:Grid ID="grdHelpCategories" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="CategoryId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                         
                
                <asp:TemplateField HeaderText="分类名称"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                         <Hi:HiImage ID="HiImage1" runat="server" DataField="IconUrl"  CssClass="Img100_30"/>
                      <asp:Literal id="lblCategoryName" runat="server" Text='<%#Eval("Name") %>'></asp:Literal>
                      <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                   </ItemTemplate>
                </asp:TemplateField>
                
                <UI:SortImageColumn  HeaderText="分类排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                
                <UI:YesNoImageColumn DataField="IsShowFooter" HeaderText="显示在底部帮助" HeaderStyle-CssClass="td_right td_left"/>
                
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                    <ItemTemplate>
                    <span class="submit_bianji"><a href='<%# "EditMyHelpCategory.aspx?CategoryId=" + Eval("CategoryId")%> '>编辑</a></span>
                    <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" Text="删除" IsShow="true"  CommandName="Delete" runat="server" /></span></span>                       
                       
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
	  

	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
     

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>


