namespace LeitorNFC.Models;

[Table("compra", Schema = "nfc")]
public class NFC : IEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [StringLength(100)]
    [Column("nome_emitente")]
    public required string NomeEmitente { get; set; }

    [StringLength(14)]
    [Column("cnpj_emitente")]
    public required string CNPJEmitente { get; set; }

    [StringLength(100)]
    [Column("endereco_emitente")]
    public required string EnderecoEmitente { get; set; }

    [Column("tipo_emissao")]
    public string? TipoEmissao { get; set; }

    [Column("numero")]
    public int? Numero { get; set; }

    [Column("serie")]
    public string? Serie { get; set; }

    [Column("data_emissao")]
    public required DateTime DataEmissao { get; set; }

    [Column("protocolo_autorizacao")]
    public string? ProtocoloAutorizacao { get; set; }

    [Column("data_protocolo_autorizacao")]
    public DateTime? DataProtocoloAutorizacao { get; set; }

    [Column("ambiente")]
    public string? Ambiente { get; set; }

    [StringLength(44)]
    [Column("chave_acesso")]
    public required string ChaveAcesso { get; set; }

    [StringLength(11)]
    [Column("cpf_consumidor")]
    public string? CPFConsumidor { get; set; }

    [Column("nome_consumidor")]
    public string? NomeConsumidor { get; set; }

    [Column("tributos_aproximados")]
    public decimal? TributosAproximados { get; set; }

    [Column("tributos_federais")]
    public decimal? TributosFederais { get; set; }

    public List<ItemNFC>? Itens { get; set; }
}
