namespace LeitorNFC.Interfaces;

public interface IRepository<TypeT> where TypeT : IEntity
{
    public Task<TypeT> GetAsync(long id);
    public Task<IEnumerable<TypeT>> GetAsync();
    public Task<long> AddAsync(TypeT entity);
    public Task UpdateAsync(TypeT entity);
    public Task DeleteAsync(long id);
}
