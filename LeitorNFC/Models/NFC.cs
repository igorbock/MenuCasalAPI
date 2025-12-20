namespace LeitorNFC.Models;

[Table("NFC_COMPRA", Schema = "NFC")]
public class NFC : IEntity
{
    [Key]
    public int Id { get; set; }
    public required DateTime DataEmissao { get; set; }
    
    [StringLength(44)]
    public required string ChaveAcesso { get; set; }

    [StringLength(100)]
    public required string NomeComercio { get; set; }

    [StringLength(11)]
    public string? CPFConsumidor { get; set; }
}
