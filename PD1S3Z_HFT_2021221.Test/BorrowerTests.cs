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
            testBorrower.MembershipLevel = MembershipLevel.Bronze;
            testBorrower.NumberOfLateLendings = 0;
            testBorrower.StartOfMembership = DateTime.Now;
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();

            borrowerRepo.Setup(repo => repo.Insert(It.IsAny<Borrower>())).Returns(testBorrower);
            BorrowerLogic logic = new BorrowerLogic(borrowerRepo.Object, lendingRepo.Object);

            Borrower insertedBorrower = logic.Insert(new Borrower() { Name="name", Age = 11});

            Assert.That(insertedBorrower.Id, Is.EqualTo(11));
        }

        [Test]
        public void TestGetLateBorrowers()
        {
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();

            Borrower borrower1 = new Borrower()
            {
                Id = 1,
                Name = "borrower1"
            };
            Borrower borrower2 = new Borrower()
            {
                Id = 1,
                Name = "borrower1"
            };

            List<Lending> lendings = new List<Lending>()
            {
                new Lending() {Id = 1, Active = true, EndDate = new DateTime(2021,11,01), Borrower = borrower1},
                new Lending() {Id = 2, Active = true, EndDate = new DateTime(2022,11,01), Borrower = borrower2},
            };

            lendingRepo.Setup(repo => repo.GetAll()).Returns(lendings.AsQueryable);

            BorrowerLogic logic = new BorrowerLogic(borrowerRepo.Object, lendingRepo.Object);

            List<Borrower> exceptedBorrowers = new List<Borrower>() { borrower1 };

            var result = logic.GetLateBorrowers();
            Assert.That(result.Count(), Is.EqualTo(exceptedBorrowers.Count));
            Assert.That(result, Is.EquivalentTo(exceptedBorrowers));
            lendingRepo.Verify(repo => repo.GetAll(), Times.Once);
        }
    }
}
