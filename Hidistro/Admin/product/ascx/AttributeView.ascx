<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeView.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.product.ascx.AttributeView" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="content">
    <UI:Grid ID="grdAttribute" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="AttributeId" HeaderStyle-CssClass="table_title" GridLines="None"
        Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="属性名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%">
                <ItemTemplate>
                    <Hi:HtmlDecodeTextBox ID="txtAttributeName" runat="server" Text='<%# Eval("AttributeName") %>'
                        Width="70px"></Hi:HtmlDecodeTextBox>
                    <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>'
                        Visible="false"></asp:Literal>
                    <asp:LinkButton ID="lbtnSave" Text="修改" runat="server" CommandName="saveAttributeName" />
                </ItemTemplate>
            </asp:TemplateField>
            <UI:YesNoImageColumn DataField="IsMultiView"  ItemStyle-Width="9%" HeaderText="支持多选" HeaderStyle-CssClass="td_right td_left" />
            <asp:TemplateField HeaderText="属性值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="45%">
                <ItemTemplate>
                    <asp:Repeater ID="rptSKUValue" runat="server" DataSource='<%# Eval("AttributeValues") %>'>
                        <ItemTemplate>
                            <span class="SKUValue"><span class="span1">
                                <asp:HyperLink ID="HyperLink1" runat="server"><%# Eval("ValueStr")%></asp:HyperLink></span>
                                <span class="span2"><a href="javascript:deleteAttributeValue(this,'<%# Eval("ValueId")%>');">
                                    删除</a></span> </span>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>
            <UI:SortImageColumn HeaderText="排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"
                ItemStyle-Width="7%" />
            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                <ItemStyle CssClass="spanD spanN" />
                <ItemTemplate>
                    <span class="submit_tiajia"><a href="javascript:void(0)" onclick="ShowAddSKUValueDiv('<%# Eval("AttributeId") %>','<%# Eval("AttributeName") %>');">
                        添加属性值</a></span> <span class="submit_bianji">
                            <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%#Globals.GetAdminAbsolutePath(string.Format("product/EditAttributeValues.aspx?TypeId={0}&AttributeId={1}",Eval("TypeId"),Eval("AttributeId")))%>'></asp:HyperLink></span>
                    <span class="submit_shanchu">
                        <Hi:ImageLinkButton ID="lkbDelete" CssClass="SmallCommonTextButton" runat="server"
                            IsShow="true" CommandName="Delete" Text="删除" /></span>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </UI:Grid>
</div>
<div class="Pg_15">
    <input type="button" name="button" id="button" value="添加扩展属性" class="submit_bnt3 "
        onclick="ShowAddKzAtribute();" />
</div>

  <%--添加扩展属性--%>
<div id="addAttribute" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">属性值名：<em>*</em></span><asp:TextBox ID="txtName" CssClass="forminput" runat="server"></asp:TextBox></p>
        <b id="ctl00_contentHolder_attributeView_txtNameTip">扩展属性的名称，最多15个字符。</b>
        <span class="frame-span frame-input90">是否支持多选：</span><asp:CheckBox ID="chkMulti_copy" Text="支持多选" runat="server" onclick="javascript:SetMultSate(this)" Checked="true"/>(有些属性是可以选择多个属性值的，如“适合人群”，就可能既适合老年人也适合中年人)

        <p><span class="frame-span frame-input90">属性值：<em>*</em></span><asp:TextBox ID="txtValues" runat="server" Width="300" CssClass="forminput"></asp:TextBox></p>
        <b> 扩展属性的值，多个属性值可用“,”号隔开，每个值的字符数最多15个字符</b>
    </div>
</div>

<%--添加属性值--%>
<div id="addAttributeValue" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">属性值名：<em>*</em></span><asp:TextBox ID="txtValueStr" CssClass="forminput" Width="300" runat="server" /></p>
        <b> 多个规格值可用“,”号隔开，每个值的字符数最多15个字符</b>
    </div>
</div>
<div style="display:none">
 <asp:Button ID="btnCreateValue" runat="server" Text="添加属性值" CssClass="submit_DAqueding" />
<asp:Button ID="btnCreate" runat="server" Text="添加扩展属性"  CssClass="submit_DAqueding" />
<asp:CheckBox ID="chkMulti" Text="支持多选"  runat="server" Checked="true" />
<input runat="server" type="hidden" id="currentAttributeId" />
</div>



<script type="text/javascript" language="javascript">

    function SetMultSate(multiobj) {
        if (multiobj.checked) {
            $("#ctl00_contentHolder_attributeView_chkMulti").attr("checked", true);
        } else {
            $("#ctl00_contentHolder_attributeView_chkMulti").attr("checked", false);
        }
    }
//判断规格值
String.prototype.trim = function(){
return this.replace(/^\s+|\s+$/g, "");//删除前后空格
}


var formtype = "";
function ShowAddSKUValueDiv(attributeId,attributename){
        formtype = "addvalue";
        arrytext = null;
        setArryText('ctl00_contentHolder_attributeView_currentAttributeId', attributeId);
        setArryText('ctl00_contentHolder_attributeView_txtValueStr', "");
        DialogShow("添加" + attributename + "的属性值", "addskuvalue", "addAttributeValue", "ctl00_contentHolder_attributeView_btnCreateValue");
    }


    //添加扩展属性
function ShowAddKzAtribute() {
    formtype = "add";
    arrytext = null;
    setArryText('ctl00_contentHolder_attributeView_txtName', "");
    setArryText('ctl00_contentHolder_attributeView_txtValues',"");

    DialogShow("添加扩展属性", "addattri", "addAttribute", "ctl00_contentHolder_attributeView_btnCreate");
}


function validatorForm() {
    if (formtype == "addvalue") {
        if ($("#ctl00_contentHolder_attributeView_txtValueStr").val().replace(/\s/g, "") == "") {
            alert("请输入属性值！");
            return false;
        }
        if ($("#ctl00_contentHolder_attributeView_currentAttributeId").val().replace(/\s/g, "")=="") {
            alert("请选择要添加值的属性");
            return false;
        }
    } else {
        if ($("#ctl00_contentHolder_attributeView_txtName").val() == "") {
            alert("请输入扩展属性名称");
            return false;
        }
        if ($("#ctl00_contentHolder_attributeView_txtValues").val() == "") {
            alert("请输入扩展属性值");
            return false;
        }
        
    }
    return true;
}
function deleteAttributeValue(obj, valueId) {
    $.ajax({
        url: "AddSpecification.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { ValueId: valueId,isCallback: "true" },
        async: false,
        success: function(data)
        {
            if (data.Status == "true")
            {
                location.reload();                    
            }
            else {
                ShowMsg("此属性值有商品在使用，删除失败", false);
            }
        }
    });
}
</script>

