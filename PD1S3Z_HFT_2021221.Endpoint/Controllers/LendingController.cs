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
    [Route("[controller]")] 
    [ApiController]
    public class LendingController : ControllerBase
    {
        ILendingLogic LendingLogic;

        public LendingController(ILendingLogic lendingLogic)
        {
            LendingLogic = lendingLogic;
        }

        [HttpPost]
        public void CreateLending([FromBody] NewLendingRequest body)
        {
            LendingLogic.StartLending(body.borrowerId, body.bookId, body.lendingWeeks);
        }

        [HttpPut("{lendingId}")]
        public void EndLending([FromRoute] int lendingId)
        {
            LendingLogic.EndLending(lendingId);
        }

        [HttpDelete("{id}")]
        public void DeleteLending([FromRoute] int id)
        {
            LendingLogic.Delete(id);
        }

        [HttpGet]
        public IEnumerable<Lending> GetLendings()
        {
            return LendingLogic.GetAll();
        } 

        [HttpGet("mostPopularLib")]
        public IEnumerable<Library> GetMostPopularLibrary()
        {
            return LendingLogic.MostPopularLibrary();
        }

        [HttpGet("mostBelatedBook")]
        public IEnumerable<Book> GetMostBelatedBook()
        {
            return LendingLogic.MostBelatedBook();
        }

        [HttpGet("mostActiveBorrower")]
        public IEnumerable<Borrower> MostActiveBorrower()
        {
            return LendingLogic.MostActiveBorrower();
        }
    }


}
