
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository <R> where R : class
    {
        Task<List<R>> GetAllAsync(Expression<Func<R, bool>>? expression = null);
        Task<R> GetFirstOrDefaultAsync(Expression<Func<R, bool>>? expression = null, bool isTracking = true);
        Task CreateAsync(R villa);
        
        Task Remove(R villa);
        Task SaveAsync();
    }
}
