namespace LeitorNFC;

public class ItemNFCRepository : IRepository<ItemNFC>
{
    private readonly DapperDbContext _context;

    public ItemNFCRepository(DapperDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ItemNFC entity)
    {
        var novo_id = _context.DbConnection.QuerySingle<int>("SELECT nextval('nfc.nfc_item_id_seq')");
        var query = "INSERT INTO nfc.nfc_item(id, codigo, descricao, quantidade, unidade, valor_unitario, valor_total, id_compra) VALUES(@id, @codigo, @descricao, @quantidade, @unidade, @valor_unitario, @valor_total, @id_compra)";
        var parametros = new
        {
            id = novo_id,
            codigo = entity.Codigo,
            descricao = entity.Descricao,
            quantidade = entity.Quantidade,
            unidade = entity.Unidade,
            valor_unitario = entity.ValorUnitario,
            valor_total = entity.ValorTotal,
            id_compra = entity.IdCompra
        };
        await _context.DbConnection.ExecuteAsync(query, parametros);
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ItemNFC>> GetAllAsync(Func<ItemNFC, bool>? predicate = null)
    {
        string query = "SELECT * FROM nfc.nfc_item";
        return await _context.DbConnection.QueryAsync<ItemNFC>(query);
    }

    public ItemNFC GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(ItemNFC entity)
    {
        throw new NotImplementedException();
    }
}
