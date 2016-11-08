<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.Tags" Assembly="Hidistro.UI.SaleSystem.Tags" %>
<%@ Import Namespace="Hidistro.Core" %>
<div class="core_formleft">
    <div id="leftbox">
        <ul class="NavClass">
            <li>
                <h2 id="MyRec">
                    我的交易记录</h2>
                <ul>                    
                    <li>
                        <Hi:SiteUrl ID="SiteUrl4" UrlName="user_UserOrders" runat="server">订单管理</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl5" UrlName="user_UserRefundApply" runat="server">退款申请单</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl6" UrlName="user_UserReturnsApply" runat="server">退货申请单</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl7" UrlName="user_UserReplaceApply" runat="server">换货申请单</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl1" UrlName="user_UserPoints" runat="server">我的积分</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl2" UrlName="user_MyCoupons" runat="server">我的优惠券</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl3" UrlName="user_MyChangeCoupons" runat="server">积分兑换优惠券</Hi:SiteUrl></li>
                </ul>
            </li>
            <li>
                <h2 id="ProFavRev">
                    商品收藏与评论</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl8" UrlName="user_Favorites" runat="server">收藏夹</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl9" UrlName="user_UserConsultations" runat="server">咨询/回复</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl10" UrlName="user_UserProductReviews" runat="server">我参与的评论</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl11" UrlName="user_UserBatchBuy" runat="server">商品批量购买</Hi:SiteUrl></li>
                </ul>
            </li>
            <li>
                <h2 id="Acu">
                    预付款账户</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl12" UrlName="user_MyAccountSummary" runat="server">预付款账户</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl13" UrlName="user_RechargeRequest" runat="server">预付款充值</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl14" UrlName="user_ReferralMembers" runat="server">我推荐的会员</Hi:SiteUrl></li>
                </ul>
            </li>
            <li>
                <h2 id="Opt">
                    个人设置</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl15" UrlName="user_UserProfile" runat="server">个人信息</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl16" UrlName="user_UpdatePassword" runat="server">修改密码</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl17" UrlName="user_UserShippingAddresses" runat="server">我的收货地址</Hi:SiteUrl></li>
                </ul>
            </li>
            <li>
                <h2 id="ProFavRev">
                    站内消息</h2>
                <ul>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl18" UrlName="user_UserReceivedMessages" runat="server">收件箱</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl19" UrlName="user_UserSendedMessages" runat="server">发件箱</Hi:SiteUrl></li>
                    <li>
                        <Hi:SiteUrl ID="SiteUrl20" UrlName="user_UserSendMessage" runat="server">给管理员发消息</Hi:SiteUrl></li>
                </ul>
            </li>
        </ul>
        <h2 id="Acu" class="login_out_member">
            <a href="url:logout">安全退出</a></h2>
    </div>
</div>
