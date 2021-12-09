using Microsoft.EntityFrameworkCore;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DbContext ctx) : base(ctx)
        {

        }
        public override Book GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Book Insert(Book entity)
        {
            ctx.Set<Book>().Add(entity);
            ctx.SaveChanges();
            return entity;
        }

        public void ModifyLibrary(int id, int newLibId)
        {
            Book book = GetOne(id);
            book.LibraryId = newLibId;
            ctx.SaveChanges();
        }

        public override bool Remove(int id)
        {
            Book book = GetOne(id);
            ctx.Set<Book>().Remove(book);
            ctx.SaveChanges();
            return true;
        }

        public void Update(int id, Book newBook)
        {
            Book book = GetOne(id);
            if (book == null) throw new InvalidOperationException("Book not found!");
            book = newBook;
            ctx.SaveChanges();
        }
    }
}
