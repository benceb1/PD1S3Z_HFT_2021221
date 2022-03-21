using Microsoft.EntityFrameworkCore;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Data
{
    public class LibraryDbContext : DbContext
    {
        public virtual DbSet<Book> Book { get; set; }   
        public virtual DbSet<Lending> BookLending { get; set; }
        public virtual DbSet<Borrower> Borrower { get; set; }
        public virtual DbSet<Library> Library { get; set; }

        public LibraryDbContext()
        {
            this.Database.EnsureCreated();
        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base (options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.
                    UseLazyLoadingProxies().
                    UseSqlServer(@"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\ProjectDb.mdf;Integrated Security=True;MultipleActiveResultSets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Library kave = new Library() { Id = 2, Name = "Kávé-könyvtár", BookCapacity = 5 };
            Library suti = new Library() { Id = 1, Name = "A-könyvtár", BookCapacity = 5 };

            Book book1 = new Book() { Id = 7, Author = "Füst Milán", Title = "A feleségem története", Genre = "regény", NumberOfPages = 408, Publishing = 1942, LibraryId = kave.Id };
            Book book2 = new Book() { Id = 1, Author = "Stefan Zweig", Title = "Sakknovella", Genre = "novella", NumberOfPages = 97, Publishing = 1941, LibraryId = kave.Id };
            Book book3 = new Book() { Id = 2, Author = "Charles Dickens", Title = "Nicolas Nickleby I.", Genre = "regény", NumberOfPages = 962, Publishing = 1839, LibraryId = suti.Id };
            Book book4 = new Book() { Id = 3, Author = "Charles Dickens", Title = "Karácsonyi ének", Genre = "regény", NumberOfPages = 98, Publishing = 1843, LibraryId = suti.Id };
            Book book5 = new Book() { Id = 4, Author = "J. K. Rowling", Title = "Harry Potter és a bölcsek köve", Genre = "szórakoztató irodalom", NumberOfPages = 287, Publishing = 1997, LibraryId = kave.Id };
            Book book6 = new Book() { Id = 5, Author = "J. K. Rowling", Title = "Harry Potter és a titkok kamrája", Genre = "szórakoztató irodalom", NumberOfPages = 315, Publishing = 1998, LibraryId = kave.Id };
            Book book7 = new Book() { Id = 6, Author = "Vladimir Nabokov", Title = "Lolita", Genre = "regény", NumberOfPages = 491, Publishing = 1955, LibraryId = suti.Id };

            Borrower borrower1 = new Borrower() { Id = 3, Name = "Sári", Age = 15, MembershipLevel = MembershipLevel.Bronze, NumberOfBooksRead = 2, StartOfMembership = new DateTime(2018, 1, 1), NumberOfLateLendings = 0 };
            Borrower borrower2 = new Borrower() { Id = 1, Name = "Lajos", Age = 30, MembershipLevel = MembershipLevel.Gold, NumberOfBooksRead = 20, StartOfMembership = new DateTime(2000, 2, 4), NumberOfLateLendings = 1 };
            Borrower borrower3 = new Borrower() { Id = 2, Name = "Erika", Age = 20, MembershipLevel = MembershipLevel.Silver, NumberOfBooksRead = 11, StartOfMembership = new DateTime(2020, 11, 20), NumberOfLateLendings = 0 };

            List<Lending> lendings = new List<Lending>()
            {
                new Lending() {Id = 1,  Late = false, BookId = 7, BorrowerId = 1, StartDate = new DateTime(2021, 9, 11), EndDate = new DateTime(2021, 10, 11), Active = true },
                new Lending() {Id = 2,  Late = false, BookId = 2,  BorrowerId = 1, StartDate = new DateTime(2021, 9, 11), EndDate = new DateTime(2021, 10, 11), Active = false },
                new Lending() {Id = 3, Late = false, BookId = 7,  BorrowerId = 1, StartDate = new DateTime(2021, 9, 11), EndDate = new DateTime(2021, 10, 11), Active = false },
                new Lending() {Id = 4,  Late = false, BookId = 2,  BorrowerId = 2, StartDate = new DateTime(2021, 9, 11), EndDate = new DateTime(2021, 10, 11), Active = false },
                new Lending() {Id = 5,  Late = false, BookId = 7,  BorrowerId = 2, StartDate = new DateTime(2021, 9, 11), EndDate = new DateTime(2021, 10, 11), Active = false },
                new Lending() {Id = 6, Late = false, BookId = 1,  BorrowerId = 3, StartDate = new DateTime(2021, 9, 11), EndDate = new DateTime(2021, 10, 11), Active = false }
            };

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(book => book.Library)
                    .WithMany(library => library.Books)
                    .HasForeignKey(book => book.LibraryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Lending>(entity =>
            {
                entity.HasOne(bookLending => bookLending.Borrower)
                    .WithMany(borrower => borrower.BookLendings)
                    .HasForeignKey(lending => lending.BorrowerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bookLending => bookLending.Book)
                    .WithMany(book => book.BookLendings)
                    .HasForeignKey(lending => lending.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Library>().HasData(kave, suti);
            modelBuilder.Entity<Book>().HasData(book1, book2, book3, book4, book5, book6, book7);
            modelBuilder.Entity<Borrower>().HasData(borrower1, borrower2, borrower3);
            modelBuilder.Entity<Lending>().HasData(lendings);
        }
    }
}
