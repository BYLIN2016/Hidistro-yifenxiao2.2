<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddShipper.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddShipper" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/05.gif" width="32" height="32" /></em>
            <h1>添加发货信息</h1>
            <span>发货信息方便打印快递单时用来选择 </span>
          </div>
      <div class="formitem validator4">
        <ul>
          <li> <span class="formitemtitle Pw_110"><em >*</em>发货点：</span>
            <asp:TextBox ID="txtShipperTag" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtShipperTagTip">发货点不能为空，用来选择从哪个点发货</p>
          </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>发货人姓名：</span>
            <asp:TextBox ID="txtShipperName" CssClass="forminput" runat="server" />
            <p id="ctl00_contentHolder_txtShipperNameTip">发货人姓名不能为空，只能是汉字或字母开头，长度在2-20个字符之间</p>
          </li>
          <li> <span class="formitemtitle Pw_110"><em >*</em>发货地区：</span>
            <abbr class="formselect"><Hi:RegionSelector runat="server" ID="ddlReggion" /></abbr>
          </li>
           <li> <span class="formitemtitle Pw_110"><em >*</em>发货详细地址：</span>
                <asp:TextBox ID="txtAddress" CssClass="forminput" runat="server" Width="450px" />
                <p id="ctl00_contentHolder_txtAddressTip">发货详细地址不能为空，长度在1-300个字符之间</p>
          </li>
           <li> <span class="formitemtitle Pw_110">手机号码：</span>
                <asp:TextBox ID="txtCellPhone" CssClass="forminput" runat="server" />
                <p id="ctl00_contentHolder_txtCellPhoneTip">手机号码的长度限制在20个字符以内</p>
          </li>
           <li> <span class="formitemtitle Pw_110">电话号码：</span>
                <asp:TextBox ID="txtTelPhone" CssClass="forminput" runat="server" />
                <p id="ctl00_contentHolder_txtTelPhoneTip">电话号码的长度限制在20个字符以内</p>
          </li>
           <li> <span class="formitemtitle Pw_110">邮政编码：</span>
                <asp:TextBox ID="txtZipcode" CssClass="forminput" runat="server" />
                <p id="ctl00_contentHolder_txtZipcodeTip">邮政编码的长度限制在20个字符以内</p>
          </li>
          <li> <span class="formitemtitle Pw_110">设为默认：</span>
           <Hi:YesNoRadioButtonList id="chkIsDefault" runat="server" RepeatLayout="Flow" />
          </li>
          <li> <span class="formitemtitle Pw_110">备注：</span>
            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine"  CssClass="forminput" Width="450" Height="120"></asp:TextBox>
            <p id="ctl00_contentHolder_txtRemarkTip">备注的长度限制在20个字符以内</p>
          </li>
      </ul>
      <ul class="btn Pa_110 clear">
        <asp:Button ID="btnAddShipper" OnClientClick="return PageIsValid();" Text="确 定" CssClass="submit_DAqueding" runat="server"/>
        </ul>
      </div>

      </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
 <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtShipperTag', 1, 30, false, null, '发货点不能为空，长度限制在30个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtShipperName', 2, 20, false, '[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*', '发货人姓名不能为空，只能是汉字或字母开头，长度在2-20个字符之间'));
                initValid(new InputValidator('ctl00_contentHolder_txtAddress', 1, 300, false, null, '发货详细地址不能为空，长度限制在300个字符以内'));
                
                initValid(new InputValidator('ctl00_contentHolder_txtCellPhone', 0, 20, true, null, '手机号码的长度限制在20个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtTelPhone', 0, 20, true, null, '电话号码的长度限制在20个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtZipcode', 0, 20, true, null, '邮政编码的长度限制在20个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 20, true, null, '备注的长度限制在20个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>
