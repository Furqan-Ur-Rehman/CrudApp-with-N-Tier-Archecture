using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using N_Tier_Architecture_DataAccess.Data;
using N_Tier_Architecture_DataAccess.Repository.IRepository;

namespace N_Tier_Architecture_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _dbContext;
        internal DbSet<T> _dbset;

        public Repository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
            _dbset = _dbContext.Set<T>();
        }
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = _dbset;
            return query.ToList();
        }

        public T GetById(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbset;
            query = query.Where(filter);
            return query.FirstOrDefault()!;
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);   
        }
    }
}
