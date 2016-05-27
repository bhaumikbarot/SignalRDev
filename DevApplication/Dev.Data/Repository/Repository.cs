using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Dev.BO.Models;
using System.Data.Entity.Validation;

namespace Dev.Data.Repository
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(DbContext dbContext)
        {

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }


        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public virtual IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public virtual T GetById(int id)
        {

            return DbSet.Find(id);
        }

        public virtual T GetById(long id)
        {

            return DbSet.Find(id);
        }

        public virtual T GetById(Guid id)
        {

            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            //DbContext.Configuration.ValidateOnSaveEnabled = false;
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
            //  DbContext.Configuration.AutoDetectChangesEnabled = true;
            //DbContext.Configuration.ValidateOnSaveEnabled = true;

        }

        public virtual void Update(T entity)
        {
            // DbContext.Configuration.AutoDetectChangesEnabled = false;
            //DbContext.Configuration.ValidateOnSaveEnabled = false;
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                if (dbEntityEntry.State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }
                dbEntityEntry.State = EntityState.Modified;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            
            //  DbContext.Configuration.AutoDetectChangesEnabled = true;
            //  DbContext.Configuration.ValidateOnSaveEnabled = true;

        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        public virtual bool Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity == null) return false; // not found; assume already deleted.
            Delete(entity);
            return true;
        }

        public IList<T> GetList()
        {

            return DbSet.ToList();
        }

        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {

            return DbSet.Where(predicate);
        }
        public object ExecWithStoreProcedure(string query, params object[] parameters)
        {

            //      IEnumerable<RdTicketCustomer> Customers =
            //       _unitOfWork.TicketCustomerRepository.ExecWithStoreProcedure(
            //       "spGetCustomers @customerid",
            //       new SqlParameter("customerid", SqlDbType.BigInt) { Value = 1 }
            //);
            return DbContext.Database.SqlQuery<T>(query, parameters);
        }
        public IEnumerable<T> GetRecordsByPage<TKey>(int pageNumber, int pageSize, Expression<Func<T, TKey>> orderBy)
        {
            //IEnumerable<RdTicketCustomer> usr = _unitOfWork.TicketCustomerRepository.GetRecordsByPage(2, 1, user => user.rd_ticket_customer_id);

            return DbSet.AsQueryable().OrderBy(orderBy).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
    }
}
