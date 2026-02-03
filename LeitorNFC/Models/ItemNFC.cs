namespace LeitorNFC.Models;

[Table("item", Schema = "nfc")]
[Sequence("seq_nfc_item", Schema = "nfc")]
public class ItemNFC : IEntity
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [StringLength(10)]
    [Column("codigo")]
    public string? Codigo { get; set; }    
    
    [StringLength(255)]
    [Column("descricao")]
    public required string Descricao { get; set; }

    [Column("quantidade", TypeName = "decimal(15,4)")]
    public decimal Quantidade { get; set; }
    
    [StringLength(3)]
    [Column("unidade")]
    public string? Unidade { get; set; }

    [Column("valor_unitario", TypeName = "decimal(15,4)")]
    public decimal ValorUnitario { get; set; }

    [Column("valor_total", TypeName = "decimal(15,4)")]
    public decimal ValorTotal { get; set; }
    
    [Column("id_compra")]
    public long IdCompra { get; set; }
}
