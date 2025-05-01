using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N_Tier_Architecture_DataAccess.Data;
using N_Tier_Architecture_DataAccess.Repository.IRepository;

namespace N_Tier_Architecture_DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly ApplicationDBContext _dbContext;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public UnitofWork(ApplicationDBContext context)
        {
            this._dbContext = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
