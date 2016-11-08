<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="UserArealDistributionStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.UserArealDistributionStatistics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>会员地区分布</h1>
  <span>客户所在地的分布统计</span>
</div>
		<!--搜索-->
	    <!--数据列表区域-->
	  <div class="datalist">
	          <UI:Grid ID="grdUserStatistics" runat="server" ShowHeader="true" DataKeyNames="RegionId" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>                                         
                            <asp:TemplateField HeaderText="所在地" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                   <asp:Label runat="server" ID="lblReionName"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:BoundField HeaderText="客户数量" DataField="UserCounts" HeaderStyle-CssClass="td_right td_left"/>                                                                                                                                                                                                                                                                                                   
                            <asp:TemplateField HeaderText="百分比"  HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <img width='<%# string.Format("{0}px", Eval("Lenth")) %>' height="15" class="votelenth"/><asp:Literal ID="lblPercentage" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.Percentage", "{0:N2}") %>' />%
                                </ItemTemplate>
                            </asp:TemplateField> 
                        </Columns>
               </UI:Grid>
     
	  </div>

</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
