<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="NoSiteDefault.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.NoSiteDefault" %>

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
					
				</li>
				<li class="lastlogintime">上次登录：<Hi:FormatedTimeLabel ID="lblTime" runat="server"></Hi:FormatedTimeLabel></li>
			</ul>
			</div>
			<div class="orderlist clearfix">
				<div class="optiontitle">
					<ul>
						<li class="menucurrent" id="fen"><span>采购单</span></li>
					</ul>
				</div>
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
                              <a target="_blank" href='<%# Globals.ApplicationPath+"/Shopadmin/purchaseOrder/MyPurchaseOrderDetails.aspx?purchaseOrderId="+Eval("PurchaseOrderId") %>'>详情</a><br />
	                           <span class="submit_faihuo">
                               <asp:HyperLink ID="lkbtnPay" runat="server" NavigateUrl='<%# Globals.ApplicationPath+ "/Shopadmin/purchaseOrder/ChoosePayment.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId")+"&PayMode="+Eval("PaymentTypeId") %>'  Target="_blank" Text="付款" Visible="false"></asp:HyperLink></span>
		      <div runat="server" visible="false" id="lkBtnCancelPurchaseOrder"><span class="submit_tongyi"><a href="javaxcript:void(0);" onclick="ShowCloseDiv('<%# Eval("PurchaseOrderId")%>')">取消采购</a></span></div>
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



				
			<div class="instantstat clearfix" id="divSendPurchseOrders">
				注：这里显示最近的<asp:Label ID="lblPurchaseOrderNumbers" runat="server"></asp:Label>笔采购单，您共有<asp:HyperLink ID="hpkWaitPayPurchaseOrder" runat="server"   Target="_blank"   ForeColor="Blue"></asp:HyperLink>笔采购单待付款。<asp:HyperLink ID="allPurchaseOrder" runat="server"   Target="_blank" Text="查看所有采购单"  ForeColor="Blue"></asp:HyperLink>
			</div>
		</div>
		</div>
		<div class="rightbusiness">

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
