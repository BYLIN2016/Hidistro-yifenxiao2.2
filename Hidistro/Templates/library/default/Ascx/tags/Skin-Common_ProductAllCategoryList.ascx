<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>

<li>
	<h2><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></h2>
    <ul>		
       <asp:Repeater runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.One") %>' >
            <ItemTemplate>	
            <li>
                <b><a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>£º</b>	
                <div>	
                <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Two") %>' >
                    <ItemTemplate>	
                        <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a><span>|</span>
                    </ItemTemplate>
                </asp:Repeater>
                 </div>
              </li>
          </ItemTemplate>
        </asp:Repeater>
     </ul>
</li>