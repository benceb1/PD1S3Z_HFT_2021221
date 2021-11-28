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
            LendingLogic.StartLending(body.borrowerId, body.bookIds, body.libraryId, body.lendingWeeks);
        }

        [HttpPost("end")]
        public void EndLending([FromBody] EndLendingRequest body)
        {
            LendingLogic.EndLending(body.lendingId, body.libraryId);
        }

        [HttpGet]
        public IEnumerable<Lending> GetLendings()
        {
            return LendingLogic.GetAllLendings();
        } 
    }

    public class NewLendingRequest
    {
        public int borrowerId { get; set; }
        public int libraryId { get; set; }
        public int lendingWeeks { get; set; }
        public int[] bookIds { get; set; }
    }

    public class EndLendingRequest
    {
        public int lendingId { get; set; }
        public int libraryId { get; set; }
    }
}
