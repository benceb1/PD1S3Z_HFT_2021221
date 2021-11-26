using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public interface ILibraryRepository : IRepository<Library>
    {
        void Update(int id, Library library);
        void DeleteBookFromLibrary(int libraryId, Book book);
        void AddBookToLibrary(int libraryId, Book book);
    }
}
