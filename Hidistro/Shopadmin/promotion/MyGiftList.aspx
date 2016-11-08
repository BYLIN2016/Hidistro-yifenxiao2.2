<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyGiftList.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyGiftList" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

  <div class="optiongroup mainwidth">
	<ul>
		<li class="optionend"><a href="MyGifts.aspx"><span>礼品下载查询</span></a></li>
		<li class="menucurrent"><a><span>礼品管理</span></a></li>
	</ul>
</div>
  <div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/06.gif" width="32" height="32" /></em>
  <h1 class="title_height"> 礼品管理 </h1>
</div>
		<!--搜索-->
		
		<div class="searcharea clearfix br_search">
		  <ul>
				<li><span>关键字：</span><span>
				  <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"></asp:TextBox>
			  </span><input type="checkbox" id="chkPromotion" runat="server" />参与促销赠送的礼品</li>
				
				<li><asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" /></li>
             
		  </ul>
	</div>
		<div class="advanceSearchArea clearfix">
			<!--预留显示高级搜索项区域-->
	    </div>
		<!--结束-->
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
				</div>
			</div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
	<div class="bottomBatchHandleArea clearfix">
			 <div class="batchHandleArea">
		      <ul>
		        <li class="batchHandleButton"> <span class="bottomSignicon"></span> <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span> 
		        <span class="reverseSelect"><a  href="javascript:void(0);" onclick="CheckReverse()">反选</a></span> 
		        <span class="delete"><asp:LinkButton runat="server" ID="lkbtnDelete">删除</asp:LinkButton></span>
		        </li>
	          </ul>
	        </div>
		</div>
      </div>
		<!--数据列表区域-->
		<div class="datalist">
		  <UI:Grid ID="grdGift" runat="server" AutoGenerateColumns="false" DataKeyNames="GiftId" SortOrderBy="GiftId" SortOrder="DESC" GridLines="None" HeaderStyle-CssClass="table_title"  Width="100%" >
            <Columns>
               <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
               <asp:TemplateField HeaderText="礼品图片" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="20%">
                    <ItemTemplate>&nbsp;<Hi:HiImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/> 
                    </ItemTemplate>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="礼品名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="20%">
                    <itemtemplate>
	                   <asp:Label ID="lblGiftName" runat="server" Text='<%# Bind("d_Name") %>'></asp:Label>
                    </itemtemplate>
                </asp:TemplateField>
             
                <asp:TemplateField HeaderText="采购价" SortExpression="PurchasePrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="15%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblSalePrice" runat="server" Money='<%# Eval("PurchasePrice")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="兑换所需积分" SortExpression="PurchasePrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="10%">
                    <itemtemplate>
                     <asp:Label ID="lblNeedPoint" runat="server" Text='<%# Bind("d_NeedPoint") %>'></asp:Label>                    
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="参与促销" SortExpression="CostPrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="15%">
                    <itemtemplate>
                        <%# Eval("d_IsPromotion").Equals(true) ? "参与促销赠送" : "不参与促销赠送"%>
                    </itemtemplate>
                 </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" SortExpression="PurchasePrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="15%">
                    <itemtemplate>
                        <a href="EditMyGifts.aspx?GiftId=<%# Eval("GiftId") %>">编辑</a>　　<a onclick="javascript:return confirm('是否真的删除礼品<%# Eval("d_Name") %>？')" href="?oper=delete&&GiftId=<%# Eval("GiftId") %>">删除</a>
                    </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
