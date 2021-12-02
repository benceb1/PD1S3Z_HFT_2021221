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
            Mock<IBookRepository> libraryRepo = new Mock<IBookRepository>();

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

            libraryRepo.Setup(repo => repo.GetAll()).Returns(books.AsQueryable());
            BookLogic logic = new BookLogic(libraryRepo.Object);

            var result = logic.GetBooksByLibraryId(2);

            Assert.That(result.Count(), Is.EqualTo(exceptedBooks.Count));

            Assert.That(result, Is.EquivalentTo(exceptedBooks));

            libraryRepo.Verify(repo => repo.GetAll(), Times.Once);

        }

        [Test]
        public void GetBooksByLibrary()
        {
            Mock<IBookRepository> libraryRepo = new Mock<IBookRepository>();

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

            libraryRepo.Setup(repo => repo.GetAll()).Returns(books.AsQueryable());
            BookLogic logic = new BookLogic(libraryRepo.Object);

            var result = logic.GetBooksByLibraryId(2);

            Assert.That(result.Count(), Is.EqualTo(exceptedBooks.Count));

            Assert.That(result, Is.EquivalentTo(exceptedBooks));

            libraryRepo.Verify(repo => repo.GetAll(), Times.Once);
        }
    }
}
