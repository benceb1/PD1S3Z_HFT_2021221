﻿using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    interface ILibraryRepository : IRepository<Library>
    {
        void Update(int id, Library library);
    }
}
