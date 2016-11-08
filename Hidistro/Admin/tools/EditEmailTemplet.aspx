<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditEmailTemplet" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">

      <div class="columnright">
          <div class="title ">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑邮件模板</h1>
            <span>邮件模板提供系统生成邮件所需的格式和样式，您可以结合每个模板提供的标签自行修改邮件模板</span>
          </div>
      <div class="formitem validator2">
        <ul>
  		<li> <span class="formitemtitle Pw_100">消息类型：</span>
          <abbr class="formselect">
            <asp:Label ID="litEmailType" runat="server"></asp:Label>
          </abbr>
        </li>
        <li class="clearfix"> <span class="formitemtitle Pw_100">标签说明：</span>
         <span><asp:Literal ID="litEmailDescription" runat="server"></asp:Literal></span>
        </li>
        <li><span class="formitemtitle Pw_100">邮件主题：</span>
          <asp:TextBox ID="txtEmailSubject" runat="server" Width="300px" CssClass="forminput" ></asp:TextBox>
		  <p id="txtEmailSubjectTip" runat="server"></p>
        </li>
        <li> <span class="formitemtitle Pw_100">邮件内容：</span>
          <span><Kindeditor:KindeditorControl id="fcContent" runat="server" Width="98%"  height="300px" /></span>  
        </li>
        <li><p>邮件內容不能为空，长度限不能超过4000个字符</p></li>
       <li></li>
      </ul>
      </div>
      <div class="btn Pa_140 clear"><asp:Button ID="btnSaveEmailTemplet" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding inbnt"/></div>

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
    function InitValidators()
     {
       
        initValid(new InputValidator('ctl00_contentHolder_txtEmailSubject', 1,60, false, null, '邮件主题不能为空，长度限制在1-60个字符之间'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>

