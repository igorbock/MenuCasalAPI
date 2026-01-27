namespace LeitorNFC.Models.Generico;

public class EntityMetadata
{
    public string TableName { get; init; } = default!;
    public string Schema { get; init; } = "public";
    public PropertyInfo Key { get; init; } = default!;
    public IReadOnlyList<PropertyInfo> Columns { get; init; } = default!;
    public string Sequence { get; set; } = default!;
}
