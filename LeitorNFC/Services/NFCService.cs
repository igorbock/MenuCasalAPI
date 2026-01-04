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

        var infos = doc.DocumentNode.SelectNodes("//*[@id='infos']");
        var div1 = infos[0].SelectSingleNode("//div[1]");
        var dataEmissao = div1.SelectSingleNode("//div//ul//li//text()[3]")?.InnerText.Trim().Split("-", 2)[0].Trim();
        var chaveAcesso = infos[0].SelectNodes("//div[2]//div/ul/li/span")[0]?.InnerText;
        var cpfConsumidor = infos[0].SelectSingleNode("//div[3]/div/ul/li[1]/text()")?.InnerText;
        var nomeComercio = doc.DocumentNode.SelectSingleNode("//*[@id=\"u20\"]")?.InnerText;

        var retorno = new NFC()
        {
            DataEmissao = DateTime.Parse(dataEmissao!),
            ChaveAcesso = chaveAcesso!,
            NomeComercio = nomeComercio!,
            CPFConsumidor = cpfConsumidor
        };

        retorno.Itens = ParseItens(html);

        return retorno;
    }
}
