<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RestoreDatabase.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.RestoreDatabase" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
    
<div class="dataarea mainwidth databody">
    <div class="title"> 
        <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1>数据恢复</h1>
        <span>通过本界面对以往备份的数据库进行恢复或下载，请注意在恢复后，所有数据库信息包括管理员用户名密码都会恢复成备份时的状态。</span>
    </div>
    <div class="datalist">
     <UI:Grid ID="grdBackupFiles" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
        <Columns>   
            <asp:TemplateField HeaderText="备份文件名"  HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>                 
                  <asp:HyperLink ID="hlinkName" runat="server" Text=' <%#Eval("BackupName") %>' NavigateUrl='<%# Globals.ApplicationPath + "/Storage/data/Backup/" + Eval("BackupName") %>' />
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="版本号"  HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <%# Eval("Version") %>
                </ItemTemplate>
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="大小(字节)"  HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <%# Eval("FileSize")%>
                </ItemTemplate>
            </asp:TemplateField>   
            <asp:TemplateField HeaderText="备份时间"  HeaderStyle-CssClass="td_right td_left">
                <ItemTemplate>
                    <%# Eval("BackupTime")%>
                </ItemTemplate>
            </asp:TemplateField>                               
            <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                <ItemTemplate>
                <span class="submit_bianji"><Hi:ImageLinkButton ID="lkRestore" Text="恢复" IsShow="true"  CommandName="Restore" runat="server" DeleteMsg="您确定要执行恢复操作吗？执行后数据将恢复到备份时的状态！" /></span>
                <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" Text="删除" IsShow="true"  CommandName="Delete" runat="server" /></span></span>                      
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
</div>
</asp:Content>
