namespace LeitorNFC.Models;

[Table("NFC_COMPRA", Schema = "NFC")]
public class NFC : IEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public required string NomeEmitente { get; set; }

    [StringLength(14)]
    public required string CNPJEmitente { get; set; }

    [StringLength(100)]
    public required string EnderecoEmitente { get; set; }

    public string? TipoEmissao { get; set; }

    public int? Numero { get; set; }

    public string? Serie { get; set; }

    public required DateTime DataEmissao { get; set; }

    public string? ProtocoloAutorizacao { get; set; }

    public DateTime? DataProtocoloAutorizacao { get; set; }

    public string? Ambiente { get; set; }

    [StringLength(44)]
    public required string ChaveAcesso { get; set; }

    [StringLength(11)]
    public string? CPFConsumidor { get; set; }

    public string? NomeConsumidor { get; set; }

    public decimal? TributosAproximados { get; set; }

    public decimal? TributosFederais { get; set; }

    public List<ItemNFC>? Itens { get; set; }
}
