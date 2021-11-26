using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Repository
{
    public interface ILendingRepository : IRepository<Lending>
    {
        void Update(int id, Lending bookLending);
        void SetActiveStatus(int id, bool isActive);
    }
}
