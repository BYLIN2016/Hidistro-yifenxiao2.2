namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendMyCouponToUnderling : DistributorPage
    {
        protected Button btnSend;
        private int couponId;
        protected UnderlingGradeDropDownList rankList;
        protected HtmlInputRadioButton rdoName;
        protected HtmlInputRadioButton rdoRank;
        protected TextBox txtMemberNames;

        private void btnSend_Click(object sender, EventArgs e)
        {
            CouponItemInfo item = new CouponItemInfo();
            IList<CouponItemInfo> listCouponItem = new List<CouponItemInfo>();
            IList<Member> memdersByNames = new List<Member>();
            if (this.rdoName.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
                {
                    IList<string> names = new List<string>();
                    string[] strArray = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (this.IsMembers(strArray[i]))
                        {
                            names.Add(strArray[i]);
                        }
                    }
                    memdersByNames = SubsitePromoteHelper.GetMemdersByNames(names);
                }
                string claimCode = string.Empty;
                foreach (Member member in memdersByNames)
                {
                    claimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(this.couponId, claimCode, new int?(member.UserId), member.Username, member.Email, DateTime.Now);
                    listCouponItem.Add(item);
                }
                if (listCouponItem.Count <= 0)
                {
                    this.ShowMsg("你输入的会员名中没有一个正确的，请输入正确的会员名", false);
                    return;
                }
                SubsiteCouponHelper.SendClaimCodes(this.couponId, listCouponItem);
                this.txtMemberNames.Text = string.Empty;
                this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", listCouponItem.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                memdersByNames = SubsitePromoteHelper.GetMembersByRank(this.rankList.SelectedValue);
                string str3 = string.Empty;
                foreach (Member member2 in memdersByNames)
                {
                    str3 = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
                    item = new CouponItemInfo(this.couponId, str3, new int?(member2.UserId), member2.Username, member2.Email, DateTime.Now);
                    listCouponItem.Add(item);
                }
                if (listCouponItem.Count <= 0)
                {
                    this.ShowMsg("您选择的会员等级下面没有会员", false);
                }
                else
                {
                    SubsiteCouponHelper.SendClaimCodes(this.couponId, listCouponItem);
                    this.txtMemberNames.Text = string.Empty;
                    this.ShowMsg(string.Format("此次发送操作已成功，优惠券发送数量：{0}", listCouponItem.Count), true);
                }
            }
        }

        private bool IsMembers(string name)
        {
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            new Regex(pattern);
            return ((name.Length >= 2) && (name.Length <= 20));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSend.Click += new EventHandler(this.btnSend_Click);
                if (!this.Page.IsPostBack)
                {
                    this.rankList.DataBind();
                }
            }
        }
    }
}

