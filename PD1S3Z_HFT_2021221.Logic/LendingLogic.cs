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

        public void Delete(int id)
        {
            LendingRepository.Remove(id);
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

            Library library = book.Library;
            if (library == null) throw new InvalidOperationException("Library not found!");

            Lending lending = new Lending()
            {
                Active = true,
                BookId = book.Id,
                BorrowerId = borrowerId,
                StartDate = DateTime.Now,
                Late = false,
                EndDate = DateTime.Now.AddDays(7 * lendingWeeks)
            };
            return LendingRepository.Insert(lending);
        }

        public IList<Library> MostPopularLibrary()
        {
            var lendings = LendingRepository.GetAll().ToList();
            var result = from lending in lendings
                         group lending by lending.Book.Library into grp
                         let count = grp.Count()
                         orderby count descending
                         select grp.Key;
            var lib = result.ToList().FirstOrDefault();
            return new List<Library>() { lib };
        }

        public IList<Book> MostBelatedBook()
        {
            var lendings = LendingRepository.GetAll().ToList();
            var result = from lending in lendings
                         group lending by lending.Book into grp
                         let count = grp.Count(x => x.Late)
                         orderby count descending
                         select grp.Key;
            var book = result.FirstOrDefault();
            return new List<Book>() { book };
        }

        // the number of books is incremented only when the lending is over
        // they may still be active lendings
        public IList<Borrower> MostActiveBorrower()
        {
            var lendings = LendingRepository.GetAll().ToList();
            var result = from lending in lendings
                         group lending by lending.Borrower into grp
                         let count = grp.Count()
                         orderby count descending
                         select grp.Key;
            var borrower = result.FirstOrDefault();
            return new List<Borrower>() { borrower };
        }
    }
}
