using System.Data;
using Dapper;

namespace Issueneter.Persistence.TypeHandlers;

public class ScanTypeHandler : SqlMapper.ITypeHandler
{
    public void SetValue(IDbDataParameter parameter, object scanType)
    {
        parameter.DbType = DbType.String;
        parameter.Value = ((ScanType)scanType).Value;
    }

    public object Parse(Type destinationType, object value)
    {
        if (destinationType == typeof(ScanType))
            return ScanType.Parse((string)value);
        return null;
    }
}