namespace LeitorNFC;

[ApiController]
[Route("api/[controller]")]
public class LeitorController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly INFCService _nfcService;

    public LeitorController(IHttpClientFactory httpFactory, INFCService nfcService)
    {
        _httpClient = httpFactory.CreateClient("LeitorNFC");
        _nfcService = nfcService;
    }

    [HttpGet("nfc")]
    public async Task<IActionResult> LerNFC(string urlNFC)
    {
        var retornoHTTP = await _httpClient.GetAsync(urlNFC);
        var retornoHTML = await retornoHTTP.Content.ReadAsStringAsync();
        //var nfc = _nfcService.ParseNFC(retornoHTML);
        var nfc = await _nfcService.SalvarNFCAsync(retornoHTML);
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

    [HttpGet("teste")]
    public async Task<IActionResult> Teste(string msg)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(new
        {
            model = "openai/gpt-oss-120b:fastest",
            stream = false,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = msg
                }
            }
        });
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("chat/completions", content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return Ok(responseContent);
    }
}
