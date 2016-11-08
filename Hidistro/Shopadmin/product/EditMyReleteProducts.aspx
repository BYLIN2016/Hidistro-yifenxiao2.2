<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeBehind="EditMyReleteProducts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyReleteProducts"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="Goodsgifts">
    <div class="title"><em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1>设置相关商品</h1>
    <span>如果不设置，系统则会自动根据商品分类来匹配相关商品</span>
  </div>
    <div class="blank12 clearfix"></div>
    <div class="left">
      <h1>所有商品</h1>
      <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
      <ul>
        <li> <abbr class="formselect">
                   <Hi:DistributorProductCategoriesDropDownList ID="dropCategories" runat="server" />
                   </abbr></li>
      	<li>
      	    <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  />
      	</li>
		<li>
		    <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/>
		</li>
      </ul>
      </asp:Panel>
      <div class="content">
	  <div class="youhuiproductlist">
	  <asp:DataList runat="server" ID="dlstProducts" Width="96%" DataKeyField="ProductId" RepeatLayout="Table">
                <ItemTemplate>
                    <table width="100%" border="0" cellspacing="0" class="conlisttd">
                         <tr>
                            <td width="14%" rowspan="2" class="img">
                                  <Hi:ListImage ID="ListImage2"  runat="server" DataField="ThumbnailUrl40"/>  
                            </td>
                            <td height="27" colspan="5"  class="br_none"><span class="Name">
                                <a href='<%#Globals.ApplicationPath+"/ProductDetails.aspx?ProductId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a>
                            </span></td>
                          </tr>
                       <tr>
                        <td width="27%" height="28" valign="top"><span class="colorC">一口价：<%# Eval("SalePrice")%></span></td>
                        <td width="19%" valign="top"> 库存：<%# Eval("Stock") %></td>
                        <td width="11%" align="right" valign="top">&nbsp;</td>
                        <td width="14%" align="left" valign="top" class="a_none">&nbsp;</td>
                        <td width="15%" valign="top"><span class="submit_tianjia"><asp:LinkButton ID="Button1" runat="server" CommandName="check" Text="添加" /></span></td>
                      </tr>
                   </table>
                </ItemTemplate>
            </asp:DataList>             
		</div>
        <div class="r">
          <div>
            <asp:Button runat="server" ID="btnAddSearch" CssClass="submit_bnt2" Text="当前结果全部添加"  />
          </div>
          <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
            </div></div>
      </div>
    </div>
    <div class="right">
      <h1>相关商品</h1>
      <ul>
		 <li>
		    <asp:Button runat="server" ID="btnClear" CssClass="submit_queding" Text="清空列表" />
		 </li>
      </ul>
       <div class="content">
	   <div class="youhuiproductlist">
	    <asp:DataList runat="server" ID="dlstSearchProducts" Width="96%" DataKeyField="ProductId" RepeatLayout="Table">
                <ItemTemplate>
                     <table width="100%" border="0" cellspacing="0" class="conlisttd">
                        <tr>
                           <td width="14%" rowspan="2" class="img">
                                    <Hi:ListImage ID="ListImage2"  runat="server" DataField="ThumbnailUrl40"/>  
                             </td>
                            <td height="27" colspan="4"  class="br_none"><span class="Name"><a href='<%#Globals.ApplicationPath+"/ProductDetails.aspx?ProductId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                
                             </td>
                         </tr>
                         <tr>
                             <td width="27%" height="28" valign="top"><span class="colorC">一口价：<%# Eval("SalePrice")%></span></td>
                             <td width="27%" valign="top"> 库存：<%# Eval("Stock") %></td>
                             <td width="15%" align="left" valign="top">&nbsp;</td>
                             <td width="15%" align="left" valign="top" class="a_none"><span class="submit_shanchu"><asp:LinkButton ID="Button2" runat="server" CommandName="Delete" Text="删除" /></span></td>
                       </tr>
                     </table>
                </ItemTemplate>
            </asp:DataList>    
		 </div>
         <div class="r">
         <div> &nbsp;</div>
           <div class="pagination">
           <UI:Pager runat="server" ShowTotalPages="false" ID="pagerSubject" PageIndexFormat="pageindex1" /> </div>
         </div>
      </div>
    </div>
</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
