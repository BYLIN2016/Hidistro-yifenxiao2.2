<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyGroupBuys.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyGroupBuys" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<!--ѡ�-->

	<div class="dataarea mainwidth">
		<!--����-->
		
		<!--����-->
	    <div class="Pa_15">
          <a href="AddMyGroupBuy.aspx" class="submit_jia">����Ź��</a>
	    </div>
        
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                
				<li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" /></span></li>
				
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"��Text="��ѯ" /></li>
		  </ul>
	  </div>

<div class="functionHandleArea clearfix m_none">
			<!--��ҳ����-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
				</div>
			</div>
			<!--����-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">ȫѡ</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">��ѡ</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" IsShow="true" runat="server" >ɾ��</Hi:ImageLinkButton></span>
                    </li>
				</ul>
			</div>
            <span style="width:110px;height:25px; background:url(../images/icon.gif) no-repeat -100px -87px;float:right;">������
            <asp:LinkButton ID="btnOrder" runat="server" Text="��������" />
          </span>
	  </div>		
		<!--�����б�����-->
	  <div class="datalist">
		    <UI:Grid ID="grdGroupBuyList" runat="server" ShowHeader="true" AutoGenerateColumns="false" SortOrderBy="DisplaySequence"  DataKeyNames="GroupBuyId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left" HeadWidth="35"/>
                 <asp:TemplateField HeaderText="��Ʒ����" HeaderStyle-CssClass="td_right td_left"  ItemStyle-Width="20%">
                       <ItemTemplate>
                         <Hi:DistributorProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' IsGroupBuyProduct="true" ImageLink="true">
                           <Hi:SubStringLabel id="lblHelpCategory" Field="ProductName" StrLength="60" StrReplace="..." runat="server"></Hi:SubStringLabel>
                         </Hi:DistributorProductDetailsLink>
                           <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="״̬" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="8%" >
                       <ItemTemplate>
                          <Hi:GroupBuyStatusLabel GroupBuyStatusCode='<%#Eval("Status") %>' GroupBuyStartTime='<%# Eval("StartDate") %>'  runat="server" ID="GroupBuyStatusLabel" />
                        </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="��ʼʱ��"  ItemStyle-Width="11%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblStartDate" Time='<%#Eval("StartDate") %>' FormatDateTime="yyyy/MM/dd HHʱ" runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="����ʱ��"  ItemStyle-Width="11%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel ID="lblEndDate" Time='<%#Eval("EndDate") %>'   FormatDateTime="yyyy/MM/dd HHʱ"   runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:BoundField HeaderText="�޹�" DataField="MaxCount" ItemStyle-Width="5%" HeaderStyle-CssClass="td_right td_left" />
                 <asp:BoundField HeaderText="������Ʒ" DataField="ProdcutQuantity" ItemStyle-Width="8%" HeaderStyle-CssClass="td_right td_left" />
                 <asp:TemplateField HeaderText="����"  ItemStyle-Width="6%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                          <a target="_blank" href='<%# "../sales/ManageMyOrder.aspx?orderStatus=0&GroupBuyId="+ Eval("GroupBuyId")%>' ><%#Eval("OrderCount") %></a>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="��ǰ�۸�"  ItemStyle-Width="8%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                          <Hi:FormatedMoneyLabel id="lblCurrentPrice"  runat="server"></Hi:FormatedMoneyLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="����" HeaderStyle-Width="50px" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:TextBox ID="txtSequence" runat="server" CssClass="forminput" Text='<%# Eval("DisplaySequence") %>' Width="50px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                 <asp:TemplateField HeaderText="����" ItemStyle-Width="11%" HeaderStyle-CssClass="td_left td_right_fff" >
                     <ItemTemplate>
                         <span class="submit_bianji"><a href='<%# "EditMyGroupBuy.aspx?GroupBuyId=" + Eval("GroupBuyId")%> '>�༭</a></span>
                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" IsShow="true" Text="ɾ��"  CommandName="Delete" runat="server" /></span>
                     </ItemTemplate>
                 </asp:TemplateField>  
            </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--�����б�ײ���������-->
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
  <!--����logo����-->
</div>

</asp:Content>
