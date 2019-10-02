using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using WeeklyNotification.App.DAL.Contracts;

namespace WeeklyNotification.App.DAL.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        private readonly ZeuxDbContext _context;


        public Repository(ZeuxDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public Task BulkInsert(IEnumerable<TEntity> entities)
        {
            return _context.BulkInsertAsync(entities.ToArray());
        }
    }
}