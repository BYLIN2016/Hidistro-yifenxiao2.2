namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Web.UI.WebControls;

    public class Common_Point_PointList : AscxTemplatedWebControl
    {
        private DataList dataListPointDetails;
        private DataListItemEventHandler ItemDataBound;
        public const string TagID = "Common_Point_PointList";

        public event DataListItemEventHandler _ItemDataBound
        {
            add
            {
                DataListItemEventHandler handler2;
                DataListItemEventHandler itemDataBound = this.ItemDataBound;
                do
                {
                    handler2 = itemDataBound;
                    DataListItemEventHandler handler3 = (DataListItemEventHandler) Delegate.Combine(handler2, value);
                    itemDataBound = Interlocked.CompareExchange<DataListItemEventHandler>(ref this.ItemDataBound, handler3, handler2);
                }
                while (itemDataBound != handler2);
            }
            remove
            {
                DataListItemEventHandler handler2;
                DataListItemEventHandler itemDataBound = this.ItemDataBound;
                do
                {
                    handler2 = itemDataBound;
                    DataListItemEventHandler handler3 = (DataListItemEventHandler) Delegate.Remove(handler2, value);
                    itemDataBound = Interlocked.CompareExchange<DataListItemEventHandler>(ref this.ItemDataBound, handler3, handler2);
                }
                while (itemDataBound != handler2);
            }
        }

        public Common_Point_PointList()
        {
            base.ID = "Common_Point_PointList";
        }

        protected override void AttachChildControls()
        {
            this.dataListPointDetails = (DataList) this.FindControl("dataListPointDetails");
            this.dataListPointDetails.ItemDataBound += new DataListItemEventHandler(this.dataListPointDetails_ItemDataBound);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListPointDetails.DataSource != null)
            {
                this.dataListPointDetails.DataBind();
            }
        }

        private void dataListPointDetails_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            this.ItemDataBound(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserPointList.ascx";
            }
            base.OnInit(e);
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListPointDetails.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListPointDetails.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
    }
}

