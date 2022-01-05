/// <summary>
/// Oracle 帮助类
/// </summary>
public static class OracleHelper
{   
    /// <summary>
    /// 获取 in where 条件
    /// 针对 Oracle 查询中 in 参数超过1000的处理方法
    /// </summary>
    /// <param name="dt">数据源 DataTable</param>
    /// <param name="columnName">DataTable 列名</param>
    /// <param name="keyName">数据库表列名</param>
    /// <returns></returns>
    public static string GetInWhere(DataTable dt, string columnName, string keyName)
    {
        var where = string.Empty;
        var whereKey = string.Empty;
        dt = dt.DefaultView.ToTable(true, columnName);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            var key = Convert.ToString(dt.Rows[i][columnName]);
            if (string.IsNullOrEmpty(key))
            {
                continue;
            }
            if (i == dt.Rows.Count - 1)
            {
                whereKey += $"'{key}'";
            }
            else if (i > 0 && i % 999 == 0)
            {
                whereKey += $"'{key}') OR {keyName} IN (";
            }
            else
            {
                whereKey += $"'{key}',";
            }
        }
        where += $" {keyName} IN ({whereKey}) ";
        return where;
    }

    /// <summary>
    /// 获取 in where 条件
    /// 针对 Oracle 查询中 in 参数超过1000的处理方法
    /// </summary>
    /// <param name="listKey">参数集合</param>
    /// <param name="keyName">列名</param>
    /// <returns></returns>
    public static string GetInWhere(List<string> listKey, string keyName)
    {
        var where = string.Empty;
        var whereKey = string.Empty;
        listKey = listKey.Distinct().ToList();
        for (int i = 0; i < listKey.Count; i++)
        {
            var key = listKey[i];
            if (string.IsNullOrEmpty(key))
            {
                continue;
            }
            if (i == listKey.Count - 1)
            {
                whereKey += $"'{key}'";
            }
            else if (i> 0 && i % 999 == 0)
            {
                whereKey += $"'{key}') OR {keyName} IN (";
            }
            else
            {
                whereKey += $"'{key}',";
            }
        }
        where += $" {keyName} IN ({whereKey}) ";
        return where;
    }

}
