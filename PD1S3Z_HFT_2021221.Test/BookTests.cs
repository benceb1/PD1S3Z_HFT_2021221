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
    public class BookTests
    {
        [Test]
        public void TestGetBooksByLibrary()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            Mock<ILibraryRepository> libraryRepo = new Mock<ILibraryRepository>();


            List<Book> books = new List<Book>()
            {
                new Book() { Title = "book1", LibraryId = 1 },
                new Book() { Title = "book2", LibraryId = 2 },
                new Book() { Title = "book3", LibraryId = 2 },
                new Book() { Title = "book4", LibraryId = 1 },
                new Book() { Title = "book5", LibraryId = 2 },
                new Book() { Title = "book6", LibraryId = 1 }
            };

            List<Book> exceptedBooks = new List<Book>() { books[1], books[2], books[4] };

            bookRepo.Setup(repo => repo.GetAll()).Returns(books.AsQueryable());
            BookLogic logic = new BookLogic(bookRepo.Object, libraryRepo.Object);

            var result = logic.GetBooksByLibraryId(2);

            Assert.That(result.Count(), Is.EqualTo(exceptedBooks.Count));

            Assert.That(result, Is.EquivalentTo(exceptedBooks));

            bookRepo.Verify(repo => repo.GetAll(), Times.Once);

        }


        Mock<IBookRepository> bookRepo;
        Mock<ILibraryRepository> libraryRepo;
        List<AvgPagesResult> expectedAverages;

        private BookLogic CreateLogicWithMocks()
        {
            bookRepo = new Mock<IBookRepository>();
            libraryRepo = new Mock<ILibraryRepository>();

            Library kave = new Library() { Id = 2, Name = "Kávé-könyvtár", BookCapacity = 5 };
            Library suti = new Library() { Id = 1, Name = "Süti-könyvtár", BookCapacity = 5 };

            List<Book> books = new List<Book>() {
                new Book() { Id = 7, Author = "Füst Milán", Title = "A feleségem története", Genre = "regény", NumberOfPages = 408, Publishing = 1942, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 1, Author = "Stefan Zweig", Title = "Sakknovella", Genre = "novella", NumberOfPages = 97, Publishing = 1941, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 2, Author = "Charles Dickens", Title = "Nicolas Nickleby I.", Genre = "regény", NumberOfPages = 962, Publishing = 1839, LibraryId = suti.Id, Library= suti },
                new Book() { Id = 3, Author = "Charles Dickens", Title = "Karácsonyi ének", Genre = "regény", NumberOfPages = 98, Publishing = 1843, LibraryId = suti.Id, Library= suti },
                new Book() { Id = 4, Author = "J. K. Rowling", Title = "Harry Potter és a bölcsek köve", Genre = "szórakoztató irodalom", NumberOfPages = 287, Publishing = 1997, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 5, Author = "J. K. Rowling", Title = "Harry Potter és a titkok kamrája", Genre = "szórakoztató irodalom", NumberOfPages = 315, Publishing = 1998, LibraryId = kave.Id, Library= kave },
                new Book() { Id = 6, Author = "Vladimir Nabokov", Title = "Lolita", Genre = "regény", NumberOfPages = 491, Publishing = 1955, LibraryId = suti.Id, Library= suti }
            };

            expectedAverages = new List<AvgPagesResult>()
            {
                new AvgPagesResult() { LibraryName = "Kávé-könyvtár", AvgPages= 276.75 },
                new AvgPagesResult() { LibraryName = "Süti-könyvtár", AvgPages= 517 }
            };

            List<Library> libraries = new List<Library>() { kave, suti };

            bookRepo.Setup(repo => repo.GetAll()).Returns(books.AsQueryable());
            libraryRepo.Setup(repo => repo.GetAll()).Returns(libraries.AsQueryable());

            return new BookLogic(bookRepo.Object, libraryRepo.Object);
        }

        [Test] 
        public void TestGetAverages()
        {
            var logic = CreateLogicWithMocks();
            var actualAverages = logic.AvgOfPageNumbersInLibraries();

            Assert.That(actualAverages, Is.EquivalentTo(expectedAverages));
            bookRepo.Verify(repo => repo.GetAll(), Times.Exactly(1));
            libraryRepo.Verify(repo => repo.GetAll(), Times.Never);
        }

        [Test] 
        public void TestGetAveragesJoin()
        {
            var logic = CreateLogicWithMocks();
            var actualAverages = logic.AvgOfPageNumbersInLibrariesJoin();

            Assert.That(actualAverages, Is.EquivalentTo(expectedAverages));
            bookRepo.Verify(repo => repo.GetAll(), Times.Once);
            libraryRepo.Verify(repo => repo.GetAll(), Times.Once);
        }
    }
}
