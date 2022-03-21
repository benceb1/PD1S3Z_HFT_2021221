using PD1S3Z_HFT_2021221.Models;
using PD1S3Z_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public class LibraryLogic : ILibraryLogic
    {
        ILibraryRepository repo;

        public LibraryLogic(ILibraryRepository repo)
        {
            this.repo = repo;
        }

        public void Create(Library item)
        {
            this.repo.Insert(item);
        }

        public void Delete(int id)
        {
            this.repo.Remove(id);
        }

        public Library Read(int id)
        {
            return this.repo.GetOne(id);
        }

        public IQueryable<Library> ReadAll()
        {
            return this.repo.GetAll();
        }

        public void Update(Library item)
        {
            this.repo.Update(item);
        }
    }
}
