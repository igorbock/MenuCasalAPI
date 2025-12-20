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
        return Ok(retornoHTML);
    }
}
