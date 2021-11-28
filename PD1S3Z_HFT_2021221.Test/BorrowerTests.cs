using Moq;
using NUnit.Framework;
using PD1S3Z_HFT_2021221.Logic;
using PD1S3Z_HFT_2021221.Models;
using PD1S3Z_HFT_2021221.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Test
{
    [TestFixture]
    public class BorrowerTests
    {
        [Test]
        public void TestBorrowerAdd()
        {
            Borrower testBorrower = new Borrower();
            testBorrower.Id = 11;
            testBorrower.Name = "name";
            testBorrower.Age = 10;
            testBorrower.MembershipLevel = "bronze";
            testBorrower.NumberOfLateLendings = 0;
            testBorrower.StartOfMembership = DateTime.Now;
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<ILibraryRepository> libraryRepo = new Mock<ILibraryRepository>();

            borrowerRepo.Setup(repo => repo.Insert(It.IsAny<Borrower>())).Returns(testBorrower);
            BorrowerLogic logic = new BorrowerLogic(libraryRepo.Object, borrowerRepo.Object);

            Borrower insertedBorrower = logic.InsertNewBorrower(new Borrower() { Name="name", Age = 11});

            Assert.That(insertedBorrower.Id, Is.EqualTo(11));
        }



    }
}
