<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionView.ascx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.Ascx.PromotionView" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<li> 
    <span class="formitemtitle Pw_110"><em>*</em>促销活动名称：</span>
    <asp:TextBox ID="txtPromoteSalesName" runat="server" CssClass="forminput"></asp:TextBox>
    <p id="ctl00_contentHolder_promotionView_txtPromoteSalesNameTip">活动的名称，在1至60个字符之间</p>
</li>
<li> 
    <span class="formitemtitle Pw_110"><em >*</em>开始日期：</span>
    <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarStartDate" />
    <p id="P1">只有达到开始日期，活动才会生效。</p>
</li>
<li> 
    <span class="formitemtitle Pw_110"><em >*</em>结束日期：</span>
    <UI:WebCalendar runat="server" CssClass="forminput" ID="calendarEndDate" />
    <p id="P2">当达到结束日期时，活动会结束。</p>
</li>
<li> 
    <span class="formitemtitle Pw_110"><em>*</em>适合的客户：</span>
    <span style="float:left;"><Hi:MemberGradeCheckBoxList ID="chklMemberGrade" runat="server" RepeatDirection="Horizontal" /></span>
</li>   
<li> 
    <span class="formitemtitle Pw_110">促销详细信息：</span> 
    <Kindeditor:KindeditorControl ID="fckDescription" runat="server"   Height="200px"/>
</li>   