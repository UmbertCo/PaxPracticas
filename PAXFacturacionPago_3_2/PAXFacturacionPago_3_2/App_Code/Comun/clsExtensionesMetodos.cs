using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsExtensionesMetodos
/// </summary>
public static class clsExtensionesMetodos
{
    public static void Map<T>(this IEnumerable<T> source, Action<T> func)
    {
        foreach (T i in source)
            func(i);
    }

    public static DataTable ToDataTable<T>(this IEnumerable<T> source)
    {
        var dt = new DataTable();
        var properties = typeof(T).GetProperties();
        dt.Columns.AddRange(properties.Select(x => new DataColumn(x.Name, x.PropertyType)).ToArray());
        source.Select(x => dt.NewRow().ItemArray = properties.Select(y => y.GetValue(x, null)).ToArray()).Map(x => dt.Rows.Add(x));
        return dt;
    }
}