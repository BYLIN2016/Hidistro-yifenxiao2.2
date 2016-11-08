<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ImageType"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	    <!--面包屑-->
<div class="dataarea mainwidth databody">
	<div class="title">
		<em><img src="../images/01.gif" width="32" height="32" /></em>
		<h1><strong>图片分类管理</strong></h1>
		<span>查看不同图片分类的图片信息 </span>
	</div>
	<div class="datalist">
		<div class="imageDataRight">
			<div class="borderthin">
				<ul class="RightHead">图片分类:</ul>
				 <Hi:ImageTypeLabel runat="server" ID="ImageTypeID" />
			</div>
		</div>
		<div class="imageDataLeft" id="ImageDataList">
			 <UI:Grid ID="ImageTypeList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="CategoryId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
			  <Columns>
				  <asp:TemplateField HeaderText="分类名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
						<ItemTemplate>
						   <Hi:HtmlDecodeTextBox ID="ImageTypeName" runat="server" Text='<%# Bind("CategoryName") %>' CssClass="forminput" />
						</ItemTemplate>
				  </asp:TemplateField>
				   <UI:SortImageColumn HeaderText="排序" ReadOnly="true" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left"/>
				  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
						<ItemTemplate>
							 <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
						</ItemTemplate>
				  </asp:TemplateField>
			  </Columns>
			</UI:Grid>
		</div>
		<div class="btn_bottom"><asp:Button ID="ImageTypeEdit" runat="server" Text="确定修改"  CssClass="submit_DAqueding float" />
			   <asp:Button ID="ImageTypeAdd" runat="server" OnClientClick="ImageType(); return false;" Text="添加分类" CssClass="submit_DAqueding" /></div>
	</div>
</div>	

<!--添加分类-->
<div id="addImageType" class="Pop_up" style="display:none;">
  <h1>添加图片分类</h1>
  <div class="img_datala"><img height="20" width="38" src="../images/icon_dalata.gif"></div>
  <div class="mianform validator2">
   <ul>
    <li>
     <span class="formitemtitle Pw_100"><em>*</em>分类名称：</span>
      <asp:TextBox name="AddImageTypeName" runat="server" Text='<%# Bind("CategoryName") %>' CssClass="forminput" ID="AddImageTypeName" Width="250"></asp:TextBox>
      <p id="contentHolder_AddImageTypeNameTip" class="Pa_100">长度限制在20个字符以内</p>
   </li>
   <li class="clear"></li>
  </ul>
   <ul class="up Pa_100">
   <asp:Button ID="btnSaveImageType" runat="server" OnClientClick="return PageIsValid();" Text="确 定"  CssClass="submit_DAqueding float" />
  </ul>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators()
    {
        initValid(new InputValidator('contentHolder_AddImageTypeName', 1, 20, false, null, '类别名称不能为空，长度限制在20个字符以内'))
    }
    
    $(document).ready(function(){ InitValidators(); });
    
     function ImageType()
     {
 　　    DivWindowOpen(630,200,'addImageType'); 　
     } 
</script>
</asp:Content>