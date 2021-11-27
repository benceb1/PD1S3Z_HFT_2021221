using PD1S3Z_HFT_2021221.Models;
using PD1S3Z_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public class BookLogic : IBookLogic
    {
        private IBookRepository BookRepository;

        public BookLogic(IBookRepository bookRepository)
        {
            BookRepository = bookRepository;
        }

        public IList<Book> GetAllBooks()
        {
            return BookRepository.GetAll().ToList();
        }

        public IList<Book> GetBooksByBorrowerId(int borrowerId)
        {
            return BookRepository.GetAll().Where(x => x.BorrowerId != null && x.BorrowerId == borrowerId).ToList();
        }

        public IList<Book> GetBooksByLibraryId(int libraryId)
        {
            return BookRepository.GetAll().Where(x => x.LibraryId != null && x.LibraryId == libraryId).ToList();

        }
    }
}
