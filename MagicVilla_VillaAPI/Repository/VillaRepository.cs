using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository :  Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        

        public async Task<Villa> Update(Villa villa)
        {
            villa.UpdatedDate = DateTime.Now;   
           _db.Villas_Api.Update(villa);
            await SaveAsync();
            return villa;
        }
    }
}
