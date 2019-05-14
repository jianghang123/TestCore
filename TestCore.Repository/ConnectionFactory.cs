using TestCore.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TestCore.IRepository;

namespace TestCore.Repository
{
    public class ConnectionFactory : IConnectionFactory
    {
        public string GamePlatformConnString { get; set; }

        public ConnectionFactory(IOptions<ConnectionStrings> ConnStringsOption)
        {
            this.GamePlatformConnString = ConnStringsOption.Value.ConnectionSqlService;
        }

        public IDbConnection OpenConnection(string connString = null)
        {
            try
            {

                if (string.IsNullOrEmpty(connString))
                {
                    connString = this.GamePlatformConnString;
                }
                var conn = new SqlConnection(connString);

                conn.Open();

                return conn;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
