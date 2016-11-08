<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<li>
<div class="icon"><Hi:HiImage ID="HiImage1" runat="server" DataField="IconUrl" alt='<%#Eval("Name") %>'  /></div>
<div class="bd">
<b class="title"><%#Eval("IconUrl").ToString()=="" ? Eval("Name") : ""%> </b>
<asp:Repeater ID="rptHelp" runat="server" DataSource='<%# DataBinder.Eval(Container, "DataItem.Category") %>'>
       <ItemTemplate>
           <div  class="list">
             <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("HelpDetails",Eval("HelpId"))%>' target="_blank"  Title='<%#Eval("Title") %>'>
                <Hi:SubStringLabel ID="SubStringLabel" Field="Title" runat="server"  />
            </a>  
           </div>
      </ItemTemplate>
    </asp:Repeater>
</div>
</li>
 