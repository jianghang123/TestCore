using System;
using System.Reflection;
using TestCore.Domain.Enums;

namespace Caiba.IRepositories
{
    public class TableHelper
    {
        public static int GetTableId(Type type)
        {
            return GetTableId(type.Name);
        }

        public static int GetTableId<T>( )
        {
            return GetTableId(typeof(T).Name);
        }

        public static int GetTableId(string tableName)
        {
            return (int)GetTableEnum(tableName);
        }

        public static TableEnum GetTableEnum(string tableName)
        {
            return  (TableEnum)(Enum.Parse(typeof(TableEnum), tableName));
        }


        public static Type GetTypeByTable(int tableId)
        {
            var typeName = ((TableEnum)tableId).ToString();

            return GetTypeByTable(typeName);
        }


        public static Type GetTypeByTable(string tableName)
        {
            AssemblyName assemblyName = new AssemblyName("TestCore.Domain, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null");

            var assembly = Assembly.Load(assemblyName);

            var type = assembly.GetType(string.Format("TestCore.Domain.Entity.{0}", tableName));

            return type;
        }
    }
}
