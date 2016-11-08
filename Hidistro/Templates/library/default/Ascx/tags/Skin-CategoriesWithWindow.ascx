<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>




<ul>
<asp:Repeater runat="server" ID="recordsone">
     <ItemTemplate>
        <input type="hidden" runat="server" ID="hidMainCategoryId" value='<%#Eval("CategoryId")%>' />	
	    <div class="my_left_cat_list">
		<div id='<%# "twoCategory_" + Eval("CategoryId")%>' class="h2_cat" onmouseover="this.className='h2_cat active_cat'" onmouseout="this.className='h2_cat'" >
		    <h3> <em><asp:Literal ID="Literal1" runat="server" Text='<%#Eval("Notes5")%>'  /></em> <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a> </h3>
		    <div class="h3_cat" id='<%# "threeCategory_" + Eval("CategoryId")%>'>
		         <div class="shadow">
				    <div class="shadow_border">
					    <ul>
					        <span  class="brand" >
                               <h5>品牌推荐：<b><a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("brand")%>'>更多品牌>></a></b></h5>
                               <asp:Repeater runat="server" ID="recordsbrands">
                                    <ItemTemplate>
                                         <a href='<%# Globals.GetSiteUrls().UrlData.FormatUrl("branddetails",Eval("BrandId"))%>'><%# Eval("BrandName")%></a>                           
                                    </ItemTemplate>
                               </asp:Repeater>
                            </span>
                            <span class="category" > 
                               <asp:Repeater runat="server" ID="recordstwo">
                                    <ItemTemplate>
                                         <input type="hidden" runat="server" ID="hidTwoCategoryId" value='<%#Eval("CategoryId")%>' />
                                         <div class="fenlei_jianduan"> <h4> <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a></h4>
                                            <div class="fthree">
                                               <asp:Repeater runat="server" ID="recordsthree">
                                                    <ItemTemplate>
                                              
                                                       <span>  <a href='<%# Globals.GetSiteUrls().SubCategory(Convert.ToInt32(Eval("CategoryId")), Eval("RewriteName")) %>'><%# Eval("Name")%></a>    </span>                        
                                                    </ItemTemplate>
                                               </asp:Repeater>
                                             </div>
                                         </div>
                                     </ItemTemplate>
                               </asp:Repeater>
  			                 </span>
					      </ul>
					    </div>
				     </div>
			</div>		
		</div> 
		</div>   
     </ItemTemplate>
</asp:Repeater>
</ul>



 