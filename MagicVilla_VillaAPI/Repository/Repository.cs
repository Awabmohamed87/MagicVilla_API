
using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {
            IQueryable<T> query = _dbSet;
            if (expression == null)
                return await query.ToListAsync();
            else
                return await query.Where(expression).ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression = null, bool isTracking = true)
        {
            IQueryable<T> query = _dbSet;


            if (!isTracking)
                query = query.AsNoTracking();

            if (expression != null)
                query = query.Where(expression);

            return await query.FirstOrDefaultAsync();
        }

        public async Task Remove(T obj)
        {
            _dbSet.Remove(obj);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
