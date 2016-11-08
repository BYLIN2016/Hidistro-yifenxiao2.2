<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AcceptDistributorRequest.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AcceptDistributorRequest" %>
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
            <em><img src="../images/03.gif" width="32" height="32" /></em>
            <h1>接受分销商申请</h1>
            <span class="font">接受分销商的申请信息</span>
      </div>
      <div class="Pg_45 Pg_15 clearfix">
       <span class="colorA"><strong>小提示：</strong></span>
       <span class="colorE">请先为分销商设置等级授权产品线，然后才可以确认接受分销商申请</span>
       </div>
          <div class="formitem validator2">
            <ul>
              <li> 
              <span class="formitemtitle Pw_100">分销商名称：</span>
              <strong><asp:Literal ID="litName" runat="server" /></strong>
              </li>
             
              <li> <span class="formitemtitle Pw_100">分销商等级：<em >*</em></span>
                  <abbr class="formselect">
                    <Hi:DistributorGradeDropDownList ID="dropDistributorGrade" runat="server" />                                                                       
                </abbr>
              </li>
              <li> <span class="formitemtitle Pw_100">授权产品线：<em >*</em></span>
                <span class="float"><Hi:ProductLineCheckBoxList ID="chklProductLine" runat="server"  RepeatColumns="6"/></span>
              </li>
                <li> <span class="formitemtitle Pw_100">合作备忘录：</span>
                 <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="400px" Height="120px" CssClass="forminput"></asp:TextBox>
                  <p id="ctl00_contentHolder_txtRemarkTip">合作备忘录的长度限制在300个字符以内</p>
                </li>
            </ul>
            <ul class="btn Pa_100">
               <asp:Button ID="btnAddDistrbutor" OnClientClick="return PageIsValid();" Text="确 定" CssClass="submit_DAqueding" runat="server"/>
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
                initValid(new InputValidator('ctl00_contentHolder_txtRemark', 0, 300, true, null, '合作备忘录的长度限制在300个字符以内'));
            }
            $(document).ready(function() { InitValidators(); });
        </script>
</asp:Content>