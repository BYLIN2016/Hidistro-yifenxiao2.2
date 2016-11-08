<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditAttributeValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.EditAttributeValues" %>
 <%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
   <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1><table><tr><td>编辑扩展属性</td><td></td></tr></table></h1>
  <span>可以添加或修改扩展属性值。</span></div>
    
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
      	<div class="searcharea clearfix br_search">
			<ul>
				<li class="batchHandleButton"><input type="button" name="button" id="button1" value="添加属性值" class="submit_bnt3 " onclick="AddAttributeValue();"/></li>
			</ul>
		</div>
     
	              <UI:Grid ID="grdAttributeValues" runat="server" SortOrderBy="DisplaySequence" SortOrder="desc" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ValueId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="属性值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                        <ItemTemplate>
		                    <asp:Label ID="lblAttributeName" runat="server" Text='<%# Eval("ValueStr") %>'></asp:Label>
		                      <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="20%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete" CssClass="SmallCommonTextButton" runat="server" IsShow="true" CommandName="Delete"  Text="删除"  /></span>
	                         <span>    <a href="javascript:UpdateAttributeValue('<%#Eval("ValueId") %>','<%#Eval("ValueStr") %>');" >修改</a></span>
                         </ItemTemplate>
                     </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
	  </div>

</div>

 
 
<%--添加属性值--%>
<div  id="addAttributeValue" style="display:none;">
    <div class="frame-content">
        <p><span>属性值：<em>*</em></span> <asp:TextBox ID="txtValue" runat="server" Width="300" ></asp:TextBox></p>
        <b>扩展属性的值，字符数最多15个字符</b>
    </div>
</div>

<%--修改属性值--%>
<div id="updateAttributeValue" style=" display:none;">
    <div class="frame-content">
        <p><span>属性值：<em>*</em></span> <asp:TextBox ID="txtOldValue" runat="server" Width="300"></asp:TextBox></p>
        <b>扩展属性的值，字符数最多15个字符</b>
    </div>
    <div class="clear"></div>
</div>
 <div style="display:none">
 <asp:Button ID="btnCreate" runat="server" Text="添加"  CssClass="submit_DAqueding" />
 <asp:Button ID="btnUpdate" runat="server" Text="修改" CssClass="submit_DAqueding" />
 <input type="hidden" id="hidvalueId" runat="server" />
 <input type="hidden" id="hidvalue" runat="server" />
</div>
<script>
var formtype = "";
function AddAttributeValue() {
    formtype = "add";
    arrytext = null;
    setArryText('ctl00_contentHolder_txtValue', "");

    DialogShow('添加属性值名称', 'attributevalue', 'addAttributeValue', 'ctl00_contentHolder_btnCreate'); 
}


function UpdateAttributeValue(ValueId, ValueStr) {
    formtype = "edite";
    arrytext = null;
    setArryText('ctl00_contentHolder_hidvalueId', ValueId);
    setArryText('ctl00_contentHolder_txtOldValue', ValueStr);
    setArryText('ctl00_contentHolder_hidvalue', ValueStr);

    DialogShow('修改扩展属性值', 'attributevalue', 'updateAttributeValue', 'ctl00_contentHolder_btnUpdate');

}

function validatorForm() {
    if (formtype == "add") {
        if ($("#ctl00_contentHolder_txtValue").val() == "") {
            alert("请输入属性值名称");
            return false;
        }
        return true;
    }
    else {
        if ($("#ctl00_contentHolder_txtOldValue").val() == "") {
            alert("扩展属性值不允许为空");
            return false;
        }
        return true;
    }
}
</script>
</asp:Content>
