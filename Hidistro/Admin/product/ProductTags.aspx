<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductTags.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ProductTags" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">

<div class="blank12 clearfix"></div>
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>商品标签管理</h1>
  <span>定义商品所属的各个标签，如果在上架商品时给商品指定了标签，则首页会浏览到各标签中所指定的商品</span></div>
  	
		<!-- 添加按钮-->



   
    
<!--结束-->
		<!--数据列表区域-->
	<div class="datalist">
		<div class="searcharea clearfix br_search">
			<ul>
				<li class="batchHandleButton">
				
				<a href="javascript:void(0)" onclick="ShowTags('add',null,this)" class="submit_jia100">添加标签</a>
				</li>
			</ul>
		</div>
	<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
		<tr class="table_title"><th scope="col">标签名称</th><th class="td_left td_right_fff" scope="col">操作</th>

		</tr>
		<asp:Repeater ID="rp_prducttag" runat="server">
		<ItemTemplate>
		<tr><td><%# Eval("TagName") %></td>
			<td>
			    <span class="submit_shanchu">
			    <a href="javascript:void(0)" onclick="ShowTags('update',<%# Eval("TagID") %>,this)">编辑</a>
			    <asp:LinkButton ID="btndelete" runat="server" CommandName="delete" CommandArgument='<%# Eval("TagID") %>' OnClientClick="return confirm('确认删除该商品标签？');"  Text="删除"></asp:LinkButton>
			    </span>
             </td>
		</tr>
		</ItemTemplate>
		</asp:Repeater>
		
		</table>
	</div>
</div>


<%--修改商品标签名--%>
<div  id="updatetag_div" style="display:none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;标签名称：</span><asp:TextBox ID="txttagname" CssClass="forminput" runat="server" MaxLength="20"/>  </p>
    </div>
</div>

<%--添加商品标签名--%>
<div id="addtag_div" style="display:none">
    <div class="frame-content">
    <p><span class="frame-span frame-input90"><em >*</em>&nbsp;标签名称：</span> <asp:TextBox ID="txtaddtagname" CssClass="forminput" runat="server" MaxLength="20"/>  </p>
    </div>
</div>
<div style="display:none">
<input type="hidden" id="hdtagId" runat="server" />
<asp:Button ID="btnaddtag" runat="server" Text="添 加 商 品 标 签"  CssClass="submit_DAqueding" />
 <asp:Button ID="btnupdatetag" runat="server" Text="修改商品标签"  CssClass="submit_DAqueding"/>
</div>
<script type="text/javascript" language="javascript">
var formtype = "";
function ShowTags(oper, tagId, link_obj) {
    arrytext = null;
    if (oper == "add") {
        formtype = "add";
        setArryText('ctl00_contentHolder_txtaddtagname', "");
        DialogShow("添加商品标签名称", "addtag", "addtag_div", "ctl00_contentHolder_btnaddtag");
    } else {
        formtype = "edite";
        var tagName = $(link_obj).parents("tr").find("td").eq(0).text();
        setArryText('ctl00_contentHolder_hdtagId', tagId);
        setArryText('ctl00_contentHolder_txttagname',tagName);
        DialogShow("编辑商品标签名称", "editetag", "updatetag_div", "ctl00_contentHolder_btnupdatetag");
    }

}

function validatorForm() {
    switch (formtype) {
        case "add":
            if ($("#ctl00_contentHolder_txtaddtagname").val().replace(/\s/g, "") == "") {
                alert("请输入标签名称");
                return false;
            }
            break;
        case "edite":
            if ($("#ctl00_contentHolder_txttagname").val().replace(/\s/g, "") == "") {
                alert("请输入标签名称");
                return false;
            }
            if ($("#ctl00_contentHolder_hdtagId").val().replace(/\s/g, "") == "") {
                alert("请选择要修改的标签名称！");
                return false;
            }
            break;
    };
    return true;
}
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
       
</asp:Content>
