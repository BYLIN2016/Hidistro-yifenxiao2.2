<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddBundlingProduct.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.AddBundlingProduct" %>
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
            <h1>���������Ʒ</h1>
            <span>����ͬ��Ʒ������������</span>
          </div>
      <div class="formitem validator5">
        <ul>
          <li> <span class="formitemtitle Pw_140"><em >*</em>������Ʒ���ƣ�</span>
          <asp:TextBox ID="txtBindName" runat="server" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtBindNameTip">���Ʋ���Ϊ�գ���1��60���ַ�֮��</p>
          </li>
          
               <li> <span class="formitemtitle Pw_140"><em >*</em>�Ƿ��ϼܣ�</span>
         <Hi:YesNoRadioButtonList ID="radstock" runat="server" RepeatLayout="Flow" />
          </li>
          <li style="display:none;">
             <span class="formitemtitle Pw_140">������Ʒ��棺</span>
             <asp:TextBox ID="txtNum" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtNumTip">������Ϊ��������
              </p>
          </li>
          <li>
            <span class="formitemtitle Pw_140"><em >*</em>ԭ�۸�</span>    <span id="totalprice"></span>
          </li>
          <li><span class="formitemtitle Pw_140"><em >*</em>�������ۼۣ�</span>
            <asp:TextBox ID="txtSalePrice" runat="server" CssClass="forminput"></asp:TextBox>
           <p id="ctl00_contentHolder_txtSalePriceTip">�������۽��ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��</p>
          </li>
          <li ><span class="formitemtitle Pw_140">������Ʒ��飺</span>
                <Hi:TrimTextBox runat="server" Rows="6" Columns="76"  MaxLength="300"    ID="txtShortDescription" TextMode="MultiLine" />
                <p  id="ctl00_contentHolder_txtShortDescriptionTip" class="Pa_198">�޶���300���ַ�����</p>
            </li>          
       <li><span class="formitemtitle Pw_140 "><em >*</em>ѡ����Ʒ��</span><span style="cursor:pointer; color:blue; font-size:14px"  onclick="ShowAddDiv();">����ѡ��</span>
               <p id="P1">������Ʒ����Ϊ����������������</p>
          </li>    
          <li class="binditems">
          <table width="100%" id="addlist">
          <tr class="table_title"><th class="td_right td_left" scope="col">��Ʒ��</th><th class="td_right td_left" scope="col">sku��Ϣ</th><th class="td_right td_left" scope="col">�۸�</th><th class="td_right td_left" scope="col">����</th><th class="td_left td_right_fff" scope="col">����</th></tr>
          </table> 
           <input   id="selectProductsinfo"  name="selectProductsinfo"  type="hidden" />
          </li>      
     
           <li> <asp:Button ID="btnAddBindProduct" runat="server" Text="ȷ��" 
                   OnClientClick="return PageIsValid()&&CollectInfos(); "  CssClass="submit_DAqueding" 
                   onclick="btnAddBindProduct_Click"  /></li>
      </ul>
      </div>

      </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script type="text/javascript" >
function InitValidators() {
initValid(new InputValidator('ctl00_contentHolder_txtBindName', 1, 60, false, null, '������Ʒ������,��1��60���ַ�֮��'));
initValid(new InputValidator('ctl00_contentHolder_txtSalePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '���ۼ۸�ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtSalePrice', 0.01, 10000000.00, '���ۼ۸�ֻ������ֵ��0.01-10000000���Ҳ��ܳ���2λС��'));
 initValid(new InputValidator('ctl00_contentHolder_txtNum', 1, 10, false, '-?[0-9]\\d*', '�������ʹ��󣬿��ֻ��������������ֵ'))
 appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNum', 1, 9999999, '�������ֵ������ϵͳ��ʾ��Χ'));
 initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 0, 300, true, null, '��Ʒ������������300���ַ�����'));       
}
$(document).ready(function(){ InitValidators(); });
</script>
    <script type="text/javascript" src="../js/BundlingProduct.js"></script>
</asp:Content>

