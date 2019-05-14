using TestCore.Common.Ioc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TestCore.IRepository
{
    public interface IConnectionFactory : IInstancePerDependency
    {

        IDbConnection OpenConnection(string connString = null);

    }
}
