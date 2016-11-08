namespace Hidistro.UI.Web.Admin.store
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class RestoreDatabase : AdminPage
    {
        protected Grid grdBackupFiles;

        private void BindBackupData()
        {
            this.grdBackupFiles.DataSource = StoreHelper.GetBackupFiles();
            this.grdBackupFiles.DataBind();
        }

        private void grdBackupFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            if (e.CommandName == "Restore")
            {
                HyperLink link = this.grdBackupFiles.Rows[rowIndex].FindControl("hlinkName") as HyperLink;
                if (StoreHelper.RestoreData(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/" + link.Text.Trim())))
                {
                    this.ShowMsg("数据库已恢复完毕", true);
                }
                else
                {
                    this.ShowMsg("数据库恢复失败，请重试", false);
                }
            }
        }

        private void grdBackupFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            HyperLink link = this.grdBackupFiles.Rows[e.RowIndex].FindControl("hlinkName") as HyperLink;
            if (StoreHelper.DeleteBackupFile(link.Text.Trim()))
            {
                File.Delete(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/" + link.Text.Trim()));
                this.BindBackupData();
                this.ShowMsg("成功删除了选择的备份文件", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdBackupFiles.RowCommand += new GridViewCommandEventHandler(this.grdBackupFiles_RowCommand);
            this.grdBackupFiles.RowDeleting += new GridViewDeleteEventHandler(this.grdBackupFiles_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindBackupData();
            }
        }
    }
}

