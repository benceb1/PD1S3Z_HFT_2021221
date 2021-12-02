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
    public class LendingTests
    {
        ILendingLogic logic;

        List<Lending> lendings = new List<Lending>()
            {
                new Lending() { Active = true, Id = 1 },
                new Lending() { Active = false, Id = 2 },
                new Lending() { Active = false, Id = 3 },
                new Lending() { Active = true, Id = 4 },
                new Lending() { Active = true, Id = 5 },
                new Lending() { Active = true, Id = 6 },
            };

        [SetUp]
        public void setup()
        {
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();

            lendingRepo.Setup(repo => repo.GetAll()).Returns(lendings.AsQueryable());

            logic = new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);
        }

        [Test]
        public void TestGetNonActiveLendings()
        {
            List<Lending> exceptedLendings = new List<Lending>() { lendings[1], lendings[2]};

            var result = logic.GetNonActiveLendings();

            Assert.That(result.Count(), Is.EqualTo(exceptedLendings.Count));

            Assert.That(result, Is.EquivalentTo(exceptedLendings));
        }

        [Test]
        public void TestGetActiveLendings()
        {
            List<Lending> exceptedLendings = new List<Lending>() { lendings[0], lendings[3], lendings[4], lendings[5] };

            var result = logic.GetActiveLendings();

            Assert.That(result.Count(), Is.EqualTo(exceptedLendings.Count));

            Assert.That(result, Is.EquivalentTo(exceptedLendings));
        }
    }
}
