using System;

namespace Norm
{
    [Flags]
    public enum DatabaseType
    {
        Other = 0,
        Sql = 1,
        Npgsql = 2,
        MySql = 4,
    }
}
