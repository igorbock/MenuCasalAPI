namespace LeitorNFC;

[ApiController]
[Route("[controller]")]
public class LeitorController : ControllerBase
{
    private readonly IRepository<NFC> _nfcRepository;
    private readonly IRepository<ItemNFC> _itemNFCRepository;
    private readonly HttpClient _httpClient;

    public LeitorController(IRepository<NFC> nfcRepository, IRepository<ItemNFC> itemNFCRepository, HttpClient httpClient)
    {
        _nfcRepository = nfcRepository;
        _itemNFCRepository = itemNFCRepository;
        _httpClient = httpClient;
    }

    [HttpGet("nfc")]
    public async Task<IActionResult> LerNFC(string urlNFC)
    {
        var retornoHTTP = await _httpClient.GetAsync(urlNFC);
        var retornoHTML = await retornoHTTP.Content.ReadAsStringAsync();
        var nfc = NFCService.ParseNFC(retornoHTML);
        //var nfc_banco = await _nfcRepository.GetAllAsync(a => a.ChaveAcesso == nfc.ChaveAcesso);
        //if (nfc_banco.Any())
        //    return BadRequest("A NFC já está cadastrada");
        //if (nfc.Itens == null)
        //    return BadRequest("A NFC não tem itens");
        //// Primeiro cadastro da NFC
        //await _nfcRepository.AddAsync(nfc);
        //// Depois dos itens
        //foreach (var item in nfc.Itens)
        //    await _itemNFCRepository.AddAsync(item);
        return Ok(nfc);
    }
}
