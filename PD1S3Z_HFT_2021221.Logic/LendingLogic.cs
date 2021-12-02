using PD1S3Z_HFT_2021221.Models;
using PD1S3Z_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public class LendingLogic : ILendingLogic
    {
        IBookRepository BookRepository;
        ILendingRepository LendingRepository;
        IBorrowerRepository BorrowerRepository;

        public LendingLogic(IBookRepository bookRepository, ILendingRepository lendingRepository, IBorrowerRepository borrowerRepository)
        {
            BookRepository = bookRepository;
            LendingRepository = lendingRepository;
            BorrowerRepository = borrowerRepository;
        }

        public Lending EndLending(int lendingId)
        {
            Lending lending = LendingRepository.GetOne(lendingId);

            if (lending == null) throw new InvalidOperationException("Lending not found!");

            if (lending.EndDate < DateTime.Now)
            {
                BorrowerRepository.IncrementLateLendingNumber(lending.BorrowerId);
            }

            BorrowerRepository.IncrementBooksRead(lending.BorrowerId);

            LendingRepository.SetActiveStatus(lending.Id, false);

            return lending;
        }

        public IList<Lending> GetActiveLendings()
        {
            return LendingRepository.GetAll().Where(x => x.Active).ToList();
        }

        public IList<Lending> GetAll()
        {
            return LendingRepository.GetAll().ToList();
        }

        public IList<Lending> GetLateLendings()
        {
            return LendingRepository.GetAll().Where(x => x.Active && x.EndDate < DateTime.Now).ToList();
        }

        public IList<Lending> GetNonActiveLendings()
        {
            return LendingRepository.GetAll().Where(x => !x.Active).ToList();
        }

        public Library GetLibraryByBookId(int bookId)
        {
            return BookRepository.GetAll().Where(b => b.Id == bookId).Select(b => b.Library).FirstOrDefault();
        }

        public Lending StartLending(int borrowerId, int bookId, int lendingWeeks)
        {
            Book book = BookRepository.GetOne(bookId);
            if (book == null) throw new InvalidOperationException("Book not found!");

            Library library = GetLibraryByBookId(bookId);
            if (library == null) throw new InvalidOperationException("Library not found!");

            Lending lending = new Lending()
            {
                Active = true,
                BookId = book.Id,
                BorrowerId = borrowerId,
                LibraryId = library.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7 * lendingWeeks)
            };
            return LendingRepository.Insert(lending);
        }
    }
}
