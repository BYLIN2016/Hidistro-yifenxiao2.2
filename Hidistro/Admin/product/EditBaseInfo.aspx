<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditBaseInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.EditBaseInfo" Title="无标题页" %>
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
	    <h1 class="title_line">批量修改商品基本信息</h1>
	    <span class="font">手工输入您想要修改的信息后在页底处保存设置即可</span>
     </div>

     <div class="searcharea clearfix">
        <ul>
            <li>商品名称： 增加前缀 <asp:TextBox ID="txtPrefix" runat="server" Width="80px" MaxLength="20"/> 增加后缀 <asp:TextBox ID="txtSuffix" runat="server" Width="80px" MaxLength="20"/></li>
            <li><asp:Button ID="btnAddOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
            <li style="margin-left:10px;">查找字符串 <asp:TextBox ID="txtOleWord" runat="server" Width="80px" /> 替换成 <asp:TextBox ID="txtNewWord" runat="server" Width="80px" /></li>
			<li><asp:Button ID="btnReplaceOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
		</ul>
     </div>

     <div class="datalist">
	      <UI:Grid ID="grdSelectedProducts" DataKeyNames="ProductId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>    
                     <asp:TemplateField HeaderText="商品图片" ItemStyle-Width="5%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                               <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                        </ItemTemplate>
                    </asp:TemplateField>             
                    <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="25%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtProductName" runat="server" Width="280px" Text = '<%#Eval("ProductName") %>'></asp:TextBox>   
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商家编码" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>  
                             <asp:TextBox ID="txtProductCode" runat="server" Width="80px" Text = '<%#Eval("ProductCode") %>'></asp:TextBox>                                        
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="市场价" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMarketPrice" runat="server" Width="80px" Text = '<%#Eval("MarketPrice", "{0:F2}") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <asp:TemplateField HeaderText="最低零售价" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLowestSalePrice" runat="server" Width="80px" Text = '<%#Eval("LowestSalePrice", "{0:F2}") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    </Columns>
                </UI:Grid>
            </div>     
                 
     <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button ID="btnSaveInfo" runat="server" OnClientClick="return PageIsValid();" Text="保存设置"  CssClass="submit_DAqueding"/></div>
</div>
    
<script>
    function CloseWindow() {
        var win = art.dialog.open.origin; //来源页面
        // 如果父页面重载或者关闭其子对话框全部会关闭
        win.location.reload();
    }

    //return false;
</script>
</asp:Content>
