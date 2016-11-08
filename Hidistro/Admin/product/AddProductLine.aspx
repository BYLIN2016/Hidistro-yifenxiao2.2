<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.AddProductLine" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/03.gif" width="32" height="32" /></em>
		    <h1>添加产品线</h1>
		    <span>将产品按照产品线分组后，可将不同的产品线授权给不同的分销商去销售</span></div>
		  <div class="formitem validator5">
		    <ul>
		      <li><span class="formitemtitle Pw_140"><em>*</em>产品线名称：</span>
		        <asp:TextBox ID="txtProductLineName" runat="server" CssClass="forminput" Width="250px" />
		        <p id="ctl00_contentHolder_txtProductLineNameTip">产品线名称不能为空，在1至60个字符之间</p>
	          </li>
	          <li><span class="formitemtitle Pw_140">供货商：</span>
              <abbr class="formselect">
		        <asp:DropDownList runat="server" ID="dropSuppliers"></asp:DropDownList>
                </abbr>
	          </li>
	        </ul>
            <ul class="btn Pa_140">
		    <asp:Button ID="btnCreate" runat="server" OnClientClick="return PageIsValid();" Text="确 定"  CssClass="submit_DAqueding" />
            </ul>
	      </div>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtProductLineName', 1, 60, false, null, '产品线名称不能为空，在1至60个字符之间'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
