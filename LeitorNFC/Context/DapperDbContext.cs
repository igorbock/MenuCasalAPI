namespace LeitorNFC;

public class DapperDbContext
{
    private readonly IDbConnection _dbConnection;
    public IDbConnection DbConnection => _dbConnection;

    public DapperDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _dbConnection = new NpgsqlConnection(connectionString);
    }
}
