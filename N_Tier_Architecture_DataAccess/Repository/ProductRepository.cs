using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using N_Tier_Architecture_DataAccess.Data;
using N_Tier_Architecture_DataAccess.Repository.IRepository;
using N_Tier_Architecture_Models;

namespace N_Tier_Architecture_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDBContext _dBContext;
        public ProductRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _dBContext = dBContext; 
        }

        public IEnumerable<Product> GetAllProduct()
        {
            IQueryable<Product> products = _dbset;
            //products = (IQueryable<Product>)products.Include(p => p.Category).ToList();
            _dBContext.Products.Include(p => p.Category).ToList();
            return products;
        }

        public void Update(Product product)
        {
            _dBContext.Products.Update(product);
        }

    }
}
