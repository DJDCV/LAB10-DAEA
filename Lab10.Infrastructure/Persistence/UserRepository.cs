using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public IEnumerable<User> GetAll() => _users;

        public User GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public void Add(User user) => _users.Add(user);

        public void Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing != null)
            {
                existing.Username = user.Username;
            }
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }
    }
}