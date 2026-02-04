namespace LeitorNFC.Services;

public class NFCService : INFCService
{
    private NFC? _compra;
    public NFC Compra
    {
        get => _compra ?? new NFC();
        set => _compra = value;
    }

    private IEnumerable<ItemNFC>? _itens;
    public IEnumerable<ItemNFC> Itens
    {
        get => _itens ?? new List<ItemNFC>();
        set => _itens = value;
    }

    private readonly IRepository<NFC> _nfcRepository;
    private readonly IRepository<ItemNFC> _itemNFCRepository;
    private readonly IDbConnection _dbConnection;

    public NFCService(IRepository<NFC> nfcRepository, IRepository<ItemNFC> itemNFCRepository, IDbConnection dbConnection)
    {
        _nfcRepository = nfcRepository;
        _itemNFCRepository = itemNFCRepository;
        _dbConnection = dbConnection;
    }

    public IEnumerable<ItemNFC> ParseItens(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var itens = new List<ItemNFC>();

        var rows = doc.DocumentNode
            .SelectNodes("//table[@id='tabResult']//tr");

        if (rows == null)
            return itens;

        foreach (var row in rows)
        {
            var descricao = row.SelectSingleNode(".//span[@class='txtTit']")?.InnerText.Trim();

            var qtdText = row.SelectSingleNode(".//span[@class='Rqtd']")?.InnerText;
            var qtdDecimal = decimal.Parse(qtdText!.Split(":")[1]);
            var strCod = row.SelectSingleNode(".//span[@class='RCod']")?.InnerText;
            var codigo = strCod!.Trim('(', ')', '\n', '\t', '\r').Split(":", 2)[1].Trim();
            var unText  = row.SelectSingleNode(".//span[@class='RUN']")?.InnerText;
            var unidade = unText!.Split(":")[1].Trim();
            var vlUnit  = row.SelectSingleNode(".//span[@class='RvlUnit']")?.InnerText;
            var valorUnitario = decimal.Parse(vlUnit!.Split(":")[1].Trim());
            var vlTotal = row.SelectSingleNode(".//span[@class='valor']")?.InnerText;

            if (descricao == null)
                continue;

            itens.Add(new ItemNFC
            {
                Descricao = descricao,
                Quantidade = qtdDecimal,
                Unidade = unidade,
                ValorUnitario = valorUnitario,
                ValorTotal = decimal.Parse(vlTotal!),
                Codigo = codigo
            });
        }

        return itens;
    }
     
    public NFC ParseNFC(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        // Conteúdo principal
        var conteudo = doc.DocumentNode.SelectSingleNode(@"//*[@id=""conteudo""]");
        // Cabeçalho com informações do Emitente
        var htmlConteudo = new HtmlDocument();
        htmlConteudo.LoadHtml(conteudo.InnerHtml);
        // Obtendo informações
        var nomeEmitente = htmlConteudo.DocumentNode.SelectSingleNode(@"//*[@id=""u20""]").InnerText.Trim();
        var cnpjNode = htmlConteudo.DocumentNode.SelectSingleNode(@"//*[@class=""txtCenter""]/div[2]");
        var cnpjEmitente = SomenteNumeros(LimparTexto(cnpjNode.InnerText));
        var enderecoNode = htmlConteudo.DocumentNode.SelectSingleNode(@"//*[@class=""txtCenter""]/div[3]");
        var enderecoEmitente = LimparTexto(enderecoNode.InnerText);
        // Tabela com informações gerais
        var infosHTML = doc.DocumentNode.SelectSingleNode(@"//*[@id=""infos""]");
        //*[@id="infos"]
        var infoGerais = new HtmlDocument();
        infoGerais.LoadHtml(infosHTML.InnerHtml);
        // Informações gerais da nota
        var tipoEmissao = infoGerais.DocumentNode.SelectSingleNode(@"//li/strong[1]").InnerText.Trim();
        var numero = infoGerais.DocumentNode.SelectSingleNode(@"//li/text()[1]").InnerText.Trim();
        var serie = infoGerais.DocumentNode.SelectSingleNode(@"//li/text()[2]").InnerText.Trim();
        var dataEmissaoRaw = infoGerais.DocumentNode.SelectSingleNode(@"//li/text()[3]").InnerText.Split(" ");
        var dataEmissao = LimparTexto($"{dataEmissaoRaw[0]} {dataEmissaoRaw[1].Replace("-", "")}");
        var protocoloAutorizacaoRaw = LimparTexto(infoGerais.DocumentNode.SelectSingleNode(@"//li/text()[4]").InnerText).Split(" ");
        var protocoloAutorizacao = LimparTexto(protocoloAutorizacaoRaw[0]);
        var dataProtocoloAutorizacao = LimparTexto($"{protocoloAutorizacaoRaw[1]} {protocoloAutorizacaoRaw[3]}");
        var ambiente = LimparTexto(infoGerais.DocumentNode.SelectSingleNode(@"//li/strong[6]/text()").InnerText);
        // Chave de acesso
        var chaveAcesso = infoGerais.DocumentNode.SelectSingleNode(@"//li/span").InnerText.Trim().Replace(" ", "");
        // Consumidor
        var nodeCPF = infoGerais.DocumentNode.SelectSingleNode(@"//div[3]/ul/li/text()[1]");
        var cpf = SomenteNumeros(nodeCPF?.InnerText ?? string.Empty);
        var nomeNode = infoGerais.DocumentNode.SelectSingleNode(@"div[3]/ul/li[2]/strong");
        var nomeRaw = nomeNode?.InnerText ?? string.Empty;
        var nomeSplit = nomeRaw.Split(":");
        var nome = nomeSplit.Length <= 1 ? string.Empty : nomeSplit[1].Trim();
        // Informações de interesse do contribuinte
        var infoContribuinteNode = infoGerais.DocumentNode.SelectSingleNode(@"//div[4]/ul/li");
        var infoContribuinteText = infoContribuinteNode?.InnerText ?? string.Empty;
        var infoContribuinteSplit = infoContribuinteText.Split(" ");
        var tribAprox = infoContribuinteSplit.Length <= 1 ? string.Empty : infoContribuinteSplit[3];
        var tribFed = infoContribuinteSplit.Length <= 1 ? string.Empty : infoContribuinteSplit[5];

        int intNumero;
        DateTime dteEmissao;
        DateTime dteProtocoloAutorizacao;
        decimal decTribAprox;
        decimal decTribFederais;

        int.TryParse(numero, out intNumero);
        DateTime.TryParse(dataEmissao!, out dteEmissao);
        DateTime.TryParse(dataProtocoloAutorizacao, out dteProtocoloAutorizacao);
        decimal.TryParse(tribAprox, out decTribAprox);
        decimal.TryParse(tribFed, out decTribFederais);

        var retorno = new NFC()
        {
            NomeEmitente = nomeEmitente!,
            CNPJEmitente = cnpjEmitente,
            EnderecoEmitente = enderecoEmitente,
            TipoEmissao = tipoEmissao,
            Numero = intNumero,
            Serie = serie,
            DataEmissao = dteEmissao,
            ProtocoloAutorizacao = protocoloAutorizacao,
            DataProtocoloAutorizacao = dteProtocoloAutorizacao,
            Ambiente = ambiente,
            ChaveAcesso = chaveAcesso!,
            CPFConsumidor = cpf ?? null,
            NomeConsumidor = nome,
            TributosAproximados = decTribAprox,
            TributosFederais = decTribFederais
        };

        retorno.Itens = ParseItens(html).ToList();

        return retorno;
    }

    public async Task<NFC> SalvarNFCAsync(string htmlNFC)
    {
        _dbConnection.Open();
        var conn = _dbConnection.BeginTransaction();
        try
        {
            //Transação NFC
            //1.Validação da NFC
            var nfc = ParseNFC(htmlNFC);
            var nfcs = await _nfcRepository.GetAsync();
            var nfc_banco = nfcs.Where(a => a.ChaveAcesso.Trim() == nfc.ChaveAcesso.Trim()).ToList();
            if (nfc_banco.Any())
                throw new Exception("A NFC já está cadastrada");
            if (nfc.Itens == null)
                throw new Exception("A NFC não tem itens");
            //2.Salva informações da compra(cabeçalho) e retorna ID
            var novo_id = await _nfcRepository.AddAsync(nfc, conn);
            //3.Coloca o ID nos itens e salva cada um deles
            if (nfc.Itens != null)
            {
                nfc.Itens.ForEach(a => a.IdCompra = novo_id);
                foreach(var item in nfc.Itens)
                    await _itemNFCRepository.AddAsync(item, conn);
            }
            //4.Em sucesso retorna com mensagem de aviso
            conn.Commit();
            return nfc;
        }
        catch (Exception)
        {
            //5.Em caso de algum erro a transação sofre rollback e retorna mensagem
            
            conn.Rollback();
            throw;
        }
        finally
        {
            conn.Dispose();
            _dbConnection.Close();
        }
    }

    private static string LimparTexto(string texto)
    {
        texto = HtmlEntity.DeEntitize(texto);
        texto = Regex.Replace(texto, @"\s+", " ");
        texto = Regex.Replace(texto, @"\s*,\s*", ", ");
        return texto.Trim();
    }

    private static string SomenteNumeros(string texto) => Regex.Replace(texto, @"\D", "");
}
