<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ShippingTypes" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title">
		  <em><img src="../images/03.gif" width="32" height="32" /></em>
		  
          <h1 class="title_height"><strong>配送方式</strong></h1>
          <span>配送方式都是针对物流公司并且结合物流公司的到达地区和收费标准设置的，您可以为不同的到达地区设置不同的收费标准</span>
		</div>
	<!--数据列表区域-->
	<div class="datalist">

	<div class="functionHandleArea clearfix">
			<!--分页功能-->
		   <div class="pageHandleArea">
				<ul>
					<li><a href="AddShippingType.aspx" class="submit_jia">添加新配送方式</a></li>
				</ul>
			</div>
			<!--结束-->
	  </div>
	<div>
	 <UI:Grid ID="grdShippingModes" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ModeId" GridLines="None" Width="100%" HeaderStyle-CssClass="border_background">
          <HeaderStyle CssClass="table_title" />
            <Columns>   
                    <asp:TemplateField HeaderText="方式名称"  HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                           <asp:Label ID="lblShippingModesName" Text='<%#Eval("Name") %>' runat="server"></asp:Label>
                           <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <a href='<%# "EditShippingTemplate.aspx?TemplateId="+Eval("TemplateId") %>'><%# Eval("TemplateName")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="首重" DataField="Weight" HeaderStyle-CssClass="td_right" DataFormatString ="{0:f2}"  />
                    <asp:TemplateField HeaderText="缺省价" HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" runat="server" Money='<%# Eval("Price") %>'></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="加重"  DataField="AddWeight" HeaderStyle-CssClass="td_right" DataFormatString ="{0:f2}" />
                    <asp:TemplateField HeaderText="加价" HeaderStyle-CssClass="td_right">
                        <ItemTemplate>
                            <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" runat="server" Money='<%# Eval("AddPrice") %>'></Hi:FormatedMoneyLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <UI:SortImageColumn HeaderText="排列序号" HeaderStyle-CssClass="td_right"/>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                        <ItemStyle CssClass="spanD spanN" />
                           <ItemTemplate>
	                           <span class="submit_bianji"><a href='<%# "EditShippingType.aspx?ModeId="+Eval("ModeId")%>' class="SmallCommonTextButton">编辑</a></span>
	                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" CommandName="Delete" IsShow="true" CssClass="SmallCommonTextButton" Text="删除"/></span>
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
<!--数据列表底部功能区域-->
</div>   
</asp:Content>
