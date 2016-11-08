namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Greeting : WebControl
    {
        private bool displayWeekday = true;

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            IUser user = HiContext.Current.User;
            if (!user.IsAnonymous)
            {
                Label child = new Label();
                child.CssClass = this.CssClass;
                child.Text = string.Format("欢迎回来<b>{0}</b>", user.Username);
                child.Text = child.Text + string.Format("，今天是：{0}&nbsp;", DateTime.Now.ToString("yyyy-MM-dd"));
                if (this.DisplayWeekday)
                {
                    string[] strArray = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                    child.Text = child.Text + strArray[Convert.ToInt16(DateTime.Now.DayOfWeek)];
                }
                this.Controls.Add(child);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderChildren(writer);
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            if (this.HasControls())
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    this.Controls[i].RenderControl(writer);
                }
            }
        }

        public bool DisplayWeekday
        {
            get
            {
                return this.displayWeekday;
            }
            set
            {
                this.displayWeekday = value;
            }
        }
    }
}

