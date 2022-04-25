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
    [Route("[controller]")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        IBorrowerLogic BorrowerLogic;
        IHubContext<SignalRHub> hub;

        public BorrowerController(IBorrowerLogic borrowerLogic, IHubContext<SignalRHub> hub)
        {
            BorrowerLogic = borrowerLogic;
            this.hub = hub;
        }

        [HttpPost]
        public void AddBorrower([FromBody] Borrower borrower)
        {
            BorrowerLogic.Insert(borrower);
            this.hub.Clients.All.SendAsync("BorrowerCreated", borrower);
        }

        [HttpDelete("{borrowerId}")]
        public void DeleteOne([FromRoute] int borrowerId)
        {
            var borrowerToDelete = this.BorrowerLogic.GetBorrowerById(borrowerId);
            this.BorrowerLogic.DeleteBorrower(borrowerId);
            this.hub.Clients.All.SendAsync("BorrowerDeleted", borrowerToDelete);

        }

        [HttpPut]
        public void Put([FromBody] Borrower value)
        {
            this.BorrowerLogic.Update(value);
            this.hub.Clients.All.SendAsync("BorrowerUpdated", value);
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
