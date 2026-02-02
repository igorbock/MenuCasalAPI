namespace LeitorNFC.Interfaces;

public interface INFCService
{
    NFC Compra { get; set; }
    IEnumerable<ItemNFC> Itens { get; set; }
    Task<NFC> SalvarNFCAsync(string htmlNFC);
    NFC ParseNFC(string html);
    IEnumerable<ItemNFC> ParseItens(string html);
}
