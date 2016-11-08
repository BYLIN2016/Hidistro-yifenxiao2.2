namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;

    public class ProductTagsCheckBoxList : CheckBoxList
    {
        public override void DataBind()
        {
            base.Items.Clear();
            foreach (DataRow row in ControlProvider.Instance().GetTags().Rows)
            {
                ListItem item = new ListItem(Globals.HtmlEncode(row["TagName"].ToString()), row["TagID"].ToString());
                base.Items.Add(item);
            }
        }

        public IList<int> SelectedValue
        {
            get
            {
                IList<int> list = new List<int>();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        list.Add(int.Parse(this.Items[i].Value));
                    }
                }
                return list;
            }
            set
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.Items[i].Selected = false;
                }
                IList<int> list = value;
                foreach (int num2 in list)
                {
                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if (this.Items[j].Value == num2.ToString())
                        {
                            this.Items[j].Selected = true;
                        }
                    }
                }
            }
        }
    }
}

