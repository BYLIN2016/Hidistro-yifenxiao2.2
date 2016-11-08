namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ClientGroup)]
    public class ClientSet : AdminPage
    {
        protected Button btnSaveClientSettings;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected HtmlInputRadioButton radioactivymoney;
        protected HtmlInputRadioButton radioactivyorder;
        protected HtmlInputRadioButton radnewday;
        protected HtmlInputRadioButton radnewtime;
        protected HtmlSelect slsactivymoney;
        protected HtmlSelect slsactivymoneychar;
        protected HtmlSelect slsactivyorder;
        protected HtmlSelect slsactivyorderchar;
        protected HtmlSelect slsnewregist;
        protected HtmlSelect slssleep;
        protected HtmlInputText txtactivymoney;
        protected HtmlInputText txtactivyorder;

        protected void btnSaveClientSettings_Click(object sender, EventArgs e)
        {
            Dictionary<int, MemberClientSet> clientset = new Dictionary<int, MemberClientSet>();
            MemberClientSet set = new MemberClientSet();
            if (this.radnewtime.Checked)
            {
                set.ClientTypeId = 1;
                if (this.calendarStartDate.SelectedDate.HasValue)
                {
                    set.StartTime = new DateTime?(this.calendarStartDate.SelectedDate.Value);
                }
                if (this.calendarEndDate.SelectedDate.HasValue)
                {
                    set.EndTime = new DateTime?(this.calendarEndDate.SelectedDate.Value);
                }
            }
            else
            {
                set.ClientTypeId = 2;
                set.LastDay = int.Parse(this.slsnewregist.Value);
            }
            clientset.Add(set.ClientTypeId, set);
            set = new MemberClientSet();
            if (this.radioactivyorder.Checked)
            {
                set.ClientTypeId = 6;
                set.LastDay = int.Parse(this.slsactivyorder.Value);
                set.ClientChar = this.slsactivyorderchar.Value;
                if (!string.IsNullOrEmpty(this.txtactivyorder.Value))
                {
                    set.ClientValue = decimal.Parse(this.txtactivyorder.Value);
                }
            }
            else
            {
                set.ClientTypeId = 7;
                set.LastDay = int.Parse(this.slsactivymoney.Value);
                set.ClientChar = this.slsactivymoneychar.Value;
                if (!string.IsNullOrEmpty(this.txtactivymoney.Value))
                {
                    set.ClientValue = decimal.Parse(this.txtactivymoney.Value);
                }
            }
            clientset.Add(set.ClientTypeId, set);
            set = new MemberClientSet();
            set.ClientTypeId = 11;
            set.LastDay = int.Parse(this.slssleep.Value);
            clientset.Add(set.ClientTypeId, set);
            if (MemberHelper.InsertClientSet(clientset))
            {
                this.ShowMsg("保存成功", true);
            }
            else
            {
                this.ShowMsg("保存失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveClientSettings.Click += new EventHandler(this.btnSaveClientSettings_Click);
            if (!base.IsPostBack)
            {
                Dictionary<int, MemberClientSet> memberClientSet = MemberHelper.GetMemberClientSet();
                if (memberClientSet.Count == 3)
                {
                    foreach (int num in memberClientSet.Keys)
                    {
                        switch (num)
                        {
                            case 1:
                            {
                                if (memberClientSet[num].StartTime.HasValue)
                                {
                                    this.calendarStartDate.SelectedDate = new DateTime?(memberClientSet[num].StartTime.Value.Date);
                                }
                                if (memberClientSet[num].EndTime.HasValue)
                                {
                                    this.calendarEndDate.SelectedDate = new DateTime?(memberClientSet[num].EndTime.Value.Date);
                                }
                                continue;
                            }
                            case 2:
                            {
                                this.slsnewregist.Value = memberClientSet[num].LastDay.ToString();
                                this.radnewday.Checked = true;
                                continue;
                            }
                            case 3:
                            case 4:
                            case 5:
                            {
                                continue;
                            }
                            case 6:
                            {
                                this.slsactivyorder.Value = memberClientSet[num].LastDay.ToString();
                                this.slsactivyorderchar.Value = memberClientSet[num].ClientChar.ToString();
                                this.txtactivyorder.Value = memberClientSet[num].ClientValue.ToString("F0");
                                continue;
                            }
                            case 7:
                            {
                                this.slsactivymoney.Value = memberClientSet[num].LastDay.ToString();
                                this.slsactivymoneychar.Value = memberClientSet[num].ClientChar.ToString();
                                this.txtactivymoney.Value = memberClientSet[num].ClientValue.ToString("F2");
                                this.radioactivymoney.Checked = true;
                                continue;
                            }
                            case 11:
                                break;

                            default:
                            {
                                continue;
                            }
                        }
                        this.slssleep.Value = memberClientSet[num].LastDay.ToString();
                    }
                }
            }
        }
    }
}

