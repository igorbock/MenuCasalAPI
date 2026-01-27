namespace LeitorNFC.Helpers;

public class SqlBuilderHelper
{
    public static string Col(PropertyInfo p) => p.GetCustomAttribute<ColumnAttribute>()!.Name ?? string.Empty;

    public static string BuildInsert<T>()
    {
        var meta = EntityMetadataResolver.Resolve<T>();

        var columns = meta.Columns.ToList();

        var colNames = string.Join(", ", columns.Select(Col));
        var paramNames = string.Join(", ", columns.Select(p => "@" + p.Name));

        return $@"
    INSERT INTO {meta.Schema}.{meta.TableName}
    ({colNames})
    VALUES ({paramNames})";
    }

    public static string BuildUpdate<T>()
    {
        var meta = EntityMetadataResolver.Resolve<T>();

        var sets = meta.Columns
            .Where(p => p != meta.Key)
            .Select(p => $"{Col(p)} = @{p.Name}");

        return $@"
    UPDATE {meta.Schema}.{meta.TableName}
    SET {string.Join(", ", sets)}
    WHERE {Col(meta.Key)} = @{meta.Key.Name}";
    }

    public static string BuildDelete<T>()
    {
        var meta = EntityMetadataResolver.Resolve<T>();

        return $@"
    DELETE FROM {meta.Schema}.{meta.TableName}
    WHERE {Col(meta.Key)} = @id";
    }

    public static string BuildSelectById<T>()
    {
        var meta = EntityMetadataResolver.Resolve<T>();

        var cols = string.Join(", ", meta.Columns.Select(Col));

        return $@"
    SELECT {cols}
    FROM {meta.Schema}.{meta.TableName}
    WHERE {Col(meta.Key)} = @id";
    }

    public static string BuildSelectAll<T>()
    {
        var meta = EntityMetadataResolver.Resolve<T>();

        var cols = string.Join(", ", meta.Columns.Select(Col));

        return $@"
    SELECT {cols}
    FROM {meta.Schema}.{meta.TableName}";
    }

    public static string BuildSequence<T>()
    {
        var meta = EntityMetadataResolver.Resolve<T>();

        return $"SELECT nextval('{meta.Sequence}')";
    }
}
