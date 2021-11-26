using Microsoft.EntityFrameworkCore;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public class LibraryRepository : Repository<Library>, ILibraryRepository
    {
        public LibraryRepository(DbContext ctx) : base(ctx) {}

        public void AddBookToLibrary(int libraryId, Book book)
        {
            Library library = GetOne(libraryId);
            library.Books.Add(book);
            ctx.SaveChanges();
        }

        public void DeleteBookFromLibrary(int libraryId, Book book)
        {
            Library library = GetOne(libraryId);
            library.Books.Remove(book);
            ctx.SaveChanges();
        }

        public override Library GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Library Insert(Library entity)
        {
            ctx.Set<Library>().Add(entity);
            ctx.SaveChanges();
            return entity;
        }

        public override bool Remove(int id)
        {
            Library lib = GetOne(id);
            try
            {
                ctx.Set<Library>().Remove(lib);
                ctx.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Update(int id, Library library)
        {
            Library bookLending = GetOne(id);
            if (bookLending == null) throw new InvalidOperationException("Library not found!");
            bookLending = library;
            ctx.SaveChanges();
        }
    }
}
