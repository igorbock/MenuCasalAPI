namespace LeitorNFC.Repository;

public class RepositoryGenerico<TEntity> : DapperRepositoryAbstract, IRepository<TEntity> where TEntity : class, IEntity, new()
{
    private static readonly string InsertSql = SqlBuilderHelper.BuildInsert<TEntity>();
    private static readonly string UpdateSql = SqlBuilderHelper.BuildUpdate<TEntity>();
    private static readonly string DeleteSql = SqlBuilderHelper.BuildDelete<TEntity>();
    private static readonly string GetByIdSql = SqlBuilderHelper.BuildSelectById<TEntity>();
    private static readonly string GetAllSql = SqlBuilderHelper.BuildSelectAll<TEntity>();
    private static readonly string SequenceSql = SqlBuilderHelper.BuildSequence<TEntity>();

    public RepositoryGenerico(IDbConnection connection) : base(connection) { }

    public async Task<long> AddAsync(TEntity entity, IDbTransaction? transaction = null)
    {
        entity.Id = await QuerySingleAsync<long>(SequenceSql, transaction: transaction);
        await ExecuteAsync(InsertSql, entity);
        return entity.Id;
    }
    public Task UpdateAsync(TEntity entity, IDbTransaction? transaction = null) => ExecuteAsync(UpdateSql, entity, transaction);
    public Task DeleteAsync(long id, IDbTransaction? transaction = null) => ExecuteAsync(DeleteSql, new { id }, transaction);
    public Task<TEntity> GetAsync(long id) => QuerySingleAsync<TEntity>(GetByIdSql, new { id });
    public Task<IEnumerable<TEntity>> GetAsync() => QueryAsync<TEntity>(GetAllSql);
}
