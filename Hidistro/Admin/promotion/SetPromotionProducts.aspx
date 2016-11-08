<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SetPromotionProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SetPromotionProducts" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    function ShowAddDiv()
    {
        var activId = $("#ctl00_contentHolder_hdactivy").val().replace(/\s/g, "");
        if(activId!=""&&parseInt(activId)>0){
            DialogFrame("promotion/SearchPromotionProduct.aspx?activityId=" + activId, "添加促销商品", 975, null);
        }
        
     
     
    }

  
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<input type="hidden" id="hdactivy" runat="server" />
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>促销活动“<asp:Literal runat="server" ID="litPromotionName" />“包括的商品 </h1>
	    <span>每件商品只能参加其中一个促销活动，如果一件商品在别的促销活动中已经被选，这些将不再参与</span>
     </div>       
	  <!--结束-->
    <div class="btn">
           <div style="float:right; margin-right:15px;" class="delete"><Hi:ImageLinkButton ID="btnDeleteAll"  runat="server" Text="清空" IsShow="true" DeleteMsg="确定要清空这些促销商品吗？" /></div>
       <a class="submit_jia" href="javascript:void(0)" onclick="ShowAddDiv()">添加促销商品</a>
    </div>
	  <!--数据列表区域-->
  <div class="datalist">
	    <UI:Grid ID="grdPromotionProducts" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ProductId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns> 
                    <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left" HeaderStyle-Width="60%">
                        <ItemTemplate>
		                    <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">商家编码：<%# Eval("ProductCode") %></span>
                                 </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="库存"  ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                    </asp:TemplateField>
                    <Hi:MoneyColumnForAdmin HeaderText=" 市场价" ItemStyle-Width="10%"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left"  />       
                    <Hi:MoneyColumnForAdmin HeaderText="一口价" ItemStyle-Width="10%"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />        
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" HeaderStyle-Width="10%">
                        <ItemStyle />
                        <itemtemplate> 
			                <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" ID="Delete" Text="删除" IsShow="true" CommandName="Delete" ></Hi:ImageLinkButton></span>
                        </itemtemplate>
                    </asp:TemplateField>
            </Columns>
        </UI:Grid>
    </div>
  <div style="padding:0px 0px 13px 15px">
        <div style="margin:0 auto;width:200px;"><asp:LinkButton ID="btnFinesh" runat="server" Text="　　完　成" CssClass="submit_DAqueding inbnt"></asp:LinkButton></div>
  </div>
  </div>
</asp:Content>
