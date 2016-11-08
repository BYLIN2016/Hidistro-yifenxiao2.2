<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ImportFromTB.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.ImportFromTB" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
		<ul>
			<li class="optionstar"><a href="ImportFromYfx.aspx" class="optionnext"><span>从分销商城数据包导入</span></a></li>
			<li class="menucurrent"><a><span>从淘宝数据包导入</span></a></li>
			<li><a href="ImportFromPP.aspx"><span>从拍拍数据包导入</span></a></li>
		</ul>
</div>
<div class="dataarea mainwidth databody">

<div class="datafrom">
<div class="formitem">
            <ul>
              <li><h2 class="colorE">数据包信息</h2></li>
              <li> <span style="color:Red;">淘宝数据包的来源：1.易分销或Hishop系统本身导出的淘宝数据包；2.淘宝或商品助理导出的文件，把CSV文件和图片文件夹都命名为products然后选中两个打成zip的压缩包</span></li>
              <li>
                <span class="formitemtitle Pw_198">导入插件版本： </span>
                <span class="formselect"><asp:DropDownList runat="server" ID="dropImportVersions" width="200"></asp:DropDownList></span>
              </li>
              <li>
                <span class="formitemtitle Pw_198">选择要导入的数据包文件： </span>
                <span class="formselect"><asp:DropDownList runat="server" ID="dropFiles" width="200"></asp:DropDownList></span>
                <div style="clear:both;" class="Pa_198">
                <span>
                导入之前需要先将数据包文件上传到服务器上；</br>
                如果上面的下拉框中没有您要导入的数据包文件，请先上传。
                </span>
                </div>
              </li>
              <li> <span class="formitemtitle Pw_198">&nbsp;</span>
                <asp:FileUpload runat="server" ID="fileUploader" CssClass="forminput"/>&nbsp;&nbsp;<asp:Button runat="server" ID="btnUpload" Text="上传"/>               
                <div style="clear:both;" class="Pa_198">
                    <span>上传数据包须小于40M，否则可能上传失败，<br/>您还可以使用FTP工具先将数据包上传到网站的/storage/data/taobao目录以后，再重新打开此页面操作。</span>
                </div>
              </li>
              </ul>
              <ul>
              <li><h2 class="colorE">导入选项</h2></li>
              <li>
                <span class="formitemtitle Pw_198">商品分类：</span>
                <abbr class="formselect">
                    <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="-请选择商品分类-" width="200"/>
                </abbr>
              </li>
              <li id="liCategory"> <span class="formitemtitle Pw_198">产品线：</span>
                   <abbr class="formselect">
                   <Hi:ProductLineDropDownList ID="dropProductLines" runat="server" NullToDisplay="-请选择产品线-" width="200" />
                   </abbr>
              </li>
                    <li > <span class="formitemtitle Pw_198">商品品牌：</span>
                   <abbr class="formselect" >
                    <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择品牌--" width="200"></Hi:BrandCategoriesDropDownList>
                   </abbr>
              </li>
              <li> <span class="formitemtitle Pw_198">商品导入状态：</span> 
                <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true"  Text="出售中"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus"  Text="下架区"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"  Text="仓库中"></asp:RadioButton>
            </li>
            </ul>
            <ul class="btntf Pa_198">
                <asp:Button ID="btnImport" runat="server" OnClientClick="return doImport();" CssClass="submit_DAqueding inbnt" Text="导 入" />
            </ul>
            <div class="blank12 clearfix"></div>
        </div>
    </div>

</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
