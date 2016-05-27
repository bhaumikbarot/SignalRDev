using Dev.BO.Models;
using Dev.Data.Repository;

namespace Dev.Data.UOW
{
    /// <summary>
    /// Interface for the  Unit of Work"
    /// </summary>
    public interface IUnitOfWork
    {
        // Save pending changes to the data store.
        void Commit();

        // Repositories
        IRepository<DevTest> DevTestRepository { get; }
    }

}