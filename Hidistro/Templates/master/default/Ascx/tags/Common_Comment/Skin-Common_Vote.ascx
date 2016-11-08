<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<div class="bMargin">
    <ul class="bPadding10">
        <li><asp:Literal ID="lblVoteName" Text='<%#Eval("VoteName") %>' runat="server"></asp:Literal></li>
        <li>
            <input type="hidden" id='<%# this.NamingContainer.ClientID + Convert.ToString(Eval("VoteId")) + "_Value" %>' />
            <input type="hidden" id='<%# this.NamingContainer.ClientID + Convert.ToString(Eval("VoteId")) + "_Vote" %>'
                value='<%# Eval("VoteId") %>' />
            <input type="hidden" id='<%# this.NamingContainer.ClientID + Convert.ToString(Eval("VoteId")) + "_MaxVote" %>'
                value='<%# Eval("MaxCheck") %>' />
            <asp:DataList runat="server" ID="dlstVoteItems" Visible='<%# Convert.ToInt32(Eval("MaxCheck")) == 1 %>'
                DataSource=' <%# DataBinder.Eval(Container, "DataItem.Vote") %> ' RepeatDirection="Vertical"
                Width="100%">
                <ItemTemplate>
                    <input type="radio" value='<%# Eval("VoteItemId") %>' onclick='document.getElementById(this.name + "_Value").value = this.value;'
                        name='<%# this.NamingContainer.ClientID + Convert.ToString(Eval("VoteId")) %>' /><%# Eval("VoteItemName") %>
                </ItemTemplate>
            </asp:DataList>
            <asp:DataList runat="server" Visible='<%# Convert.ToInt32(Eval("MaxCheck")) > 1 %>'
                ID="DataList1" DataSource='<%# DataBinder.Eval(Container, "DataItem.Vote") %>'
                RepeatDirection="Vertical">
                <ItemTemplate>
                    <input type="checkbox" value='<%# Eval("VoteItemId") %>' onclick='setcheckbox(this)'
                        name='<%# this.NamingContainer.ClientID + Convert.ToString(Eval("VoteId")) %>' /><%# Eval("VoteItemName") %>
                </ItemTemplate>
            </asp:DataList>
           </li>
           </ul>
           
            <input style="padding:0px; margin:0px; border :0px; width:82px; height :25px; " type="button" name='<%# Eval("VoteId") %>' id="btnOK" value="提交/查看" onclick='voteOption(this.name , document.getElementById("<%# this.NamingContainer.ClientID + Convert.ToString(Eval("VoteId"))%>_Value").value);' />
         
 </div>
