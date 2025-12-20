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
        string query = "INSERT INTO public.\"ItemNFC\"(\"Id\", \"Codigo\", \"Descricao\", \"Quantidade\", \"Unidade\", \"ValorUnitario\", \"ValorTotal\", \"IdCompra\") VALUES(@Id, @Codigo, @Descricao, @Quantidade, @Unidade, @ValorUnitario, @ValorTotal, @IdCompra)";
        await _context.DbConnection.ExecuteAsync(query, entity);
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ItemNFC>> GetAllAsync(Func<ItemNFC, bool>? predicate = null)
    {
        string query = "SELECT * FROM public.\"ItemNFC\"";
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
