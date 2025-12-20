
namespace LeitorNFC;

public class CompraNFCRepository : IRepository<NFC>
{
    private readonly DapperDbContext _context;

    public CompraNFCRepository(DapperDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NFC entity)
    {
        string query = "INSERT INTO public.\"NFC\"(\"Id\", \"Data\", \"ValorTotal\", \"CNPJ\", \"RazaoSocial\") VALUES(@Id, @Data, @ValorTotal, @CNPJ, @RazaoSocial)";
        await _context.DbConnection.ExecuteAsync(query, entity);
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<NFC>> GetAllAsync(Func<NFC, bool>? predicate = null)
    {
        string query = "SELECT * FROM public.\"NFC\"";
        return await _context.DbConnection.QueryAsync<NFC>(query);
    }

    public NFC GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(NFC entity)
    {
        throw new NotImplementedException();
    }
}
