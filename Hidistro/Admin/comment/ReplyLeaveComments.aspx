<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ReplyLeaveComments.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ReplyLeaveComments" EnableSessionState="True" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">



<div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom"> 
      <em><img src="../images/07.gif" width="32" height="32" /></em>
      <h1>回复客户留言</h1>
    <span>回复客户留言</span></div>
    <div class="datafrom">
    <div class="formitem">
                <ul>
                  <li> <span class="formitemtitle Pw_100">留言客户：</span> <a><asp:Literal ID="litUserName" runat="server"></asp:Literal></a></li>
                  <li> <span class="formitemtitle Pw_100">标题：</span><strong class="colorQ"><asp:Literal ID="litTitle" runat="server"></asp:Literal></strong></li>
                  <li> <span class="formitemtitle Pw_100">内容：</span><strong class="colorQ"><asp:Label ID="litContent" style="word-wrap: break-word; word-break: break-all;" runat="server"></asp:Label></strong></li>
                  <li><span class="formitemtitle Pw_100">留言回复：</span> 
                  <span style="float:left;"><Kindeditor:KindeditorControl id="fckReplyContent" runat="server" Width="550px"  height="200px" /></span>

                  </li>
                </ul>
                <ul class="btntf Pa_100 clear">
                 <asp:Button ID="btnReplyLeaveComments" runat="server" Text="回复" CssClass="submit_DAqueding inbnt"  />
          </ul>
      </div>
    </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
        
</asp:Content>
           
