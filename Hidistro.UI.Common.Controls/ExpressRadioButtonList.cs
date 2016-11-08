namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class ExpressRadioButtonList : RadioButtonList
    {
        
        private IList<string> _ExpressCompanies;
        
        private string _Name;

        public override void DataBind()
        {
            IList<string> expressCompanies = this.ExpressCompanies;
            if ((expressCompanies == null) || (expressCompanies.Count == 0))
            {
                expressCompanies = ExpressHelper.GetAllExpressName();
            }
            base.Items.Clear();
            foreach (string str in expressCompanies)
            {
                ListItem item = new ListItem(str, str);
                if (string.Compare(item.Value, this.Name, false) == 0)
                {
                    item.Selected = true;
                }
                base.Items.Add(item);
            }
        }

        public IList<string> ExpressCompanies
        {
            
            get
            {
                return _ExpressCompanies;
            }
            
            set
            {
                _ExpressCompanies = value;
            }
        }

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            set
            {
                _Name = value;
            }
        }
    }
}

