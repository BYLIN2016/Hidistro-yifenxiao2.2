<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditDistributorSettings" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/04.gif" width="32" height="32" /></em>
            <h1>编辑分销商</h1>
            <span>编辑分销商信息</span>
          </div>
          <div class="formtab Pg_45">
                   <ul>
                      <li class="visited">基本信息</li>                                      
                      <li><a href='<%="EditDistributorLoginPassword.aspx?userId="+ Page.Request.QueryString["userId"] %>'>登录密码</a></li>
                      <li><a href='<%="EditDistributorTradePassword.aspx?userId="+ Page.Request.QueryString["userId"] %>'>交易密码</a></li>
            </ul>
          </div>
      <div class="formitem validator2">
        <ul>
          <li> <span class="formitemtitle Pw_110">分销商名称：</span>
               <strong class="colorE"><asp:Literal ID="litUserName" runat="server" /> <Hi:WangWangConversations runat="server" ID="WangWangConversations" /></strong>
           </li>
          <li> <span class="formitemtitle Pw_110">分销商等级：<em >*</em></span>
                <abbr class="formselect">
                   <Hi:DistributorGradeDropDownList runat="server" ID="drpDistributorGrade" />
          </abbr></li>
          <li> <span class="formitemtitle Pw_110">授权产品线：<em >*</em></span>
             <Hi:ProductLineCheckBoxList runat="server" ID="chkListProductLine" RepeatDirection="Horizontal" RepeatLayout="Flow"/>
             <p style="color:Red;">注意：已经选择的授权产品线最好不要取消，因为取消后分销商下载了这个产品线下的商品将会被删除</p>
          </li>
          <li> <span class="formitemtitle Pw_110">合作备忘录：</span>
            <asp:TextBox runat="server" ID="txtRemark" TextMode="MultiLine" Height="100px" Columns="60" Rows="8"></asp:TextBox>
            <p id="txtRemarkTip" runat="server">合作备忘录的长度限制在300个字符以内</p>
          </li>
      </ul>
      <ul class="btn Pa_110 clear">
        <asp:Button ID="btnEditDistributorSettings" OnClientClick="return PageIsValid();" Text="保 存" CssClass="submit_DAqueding" runat="server" />
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
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
            function InitValidators() {
                initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 300, true, null, '合作备忘录的长度限制在300个字符以内'));
            }
 $(document).ready(function(){InitValidators();});
</script>
</asp:Content>
