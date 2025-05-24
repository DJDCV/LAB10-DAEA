using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Lab10.Infrastructure.Context;

namespace Lab10.Infrastructure.Persistence;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly Lab10DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(Lab10DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // Método específico para obtener un usuario por su username
    public async Task<user?> GetByUsernameAsync(string username)
    {
        // Solo se debe usar OfType<user>() si T es un tipo más general.
        return await _context.Set<user>().FirstOrDefaultAsync(u => u.username == username);
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}