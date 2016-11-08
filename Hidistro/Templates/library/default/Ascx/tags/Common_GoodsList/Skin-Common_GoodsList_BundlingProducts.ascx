<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
 <div class="bundlitem clearboth">
     <h3><em>【捆绑促销】</em><%#Eval("name") %></h3>
     <div class="desc"><%#Eval("ShortDescription") %></div>
     
     <div class="bunditem_r">
           <h4>此捆绑促销包括:</h4>
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                           <th width="578" class="bunditem_td" align="left">捆绑商品名称</th> 
                           <th width="70" align="center">数量</th>
                           <th width="130" align="center">单价</th>
                 </tr>
               <asp:Repeater ID="rpbundlingitem"  runat="server">
                   <ItemTemplate>
                       <tr>
                           <td width="578"  class="bunditem_td"><Hi:ProductDetailsLink ID="ProductDetailsLink2" runat="server"  ProductName='<%# Eval("ProductName") %>'  ProductId='<%# Eval("ProductId") %>' ImageLink="false" StringLenth="30"/>
                           <Hi:SkuContentLabel runat="server" ID="litSkuContent" Text='<%#Eval("SkuId") %>' />
                           </td> 
                           <td width="70" align="center"><b><%#Eval("productnum")%></b></td>
                           <td width="130" align="center" ><b>￥<Hi:FormatedMoneyLabel runat="server" Money='<%# Eval("productPrice") %>' /></b></td>
                       </tr>
                   </ItemTemplate>
               </asp:Repeater>
           </table>
     </div>
     
     <div class="bunditem_l">
        
            <ul class="clearboth">    
                  <li><asp:HyperLink ID="hlBuy" runat="server" CssClass="Product_btn_gm" bundingId='<%# Eval("BundlingID") %>' NavigateUrl='<%# "~/SubmmitOrder.aspx?buyAmount=1&Bundlingid="+Eval("BundlingID")+"&from=Bundling"%>'></asp:HyperLink>
                  <%--<a class="Product_btn_gm" id="lbbuy" runat="server" target="_blank" href="SubmmitOrder.aspx?buyAmount=1&Bundlingid=<%# Eval("BundlingID") %>&from=Bundling"></a>--%>
                  <asp:Label Visible="false" runat="server" ID="lbBundlingID" Text='<%# Eval("BundlingID") %>' />
                  </li>
                   <li><em>节省：</em><strong style="color:#0159A0">￥<Hi:FormatedMoneyLabel  runat="server" ID="lblsaving" /></strong></li>
                   <li><em>捆绑价：</em><strong class="bundlprice">￥<Hi:FormatedMoneyLabel runat="server" ID="lblbundlingPrice" Money='<%# Eval("Price") %>' /></strong> </li>
                     <li><em>原价：</em><strong class="markprice">￥<Hi:FormatedMoneyLabel runat="server" ID="lbltotalPrice"  /></strong></li>                                   
               </ul>
         
     </div>
     
     
 </div>
