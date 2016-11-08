namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Subsites.Members;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class UnderlingGradeCheckBoxList : CheckBoxList
    {
        public override void DataBind()
        {
            this.Items.Clear();
            IList<MemberGradeInfo> underlingGrades = UnderlingHelper.GetUnderlingGrades();
            int num = 0;
            foreach (MemberGradeInfo info in underlingGrades)
            {
                this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.GradeId.ToString()));
                this.Items[num++].Selected = true;
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

