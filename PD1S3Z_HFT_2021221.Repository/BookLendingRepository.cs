using Microsoft.EntityFrameworkCore;
using PD1S3Z_HFT_2021221.Data;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public class BookLendingRepository : Repository<BookLending>, IBookLendingRepository
    {
        public BookLendingRepository(DbContext ctx) : base(ctx) {}
        public override BookLending GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override void Insert(BookLending entity)
        {
            ctx.Set<BookLending>().Add(entity);
            ctx.SaveChanges();
        }

        public override bool Remove(int id)
        {
            BookLending bookLending = GetOne(id);
            try
            {
                ctx.Set<BookLending>().Remove(bookLending);
                ctx.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Update(int id, BookLending newBookLending)
        {
            BookLending bookLending = GetOne(id);
            if (bookLending == null) throw new InvalidOperationException("BookLending not found!");
            bookLending = newBookLending;
            ctx.SaveChanges(); 
        }
    }
}
