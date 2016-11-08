<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyPaymentTypes.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyPaymentTypes" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>支付方式列表</h1>
<span>已添加的支付方式列表</span></div>
<div class="btn"><a href="AddMyPaymentType.aspx" class="submit_jia">添加新支付方式</a></div>
<div class="datalist">
<asp:GridView ID="grdPaymentMode" runat="server" AutoGenerateColumns="false" ShowHeader="true" Width="100%" DataKeyNames="ModeId" GridLines="None">
                 <HeaderStyle CssClass="table_title" /> 
                   <Columns>
                        <asp:TemplateField HeaderText="支付方式名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                               <asp:Label ID="lblModeName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                               <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible="false"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <UI:SortImageColumn HeaderText="显示顺序" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_right_fff">
                            <ItemTemplate>
		                        <span class="submit_bianji"><a href='<%# "EditMyPaymentType.aspx?ModeId="+Eval("ModeId") %>' >编辑</a></span>
		                        <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" IsShow="true" ID="Delete" Text="删除" CommandName="Delete" /></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

   
  </div>
</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
