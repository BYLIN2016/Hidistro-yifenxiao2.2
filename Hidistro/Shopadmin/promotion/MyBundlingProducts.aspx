<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyBundlingProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyBundlingProducts" %>
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
          <a href="AddMyBundlingProduct.aspx" class="submit_jia">���������Ʒ</a>
	    </div>
        
	    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
                
				<li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtProductName" runat="server" CssClass="forminput" /></span></li>
				
				<li><asp:Button ID="btnSearch" runat="server" CssClass="searchbutton"��Text="��ѯ" 
                        onclick="btnSearch_Click" /></li>
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
	  </div>
		
		<!--�����б�����-->
	  <div class="datalist">
		    <UI:Grid ID="grdBundlingList" runat="server" ShowHeader="true" AutoGenerateColumns="false" SortOrderBy="DisplaySequence"  DataKeyNames="BundlingID" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>                 
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left" HeadWidth="35"/>
                 <asp:TemplateField HeaderText="��Ʒ����" HeaderStyle-CssClass="td_right td_left"  ItemStyle-Width="30%">
                       <ItemTemplate>
                        <a href="../../BundlingProducts.aspx" target="_blank">
                        <%#Eval("name") %>
                         </a>                              
                        </ItemTemplate>
                 </asp:TemplateField>
                  <%--<asp:TemplateField HeaderText="������"  ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                     <%#Eval("num")%> 
                       </ItemTemplate>
                 </asp:TemplateField>--%>
                 <asp:TemplateField HeaderText="��������"  ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                     <%#Eval("OrderCount") %>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="����۸�"  ItemStyle-Width="15%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                          <Hi:FormatedMoneyLabel id="lblPrice"  runat="server" Money='<%# Eval("Price") %>'></Hi:FormatedMoneyLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="״̬"  ItemStyle-Width="10%"  HeaderStyle-CssClass="td_right td_left" >
                       <ItemTemplate>
                  <%# Eval("salestatus").ToString()=="1"?"�ϼ�":"�¼�" %>
                       </ItemTemplate>
                 </asp:TemplateField>
                 
                 <asp:TemplateField HeaderText="����" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff" >
                     <ItemTemplate>
                         <span class="submit_bianji"><a href='<%# "EditmyBundlingProduct.aspx?bundlingid=" + Eval("bundlingID")%>'>�༭</a></span>
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
