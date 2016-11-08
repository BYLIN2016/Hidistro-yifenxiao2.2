<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditStocks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditStocks" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	<div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1 class="title_line">批量修改商品库存</h1>
	    <span class="font">您可以对已选的这些商品的库存直接改成某个值，或增加/减少某个值，也可以手工输入您想要的库存后在页底处保存设置</span>
    </div>

    <div class="searcharea clearfix">
        <ul>
            <li>将原库存改为：<asp:TextBox ID="txtTagetStock" runat="server" Width="80px" /></li>
            <li><asp:Button ID="btnTargetOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
            <li>将原库存增加(输入负数为减少)：<asp:TextBox ID="txtAddStock" runat="server" Width="80px" /></li>
			<li><asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
		</ul>
     </div>

    <div class="datalist">
	      <UI:Grid ID="grdSelectedProducts" DataKeyNames="SkuId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="货号" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            &nbsp;<%#Eval("SKU") %>                                             
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                            <%#Eval("ProductName") %> <%#Eval("SKUContent")%>                                                  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="库存" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtStock" runat="server" Text = '<%#Eval("Stock") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="警戒库存" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            &nbsp;<%#Eval("AlertStock") %>                                             
                        </ItemTemplate>
                    </asp:TemplateField>           
                    </Columns>
                </UI:Grid>
    </div>        
              
     <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button ID="btnSaveStock" runat="server" OnClientClick="return PageIsValid();" Text="保存设置"  CssClass="submit_DAqueding"/></div>
</div>
</asp:Content>
