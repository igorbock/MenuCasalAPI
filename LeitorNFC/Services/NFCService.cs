using System.Text.RegularExpressions;

namespace LeitorNFC.Services;

public static class NFCService
{
    public static List<ItemNFC> ParseItens(string html)
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

    public static NFC ParseNFC(string html)
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
        var cnpjEmitente = SomenteNumeros(LimparTexto(htmlConteudo.DocumentNode.SelectSingleNode(@"//div[1]/div[2]").InnerText));
        var enderecoEmitente = LimparTexto(htmlConteudo.DocumentNode.SelectSingleNode(@"//div[1]/div[3]").InnerText);
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
        var cpf = SomenteNumeros(infoGerais.DocumentNode.SelectSingleNode(@"//div[3]/ul/li/text()[1]").InnerText);
        var nome = infoGerais.DocumentNode.SelectSingleNode(@"div[3]/ul/li[2]/strong").InnerText.Split(":")[1].Trim();
        // Informações de interesse do contribuinte
        var infoContribuinteRaw = infoGerais.DocumentNode.SelectSingleNode(@"//div[4]/ul/li").InnerText.Split(" ");
        var tribAprox = infoContribuinteRaw[3];
        var tribFed = infoContribuinteRaw[5];

        var retorno = new NFC()
        {
            NomeEmitente = nomeEmitente!,
            CNPJEmitente = cnpjEmitente,
            EnderecoEmitente = enderecoEmitente,
            TipoEmissao = tipoEmissao,
            Numero = int.Parse(numero),
            Serie = serie,
            DataEmissao = DateTime.Parse(dataEmissao!),
            ProtocoloAutorizacao = protocoloAutorizacao,
            DataProtocoloAutorizacao = DateTime.Parse(dataProtocoloAutorizacao),
            Ambiente = ambiente,
            ChaveAcesso = chaveAcesso!,
            CPFConsumidor = cpf ?? null,
            NomeConsumidor = nome,
            TributosAproximados = decimal.Parse(tribAprox),
            TributosFederais = decimal.Parse(tribFed)
        };

        retorno.Itens = ParseItens(html);

        return retorno;
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
