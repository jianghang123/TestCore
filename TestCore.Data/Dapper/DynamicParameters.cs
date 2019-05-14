using Dapper;
using System;

namespace TestCore.Data.Dapper
{
    public class DynamicParameters<T> : DynamicParameters
    {
 
        public void AddParams(Func<T, bool> func)
        {
        }
    }
}
