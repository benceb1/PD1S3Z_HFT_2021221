using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PD1S3Z_HFT_2021221.Endpoint.Services;
using PD1S3Z_HFT_2021221.Logic;
using PD1S3Z_HFT_2021221.Models;
using System.Collections.Generic;

namespace PD1S3Z_HFT_2021221.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        ILibraryLogic logic;
        IHubContext<SignalRHub> hub;

        public LibraryController(ILibraryLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        [HttpGet]
        public IEnumerable<Library> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Library Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Library value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("LibraryCreated", value);
        }

        [HttpPut]
        public void Put([FromBody] Library value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("LibraryUpdated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var libraryToDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("LibraryDeleted", libraryToDelete);
        }
    }
}
