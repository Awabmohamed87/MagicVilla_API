
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

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, string? includeProperties = null,
            int PageSize = 0, int PageNumber = 1)
        {

            IQueryable<T> query = _dbSet;
            if (expression != null)
                query = query.Where(expression);
            if (PageSize > 0)
            {
                if (PageSize > 100)
                    PageSize = 100;

                query = query.Skip(PageSize * (PageNumber - 1)).Take(PageSize);
            }
          
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression = null, bool isTracking = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;


            if (!isTracking)
                query = query.AsNoTracking();

            if (expression != null)
                query = query.Where(expression);

            if(includeProperties != null)
            {
                foreach(var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

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
