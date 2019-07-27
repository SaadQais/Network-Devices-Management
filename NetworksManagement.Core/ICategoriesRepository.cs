using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Core
{
    public interface ICategoriesRepository
    {
        public IQueryable<Category> GetAll();
        public Task AddAsync(Category category);
        public Task<Category> GetAsync(int? categoryId);
        public Task UpdateAsync(Category category);
        public Task RemoveAsync(Category category);
        public bool Any(int categoryId);
    }
}
