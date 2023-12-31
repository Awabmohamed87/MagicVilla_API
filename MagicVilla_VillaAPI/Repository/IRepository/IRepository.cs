
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository <R> where R : class
    {
        Task<List<R>> GetAllAsync(Expression<Func<R, bool>>? expression = null, string? includeProperties = null,
            int PageSize = 3, int PageNumber = 1);
        Task<R> GetFirstOrDefaultAsync(Expression<Func<R, bool>>? expression = null, bool isTracking = true, string? includeProperties = null);
        Task CreateAsync(R villa);
        
        Task Remove(R villa);
        Task SaveAsync();
    }
}
