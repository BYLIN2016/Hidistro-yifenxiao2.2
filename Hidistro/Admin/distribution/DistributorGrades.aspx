<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DistributorGrades.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorGrades" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

	<!--选项卡-->
	<div class="dataarea mainwidth databody">
		  <div class="title">
  <em><img src="../images/02.gif" width="32" height="32" /></em>
  <h1>分销商等级</h1>
  <span>使用分销等级区分分销商的级别，不同级别的分销商可以享受不同的折扣率</span>
</div>
	  <!--搜索-->
	  
	  <!--结束-->

	  
	  <!--数据列表区域-->
	  <div class="datalist">
	  	  <div class="searcharea clearfix br_search">
	    <ul>
	      <li>	        
          <a href="AddDistributorGrades.aspx"  class="submit_jia">添加分销等级</a></li>
        </ul>
      </div>
	   <UI:Grid ID="grdDistributorRankList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="GradeId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="等级名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Name") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" Text='<%#Eval("Name") %>'></asp:TextBox>
                        </EditItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="折扣" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           采购价×<asp:Literal ID="litDiscountRate" Text='<%#Eval("Discount")%>' runat="server"></asp:Literal>%
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDiscountRate" runat="server" Text='<%#Eval("Discount") %>' Width="50px"></asp:TextBox>
                        </EditItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="备注" ItemStyle-Width="28%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>&nbsp;
                           <asp:Literal ID="litDescription" Text='<%#Eval("Description") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" Text='<%#Eval("Description") %>'></asp:TextBox>
                        </EditItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="22%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_bianji"><a href='<%# "EditDistributorGrades.aspx?GradeId="+Eval("GradeId") %>'>编辑</a></span>
                             <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                        </ItemTemplate>                        
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
            
            
	    
	    <div class="blank5 clearfix"></div>
      </div>
	  <!--数据列表底部功能区域-->
	 
</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
</asp:Content>