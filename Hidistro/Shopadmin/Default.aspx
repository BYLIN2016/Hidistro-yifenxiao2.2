<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.Default" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">



	  <div class="datalist">
	  <div class="businessinfo">
		<div class="leftbusiness">
			<div class="welcomemessage clearfix">
			<ul>
				<li class="managername"><span><asp:Literal runat="server" ID="ltrAdminName"></asp:Literal> 欢迎回来.</span>
					<span class="message"></span><asp:HyperLink ID="hpkMessages" runat="server" ></asp:HyperLink>
					<span class="zixun"></span><asp:HyperLink ID="hpkZiXun" runat="server" ></asp:HyperLink>
					<span class="liuyan"></span><asp:HyperLink ID="hpkLiuYan" runat="server" ></asp:HyperLink>
				</li>
				<li class="lastlogintime">上次登录：<Hi:FormatedTimeLabel ID="lblTime" runat="server"></Hi:FormatedTimeLabel></li>
			</ul>
			</div>
			<div class="orderlist clearfix">
				<div class="optiontitle">
					<ul>
						<li id="fen"><span><a href="Default.aspx?Status=0" style="color:BlueViolet;">采购单</a></span></li>
						<li id="hui"><span><a href="Default.aspx?Status=1" style="color:BlueViolet;">会员订单</a></span></li>
					</ul>
				</div>
				<input type="hidden" runat="server" id="hidStatus" />
				<div class="orderdata" id="divPurchaseOrders">
				
				
				 <UI:Grid ID="grdPurchaseOrders" runat="server" SortOrderBy="PurchaseDate" SortOrder="DESC" AutoGenerateColumns="False" DataKeyNames="PurchaseOrderId" HeaderStyle-CssClass="border_background"  GridLines="None" AllowSorting="true" Width="100%" ShowHeader="false">
                    <Columns>
                                                
                        <asp:TemplateField HeaderText="成交时间" SortExpression="PurchaseDate" HeaderStyle-CssClass="datetime">
                            <itemtemplate>
                               <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel>
                            </itemtemplate>
                        </asp:TemplateField>                        
                        
                        <asp:TemplateField HeaderText="采购单编号" HeaderStyle-CssClass="info">
                         <itemtemplate>
                               <asp:Label ID="lblPurchaseOrderID" Text='<%#Eval("PurchaseOrderId") %>' runat="server" ></asp:Label>                        
                            </itemtemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="采购单状态" HeaderStyle-CssClass="estate">
                            <itemtemplate>
                               <Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>'  />
                            </itemtemplate>
                        </asp:TemplateField>
                                             
                        <asp:TemplateField HeaderText="采购单操作" HeaderStyle-CssClass="handle">
                            <itemtemplate>
                              <a target="_blank" href='<%# "purchaseorder/MyPurchaseOrderDetails.aspx?purchaseOrderId="+Eval("PurchaseOrderId") %>'>详情</a><br />
	                           <span class="submit_faihuo">
                               <asp:HyperLink ID="lkbtnPay" runat="server" 
                               NavigateUrl='<%# Globals.ApplicationPath+ "/Shopadmin/purchaseOrder/ChoosePayment.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId")+"&PayMode="+Eval("PaymentTypeId") %>'  Target="_blank" Text="付款" Visible="false"></asp:HyperLink></span>
		      <span class="submit_tongyi"><asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%#Globals.ApplicationPath +  "/Shopadmin/sales/SendMyGoods.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank" Text="辅助发货" Visible="false"></asp:HyperLink> </span>
		      <div runat="server" visible="false" id="lkBtnCancelPurchaseOrder"><span class="submit_tongyi"><a href="javascript:ShowCloseDiv('<%# Eval("PurchaseOrderId")%>');">取消采购</a></span></div>
		       </itemtemplate>
		      </asp:TemplateField>
                       
                      
                    </Columns>
                    <EmptyDataTemplate><span class="empty">最近记录为空</span></EmptyDataTemplate>
                  </UI:Grid>
				</div>
				<input type="hidden" id="hidPurchaseOrderId" runat="server" />
			<div class="Pop_up" id="ClosePurchaseOrder" style="display:none;">
  <h1>关闭采购单 </h1>
  <div class="img_datala"><img src="./images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform fonts colorA borbac"><strong>取消采购单?请选择取消采购单的理由：</strong></div>
  <div class="mianform ">
    <ul>
      <li><span class="formitemtitle Pw_160">取消该采购单的理由：</span> <abbr class="formselect">
        <Hi:DistributorClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
      </abbr> </li>
    </ul>
    <ul class="up Pa_160">
      <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" OnClientClick="return ValidationCloseReason()" Text="确 定"   />
    </ul>
  </div>
</div>
			
			
			
				<div class="orderdata" id="divOrders">
					  <UI:Grid ID="grdOrders" runat="server" SortOrderBy="OrderDate" SortOrder="Desc" AutoGenerateColumns="False" DataKeyNames="OrderId" HeaderStyle-CssClass="border_background"  GridLines="None" ShowHeader="false" AllowSorting="true" Width="100%">
                    <Columns>
                     
                        <asp:TemplateField HeaderText="下单时间" SortExpression="OrderDate" HeaderStyle-CssClass="datetime" >
                            <itemtemplate>
                               <Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel>
                            </itemtemplate>
                        </asp:TemplateField>                        
                         <asp:TemplateField HeaderText="订单号" SortExpression="OrderId" HeaderStyle-CssClass="info" >
                         <itemtemplate>     
                            <asp:Label ID="lblPurchaseOrderID" Text='<%#Eval("OrderId") %>' runat="server" ></asp:Label> <asp:Literal ID="group" runat="server" Text='<%# Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' />
                         </itemtemplate> 
                         </asp:TemplateField>                        
                       
                        <asp:TemplateField HeaderText="订单状态" HeaderStyle-CssClass="estate" >
                            <itemtemplate>
                               <Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" />
                           </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="handle">
                            <itemtemplate>
                               <a href='<%# "sales/MyOrderDetails.aspx?OrderId="+Eval("OrderId") %>' target="_blank">详情</a><br />
	                           <asp:HyperLink ID="lkbtnEditPrice" runat="server" NavigateUrl='<%# Globals.ApplicationPath +  "/Shopadmin/sales/EditMyOrder.aspx?OrderId="+ Eval("OrderId") %>'  Target="_blank" Text="修改价格" Visible="false" ForeColor="Blue"></asp:HyperLink>
	                           <asp:HyperLink ID="lkbtnSendGoods" runat="server" NavigateUrl='<%# Globals.ApplicationPath +  "/Shopadmin/sales/SendMyGoods.aspx?OrderId="+ Eval("OrderId") %>' Target="_blank" Text="发货" Visible="false" ForeColor="Red"></asp:HyperLink>
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                     <EmptyDataTemplate><span class="empty">最近记录为空</span></EmptyDataTemplate>
                  </UI:Grid>            
				</div>
			</div>
			
			
			<div class="instantstat clearfix" id="divSendOrders">
				注：这里显示最近的 <asp:Label ID="lblOrderNumbers"  runat="server"></asp:Label> 笔订单，您共有<asp:HyperLink ID="hpksendOrder" runat="server"  Target="_blank"  ForeColor="Blue"></asp:HyperLink>笔订单待发货。<asp:HyperLink ID="allorders" runat="server"   Target="_blank" Text="查看所有会员订单"  ForeColor="Blue"></asp:HyperLink>
	                           
			</div>
			
			<div class="instantstat clearfix" id="divSendPurchseOrders">
				注：这里显示最近的<asp:Label ID="lblPurchaseOrderNumbers" runat="server"></asp:Label>笔采购单<asp:HyperLink ID="allPurchaseOrder" runat="server"   Target="_blank" Text="查看生成的采购单"  ForeColor="Blue"></asp:HyperLink>&nbsp;&nbsp;<asp:HyperLink ID="allPurchaseOrder2" runat="server" Target="_blank" Text="查看提交的采购单"  ForeColor="Blue"></asp:HyperLink>
			</div>
		</div>
		<div class="rightbusiness">
			<div class="statistics">
				<h3>统计</h3>
				<ul>
					<li><span class="explain">今日订单金额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblTodayOrderAmout" /></span></li>
					<li><span class="explain">今日销售利润(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblTodaySalesProfile" /></span></li>
					<li><span class="explain">今日新增会员：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrTodayAddMemberNumber" /></span></li>
					<li><span class="explain">待付款采购单：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrWaitSendPurchaseOrdersNumber" /></span></li>
					<li><span class="explain">待发货订单：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="ltrWaitSendOrdersNumber" /></span></li>
					<li><span class="explain">库存报警商品：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0" Id="lblProductCountTotal" /></span></li>
					<li><span class="explain">会员预付款总额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblMembersBalanceTotal" /></span></li>
					<li><span class="explain">我的预付款总额(元)：</span><span class="statresult"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblDistrosBalanceTotal" /></span></li>
				</ul>
			</div>
			<div class="blank12 clearfix"></div>
			<%--<div class="statistics clearfix">
				<h3>常见问题</h3>
				<ul>
					<li><a href="#">·易分销能做什么？</a></li>
					<li><a href="#">·如何进行分销商的域名绑定。</a></li>
					<li><a href="#">·为什么会员的预付款功能无法使用？</a></li>
					<li><a href="#">·如何设定分销商的样式？</a></li>
					<li><a href="#">·库存如何同步？</a></li>
					<li><a href="#">·分销商如何给我付款？</a></li>
				</ul>
			</div>--%>
		</div>
	  </div>
	  </div>

	  <div class="blank12 clearfix"></div>
	  
	  <script type="text/javascript">
	      $(document).ready(function() {

	          if ($("#ctl00_contentHolder_hidStatus").val() == "0") {
	              purchaseOrders();
	          }
	          else {
	              orders();
	          }
	      });
	      
	      
	      function orders() {
	          document.getElementById("divOrders").style.display = "block";
	          document.getElementById("divPurchaseOrders").style.display = "none";
	          document.getElementById("divSendOrders").style.display = "block";
	          document.getElementById("divSendPurchseOrders").style.display = "none";
	          document.getElementById("fen").className = "";
	          document.getElementById("hui").className = "menucurrent";
	      }


	      function purchaseOrders() {
	          document.getElementById("divOrders").style.display = "none";
	          document.getElementById("divPurchaseOrders").style.display = "block";
	          document.getElementById("divSendOrders").style.display = "none";
	          document.getElementById("divSendPurchseOrders").style.display = "block";
	          document.getElementById("fen").className = "menucurrent";
	          document.getElementById("hui").className = "";
	      }	     


	      function ValidationCloseReason() {
	          var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
	          if (reason == "请选择取消的理由") {
	              alert("请选择取消的理由");
	              return false;
	          }

	          return true;
	      }


	      function ShowCloseDiv(purchaseOrderId) {

	          $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);

	          DivWindowOpen(550, 200, 'ClosePurchaseOrder');
	      }

	     
	  </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
