using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public interface IBookLogic
    {
        IList<Book> GetAllBooks();
        IList<Book> GetBooksByLibraryId(int libraryId);
        Book GetOneById(int Id);
        Book Insert(Book book);
    }
}
