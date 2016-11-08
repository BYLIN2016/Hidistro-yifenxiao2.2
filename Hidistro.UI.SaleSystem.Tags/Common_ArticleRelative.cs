namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Data;

    public class Common_ArticleRelative : ThemedTemplatedRepeater
    {
        private int maxNum = 6;
        public const string TagID = "list_Common_ArticleRelative";

        public Common_ArticleRelative()
        {
            base.ID = "list_Common_ArticleRelative";
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            DataTable dataSource = (DataTable) this.DataSource;
            if ((dataSource != null) && (dataSource.Rows.Count > this.maxNum))
            {
                int num = dataSource.Rows.Count - 1;
                for (int i = num; i >= this.maxNum; i--)
                {
                    dataSource.Rows.RemoveAt(i);
                }
            }
            base.DataSource = dataSource;
            base.DataBind();
        }

        [Browsable(false)]
        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                base.DataSource = value;
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

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}

