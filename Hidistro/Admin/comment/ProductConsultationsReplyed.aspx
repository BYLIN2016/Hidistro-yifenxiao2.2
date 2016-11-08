<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductConsultationsReplyed" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
		
      <div class="optiongroup mainwidth">
	  <ul>
        <li class="optionstar"><a href="ProductConsultations.aspx" class="optionnext"><span>未回复咨询</span></a></li>
	    <li class="menucurrent"><a href="javascript:void(0);" class="optioncurrentend"><span>已回复咨询</span></a></li>
      </ul>
</div>
	<div class="dataarea mainwidth">
    <div class="toptitle"> <em><img src="../images/07.gif" width="32" height="32" /></em> <span class="title_height">管理店铺的所有商品咨询，您可以查询或删除商品咨询</span> </div>
    <!--搜索-->
    <div class="searcharea clearfix br_search">
      <ul>
        <li><span>商品名称：</span> <span><asp:TextBox ID="txtSearchText" runat="server"  CssClass="forminput" /></span> </li>
        <li><abbr class="formselect">商品分类：<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server"  style="padding:5px 0px;"/></abbr> </li>
         <li><span>商家编码：</span> <span><asp:TextBox ID="txtSKU"  CssClass="forminput" runat="server"></asp:TextBox></span> </li>      
         <li>
           <asp:Button ID="btnSearch" runat="server" CssClass="searchbutton" Text="搜索" />
        </li>
      </ul>
    </div>
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
    </div>
    <!--数据列表区域-->
    <div class="datalist">
     <UI:Grid ID="grdConsultation" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ConsultationId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="20%" SortExpression="ProductName" HeaderText="咨询商品" HeaderStyle-CssClass="td_right td_left">
                        <itemstyle  CssClass="Name" />
                            <ItemTemplate >
                                    <div style="float:left;"><Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" /></div>                                   
                                    <div style="float:left;margin-left:10px;">
                                      <%# Eval("ProductCode")%> <br />
                                      <a href='<%#string.Format("../../ProductDetails.aspx?productId={0}",Eval("productId"))%>' target="_blank"><asp:Literal ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>' /></a>
                                    </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-Width="12%"  HeaderText="咨询用户" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">
                        <itemstyle  CssClass="Name" />
                            <ItemTemplate >
                                 <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-Width="30%" HeaderText="咨询内容" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate >
                               <asp:Label ID="lblConsultationText" runat="server" Text='<%# Eval("ConsultationText") %>' CssClass="line"></asp:Label>
                               <span class="p"><strong class="colorE">回复：</strong><asp:Label ID="Label1" runat="server" Text='<%# Eval("ReplyText") %>' CssClass="line"></asp:Label></span><span>回复时间：<Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ReplyDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel> </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-Width="12%" HeaderText="咨询时间" SortExpression="ConsultationDate" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="ConsultationDateTime1" Time='<%# Eval("ConsultationDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="6%" HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
           
                            <ItemTemplate>
                                <span class="submit_shanchu"><Hi:ImageLinkButton ID="ilikbReplyDelete" runat="server" CommandName="Delete" IsShow="true" CommandArgument='<%# Eval("ConsultationId")%>' OnClientClick="" Text="删除" /></span>
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

