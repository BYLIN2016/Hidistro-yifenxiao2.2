<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OpenIdServices.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.OpenIdServices" %>
<%@ Import Namespace="Hidistro.Core"%>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
 <style>
.loginTabao{border:1px solid #dedfd7;background:#fbffda;padding:10px 5px;color:#666;height:180px;}
.loginTabao .taobaoTitle{font-size:14px;font-weight:bold;color:#306fa2;line-height:24px;height:24px;}
.loginTabao ul li.li1{height:46px;line-height:20px;linclear:both;overflow:visible;}
.loginTabao ul li.li2{line-height:30px;height:30px;overflow:visible;}
.loginTabao ul li.li2 a img{border:none;margin-bottom:3px;}
.loginTabao ul li.li2 label,.loginTabao ul li.li2 a{float:left;padding-right:5px;}
.loginTabao ul.ul2{clear:both;}.loginTabao ul.ul2 a{padding-left:3px;color:#306fa2;}
.loginTabao ul.ul2 li{clear:both;word-break: break-all;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title">
	    <h1><table><tr><td>已开启</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/xrdl.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
    </div>
  <div class="datalist">
    <asp:Panel runat="server" ID="pnlConfigedList">
    <asp:GridView runat="server" ID="grdConfigedItems" AutoGenerateColumns="False" ShowHeader="False" DataKeyNames="FullName">
        <Columns>
            <asp:ImageField DataImageUrlField="Logo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
            </asp:ImageField>
            <asp:BoundField DataField="DisplayName" ItemStyle-Width="100px" />
             <asp:TemplateField>            
                <ItemTemplate>
                    <%# Globals.HtmlDecode(Eval("ShortDescription").ToString())%>                    
                </ItemTemplate>                
                </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" Text="配置" NavigateUrl='<%# "OpenIdSettings.aspx?t="+Eval("FullName") %>'></asp:HyperLink>
                    |
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return confirm('确定要关闭此信任登录吗？');" CommandName="Delete" Text="关闭"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlConfigedNote">
        <span>还没有配置任何信任登录信息，请从未开启的列表中选择配置。</span>
    </asp:Panel>
    </div>
	  <div class="title">
	    <h1><table><tr><td>未开启</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/xrdl.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
    </div>
    <div class="datalist">
    <asp:Panel runat="server" ID="pnlEmptyList">
    <asp:GridView runat="server" ID="grdEmptyList" AutoGenerateColumns="False" ShowHeader="False" DataKeyNames="FullName">
        <Columns>
            <asp:ImageField DataImageUrlField="Logo" ItemStyle-Width="150px">
            </asp:ImageField>
            <asp:BoundField DataField="DisplayName" ItemStyle-Width="100px" />
            <asp:TemplateField>
                <ItemTemplate>
                   <%# Globals.HtmlDecode(Eval("ShortDescription").ToString())%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" Text="配置" NavigateUrl='<%# "OpenIdSettings.aspx?t="+Eval("FullName") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlEmptyNote">
        <span>所有可使用的信任登录都已开启。</span>
    </asp:Panel>
    </div>
  </div>
</asp:Content>
