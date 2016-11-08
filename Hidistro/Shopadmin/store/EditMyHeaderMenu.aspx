<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyHeaderMenu.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.store.EditMyHeaderMenu" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtTitle', 1, 60, false, null, '用于标识广告位，不能为空，长度限制在60个字符以内'))
    }
    $(document).ready(function () {
        InitValidators();

        var menuType = $("#ctl00_contentHolder_txtMenuType").val();

        if (menuType == "1") {
            $("#liSystemPage").show();
            $("#liSearchLink").hide();
            $("#liCustomLink").hide();
        }

        else if (menuType == "2") {
            $("#liSystemPage").hide();
            $("#liSearchLink").show();
            $("#liCustomLink").hide();
        }
        else {
            $("#liSystemPage").hide();
            $("#liSearchLink").hide();
            $("#liCustomLink").show();
        }
    });

    function Valid() {
        var menuType = $("#ctl00_contentHolder_txtMenuType").val();

        if (menuType == "1") {
            if ($("#ctl00_contentHolder_dropSystemPageDropDownList").val() == "") {
                alert("请选系统页面！")
                return false;
            }
        }
        else if (menuType == "3") {
            if ($("#ctl00_contentHolder_txtCustomLink").val() == "") {
                alert("请输入系统链接！")
                return false;
            }
        }

        if (!PageIsValid())
            return false;

        return true;
    }
</script>

<div class="areacolumn clearfix">
      <div class="columnright">
          <div class="title">
            <em><img src="../images/01.gif" width="32" height="32" /></em>
            <h1>编辑页头菜单</h1>
            <span>编辑你设置好的页头栏目的菜单</span>
          </div>
       <div class="formitem validator2">
        <ul>
          <li><span class="formitemtitle Pw_110"><em >*</em>菜单名称：</span>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="forminput" />
            <p id="txtMenuNameTip" runat="server">菜单名称，不能为空，长度限制在60个字符以内</p>
            <Hi:TrimTextBox runat="server" ID="txtMenuType" style="display:none;" />
          </li>
          <li id="liSystemPage">
            <span class="formitemtitle Pw_110"><em >*</em>系统页面：</span>
              <span class="formselect"><Hi:SystemPageDropDownList runat="server" ID="dropSystemPageDropDownList" /></span>
          </li>
          <li id="liSearchLink">
            <span class="formitemtitle Pw_110"><em >*</em>筛选条件：</span>
            <div style="float:left;">不选择或不输入筛选条件将默认展示全部商品
            <table border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td style="width:80px;">价格区间：</td>
                    <td width="45">
                        <asp:TextBox runat="server" ID="txtMinPrice" CssClass="forminput" Width="100px" />
                    </td>
                    <td width="55" align="center"><strong>-</strong></td>
                    <td width="48">
                        <asp:TextBox runat="server" ID="txtMaxPrice" CssClass="forminput" Width="100px" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td style="width:80px;">关 键 词：</td>
                    <td>
                        <asp:TextBox ID="txtKeyword" runat="server" CssClass="forminput" Width="260px" /> 
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td>
                        商品分类<br/>
                        <Hi:DistributorProductCategoriesListBox runat="server" ID="listProductCategories" Width="200" Height="150px" />
                    </td>
                     <td width="25px" ></td>
                     <td>
                        品牌<br/>
                        <Hi:BrandCategoriesList  runat="server" ID="listBrandCategories" Width="150" Height="150px" />
                    </td>
                     <td width="25px" ></td>
                    <td>
                        标签<br/>
                        <asp:ListBox ID="radProductTags" runat="server" Width="150" Height="150px" ></asp:ListBox>
                    </td>
                </tr>
            </table>
            </div>
          </li>
          <li id="liCustomLink">
            <span class="formitemtitle Pw_110">自定义链接：<em >*</em></span>
            <asp:TextBox ID="txtCustomLink" runat="server" Text="http:\\" CssClass="forminput" Width="260px" />
          </li>
      </ul>
      <div style="clear:both"></div>
      <ul class="btn Pa_100">
        <asp:Button ID="btnSave" runat="server" OnClientClick="return Valid();" Text="保 存"  CssClass="submit_DAqueding" />
        </ul>
      </div>

      </div>
  </div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
</asp:Content>
