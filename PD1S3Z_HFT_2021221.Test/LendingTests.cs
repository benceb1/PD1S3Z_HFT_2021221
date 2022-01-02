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
        [Test]
        public void TestGetNonActiveLendings()
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

            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>(); 

            lendingRepo.Setup(repo => repo.GetAll()).Returns(lendings.AsQueryable());

            logic = new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);

            List<Lending> exceptedLendings = new List<Lending>() { lendings[1], lendings[2]};

            var result = logic.GetNonActiveLendings();

            Assert.That(result.Count(), Is.EqualTo(exceptedLendings.Count));

            Assert.That(result, Is.EquivalentTo(exceptedLendings));
        }

        [Test]
        public void TestAddNewLending()
        {
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();

            Lending lending = new Lending() { Id = 42, BookId = 1, BorrowerId = 1, Active = true, StartDate = DateTime.Now };

            lendingRepo.Setup(repo => repo.Insert(It.IsAny<Lending>())).Returns(lending);
            bookRepo.Setup(repo => repo.GetOne(It.IsAny<int>())).Returns(new Book() { Id = 1, LibraryId = 1, Library = new Library() { Id = 1 } });
            LendingLogic logic = new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);

            Lending inserted = logic.StartLending(1, 1, 1);

            Assert.That(inserted.Id, Is.EqualTo(42));
            lendingRepo.Verify(repo => repo.Insert(It.IsAny<Lending>()), Times.Once);
        }

        [Test]
        public void TestAddLendingThrowException()
        {
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();


            LendingLogic logic = new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);

            Assert.That(() => logic.StartLending(1, 1, 1),
	            Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestEndLendingThrowException()
        {
            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();


            LendingLogic logic = new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);

            Assert.That(() => logic.EndLending(1),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void TestGetActiveLendings()
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

            Mock<IBorrowerRepository> borrowerRepo = new Mock<IBorrowerRepository>();
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILendingRepository> lendingRepo = new Mock<ILendingRepository>();


            lendingRepo.Setup(repo => repo.GetAll()).Returns(lendings.AsQueryable());

            logic = new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);

            List<Lending> exceptedLendings = new List<Lending>() { lendings[0], lendings[3], lendings[4], lendings[5] };

            var result = logic.GetActiveLendings();

            Assert.That(result.Count(), Is.EqualTo(exceptedLendings.Count));

            Assert.That(result, Is.EquivalentTo(exceptedLendings));
        }

        Mock<IBorrowerRepository> borrowerRepo;
        Mock<IBookRepository> bookRepo;
        Mock<ILendingRepository> lendingRepo;
        Library expectedLibrary;
        Book expectedBook;
        Borrower expectedBorrower;

        private LendingLogic CreateLogicWithMocks()
        {
            borrowerRepo = new Mock<IBorrowerRepository>();
            bookRepo = new Mock<IBookRepository>();
            lendingRepo = new Mock<ILendingRepository>();

            Library kave = new Library() { Id = 2, Name = "Kávé-könyvtár", BookCapacity = 5 };
            Library suti = new Library() { Id = 1, Name = "Süti-könyvtár", BookCapacity = 5 };

            Borrower borrower1 = new Borrower() { Id = 1, Name = "Sári", Age = 15, MembershipLevel = MembershipLevel.Bronze, NumberOfBooksRead = 2, StartOfMembership = new DateTime(2018, 1, 1), NumberOfLateLendings = 0 };
            Borrower borrower2 = new Borrower() { Id = 2, Name = "Lajos", Age = 30, MembershipLevel = MembershipLevel.Gold, NumberOfBooksRead = 20, StartOfMembership = new DateTime(2000, 2, 4), NumberOfLateLendings = 0 };
            Borrower borrower3 = new Borrower() { Id = 3, Name = "Erika", Age = 20, MembershipLevel = MembershipLevel.Silver, NumberOfBooksRead = 11, StartOfMembership = new DateTime(2020, 11, 20), NumberOfLateLendings = 0 };

            List<Book> books = new List<Book>() {
                new Book() { Id = 7, Author = "Füst Milán", Title = "A feleségem története", Genre = "regény", NumberOfPages = 408, Publishing = 1942, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 1, Author = "Stefan Zweig", Title = "Sakknovella", Genre = "novella", NumberOfPages = 97, Publishing = 1941, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 2, Author = "Charles Dickens", Title = "Nicolas Nickleby I.", Genre = "regény", NumberOfPages = 962, Publishing = 1839, LibraryId = suti.Id, Library= suti },
                new Book() { Id = 3, Author = "Charles Dickens", Title = "Karácsonyi ének", Genre = "regény", NumberOfPages = 98, Publishing = 1843, LibraryId = suti.Id, Library= suti },
                new Book() { Id = 4, Author = "J. K. Rowling", Title = "Harry Potter és a bölcsek köve", Genre = "szórakoztató irodalom", NumberOfPages = 287, Publishing = 1997, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 5, Author = "J. K. Rowling", Title = "Harry Potter és a titkok kamrája", Genre = "szórakoztató irodalom", NumberOfPages = 315, Publishing = 1998, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 6, Author = "Vladimir Nabokov", Title = "Lolita", Genre = "regény", NumberOfPages = 491, Publishing = 1955, LibraryId = suti.Id, Library= suti }
            };

            List<Lending> lendings = new List<Lending>()
            {
                new Lending() {Id = 1,  Late = true, BookId = 7, Book=books[0], BorrowerId = 1, Borrower = borrower1},
                new Lending() {Id = 2,  Late = true, BookId = 2, Book=books[2], BorrowerId = 1, Borrower = borrower1},
                new Lending() {Id = 3,  Late = true, BookId = 7, Book=books[0], BorrowerId = 1, Borrower = borrower1},
                new Lending() {Id = 4,  Late = true, BookId = 2, Book=books[2], BorrowerId = 2, Borrower = borrower2},
                new Lending() {Id = 5,  Late = true, BookId = 7, Book=books[0], BorrowerId = 2, Borrower = borrower2},
                new Lending() {Id = 6,  Late = true, BookId = 1, Book=books[1], BorrowerId = 3, Borrower = borrower3}
            };

            expectedBook = books[0];
            expectedLibrary = kave;
            expectedBorrower = borrower1;

            bookRepo.Setup(repo => repo.GetAll()).Returns(books.AsQueryable());
            lendingRepo.Setup(repo => repo.GetAll()).Returns(lendings.AsQueryable());

            return new LendingLogic(bookRepo.Object, lendingRepo.Object, borrowerRepo.Object);
        }

        [Test]
        public void TestGetPopularLibrary()
        {
            var logic = CreateLogicWithMocks();
            var lib = logic.MostPopularLibrary().First();

            Assert.That(lib, Is.EqualTo(expectedLibrary));
            lendingRepo.Verify(repo => repo.GetAll(), Times.Exactly(1));
            bookRepo.Verify(repo => repo.GetAll(), Times.Never);
        }

        [Test]
        public void TestGetMostBelatedBook()
        {
            var logic = CreateLogicWithMocks();
            var book = logic.MostBelatedBook().First();

            Assert.That(book, Is.EqualTo(expectedBook));
            lendingRepo.Verify(repo => repo.GetAll(), Times.Exactly(1));
            bookRepo.Verify(repo => repo.GetAll(), Times.Never);
        }

        [Test]
        public void TestMostActiveBorrower()
        {
            var logic = CreateLogicWithMocks();
            var borrower = logic.MostActiveBorrower().First();

            Assert.That(borrower, Is.EqualTo(expectedBorrower));
            lendingRepo.Verify(repo => repo.GetAll(), Times.Exactly(1));
            bookRepo.Verify(repo => repo.GetAll(), Times.Never);
        }
    }
}
