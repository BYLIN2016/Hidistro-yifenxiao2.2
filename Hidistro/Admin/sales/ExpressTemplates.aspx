<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ExpressTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ExpressTemplates" Title="无标题页" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->
	<div class="dataarea mainwidth databody">
		<!--搜索-->
		  <div class="title">
		  <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1><strong>快递单模板</strong></h1>
          <span>快递单模板管理</span>
          <span></span>
	</div>
		 	
		<!--数据列表区域-->
	  <div class="datalist">
     <UI:Grid ID="grdExpressTemplates" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ExpressId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                <asp:TemplateField HeaderText="单据编号"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <asp:Literal id="lblExpressId" runat="server" Text='<%#Eval("ExpressId") %>'></asp:Literal>
                      <asp:Literal ID="litXmlFile" runat="server" Text='<%#Eval("XmlFile") %>' Visible="false"></asp:Literal>
                   </ItemTemplate>
                </asp:TemplateField>                   
                <asp:TemplateField HeaderText="单据名称"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <asp:Literal id="lblExpressName" runat="server" Text='<%#Eval("ExpressName") %>'></asp:Literal>
                   </ItemTemplate>
                </asp:TemplateField>  
                <UI:YesNoImageColumn DataField="IsUse" HeaderText="是否启用" HeaderStyle-Width="16%" HeaderStyle-CssClass="td_right td_left" />                                                                 
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="40%" HeaderStyle-CssClass="td_left td_right_fff">
                    <ItemTemplate>
                    <span style="float:left; margin-right:10px;"><a href='<%# "EditExpressTemplate.aspx?ExpressId=" + Eval("ExpressId") + "&ExpressName=" + Globals.UrlEncode((string)Eval("ExpressName")) + "&XmlFile=" + Eval("XmlFile")%>'>编辑</a></span>
                    <span style="float:left; margin-right:10px;"><a href='<%# "AddSampleExpressTemplate.aspx?ExpressName=" + Globals.UrlEncode((string)Eval("ExpressName")) + "&XmlFile=" + Eval("XmlFile")%> '>添加相似单据</a></span>
                    <span style="float:left"><Hi:ImageLinkButton ID="lkDelete" Text="删除" IsShow="true"  CommandName="Delete" runat="server" /></span>    
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
