<%@ Page Language="C#" MasterPageFile="~/Shopadmin/Shopadmin.Master" AutoEventWireup="true" CodeBehind="EditMySaleCounts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMySaleCounts" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	 <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1 class="title_line">批量修改前台商品显示的销售数量</h1>
	    <span class="font">手工输入您想要修改的信息后在页底处保存设置即可</span>
     </div>

     <div class="searcharea clearfix">
        <ul>
            <li>直接调整：前台显示的销售数量= <asp:TextBox ID="txtSaleCounts" runat="server" Width="80px" MaxLength="20"/></li>
            <li><asp:Button ID="btnAddOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
            <li style="margin-left:10px;">公式调整：前台显示的销售数量=实际销售数量
                <Hi:OperationDropDownList ID="ddlOperation" runat="server" AllowNull="false"  /> <asp:TextBox ID="txtOperationSaleCounts" runat="server" Width="80px" /></li>
			<li><asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="searchbutton"/></li>
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
                    <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <%#Eval("ProductName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商家编码" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>  
                             <%#Eval("ProductCode") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="实际销售数量" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <%#Eval("SaleCounts")%>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <asp:TemplateField HeaderText="前台显示的销售数量" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtShowSaleCounts" runat="server" CssClass="forminput" Width="80px" Text = '<%#Eval("ShowSaleCounts") %>'></asp:TextBox>
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
