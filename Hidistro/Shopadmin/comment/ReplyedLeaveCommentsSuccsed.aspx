<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ReplyedLeaveCommentsSuccsed.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ReplyedLeaveCommentsSuccsed" %>

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
      <h1>客户留言详情</h1>
    <span>客户留言及回复详情</span></div>
    <div class="datalist">
      <table width="200" border="0" cellspacing="0">
        <tr class="table_title">
          <td colspan="3" class="td_right td_right_fff"><span class="Name float"><a href="#"><asp:Label ID="lblUserName" runat="server"></asp:Label></a></span>  <Hi:FormatedTimeLabel ID="litLeaveDate" ShopTime="true" runat="server"  /></td>
        </tr>
        <tr>
          <td width="9%" align="right"><strong>标题：</strong></td>
          <td width="91%" colspan="2">  <asp:Literal ID="litTitle" runat="server" ></asp:Literal></td>
        </tr>
        <tr>
          <td>&nbsp;</td>
          <td colspan="2"><span class="line"><asp:Literal ID="litPublishContent" runat="server" ></asp:Literal></span></td>
        </tr>
        </table>
      <div class="Settlement"></div>
    </div>
    
    <div class="datalist">
     <asp:DataList ID="dtlistLeaveCommentsReply" runat="server"  DataKeyField="ReplyId" RepeatLayout="Flow"  >
                    <ItemTemplate>
                   <table>
                   <tr class="p">
          <td align="right" Width="9%" ><strong>管理员</strong>:</td>
          <td ><span class="float"><Hi:FormatedTimeLabel ID="FormatedTimeLabel2" time='<%# Eval("ReplyDate") %>' runat="server" /> </span><span class="Name float"> <Hi:ImageLinkButton ID="ImageLinkButton1" CommandName="Delete" Text="删除回复" runat="server" /></span></td>
        </tr>
        <tr class="p">
          <td align="right">&nbsp;</td>
          <td><asp:Label ID="lblReplyCont" runat="server" Text='<%# Eval("ReplyContent") %>'></asp:Label></td>
        </tr>
        </table> 
                    </ItemTemplate>
                </asp:DataList> 
           <div style="margin-left:100px;height:30px;"><span class="submit_faihuo"><asp:HyperLink runat="server" ID="hlReply" Text="回复" /></span></div>
       </div> 
                
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
