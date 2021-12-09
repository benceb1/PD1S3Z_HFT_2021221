using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public interface IBorrowerLogic
    {
        public Borrower Insert(Borrower borrower);

        public bool DeleteBorrower(int borrowerId);

        public IList<Borrower> GetBorrowers();

        public IList<Borrower> GetLateBorrowers();

        public void ModifyName(int id, string newName);
    }
}
