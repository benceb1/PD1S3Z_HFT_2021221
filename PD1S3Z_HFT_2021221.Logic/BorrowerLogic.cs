using PD1S3Z_HFT_2021221.Models;
using PD1S3Z_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public class BorrowerLogic : IBorrowerLogic
    {

        private IBorrowerRepository BorrowerRepository;
        private ILendingRepository LendingRepository;

        public BorrowerLogic(IBorrowerRepository borrowerRepository, ILendingRepository lendingRepository)
        {
            BorrowerRepository = borrowerRepository;
            LendingRepository = lendingRepository;
        }

        public bool DeleteBorrower(int borrowerId)
        {
            Borrower borrower = BorrowerRepository.GetOne(borrowerId);

            if (borrower == null) throw new InvalidOperationException("Borrower not found!");

            return BorrowerRepository.Remove(borrowerId);
        }

        public IList<Borrower> GetBorrowers()
        {
            return BorrowerRepository.GetAll().ToList();
        }

        public IList<Borrower> GetLateBorrowers()
        {
            var lendings = LendingRepository.GetAll();
            var result =  from lending in lendings
                          where lending.Active && lending.EndDate < DateTime.Now
                   select lending.Borrower;
            return result.ToList();
        }

        public Borrower Insert(Borrower borrower)
        {
            borrower.MembershipLevel = MembershipLevel.Bronze;
            borrower.NumberOfLateLendings = 0;
            borrower.StartOfMembership = DateTime.Now;
            return BorrowerRepository.Insert(borrower);
        }
    }
}
