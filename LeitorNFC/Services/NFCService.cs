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
                ValorTotal = decimal.Parse(vlTotal!)
            });
        }

        return itens;
    }
}
