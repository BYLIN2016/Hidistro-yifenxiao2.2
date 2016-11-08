<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MySiteContent.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MySiteContent" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="dataarea mainwidth databody">
      <div class="title title_height m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
        <h1 class="title_line">店铺基本设置</h1>
      </div>
      <div class="datafrom">
        <div class="formitem validator1">
          <ul>
            <li><span class="formitemtitle Pw_198">店铺名称：<em >*</em></span>
              <asp:TextBox ID="txtSiteName" CssClass="forminput" runat="server"  />
              <p id="txtSiteNameTip" runat="server">店铺名称为必填项，长度限制在60字符以内</p>
            </li>
            <li><span class="formitemtitle Pw_198">店铺标志：</span>
              <asp:FileUpload ID="fileUpload" runat="server" CssClass="forminput" />
              <div style="margin:0px auto;width:500px;">
                <table width="300" border="0" cellspacing="0">
                  <tr>
                     <td valign="middle" style="padding-top:10px;"><Hi:HiImage ID="imgLogo" runat="server" Width="180" Height="40" /></td>
                    <td width="55" align="center"><Hi:ImageLinkButton ID="btnDeleteLogo"  runat="server" Text="删除" IsShow="true" /></td>
                  </tr>
                  <tr><td colspan="2"></td></tr>
                </table>
              </div>
            </li>
            <li> <span class="formitemtitle Pw_198">自定义页尾：</span>
            <span> <Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fkFooter" runat="server" Width="533px" Height="200px"/></span>
            </li>
                 <li> <span class="formitemtitle Pw_198">会员注册协议：</span>
            <span> <Kindeditor:KindeditorControl ID="fckRegisterAgreement" ImportLib="false" runat="server" Width="550px" Height="300px"/></span>
            </li>
            <h2  class="clear">商品设置</h2>
            <li> <span class="formitemtitle Pw_198">商品价格精确到小数点后位数：</span>
              <Hi:DecimalLengthDropDownList ID="dropBitNumber" CssClass="forminput" runat="server" />
            </li>
            <li> <span class="formitemtitle Pw_198">“您的价”重命名为：</span>
             <asp:TextBox Id="txtNamePrice" runat="server" CssClass="forminput"></asp:TextBox>
              <p id="txtNamePriceTip" runat="server">设置前台商品的“您的价”重命名为其他名称，如“批发价”</p>
            </li>
            <h2 class="clear">订单设置</h2>
            <li> <span class="formitemtitle Pw_198">显示订单数：<em >*</em></span>
              <asp:TextBox ID="txtShowDays" runat="server" CssClass="forminput" /></span>
			  <p id="txtShowDaysTip" runat="server">设置前台发货查询中显示最近几天内的已发货订单</p>
            </li>           
            <h2 class="clear">SEO设置</h2>
            <li> <span class="formitemtitle Pw_198">简单介绍：</span>
              <asp:TextBox ID="txtSiteDescription" CssClass="forminput" runat="server"></asp:TextBox>
              <p id="txtSiteDescriptionTip" runat="server">简单介绍TITLE的长度限制在100字符以内</p>
            </li>
            <li> <span class="formitemtitle Pw_198">店铺描述：</span>
              <asp:TextBox ID="txtSearchMetaDescription" runat="server" CssClass="forminput"></asp:TextBox>
              <p id="txtSearchMetaDescriptionTip" runat="server">店铺描述META_DESCRIPTION的长度限制在260字符以内</p>
            </li>
            <li> <span class="formitemtitle Pw_198">搜索关键字：</span>
              <asp:TextBox ID="txtSearchMetaKeywords" CssClass="forminput" runat="server" />
              <p id="txtSearchMetaKeywordsTip" runat="server">搜索关键字META_KEYWORDS的长度限制在160字符以内，多个关键字之间用,号分开</p>
            </li>
              <h2>在线客服设置</h2>
            <li> <span class="formitemtitle Pw_198">在线客服：</span><span><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fcOnLineServer" runat="server" Width="533px" ImportLib="false" Height="200px"/></span>
              <p>请在这里填入您获取的在线客服代码</p>
            </li>
          </ul>
           <ul class="btntf Pa_198 clearfix">
		    <asp:Button ID="btnOK" runat="server" Text="提 交" CssClass="submit_DAqueding inbnt" OnClientClick="return PageIsValid();" />
			</ul>
</div>
      </div>
</div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators()
    {
        initValid(new InputValidator('ctl00_contentHolder_txtSiteName', 1, 60, false, null, '店铺名称为必填项，长度限制在60字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSiteDescription', 0, 100, true, null, '简单介绍TITLE的长度限制在100字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaDescription', 0, 260, true, null, '店铺描述META_DESCRIPTION的长度限制在260字符以内'))
        initValid(new InputValidator('ctl00_contentHolder_txtSearchMetaKeywords', 0, 160, true, null, '搜索关键字META_KEYWORDS的长度限制在160字符以内，多个关键字之间用,号分开'))
        initValid(new SelectValidator('ctl00_contentHolder_dropBitNumber', false, '设置商品的价格经过运算后数值精确到小数点后几位，超出将四舍五入。'))
        initValid(new InputValidator('ctl00_contentHolder_txtNamePrice', 1, 10, true, null, '您的价长度不能超过10个字符'))
        initValid(new InputValidator('ctl00_contentHolder_txtShowDays', 1, 10, false, '-?[0-9]\\d*', '设置前台发货查询中显示最近几天内的已发货订单'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtShowDays', 0, 90, '设置前台发货查询中显示最近几天内的已发货订单'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
