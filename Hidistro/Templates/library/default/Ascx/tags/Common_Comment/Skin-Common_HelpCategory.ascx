<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<h4><%#Eval("Name")%></h4>
<ul>
    <asp:Repeater ID="rp_title" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Category") %>'>
        <ItemTemplate>
	           <li  id="dd_<%#Eval("HelpId") %>"><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails",Eval("HelpId"))%>'><%# Eval("Title")%></a></li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<script>
var helpurl=document.location.href;
if(helpurl.indexOf("show")>=0){
    var str2=helpurl.substr(helpurl.lastIndexOf("-")+1);
    var dd_id =str2.substring(0,str2.lastIndexOf("."));
    $("#dd_"+dd_id).attr("class","d_select");
}

</script>