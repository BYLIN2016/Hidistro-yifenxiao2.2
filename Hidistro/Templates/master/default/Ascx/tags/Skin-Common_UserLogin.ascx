<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Membership.Context" %>
<%@ Import Namespace="Hidistro.Core" %>

<script type="text/javascript">
    $(document).ready(function() {
        $('#btn_Common_Login_Button').click(function() {
            var username = $("#txt_Common_Login_UserName").val();
            var password = $("#txt_Common_Login_Password").val();

            if (username.length == 0 || password.length == 0) {
                alert("�����������û���������!");
                return;
            }

            $.ajax({
                url: "Login.aspx",
                type: "post",
                dataType: "json",
                timeout: 10000,
                data: { username: username, password: password, action: "Common_UserLogin" },
                async: false,
                success: function(data) {
                    if (data.Status == "Succes") {
                        window.location.reload();
                    }
                    else {
                        alert(data.Msg);
                    }
                }
            });
        });

        $("#txt_Common_Login_Password").keydown(function(e) {
            if (e.keyCode == 13) {
                $('#btn_Common_Login_Button').focus();
                $('#btn_Common_Login_Button').click(function() { });
            }
        });

        $("#txt_Common_Login_UserName").keydown(function(e) {
            if (e.keyCode == 13) {
                $('#btn_Common_Login_Button').focus();
                $('#btn_Common_Login_Button').click(function() { });
            }
        });

    });
</script>
<asp:Panel runat="server" ID="pnlLogin">
	 <ul class="Default_Login_mar"> �û�����<input id="txt_Common_Login_UserName" type="text" class="Default_Input" style="width:96px;"/></ul>
     <ul class="Default_Login_mar">�ܡ��룺<input name="Common_Login_Password" type="password" id="txt_Common_Login_Password" style="width:96px;"  class="Default_Input" /></ul>
     <ul class="Default_Login_pad5">
        <li class="Default_Login_bg">
            <a href="#" class="cWhite" id="btn_Common_Login_Button" type="button"  >��¼</a> /
            <a href="#" class="cWhite" onclick='<%= "top.location.href=\"" + Globals.GetSiteUrls().UrlData.FormatUrl("register") + "\"" %>'>ע��</a>
         </li>
         <li class="Default_Login_fwbg lCenter">
            <a href='<%= Globals.GetSiteUrls().UrlData.FormatUrl("ForgotPassword")%>' class="cWhite" target="_blank">�������룿</a>
        </li>
     </ul>
</asp:Panel>
<asp:Panel runat="server" ID="pnlLogout">
   <div style="margin-left:17px">
     <ul>���ã�<span style="color: #36c;"><%= HiContext.Current.User.Username %></span> (<asp:Literal ID="litMemberGrade" runat="server" />)</ul>
     <ul >Ԥ������<asp:Literal ID="litAccount" runat="server"></asp:Literal></ul>
     <ul>���û��֣�<asp:Literal ID="litPoint" runat="server" /></ul>
     <ul >δ��վ���ţ�<asp:Literal ID="litNum" runat="server"></asp:Literal></ul>
     <ul>
         <span id="Span1"><Hi:Common_MyAccountLink ID="Common_Link_MyAccount1" runat="server" Target="_parent" /></span>
         <span id="Span2"><a href='<%= Globals.GetSiteUrls().UrlData.FormatUrl("logout") %>' target="_parent">�˳�</a></span>
     </ul>
   </div>
</asp:Panel>

