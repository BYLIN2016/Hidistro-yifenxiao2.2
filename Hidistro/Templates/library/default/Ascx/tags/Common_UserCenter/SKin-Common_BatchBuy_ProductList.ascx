<%@ Control Language="C#" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<UI:Grid  ID="grdProducts" DataKeyNames="SkuId" runat="server"  AutoGenerateColumns="false"  GridLines="None" Width="100%" ShowHeader="true"  CssClass="User_manForm" HeaderStyle-CssClass="diplayth1">
   <Columns>     
        <UI:CheckBoxColumn HeaderStyle-CssClass="td_right td_left"/>
        <asp:TemplateField >                       
            <ItemTemplate >
                <Hi:ListImage ID="HiImage1"  runat="server" DataField="ThumbnailUrl40"/>
            </ItemTemplate>
        </asp:TemplateField>                             
       <asp:TemplateField HeaderText="货号">                       
           <ItemTemplate >
             <%#Eval("SKU") %>   
           </ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="商品名称">                       
           <ItemTemplate >
             <%#Eval("ProductName") %>&nbsp;<Hi:SkuContentLabel runat="server" ID="litSkuContent" Text='<%#Eval("SkuId") %>' />
           </ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="购买数量">                       
           <ItemTemplate >
             <asp:TextBox runat="server" ID="txtBuyNum" CssClass="UserInput" Text="1" Width="60" onblur='CheckProductNum(this)'/>
           </ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField  HeaderText="库存">                       
           <ItemTemplate >
             <span class="colorC"><%#Eval("Stock")%></span>          
           </ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="一口价">                       
           <ItemTemplate >
             <span class="colorC"><%#Eval("SalePrice", "{0:F2}")%></span>          
           </ItemTemplate>
       </asp:TemplateField>                 
    </Columns>
</UI:Grid>