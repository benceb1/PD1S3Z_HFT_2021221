using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public interface ILibraryLogic
    {
        void Create(Library item);
        void Delete(int id);
        Library Read(int id);
        IQueryable<Library> ReadAll();
        void Update(Library item);
    }
}
