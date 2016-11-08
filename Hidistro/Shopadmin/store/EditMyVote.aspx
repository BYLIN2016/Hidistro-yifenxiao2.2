<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyVote.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyVote" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="MyVotes.aspx"><span>投票调查管理</span></a></li>
                  </ul>
    </div>
      <div class="columnright">
          <div class="title title_height">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑投票</h1>
        </div>
      <div class="formitem validator3">
        <ul>
          <li> <span class="formitemtitle Pw_128">投票标题：<em >*</em></span>
            <asp:TextBox ID="txtAddVoteName" runat="server" CssClass="forminput" />
            <p runat="server" id="txtAddVoteNameTip">投票调查的标题，长度限制在60个字符以内</p>
          </li>
           <li> <span class="formitemtitle Pw_128">在前台是否显示：</span>
             <asp:CheckBox ID="checkIsBackup" runat="server" />
          </li>
          <li> <span class="formitemtitle Pw_128">最多可选项数：<em >*</em></span>
            <asp:TextBox ID="txtMaxCheck" runat="server" Text="1"  CssClass="forminput" />
             <p runat="server" id="txtMaxCheckTip" >最多可选项数不允许为空，范围为1-100之间的整数</p>
          </li>
          <li> <span class="formitemtitle Pw_128">投票选项：<em >*</em></span>
           <asp:TextBox ID="txtValues" runat="server" Width="300" Height="100" TextMode="MultiLine"></asp:TextBox>
            <p runat="server" id="txtValuesTip" >在输入框中用回车换行区分多个选项值</p>
          </li>
         
      </ul>
      <ul class="btn Pa_128">
         <asp:Button ID="btnEditVote"  runat="server" Text="确 定"  OnClientClick="return PageIsValid()"  CssClass="submit_DAqueding"/>  
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
        initValid(new InputValidator('ctl00_contentHolder_txtAddVoteName', 1, 60, false, null, '投票调查的标题，长度限制在60个字符以内'));
        initValid(new InputValidator('ctl00_contentHolder_txtMaxCheck', 1, 10, false, '-?[0-9]\\d*', '设置一次投票最多可以选择投几个选项'));
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtMaxCheck', 1, 100, '您设置的最多可选项数量超出了系统允许1-100的范围'));
        initValid(new InputValidator('ctl00_contentHolder_txtValues', 0, 300, false, null, '在输入框中用回车换行区分多个选项值'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
