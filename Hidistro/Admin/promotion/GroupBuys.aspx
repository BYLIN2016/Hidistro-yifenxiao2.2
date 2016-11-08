<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="GroupBuys.aspx.cs" Inherits="Hidistro.UI.Web.Admin.GroupBuys" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
		<!--搜索-->
		  <div class="title">
          <em><img src="../images/06.gif" width="32" height="32" /></em>
          <h1> 团购管理 </h1>
          <span>管理所有团购活动</span>
        </div>
		<!--结束-->
	    <div class="btn">
          <a href="AddGroupBuy.aspx" class="submit_jia">添加团购活动</a>
	    </div>
		<!--数据列表区域-->
	  <div class="datalist">
      	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                
				<li><span>商品名称：</span><span><asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" /></span></li>
				
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"　Text="查询" /></li>
		  </ul>
	  </div>

<div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
				</div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" IsShow="true" runat="server" >删除</Hi:ImageLinkButton></span>
                    </li>
				</ul>
			</div>
            <span style="width:110px;height:25px; background:url(../images/icon.gif) no-repeat -100px -87px;float:right;">　　　
            <asp:LinkButton ID="btnOrder" runat="server" Text="保存排序" />
          </span>
	  </div>
		    <UI:Grid ID="grdGroupBuyList" runat="server" ShowHeader="true" AutoGenerateColumns="false" SortOrderBy="DisplaySequence"  DataKeyNames="GroupBuyId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left" HeadWidth="36"/>
                 <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left"  ItemStyle-Width="30%">
                       <ItemTemplate>
                                <a href='<%#"../../GroupBuyProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                  <Hi:SubStringLabel id="lblHelpCategory" Field="ProductName" StrLength="60" StrReplace="..." runat="server"></Hi:SubStringLabel>
                                 </a>                                 
                           <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible="false"></asp:Literal>
                        </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="状态" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="7%" >
                       <ItemTemplate>
                          <Hi:GroupBuyStatusLabel GroupBuyStatusCode='<%#Eval("Status") %>' GroupBuyStartTime='<%# Eval("StartDate") %>' runat="server" ID="GroupBuyStatusLabel" />
                        </ItemTemplate>
                 </asp:TemplateField>
                    <asp:TemplateField HeaderText="开始时间"  ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblStartDate" Time='<%#Eval("StartDate") %>' FormatDateTime="yyyy/MM/dd HH时" runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="结束时间"  ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblEndDate"  FormatDateTime="yyyy/MM/dd HH时"      Time='<%#Eval("EndDate") %>' runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="违约金"  ItemStyle-Width="5%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                          <Hi:FormatedMoneyLabel id="lblNeedPrice" Money='<%#Eval("NeedPrice") %>' runat="server"></Hi:FormatedMoneyLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField HeaderText="限购" DataField="MaxCount" ItemStyle-Width="4%" HeaderStyle-CssClass="td_right td_left" />
                 <asp:BoundField HeaderText="订购商品" DataField="ProdcutQuantity" ItemStyle-Width="5%" HeaderStyle-CssClass="td_right td_left" />
                 <asp:TemplateField HeaderText="订单"  ItemStyle-Width="4%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                        <a href="javascript:DialogFrame('sales/ManageOrder.aspx?orderStatus=0&GroupBuyId=<%# Eval("GroupBuyId") %>','团购订单',null,null);"><%#Eval("OrderCount") %></a>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="当前价格"  ItemStyle-Width="5%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                          <Hi:FormatedMoneyLabel id="lblCurrentPrice"  runat="server"></Hi:FormatedMoneyLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="排序" HeaderStyle-Width="50px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:TextBox ID="txtSequence" runat="server" Text='<%# Eval("DisplaySequence") %>' Width="50px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="10%" HeaderStyle-CssClass="td_left td_right_fff" >
                     <ItemTemplate>
                         <span class="submit_bianji"><a href='<%# "EditGroupBuy.aspx?GroupBuyId=" + Eval("GroupBuyId")%> '>编辑</a></span>
                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" IsShow="true" Text="删除"  CommandName="Delete" runat="server" /></span>
                     </ItemTemplate>
                 </asp:TemplateField>  
            </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
			<div class=pagination>
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
			</div>
			</div>
		</div>
      </div>

</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

     
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

