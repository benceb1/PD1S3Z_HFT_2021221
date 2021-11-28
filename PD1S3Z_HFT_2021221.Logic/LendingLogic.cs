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
        ILibraryRepository LibraryRepository;
        ILendingRepository LendingRepository;
        IBorrowerRepository BorrowerRepository;

        public LendingLogic(ILibraryRepository libraryRepository, ILendingRepository lendingRepository, IBorrowerRepository borrowerRepository)
        {
            LibraryRepository = libraryRepository;
            LendingRepository = lendingRepository;
            BorrowerRepository = borrowerRepository;
        }

        public Lending EndLending(int lendingId, int libraryId)
        {
            Library library = LibraryRepository.GetOne(libraryId);
            Lending lending = LendingRepository.GetOne(lendingId);

            int totalBooks = library.Books.Count + lending.Books.Count;

            if (totalBooks > library.BookCapacity)
            {
                throw new InvalidOperationException("Library capacity is too small");
            }

            foreach (var book in lending.Books)
            {
                LibraryRepository.AddBookToLibrary(libraryId, book);
                BorrowerRepository.DeleteBookFromBorrower(lending.BorrowerId, book);
            }

            if (lending.EndDate < DateTime.Now)
            {
                BorrowerRepository.IncrementLateLendingNumber(lending.BorrowerId);
            }

            LendingRepository.SetActiveStatus(lending.Id, false);

            return lending;
        }

        public IList<Lending> GetActiveLendings()
        {
            return LendingRepository.GetAll().Where(x => x.Active).ToList();
        }

        public IList<Lending> GetAllLendings()
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

        public Lending StartLending(int borrowerId, int[] bookIds, int libraryId, int lendingWeeks)
        {
            Library library = LibraryRepository.GetOne(libraryId);
            List<Book> books = new List<Book>();
            foreach (var bookId in bookIds)
            {
                Book bookFromLibrary = library.Books.Where(b => bookId == b.Id).FirstOrDefault();
                if (bookFromLibrary == null)
                {
                    throw new InvalidOperationException("Cannot find book in library");
                }

                LibraryRepository.DeleteBookFromLibrary(library.Id, bookFromLibrary);
                BorrowerRepository.AddBookToBorrower(borrowerId, bookFromLibrary);
                books.Add(bookFromLibrary);
            }
            Lending lending = new Lending()
            {
                Active = true,
                Books = books,
                BorrowerId = borrowerId,
                LibraryId = libraryId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7 * lendingWeeks)
            };
            return LendingRepository.Insert(lending);
        }
    }
}
