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

<!--ѡ�-->
	<div class="optiongroup mainwidth">
		<ul>
			<li id="anchors0"><asp:HyperLink ID="hlinkAllOrder" runat="server"><span>���вɹ���</span></asp:HyperLink></li>
			<li id="anchors1"><asp:HyperLink ID="hlinkNotPay" runat="server"><span>�ȴ�����</span></asp:HyperLink></li>
			<li id="anchors2"><asp:HyperLink ID="hlinkYetPay" runat="server"><span>�ȴ�����</span></asp:HyperLink></li>
            <li id="anchors3"><asp:HyperLink ID="hlinkSendGoods" runat="server"><span>�ѷ���</span></asp:HyperLink></li>     
            <li  id="anchors5"><asp:HyperLink ID="hlinkTradeFinished" runat="server" Text=""><span>�ɹ��ɹ���</span></asp:HyperLink></li>       
            <li id="anchors4"><asp:HyperLink ID="hlinkClose" runat="server"><span>�ѹر�</span></asp:HyperLink></li>
            <li id="anchors99"><asp:HyperLink ID="hlinkHistory" runat="server"><span>��ʷ�ɹ���</span></asp:HyperLink></li>                                                                             
		</ul>
	</div>
	<!--ѡ�-->
<input type="hidden" runat="server" id="lblPurchaseOrderId" />
<div class="dataarea mainwidth">
		<!--����-->
        <style>
        .searcharea ul li{ padding:5px 0px;}
        </style>
		<div class="searcharea clearfix br_search">
		  <ul  class="a_none_left">
		    <li> <span>ѡ��ʱ��Σ�</span><span>
		     <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		      </span> <span class="Pg_1010">��</span> <span>
		       <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </span></li>
		    <li><span>���������ƣ�</span><span>
		      <asp:TextBox ID="txtDistributorName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>��Ʒ���ƣ�</span><span>
		      <asp:TextBox ID="txtProductName" runat="server" cssclass="forminput" />
		      </span></li>
		      <li><span>������ţ�</span><span>
		      <asp:TextBox ID="txtOrderId" runat="server" cssclass="forminput" />
		      </span></li>		      
		    <li><span>�ɹ�����ţ�</span><span>
		      <asp:TextBox ID="txtPurchaseOrderId" runat="server" cssclass="forminput"></asp:TextBox> <asp:Label ID="lblStatus" runat="server" style="display:none;"></asp:Label>
		      </span></li>
		      <li><span>�ջ��ˣ�</span><span>
		      <asp:TextBox ID="txtShopTo" runat="server" cssclass="forminput" Width="113"></asp:TextBox>
		      </span></li> 
		      <li style=" margin-left:14px;"><span>���ͷ�ʽ��</span><span>
		        <abbr class="formselect"><hi:ShippingModeDropDownList runat="server" AllowNull="true" width="152" ID="shippingModeDropDownList" /></abbr>
		      </span></li>
		      <li  ><span>��ӡ״̬��</span><span>
		        <abbr class="formselect">
		        <asp:DropDownList runat="server" ID="ddlIsPrinted" width="152"/></abbr>
		      </span></li>
		    <li>
		      <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="��ѯ" />
	        </li>
	      </ul>
  </div>
		<!--����-->
      <div class="functionHandleArea clearfix m_none">
			<!--��ҳ����-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
           </div>
			</div>
			<!--����-->
			 <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">��ѡ</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="ɾ��" IsShow="true"/></span>
		  <span class="printorder"><a href="javascript:printPosts();">������ӡ��ݵ�</a> </span>
		  <span class="printorder"><a href="javascript:printGoods();">������ӡ������</a></span>
          <span class="downproduct"> <a href="javascript:downOrder()">���������</a></span>
          <span class="sendproducts"><a href="javascript:sendGoods();">��������</a> </span>
		  </li>
        </ul>
      </div>
		</div>
		 <input type="hidden" id="hidPurchaseOrderId" runat="server" />
        		<!--�����б�����-->
	  <div class="datalist">	  
	   <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId" Width="100%">
	   <HeaderTemplate>
	   <table width=" 0" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td width="24%" class="td_right td_left">������</td>
		      <td width="20%" class="td_right td_left">�ջ���</td>
		      <td width="12%" class="td_right td_left">����ʵ�տ�(Ԫ)</td>
		      <td width="12%" class="td_right td_left">�ɹ���ʵ�տ�(Ԫ)</td>
		      <td width="18%" class="td_right td_left">�ɹ�״̬</td>
		      <td width="12%" class="td_left td_right_fff">����</td>
	        </tr>
	   </HeaderTemplate>
	  <ItemTemplate>	   
	        <tr class="td_bg">
		      <td><input name="CheckBoxGroup" type="checkbox" value='<%#Eval("PurchaseOrderId") %>'>�ɹ�����ţ�<%#Eval("PurchaseOrderId") %><%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "<br>��������ţ�" + Eval("ShipOrderNumber")%></td>
		      <td>�ύʱ�䣺<Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("PurchaseDate") %>' ShopTime="true" runat="server" ></Hi:FormatedTimeLabel></td>
		      <td><%# (bool)Eval("IsPrinted")?"�Ѵ�ӡ":"δ��ӡ" %></td>
		      <td><%#Eval("PaymentType") %>&nbsp;</td>
		      <td><Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>'  /></td>
		      <td align="right"><a href="javascript:RemarkPurchaseOrder('<%#Eval("PurchaseOrderId") %>','<%#Eval("OrderId") %>','<%#Eval("PurchaseDate") %>','<%#Eval("PurchaseTotal") %>','<%#Eval("ManagerMark") %>','<%#  Eval("ManagerRemark") %>')"><Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></a></td>
	        </tr>  
		    <tr>
		      <td>&nbsp;<%#Eval("Distributorname") %> <Hi:WangWangConversations runat="server" ID="WangWangConversations"  WangWangAccounts='<%#Eval("DistributorWangwang") %>'/>  </td>
		      <td><%#Eval("ShipTo") %>&nbsp;</td>
		      <td><Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" /></td>
		      <td><Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" />
		        <span runat="server" visible="false" id="lkbtnEditPurchaseOrder" class="Name"><a href="javascript:void(0);" onclick="OpenWindow('<%# Eval("PurchaseOrderId")%>','<%# Eval("PurchaseTotal")%>', '<%# Eval("AdjustedDiscount")%>')">�޸Ĳɹ����۸�</a></span>
		      </td>
		      <td>&nbsp;	        
		        <span class="Name">
                    <a href='<%# "PurchaseOrderDetails.aspx?purchaseOrderId="+Eval("PurchaseOrderId") %>'>����</a>
                </span>
		        <Hi:PurchaseOrderItemUpdateHyperLink ID="link_purcharprice" runat="server" PurchaseOrderId='<%# Eval("PurchaseOrderId") %>' PurchaseStatusCode='<%# Eval("PurchaseStatus") %>' DistorUserId='<%# Eval("DistributorId") %>' Text="�޸Ĳɹ���Ʒ" />  
              </td>
		      <td>&nbsp;
                <a href="javascript:ClosePurchaseOrder('<%#Eval("PurchaseOrderId") %>');"><asp:Literal runat="server" ID="litClosePurchaseOrder" Visible="false" Text="�رղɹ���" /></a> 
		          <asp:Label CssClass="submit_faihuo" id="lkbtnSendGoods" Visible="false" runat="server" >
                          <a style="color:Red" href="javascript:DialogFrame('<%# "purchaseOrder/SendPurchaseOrderGoods.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId") %>','�ɹ�������',null,null)">����</a>
                   </asp:Label>                   
		           <span class="Name"><Hi:ImageLinkButton ID="lkbtnPayOrder" runat="server" Text="���������տ�" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="CONFIRM_PAY" OnClientClick="return ConfirmPayOrder()" Visible="false" ForeColor="Red"></Hi:ImageLinkButton>
		        <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" 
                Text="��ɲɹ���" CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE"  
                DeleteMsg="ȷ��Ҫ��ɸòɹ�����" Visible="false" ForeColor="Red" />&nbsp;
                 <a style="color: Red"></a><a href="javascript:void(0)" onclick="return CheckRefund(this.title)"
                                    runat="server" id="lkbtnCheckPurchaseRefund" visible="false" title='<%# Eval("PurchaseOrderId") %>'>
                                    ȷ���˿�</a> <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                        id="lkbtnCheckPurchaseReturn" visible="false" title='<%# Eval("PurchaseOrderId") %>'>ȷ���˻�</a>
                                <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server"
                                    id="lkbtnCheckPurchaseReplace" visible="false" title='<%# Eval("PurchaseOrderId") %>'>ȷ�ϻ���</a>
		      </td>
	        </tr>
	   </ItemTemplate>
	   <FooterTemplate>
	   </table>
	   </FooterTemplate>
	   </asp:DataList>    
      <div class="blank12 clearfix"></div>
    <!--�����б�ײ���������-->
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
  <!--����logo����-->
</div>

 <!--�޸ļ۸�-->
 <div id="EditPurchaseOrder" style="display:none">
    <div class="frame-content">
        <p><span class="frame-span frame-input110">�ɹ���ԭʵ�տ</span><em><asp:Label ID="lblPurchaseOrderAmount" Text="22"  runat="server"/></em>Ԫ</p>
        <p><span class="frame-span frame-input110">�Ǽۻ��ۿۣ�<em>*</em></span><asp:TextBox ID="txtPurchaseOrderDiscount" runat="server" cssclass="forminput" onblur="ChargeAmount()" /> </p>
        <b>���������ۿۣ����������Ǽ�</b>
        <p>
            <span class="frame-span frame-input110">������ʵ��:</span> 
            <asp:Label ID="lblPurchaseOrderAmount1" Text="22" runat="server" /><span>+</span>
            <asp:Label ID="lblPurchaseOrderAmount2" Text="22" runat="server" /><span>=</span>
            <em><asp:Label ID="lblPurchaseOrderAmount3" Text="22"  runat="server" /></em>
        </p>
        <b>������ʵ�����ɹ���ԭʵ�տ�+�Ǽۻ��ۿ�</b>
    </div>
 </div>
      
  <!--�༭��ע��Ϣ-->
 <div  id="RemarkPurchaseOrder"  style="display:none;">
      <div class="frame-content">
          <p><span class="frame-span frame-input130">������ţ�</span><span id="spanOrderId" runat="server" /></p>
          <p><span class="frame-span frame-input130">�ɹ�����ţ�</span><span id="spanpurcharseOrderId" runat="server" /></p>
          <p><span class="frame-span frame-input130">��ʱ�䣺</span><span id="lblpurchaseDateForRemark" runat="server" /></p>
          <p><span class="frame-span frame-input130">�ɹ���ʵ�տ�(Ԫ)��</span><Hi:FormatedMoneyLabel ID="lblpurchaseTotalForRemark" runat="server" /></p>
          <span class="frame-span frame-input130">��־��</span><Hi:OrderRemarkImageRadioButtonList runat="server" ID="orderRemarkImageForRemark" CssClass="frame-input-radio" />
          <p><span class="frame-span frame-input130">����¼:</span><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" /></p>
      </div>  
  </div>

<!---�رղɹ���-->
 <div id="closePurchaseOrder" style="display:none;">
         <div class="frame-content">
              <p><em>�رս���?����ȷ���Ѿ�֪ͨ������,���Ѵ��һ�����,��������رս���,�����ܵ��½��׾���</em></p>
              <p><span class="frame-span input130">�رղɹ�������<em>*</em></span><Hi:ClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" /></p>
         </div>
</div>
<div id="DownOrder" style="display: none;">
        <div class="frame-content" style="text-align:center;">
            <input type="button" id="btnorderph" onclick="javascript:Setordergoods();" class="submit_DAqueding" value="�ɹ��������"/>
        &nbsp;
        <input type="button" id="Button1" onclick="javascript:Setproductgoods();" class="submit_DAqueding" value="��Ʒ�����"/>
            <p>��������ֻ�����ȴ�����״̬�Ĳɹ���</p>
            <p>�ɹ����������ϲ���ͬ����Ʒ,��Ʒ��������ϲ���ͬ����Ʒ��</p>
        </div>
    </div>
<!--ȷ���˿�--->
    <div id="CheckRefund" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ִ�б�����ǰȷ����1.�������Ѹ�����ɣ���ȷ������2.ȷ�Ϸ����̵������˿ʽ��</em></p>
            <p>
                <span class="frame-span frame-input100">�ɹ�����:</span> <span>
                    <asp:Label ID="refund_lblPurchaseOrderId" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">�ɹ������:</span> <span>
                    <asp:Label ID="lblPurchaseOrderTotal" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">�������˿ʽ:</span> <span>
                    <asp:Label ID="lblRefundType" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">�˿�ԭ��:</span> <span>
                    <asp:Label ID="lblRefundRemark" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��ϵ��:</span> <span>
                    <asp:Label ID="lblContacts" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">�����ʼ�:</span> <span>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��ϵ�绰:</span> <span>
                    <asp:Label ID="lblTelephone" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��ϵ��ַ:</span> <span>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">����Ա��ע:</span> <span>
                    <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="forminput" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button2" onclick="javascript:acceptRefund();" class="submit_DAqueding"
                    value="ȷ���˿�" />
                &nbsp;
                <input type="button" id="Button3" onclick="javascript:refuseRefund();" class="submit_DAqueding"
                    value="�ܾ��˿�" />
            </div>
        </div>
    </div>
    <!--ȷ���˻�--->
    <div id="CheckReturn" style="display: none;">
        <div class="frame-content" style="margin-top:-20px;">
            <p>
                <em>ִ�б�����ǰȷ����1.���յ���ҼĻ������Ļ�Ʒ����ȷ������
                    2.ȷ����ҵ������˿ʽ��</em></p>
            <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
              <tr>
                <td align="right" width="30%">�ɹ�����:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblPurchaseOrderId" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">�ɹ������:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblPurchaseOrderTotal" runat="server" /></td>
              </tr>
              <tr>
                <td align="right">����˿ʽ:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">�˻�ԭ��:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">��ϵ��:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">�����ʼ�:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblEmail" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">��ϵ�绰:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">��ϵ��ַ:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label></td>
              </tr>
              <tr>
                <td align="right">�˿���:</td>
                <td align="left"  class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" /></td>
              </tr>
              <tr>
              <td align="right">����Ա��ע:</td>
              <td align="left"  class="bd_td">&nbsp;<asp:TextBox ID="return_txtAdminRemark" runat="server" Width="243"/></td>
              </tr>
            </table>
          
            <p>
                <span class="frame-span frame-input100"  style="margin-left:10px;"></span> <span >
                    </span></p>
            
            <div style="text-align: center; padding-top:10px;">
                <input type="button" id="Button4" onclick="javascript:acceptReturn();" class="submit_DAqueding"
                    value="ȷ���˻�" />
                &nbsp;
                <input type="button" id="Button5" onclick="javascript:refuseReturn();" class="submit_DAqueding"
                    value="�ܾ��˻�" />
            </div>
        </div>
    </div>
    <!--ȷ�ϻ���--->
    <div id="CheckReplace" style="display: none;">
        <div class="frame-content">
            <p>
                <em>ִ�б�����ǰȷ����1.���յ������̼Ļ������Ļ�Ʒ����ȷ������<br />
                    </em></p>
            <p>
                <span class="frame-span frame-input100">�ɹ�����:</span> <span>
                    <asp:Label ID="replace_lblOrderId" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">�ɹ������:</span> <span>
                    <asp:Label ID="replace_lblOrderTotal" runat="server" /></span></p>
            <p>
                <span class="frame-span frame-input100">������ע:</span> <span>
                    <asp:Label ID="replace_lblComments" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��ϵ��:</span> <span>
                    <asp:Label ID="replace_lblContacts" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">�����ʼ�:</span> <span>
                    <asp:Label ID="replace_lblEmail" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��ϵ�绰:</span> <span>
                    <asp:Label ID="replace_lblTelephone" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��ϵ��ַ:</span> <span>
                    <asp:Label ID="replace_lblAddress" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">��������:</span> <span>
                    <asp:Label ID="replace_lblPostCode" runat="server"></asp:Label></span></p>
            <p>
                <span class="frame-span frame-input100">����Ա��ע:</span> <span>
                    <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="forminput" /></span></p>
            <br />
            <div style="text-align: center;">
                <input type="button" id="Button6" onclick="javascript:acceptReplace();" class="submit_DAqueding"
                    value="ȷ�ϻ���" />
                &nbsp;
                <input type="button" id="Button7" onclick="javascript:refuseReplace();" class="submit_DAqueding"
                    value="�ܾ�����" />
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidOrderTotal" runat="server" />
        <input type="hidden" id="hidRefundType" runat="server" />
        <input type="hidden" id="hidRefundMoney" runat="server" />
        <input type="hidden" id="hidAdminRemark" runat="server" />
        <asp:Button ID="btnCloseOrder" runat="server" CssClass="submit_DAqueding" Text="�رն���" />
        <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="ȷ���˿�" />
        <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="�ܾ��˿�" />
        <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="ȷ���˻�" />
        <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="�ܾ��˻�" />
        <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="ȷ�ϻ���" />
        <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="�ܾ�����" />
    <asp:Button ID="btnClosePurchaseOrder"  runat="server" CssClass="submit_DAqueding" Text="�رղɹ���"   />
    <asp:Button runat="server" ID="btnRemark" Text="�༭��ע" CssClass="submit_DAqueding"/>
    <asp:Button ID="btnEditOrder"  runat="server"  Text="�޸Ĳɹ����۸�" CssClass="submit_DAqueding"   /> 
                <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="���������" />&nbsp;
            <asp:Button runat="server" ID="btnProductGoods" Text="��Ʒ�����" CssClass="submit_DAqueding" /> 
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script src="purchaseorder.helper.js" type="text/javascript"></script>
 <script type="text/javascript">
    var formtype = "";
     function ConfirmPayOrder() {
         return confirm("����������Ѿ�ͨ������;��֧���˲ɹ������������ʹ�ô˲����޸Ĳɹ���״̬\n\n�˲����ɹ�����Ժ󣬲ɹ����ĵ�ǰ״̬����Ϊ�Ѹ���״̬��ȷ�Ϸ������Ѹ��");
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
         initValid(new InputValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', 1, 10, true, '(0|^-?(0+(\\.[0-9]{1,2}))|^-?[1-9]\\d*(\\.\\d{1,2})?)', '�ۿ�ֻ������ֵ���Ҳ��ܳ���2λС��'))
         appendValid(new MoneyRangeValidator('ctl00_contentHolder_txtPurchaseOrderDiscount', -10000000, 10000000, '�ۿ�ֻ������ֵ�����ܳ���10000000���Ҳ��ܳ���2λС��'));

         var pathurl = $("#ctl00_contentHolder_dlstPurchaseOrders_ctl01_link_purcharprice").attr("href");
         $("#ctl00_contentHolder_dlstPurchaseOrders_ctl01_link_purcharprice").attr("href", "javascript:DialogFrame('" + pathurl + "','�޸Ĳɹ���Ʒ',null,null);")
     }

     $(document).ready(function() { showOrderState(); });


         //�޸Ĳɹ����۸�
     function OpenWindow(PurchaseOrderId, PurchaseTotal, AdjustedDiscount) {
             formtype = "updateprice";
             arrytext = null;

             $("#ctl00_contentHolder_lblPurchaseOrderId").val(PurchaseOrderId);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount").html(Math.floor((eval(PurchaseTotal) - eval(AdjustedDiscount))*100)/100);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount1").html(Math.floor((eval(PurchaseTotal) - eval(AdjustedDiscount)) * 100)/100);
             $("#ctl00_contentHolder_lblPurchaseOrderAmount3").html(eval(PurchaseTotal));
             $("#ctl00_contentHolder_lblPurchaseOrderAmount2").html(eval(AdjustedDiscount));

             DialogShow("�޸Ĳɹ����۸�", 'updateprice', 'EditPurchaseOrder', 'ctl00_contentHolder_btnEditOrder');
         }

         //�༭��ע
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
             DialogShow("�޸ı�ע", 'updateremark', 'RemarkPurchaseOrder', 'ctl00_contentHolder_btnRemark');
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
            if (reason == "��ѡ��رյ�����") {
                alert("��ѡ��رյ�����");
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }
        //�رղɹ���
        function ClosePurchaseOrder(purchaseOrderId) {
            formtype = "close";
            arrytext = null;
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DialogShow("�رղɹ���", 'closepurchar', 'closePurchaseOrder', 'ctl00_contentHolder_btnClosePurchaseOrder');
         }
         function Setordergoods() {
             $("#ctl00_contentHolder_btnOrderGoods").trigger("click");
         }
         function Setproductgoods() {
             $("#ctl00_contentHolder_btnProductGoods").trigger("click");
         }
         //�˻زɹ�������
         function ValidationCloseReason() {
             var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
             if (reason == "��ѡ���˻ص�����") {
                 alert("��ѡ���˻ص�����");
                 return false;
             }
             setArryText("ctl00_contentHolder_ddlCloseReason", reason);
             return true;
         }

         //��ӡ������
         function printGoods() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function() {
                 orderIds += $(this).val() + ",";
             });
             if (orderIds == "") {
                 alert("��ѡҪ��ӡ�ķ�����");
             }
             else {
                 var url = "/Admin/purchaseOrder/BatchPrintSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + orderIds;
                 window.open(url, "������ӡ������", "width=700, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=n o, status=no");
                
             }
         }

         //��ӡ�Ŀ�ݵ�
         function printPosts() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function() {
                 orderIds += $(this).val() + ",";
             }
             );
             if (orderIds == "") {
                 alert("��ѡҪ��ӡ�Ŀ�ݵ�");
             }
             else {
                 var url = "purchaseOrder/BatchPrintPurchaseData.aspx?PurchaseOrderIds=" + orderIds;
                 DialogFrame(url, "������ӡ��ݵ�", null, null);
             }
         }

         //��������
         function sendGoods() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function () {
                 orderIds += $(this).val() + ",";
             });
             if (orderIds == "") {
                 alert("��ѡҪ���������Ĳɹ���");
             } else {
                orderIds=orderIds.substring(0,orderIds.length-1);
                 var url = "purchaseOrder/BatchSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + orderIds;
                 DialogFrame(url, "��������", null, null);
             }
         
         }

         //��֤
         function validatorForm() {
             switch (formtype) {
                 case "remark":
                     arrytext = null;
                     $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                     if ($radioId == null || $radioId == "undefined") {
                         alert('���ȱ�Ǳ�ע');
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
         // ���������
         function downOrder() {
             var orderIds = "";
             $("input:checked[name='CheckBoxGroup']").each(function () {
                 orderIds += $(this).val() + ",";
             }
             );
             if (orderIds == "") {
                 alert("��ѡҪ����������Ĳɹ���");
             }
             else {
                 ShowMessageDialog("����������α�", "downorder", "DownOrder");
             }
         }
         $(function () {
             $(".datalist img[src$='tui.gif']").each(function (item, i) {
                 $parent_link = $(this).parent();
                 $parent_link.attr("href", "javascript:DialogFrame('purchaseOrder/" + $parent_link.attr("href") + "','�˿���ϸ��Ϣ',null,null)");
             });
         });
     </script>
</asp:Content>
