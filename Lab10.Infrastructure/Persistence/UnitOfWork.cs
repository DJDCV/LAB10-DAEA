using Lab10.Domain.Interfaces;
using Lab10.Infrastructure.Context;

namespace Lab10.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly Lab10DbContext _context;
    private bool _disposed;
    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(Lab10DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    // Obtener repositorio de un tipo específico
    public IRepository<T> Repository<T>() where T : class
    {
        if (_repositories.ContainsKey(typeof(T)))
        {
            return (IRepository<T>)_repositories[typeof(T)];
        }

        var repository = new Repository<T>(_context);
        _repositories[typeof(T)] = repository; // Guardar el repositorio para futuras solicitudes
        return repository;
    }

    // Guardar cambios en la base de datos
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Liberar recursos
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