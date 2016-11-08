<%@ Control Language="C#"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>


<tr>
    <td width="150" style="padding-left:20px;"><asp:Label ID="lblVoteItemName" runat="server" Text='<%# Eval("VoteItemName") %>'></asp:Label></td>
    <td width="610">
      <div class="votefacebg">
        <div style="clear"><Hi:ThemeImage  src="images/sub/voteadd.jpg"   runat="server"/><Hi:ThemeImage ID="themesImg5" runat="server" Src ="images//process/voteface.jpg" style='<%# string.Format("height:15px;width:{0}px;overflow:hidden;", Eval("Lenth")) %>'/><Hi:ThemeImage  src="images/sub/voteaddr.jpg"   runat="server"/></div>
      </div>
    </td>  
    <td width="100" align="center"><asp:Label ID="lblPercentage" runat="server" Text='<%# Eval("Percentage", "{0:N2}") %>' ></asp:Label>%</td>
    <td width="100" align="center"><asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemCount") %>' ></asp:Label></td>
</tr>
 
