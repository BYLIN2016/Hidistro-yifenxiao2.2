<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddDistributorGrades.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddDistributorGrades" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" Runat="Server">

<div class="areacolumn clearfix">
		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/02.gif" width="32" height="32" /></em>
		    <h1>添加分销商等级</h1>
		    <span>使用分销等级区分分销商的级别，不同级别的分销商可以享受不同的折扣率 </span></div>
		  <div class="formitem validator3">
		    <ul>
		      <li> <span class="formitemtitle Pw_128"><em >*</em>分销商等级名称：</span>
		         <asp:TextBox ID="txtRankName" CssClass="forminput" runat="server"></asp:TextBox>
		         <p id="txtRankNameTip" runat="server">会员等级名称不能为空，长度限制在60个字符以内</p>
		      </li>
		      <li> 
              <span class="formitemtitle Pw_128"><em >*</em>分销商采购价：</span>
		      <span><table width="400" border="0" cellspacing="0">
                          <tr>
                            <td width="55">采购价&nbsp;×</td>
                            <td width="71"><asp:TextBox ID="txtValue" CssClass="forminput" Width="60" runat="server" ></asp:TextBox></td>
                            <td width="21" align="center"> %</td>
                            <td width="245" align="center">&nbsp;</td>
                          </tr>
                        </table>
                    </span><br />
                     <p id="txtValueTip" runat="server">等级折扣为不能为空，只能是1-100的正整数</p>
		          </li>
                <li>
               <span class="formitemtitle Pw_128">备注：</span>
                 <asp:TextBox ID="txtRankDesc" runat="server" TextMode="MultiLine" Width="400px" Height="120px"></asp:TextBox>
                 <p id="txtRankDescTip" runat="server">备注的长度限制在300个字符以内</p>
              </li>
	        </ul>
            <ul class="btn Pa_128">
		    <asp:Button ID="btnAddDistrbutor" OnClientClick="return PageIsValid();" Text="添 加" CssClass="submit_DAqueding" runat="server"/>
            </ul>
	      </div>
  </div>
</div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" Runat="Server">
        <script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtRankName', 1, 60, false, null, '会员等级名称不能为空，长度限制在60个字符以内'));
                initValid(new InputValidator('ctl00_contentHolder_txtValue', 1, 10, false, '-?[0-9]\\d*', '等级折扣为不能为空，只能是1-100的正整数'));
                appendValid(new NumberRangeValidator('ctl00_contentHolder_txtValue', 1, 100, '等级折扣必须在1-100之间'));
                initValid(new InputValidator('ctl00_contentHolder_txtRankDesc', 0, 300, true, null, '备注的长度限制在300个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>