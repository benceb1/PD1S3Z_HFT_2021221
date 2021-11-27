﻿using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public interface ILendingLogic
    {
        Lending StartLending(int borrowerId, List<Book> books, int libraryId, int lendingWeeks);

        Lending EndLending(Lending lending, int libraryId);

        IList<Lending> GetActiveLendings();

        IList<Lending> GetAllLendings();

        IList<Lending> GetLateLendings();
        IList<Lending> GetNonActiveLendings();
    }
}