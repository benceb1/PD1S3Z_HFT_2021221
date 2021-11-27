using PD1S3Z_HFT_2021221.Models;
using PD1S3Z_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Logic
{
    public class BorrowerLogic : IBorrowerLogic
    {

        private LibraryRepository LibraryRepository;
        private BorrowerRepository BorrowerRepository;

        public void DeleteBorrower(int borrowerId)
        {
            Borrower borrower = BorrowerRepository.GetOne(borrowerId);

            if (borrower == null) throw new InvalidOperationException("Borrower not found!");

            if (borrower.Books.Count != 0)
            {
                List<Library> libraries = LibraryRepository.GetAll().ToList();
                if (libraries.Count == 0)
                {
                    throw new Exception("Libraries not found");
                }

                int libIndex = 0;

                foreach (Book book in borrower.Books)
                {
                    if (libraries[libIndex].BookCapacity <= libraries[libIndex].Books.Count &&
                        libraries.Count > (libIndex + 1))
                    {
                        libIndex++;
                    } 
                    else
                    {
                        throw new Exception("There is no more libraries where we can to store books");
                    }
                    int libraryId = libraries[libIndex].Id;
                    LibraryRepository.AddBookToLibrary(libraryId, book);
                    BorrowerRepository.DeleteBookFromBorrower(borrowerId, book);
                }
            }
            BorrowerRepository.Remove(borrowerId);
        }

        public Borrower InsertNewBorrower(string name, int age)
        {
            Borrower borrower = new Borrower();
            borrower.Name = name;
            borrower.Age = age;
            borrower.MembershipLevel = "bronze";
            borrower.NumberOfLateLendings = 0;
            borrower.StartOfMembership = DateTime.Now;
            BorrowerRepository.Insert(borrower);
            return borrower;
        }
    }
}
