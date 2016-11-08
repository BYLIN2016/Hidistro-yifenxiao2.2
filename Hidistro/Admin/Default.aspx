<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Main" MasterPageFile="~/Admin/Admin.Master" CodeBehind="Default.aspx.cs"%>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="hishop_quick">运营快捷入口：</div>
<div class="hishop_list">
    <div class="sp_color">零售/批发业务：</div>
    <ul>
        <li><a href="javascript:ShowSecondMenuLeft('商 品','product/selectcategory.aspx',null)"><img src="images/1.png" /><br />添加商品</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('CRM管理','tools/sendmessagetemplets.aspx',null)"><img src="images/2.png" /><br />短信营销</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('统计报表','tools/cnzzstatistictotal.aspx',null)"><img src="images/3.png" /><br />网站流量</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('订 单','sales/manageorder.aspx',null)"><img src="images/4.png" /><br />订单列表</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('会 员','member/managemembers.aspx',null)"><img src="images/5.png" /><br />会员管理</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('财务管理','member/accountsummarylist.aspx',null)"><img src="images/6.png" /><br />会员预存款</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('营销推广','promotion/productpromotions.aspx?isWholesale=true',null)"><img src="images/7.png" /><br />批发规则</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('营销推广','promotion/productpromotions.aspx',null)"><img src="images/8.png" /><br />商品促销</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('营销推广','promotion/orderpromotions.aspx',null)"><img src="images/9.png" /><br />订单促销</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('统计报表','sales/salereport.aspx',null)"><img src="images/10.png" /><br />零售统计</a></li>
    </ul>
    <div class="clear"></div>
    <div class="sp_color">代销/批发业务：</div>
     <ul>
        <li><a href="javascript:ShowSecondMenuLeft('采购单','purchaseorder/managepurchaseorder.aspx',null)"><img src="images/20.png" /><br />采购单列表</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('分销管理','distribution/managedistributor.aspx',null)"><img src="images/21.png" /><br />分销商管理</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('财务管理','distribution/distributoraccountquery.aspx',null)"><img src="images/22.jpg" /><br />分销商预存款</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('分销管理','distribution/managesites.aspx',null)"><img src="images/23.jpg" /><br />连锁加盟店管理</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('分销管理','product/maketaobaoproducts.aspx',null)"><img src="images/24.jpg" /><br />淘宝代销铺货</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('分销管理','comment/distributorreceivedmessages.aspx',null)"><img src="images/25.jpg" /><br />分销站内信</a></li>
        <li><a href="javascript:ShowSecondMenuLeft('统计报表','distribution/distributionreport.aspx',null)"><img src="images/26.jpg" /><br />分销统计</a></li>
    </ul>
    <div class="clear"></div>
</div>	 

<div class="hishop_list_content">
    <div class="hishop_list_l">
        <div class="hishop_contenttitle">待处理事务</div>
        <table>
            <tr>
                <td class="td_cont_left">等待发货订单</td><td class="sp_img"><asp:HyperLink ID="ltrWaitSendOrdersNumber" runat="server" ></asp:HyperLink></td>
                <td class="td_cont_left">等待发货采购单</td><td class="sp_img"><asp:HyperLink ID="ltrWaitSendPurchaseOrdersNumber" runat="server"></asp:HyperLink></td>
            </tr>
            <tr>
                <td class="td_cont_left">商品库存报警</td><td class="sp_img"><a href="javascript:ShowSecondMenuLeft('商 品','product/productonsales.aspx?isAlert=True',null);"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="" Id="lblProductCountTotal" /></a></td>
                <td class="td_cont_left">等待付款的采购单</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblPurchaseOrderNumbWait"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">未处理会员提现申请</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblMemberBlancedrawRequest"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">未处理分销提现申请</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblDistributorBlancedrawRequest"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">未回复的商品咨询</td><td class="sp_img"><asp:HyperLink ID="hpkZiXun" runat="server" ></asp:HyperLink></td>
                <td class="td_cont_left">未处理连锁加盟申请</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblDistributorSiteRequest"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">未回复站内信</td><td class="sp_img"><span class="sp_img"><asp:HyperLink ID="hpkMessages" runat="server" ></asp:HyperLink></span></td>
                <td class="td_cont_left">未回复店铺留言</td><td class="sp_img"><span class="sp_img"><asp:HyperLink ID="hpkLiuYan" runat="server" ></asp:HyperLink></span></td>
            </tr>
        </table>

        <div class="clear"></div>

        <div class="hishop_contenttitle">近两日业务量</div>
        <table>
            <tr>
                <td class="td_cont_left">今日成交订单数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTodayFinishOrder"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">昨日成交订单数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblYesterdayFinishOrder"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日订单金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblTodayOrderAmout" /></td>
                <td class="td_cont_left">昨日订单金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblOrderPriceYesterDay"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日成交采购单数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTodayFinishPurchaseOrder"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">昨日成交采购单数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblYesterdayFinishPurchaseOrder"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日采购单金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblPurchaseorderPriceToDay"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">昨日采购单金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblPurchaseorderPriceYesterDay"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日新增会员数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0位" Id="ltrTodayAddMemberNumber" /></td>
                <td class="td_cont_left">昨日新增会员数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblUserNewAddYesterToday"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">今日新增分销商数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="0位" Id="ltrTodayAddDistroNumber" /></td>
                <td class="td_cont_left">昨日新增分销商数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblDistroNewAddYesterToday"></Hi:ClassShowOnDataLitl></td>
            </tr>
        </table>
        
        <div class="hishop_contenttitle">店铺信息</div>
        <table>
            <tr>
                <td class="td_cont_left">会员总数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblTotalMembers"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">商品总数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0条" ID="lblTotalProducts"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">分销商总数</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="0位" ID="lblTotalDistributors"></Hi:ClassShowOnDataLitl></td>
                <td class="td_cont_left">最近30天的订单总金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblOrderPriceMonth"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">会员预付款总额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblMembersBalanceTotal" /></td>
                <td class="td_cont_left">最近30天的采购单总金额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server" DefaultText="￥0.00" ID="lblPurchaseorderPriceMonth"></Hi:ClassShowOnDataLitl></td>
            </tr>
            <tr>
                <td class="td_cont_left">分销商预付款总额</td><td class="sp_img"><Hi:ClassShowOnDataLitl runat="server"  DefaultText="￥0.00" Id="lblDistrosBalanceTotal" /></td>
                <td class="td_cont_left">当前版本</td><td class="sp_img">V2.2</td>
            </tr>
        </table>

        <div class="clear"></div>
    </div>
    <div class="hishop_list_r">
             <div class="hishop_contenttitle">易分销动态</div>
             
            
          <div class="hishop_list_tab">
              <div style="height:370px;">
              <Hi:HiControls LinkURL="http://www.hishop.com.cn/efx01/efxv2.html" runat="server" Height="370" ID="HiControlsId"/>              
              </div>
              <iframe src="http://www.hishop.com.cn/efx01/efxv21.html"  frameborder="0" width="100%"></iframe>
          </div>
    </div>
</div>
<script type="text/javascript">
    function ShowSecondMenuLeft(firstnode, secondurl,threeurl) {
        window.parent.ShowMenuLeft(firstnode, secondurl, threeurl);
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server"></asp:Content>

