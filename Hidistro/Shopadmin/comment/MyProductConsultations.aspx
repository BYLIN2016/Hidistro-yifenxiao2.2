<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyProductConsultations.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyProductConsultations" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<div class="optiongroup mainwidth">
	  <ul>
        <li class="menucurrent"><a href="MyProductConsultations.aspx"><span>未回复咨询</span></a></li>
	    <li class="optionend"><a href="MyProductConsultationsReplyed.aspx"><span>已回复咨询</span></a></li>
      </ul>
</div>
	<div class="dataarea mainwidth">
    <div class="toptitle"> <em><img src="../images/07.gif" width="32" height="32" /></em> <span class="title_height">管理店铺的所有商品咨询，您可以查询或删除商品咨询</span> </div>
    <!--搜索-->
    <div class="searcharea clearfix br_search">
      <ul>
        <li><span>商品名称：</span> <span><asp:TextBox ID="txtSearchText" runat="server"  CssClass="forminput" /></span> </li>
        <li><abbr class="formselect">商品分类：<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" /></abbr> </li>
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
      <div class="bottomPageNumber clearfix">
      <div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
				</div>
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
                                   
                                     <Hi:DistributorProductDetailsLink ID="ProductDetailsLink1" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>'></Hi:DistributorProductDetailsLink>
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
                               <asp:Label ID="lblConsultationText" runat="server" Text='<%# Eval("ConsultationText")%>' CssClass="line"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-Width="12%" HeaderText="咨询时间" SortExpression="ConsultationDate" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ConsultationDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="15%" HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
           
                            <ItemTemplate>
                                <span class="submit_faihuo"><a href='<%# Globals.ApplicationPath+(string.Format("/Shopadmin/comment/ReplyMyProductConsultations.aspx?ConsultationId={0}", Eval("ConsultationId")))%>'>回复</a></span>
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


<%--
<div class="areacolumn clearfix">

        <h1>商品咨询管理
        <span class="spanF">管理店铺的所有商品咨询，您可以查询或删除商品咨询 </span></h1>

          <fieldset>
          <legend style="search_title">搜索条件</legend>
          <ul>
                <li style="float:left; margin-left:15px;">商品名称：<asp:TextBox ID="txtSearchText" runat="server"  /></li>
                <li style="float:left; margin-left:15px;">商品分类：<Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" /></li>
                <li style="float:left; margin-left:15px;">商家编码：<asp:TextBox ID="txtSKU" Width="110" runat="server"></asp:TextBox></li>
                <li style="float:left; margin-left:15px;">页面大小：<Hi:PageSizeDropDownList ID="dropPageSize" runat="server" />
                    <asp:Button ID="btnSearch" runat="server" class="submit54" Text="搜索" /></li>
          </ul>
        </fieldset>
       
          <div class="table">
         <div class="subnav">
           <ul>
             <li class="subnav_a"><span class="subnav_padding">未回复咨询</span></li>
             <li class="subnav_link subnav_right"><a href="MyProductConsultationsReplyed.aspx"><span class="subnav_padding">已回复咨询</span></a></li>
           </ul>
         </div>
         <div class="blank12 clearfix"></div>
         <div>
	          <UI:Grid ID="grdConsultation" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ConsultationId" HeaderStyle-CssClass="border_background" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField SortExpression="ProductName" HeaderText="咨询商品" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <ItemTemplate>
                                    <div style="float:left;"><Hi:ListImage ID="HiImage1" runat="server" DataField="ThumbnailUrl40" /></div>                                   
                                    <div style="float:left;margin-left:10px;">
                                      <%# Eval("SKU") %> <br />
                                      <a href="../../ProductDetails.aspx" target="_blank"><asp:Literal ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>' /></a>
                                    </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="咨询用户" SortExpression="UserName" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="咨询内容" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <ItemTemplate>
                                <asp:Label ID="lblConsultationText" runat="server" Text='<%# Eval("ConsultationText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField ItemStyle-Width="120px" HeaderText="咨询时间" SortExpression="ConsultationDate" HeaderStyle-CssClass="border_right border_top border_bottom">
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ConsultationDate") %>'
                                    runat="server"></Hi:FormatedTimeLabel>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                            <ItemStyle CssClass="spanD spanN" />
                            <ItemTemplate>
                                <a href='<%# Globals.ApplicationPath+(string.Format("/Shopadmin/comment/ReplyMyProductConsultations.aspx?ConsultationId={0}", Eval("ConsultationId")))%>'>回复</a> |
                                <Hi:ImageLinkButton ID="ilikbReplyDelete" runat="server" CommandName="Delete" IsShow="true" CommandArgument='<%# Eval("ConsultationId")%>' OnClientClick="" Text="删除" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </UI:Grid>
            </div>
            <div class="page"><UI:Pager runat="server" ID="pager" RunningMode="Get" ListToPaging="grdConsultation" /></div>
          </div>
          </div>
          --%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
