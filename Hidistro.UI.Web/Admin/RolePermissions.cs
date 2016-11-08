namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class RolePermissions : AdminPage
    {
        protected Button btnSet1;
        protected LinkButton btnSetTop;
        protected CheckBox cbAccountSummary;
        protected CheckBox cbAddProductLine;
        protected CheckBox cbAfficheList;
        protected CheckBox cbAll;
        protected CheckBox cbArticleCategories;
        protected CheckBox cbArticleList;
        protected CheckBox cbBalanceDetailsStatistics;
        protected CheckBox cbBalanceDrawRequest;
        protected CheckBox cbBalanceDrawRequestStatistics;
        protected CheckBox cbBrandCategories;
        protected CheckBox cbBundPromotion;
        protected CheckBox cbClientActivy;
        protected CheckBox cbClientGroup;
        protected CheckBox cbClientNew;
        protected CheckBox cbClientSleep;
        protected CheckBox cbCountDown;
        protected CheckBox cbCoupons;
        protected CheckBox cbCRMmanager;
        protected CheckBox cbDeleteProductLine;
        protected CheckBox cbDistribution;
        protected CheckBox cbDistributionProductSaleRanking;
        protected CheckBox cbDistributionReport;
        protected CheckBox cbDistributorAcceptMsg;
        protected CheckBox cbDistributorAccount;
        protected CheckBox cbDistributorAchievementsRanking;
        protected CheckBox cbDistributorBalanceDrawRequest;
        protected CheckBox cbDistributorGrades;
        protected CheckBox cbDistributorGradesAdd;
        protected CheckBox cbDistributorGradesDelete;
        protected CheckBox cbDistributorGradesEdit;
        protected CheckBox cbDistributorGradesView;
        protected CheckBox cbDistributorNewMsg;
        protected CheckBox cbDistributorReCharge;
        protected CheckBox cbDistributors;
        protected CheckBox cbDistributorsDelete;
        protected CheckBox cbDistributorsEdit;
        protected CheckBox cbDistributorSendedMsg;
        protected CheckBox cbDistributorSiteRequests;
        protected CheckBox cbDistributorsRequestInstruction;
        protected CheckBox cbDistributorsRequests;
        protected CheckBox cbDistributorsView;
        protected CheckBox cbEditProductLine;
        protected CheckBox cbEmailSettings;
        protected CheckBox cbExpressComputerpes;
        protected CheckBox cbExpressPrint;
        protected CheckBox cbExpressTemplates;
        protected CheckBox cbFinancial;
        protected CheckBox cbFriendlyLinks;
        protected CheckBox cbGifts;
        protected CheckBox cbGroupBuy;
        protected CheckBox cbHelpCategories;
        protected CheckBox cbHelpList;
        protected CheckBox cbInStock;
        protected CheckBox cbMakeProductsPack;
        protected CheckBox cbManageCategories;
        protected CheckBox cbManageCategoriesAdd;
        protected CheckBox cbManageCategoriesDelete;
        protected CheckBox cbManageCategoriesEdit;
        protected CheckBox cbManageCategoriesView;
        protected CheckBox cbManageDistributorSites;
        protected CheckBox cbManageHotKeywords;
        protected CheckBox cbManageLeaveComments;
        protected CheckBox cbManageMembers;
        protected CheckBox cbManageMembersDelete;
        protected CheckBox cbManageMembersEdit;
        protected CheckBox cbManageMembersView;
        protected CheckBox cbManageOrder;
        protected CheckBox cbManageOrderConfirm;
        protected CheckBox cbManageOrderDelete;
        protected CheckBox cbManageOrderEdit;
        protected CheckBox cbManageOrderRemark;
        protected CheckBox cbManageOrderSendedGoods;
        protected CheckBox cbManageOrderView;
        protected CheckBox cbManageProducts;
        protected CheckBox cbManageProductsAdd;
        protected CheckBox cbManageProductsDelete;
        protected CheckBox cbManageProductsDown;
        protected CheckBox cbManageProductsEdit;
        protected CheckBox cbManageProductsUp;
        protected CheckBox cbManageProductsView;
        protected CheckBox cbManagePurchaseOrder;
        protected CheckBox cbManagePurchaseOrderDelete;
        protected CheckBox cbManagePurchaseOrderEdit;
        protected CheckBox cbManagePurchaseOrderView;
        protected CheckBox cbManageThemes;
        protected CheckBox cbManageUsers;
        protected CheckBox cbMarketing;
        protected CheckBox cbMemberArealDistributionStatistics;
        protected CheckBox cbMemberMarket;
        protected CheckBox cbMemberRanking;
        protected CheckBox cbMemberRanks;
        protected CheckBox cbMemberRanksAdd;
        protected CheckBox cbMemberRanksDelete;
        protected CheckBox cbMemberRanksEdit;
        protected CheckBox cbMemberRanksView;
        protected CheckBox cbMessageTemplets;
        protected CheckBox cbOpenIdServices;
        protected CheckBox cbOpenIdSettings;
        protected CheckBox cbOrderPromotion;
        protected CheckBox cbOrderRefundApply;
        protected CheckBox cbOrderReplaceApply;
        protected CheckBox cbOrderReturnsApply;
        protected CheckBox cbPackProduct;
        protected CheckBox cbPageManger;
        protected CheckBox cbPaymentModes;
        protected CheckBox cbPictureMange;
        protected CheckBox cbProductBatchExport;
        protected CheckBox cbProductBatchUpload;
        protected CheckBox cbProductCatalog;
        protected CheckBox cbProductConsultationsManage;
        protected CheckBox cbProductLines;
        protected CheckBox cbProductLinesView;
        protected CheckBox cbProductPromotion;
        protected CheckBox cbProductReviewsManage;
        protected CheckBox cbProductSaleRanking;
        protected CheckBox cbProductSaleStatistics;
        protected CheckBox cbProductTypes;
        protected CheckBox cbProductTypesAdd;
        protected CheckBox cbProductTypesDelete;
        protected CheckBox cbProductTypesEdit;
        protected CheckBox cbProductTypesView;
        protected CheckBox cbProductUnclassified;
        protected CheckBox cbPurchaseOrder;
        protected CheckBox cbPurchaseOrderRefundApply;
        protected CheckBox cbPurchaseOrderRemark;
        protected CheckBox cbPurchaseOrderReplaceApply;
        protected CheckBox cbPurchaseOrderReturnsApply;
        protected CheckBox cbPurchaseOrderSendGoods;
        protected CheckBox cbPurchaseOrderStatistics;
        protected CheckBox cbReceivedMessages;
        protected CheckBox cbReCharge;
        protected CheckBox cbRequests;
        protected CheckBox cbSaleList;
        protected CheckBox cbSales;
        protected CheckBox cbSaleTargetAnalyse;
        protected CheckBox cbSaleTotalStatistics;
        protected CheckBox cbSendedMessages;
        protected CheckBox cbSendMessage;
        protected CheckBox cbShipper;
        protected CheckBox cbShippingModes;
        protected CheckBox cbShippingTemplets;
        protected CheckBox cbShop;
        protected CheckBox cbSiteContent;
        protected CheckBox cbSMSSettings;
        protected CheckBox cbSubjectProducts;
        protected CheckBox cbSummary;
        protected CheckBox cbTotalReport;
        protected CheckBox cbUpPackProduct;
        protected CheckBox cbUserIncreaseStatistics;
        protected CheckBox cbUserOrderStatistics;
        protected CheckBox cbVotes;
        protected CheckBox ckTaobaoNote;
        protected Literal lblRoleName;
        private string RequestRoleId;

        private void btnSet_Click(object sender, EventArgs e)
        {
            Guid roleId = new Guid(this.RequestRoleId);
            this.PermissionsSet(roleId);
            this.Page.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/store/RolePermissions.aspx?roleId={0}&Status=1", roleId)));
        }

        private void LoadData(Guid roleId)
        {
            IList<int> privilegeByRoles = RoleHelper.GetPrivilegeByRoles(roleId);
            this.cbSummary.Checked = privilegeByRoles.Contains(0x3e8);
            this.cbSiteContent.Checked = privilegeByRoles.Contains(0x3e9);
            this.cbVotes.Checked = privilegeByRoles.Contains(0x7d9);
            this.cbFriendlyLinks.Checked = privilegeByRoles.Contains(0x7d7);
            this.cbManageThemes.Checked = privilegeByRoles.Contains(0x7d1);
            this.cbManageHotKeywords.Checked = privilegeByRoles.Contains(0x7d8);
            this.cbAfficheList.Checked = privilegeByRoles.Contains(0x7d2);
            this.cbHelpCategories.Checked = privilegeByRoles.Contains(0x7d3);
            this.cbHelpList.Checked = privilegeByRoles.Contains(0x7d4);
            this.cbArticleCategories.Checked = privilegeByRoles.Contains(0x7d5);
            this.cbArticleList.Checked = privilegeByRoles.Contains(0x7d6);
            this.cbEmailSettings.Checked = privilegeByRoles.Contains(0x3ea);
            this.cbSMSSettings.Checked = privilegeByRoles.Contains(0x3eb);
            this.cbMessageTemplets.Checked = privilegeByRoles.Contains(0x3f0);
            this.cbShippingTemplets.Checked = privilegeByRoles.Contains(0x3ee);
            this.cbExpressComputerpes.Checked = privilegeByRoles.Contains(0x3ef);
            this.cbPictureMange.Checked = privilegeByRoles.Contains(0x3f1);
            this.cbDistributorGradesView.Checked = privilegeByRoles.Contains(0x1771);
            this.cbDistributorGradesAdd.Checked = privilegeByRoles.Contains(0x1772);
            this.cbDistributorGradesEdit.Checked = privilegeByRoles.Contains(0x1773);
            this.cbDistributorGradesDelete.Checked = privilegeByRoles.Contains(0x1774);
            this.cbDistributorsView.Checked = privilegeByRoles.Contains(0x1775);
            this.cbDistributorsEdit.Checked = privilegeByRoles.Contains(0x1776);
            this.cbDistributorsDelete.Checked = privilegeByRoles.Contains(0x1777);
            this.cbDistributorsRequests.Checked = privilegeByRoles.Contains(0x1778);
            this.cbDistributorsRequestInstruction.Checked = privilegeByRoles.Contains(0x1779);
            this.cbManagePurchaseOrderView.Checked = privilegeByRoles.Contains(0x2af9);
            this.cbManagePurchaseOrderEdit.Checked = privilegeByRoles.Contains(0x2afa);
            this.cbManagePurchaseOrderDelete.Checked = privilegeByRoles.Contains(0x2afb);
            this.cbPurchaseOrderSendGoods.Checked = privilegeByRoles.Contains(0x2afc);
            this.cbPurchaseOrderRemark.Checked = privilegeByRoles.Contains(0x2afe);
            this.cbPurchaseOrderRefundApply.Checked = privilegeByRoles.Contains(0x2aff);
            this.cbPurchaseOrderReturnsApply.Checked = privilegeByRoles.Contains(0x2b01);
            this.cbPurchaseOrderReplaceApply.Checked = privilegeByRoles.Contains(0x2b00);
            this.cbDistributorAccount.Checked = privilegeByRoles.Contains(0x232c);
            this.cbDistributorReCharge.Checked = privilegeByRoles.Contains(0x232d);
            this.cbDistributorBalanceDrawRequest.Checked = privilegeByRoles.Contains(0x232e);
            this.cbDistributionReport.Checked = privilegeByRoles.Contains(0x271a);
            this.cbPurchaseOrderStatistics.Checked = privilegeByRoles.Contains(0x271b);
            this.cbDistributionProductSaleRanking.Checked = privilegeByRoles.Contains(0x18710);
            this.cbDistributorAchievementsRanking.Checked = privilegeByRoles.Contains(0x18711);
            this.cbManageDistributorSites.Checked = privilegeByRoles.Contains(0x177a);
            this.cbDistributorSiteRequests.Checked = privilegeByRoles.Contains(0x177b);
            this.cbProductLinesView.Checked = privilegeByRoles.Contains(0xbc5);
            this.cbAddProductLine.Checked = privilegeByRoles.Contains(0xbc6);
            this.cbEditProductLine.Checked = privilegeByRoles.Contains(0xbc7);
            this.cbDeleteProductLine.Checked = privilegeByRoles.Contains(0xbc8);
            this.cbProductTypesView.Checked = privilegeByRoles.Contains(0xbc9);
            this.cbProductTypesAdd.Checked = privilegeByRoles.Contains(0xbca);
            this.cbProductTypesEdit.Checked = privilegeByRoles.Contains(0xbcb);
            this.cbProductTypesDelete.Checked = privilegeByRoles.Contains(0xbcc);
            this.cbManageCategoriesView.Checked = privilegeByRoles.Contains(0xbcd);
            this.cbManageCategoriesAdd.Checked = privilegeByRoles.Contains(0xbce);
            this.cbManageCategoriesEdit.Checked = privilegeByRoles.Contains(0xbcf);
            this.cbManageCategoriesDelete.Checked = privilegeByRoles.Contains(0xbd0);
            this.cbBrandCategories.Checked = privilegeByRoles.Contains(0xbd1);
            this.cbManageProductsView.Checked = privilegeByRoles.Contains(0xbb9);
            this.cbManageProductsAdd.Checked = privilegeByRoles.Contains(0xbba);
            this.cbManageProductsEdit.Checked = privilegeByRoles.Contains(0xbbb);
            this.cbManageProductsDelete.Checked = privilegeByRoles.Contains(0xbbc);
            this.cbInStock.Checked = privilegeByRoles.Contains(0xbbd);
            this.cbManageProductsUp.Checked = privilegeByRoles.Contains(0xbbe);
            this.cbManageProductsDown.Checked = privilegeByRoles.Contains(0xbbf);
            this.cbPackProduct.Checked = privilegeByRoles.Contains(0xbc0);
            this.cbUpPackProduct.Checked = privilegeByRoles.Contains(0xbc1);
            this.cbProductUnclassified.Checked = privilegeByRoles.Contains(0xbc2);
            this.cbProductBatchUpload.Checked = privilegeByRoles.Contains(0xbc4);
            this.cbProductBatchExport.Checked = privilegeByRoles.Contains(0xbd2);
            this.cbMakeProductsPack.Checked = privilegeByRoles.Contains(0x177c);
            this.ckTaobaoNote.Checked = privilegeByRoles.Contains(0x177d);
            this.cbDistributorSendedMsg.Checked = privilegeByRoles.Contains(0x177e);
            this.cbDistributorAcceptMsg.Checked = privilegeByRoles.Contains(0x177f);
            this.cbDistributorNewMsg.Checked = privilegeByRoles.Contains(0x1780);
            this.cbSubjectProducts.Checked = privilegeByRoles.Contains(0xbc3);
            this.cbClientGroup.Checked = privilegeByRoles.Contains(0x1b5f);
            this.cbClientActivy.Checked = privilegeByRoles.Contains(0x1b61);
            this.cbClientNew.Checked = privilegeByRoles.Contains(0x1b60);
            this.cbClientSleep.Checked = privilegeByRoles.Contains(0x1b62);
            this.cbMemberRanksView.Checked = privilegeByRoles.Contains(0x138c);
            this.cbMemberRanksAdd.Checked = privilegeByRoles.Contains(0x138d);
            this.cbMemberRanksEdit.Checked = privilegeByRoles.Contains(0x138e);
            this.cbMemberRanksDelete.Checked = privilegeByRoles.Contains(0x138f);
            this.cbManageMembersView.Checked = privilegeByRoles.Contains(0x1389);
            this.cbManageMembersEdit.Checked = privilegeByRoles.Contains(0x138a);
            this.cbManageMembersDelete.Checked = privilegeByRoles.Contains(0x138b);
            this.cbBalanceDrawRequest.Checked = privilegeByRoles.Contains(0x232b);
            this.cbAccountSummary.Checked = privilegeByRoles.Contains(0x2329);
            this.cbReCharge.Checked = privilegeByRoles.Contains(0x232a);
            this.cbBalanceDetailsStatistics.Checked = privilegeByRoles.Contains(0x1392);
            this.cbBalanceDrawRequestStatistics.Checked = privilegeByRoles.Contains(0x1393);
            this.cbMemberArealDistributionStatistics.Checked = privilegeByRoles.Contains(0x2718);
            this.cbUserIncreaseStatistics.Checked = privilegeByRoles.Contains(0x2719);
            this.cbMemberRanking.Checked = privilegeByRoles.Contains(0x2717);
            this.cbOpenIdServices.Checked = privilegeByRoles.Contains(0x1390);
            this.cbOpenIdSettings.Checked = privilegeByRoles.Contains(0x1391);
            this.cbManageOrderView.Checked = privilegeByRoles.Contains(0xfa1);
            this.cbManageOrderDelete.Checked = privilegeByRoles.Contains(0xfa2);
            this.cbManageOrderEdit.Checked = privilegeByRoles.Contains(0xfa3);
            this.cbManageOrderConfirm.Checked = privilegeByRoles.Contains(0xfa4);
            this.cbManageOrderSendedGoods.Checked = privilegeByRoles.Contains(0xfa5);
            this.cbExpressPrint.Checked = privilegeByRoles.Contains(0xfa6);
            this.cbManageOrderRemark.Checked = privilegeByRoles.Contains(0xfa8);
            this.cbExpressTemplates.Checked = privilegeByRoles.Contains(0xfa9);
            this.cbShipper.Checked = privilegeByRoles.Contains(0xfaa);
            this.cbPaymentModes.Checked = privilegeByRoles.Contains(0x3ec);
            this.cbShippingModes.Checked = privilegeByRoles.Contains(0x3ed);
            this.cbOrderRefundApply.Checked = privilegeByRoles.Contains(0xfac);
            this.cbOrderReturnsApply.Checked = privilegeByRoles.Contains(0xfae);
            this.cbOrderReplaceApply.Checked = privilegeByRoles.Contains(0xfad);
            this.cbSaleTotalStatistics.Checked = privilegeByRoles.Contains(0x2711);
            this.cbUserOrderStatistics.Checked = privilegeByRoles.Contains(0x2712);
            this.cbSaleList.Checked = privilegeByRoles.Contains(0x2713);
            this.cbSaleTargetAnalyse.Checked = privilegeByRoles.Contains(0x2714);
            this.cbProductSaleRanking.Checked = privilegeByRoles.Contains(0x2715);
            this.cbProductSaleStatistics.Checked = privilegeByRoles.Contains(0x2716);
            this.cbGifts.Checked = privilegeByRoles.Contains(0x1f41);
            this.cbGroupBuy.Checked = privilegeByRoles.Contains(0x1f45);
            this.cbCountDown.Checked = privilegeByRoles.Contains(0x1f46);
            this.cbCoupons.Checked = privilegeByRoles.Contains(0x1f47);
            this.cbProductPromotion.Checked = privilegeByRoles.Contains(0x1f42);
            this.cbOrderPromotion.Checked = privilegeByRoles.Contains(0x1f43);
            this.cbBundPromotion.Checked = privilegeByRoles.Contains(0x1f44);
            this.cbProductConsultationsManage.Checked = privilegeByRoles.Contains(0x1b59);
            this.cbProductReviewsManage.Checked = privilegeByRoles.Contains(0x1b5a);
            this.cbReceivedMessages.Checked = privilegeByRoles.Contains(0x1b5b);
            this.cbSendedMessages.Checked = privilegeByRoles.Contains(0x1b5c);
            this.cbSendMessage.Checked = privilegeByRoles.Contains(0x1b5d);
            this.cbManageLeaveComments.Checked = privilegeByRoles.Contains(0x1b5e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["roleId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.RequestRoleId = this.Page.Request.QueryString["roleId"];
                this.btnSet1.Click += new EventHandler(this.btnSet_Click);
                this.btnSetTop.Click += new EventHandler(this.btnSet_Click);
                if (!this.Page.IsPostBack)
                {
                    Guid roleID = new Guid(this.RequestRoleId);
                    if (Regex.IsMatch(this.RequestRoleId, "[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}", RegexOptions.IgnoreCase))
                    {
                        RoleInfo role = RoleHelper.GetRole(roleID);
                        this.lblRoleName.Text = role.Name;
                    }
                    if (this.Page.Request.QueryString["Status"] == "1")
                    {
                        this.ShowMsg("设置部门权限成功", true);
                    }
                    this.LoadData(roleID);
                }
            }
        }

        private void PermissionsSet(Guid roleId)
        {
            string str = string.Empty;
            if (this.cbSummary.Checked)
            {
                str = str + 0x3e8 + ",";
            }
            if (this.cbSiteContent.Checked)
            {
                str = str + 0x3e9 + ",";
            }
            if (this.cbVotes.Checked)
            {
                str = str + 0x7d9 + ",";
            }
            if (this.cbFriendlyLinks.Checked)
            {
                str = str + 0x7d7 + ",";
            }
            if (this.cbManageThemes.Checked)
            {
                str = str + 0x7d1 + ",";
            }
            if (this.cbManageHotKeywords.Checked)
            {
                str = str + 0x7d8 + ",";
            }
            if (this.cbAfficheList.Checked)
            {
                str = str + 0x7d2 + ",";
            }
            if (this.cbHelpCategories.Checked)
            {
                str = str + 0x7d3 + ",";
            }
            if (this.cbHelpList.Checked)
            {
                str = str + 0x7d4 + ",";
            }
            if (this.cbArticleCategories.Checked)
            {
                str = str + 0x7d5 + ",";
            }
            if (this.cbArticleList.Checked)
            {
                str = str + 0x7d6 + ",";
            }
            if (this.cbEmailSettings.Checked)
            {
                str = str + 0x3ea + ",";
            }
            if (this.cbSMSSettings.Checked)
            {
                str = str + 0x3eb + ",";
            }
            if (this.cbMessageTemplets.Checked)
            {
                str = str + 0x3f0 + ",";
            }
            if (this.cbShippingTemplets.Checked)
            {
                str = str + 0x3ee + ",";
            }
            if (this.cbExpressComputerpes.Checked)
            {
                str = str + 0x3ef + ",";
            }
            if (this.cbPictureMange.Checked)
            {
                str = str + 0x3f1 + ",";
            }
            if (this.cbDistributorGradesView.Checked)
            {
                str = str + 0x1771 + ",";
            }
            if (this.cbDistributorGradesAdd.Checked)
            {
                str = str + 0x1772 + ",";
            }
            if (this.cbDistributorGradesEdit.Checked)
            {
                str = str + 0x1773 + ",";
            }
            if (this.cbDistributorGradesDelete.Checked)
            {
                str = str + 0x1774 + ",";
            }
            if (this.cbDistributorsView.Checked)
            {
                str = str + 0x1775 + ",";
            }
            if (this.cbDistributorsEdit.Checked)
            {
                str = str + 0x1776 + ",";
            }
            if (this.cbDistributorsDelete.Checked)
            {
                str = str + 0x1777 + ",";
            }
            if (this.cbDistributorsRequests.Checked)
            {
                str = str + 0x1778 + ",";
            }
            if (this.cbDistributorsRequestInstruction.Checked)
            {
                str = str + 0x1779 + ",";
            }
            if (this.cbDistributorAccount.Checked)
            {
                str = str + 0x232c + ",";
            }
            if (this.cbDistributorReCharge.Checked)
            {
                str = str + 0x232d + ",";
            }
            if (this.cbDistributorBalanceDrawRequest.Checked)
            {
                str = str + 0x232e + ",";
            }
            if (this.cbDistributionReport.Checked)
            {
                str = str + 0x271a + ",";
            }
            if (this.cbPurchaseOrderStatistics.Checked)
            {
                str = str + 0x271b + ",";
            }
            if (this.cbDistributionProductSaleRanking.Checked)
            {
                str = str + 0x18710 + ",";
            }
            if (this.cbDistributorAchievementsRanking.Checked)
            {
                str = str + 0x18711 + ",";
            }
            if (this.cbManageDistributorSites.Checked)
            {
                str = str + 0x177a + ",";
            }
            if (this.cbDistributorSiteRequests.Checked)
            {
                str = str + 0x177b + ",";
            }
            if (this.cbProductLinesView.Checked)
            {
                str = str + 0xbc5 + ",";
            }
            if (this.cbAddProductLine.Checked)
            {
                str = str + 0xbc6 + ",";
            }
            if (this.cbEditProductLine.Checked)
            {
                str = str + 0xbc7 + ",";
            }
            if (this.cbDeleteProductLine.Checked)
            {
                str = str + 0xbc8 + ",";
            }
            if (this.cbProductTypesView.Checked)
            {
                str = str + 0xbc9 + ",";
            }
            if (this.cbProductTypesAdd.Checked)
            {
                str = str + 0xbca + ",";
            }
            if (this.cbProductTypesEdit.Checked)
            {
                str = str + 0xbcb + ",";
            }
            if (this.cbProductTypesDelete.Checked)
            {
                str = str + 0xbcc + ",";
            }
            if (this.cbManageCategoriesView.Checked)
            {
                str = str + 0xbcd + ",";
            }
            if (this.cbManageCategoriesAdd.Checked)
            {
                str = str + 0xbce + ",";
            }
            if (this.cbManageCategoriesEdit.Checked)
            {
                str = str + 0xbcf + ",";
            }
            if (this.cbManageCategoriesDelete.Checked)
            {
                str = str + 0xbd0 + ",";
            }
            if (this.cbBrandCategories.Checked)
            {
                str = str + 0xbd1 + ",";
            }
            if (this.cbManageProductsView.Checked)
            {
                str = str + 0xbb9 + ",";
            }
            if (this.cbManageProductsAdd.Checked)
            {
                str = str + 0xbba + ",";
            }
            if (this.cbManageProductsEdit.Checked)
            {
                str = str + 0xbbb + ",";
            }
            if (this.cbManageProductsDelete.Checked)
            {
                str = str + 0xbbc + ",";
            }
            if (this.cbInStock.Checked)
            {
                str = str + 0xbbd + ",";
            }
            if (this.cbManageProductsUp.Checked)
            {
                str = str + 0xbbe + ",";
            }
            if (this.cbManageProductsDown.Checked)
            {
                str = str + 0xbbf + ",";
            }
            if (this.cbPackProduct.Checked)
            {
                str = str + 0xbc0 + ",";
            }
            if (this.cbUpPackProduct.Checked)
            {
                str = str + 0xbc1 + ",";
            }
            if (this.cbProductUnclassified.Checked)
            {
                str = str + 0xbc2 + ",";
            }
            if (this.cbProductBatchUpload.Checked)
            {
                str = str + 0xbc4 + ",";
            }
            if (this.cbProductBatchExport.Checked)
            {
                str = str + 0xbd2 + ",";
            }
            if (this.cbSubjectProducts.Checked)
            {
                str = str + 0xbc3 + ",";
            }
            if (this.cbMakeProductsPack.Checked)
            {
                str = str + 0x177c + ",";
            }
            if (this.ckTaobaoNote.Checked)
            {
                str = str + 0x177d + ",";
            }
            if (this.cbDistributorSendedMsg.Checked)
            {
                str = str + 0x177e + ",";
            }
            if (this.cbDistributorAcceptMsg.Checked)
            {
                str = str + 0x177f + ",";
            }
            if (this.cbDistributorNewMsg.Checked)
            {
                str = str + 0x1780 + ",";
            }
            if (this.cbClientGroup.Checked)
            {
                str = str + 0x1b5f + ",";
            }
            if (this.cbClientNew.Checked)
            {
                str = str + 0x1b60 + ",";
            }
            if (this.cbClientSleep.Checked)
            {
                str = str + 0x1b62 + ",";
            }
            if (this.cbClientActivy.Checked)
            {
                str = str + 0x1b61 + ",";
            }
            if (this.cbMemberRanksView.Checked)
            {
                str = str + 0x138c + ",";
            }
            if (this.cbMemberRanksAdd.Checked)
            {
                str = str + 0x138d + ",";
            }
            if (this.cbMemberRanksEdit.Checked)
            {
                str = str + 0x138e + ",";
            }
            if (this.cbMemberRanksDelete.Checked)
            {
                str = str + 0x138f + ",";
            }
            if (this.cbManageMembersView.Checked)
            {
                str = str + 0x1389 + ",";
            }
            if (this.cbManageMembersEdit.Checked)
            {
                str = str + 0x138a + ",";
            }
            if (this.cbManageMembersDelete.Checked)
            {
                str = str + 0x138b + ",";
            }
            if (this.cbBalanceDrawRequest.Checked)
            {
                str = str + 0x232b + ",";
            }
            if (this.cbAccountSummary.Checked)
            {
                str = str + 0x2329 + ",";
            }
            if (this.cbReCharge.Checked)
            {
                str = str + 0x232a + ",";
            }
            if (this.cbBalanceDetailsStatistics.Checked)
            {
                str = str + 0x1392 + ",";
            }
            if (this.cbBalanceDrawRequestStatistics.Checked)
            {
                str = str + 0x1393 + ",";
            }
            if (this.cbMemberArealDistributionStatistics.Checked)
            {
                str = str + 0x2718 + ",";
            }
            if (this.cbUserIncreaseStatistics.Checked)
            {
                str = str + 0x2719 + ",";
            }
            if (this.cbMemberRanking.Checked)
            {
                str = str + 0x2717 + ",";
            }
            if (this.cbOpenIdServices.Checked)
            {
                str = str + 0x1390 + ",";
            }
            if (this.cbOpenIdSettings.Checked)
            {
                str = str + 0x1391 + ",";
            }
            if (this.cbManageOrderView.Checked)
            {
                str = str + 0xfa1 + ",";
            }
            if (this.cbManageOrderDelete.Checked)
            {
                str = str + 0xfa2 + ",";
            }
            if (this.cbManageOrderEdit.Checked)
            {
                str = str + 0xfa3 + ",";
            }
            if (this.cbManageOrderConfirm.Checked)
            {
                str = str + 0xfa4 + ",";
            }
            if (this.cbManageOrderSendedGoods.Checked)
            {
                str = str + 0xfa5 + ",";
            }
            if (this.cbExpressPrint.Checked)
            {
                str = str + 0xfa6 + ",";
            }
            if (this.cbExpressTemplates.Checked)
            {
                str = str + 0xfa9 + ",";
            }
            if (this.cbShipper.Checked)
            {
                str = str + 0xfaa + ",";
            }
            if (this.cbPaymentModes.Checked)
            {
                str = str + 0x3ec + ",";
            }
            if (this.cbShippingModes.Checked)
            {
                str = str + 0x3ed + ",";
            }
            if (this.cbSaleTotalStatistics.Checked)
            {
                str = str + 0x2711 + ",";
            }
            if (this.cbUserOrderStatistics.Checked)
            {
                str = str + 0x2712 + ",";
            }
            if (this.cbSaleList.Checked)
            {
                str = str + 0x2713 + ",";
            }
            if (this.cbSaleTargetAnalyse.Checked)
            {
                str = str + 0x2714 + ",";
            }
            if (this.cbProductSaleRanking.Checked)
            {
                str = str + 0x2715 + ",";
            }
            if (this.cbProductSaleStatistics.Checked)
            {
                str = str + 0x2716 + ",";
            }
            if (this.cbOrderRefundApply.Checked)
            {
                str = str + 0xfac + ",";
            }
            if (this.cbOrderReplaceApply.Checked)
            {
                str = str + 0xfad + ",";
            }
            if (this.cbOrderReturnsApply.Checked)
            {
                str = str + 0xfae + ",";
            }
            if (this.cbManagePurchaseOrderView.Checked)
            {
                str = str + 0x2af9 + ",";
            }
            if (this.cbManagePurchaseOrderEdit.Checked)
            {
                str = str + 0x2afa + ",";
            }
            if (this.cbManagePurchaseOrderDelete.Checked)
            {
                str = str + 0x2afb + ",";
            }
            if (this.cbPurchaseOrderSendGoods.Checked)
            {
                str = str + 0x2afc + ",";
            }
            if (this.cbPurchaseOrderRemark.Checked)
            {
                str = str + 0x2afe + ",";
            }
            if (this.cbPurchaseOrderRefundApply.Checked)
            {
                str = str + 0x2aff + ",";
            }
            if (this.cbPurchaseOrderReturnsApply.Checked)
            {
                str = str + 0x2b01 + ",";
            }
            if (this.cbPurchaseOrderReplaceApply.Checked)
            {
                str = str + 0x2b00 + ",";
            }
            if (this.cbGifts.Checked)
            {
                str = str + 0x1f41 + ",";
            }
            if (this.cbGroupBuy.Checked)
            {
                str = str + 0x1f45 + ",";
            }
            if (this.cbCountDown.Checked)
            {
                str = str + 0x1f46 + ",";
            }
            if (this.cbCoupons.Checked)
            {
                str = str + 0x1f47 + ",";
            }
            if (this.cbProductPromotion.Checked)
            {
                str = str + 0x1f42 + ",";
            }
            if (this.cbOrderPromotion.Checked)
            {
                str = str + 0x1f43 + ",";
            }
            if (this.cbBundPromotion.Checked)
            {
                str = str + 0x1f44 + ",";
            }
            if (this.cbProductConsultationsManage.Checked)
            {
                str = str + 0x1b59 + ",";
            }
            if (this.cbProductReviewsManage.Checked)
            {
                str = str + 0x1b5a + ",";
            }
            if (this.cbReceivedMessages.Checked)
            {
                str = str + 0x1b5b + ",";
            }
            if (this.cbSendedMessages.Checked)
            {
                str = str + 0x1b5c + ",";
            }
            if (this.cbSendMessage.Checked)
            {
                str = str + 0x1b5d + ",";
            }
            if (this.cbManageLeaveComments.Checked)
            {
                str = str + 0x1b5e + ",";
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.LastIndexOf(","));
            }
            RoleHelper.AddPrivilegeInRoles(roleId, str);
            ManagerHelper.ClearRolePrivilege(roleId);
        }
    }
}

