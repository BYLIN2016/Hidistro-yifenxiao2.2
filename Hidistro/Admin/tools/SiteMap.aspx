<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteMap.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SiteMap"     MasterPageFile="~/Admin/Admin.Master"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title m_none m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>
                网站地图设置</h1>
            <span>本站为您自动生成的网站地图地址如下:<asp:HyperLink ID="Hysitemap"
                runat="server"></asp:HyperLink></span>
        </div>
        <div class="datafrom">
            <div class="formitem">
               <span style="color:Red; margin-bottom:20px;">什么是网站地图？网站地图又称站点地图，它就是一个页面，上面放置了网站上所有页面的链接。<br/>
大多数人在网站上找不到自己所需要的信息时，可能会将网站地图作为一种补救措施。搜索引擎蜘蛛非常喜欢网站地图 ----引自 百度百科
                </span> 
                <ul class="attributeContent2">
                    <li>
                        <span style="float:left">更新频率：</span> 
                        <asp:TextBox ID="tbsitemaptime" runat="server">24</asp:TextBox>(24小时是最合理的时间，用以通知搜索引擎抓取频率)</li>
                      <li>
                        <li>
                        <span style="float:left">商品数量：</span> 
                        <asp:TextBox ID="tbsitemapnum" runat="server" >100</asp:TextBox>(参与网站地图的商品数量，搜索引擎推荐为50-100个。过多可能会有负作用)</li>
                </ul>
                <div style="clear: both; margin-left:60px;">
                <asp:Button ID="btnSaveMapSettings" runat="server" OnClientClick="return Save();" 
                        Text="保 存" CssClass="submit_DAqueding float" 
                        onclick="btnSaveMapSettings_Click" /></div>
                <br />
                <br />
                我需要做什么？
您设置完后，只需静静地等待收录即可。新站通常会在20个工作日左右被收录。<br />
&nbsp;</div>
        </div>
    </div>
</asp:Content>
