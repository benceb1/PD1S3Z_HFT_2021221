using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public interface ILendingLogic
    {
        Lending StartLending(int borrowerId, int bookId, int lendingWeeks);

        Lending EndLending(int lendingId);

        Library GetLibraryByBookId(int bookId);

        IList<Lending> GetActiveLendings();

        IList<Lending> GetAll();

        IList<Lending> GetLateLendings();

        IList<Lending> GetNonActiveLendings();

        IList <Library> MostPopularLibrary();

        IList <Book> MostBelatedBook();

        IList <Borrower> MostActiveBorrower();

        void Delete(int id);
    }
}
