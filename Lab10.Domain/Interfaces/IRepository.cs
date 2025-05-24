using Lab10.Domain.Entities;

namespace Lab10.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(object id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<user?> GetByUsernameAsync(string username);
}