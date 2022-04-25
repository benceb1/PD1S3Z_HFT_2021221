using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PD1S3Z_HFT_2021221.Endpoint.Services;
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
        IHubContext<SignalRHub> hub;

        public BookController(IBookLogic bookLogic, IHubContext<SignalRHub> hub)
        {
            BookLogic = bookLogic;
            this.hub = hub;
        }

        [HttpPost]
        public void Insert([FromBody]Book book)
        {
            BookLogic.Insert(book);
            this.hub.Clients.All.SendAsync("BookCreated", book);

        }

        [HttpGet] 
        public IEnumerable<Book> GetAll()
        {
            return BookLogic.GetAllBooks();
        }

        [HttpPut]
        public void Put([FromBody] Book value)
        {
            this.BookLogic.Update(value);
            this.hub.Clients.All.SendAsync("BookUpdated", value);

        }

        [HttpGet("{bookId}")]
        public Book GetBookById([FromRoute] int bookId)
        {
            return BookLogic.GetOneById(bookId);
        }

        [HttpDelete("{id}")]
        public void DeleteBook([FromRoute] int id)
        {
            var bookToDelete = this.BookLogic.GetOneById(id);
            this.BookLogic.Delete(id);
            this.hub.Clients.All.SendAsync("BookDeleted", bookToDelete);

        }

        [HttpPut("{id}")]
        public void ModifyLibrary([FromRoute]int id, [FromBody]ModifyLibRequest req)
        {
            BookLogic.ModifyLibrary(id, req.newLibId);
        }

        [HttpGet("library/{libId}")]
        public IEnumerable<Book> GetBooksByLibId([FromRoute] int libId)
        {
            return BookLogic.GetBooksByLibraryId(libId);
        }

        [HttpGet("avgPages")]
        public IEnumerable<AvgPagesResult> GetAvgPagesResults()
        {
            return BookLogic.AvgOfPageNumbersInLibraries();
        }
    }
}
