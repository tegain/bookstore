using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using BookStore_API.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IList<Author>> FindAll()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> FindById(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<bool> Create(Author entity)
        {
            await _context.Authors.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Update(Author entity)
        {
            _context.Authors.Update(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            _context.Authors.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Authors.AnyAsync(author => author.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}