using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using BookStore_API.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IList<Book>> FindAll()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> FindById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<bool> Create(Book entity)
        {
            await _context.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Update(Book entity)
        {
            _context.Books.Update(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            _context.Books.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Books.AnyAsync(book => book.Id == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}