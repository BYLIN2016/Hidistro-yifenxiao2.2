<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditSpecificationValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditSpecificationValues" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
    //编辑规格值
    function UpdateAttributeValue(ValueId, ValueStr, ImageUrl, useAttributeImage) {
        var pathurl = "product/SkuValue.aspx?action=update&valueId=" + ValueId + "&useImg="+useAttributeImage;
        var title = "修改规格值";
        if (useAttributeImage == "True") {
            DialogFrame(pathurl, title, 420, 200);
        } else {
            DialogFrame(pathurl, title, 440, 160);
        }
    }
     
    //添加新规格值
     function ShowAddSKUValueDiv(attributeId, useAttributeImage) {
         var pathurl = "product/SkuValue.aspx?action=add&attributeId=" + attributeId + "&useImg=" + useAttributeImage;
         var title = "添加规格值";
         if (useAttributeImage == "True") {
             DialogFrame(pathurl, title, 420, 200);
         } else {
             DialogFrame(pathurl, title, 450, 160);
         }
     }
     
     String.prototype.trim = function(){
return this.replace(/^\s+|\s+$/g, "");//删除前后空格
}
     </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
      <div class="columnright">
	 
    <div class="title"> <em><img src="../images/03.gif" width="32" height="32" /></em>
      <h1>编辑规格值 </h1>
    <span>编辑规格值</span>
   </div>
    <!-- 添加按钮-->
    <div class="btn">
    <a href="javascript:ShowAddSKUValueDiv( '<%=Page.Request.QueryString["AttributeId"]%>','<%=Page.Request.QueryString["UseAttributeImage"]%>');" class="submit_jia">添加新规格值</a>
    </div>
     
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
    
     <div>
	              <UI:Grid ID="grdAttributeValues" runat="server" SortOrderBy="DisplaySequence" SortOrder="desc" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ValueId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="规格值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
		                    <Hi:SKUImage ID="SKUImage1" runat="server" CssClass="a_none" ImageUrl='<%# Eval("ImageUrl")%>' ValueStr='<%# Eval("ValueStr")%>' />
		                      <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="20%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_shanchu">
	                         <asp:LinkButton ID="btnAdd" Text="删除" runat="server"   CommandName="dele" CommandArgument='<%#Eval("ImageUrl") %>' />
	                         </span>
	                         <span> <a href="javascript:UpdateAttributeValue('<%#Eval("ValueId") %>','<%#Eval("ValueStr") %>','<%# Eval("ImageUrl")%>','<%=Page.Request.QueryString["UseAttributeImage"]%>');" >修改</a></span>
                         </ItemTemplate>
                     </asp:TemplateField>          
            </Columns>
        </UI:Grid>
	  </div>
	  </div>
	 </div>
        
<input runat="server" type="hidden" id="currentAttributeId" />

</div>

</asp:Content>

