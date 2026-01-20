
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
        var novo_id = _context.DbConnection.QuerySingle<int>("SELECT nextval('nfc.nfc_compra_id_seq')");
        var query = "INSERT INTO nfc.nfc_compra(id, data_emissao, chave_acesso, nome_comercio, cpf_consumidor) VALUES(@Id, @Data, @ChaveAcesso, @NomeComercio, @CPF)";
        var parametros = new
        {
            id = novo_id,
            Data = entity.DataEmissao,
            ChaveAcesso = entity.ChaveAcesso,
            NomeComercio = entity.NomeEmitente,
            CPF = entity.CPFConsumidor
        };
        await _context.DbConnection.ExecuteAsync(query, parametros);
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<NFC>> GetAllAsync(Func<NFC, bool>? predicate = null)
    {
        string query = "SELECT * FROM nfc.nfc_compra";
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
