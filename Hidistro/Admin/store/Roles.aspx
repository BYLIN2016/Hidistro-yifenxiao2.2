<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Roles" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

	<div class="dataarea mainwidth databody">
     <div class="title">
		  <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1><strong>部门管理</strong></h1>
          <span>如果需要为店铺添加多个管理员，则可以在划分部门以后，将管理员加入到各自的部门</span>
	</div>
    <!--数据列表区域-->
    <div class="datalist">
      <!--搜索-->
    <div class="functionHandleArea">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li><a class="submit_jia"  href="javascript:void(0)" onclick="javascript:AddNewRoles()" >添加新部门</a></li>
        </ul>
      </div>
      <!--结束-->
    <input runat="server" type="hidden" id="txtRoleId" />
    <input runat="server" type="hidden" id="txtRoleName" />
    </div>
    <UI:Grid ID="grdGroupList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="RoleId"  GridLines="None" HeaderStyle-CssClass="table_title" Width="100%">    
                       <Columns>
                           
                            <asp:TemplateField HeaderText="部门名称" HeaderStyle-CssClass="td_right td_left">
                               <ItemTemplate>
		                              <asp:Label ID="lblRoleName" Text='<%#Eval("Name")%>' runat="server" />
                               </ItemTemplate>                                                                                    
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="职能说明" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                                  <ItemTemplate>
                                    <div style="word-break:break-all;" ><asp:Literal ID="lblRoleDesc" Text='<%#Eval("Description") %>' runat="server"></asp:Literal></div>
                                  </ItemTemplate>                                 
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="操作" ItemStyle-Width="35%" HeaderStyle-CssClass="td_left td_right_fff">
                                 <ItemTemplate>
			                          <span class="submit_tiajia"><a href='<%# Globals.GetAdminAbsolutePath("/store/RolePermissions.aspx?roleId=" + Eval("RoleID"))%> ' >部门权限</a></span>
			                          <span class="submit_bianji"><a href="javascript:ShowEditDiv('<%# Eval("RoleId")%>','<%# Eval("Name")%>','<%#  Eval("Description")%>');">编辑</a></span>
			                          <span class="submit_shanchu"><Hi:ImageLinkButton  runat="server" IsShow="true" ID="DeleteImageLinkButton1" CommandName="Delete" Text="删除"/></span>
                                 </ItemTemplate>                               
                             </asp:TemplateField>  
                      </Columns>
                </UI:Grid>
                
     
      <div class="blank5 clearfix"></div>
    </div>
    <!--数据列表底部功能区域-->
    
    </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  
    <!--添加部门-->
    <div id="divaddroles" style="display: none;">
        <div class="frame-content">
            <p><span class="frame-span frame-input90"><em >*</em>&nbsp;部门名称：</span><asp:TextBox ID="txtAddRoleName" runat="server"></asp:TextBox></p>
            <b id="ctl00_contentHolder_txtAddRoleNameTip">部门名称不能为空,长度限制在60个字符以内</b>
            <p><span class="frame-span frame-input90">职能说明：</span><asp:TextBox ID="txtRoleDesc" runat="server"></asp:TextBox></p>
            <b id="ctl00_contentHolder_txtRoleDescTip">说明部门具备哪些职能，长度限制在100个字符以内</b>
        </div>
    </div>

<!--编辑部门-->
<div id="EditRole" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">部门名称：<em >*</em></span><asp:TextBox ID="txtEditRoleName" runat="server" CssClass="forminput"></asp:TextBox></p>
        <b id="ctl00_contentHolder_txtEditRoleNameTip">部门名称不能为空,长度限制在60个字符以内</b>
        <p><span class="frame-span frame-input90">职能说明：</span> <asp:TextBox ID="txtEditRoleDesc" runat="server" CssClass="forminput"></asp:TextBox></p>
        <b id="ctl00_contentHolder_txtEditRoleDescTip">说明部门具备哪些职能，长度限制在100个字符以内</b>
    </div>
</div>
<div style="display:none">
<asp:Button ID="btnEditRoles" runat="server" Text="编辑部门"  CssClass="submit_sure"/>
<asp:Button ID="btnSubmitRoles" runat="server" Text="添加部门"/>
</div>
<script>
  var  formtype = "";



   function AddNewRoles() {
       arrytext = null;
       formtype = "add";
       setArryText('ctl00_contentHolder_txtAddRoleName', "");
       setArryText('ctl00_contentHolder_txtRoleDesc', "");
       DialogShow('添加部门', 'rolesetcmp', 'divaddroles', 'ctl00_contentHolder_btnSubmitRoles');
   }

  
   function ShowEditDiv(roleId, name, description) {
        arrytext = null;
        formtype = "edite";
        setArryText('ctl00_contentHolder_txtRoleId', roleId);
        setArryText('ctl00_contentHolder_txtRoleName', name);
        setArryText('ctl00_contentHolder_txtEditRoleName', name);
        setArryText('ctl00_contentHolder_txtEditRoleDesc', description);
        DialogShow('修改部门', 'rolesetcmp', 'EditRole', 'ctl00_contentHolder_btnEditRoles');
    }

    function validatorForm() {
        //InitValidators();
        if (formtype == "add") {
            var rolename = $("#ctl00_contentHolder_txtAddRoleName").val().replace(/\s/g, "");
            var roledes = $("#ctl00_contentHolder_txtRoleDesc").val().replace(/\s/g, "");
            if (rolename == "" || rolename.length < 1 || rolename.length > 60) {
                alert("部门名称不能为空,长度限制在60个字符以内");
                return false;
            }
            if (roledes != "" && roledes.length > 100) {
                alert("职能说明的长度限制在100个字符以内");
                return false;
            }
        } else {
            var rolieId = $("#ctl00_contentHolder_txtRoleId").val();
            var rolename = $("#ctl00_contentHolder_txtEditRoleName").val();
            var roledes = $("#ctl00_contentHolder_txtEditRoleDesc").val();
            if (rolename == "" || rolename.length < 1 || rolename.length > 60) {
                alert("部门名称不能为空,长度限制在60个字符以内");
                return false;
            }
            if (roledes != "" && roledes.length > 100) {
                alert("职能说明的长度限制在100个字符以内");
                return false;
            }
        }
        return true;
    }
</script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>