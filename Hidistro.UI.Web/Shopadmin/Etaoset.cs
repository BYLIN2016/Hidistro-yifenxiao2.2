namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Etaoset : DistributorPage
    {
        protected Button btnChangeEmailSettings;
        protected Button BtnCreateEtao;
        protected Button btnUpoad;
        protected HtmlGenericControl etaoset;
        protected FileUpload fudVerifyFile;
        protected HtmlGenericControl fulDir;
        protected HtmlGenericControl incDir;
        protected Label lbEtaoCreate;
        protected Label lblEtaoFeedFull;
        protected Label lblEtaoFeedInc;
        protected YesNoRadioButtonList rdobltIsCreateFeed;
        private SiteSettings siteSettings;
        protected TextBox txtEtaoID;

        protected void btnChangeEmailSettings_Click(object sender, EventArgs e)
        {
            this.siteSettings.EtaoID = this.txtEtaoID.Text;
            this.siteSettings.EtaoStatus = this.rdobltIsCreateFeed.SelectedValue ? 1 : 0;
            SettingsManager.Save(this.siteSettings);
            this.ShowMsg("保存成功。", true);
        }

        protected void BtnCreateEtao_Click(object sender, EventArgs e)
        {
            this.siteSettings.EtaoStatus = -1;
            this.siteSettings.EtaoApplyTime = new DateTime?(DateTime.Now);
            SettingsManager.Save(this.siteSettings);
            this.ShowMsg("申请成功。", true);
            this.BtnCreateEtao.Visible = false;
            this.lbEtaoCreate.Visible = true;
            this.lbEtaoCreate.Text = "您已经于" + this.siteSettings.EtaoApplyTime.ToString() + " 申请开通一淘，请等待管理员审核。";
        }

        protected void btnUpoad_Click(object sender, EventArgs e)
        {
            if (this.fudVerifyFile.HasFile)
            {
                if (this.fudVerifyFile.PostedFile.ContentType.ToLower(CultureInfo.InvariantCulture) != "text/plain")
                {
                    this.ShowMsg("只能上传TXT文本文件", false);
                }
                else if (!this.fudVerifyFile.FileName.ToLower(CultureInfo.InvariantCulture).EndsWith(".txt") || (this.fudVerifyFile.FileName.IndexOf('.') != (this.fudVerifyFile.FileName.Length - 4)))
                {
                    this.ShowMsg("文件名只能有一个.号", false);
                }
                else if (this.fudVerifyFile.FileName.ToLower() != "etao_domain_verify.txt")
                {
                    this.ShowMsg("你上传的不是一淘的验证文件!", false);
                }
                else
                {
                    string str = "etao_domain_verify.txt";
                    string applicationPath = string.Empty;
                    string filename = string.Empty;
                    if (!string.IsNullOrEmpty(Globals.ApplicationPath))
                    {
                        if (Globals.ApplicationPath.EndsWith(@"\"))
                        {
                            applicationPath = Globals.ApplicationPath;
                        }
                        else
                        {
                            applicationPath = Globals.ApplicationPath + @"\";
                        }
                        filename = HiContext.Current.Context.Request.MapPath(applicationPath + str);
                    }
                    else
                    {
                        filename = HiContext.Current.Context.Request.MapPath("/");
                        if (filename.EndsWith(@"\"))
                        {
                            filename = filename + str;
                        }
                        else
                        {
                            filename = filename + @"\" + str;
                        }
                    }
                    this.fudVerifyFile.SaveAs(filename);
                    this.ShowMsg("上传成功。", true);
                }
            }
            else
            {
                this.ShowMsg("需要选择验证文件再点击上传。", false);
            }
        }

        protected void LoadEtaoOpen()
        {
            if (!string.IsNullOrEmpty(this.siteSettings.EtaoID))
            {
                this.txtEtaoID.Text = this.siteSettings.EtaoID;
            }
            this.rdobltIsCreateFeed.SelectedValue = this.siteSettings.EtaoStatus == 1;
            this.lblEtaoFeedInc.Text = string.Concat(new object[] { "http://", this.siteSettings.SiteUrl, Globals.ApplicationPath, "/Storage/Root/", this.siteSettings.UserId, "/IncrementIndex.xml" });
            this.lblEtaoFeedFull.Text = string.Concat(new object[] { "http://", this.siteSettings.SiteUrl, Globals.ApplicationPath, "/Storage/Root/", this.siteSettings.UserId, "/FullIndex.xml" });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if (this.siteSettings.IsOpenEtao)
            {
                this.etaoset.Visible = true;
                this.lbEtaoCreate.Visible = false;
                this.BtnCreateEtao.Visible = false;
                if (!this.Page.IsPostBack)
                {
                    this.LoadEtaoOpen();
                }
            }
            else if (this.siteSettings.EtaoStatus == -1)
            {
                this.BtnCreateEtao.Visible = false;
                this.lbEtaoCreate.Visible = true;
                this.lbEtaoCreate.Text = "您已经于" + this.siteSettings.EtaoApplyTime.ToString() + " 申请开通一淘，请等待管理员审核。";
                this.etaoset.Visible = false;
            }
            else if (this.siteSettings.EtaoStatus == 0)
            {
                if (!this.siteSettings.IsOpenEtao)
                {
                    this.BtnCreateEtao.Visible = true;
                    this.lbEtaoCreate.Visible = false;
                    this.etaoset.Visible = false;
                }
            }
            else if ((this.siteSettings.EtaoStatus == 1) && !this.siteSettings.IsOpenEtao)
            {
                this.BtnCreateEtao.Visible = false;
                this.lbEtaoCreate.Visible = true;
                this.lbEtaoCreate.Text = "您的一淘已被管理员暂停。";
                this.etaoset.Visible = false;
            }
        }
    }
}

