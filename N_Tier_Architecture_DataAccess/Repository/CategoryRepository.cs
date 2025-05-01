using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using N_Tier_Architecture_DataAccess.Data;
using N_Tier_Architecture_DataAccess.Repository.IRepository;
using N_Tier_Architecture_Models;

namespace N_Tier_Architecture_DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _dBContext;
        public CategoryRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
            _dBContext = dBContext; 
        }

        public void Update(Category category)
        {
            _dBContext.Categories.Update(category);
        }

    }
}
