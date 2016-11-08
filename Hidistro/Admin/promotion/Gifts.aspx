<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Gifts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Gifts" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
  <div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/06.gif" width="32" height="32" /></em>
  <h1> 礼品管理 </h1>
  <span>礼品管理</span>
</div>


		<!--数据列表区域-->
		<div class="datalist">
				<div class="searcharea clearfix br_search">
		  <ul>
				<li><span>关键字：</span><span>
				  <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"></asp:TextBox>
				  <input type="checkbox" id="chkPromotion" runat="server" />参与促销赠送的礼品
			  </span></li>
				
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
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span> 
              <span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span> 
              <span class="delete"><Hi:ImageLinkButton ID="lkbDelectCheck"  IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
	        </ul>
	      </div>
      </div>
		  <UI:Grid ID="grdGift" runat="server" AutoGenerateColumns="false" DataKeyNames="GiftId" SortOrderBy="GiftId" SortOrder="DESC" GridLines="None" HeaderStyle-CssClass="table_title"  Width="100%" >
            <Columns>
                <UI:CheckBoxColumn HeaderStyle-CssClass="td_right td_left"/>
                <asp:TemplateField HeaderText="礼品图片" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="13%">
                    <ItemTemplate>
                        <a href='<%#"../../GiftDetails.aspx?GiftId="+Eval("GiftId")%>' target="_blank"><Hi:HiImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/></a>  
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="礼品名称" SortExpression="Name" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="15%">
                    <itemtemplate>	                  
                             <a href='<%#"../../GiftDetails.aspx?GiftId="+Eval("GiftId")%>' target="_blank"><asp:Label ID="lblGiftName" runat="server" Text='<%# Bind("Name") %>'></asp:Label></a>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="成本价" SortExpression="CostPrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="5%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblCostPrice" runat="server" Money='<%# Eval("CostPrice")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="采购价" SortExpression="PurchasePrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="5%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblSalePrice" runat="server" Money='<%# Eval("PurchasePrice")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="市场参考价" SortExpression="CostPrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="10%">
                    <itemtemplate>
                        <Hi:FormatedMoneyLabel ID="lblMarketPrice" runat="server" Money='<%# Eval("MarketPrice")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="兑换所需积分" SortExpression="CostPrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="10%">
                    <itemtemplate>
                        <asp:Label ID="lblNeedPoint" runat="server" Text='<%# Eval("NeedPoint")%>' />
                    </itemtemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="允许下载" SortExpression="CostPrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="8%">
                    <itemtemplate>
                        <%# Eval("IsDownLoad").Equals(true) ? "<a href='?oper=update&&GiftId=" + Eval("GiftId") + "&&Status=0'><img alt='点击取消' src='../images/iconaf.gif' /></a>" : "<a href='?oper=update&&GiftId=" + Eval("GiftId") + "&&Status=1'><img alt='点击发布' src='../images/ta.gif' /></a>"%>
                    </itemtemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="参与促销" SortExpression="CostPrice" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="12%">
                    <itemtemplate>
                        <%# Eval("IsPromotion").Equals(true) ? "参与促销赠送" : "不参与促销赠送"%>
                    </itemtemplate>
                 </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                    <ItemStyle />
                    <itemtemplate> 
			            <span class="submit_bianji"><asp:LinkButton runat="server"  ID="Edit" Text="编辑" CommandName="Edit"  /> </span><span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" IsShow="true" CommandName="Delete" ></Hi:ImageLinkButton></span>
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


