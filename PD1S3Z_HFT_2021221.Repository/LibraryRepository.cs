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
        public override Library GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override void Insert(Library entity)
        {
            ctx.Set<Library>().Add(entity);
            ctx.SaveChanges();
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
