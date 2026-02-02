namespace LeitorNFC;

[ApiController]
[Route("api/[controller]")]
public class LeitorController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly INFCService _nfcService;

    public LeitorController(HttpClient httpClient, INFCService nfcService)
    {
        _httpClient = httpClient;
        _nfcService = nfcService;
    }

    [HttpGet("nfc")]
    public async Task<IActionResult> LerNFC(string urlNFC)
    {
        var retornoHTTP = await _httpClient.GetAsync(urlNFC);
        var retornoHTML = await retornoHTTP.Content.ReadAsStringAsync();
        var nfc = _nfcService.ParseNFC(retornoHTML);
        //var nfc_banco = (await _nfcRepository.GetAsync()).Where(a => a.ChaveAcesso == nfc.ChaveAcesso);
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
