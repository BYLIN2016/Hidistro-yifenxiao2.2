<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SendMessageToDistributor.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SendMessageToDistributor" EnableSessionState="True"  %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
        function InitValidators()
        {
            initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '标题长度限制在1-60个字符内'))
            initValid(new InputValidator('ctl00_contentHolder_txtContent', 1, 300, false, null, '内容长度限制在1-300个字符内'))
        }
        $(document).ready(function(){ InitValidators(); });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix validator4">
	<div class="columnright">
		<div class="title">
			<em><img src="../images/07.gif" width="32" height="32" /></em>
			<h1>发送站内信给分销商</h1>
			<span>发送站内消息给指定的分销商.</span>
		</div>
		<div class="Emal">
			<ul>
			<li class="colorE"><strong>1.填写消息内容</strong></li>
			<li class="colorQ"><strong>2.发送</strong></li>
			</ul>
		</div>
    <!--搜索-->
    <!--数据列表区域-->
    <div class="formitem">
		<ul>
        	<li><span class="formitemtitle Pw_100">对象：</span>
        	  发送站内信给分销商
        	</li>
            <li><span class="formitemtitle Pw_100"><em >*</em>标题：</span>
              <asp:TextBox id="txtTitle" runat="Server" CssClass="forminput" ></asp:TextBox>
              <p id="txtTitleTip" runat="server" style="margin-left:100px;">标题长度限制在1-60个字符内</p>
            </li>
            <li><span class="formitemtitle Pw_100"><em >*</em>内容：</span>
             <asp:TextBox id="txtContent" Height="120"  TextMode="MultiLine" runat="Server" Width="360"></asp:TextBox>
             <p id="ctl00_contentHolder_txtContentTip"  style="margin-left:100px;">内容长度限制在1-300个字符内</p>
            </li>
        </ul>
	</div>
    <div class="btn Pa_100 clear">
     <asp:Button ID="btnRefer" runat="server" OnClientClick="return PageIsValid();" Text="下一步"  CssClass="submit_DAqueding"/>
    </div>
	</div>
</div>

</asp:Content>
