<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ReplyMyLeaveComments.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ReplyMyLeaveComments" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">


<div class="dataarea mainwidth databody">
    <div class="title m_none td_bottom"> 
      <em><img src="../images/07.gif" width="32" height="32" /></em>
      <h1>回复客户留言</h1>
    <span>回复客户咨询消息</span></div>
    <div class="datafrom">
    <div class="formitem">
                <ul>
                  <li> <span class="formitemtitle Pw_100">留言客户：</span><a ><asp:Literal ID="litUserName" runat="server"></asp:Literal></a></li>
                  <li> <span class="formitemtitle Pw_100">标题：</span><strong class="colorQ"><asp:Literal ID="litTitle" runat="server"></asp:Literal></strong></li>
                  <li> <span class="formitemtitle Pw_100">内容：</span><strong class="colorQ"><asp:Label ID="litContent" style="word-wrap: break-word; word-break: break-all;" runat="server"></asp:Label></strong></li>
                  <li><span class="formitemtitle Pw_100">留言回复：</span> 
                  <span style="float:left;"><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fckReplyContent" runat="server" Width="550px"  height="200px" /></span>
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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtReplyTitle', 1, 60, false, null, '回复标题不能为空，长度限制在60个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>
