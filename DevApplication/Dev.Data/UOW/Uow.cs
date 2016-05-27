using System;
using Dev.BO.Models;
using Dev.Data.DBContext;
using Dev.Data.Repository;
using System.Data.Entity.Validation;

namespace Dev.Data.UOW
{
    /// <summary>
    /// The "Unit of Work"
    ///     1) decouples the repos from the console,controllers,ASP.NET pages....
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a applicant.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data .
    /// </remarks>
    public class UnitofWork : IUnitOfWork, IDisposable
    {


        private DevContext DbContext { get; set; }


        public UnitofWork()
        {
            CreateDbContext();


        }

        private IRepository<DevTest> _devtestrepository;

        //get Skills repo
        public IRepository<DevTest> DevTestRepository
        {
            get {
                return _devtestrepository ??
                       (_devtestrepository = new Repository<DevTest>(DbContext));
            }
        }
        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.ToString();
                        

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            
        }


        protected void CreateDbContext()
        {
            DbContext = new DevContext();
           
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion





    }


}