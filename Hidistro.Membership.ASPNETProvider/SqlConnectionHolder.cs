namespace Hidistro.Membership.ASPNETProvider
{
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.Hosting;

    internal sealed class SqlConnectionHolder
    {
        internal SqlConnection _Connection;
        private bool _Opened;

        internal SqlConnectionHolder(string connectionString)
        {
            try
            {
                this._Connection = new SqlConnection(connectionString);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("An error occurred while attempting to initialize a System.Data.SqlClient.SqlConnection object. The value that was provided for the connection string may be wrong, or it may contain an invalid syntax."), "connectionString", exception);
            }
        }

        internal void Close()
        {
            if (this._Opened)
            {
                this.Connection.Close();
                this._Opened = false;
            }
        }

        internal void Open(HttpContext context, bool revertImpersonate)
        {
            if (this._Opened)
            {
                return;
            }
            if (revertImpersonate)
            {
                using (HostingEnvironment.Impersonate())
                {
                    this.Connection.Open();
                    goto Label_0034;
                }
            }
            this.Connection.Open();
        Label_0034:
            this._Opened = true;
        }

        internal SqlConnection Connection
        {
            get
            {
                return this._Connection;
            }
        }
    }
}

