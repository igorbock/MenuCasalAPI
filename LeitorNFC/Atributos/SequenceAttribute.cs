namespace LeitorNFC.Atributos;

public class SequenceAttribute : Attribute
{
    public string? Schema { get; set; }
    public string? Name { get; }

    public SequenceAttribute(string name)
    {
        Name = name;
    }
}