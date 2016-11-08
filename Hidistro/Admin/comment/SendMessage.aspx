<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SendMessage" MasterPageFile="~/Admin/Admin.Master" EnableSessionState="True" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">

	<!--选项卡-->
	<div class="optiongroup mainwidth">
	  <ul>
	    <li class="optionstar"><a href="ReceivedMessages.aspx"><span>收件箱</span></a></li>
        <li><a href="SendedMessages.aspx" class="optionnext"><span>发件箱</span></a></li>
        <li class="menucurrent"><a href="javascript:void(0);"><span class="optioncenter">发送站内消息</span></a></li>
      </ul>
</div>
	<div class="dataarea mainwidth">
    <div class="toptitle"> <em><img src="../images/07.gif" width="32" height="32" /></em> <span class="title_height">发送站内消息给指定的会员.</span> </div>
    <div class="Emal">
    <ul>
    <li class="colorE"><strong>1.填写消息内容</strong></li>
    <li class="colorQ"><strong>2.发送</strong></li>
     </ul>
    </div>
    <!--搜索-->
    <!--数据列表区域-->
    <div class="areaform validator2">
		<ul>
        	<li><span class="formitemtitle Pw_100">对象：</span>
        	  发送站内信给会员
        	</li>
            <li><span class="formitemtitle Pw_100"><em >*</em>标题：</span>
              <asp:TextBox id="txtTitle" runat="Server" CssClass="forminput" ></asp:TextBox>
              <p id="txtTitleTip" runat="server">标题长度限制在1-60个字符内</p>
            </li>
            <li><span class="formitemtitle Pw_100"><em >*</em>内容：</span>
             <asp:TextBox id="txtContent" Height="120"  TextMode="MultiLine" runat="Server" Width="360"></asp:TextBox>
             <p id="ctl00_contentHolder_txtContentTip">内容长度限制在1-300个字符内</p>
            </li>
        </ul>
	</div>
    <div class="btn Pa_100 clear">
     <asp:Button ID="btnRefer" runat="server" OnClientClick="return PageIsValid();" Text="下一步"  CssClass="submit_DAqueding"/>
    </div>
</div>
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

