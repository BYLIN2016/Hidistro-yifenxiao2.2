<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ExpressComputerpes" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title">
		  <em><img src="../images/03.gif" width="32" height="32" /></em>
		  
          <h1><strong>物流公司</strong></h1>
          <span>物流公司中的快递100Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的，请不要随意修改</span>
		</div>
	<!--数据列表区域-->
	<div class="datalist">
			<div class="searcharea clearfix br_search">
      <ul>
        <li> <span>公司名称：</span> <span>
          <asp:TextBox ID="txtcompany" runat="server"></asp:TextBox>
        </span> </li>
        <li><span>快递100Code:</span><span><asp:TextBox ID="txtKuaidi100Code" runat="server"></asp:TextBox></span></li>
        <li><span>淘宝Code：</span><span><asp:TextBox ID="txtTaobaoCode" runat="server"></asp:TextBox></span></li>
        <li>
          <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
        </li>
      </ul>
    </div>
   <div class="functionHandleArea clearfix">
			<!--分页功能-->
			   <div class="pageHandleArea">
				<ul><li><a href="javascript:void(0)" onclick="javascript:ShowAddSKUValueDiv('添加','','','')" class="submit_jia">添加物流公司</a></li></ul>
			</div>			
			<!--结束-->
	  </div>
	<div>
	 <UI:Grid ID="grdExpresscomputors" runat="server" ShowHeader="true" DataKeyNames="Name" AutoGenerateColumns="false" GridLines="None" Width="100%" HeaderStyle-CssClass="border_background">
          <HeaderStyle CssClass="table_title" />
            <Columns>   
                    <asp:TemplateField HeaderText="物流公司"  HeaderStyle-CssClass="td_right">
                        <ItemTemplate><%#Eval("Name") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="快递100Code" HeaderStyle-CssClass="td_right">
                        <ItemTemplate><%# Eval("Kuaidi100Code")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="淘宝Code" HeaderStyle-CssClass="td_right">
                        <ItemTemplate><%# Eval("TaobaoCode")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                        <ItemStyle CssClass="spanD spanN" />
                           <ItemTemplate>
	                           <span class="submit_bianji"><a href="javascript:void(0)" onclick="ShowEditSKUValueDiv('编辑',this)" class="SmallCommonTextButton">编辑</a></span>
	                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" CommandName="Delete" CommandArgument=<%# Eval("Name") %>   IsShow="true" CssClass="SmallCommonTextButton" Text="删除"/></span>
                           </ItemTemplate>
                    </asp:TemplateField>                                         
            </Columns>
        </UI:Grid>
</div>
<div class="blank12 clearfix"></div>
	 <div class="page">
      </div>
      </div>
<!--数据列表底部功能区域-->
</div>  
<div id="divexpresscomputers" style="display: none;">
        <input type="hidden" id="hdcomputers" runat="server" />
        <div class="frame-content">
            <span id="spMsg" style="color:Red; margin-bottom:5px;"></span>
            <p><span class="frame-span frame-input90"><em>*</em>&nbsp;公司名称：</span><asp:TextBox ID="txtAddCmpName" CssClass="forminput" Width="250" runat="server" /></p>
            <p><span  class="frame-span frame-input90"><em>*</em>&nbsp;快递100Code：</span><asp:TextBox ID="txtAddKuaidi100Code" CssClass="forminput" Width="250" runat="server"></asp:TextBox></p>
            <p><span  class="frame-span frame-input90"><em>*</em>&nbsp;淘宝Code：</span><asp:TextBox ID="txtAddTaobaoCode" CssClass="forminput" Width="250" runat="server"></asp:TextBox></p>
        </div>
</div>

<div style="display:none">  <asp:Button ID="btnCreateValue" runat="server" Text="确 定" CssClass="submit_sure"/></div>
<script>

function ShowAddSKUValueDiv(opers, strname, strKuaidi100Code, strTaobaoCode) {

    arrytext = null;
    setArryText('ctl00_contentHolder_hdcomputers',strname);
    setArryText('ctl00_contentHolder_txtAddCmpName', strname);
    setArryText('ctl00_contentHolder_txtAddKuaidi100Code', strKuaidi100Code);
    setArryText('ctl00_contentHolder_txtAddTaobaoCode', strTaobaoCode);

    var Cmptitle = "添加物流公司";
    if (strname != "") {
        $("#spMsg").html("快递100Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的，请不要随意修改！");
        Cmptitle = "编辑"+strname+"物流公司";
    } else {
        $("#spMsg").html("快递100Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的,请填写正确！");
    }

    DialogShow(Cmptitle, 'expresscmp', 'divexpresscomputers', 'ctl00_contentHolder_btnCreateValue');
}
function ShowEditSKUValueDiv(opers, link_obj) {

    arrytext = null;
    var strname = $(link_obj).parents("tr").find("td").eq(0).text();
    var strKuaidi100Code = $(link_obj).parents("tr").find("td").eq(1).text();
    var strTaobaoCode = $(link_obj).parents("tr").find("td").eq(2).text();
    setArryText('ctl00_contentHolder_hdcomputers', strname);
    setArryText('ctl00_contentHolder_txtAddCmpName', strname);
    setArryText('ctl00_contentHolder_txtAddKuaidi100Code', strKuaidi100Code);
    setArryText('ctl00_contentHolder_txtAddTaobaoCode', strTaobaoCode);

    var Cmptitle = "添加物流公司";
    if (strname != "") {
        $("#spMsg").html("快递100Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的，请不要随意修改！");
        Cmptitle = "编辑物流公司";
    } else {
        $("#spMsg").html("快递100Code是物流跟踪所需要的，淘宝Code是同步淘宝发货所需要的,请填写正确！");
    }

    DialogShow(Cmptitle, 'expresscmp', 'divexpresscomputers', 'ctl00_contentHolder_btnCreateValue');
}
function validatorForm() {
   var strname = $("#ctl00_contentHolder_hdcomputers").val().replace(/\s/g,"");
   var cmpname = $("#ctl00_contentHolder_txtAddCmpName").val().replace(/\s/g, "");
   var strKuaidi100Code = $("#ctl00_contentHolder_txtAddKuaidi100Code").val().replace(/\s/g, "");
   var strTaobaoCode = $("#ctl00_contentHolder_txtAddTaobaoCode").val().replace(/\s/g, "");
   if (cmpname == "") {
       alert("物流公司名称不允许为空!");
       return false;
   }
   if (strKuaidi100Code== "") {
       alert("快递100Code不允许为空！");
       return false;
   }
   if (strTaobaoCode== "") {
       alert("淘宝code不允许为空！");
       return false;
   }
   return true;
}
</script> 
</asp:Content>
