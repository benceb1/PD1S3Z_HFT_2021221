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
    public class LendingRepository : Repository<Lending>, ILendingRepository
    {
        public LendingRepository(DbContext ctx) : base(ctx) {}
        public override Lending GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Lending Insert(Lending entity)
        {
            ctx.Set<Lending>().Add(entity);
            ctx.SaveChanges();
            return entity;
        }

        public override bool Remove(int id)
        {
            Lending bookLending = GetOne(id);
            try
            {
                ctx.Set<Lending>().Remove(bookLending);
                ctx.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void SetActiveStatus(int id, bool isActive)
        {
            Lending bookLending = GetOne(id);
            if (bookLending == null) throw new InvalidOperationException("BookLending not found!");
            bookLending.Active = isActive;
            if (bookLending.EndDate < DateTime.Now)
            {
                bookLending.Late = true;
            }
            ctx.SaveChanges();
        }

        public void Update(int id, Lending newBookLending)
        {
            Lending bookLending = GetOne(id);
            if (bookLending == null) throw new InvalidOperationException("BookLending not found!");
            bookLending = newBookLending;
            ctx.SaveChanges(); 
        }

        public override void Update(Lending entity)
        {
            var old = GetOne(entity.Id);
            foreach (var prop in old.GetType().GetProperties())
            {
                prop.SetValue(old, prop.GetValue(entity));
            }
            ctx.SaveChanges();
        }
    }
}
