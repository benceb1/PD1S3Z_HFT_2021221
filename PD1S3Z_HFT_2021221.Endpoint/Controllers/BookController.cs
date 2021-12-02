using Microsoft.AspNetCore.Mvc;
using PD1S3Z_HFT_2021221.Logic;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Data
{
    [Route("[controller]")] //    /book
    [ApiController]
    public class BookController : ControllerBase
    {
        IBookLogic BookLogic;

        public BookController(IBookLogic bookLogic)
        {
            BookLogic = bookLogic;
        }

        [HttpPost]
        public Book Insert([FromBody]Book book)
        {
            return BookLogic.Insert(book);
        }

        [HttpGet] 
        public IEnumerable<Book> GetAll()
        {
            return BookLogic.GetAllBooks();
        }

        [HttpGet("{bookId}")]
        public Book GetBookById([FromRoute] int bookId)
        {
            return BookLogic.GetOneById(bookId);
        }

        [HttpGet("library/{libId}")]
        public IEnumerable<Book> GetBooksByLibId([FromRoute] int libId)
        {
            return BookLogic.GetBooksByLibraryId(libId);
        }

        [HttpGet("avgpages")]
        public IEnumerable<AvgPagesResult> GetAvgPagesResults()
        {
            return BookLogic.AvgOfPageNumbersInLibraries();
        }
    }
}
