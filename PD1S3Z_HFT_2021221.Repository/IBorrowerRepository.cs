using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public interface IBorrowerRepository : IRepository<Borrower>
    {
        void Update(int id, Borrower borrower);
        void IncrementLateLendingNumber(int borrowerId);
        void IncrementBooksRead(int borrowerId);
    }
}
