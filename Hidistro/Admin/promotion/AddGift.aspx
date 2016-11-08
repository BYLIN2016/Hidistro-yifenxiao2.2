<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddGift.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddGift" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="areacolumn clearfix validator4">
	<div class="columnright">
	   <div class="title"> <em><img src="../images/06.gif" width="32" height="32" /></em>
        <h1>添加礼品</h1>
        <span>填写礼品详细信息</span>
      </div>
            <div class="datafrom">
        <div class="formitem validator2">
          <ul>
            <li><span class="formitemtitle Pw_110"><em >*</em>礼品名称：</span>
              <asp:TextBox ID="txtGiftName" runat="server" CssClass="forminput"></asp:TextBox>　
              <input type="checkbox" id="chkDownLoad" runat="server" checked="checked" />是否允许分销站点下载使用
              <input type="checkbox" id="chkPromotion" runat="server" />是否参与促销赠送
              <p id="ctl00_contentHolder_txtGiftNameTip">礼品名称，在1至60个字符之间</p>
            </li>
            <li class="m_none"><span class="formitemtitle Pw_110">礼品图片：</span>
                      <span class="Add_Goods">
                             <Hi:ImageUploader runat="server" UploadType="Gift" ID="uploader1" />
             </span>                        
            </li>
            <li class="clearfix"><span class="formitemtitle Pw_110">计量单位：</span>
             <asp:TextBox ID="txtUnit" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtUnitTip">计量单位，在1至10个字符之间</p>             
            </li>
            <li> <span class="formitemtitle Pw_110">成本价：</span>
               <asp:TextBox ID="txtCostPrice" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtCostPriceTip">成本价只能是数值，且不能超过2位小数</p>
            </li>
            <li> <span class="formitemtitle Pw_110"><em >*</em>采购价：</span>
                 <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="forminput"></asp:TextBox>
              <p id="ctl00_contentHolder_txtPurchasePriceTip">采购价只能是数值，不能为空，且不能超过2位小数</p>
            </li>
            <li> <span class="formitemtitle Pw_110">市场参考价：</span>
               <asp:TextBox ID="txtMarketPrice" runat="server" CssClass="forminput"></asp:TextBox>
             <p id="ctl00_contentHolder_txtMarketPriceTip">市场参考价只能是数值，且不能超过2位小数</p>
            </li>
            <li>
            <span class="formitemtitle Pw_110"><em >*</em>兑换需积分：</span>
            <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtNeedPointTip">兑换所需积分只能是数字，必须大于等于O,0表示不能兑换</p>
	      </li>     
            <h2>礼品描述</h2>
            
            <li> <span class="formitemtitle Pw_110">简单介绍：</span>
             <asp:TextBox ID="txtShortDescription" runat="server" TextMode="MultiLine" Width="450px" Height="70px"></asp:TextBox>
               <p id="ctl00_contentHolder_txtShortDescriptionTip">简单介绍，在1至300个字符之间</p>
            </li>
            <li> <span class="formitemtitle Pw_110">详细信息：</span><Kindeditor:KindeditorControl ID="fcDescription" runat="server"    Height="200px"/>           </li>
              <li class="clear"></li>
            <h2>SEO设置</h2>
            <li><span class="formitemtitle Pw_110">详细页标题：</span>
                <asp:TextBox ID="txtGiftTitle" runat="server" CssClass="forminput"></asp:TextBox>
                  <p id="ctl00_contentHolder_txtGiftTitleTip">详细页标题，在1至100个字符之间</p>
            </li>
            <li> <span class="formitemtitle Pw_110">详细页关键字：</span>
               <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="forminput"></asp:TextBox>
                 <p id="ctl00_contentHolder_txtTitleKeywordsTip">详细页关键字，在1至160个字符之间</p>
            </li>
            <li> <span class="formitemtitle Pw_110">详细页描述：</span>
               <asp:TextBox ID="txtTitleDescription" runat="server" CssClass="forminput"></asp:TextBox>
                 <p id="ctl00_contentHolder_txtTitleDescriptionTip">详细页描述，在1至260个字符之间</p>
            </li>
          </ul>
           <ul class="btntf Pw_110">
		     <asp:Button ID="btnCreate" runat="server" Text="添加" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding inbnt"  />
            
		  </ul>
</div>
      </div>
	</div>
</div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_txtGiftName', 1, 60, false, null,  '礼品名称，在1至60个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtUnit', 1, 10, true, null,  '计量单位，在1至10个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtCostPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '成本价只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtCostPrice', 0.01, 10000000, '成本价只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtPurchasePrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', ' 采购价只能是数值，不能为空，且不能超过2位小数'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtPurchasePrice', 0.01, 10000000, ' 采购价只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtMarketPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '市场参考价只能是数值，且不能超过2位小数'))
appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtMarketPrice', 0.01, 10000000, '市场参考价只能是数值，不能超过10000000，且不能超过2位小数'));
initValid(new InputValidator('ctl00_contentHolder_txtNeedPoint', 1, 10, false, '-?[0-9]\\d*', '兑换所需积分只能是数字，必须大于等O,0表示不能兑换'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPoint', 0, 10000, '兑换所需积分不能为空，大小0-10000之间'));
initValid(new InputValidator('ctl00_contentHolder_txtShortDescription', 1, 300, true, null,  '简单介绍，在1至300个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtGiftTitle', 1, 100, true, null,  '详细页标题，在1至100个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtTitleKeywords', 1,160, true, null,  '详细页关键字，在1至160个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtTitleDescription', 1, 260, true, null,  '详细页描述，在1至260个字符之间'))

}
$(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>