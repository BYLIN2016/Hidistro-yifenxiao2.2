<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.HelpCategories" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
            <li class="optionstar"><a href="HelpList.aspx" class="optionnext"><span>帮助管理</span></a></li>
            <li class="menucurrent"><a href="HelpCategories.aspx" class="optioncurrentend"><span class="optioncenter">帮助分类管理</span></a></li>
		</ul>
	</div>
	<!--选项卡-->
	<div class="dataarea mainwidth">
		<!--搜索-->
		 <div class="Pa_15">
          <a href="javascript:DialogFrame('comment/AddHelpCategory.aspx?source=add','添加帮助分类',null,null)" class="submit_jia">添加帮助分类</a></li>
		 </div>
		 <div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allOrder"><asp:LinkButton OnClientClick="return CheckOrderNumber()" ID="btnorder" runat="server">批量保存排序</asp:LinkButton> </span>
					</li>
				</ul>
			</div>		
			<div class="clear"></div>
		<!--数据列表区域-->
	  <div class="datalist">
     <UI:Grid ID="grdHelpCategories" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="CategoryId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                <asp:TemplateField HeaderText="分类名称"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <Hi:HiImage ID="HiImage1" runat="server" DataField="IconUrl"  CssClass="Img100_30" />
                      <asp:Literal id="lblCategoryName" runat="server" Text='<%#Eval("Name") %>'></asp:Literal>
                      <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
                   </ItemTemplate>
                </asp:TemplateField>            
                <UI:SortImageColumn  HeaderText="分类排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/> 
                  <asp:TemplateField HeaderText="显示顺序"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <input type="text" runat="server" value='<%# Eval("DisplaySequence") %>' style="width:60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                   </ItemTemplate>
                </asp:TemplateField>                
                <UI:YesNoImageColumn DataField="IsShowFooter" HeaderText="显示在底部帮助" HeaderStyle-CssClass="td_right td_left"/>                
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                    <ItemTemplate>
                    <span class="submit_bianji"><a  href="javascript:DialogFrame('<%# "comment/EditHelpCategory.aspx?CategoryId="+ Eval("CategoryId") %>','编辑文章分类',null,null)">编辑</a></span>                    
                    <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkDelete" Text="删除" IsShow="true"  CommandName="Delete" runat="server" /></span></span>                      
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
      <div class="blank5 clearfix"></div>
	  </div>
	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>   

<script>
function CheckOrderNumber(){
    var reg=/^[0-9]*[1-9][0-9]*$/;
    tag=true;
    $(".datalist input[type='text']").each(function(index,item){
        if(!reg.test($(this).val().replace(/\s/g,""))){
           alert("排序值不允许为非负数！");
           tag=false; 
        }
    });
    return tag;
}
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

