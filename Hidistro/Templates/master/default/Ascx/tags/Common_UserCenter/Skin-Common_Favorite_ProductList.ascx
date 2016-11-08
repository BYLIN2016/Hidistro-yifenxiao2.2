<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.AccountCenter.CodeBehind" Assembly="Hidistro.UI.AccountCenter.CodeBehind" %>

<asp:DataList ID="dtlstFavorite" runat="server" Width="100%" DataKeyField="FavoriteId">
    <ItemTemplate>
              <table style="margin:2em auto;" width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr><td><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("FavoriteId") %>'></td></tr>
                   <tr>                  
                      <td width="23%" rowspan="4" align="center" valign="middle" >
				           <Hi:ProductDetailsLink ID="ProductDetailsLink1" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' ImageLink="true" runat="server">
				           <Hi:ListImage ID="Common_ProductThumbnail1" runat="server" DataField="ThumbnailUrl100" /></Hi:ProductDetailsLink></td>
                      <td style="font-weight:bold;"  class="pad-left-20" >
                           <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' runat="server" /></span>
				      </td>
                      <td width="10%" rowspan="4" align="center" class="blueFontStyle" >
                           <asp:LinkButton ID="lbtnEdit" CommandName="Edit" runat="server" Text="编辑"></asp:LinkButton><br />
                           <Hi:ImageLinkButton runat="server" ID="lbtnDeleted"  Text="删除"  CommandName="Deleted"/>
				      </td>
                   </tr>
                   <tr>
                      <td>
                           <span class="color_red">市场价：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice") %>' runat="server" /></span> 
                           <span>您的价：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("RankPrice") %>' runat="server" /></span> 
                           <span class="color_red">节省：</span>
                           <span class="color_blue"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel3" Money='<%# Math.Abs((Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice"))-(decimal)Eval("RankPrice") )%>' runat="server"></Hi:FormatedMoneyLabel></span>
                      </td>
                   </tr>
                   <tr>
                      <td>
                         <span class="Tags_icon1" style="padding:0 10px 0 20px;">标签</span> 
                             <asp:Label ID="lblTags" runat="server" Text='<%#Bind("Tags") %>'></asp:Label>
					     <div style="clear:both;">
                             <span class="Tags_icon2" style="padding:0 10px 0 20px;">备注</span> 
                             <asp:Label ID="Label1" runat="server" Text='<%#Bind("Remark") %>' CssClass="color_gray" Width="200" style="word-break:break-all" ></asp:Label>
				        </div>
				      </td>
                   </tr>                
               </table>
     </ItemTemplate>
     <EditItemTemplate>
               <table style="margin:2em auto;" width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                  <tr><td><input name="CheckboxGroup" type="checkbox" value='<%#Eval("FavoriteId") %>'></td></td></tr>
                   <tr>
                      <td width="23%" rowspan="4" align="center" valign="middle" >
                           <Hi:ListImage ID="Common_ProductThumbnail2" AutoResize="true" runat="server" DataField="ThumbnailUrl100" CustomToolTip="ProductName" />
				      </td>
                      <td style="font-weight:bold;"  class="pad-left-20" >
                           <Hi:ProductDetailsLink ID="productNavigationDetails" ProductName='<%# Eval("ProductName") %>' ProductId='<%# Eval("ProductId") %>' runat="server" />
				      </td>
                      <td width="10%" rowspan="4" align="center" class="blueFontStyle" >
                           <asp:LinkButton ID="lbtnUpdate" CommandName="Update" runat="server" Text="更新"></asp:LinkButton><br />
                           <asp:LinkButton ID="lbtnCancel" CommandName="Cancel" runat="server" Text="取消"></asp:LinkButton>
				      </td>
                   </tr>
                   <tr>
                      <td>
                           <span class="color_red">市场价：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel1" Money='<%# Eval("MarketPrice") %>' runat="server" /></span> 
                           <span>您的价：<Hi:FormatedMoneyLabel ID="FormatedMoneyLabel2" Money='<%# Eval("RankPrice") %>' runat="server" /></span> 
                           <span class="color_red">节省：</span>
                           <span class="color_blue"><Hi:FormatedMoneyLabel ID="FormatedMoneyLabel3"  Money='<%# Math.Abs((Eval("MarketPrice").ToString()==""?0:(decimal)Eval("MarketPrice"))-(decimal)Eval("RankPrice") )%>' runat="server"></Hi:FormatedMoneyLabel></span></td>
                   </tr>
                   <tr>
                      <td>
                         <span class="Tags_icon1" style="padding:0 10px 0 20px;">标签</span> 
                             <asp:TextBox ID="txtTags" runat="server" Width="200" Text='<%#Globals.HtmlDecode(Eval("Tags").ToString()) %>' class="userInput"></asp:TextBox>
                             
					     <div style="clear:both;">
                             <span class="Tags_icon2" style="padding:0 10px 0 20px;">备注</span> 
                             <asp:TextBox ID="txtRemark" runat="server" Text='<%#Globals.HtmlDecode(Eval("Remark").ToString()) %>' Height="50px" Width="200px" TextMode="MultiLine"></asp:TextBox>
				        </div>
				      </td>
                   </tr>                
               </table>
    </EditItemTemplate>
</asp:DataList>	