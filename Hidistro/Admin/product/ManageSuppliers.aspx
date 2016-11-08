<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageSuppliers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ManageSuppliers" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>供货商管理</h1>
  <span>管理店铺的供货商信息.您可以添加、编辑、删除店铺供货商信息</span>
  </div>
  <div class="btn">
	    <a href="javascript:void(0);" onclick="openAddBox();" class="submit_jia">添加供货商</a>
  </div>
  		<!--结束-->

 <!--数据列表区域-->
  <div class="datalist">
  		<div class="functionHandleArea clearfix">
			<!--分页功能-->
		  <div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
            </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" /></span>
                    </li>
				</ul>
			</div>
		</div>
    <UI:Grid ID="grdSuppliers" DataKeyNames="SupplierName" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
       <Columns>
             <%--<UI:CheckBoxField HeadWidth="35" HeaderStyle-CssClass="td_right td_left" />--%>
             <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value="<%# Eval("SupplierName") %>" />
                            </itemtemplate>
                        </asp:TemplateField>                               
             <asp:TemplateField HeaderText="供货商名称" HeaderStyle-Width="40%" HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                        <span class="Name" id="spCategoryName"><asp:Literal ID="litSupplierName" Text='<%# Eval("SupplierName") %>' runat="server" /></span>
                   </ItemTemplate>
              </asp:TemplateField>
             <asp:TemplateField HeaderText="供货商描述" HeaderStyle-Width="40%" HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                        <span class="Name" id="Span1"><asp:Literal ID="litSupplierDescription" Text='<%# Eval("Remark") %>' runat="server" /></span>
                   </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff">
                    <ItemTemplate>
                        <span class="submit_shanchu">
                            <a href="javascript:void(0);" onclick="openEditBox(this);">编辑</a>
                        </span>
                    </ItemTemplate>
               </asp:TemplateField>
        </Columns>
    </UI:Grid>
  </div>
  <!--数据列表底部功能区域-->
      <div class="bottomBatchHandleArea clearfix">
		 <div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="bottomSignicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete1" runat="server" Text="删除" IsShow="true" /></span>
                    </li>
				</ul>
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
            </div>
			</div>
		</div>
		</div>
</div>


<div id="supplierBox" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90"><em >*</em>&nbsp;供货商名称：</span> <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSupplierName"/>
	          <Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtOldSupplierName" style="display:none;"/></p>
              <b id="ctl00_contentHolder_txtSupplierNameTip">供货商名称为必填项，且长度不能超过20个字符</b>
         <p>
             <span class="frame-span frame-input90">供货商描述：</span><Hi:TrimTextBox runat="server" CssClass="forminput" ID="txtSupplierDescription" Width="300" />
         </p>
         <b id="ctl00_contentHolder_txtSupplierDescriptionTip">供货商描述的长度不能超过500个字符</b>
    </div>
</div>

<div style="display:none">
<asp:CheckBox runat="server" ID="chkAddFlag" />
<asp:Button runat="server" ID="btnOk" Text="确定" CssClass="submit_DAqueding" />
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
    function openAddBox() {
        formtype = "add";
        arrytext = null;
        setArryText('ctl00_contentHolder_txtOldSupplierName', "");
        setArryText('ctl00_contentHolder_txtSupplierName', "");
        setArryText('ctl00_contentHolder_txtSupplierDescription', "");
        $("#ctl00_contentHolder_chkAddFlag").attr("checked", "checked");
        DialogShow("添加供应商", "addsuplier", "supplierBox", "ctl00_contentHolder_btnOk");
  
    }

    function openEditBox(link_obj) {
        formtype = "edite";
        arrytext = null;

        var supplierName = $(link_obj).parents("tr").find("td").eq(1).find("span").text();
        var remark = $(link_obj).parents("tr").find("td").eq(2).find("span").text();
        setArryText('ctl00_contentHolder_txtOldSupplierName', supplierName);
        setArryText('ctl00_contentHolder_txtSupplierName', supplierName);
        setArryText('ctl00_contentHolder_txtSupplierDescription', remark);
        $("#ctl00_contentHolder_chkAddFlag").attr("checked", false);
        DialogShow("编辑供应商信息", "addsuplier", "supplierBox", "ctl00_contentHolder_btnOk");
    }

    function validatorForm() {
        if ($("#ctl00_contentHolder_txtSupplierName").val().length == 0) {
            alert("请填写供货商名称！");
            return false;
        }

        if ($("#ctl00_contentHolder_txtSupplierName").val().length > 20) {
            alert("供货商名称的长度不能超过20个字符！");
            return false;
        }

        if ($("#ctl00_contentHolder_txtSupplierDescription").val().length > 500) {
            alert("供货商描述的长度不能超过500个字符！");
            return false;
        }

        return true;
    }


</script>
</asp:Content>