namespace LeitorNFC.Abstract;

public abstract class DapperRepositoryAbstract
{
    protected readonly IDbConnection Connection;

    protected DapperRepositoryAbstract(IDbConnection connection)
    {
        this.Connection = connection;
    }

    protected async Task ExecuteAsync(string sql, object? param = null) => await Connection.ExecuteAsync(sql, param);
    protected async Task ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null) => await Connection.ExecuteAsync(sql, param, transaction);
    protected async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null) where T : new() => (await Connection.QueryAsync<T>(sql, param, transaction)).FirstOrDefault(new T());
    protected async Task<dynamic> QuerySingleAsync(string sql, object? param = null, IDbTransaction? transaction = null) => (await Connection.QueryAsync(sql, param, transaction)).FirstOrDefault(new object());
    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null) => (await Connection.QueryAsync<T>(sql, param, transaction)).ToList();
    protected async Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null, IDbTransaction? transaction = null) => (await Connection.QueryAsync(sql, param, transaction)).ToList();
}
