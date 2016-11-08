<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="RolePermissions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.RolePermissions" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
<style>
	.PurviewItem {clear:both}
	.PurviewItemSave{float:left;height:25px;line-height:25px;padding-left:20px;margin:0 0 0 5px;_margin-left:3px;vertical-align:middle;background:url(images/saveitem.gif) no-repeat 0px 5px; padding-left:20px;}
	.PurviewItem ul{width:850px;list-style:none;}
	.PurviewItem ul li{float:left;height:20px;line-height:20px;margin-right:8px;width:140px;}
	.PurviewItem ol {clear:both;padding-left:98px;}
	.PurviewItem ol li{float:left;height:20px;line-height:20px;margin-right:8px;width:140px;}
	.PurviewItemDivide {height:1px;width:100%;overflow:hidden;background-color:#ddd;margin:5px 0;}
	.PurviewItemBackground {background:#E1F3FF;border:1px solid #8ACEFF;} 
	.clear{ clear:both;}

</style> 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
 <div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>设置部门权限</h1>
  <span>根据不同的部门设置不同的权限，方便控制店铺的管理 </span></div>
        <div class="datalist">
        <table width="100%"  height="30px"  border="0" cellspacing="0">
		    <tr class="table_title">	          
	          <td width="9%" class="td_left"><strong>当前部门：</strong></td>
	          <td align="left" class="td_right"><span style=" font-weight:800;"><strong><asp:Literal runat="server" Id="lblRoleName"></asp:Literal></strong></span></td>	          
      </tr>
      
      </table>
      
      <div style="margin-left:15px; margin-top:10px;"><span class="submit_btnquanxuan"><asp:LinkButton ID="btnSetTop" runat="server" Text="保存" /></span>
	  <span class="submit_btnchexiao"><a href="Roles.aspx">返回</a></span></div>
         
           
		  <div class="grdGroupList clear" style="padding-left:10px;margin-top:5px">

			<div style="color:Blue;font-weight:700;"><label><asp:CheckBox ID="cbAll" runat="server"  />全部选定</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="font-weight:700;color:#000066"><label><asp:CheckBox ID="cbSummary" runat="server" />后台即时营业信息</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="font-weight:700;color:#000066"><label><asp:CheckBox ID="cbShop" runat="server"  />店铺管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
				<div class="PurviewItem">
					<ul>
						<li style="width:90px;font-weight:700;">基本设置：</li>
						<li><label><asp:CheckBox ID="cbSiteContent" runat="server" />网店基本设置</label></li>	
                        <li><label><asp:CheckBox ID="cbEmailSettings" runat="server" />邮件设置</label></li>		
                        <li style="margin-left:-10px;"><label><asp:CheckBox ID="cbSMSSettings" runat="server" />手机短信设置</label></li>		
					</ul>
				</div>
                
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">支付设置：</li>
						<li><label><asp:CheckBox ID="cbPaymentModes" runat="server" />支付方式</label></li>
					</ul>
				</div>
                <div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">配送设置：</li>
						<li><label><asp:CheckBox ID="cbShippingModes" runat="server" />配送方式列表</label></li>
                        <li><label><asp:CheckBox ID="cbShippingTemplets" runat="server" />运费模板</label></li>
                        <li><label><asp:CheckBox ID="cbExpressComputerpes" runat="server" />物流公司</label></li>
					</ul>
				</div>


				<div class="PurviewItem">
					<ul>
					    <li style="width:100px;font-weight:700;">邮件短信模板：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbMessageTemplets" runat="server" />邮件短信模板</label></li>

					</ul>
				</div>

               <div class="PurviewItem">
					<ul>
					    <li style="width:100px;font-weight:700;">图库管理：</li>
						<li><label><asp:CheckBox ID="cbPictureMange" runat="server" />图库管理</label></li>
					</ul>
				</div>
			</div>


           <div style="clear:both;margin-top:40px;font-weight:700;color:#000066"><label><asp:CheckBox ID="cbPageManger" runat="server"  />页面管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
				<div class="PurviewItem">
                	<ul>
					    <li style="width:90px;font-weight:700;">模板管理：</li>
					    <li><label><asp:CheckBox ID="cbManageThemes" runat="server" />模板管理</label></li>
					</ul>
                </div>
                <div class="PurviewItem">
                	<ul>
					    <li style="width:90px;font-weight:700;">内容管理：</li>
					    <li><label><asp:CheckBox ID="cbAfficheList" runat="server" />店铺公告</label></li>
                        <li><label><asp:CheckBox ID="cbHelpCategories" runat="server" />帮助分类</label></li>
                        <li><label><asp:CheckBox ID="cbHelpList" runat="server" />帮助管理</label></li>
                        <li><label><asp:CheckBox ID="cbArticleCategories" runat="server" />文章分类</label></li>
						<li><label><asp:CheckBox ID="cbArticleList" runat="server" />文章管理</label></li>
					</ul>
                    <ol>
                        <li><label><asp:CheckBox ID="cbFriendlyLinks" runat="server" />友情链接</label></li>
						<li><label><asp:CheckBox ID="cbManageHotKeywords" runat="server" />热门关键字</label></li>
						<li><label><asp:CheckBox ID="cbVotes" runat="server" />投票调查</label></li>
                    </ol>
                </div>
            </div>
           

            <div style="clear:both;margin-top:40px;font-weight:700; color:#000066"><label><asp:CheckBox ID="cbProductCatalog" runat="server"  />商品管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
			
					
				<div class="PurviewItem">
					<ul>
						<li style="width:90px;font-weight:700;">商品管理：</li>
						<li><label><asp:CheckBox ID="cbManageProducts" runat="server"  />商品：</label></li>
						<li><label><asp:CheckBox ID="cbManageProductsView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbManageProductsAdd" runat="server" />添加</label></li>
						<li><label><asp:CheckBox ID="cbManageProductsEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbManageProductsDelete" runat="server" />删除</label></li>
						<li style="width: 90px">&nbsp;</li>
						<li>&nbsp;</li>
					    <li><label><asp:CheckBox ID="cbInStock" runat="server" />入库</label></li>
					    <li><label><asp:CheckBox ID="cbManageProductsUp" runat="server" />上架</label></li>
					    <li><label><asp:CheckBox ID="cbManageProductsDown" runat="server" />下架</label></li>
					    <li><label><asp:CheckBox ID="cbPackProduct" runat="server" />铺货</label></li>
					     <li style="width: 90px">&nbsp;</li>	
					    <li>&nbsp;</li>
					    <li><label><asp:CheckBox ID="cbUpPackProduct" runat="server" />取消铺货</label></li>		
					</ul>					
					<ol>
					<li><label><asp:CheckBox ID="cbProductUnclassified" runat="server" />未分类商品</label></li>
					<li><label><asp:CheckBox ID="cbSubjectProducts" runat="server" />商品标签管理</label></li>
					<li><label><asp:CheckBox ID="cbProductBatchUpload" runat="server" />批量上传</label></li>
				    <li><label><asp:CheckBox ID="cbProductBatchExport" runat="server" />批量导出</label></li>
					</ol>
				</div>

                <div class="PurviewItem">
				    <ul>
						<li style="width:90px;font-weight:700;">产品线：</li>
						<li><label><asp:CheckBox ID="cbProductLines" runat="server"  />产品线：</label></li>
						<li><label><asp:CheckBox ID="cbProductLinesView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbAddProductLine" runat="server" />添加</label></li>
						<li><label><asp:CheckBox ID="cbEditProductLine" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbDeleteProductLine" runat="server" />删除</label></li>
					</ul>
				</div>


                	<div class="PurviewItem">
					<ul>
						<li style="width:90px;font-weight:700;">商品类型：</li>
						<li><label><asp:CheckBox ID="cbProductTypes" runat="server"  />商品类型：</label></li>
						<li><label><asp:CheckBox ID="cbProductTypesView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbProductTypesAdd" runat="server" />添加</label></li>
						<li><label><asp:CheckBox ID="cbProductTypesEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbProductTypesDelete" runat="server" />删除</label></li>
					</ul>
				</div>
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">商品分类：</li>
						<li><label><asp:CheckBox ID="cbManageCategories" runat="server"  />商品分类：</label></li>
						<li><label><asp:CheckBox ID="cbManageCategoriesView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbManageCategoriesAdd" runat="server" />添加</label></li>
						<li><label><asp:CheckBox ID="cbManageCategoriesEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbManageCategoriesDelete" runat="server" />删除</label></li>
					</ul>
				</div>
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">品牌分类：</li>
						<li><label><asp:CheckBox ID="cbBrandCategories" runat="server"  />品牌分类</label></li>						
					</ul>
				</div>		
			</div>

                
			<div style="clear:both;margin-top:40px;font-weight:700; color:#000066"><label><asp:CheckBox ID="cbSales" runat="server"  />订单管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">订单管理：</li>
						<li><label><asp:CheckBox ID="cbManageOrder" runat="server"  />订单管理：</label></li>
						<li><label><asp:CheckBox ID="cbManageOrderView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbManageOrderDelete" runat="server" />删除</label></li>
						<li><label><asp:CheckBox ID="cbManageOrderEdit" runat="server" />修改</label></li>
						<li><label><asp:CheckBox ID="cbManageOrderConfirm" runat="server" />确认收款</label></li>
						<li style="width:90px;">&nbsp;</li>
						<li><label><asp:CheckBox ID="cbManageOrderSendedGoods" runat="server" />订单发货</label></li>
						<li><label><asp:CheckBox ID="cbExpressPrint" runat="server" />快递单打印</label></li>
						<li><label><asp:CheckBox ID="cbManageOrderRemark" runat="server" />管理员备注</label></li>
					</ul>
				
				</div>

				<div class="PurviewItem">
					<ul>
					   <li style="width:90px;font-weight:700;">快递单模板：</li>
					    <li style="width:90px;"><label><asp:CheckBox ID="cbExpressTemplates" runat="server" />快递单模板</label></li>
					</ul>
				</div>
				<div class="PurviewItem">
					<ul>
					   <li style="width:90px;font-weight:700;">发货人信息：</li>
					    <li style="width:90px;"><label><asp:CheckBox ID="cbShipper" runat="server" />发货人信息</label></li>
					</ul>
				</div>
	            <div class="PurviewItem">
					<ul>
					   <li style="width:90px;font-weight:700;">退换货设置：</li>
					    <li style="width:90px;"><label><asp:CheckBox ID="cbOrderRefundApply" runat="server" />退款申请单</label></li>
                        <li style="width:90px;"><label><asp:CheckBox ID="cbOrderReturnsApply" runat="server" />退货申请单</label></li>
                        <li style="width:90px;"><label><asp:CheckBox ID="cbOrderReplaceApply" runat="server" />换货申请单</label></li>
					</ul>
				</div>

			</div>


            <div style="clear:both;margin-top:40px;font-weight:700; color:#000066"><label><asp:CheckBox ID="cbPurchaseOrder" runat="server"  />采购单管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
                <div class="PurviewItem">
						<ul>
						<li style="width:100px;font-weight:700;">采购单管理：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbManagePurchaseOrder" runat="server"  />采购单：</label></li>
						<li><label><asp:CheckBox ID="cbManagePurchaseOrderView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbManagePurchaseOrderEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbManagePurchaseOrderDelete" runat="server" />删除</label></li>	
						<li>&nbsp;</li>
						<li style="width:100px;font-weight:700;">&nbsp;</li>
						<li>&nbsp;</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbPurchaseOrderSendGoods" runat="server" />发货</label></li>
						<li><label><asp:CheckBox ID="cbPurchaseOrderRemark" runat="server" />备注</label></li>
						</ul>
			    </div>

                <div class="PurviewItem">
					<ul>
					   <li style="width:90px;font-weight:700;">退换货设置：</li>
					    <li style="width:90px;"><label><asp:CheckBox ID="cbPurchaseOrderRefundApply" runat="server" />退款申请单</label></li>
                        <li style="width:90px;"><label><asp:CheckBox ID="cbPurchaseOrderReturnsApply" runat="server" />退货申请单</label></li>
                        <li style="width:90px;"><label><asp:CheckBox ID="cbPurchaseOrderReplaceApply" runat="server" />换货申请单</label></li>
					</ul>
				</div>
            </div>
            
			<div style="clear:both;margin-top:40px;font-weight:700; color:#000066"><label><asp:CheckBox ID="cbManageUsers" runat="server"  />会员管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
	
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">会员：</li>
						<li><label><asp:CheckBox ID="cbManageMembers" runat="server"  />会员：</label></li>
						<li><label><asp:CheckBox ID="cbManageMembersView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbManageMembersEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbManageMembersDelete" runat="server" />删除</label></li>
					</ul>
				</div>					
				
                
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">会员等级：</li>
						<li><label><asp:CheckBox ID="cbMemberRanks" runat="server"  />会员等级：</label></li>
						<li><label><asp:CheckBox ID="cbMemberRanksView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbMemberRanksAdd" runat="server" />添加</label></li>
						<li><label><asp:CheckBox ID="cbMemberRanksEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbMemberRanksDelete" runat="server" />删除</label></li>
					</ul>
				</div>
                	
				<div class="PurviewItem">
					<ul>
					    <li style="width:91px;font-weight:700;">信任登录：</li>
						<li><label><asp:CheckBox ID="cbOpenIdServices" runat="server" />信任登录列表</label></li>
						<li><label><asp:CheckBox ID="cbOpenIdSettings" runat="server" />信任登录配置</label></li>
						
					</ul>
				</div>
			</div>

			<div style="clear:both;margin-top:40px;font-weight:700;color:#000066"><label><asp:CheckBox ID="cbDistribution" runat="server"  />分销管理</label></div>
			<div class="PurviewItemDivide"></div>
			<div style="padding-left:20px;">
				<div class="PurviewItem">
				<ul>
						<li style="width:100px;font-weight:700;">分销商管理：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbDistributorGrades" runat="server"  />分销商等级：</label></li>
						<li><label><asp:CheckBox ID="cbDistributorGradesView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbDistributorGradesAdd" runat="server" />添加</label></li>
						<li><label><asp:CheckBox ID="cbDistributorGradesEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbDistributorGradesDelete" runat="server" />删除</label></li>
				</ul>
						<ol>
						<li><label><asp:CheckBox ID="cbDistributors" runat="server" />分销商：</label></li>
						<li><label><asp:CheckBox ID="cbDistributorsView" runat="server" />查看</label></li>
						<li><label><asp:CheckBox ID="cbDistributorsEdit" runat="server" />编辑</label></li>
						<li><label><asp:CheckBox ID="cbDistributorsDelete" runat="server" />删除</label></li>
						</ol>
						</div>
				<div class="PurviewItem">
						<ul>
						<li style="width:100px;font-weight:700;">招募分销商：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbRequests" runat="server"  />招募分销商：</label></li>
						<li><label><asp:CheckBox ID="cbDistributorsRequests" runat="server" />招募</label></li>
						<li><label><asp:CheckBox ID="cbDistributorsRequestInstruction" runat="server" />招募说明</label></li>
						</ul>
				</div>

                	    
				<div class="PurviewItem">
						<ul>
						<li style="width:120px;font-weight:700;">连锁加盟业务管理：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbManageDistributorSites" runat="server" />连锁加盟站点列表</label></li>
						<li style="width:140px;"><label><asp:CheckBox ID="cbDistributorSiteRequests" runat="server" />连锁加盟店开通申请</label></li>							
						</ul>
						
			    </div>

                <div class="PurviewItem">
                <ul>
                    <li style="width:120px;font-weight:700;">淘宝代销业务管理：</li>
                    						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbMakeProductsPack" runat="server" />制作淘宝数据</label></li>
                	<li style="width:140px;"><label><asp:CheckBox ID="ckTaobaoNote" runat="server" />订单处理精灵介绍</label></li>
                </ul>
                </div>
			    
                <div class="PurviewItem">
                <ul>
                   <li style="width:100px;font-weight:700;">招募分销商：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbDistributorSendedMsg" runat="server" />发件箱</label></li>
                	<li><label><asp:CheckBox ID="cbDistributorAcceptMsg" runat="server" />收件箱</label></li>
                    <li><label><asp:CheckBox ID="cbDistributorNewMsg" runat="server" />发送新消息</label></li>
                </ul>
                </div>
		
				
		   </div>

           	<div style="clear:both;margin-top:40px;font-weight:700;color:#000066"><label><asp:CheckBox ID="cbCRMmanager" runat="server"  />CRM管理</label></div>
			<div class="PurviewItemDivide"></div>
            <div style="padding-left:20px;">
                <div class="PurviewItem">
                     <ul>
                           <li style="width:100px;font-weight:700;">会员深度营销：</li>
                           <li style="margin-left:-10px;"><label><asp:CheckBox ID="cbMemberMarket" runat="server"  />会员深度营销：</label></li>
                           <li><label><asp:CheckBox ID="cbClientGroup" runat="server" />客户分组</label></li>
                           <li><label><asp:CheckBox ID="cbClientNew" runat="server" />新客户</label></li>
                           <li><label><asp:CheckBox ID="cbClientActivy" runat="server" />活跃客户</label></li>
                           <li><label><asp:CheckBox ID="cbClientSleep" runat="server" />休眠客户</label></li>
                     </ul>
                </div>
                <div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">商品评论：</li>
						<li><label><asp:CheckBox ID="cbProductConsultationsManage" runat="server"/>商品咨询管理</label></li>
						<li><label><asp:CheckBox ID="cbProductReviewsManage" runat="server"/>商品评论管理</label></li>
					</ul>
				</div>
                <div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">站内消息：</li>
						<li><label><asp:CheckBox ID="cbReceivedMessages" runat="server"/>收件箱</label></li>
						<li><label><asp:CheckBox ID="cbSendedMessages" runat="server"/>发件箱</label></li>
						<li><label><asp:CheckBox ID="cbSendMessage" runat="server"/>写新消息</label></li>
					</ul>
				</div>
				<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">客户留言：</li>
						<li><label><asp:CheckBox ID="cbManageLeaveComments" runat="server"/>客户留言管理</label></li>
					</ul>
				</div>
            </div>

            <div style="clear:both;margin-top:40px;font-weight:700;color:#000066"><label><asp:CheckBox ID="cbMarketing" runat="server"  />营销推广</label></div>
			<div class="PurviewItemDivide"></div>
             <div style="padding-left:20px;">
                <div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">积分商城：</li>
						<li><label><asp:CheckBox ID="cbGifts" runat="server" />礼品</label></li>
					</ul>
				</div>

                   <div class="PurviewItem">
					<ul>
					    <li style="width:100px;font-weight:700;">店铺促销活动：</li>
						<li><label><asp:CheckBox ID="cbProductPromotion" runat="server" />商品促销</label></li>
                        <li><label><asp:CheckBox ID="cbOrderPromotion" runat="server" />订单促销</label></li>
                        <li><label><asp:CheckBox ID="cbBundPromotion" runat="server" />捆绑促销</label></li>
                        <li><label><asp:CheckBox ID="cbGroupBuy" runat="server" />团购</label></li>
                        <li><label><asp:CheckBox ID="cbCountDown" runat="server" />限时抢购</label></li>

					</ul>
				</div>
                	<div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">优惠券：</li>
						<li><label><asp:CheckBox ID="cbCoupons" runat="server" />优惠券</label></li>
					</ul>
				</div>
             </div>
			
			<div style="clear:both;margin-top:40px;font-weight:700;color:#000066"><label><asp:CheckBox ID="cbFinancial" runat="server"  />财务管理</label></div>
			<div class="PurviewItemDivide"></div>
             <div style="padding-left:20px;">
                 <div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">会员预付款：</li>
						<li><label><asp:CheckBox ID="cbAccountSummary" runat="server" />账户查询</label></li>
						<li><label><asp:CheckBox ID="cbReCharge" runat="server" />账户加款</label></li>
						<li><label><asp:CheckBox ID="cbBalanceDrawRequest" runat="server" />提现申请明细</label></li>
					</ul>					
				</div>
                <div class="PurviewItem">
						<ul>
						<li style="width:100px;font-weight:700;">分销商预付款：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbDistributorAccount" runat="server" />账户查询</label></li>
						<li><label><asp:CheckBox ID="cbDistributorReCharge" runat="server" />账户加款</label></li>
						<li><label><asp:CheckBox ID="cbDistributorBalanceDrawRequest" runat="server" />提现申请明细</label></li>	
						</ul>
			    </div>
                	
				<div class="PurviewItem">
					<ul>
					    <li style="width:91px;font-weight:700;">预付款报表：</li>
                        <li><label><asp:CheckBox ID="cbBalanceDetailsStatistics" runat="server" />预付款报表</label></li>
						<li><label><asp:CheckBox ID="cbBalanceDrawRequestStatistics" runat="server" />提现报表</label></li>
						
					</ul>
				</div>
             </div>


            <div style="clear:both;margin-top:40px;font-weight:700;color:#000066"><label><asp:CheckBox ID="cbTotalReport" runat="server"  />统计报表</label></div>
			<div class="PurviewItemDivide"></div>
             <div style="padding-left:20px;">
                 <div class="PurviewItem">
					<ul>
					    <li style="width:90px;font-weight:700;">零售业务统计：</li>
						<li><label><asp:CheckBox ID="cbSaleTotalStatistics" runat="server" />生意报告</label></li>
						<li><label><asp:CheckBox ID="cbUserOrderStatistics" runat="server" />订单统计</label></li>
						<li><label><asp:CheckBox ID="cbSaleList" runat="server" />销售明细</label></li>
						<li><label><asp:CheckBox ID="cbSaleTargetAnalyse" runat="server" />销售指标分析</label></li>
					</ul>
					<ol>
						<li><label><asp:CheckBox ID="cbProductSaleRanking" runat="server" />商品销售排行</label></li>
						<li style="width:130px;"><label><asp:CheckBox ID="cbProductSaleStatistics" runat="server" />商品访问与购买次数 </label></li>
                        <li><label><asp:CheckBox ID="cbMemberRanking" runat="server" />会员消费排行</label></li>
						<li style="width:110px;"><label><asp:CheckBox ID="cbMemberArealDistributionStatistics" runat="server" />会员分布统计</label></li>
						<li style="width:110px;"><label><asp:CheckBox ID="cbUserIncreaseStatistics" runat="server" />会员增长统计</label></li>
					</ol>			
				</div>
         			    
			    
			    <div class="PurviewItem">
						<ul>
						<li style="width:100px;font-weight:700;">分销统计：</li>
						<li style="margin-left:-10px;"><label><asp:CheckBox ID="cbDistributionReport" runat="server" />分销生意报告</label></li>
						<li><label><asp:CheckBox ID="cbPurchaseOrderStatistics" runat="server" />采购单统计</label></li>
						<li><label><asp:CheckBox ID="cbDistributionProductSaleRanking" runat="server" />商品销售排行</label></li>
						<li style="width:140px;"><label><asp:CheckBox ID="cbDistributorAchievementsRanking" runat="server" />分销商业绩排行</label></li>	
						</ul>
			    </div>


             </div>

             

			<div class="PurviewItemDivide"></div>
            
	        <div style="margin-top:10px;margin-bottom:10px;">
                <asp:Button ID="btnSet1" runat="server" Text="保 存" class="submit_queding" ></asp:Button>
                <input type="button" value="返 回" class="submit_queding" onclick="link()" />
	        </div>
            
        </div>
		</div>
		</div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<Hi:Script runat="server" Src="/admin/js/PrivilegeInRoles.js" />

    <script type="text/javascript" language="javascript">
        function link()
        {
            window.location.href='Roles.aspx';
        }
    </script>
</asp:Content>
