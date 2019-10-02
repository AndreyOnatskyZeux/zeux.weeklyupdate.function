using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeeklyNotification.App.DAL.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task BulkInsert(IEnumerable<TEntity> entities);
    }
}