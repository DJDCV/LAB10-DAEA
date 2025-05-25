using Lab10.Domain.Interfaces;
using Lab10.Infrastructure.Context;

namespace Lab10.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lab10DbContext _context;
    private bool _disposed;
    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(Lab10DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }
    
    public IRepository<T> Repository<T>() where T : class
    {
        if (_repositories.ContainsKey(typeof(T)))
        {
            return (IRepository<T>)_repositories[typeof(T)];
        }

        var repository = new Repository<T>(_context);
        _repositories[typeof(T)] = repository; 
        return repository;
    }
    
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}