<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductTypes" MasterPageFile="~/Admin/Admin.Master" MaintainScrollPositionOnPostback="true" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>商品类型</h1>
	    <span>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，比如服装类型的颜色，尺码；图书类型的作者，出版社等</span>
     </div>
	  <!-- 添加按钮-->
	  <div class="btn">
	    <a href="AddProductType.aspx" class="submit_jia">添加新商品类型</a>
    </div>        
	  <!--结束-->
	  <!--数据列表区域-->
  <div class="datalist">
	    <div class="search clearfix">
			<ul>
				<li><span>商品类型名称：</span>
				    <span><asp:TextBox ID="txtSearchText" runat="server" Button="btnSearchButton" CssClass="forminput" /></span>
				</li>
			    <li>
				    <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="searchbutton"/>
				</li>
			</ul>
	</div>
	    <UI:Grid ID="grdProductTypes" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="TypeId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
                    <asp:TemplateField HeaderText="商品类型名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
		                    <asp:Label ID="lblTypeName" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTypeName" runat="server" Text='<%# Eval("TypeName") %>'></asp:TextBox>                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
                            <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_bianji"><asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditProductType.aspx?TypeId="+Eval("TypeId")%>' ></asp:HyperLink></span> 
	                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete"  Text="删除" /></span>
                         </ItemTemplate>
                     </asp:TemplateField>  
                                     
            </Columns>
        </UI:Grid>
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
  </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

