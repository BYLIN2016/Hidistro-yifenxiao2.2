<%@ Page Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditMyGifts.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditMyGifts" Title="无标题页" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="optiongroup mainwidth">
	<ul>
		<li class="optionend"><a href="MyGifts.aspx"><span>礼品下载查询</span></a></li>
		<li class="menucurrent"><a href="MyGiftList.aspx"><span>礼品管理</span></a></li>
	</ul>
</div>
<div class="dataarea mainwidth databody">
	  <div class="title  m_none td_bottom"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>编辑礼品信息</h1>
        <span>礼品基本信息</span>
      </div>
	  <div class="datafrom">
	    <div class="formitem validator1">
	      <ul id="ul_input">
	     
          <h2 class="colorE">1.礼品基本信息</h2>
	        <li>
	            <span class="formitemtitle Pw_198">礼品名称：<em >*</em></span>
               <asp:TextBox ID="txtGiftName" runat="server" CssClass="forminput"></asp:TextBox> <input type="checkbox" id="ckpromotion"  runat="server"/>参与促销
              <p id="ctl00_contentHolder_txtGiftNameTip">礼品名称，在1至60个字符之间</p>
            </li>
	        <li>
	            <span class="formitemtitle Pw_198">礼品图片：</span>
                 <Hi:ImageUploader runat="server" UploadType="Gift" ID="uploader1" />
            </li>
	        <li class="m_none" id="attributeRow">        
	          <span class="formitemtitle Pw_198">计量单位：</span>
              <asp:Label ID="lblUnit" runat="server"></asp:Label>
            <p id="ctl00_contentHolder_txtUnitTip"></p>
            </li>
	        <li class=" clearfix"> <span class="formitemtitle Pw_198">采购价：<em >*</em></span>
	         <asp:Label ID="lblPurchasePrice" runat="server"></asp:Label>
             <p id="ctl00_contentHolder_txtPurchasePriceTip"></p>
	        </li>
	        
	        
             <li> <span class="formitemtitle Pw_198">市场参考价：</span>
	       <asp:Label ID="lblMarketPrice" runat="server"></asp:Label>
             <p id="ctl00_contentHolder_txtMarketPriceTip">市场参考价只能是数值，且不能超过2位小数</p>
	        </li>  
	        
            <li> <span class="formitemtitle Pw_198">兑换需积分：<em >*</em></span>
	           <asp:TextBox ID="txtNeedPoint" runat="server" Text="0" CssClass="forminput"></asp:TextBox>
            <p id="ctl00_contentHolder_txtNeedPointTip">兑换所需积分只能是数字，必须大于等于O,0表示不能兑换</p>
	        </li>
            
	        <h2 class="colorE">2. 礼品描述</h2>
	        <li>
			  <span class="formitemtitle Pw_198">简单介绍：</span>
				<asp:Label ID="lblShortDescription" runat="server"  Width="250px" Height="70px"></asp:Label>
             <p id="ctl00_contentHolder_txtShortDescriptionTip"></p>
 			</li>
           
            <li>
			  <span class="formitemtitle Pw_198">详细信息：</span>
			  <asp:Label ID="fcDescription" runat="server" Width="600px"></asp:Label>
				<%--<Kindeditor:KindeditorControl ID="fcDescription" runat="server" Width="85%"  Height="200px" Enabled="false"/>--%>
            </li>

	      <li><span class="formitemtitle Pw_198">详细页标题：</span>
                <asp:TextBox ID="txtGiftTitle" runat="server" CssClass="forminput"></asp:TextBox>
                <p id="ctl00_contentHolder_txtGiftTitleTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_198">详细页关键字：</span>
               <asp:TextBox ID="txtTitleKeywords" runat="server" CssClass="forminput"></asp:TextBox>
               <p id="ctl00_contentHolder_txtTitleKeywordsTip"></p>
            </li>
            <li> <span class="formitemtitle Pw_198">详细页描述：</span>
               <asp:TextBox ID="txtTitleDescription" runat="server" CssClass="forminput"></asp:TextBox>
               <p id="ctl00_contentHolder_txtTitleDescriptionTip"></p>
            </li>
	      </ul>
	      <ul class="btntf Pa_198 clear">
	        <asp:Button runat="server" ID="btnUpdate" Text="保 存" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding inbnt" />
          </ul>
        </div>
      </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
function InitValidators()
{
initValid(new InputValidator('ctl00_contentHolder_txtGiftName', 1, 60, false, null,  '礼品的名称，在1至60个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtNeedPoint', 1, 10, false, '-?[0-9]\\d*', '兑换所需积分只能是数字，必须大于等O,0表示不能兑换'))
appendValid(new NumberRangeValidator('ctl00_contentHolder_txtNeedPoint', 0, 10000, '兑换所需积分不能为空，大小0-10000之间'));
initValid(new InputValidator('ctl00_contentHolder_txtGiftTitle', 1, 100, true, null,  '详细页标题，在1至100个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtTitleKeywords', 1, 160, true, null,  '详细页关键字，在1至160个字符之间'))
initValid(new InputValidator('ctl00_contentHolder_txtTitleDescription', 1, 260, true, null,  '详细页描述，在1至260个字符之间'))

}
$(document).ready(function(){ 
    InitValidators();
   
 });
</script>
</asp:Content>
