using Microsoft.EntityFrameworkCore;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public class BorrowerRepository : Repository<Borrower>, IBorrowerRepository
    {
        public BorrowerRepository(DbContext ctx) : base(ctx) { }

        public override Borrower GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public void IncrementLateLendingNumber(int borrowerId)
        {
            Borrower borrower = GetOne(borrowerId);
            if (borrower == null) throw new InvalidOperationException("Borrower not found!");
            borrower.NumberOfLateLendings++;
            ctx.SaveChanges();
        }

        public void IncrementBooksRead(int borrowerId)
        {
            Borrower borrower = GetOne(borrowerId);
            if (borrower == null) throw new InvalidOperationException("Borrower not found!");
            borrower.NumberOfLateLendings++;

            MembershipLevel level = Helper.GetLevelByBookNumber(borrower.NumberOfLateLendings);
            if (level != borrower.MembershipLevel)
            {
                borrower.MembershipLevel = level;
            }

            ctx.SaveChanges();
        }

        public override Borrower Insert(Borrower entity)
        {
            ctx.Set<Borrower>().Add(entity);
            ctx.SaveChanges();
            return entity;
        }

        public override bool Remove(int id)
        {
            Borrower borrower = GetOne(id);
            try
            {
                if (borrower == null) throw new InvalidOperationException("Borrower not found!");
                ctx.Set<Borrower>().Remove(borrower);
                ctx.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Update(int id, Borrower newBorrower)
        {
            Borrower borrower = GetOne(id);
            if (borrower == null) throw new InvalidOperationException("Borrower not found!");
            borrower = newBorrower;
            ctx.SaveChanges();
        }

        public void ModifyName(int id, string newName)
        {
            Borrower borrower = GetOne(id);
            borrower.Name = newName;
            ctx.SaveChanges();
        }
    }
}
