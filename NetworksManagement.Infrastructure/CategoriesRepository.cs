using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksManagement.Infrastructure
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }

        public bool Any(int categoryId)
        {
            return _context.Categories.Any(l => l.Id == categoryId);
        }

        public IQueryable<Category> GetAll()
        {
            var categories = _context.Categories.OrderBy(c => c.Name);
            return categories;
        }

        public async Task<Category> GetAsync(int? categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(l => l.Id == categoryId);
            return category;
        }

        public async Task RemoveAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
