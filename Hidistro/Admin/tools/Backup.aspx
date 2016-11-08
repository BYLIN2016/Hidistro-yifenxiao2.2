<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.Backup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
    <div class="title"> 
        <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>数据备份</h1>
        <span>在本处可备份所有的数据库数据与结构,备份完成后可通过数据库恢复功能进行恢复，备份过程中请勿进行其他页面操作。</span>
    </div>
    <div class="datafrom">
        <div class="formitem validator1">
            <ul class="btntf Pa_198">
            <asp:Button ID="btnBackup" runat="server" Text="开始备份" CssClass="submit_DAqueding inbnt"  />
           </ul>
        </div>
    </div>
</div>
</asp:Content>
