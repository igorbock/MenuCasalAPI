namespace LeitorNFC.Helpers;

public class EntityMetadataResolver
{
    public static EntityMetadata Resolve<T>()
    {
        var type = typeof(T);

        var tableAttr = type.GetCustomAttribute<TableAttribute>() ?? throw new InvalidOperationException($"Entidade {type.Name} sem [Table]");
        var sequence = type.GetCustomAttribute<SequenceAttribute>() ?? throw new InvalidOperationException($"Entidade {type.Name} sem [Sequence]");

        var properties = type.GetProperties()
            .Where(p => p.GetCustomAttribute<ColumnAttribute>() != null)
            .ToList();

        var key = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null) ?? throw new InvalidOperationException($"Entidade {type.Name} sem [Key]");

        return new EntityMetadata
        {
            TableName = tableAttr.Name,
            Schema = tableAttr.Schema ?? "public",
            Key = key,
            Columns = properties,
            Sequence = $"{sequence.Schema}.{sequence.Name}"
        };
    }
}
