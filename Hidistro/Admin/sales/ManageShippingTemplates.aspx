<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageShippingTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.ManageShippingTemplates" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
       <div class="title">
		  <em><img src="../images/03.gif" width="32" height="32" /></em>
          <h1><strong>运费模板</strong></h1>
          <span>配送地区需要按照国家的行政区域逐级划分，在列表里面点击编辑可以转到此运费模板的运费价格设置</span>
		</div>
	<div class="datalist">
           <div class="functionHandleArea clearfix">
			<!--分页功能-->
		   <div class="pageHandleArea">
				<ul>
					<li><a href="AddShippingTemplate.aspx" class="submit_jia">添加新运费模板</a></li>
				</ul>
			</div>
			<!--结束-->
	  </div>
	<div>
	 <UI:Grid ID="grdShippingTemplates" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="TemplateId" GridLines="None" Width="100%" HeaderStyle-CssClass="border_background">
          <HeaderStyle CssClass="table_title" />
            <Columns>   
                    <asp:TemplateField HeaderText="方式名称"  HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                           <asp:Label ID="lblShippingModesName" Text='<%#Eval("TemplateName") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="首重" DataField="Weight" HeaderStyle-CssClass="td_right" DataFormatString ="{0:f2}"  />
                    <asp:TemplateField HeaderText="起步价" HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" runat="server" Money='<%# Eval("Price") %>'></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="加重"  DataField="AddWeight" HeaderStyle-CssClass="td_right" DataFormatString ="{0:f2}"/>
                    <asp:TemplateField HeaderText="加价" HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# Eval("AddPrice") %>'></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                        <ItemStyle CssClass="spanD spanN" />
                           <ItemTemplate>
	                           <span class="submit_bianji"><a href='<%# "EditShippingTemplate.aspx?TemplateId="+Eval("TemplateId")%>' class="SmallCommonTextButton">编辑</a></span>
	                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" CommandArgument='<%# Eval("TemplateId") %>' CommandName="DEL_Template" IsShow="true" CssClass="SmallCommonTextButton" Text="删除"/></span>
                           </ItemTemplate>
                    </asp:TemplateField>
                                                         
            </Columns>
        </UI:Grid>
</div>
<div class="blank12 clearfix"></div>
	 <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
               </div>

			</div>
		</div>
      </div>
      </div>
</div>
</asp:Content>
