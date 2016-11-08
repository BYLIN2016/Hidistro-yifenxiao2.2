<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageHotKeywords.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageHotKeywords" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
  <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>热门关键字管理</h1>
     <span>设置前台显示的热门关键字</span></div>
    <!-- 添加按钮-->
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
    	<div class="searcharea clearfix br_search">
			<ul><li>
                <a class="submit_jia" href="javascript:AddHotKeyDiv();" >添加热门关键字</a>
                 <input runat="server" type="hidden" id="txtHid" />
                </li>
             </ul>
		</div>
      <UI:Grid runat="server" AutoGenerateColumns="False" ID="grdHotKeywords" ShowHeader="true" DataKeyNames="Hid" GridLines="None" HeaderStyle-CssClass="table_title" AllowSorting="true"
                SortOrderBy="Frequency" SortOrder="DESC" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="关键字" HeaderStyle-CssClass="td_right td_left">
	                    <ItemTemplate>
	                        <asp:Literal ID="litName" runat="server" Text='<%# Eval("Keywords") %>' />
	                        <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("Frequency") %>' Visible="false"></asp:Literal>
	                    </ItemTemplate>	                    
	                </asp:TemplateField>     
	                <asp:BoundField DataField="CategoryName" HeaderText="所属商品分类" HeaderStyle-CssClass="td_right td_left" />       
                    <UI:SortImageColumn HeaderText="显示顺序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left" />
                     <asp:TemplateField HeaderText="操作" ItemStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemTemplate>
                              <span class="submit_bianji"><a href="javascript:ShowEditDiv('<%# Eval("Hid")%>', '<%# Eval("CategoryId")%>','<%# Eval("Keywords") %>');" id="dd" >编辑</a></span>
	                          <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" /></span>
                         </ItemTemplate>
                     </asp:TemplateField>    
                </Columns>
            </UI:Grid>     
    </div>
    <!--数据列表底部功能区域-->
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  <!--添加热门关键字-->
  <div id="AddHotKeyword" style="display:none;">
    <div class="frame-content">
     <p><span class="frame-span frame-input90"><em >*</em>&nbsp;商品主分类：</span> <Hi:ProductCategoriesDropDownList IsTopCategory="true" ID="dropCategory" runat="server" /></p>
     <b id="ctl00_contentHolder_txtHotKeywordsTip">不能为空，一行代表一个关键字，一次可以添加多个关键字</b>
     <p><span class="frame-span frame-input90"><em >*</em>&nbsp;关键字名称：</span><asp:TextBox ID="txtHotKeywords" runat="server" CssClass="forminput" TextMode="MultiLine" Height="100px" Width="300px"></asp:TextBox></p>
    </div>
  </div>

  <div style="display:none">
     <asp:Button ID="btnSubmitHotkeyword" runat="server" Text="添加" CssClass="submit_sure" />
       <asp:Button ID="btnEditHotkeyword" runat="server" Text="编辑"  CssClass="submit_DAqueding"/> 
  </div>


<!--编辑热门关键字-->
<div id="EditHotKeyword"  style=" display:none;">
  <div class="frame-content">
    <p><span class="frame-span frame-input90"><em >*</em>&nbsp;商品主分类：</span><Hi:ProductCategoriesDropDownList IsTopCategory="true" ID="dropEditCategory" runat="server" />   </p>
    <p> <span class="frame-span frame-input90"><em >*</em>&nbsp;关键字名称：</span> <asp:TextBox ID="txtEditHotKeyword" runat="server" CssClass="forminput"></asp:TextBox>  </p>
    <b id="ctl00_contentHolder_txtEditHotKeywordTip">不能为空，只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾</b>
   <input type="hidden" runat="server" id="hiHotKeyword" />    
   <input type="hidden" runat="server" id="hicategory" /> 
  </div>
</div>

  

        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    var formtype = "add";
    function validatorForm() {
        if (formtype == "add") {
            if ($("#ctl00_contentHolder_dropCategory").val() == "") {
                alert("请选择商品主分类");
                return false;
            }
            if ($("#ctl00_contentHolder_txtHotKeywords").val() == "") {
                alert("请输入热门关键字");
                return false;
            }
            return true;
        } else {
            if ($("#ctl00_contentHolder_dropEditCategory").val() == "") {
                alert("请选择商品主分类");
                return false;
            }
            if ($("#ctl00_contentHolder_txtEditHotKeyword").val() == "") {
                alert("请输入热门关键字");
                return false;
            }
            return true;
        }
    }

    function ShowEditDiv(id,categoryId, keywords) {
        formtype = "edite";
        arrytext = null;
        setArryText('ctl00_contentHolder_dropEditCategory', categoryId);
        setArryText('ctl00_contentHolder_hicategory', categoryId);
        setArryText('ctl00_contentHolder_txtEditHotKeyword', keywords);
        setArryText('ctl00_contentHolder_hiHotKeyword', keywords);
        setArryText('ctl00_contentHolder_txtHid', id);

        DialogShow("编辑热门关键字", "hotkey", "EditHotKeyword", "ctl00_contentHolder_btnEditHotkeyword");

    }

    function AddHotKeyDiv() {
        arrytext = null;
        formtype = "add";
        setArryText('ctl00_contentHolder_dropCategory', "");
        setArryText('ctl00_contentHolder_txtHotKeywords', "");

        DialogShow('添加热门关键字', 'hotkey', 'AddHotKeyword', 'ctl00_contentHolder_btnSubmitHotkeyword'); 
    }
</script>
</asp:Content>