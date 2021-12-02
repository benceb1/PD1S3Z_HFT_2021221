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
        private ILibraryRepository LibraryRepository;

        public BookLogic(IBookRepository bookRepository, ILibraryRepository libraryRepository)
        {
            BookRepository = bookRepository;
            LibraryRepository = libraryRepository;
        }

        public IList<Book> GetAllBooks()
        {
            return BookRepository.GetAll().ToList();
        }

        public IList<Book> GetBooksByLibraryId(int libraryId)
        {
            return BookRepository.GetAll().Where(x => x.LibraryId == libraryId).ToList();
        }

        public Book GetOneById(int Id)
        {
            return BookRepository.GetOne(Id);
        }

        public Book Insert(Book book)
        {
            return BookRepository.Insert(book);
        }

        public IList<AvgPagesResult> AvgOfPageNumbersInLibraries()
        {
            var result = from book in BookRepository.GetAll()
                         group book by new { book.Library.Id, book.Library.Name } into grp
                         select new AvgPagesResult()
                         {
                             LibraryName = grp.Key.Name,
                             AvgPages = grp.Average(b => b.NumberOfPages)
                         };
            return result.ToList();
        }

        public IList<AvgPagesResult> AvgOfPageNumbersInLibrariesJoin()
        {
            var q = from book in BookRepository.GetAll()
                    join library in LibraryRepository.GetAll() on book.LibraryId equals library.Id
                    let item = new { LibraryName = library.Name, Pages = book.NumberOfPages }
                    group item by item.LibraryName into grp
                    select new AvgPagesResult()
                    {
                        LibraryName = grp.Key,
                        AvgPages = grp.Average(item => item.Pages)
                    };
            return q.ToList();
        }
    }
}
