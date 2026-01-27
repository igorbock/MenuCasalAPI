namespace LeitorNFC.Models;

[Table("NFC_ITEM", Schema = "NFC")]
public class ItemNFC : IEntity
{
    [Key]
    public long Id { get; set; }

    [StringLength(2)]
    public string? Codigo { get; set; }    
    
    [StringLength(255)]
    public required string Descricao { get; set; }

    [Column(TypeName = "decimal(15,4)")]
    public decimal Quantidade { get; set; }
    
    [StringLength(3)]
    public string? Unidade { get; set; }

    [Column(TypeName = "decimal(15,4)")]
    public decimal ValorUnitario { get; set; }

    [Column(TypeName = "decimal(15,4)")]
    public decimal ValorTotal { get; set; }
    
    [ForeignKey("NFCCompra")]
    public int IdCompra { get; set; }
}
