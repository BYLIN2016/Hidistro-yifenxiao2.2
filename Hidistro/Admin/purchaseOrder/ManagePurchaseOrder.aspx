<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManagePurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManagePurchaseOrder"  %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>

<%@ Import Namespace="Hidistro.Membership.Context" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<div class="optiongroup mainwidth">
		<ul>
			<li id="anchors0"><asp:HyperLink ID="hlinkAllOrder" runat="server"><span>所有采购单</span></asp:HyperLink></li>
			<li id="anchors1"><asp:HyperLink ID="hlinkNotPay" runat="server"><span>等待付款</span></asp:HyperLink></li>
			<li id="anchors2"><asp:HyperLink ID="hlinkYetPay" runat="server"><span>等待发货</span></asp:HyperLink></li>
            <li id="anchors3"><asp:HyperLink ID="hlinkSendGoods" runat="server"><span>已发货</span></asp:HyperLink></li>     
            <li  id="anchors5"><asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>成功采购单</span></asp:HyperLink></li>       
            <li id="anchors4"><asp:HyperLink ID="hlinkClose" runat="server"><span>已关闭</span></asp:HyperLink></li>
            <li id="anchors99"><asp:HyperLink ID="hlinkHistory" runat="server"><span>历史采购单</span></asp:HyperLink></li>                                                                             
		</ul>
	</div>
	<!--选项卡-->
<input type="hidden" runat="server" id="lblPurchaseOrderId" />
<div class="dataarea mainwidth">
		<!--搜索-->
        <style>
        .searcharea ul li{ padding:5px 0px;}
        </style>
		<div class="searcharea clearfix br_search">
		  <ul  class="a_none_left">
		    <li> <span>选择时间段：</span><span>
		     <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">至</span> <span>
		       <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>分销商名称：</span><span>
		      <asp:TextBox ID="txtDistributorName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>商品名称：</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>订单编号：</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" />
		      </span></li>		      
		    <li><span>采购单编号：</span><span>
		      <asp:TextBox ID="txtPurchaseOrderId" runat="server" cssclass="forminput"></asp:TextBox> <asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>
		      <li><span>收货人：</span><span>
		      <asp:TextBox ID="txtShopTo" runat="server" cssclass="forminput" Width="113"></asp:TextBox>
		      </span></li> 
		      <li style=" margin-left:14px;"><span>配送方式：</span><span>
		        <abbr class="formselect"><hi:ShippingModeDropDownList runat="server" AllowNull="true" width="152" ID="shippingModeDropDownList" /></abbr>
		      </span></li>
		      <li  ><span>打印状态：</span><span>
		        <abbr class="formselect">
		        <asp:DropDownList runat="server" ID="ddlIsPrinted" width="152"/></abbr>
		      </span></li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
	        </li>
	      </ul>
  </div>
		<!--结束-->
      <div class="functionHandleArea clearfix m_none">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
           </div>
			</div>
			<!--结束-->
			 <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true"/></span>
		  <span class="printorder"><a href="javascript:printPosts();">批量打印快递单</a> </span>
		  <span class="printorder"><a href="javascript:printGoods();">批量打印发货单</a></span>
          <span class="downproduct"> <a href="javascript:downOrder()">下载配货单</a></span>
          <span class="sendproducts"><a href="javascript:sendGoods();">批量发货</a> </span>
		  </li>
        </ul>
      </div>
		</div>
		 <input type="hidden" id="hidPurchaseOrderId" runat="server" />
        		<!--数据列表区域-->
	  <div class="datalist">	  
	   <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId" Width="100%">
	   <HeaderTemplate>
	   <table width=" 0" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td width="24%" class="td_right td_left">分销商</td>
		      <td width="20%" class="td_right td_left">收货人</td>
		      <td width="12%" class="td_right td_left">订单实收款(元)</td>
		      <td width="12%" class="td_right td_left">采购单实收款(元)</td>
		      <td width="18%" class="td_right td_left">采购状态</td>
		      <td width="12%" class="td_left td_right_fff">操作</td>
	        </tr>
	   </HeaderTemplate>
	  <ItemTemplate>	   
	        <tr class="td_bg">
		      <td><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("PurchaseOrderId") %>'>采购单编号：<%#Eval("PurchaseOrderId") %><%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "<br>物流单编号：" + Eval("ShipOrderNumber")%></td>
		      <td>提交时间：<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td><%# (bool)Eval("IsPrinted")?"已打印":"未打印" %></td>
		      <td><%#Eval("PaymentType") %>&nbsp;</td>
		      <td><Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>'  /></td>
		      <td align="right"><a href="javascript:RemarkPurchaseOrder('<%#Eval("PurchaseOrderId") %>','<%#Eval("OrderId") %>','<%#Eval("PurchaseDate") %>','<%#Eval("PurchaseTotal") %>','<%#Eval("ManagerMark") %>','<%#  Eval("ManagerRemark") %>')"><Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a></td>
	        </tr>  
		    <tr>
		      <td>&nbsp;<%#Eval("Distributorname") %> <Hi:WangWangConversations runat="server" ID="WangWangConversations"  WangWangAccounts='<%#Eval("DistributorWangwang") %>'/>  </td>
		      <td><%#Eval("ShipTo") %>&nbsp;</td>
		      <td><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" /></td>
		      <td><Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" />
		        <span runat="server" visible="false" id="lkbtnEditPurchaseOrder" class="Name"><a href="javascript:void(0);" onclick="OpenWindow('<%# Eval("PurchaseOrderId")%>','<%# Eval("PurchaseTotal")%>', '<%# Eval("AdjustedDiscount")%>')">修改采购单价格</a></span>
		      </td>
		      <td>&nbsp;	        
		        <span class="Name">
                    <a href='<%# "PurchaseOrderDetails.aspx?purchaseOrderId="+Eval("PurchaseOrderId") %>'>详情</a>
                </span>
		        <Hi:PurchaseOrderItemUpdateHyperLink ID="link_purcharprice" runat="server" PurchaseOrderId='<%# Eval("PurchaseOrderId") %>' PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' DistorUserId='<%# Eval("DistributorId") %>' Text="修改采购商品" />  
              </td>
		      <td>&nbsp;
                <a href="javascript:ClosePurchaseOrder('<%#Eval("PurchaseOrderId") %>');"><asp:Literal runat="server" ID="litClosePurchaseOrder" Visible="false" Text="关闭采购单" /></a> 
		          <asp:Label CssClass="submit_faihuo" id="lkbtnSendGoods" Visible="false" runat="server" >
                          <a style="color:Red" href="javascript:DialogFrame('<%# "purchaseOrder/SendPurchaseOrderGoods.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>','采购单发货',null,null)">发货</a>
                   </asp:Label>                   
		           <span class="Name"><Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="我已线下收款" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false" ForeColor="Red"></Hi:ImageLinkButton>
		        <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" 
                Text="完成采购单" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE"  
                DeleteMsg="确认要完成该采购单吗？" Visible="false" ForeColor="Red" />&nbsp;
                 <a style="color: Red"></a><a href="javascript:void(0)" onclick="return CheckRefund(this.title)"
                                    runat="server" id="lkbtnCheckPurchaseRefund" visible="false" title='<%# Eval("PurchaseOrderId") %>'>
                                    确认退款</a> <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                        id="lkbtnCheckPurchaseReturn" visible="false" title='<%# Eval("PurchaseOrderId") %>'>确认退货</a>
                                <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server"
                                    id="lkbtnCheckPurchaseReplace" visible="false" title='<%# Eval("PurchaseOrderId") %>'>确认换货</a>
		      </td>
	        </tr>
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>
	   </asp:DataList>    
      <div class="blank12 clearfix"></div>
    <!--数据列表底部功能区域-->
	  <div class="page">
	  <div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
               </div>

			</div>
		</div>
      </div>
  </div>
</div>

	<div class="databottom"></div>
	
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

 <!--修改价格-->
 <div id="EditPurchaseOrder" style="display:none">
    <div class="frame-content">
        <p><span class="frame-span frame-input110">采购单原实收款：</span><em><asp:Label ID="lblPurchaseOrderAmount" Text="22"  runat="server"/></em>元</p>
        <p><span class="frame-span frame-input110">涨价或折扣：<em>*</em></span><asp:TextBox ID="txtPurchaseOrderDiscount" runat="server" cssclass="forminput" onblur="ChargeAmount()" /> </p>
        <b>负数代表折扣，正数代表涨价</b>
        <p>
            <span class="frame-span frame-input110">分销商实付:</span> 
            <asp:Label ID="lblPurchaseOrderAmount1" Text="22" runat="server" /><span>+</span>
            <asp:Label ID="lblPurchaseOrderAmount2" Text="22" runat="server" /><span>=</span>
            <em><asp:Label ID="lblPurchaseOrderAmount3" Text="22"  runat="server" /></em>
        </p>
        <b>分销商实付：采购单原实收款+涨价或折扣</b>
    </div>
 </div>
      
  <!--编辑备注信息-->
 <div  id="RemarkPurchaseOrder"  style="display:none;">
      <div class="frame-content">
          <p><span class="frame-span frame-input130">订单编号：</span><span id="spanOrderId" runat="server" /></p>
          <p><span class="frame-span frame-input130">采购单编号：</span><span id="spanpurcharseOrderId" runat="server" /></p>
          <p><span class="frame-span frame-input130">交时间：</span><span id="lblpurchaseDateForRemark" runat="server" /></p>
          <p><span class="frame-span frame-input130">采购单实收款(元)：</span><Hi:FormatedMoneyLabel ID="lblpurchaseTotalForRemark" runat="server" /></p>
          <span class="frame-span frame-input130">标志：</span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" CssClass="frame-input-radio" />
          <p><span class="frame-span frame-input130">备忘录:</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></p>
      </div>  
  </div>

<!---关闭采购单-->
 <div id="closePurchaseOrder" style="display:none;">
         <div class="frame-content">
              <p><em>关闭交易?请您确认已经通知分销商,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷</em></p>
              <p><span class="frame-span input130">关闭采购单理由<em>*</em></span><Hi:ClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" /></p>
         </div>
</div>
<div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align:center;">
            <input type="button" id="btnorderph" onclick="javascript:Setordergoods();" class="submit_DAqueding" value="采购单配货表"/>
        &nbsp;
        <input type="button" id="Button1" onclick="javascript:Setproductgoods();" class="submit_DAqueding" value="商品配货表"/>
            <p>导出内容只包括等待发货状态的采购单</p>
            <p>采购单配货表不会合并相同的商品,商品配货表则会合并相同的商品。</p>
        </div>
    </div>
<!--确认退款--->
    <div id="CheckRefund" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：1.分销商已付款完成，并确认无误；2.确认分销商的申请退款方式。</em></p>
            <p>
                <span class="frame-span frame-input100">采购单号:</span> <span>
                    <asp:Label ID="refund_lblPurchaseOrderId" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">采购单金额:</span> <span>
                    <asp:Label ID="lblPurchaseOrderTotal" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">分销商退款方式:</span> <span>
                    <asp:Label ID="lblRefundType" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">退款原因:</span> <span>
                    <asp:Label ID="lblRefundRemark" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系人:</span> <span>
                    <asp:Label ID="lblContacts" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">电子邮件:</span> <span>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系电话:</span> <span>
                    <asp:Label ID="lblTelephone" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系地址:</span> <span>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">管理员备注:</span> <span>
                    <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="forminput" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button2" onclick="javascript:acceptRefund();" class="submit_DAqueding"
                    value="确认退款" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript:refuseRefund();" class="submit_DAqueding"
                    value="拒绝退款" />
            </div>
        </div>
    </div>
    <!--确认退货--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
            <p>
                <em>执行本操作前确保：1.已收到买家寄换回来的货品，并确认无误；
                    2.确认买家的申请退款方式。</em></p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
              <tr>
                <td align="right" width="30%">采购单号:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblPurchaseOrderId" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">采购单金额:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblPurchaseOrderTotal" runat="server" /></td>
              </tr>
              <tr>
                <td align="right">买家退款方式:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">退货原因:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系人:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">电子邮件:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblEmail" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系电话:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">联系地址:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">退款金额:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" /></td>
              </tr>
              <tr>
              <td align="right">管理员备注:</td>
              <td align="left"  class="bd_td">&nbsp;<asp:TextBox ID="return_txtAdminRemark" runat="server" Width="243"/></td>
              </tr>
            </table>
          
            <p>
                <span class="frame-span frame-input100"  style="margin-left:10px;"></span> <span >
                    </span></p>
            
            <div style="text-align: center; padding-top:10px;">
                <input type="button" id="Button4" onclick="javascript:acceptReturn();" class="submit_DAqueding"
                    value="确认退货" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript:refuseReturn();" class="submit_DAqueding"
                    value="拒绝退货" />
            </div>
        </div>
    </div>
    <!--确认换货--->
    <div id="CheckReplace" style="display: none;">
        <div class="frame-content">
            <p>
                <em>执行本操作前确保：1.已收到分销商寄还回来的货品，并确认无误；<br />
                    </em></p>
            <p>
                <span class="frame-span frame-input100">采购单号:</span> <span>
                    <asp:Label ID="replace_lblOrderId" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">采购单金额:</span> <span>
                    <asp:Label ID="replace_lblOrderTotal" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">换货备注:</span> <span>
                    <asp:Label ID="replace_lblComments" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系人:</span> <span>
                    <asp:Label ID="replace_lblContacts" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">电子邮件:</span> <span>
                    <asp:Label ID="replace_lblEmail" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系电话:</span> <span>
                    <asp:Label ID="replace_lblTelephone" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">联系地址:</span> <span>
                    <asp:Label ID="replace_lblAddress" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">邮政编码:</span> <span>
                    <asp:Label ID="replace_lblPostCode" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">管理员备注:</span> <span>
                    <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="forminput" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button6" onclick="javascript:acceptReplace();" class="submit_DAqueding"
                    value="确认换货" />
                &nbsp;
                <input type="button" id="Button7" onclick="javascript:refuseReplace();" class="submit_DAqueding"
                    value="拒绝换货" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" Text="关闭订单" />
        <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="确认退款" />
        <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="拒绝退款" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" />
        <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="确认换货" />
        <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="拒绝换货" />
    <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" Text="关闭采购单"   />
    <asp:Button runat="server" ID="btnRemark" Text="编辑备注" CssClass="submit_DAqueding"/>
    <asp:Button ID="btnEditOrder"  runat="server"  Text="修改采购单价格" CssClass="submit_DAqueding"   /> 
                <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="订单配货表" />&nbsp;
            <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="submit_DAqueding" /> 
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script src="purchaseorder.helper.js" type="text/javascript"></script>
 <script type="text/javascript">
    var formtype = "";
     function ConfirmPayOrder() {
         return confirm("如果分销商已经通过其他途径支付了采购单款项，您可以使用此操作修改采购单状态\n\n此操作成功完成以后，采购单的当前状态将变为已付款状态，确认分销商已付款？");
     }

     function showOrderState() {
         var status;
         if (navigator.appName.indexOf("Explorer") > -1) {

             status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

         } else {

             status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

         }
         if (status != "0") {
             document.getElementById("anchors0").className = 'optionstar';
         }
         if (status != "99") {
             document.getElementById("anchors99").className = 'optionend';
         }
         document.getElementById("anchors" + status).className = 'menucurrent';
         if ($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val("")) {
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html("0.00");
         }
         initValid(new InputValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', 1, 10, true, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '折扣只能是数值，且不能超过2位小数'))
         appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', -10000000, 10000000, '折扣只能是数值，不能超过10000000，且不能超过2位小数'));

         var pathurl = $("#ctl00_contentHolder_dlstPurchaseOrders_ctl01_link_purcharprice").attr("href");
         $("#ctl00_contentHolder_dlstPurchaseOrders_ctl01_link_purcharprice").attr("href", "javascript:DialogFrame('" + pathurl + "','修改采购商品',null,null);")
     }

     $(document).ready(function() { showOrderState(); });


         //修改采购单价格
     function OpenWindow(PurchaseOrderId, PurchaseTotal, AdjustedDiscount) {
             formtype = "updateprice";
             arrytext = null;

             $("#ctl00_contentHolder_lblPurchaseOrderId").val(PurchaseOrderId);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount").html(Math.floor((eval(PurchaseTotal) - eval(AdjustedDiscount))*100)/100);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount1").html(Math.floor((eval(PurchaseTotal) - eval(AdjustedDiscount)) * 100)/100);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(eval(PurchaseTotal));
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html(eval(AdjustedDiscount));

             DialogShow("修改采购单价格", 'updateprice', 'EditPurchaseOrder', 'ctl00_contentHolder_btnEditOrder');
         }

         //编辑备注
         function RemarkPurchaseOrder(purchaseOrderId, orderId, purchaseDate, purchaseTotal, managerMark, managerRemark) {
             formtype = "remark";
             arrytext = null;

             $("#ctl00_contentHolder_spanOrderId").html(orderId);
             $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
             $("#ctl00_contentHolder_spanpurcharseOrderId").html(purchaseOrderId);
             $("#ctl00_contentHolder_lblpurchaseDateForRemark").html(purchaseDate);
             $("#ctl00_contentHolder_lblpurchaseTotalForRemark").html(purchaseTotal);
             $("#ctl00_contentHolder_txtRemark").val(managerRemark);

            

             for (var i = 0; i <= 5; i++) {
                 if (document.getElementById("ctl00_contentHolder_orderRemarkImageForRemark_" + i).value == managerMark) {
                     setArryText("ctl00_contentHolder_orderRemarkImageForRemark_" + i, "true");
                     $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", true);
                    
                 }
                 else {
                     $("#ctl00_contentHolder_orderRemarkImageForRemark_" + i).attr("check", false);
                 }
             }
             setArryText("ctl00_contentHolder_txtRemark", managerRemark);
             DialogShow("修改备注", 'updateremark', 'RemarkPurchaseOrder', 'ctl00_contentHolder_btnRemark');
         }


         function ChargeAmount() {
            var reg = /^\-?([1-9]\d*|0)(\.\d+)?$/;
            if (($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val() != "") && reg.test($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val())) {
                $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html($("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
                var amount1 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount").html());
                var amount2 = parseFloat($("#ctl00_contentHolder_lblPurchaseOrderAmount2").html());

                var amount3 = amount1 + amount2;
                $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(amount3);
            }
        }
        
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                alert("请选择关闭的理由");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }
        //关闭采购单
        function ClosePurchaseOrder(purchaseOrderId) {
            formtype = "close";
            arrytext = null;
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DialogShow("关闭采购单", 'closepurchar', 'closePurchaseOrder', 'ctl00_contentHolder_btnClosePurchaseOrder');
         }
         function Setordergoods() {
             $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
         }
         function Setproductgoods() {
             $("#ctl00_contentHolder_btnProductGoods").trigger("click");
         }
         //退回采购单理由
         function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "请选择退回的理由") {
                 alert("请选择退回的理由");
                 return false;
             }
             setArryText("ctl00_contentHolder_ddlCloseReason", reason);
             return true;
         }

         //打印发货单
         function printGoods() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function() {
                 orderIds += $(this).val() + ",";
             });
             if (orderIds == "") {
                 alert("请选要打印的发货单");
             }
             else {
                 var url = "/Admin/purchaseOrder/BatchPrintSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + orderIds;
                 window.open(url, "批量打印发货单", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no");
                
             }
         }

         //打印的快递单
         function printPosts() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function() {
                 orderIds += $(this).val() + ",";
             }
             );
             if (orderIds == "") {
                 alert("请选要打印的快递单");
             }
             else {
                 var url = "purchaseOrder/BatchPrintPurchaseData.aspx?PurchaseOrderIds=" + orderIds;
                 DialogFrame(url, "批量打印快递单", null, null);
             }
         }

         //批量发货
         function sendGoods() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function () {
                 orderIds += $(this).val() + ",";
             });
             if (orderIds == "") {
                 alert("请选要批量发货的采购单");
             } else {
                orderIds=orderIds.substring(0,orderIds.length-1);
                 var url = "purchaseOrder/BatchSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + orderIds;
                 DialogFrame(url, "批量发货", null, null);
             }
         
         }

         //验证
         function validatorForm() {
             switch (formtype) {
                 case "remark":
                     arrytext = null;
                     $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                     if ($radioId == null || $radioId == "undefined") {
                         alert('请先标记备注');
                         return false;
                     }
                     setArryText($radioId.id, "true");
                     setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                     break;
                 case "close":
                     return ValidationCloseReason();
                     break;
                 case "updateprice":
                     setArryText("ctl00_contentHolder_txtPurchaseOrderDiscount", $("#ctl00_contentHolder_txtPurchaseOrderDiscount").val());
                     break;
             };
             return true;
         }
         // 下载配货单
         function downOrder() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function () {
                 orderIds += $(this).val() + ",";
             }
             );
             if (orderIds == "") {
                 alert("请选要下载配货单的采购单");
             }
             else {
                 ShowMessageDialog("下载配货批次表", "downorder", "DownOrder");
             }
         }
         $(function () {
             $(".datalist img[src$='tui.gif']").each(function (item, i) {
                 $parent_link = $(this).parent();
                 $parent_link.attr("href", "javascript:DialogFrame('purchaseOrder/" + $parent_link.attr("href") + "','退款详细信息',null,null)");
             });
         });
     </script>
</asp:Content>
