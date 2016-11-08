<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="UnderlingGrades.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnderlingGrades" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/04.gif" width="32" height="32" /></em>
	    <h1>会员等级管理</h1>
<span>使用会员等级区分买家的级别，不同级别的买家可以享受不同的折扣率.</span></div>
	  <!-- 添加按钮-->
	  <div class="btn"><a href="AddUnderlingGrade.aspx" class="submit_jia">添加会员等级</a></div>
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
    <UI:Grid ID="grdUnderlingGrades" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="GradeId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="等级名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Name") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <UI:YesNoImageColumn DataField="IsDefault" HeaderText="默认会员等级" HeaderStyle-Width="16%" HeaderStyle-CssClass="td_right td_left" />
                  <asp:TemplateField HeaderText="积分满足点" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litPoints" Text='<%#Eval("Points") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="折扣" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           一口价×<asp:Literal ID="litDiscountRate" Text='<%#Eval("Discount") %>' runat="server"></asp:Literal>%
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="备注" ItemStyle-Width="28%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           &nbsp;<asp:Literal ID="litDescription" Text='<%#Eval("Description") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="22%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditUnderlingGrade.aspx?GradeId="+Eval("GradeId")%>' ></asp:HyperLink></span>
                             <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>	   
    </div>
  </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
</asp:Content>
