<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ManageMyHotKeywords.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyHotKeywords" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">    
      
       <ul>
        <li class="optionstar"><a href="MyFriendlyLinks.aspx" class="optionnext"><span>友情链接</span></a></li>
        <li class="menucurrent"><a href="#"><span>热门关键字</span></a></li>
        <li class="optionend"><a href="MyVotes.aspx"><span>投票调查</span></a></li>
      </ul>
</div>
  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>热门关键字管理</h1>
     <span>设置前台显示的热门关键字</span></div>
    <!-- 添加按钮-->
    <div class="btn">
      <a class="submit_jia" href="javascript:DivWindowOpen(550,350,'AddHotKeyword');">添加热门关键字</a>
      <input runat="server" type="hidden" id="txtHid" />
    </div>
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">
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
                              <span class="submit_bianji"><a href="javascript:ShowEditDiv('<%# Eval("Hid")%>','<%# Eval("CategoryId")%>','<%# Eval("Keywords") %>');" id="dd">编辑</a></span>
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
  <div class="Pop_up" id="AddHotKeyword" style=" display:none;">
  <h1>添加热门关键字 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform validator2">
   
      
        <table style="width:100%;font-size:14px;">
    <tr>
        <td width="30%" style="text-align:right; vertical-align:top;">商品主分类：<em >*</em></td><td>   <Hi:DistributorProductCategoriesDropDownList IsTopCategory="true" ID="dropCategory" runat="server" />   </td>
    </tr>
    <tr>
        <td style="text-align:right;vertical-align:top;">关键字名称：<em >*</em></td><td>              <asp:TextBox ID="txtHotKeywords" runat="server" CssClass="forminput" TextMode="MultiLine" Height="100px" Width="200px"></asp:TextBox>
            <p id="ctl00_contentHolder_txtHotKeywordsTip">不能为空，一行代表一个关键字，一次可以添加多个关键字</p>
        </td>
    </tr>

    <tr><td colspan="2" style="text-align:center;"> <asp:Button ID="btnSubmitHotkeyword" runat="server" Text="添 加"  OnClientClick="return validatorForm()"  CssClass="submit_DAqueding"/> </td></tr>
    </table>

  </div>
</div>

<!--编辑热门关键字-->
<div class="Pop_up" id="EditHotKeyword"  style=" display:none;">
  <h1>编辑热门关键字 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform validator2">
    <ul>
        <li> <span class="formitemtitle Pw_100">商品主分类：<em >*</em></span>
            <Hi:DistributorProductCategoriesDropDownList IsTopCategory="true" ID="dropEditCategory" runat="server" />   
        </li>
        <li> <span class="formitemtitle Pw_100">关键字名称：<em >*</em></span>
            <asp:TextBox ID="txtEditHotKeyword" runat="server" CssClass="forminput"></asp:TextBox>     
            <input type="hidden" runat="server" id="hiHotKeyword" />    
                  <input type="hidden" runat="server" id="hicategory" />                
            <p id="ctl00_contentHolder_txtEditHotKeywordTip">关键字名称不能为空，只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾,长度限制在60个字符以内</p>
        </li>
    </ul>
        <ul class="up Pa_100">
      <asp:Button ID="btnEditHotkeyword" runat="server" Text="确 定"  OnClientClick="return validatorEditForm()"  CssClass="submit_DAqueding"/> 
  </ul>
  </div>
</div>
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function validatorEditForm(){
        if($("#ctl00_contentHolder_dropEditCategory").val() == ""){
            alert("请选择商品主分类");
            return false;
        }
        if($("#ctl00_contentHolder_txtEditHotKeyword").val() == ""){
            alert("请输入热门关键字");
            return false;
        }
        return true;
    }
    
    function validatorForm(){
        if($("#ctl00_contentHolder_dropCategory").val() == ""){
            alert("请选择商品主分类");
            return false;
        }
        if($("#ctl00_contentHolder_txtHotKeywords").val() == ""){
            alert("请输入热门关键字");
            return false;
        }
        return true;
    }
  
    function ShowEditDiv(id, categoryId, keywords) {
        $("#ctl00_contentHolder_dropEditCategory").val(categoryId);
        $("#ctl00_contentHolder_hicategory").val(categoryId);
        $("#ctl00_contentHolder_txtEditHotKeyword").val(keywords);
        $("#ctl00_contentHolder_hiHotKeyword").val(keywords);
        $("#ctl00_contentHolder_txtHid").val(id)
        DivWindowOpen(650, 200, 'EditHotKeyword');

    }


</script>
</asp:Content>
