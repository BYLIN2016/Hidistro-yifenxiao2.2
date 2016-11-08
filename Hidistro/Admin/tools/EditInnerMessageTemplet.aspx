<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditInnerMessageTemplet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditInnerMessageTemplet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">

      <div class="columnright">
          <div class="title ">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑站内信模板</h1>
            <span>根据标签说明调整模板的内容，站内信模板只支持系统列出来的标签</span>
          </div>
      <div class="formitem  validator2">
      <ul>  		
        <li> <span class="formitemtitle Pw_100">消息类型：</span>
          <abbr class="formselect">
            <asp:Label ID="litEmailType" runat="server"></asp:Label>
          </abbr>
        </li>
        <li class="clearfix"> <span class="formitemtitle Pw_100">标签说明：</span>
         <span><asp:Literal ID="litTagDescription" runat="server"></asp:Literal></span>
        </li>
        <li class="clearfix"><span class="formitemtitle Pw_100">站内信标题：</span><asp:TextBox ID="txtMessageSubject" Width="300px" runat="server" CssClass="forminput" ></asp:TextBox>
         <p id="txtMessageSubjectTip" runat="server">站内信标题不能为空，长度限制在1-60个字符之间</p></li>        
         <li><span class="formitemtitle Pw_100">内容模板：</span>
          <asp:TextBox TextMode="MultiLine" ID="txtContent" runat="server" Width="500px" height="200px" ></asp:TextBox><br />
		  <p id="txtContentTip" runat="server">内容不能为空，长度限制在1-300个字符之间</p>
        </li>
      <li></li>
    </ul>     
     </div>
      <div class="btn Pa_140 clear"><asp:Button ID="btnSaveMessageTemplet" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"/></div>
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
    initValid(new InputValidator('ctl00_contentHolder_txtMessageSubject', 1, 60, false, null, '站内信标题不能为空，长度限制在1-60个字符之间'))
    initValid(new InputValidator('ctl00_contentHolder_txtContent', 1, 300, false, null, '内容不能为空，长度限制在1-300个字符之间'))

    }
    $(document).ready(function() { InitValidators(); });
    </script>
</asp:Content>
