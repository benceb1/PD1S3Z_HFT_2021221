﻿using Microsoft.AspNetCore.Mvc;
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
        public void EndLending([FromBody] int lendingId)
        {
            LendingLogic.EndLending(lendingId);
        }

        [HttpGet]
        public IEnumerable<Lending> GetLendings()
        {
            return LendingLogic.GetAll();
        } 
    }

    public class NewLendingRequest
    {
        public int borrowerId { get; set; }
        public int lendingWeeks { get; set; }
        public int bookId { get; set; }
    }
}
