<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddCoupon.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddCoupon" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix">

      <div class="columnright">
          <div class="title">
            <em><img src="../images/06.gif" width="32" height="32" /></em>
            <h1>����Ż�ȯ</h1>
            <span>�����Ż�ȯ��Ϣ</span>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_100"><em >*</em>�Ż�ȯ���ƣ�</span>
          <asp:TextBox ID="txtCouponName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtCouponNameTip">���Ʋ���Ϊ�գ���1��60���ַ�֮��</p>
          </li>
          <li>
             <span class="formitemtitle Pw_100">�����</span>
             <asp:TextBox ID="txtAmount" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtAmountTip">������ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��</p>
          </li>
          <li><span class="formitemtitle Pw_100"><em >*</em>�ɵֿ۽�</span>
            <asp:TextBox ID="txtDiscountValue" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtDiscountValueTip">�ɵֿ۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��</p>
          </li>
               <li>
            <span class="formitemtitle Pw_100"><em >*</em>��ʼ���ڣ�</span>
            <UI:WebCalendar ID="calendarStartDate" runat="server" cssclass="forminput" />       
          </li>     
          <li>
            <span class="formitemtitle Pw_100"><em >*</em>�������ڣ�</span>
            <UI:WebCalendar ID="calendarEndDate" runat="server" cssclass="forminput" />       
          </li>     
          <li>
            <span class="formitemtitle Pw_100"><em >*</em>�һ�����֣�</span>
            <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
            <p id="P1">�һ��������ֻ�������֣�������ڵ���O,0��ʾ���ܶһ�</p>
	      </li>     
            <asp:Button ID="btnAddCoupons" runat="server" Text="���" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding"  />
      </ul>
      </div>

      </div>
  </div>







</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

     <script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_txtCouponName', 1, 60, false, null,  '�Ż�ȯ�����ƣ���1��60���ַ�֮��'));
initValid(new InputValidator('ctl00_contentHolder_txtAmount', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '������ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtAmount', 0.01, 10000000.00, '������ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
initValid(new InputValidator('ctl00_contentHolder_txtDiscountValue', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '�ɵֿ۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtDiscountValue', 0.01, 10000000.00, '�ɵֿ۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
initValid(new InputValidator('ctl00_contentHolder_txtCount', 0, 10, false, '-?[0-9]\\d*', '��������ֻ�������֣�������ڵ�O,0��ʾ������'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtCount', 0, 1000, '��������ֻ�������֣�������ڵ�O,С��1000��0��ʾ������'));
}
$(document).ready(function(){ InitValidators(); });
</script>

</asp:Content>

