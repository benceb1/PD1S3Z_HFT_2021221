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
    public class BorrowerController : ControllerBase
    {
        IBorrowerLogic BorrowerLogic;

        public BorrowerController(IBorrowerLogic borrowerLogic)
        {
            BorrowerLogic = borrowerLogic;
        }

        [HttpPost]
        public void AddBorrower([FromBody] Borrower borrower)
        {
            BorrowerLogic.Insert(borrower);
        }

        [HttpDelete("{borrowerId}")]
        public void DeleteOne([FromRoute] int borrowerId)
        {
            BorrowerLogic.DeleteBorrower(borrowerId);
        }

        [HttpPut("{id}")]
        public void ModifyName([FromRoute]int id, [FromBody]ModifyBorrowerNameRequest req)
        {
            BorrowerLogic.ModifyName(id, req.newName);
        }

        [HttpGet] 
        public IEnumerable<Borrower> GetAll()
        {
            return BorrowerLogic.GetBorrowers();
        }

        [HttpGet("late")]
        public IEnumerable<Borrower> GetLateBorrowers()
        {
            return BorrowerLogic.GetLateBorrowers();
        }
    }
}
