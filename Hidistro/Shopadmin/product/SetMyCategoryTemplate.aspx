<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SetMyCategoryTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SetMyCategoryTemplate" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
	<ul>
		<li class="optionstar"><a href="ManageMyCategories.aspx" class="optionnext"><span>商品分类管理</span></a></li>
		<li class="menucurrent"><a class="optioncurrentend"><span class="optioncenter">分类模板设置</span></a></li>
	</ul>
</div>
<div class="blank12 clearfix"></div>
<div class="dataarea mainwidth databody">
    <div class="title">
      <em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1>顶级分类模板设置</h1>
      <span>可以为顶级分类设置不同的模板风格，分类模板只对顶级分类有用，各个子分类依然使用默认模板 </span>
    </div>  
    <div style="height:25px;padding-left:23px;">
        上传分类模板文件： <asp:FileUpload ID="fileThame" runat="server" Width="280px" />
        <asp:Button runat="server" ID="btnUpload" Text="上传" /><span style="color:Red">(上传文件必须为html格式)</span>
    </div>
    <div style="height:25px;line-height:25px;padding-left:23px;">
        删除分类模板文件： <asp:DropDownList runat="server" ID="dropThmes" />
	    <Hi:ImageLinkButton  runat="server" ID="btnDelete" Text="删除" DeleteMsg="请确认是否删除模板" CssClass="inp_L1" />
    </div>
    <div class="datalist">
        <UI:Grid ID="grdTopCategries" DataKeyNames="CategoryId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
            <asp:TemplateField HeaderText="分类名称" HeaderStyle-Width="40%" HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                   <%# Eval("Name") %>
                </ItemTemplate>
            </asp:TemplateField>
            <UI:DropdownColumn HeaderText="套用的模板" JustForEditItem="false" ID="dropTheme" DataKey="Theme" DataTextField="Name"  NullToDisplay="无" 
                DataValueField="ThemeName" AllowNull="true"  SortExpression="ThemeName" ItemStyle-Width="40"></UI:DropdownColumn>
             <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                 <ItemTemplate>
                    <span class="submit_shanchu"><asp:LinkButton ID="lblSave" CommandName="Save" runat="server" Text="保存模板设置" /></span>
                 </ItemTemplate>
             </asp:TemplateField>                     
            </Columns>
        </UI:Grid>
    </div>
    <div style="padding-left:12px">
        <asp:Button ID="btnSaveAll" runat="server" Text="批量保存设置" CssClass="inp_L1" />
    </div>
</div>
</asp:Content>
