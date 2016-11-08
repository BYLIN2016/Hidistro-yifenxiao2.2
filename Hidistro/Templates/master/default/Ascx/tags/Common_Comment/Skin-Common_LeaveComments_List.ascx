<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<li class="message_list_c1">
    <div class="message_list_ask">
        <b><%# Globals.HtmlDecode(Eval("UserName").ToString() )%> 说：</b> 
		<%# Globals.HtmlDecode(Eval("PublishContent").ToString() )%>
        <div class="message_list_time"><Hi:FormatedTimeLabel Time='<%# Eval("PublishDate") %>' runat="server" /></div>
    </div>
    <asp:Repeater ID="dtlistLeaveCommentsReply" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.LeaveCommentReplays") %>'>
        <ItemTemplate>
            <div class="message_list_answer1"></div>
            <div class="message_list_answer">
                <b>管理员回复：</b> <%# Eval("ReplyContent") %>
                <div class="message_list_time"><Hi:FormatedTimeLabel Time='<%# Eval("ReplyDate") %>' runat="server" /></div>
            </div>
            <div class="message_list_answer2"></div>
        </ItemTemplate>
    </asp:Repeater>
</li>


