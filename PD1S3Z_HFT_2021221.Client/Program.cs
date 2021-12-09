using ConsoleTools;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PD1S3Z_HFT_2021221.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(8000);
            RestService restService = new RestService("http://localhost:26706");

            ConsoleMenu queryMenu = new ConsoleMenu();
            ConsoleMenu mainMenu = new ConsoleMenu();
            ConsoleMenu crudMenu = new ConsoleMenu();
            ConsoleMenu bookMenu = new ConsoleMenu();
            ConsoleMenu lendingMenu = new ConsoleMenu();
            ConsoleMenu borrowerMenu = new ConsoleMenu();


            bookMenu
                .Add("GetAll", () => Get<Book>(restService, "book"))
                .Add("Delete", () => Delete(restService, "book"))
                .Add("Create", () => CreateBook(restService))
                .Add("ModifyLibrary", () => {
                    Console.WriteLine("This method modify the first book's lib id to 1");
                    ModifyLibRequest req = new ModifyLibRequest() { newLibId = 1 };
                    restService.Put<ModifyLibRequest>(req, "book/1");
                    Console.WriteLine("executed...\nPress enter to continue");
                    Console.ReadLine();
                })
                .Add("Back", () => mainMenu.Show())
                .Add("Close", ConsoleMenu.Close);

            lendingMenu
                .Add("GetAll", () => Get<Lending>(restService, "lending"))
                .Add("Delete", () => Delete(restService, "lending"))
                .Add("Create", () => CreateLending(restService))
                .Add("EndLending", () => {
                    Console.WriteLine("Enter an acive lending id");
                    int id = int.Parse(Console.ReadLine());
                    restService.Put<Lending>(new Lending(), $"lending/{id}");
                    Console.WriteLine("executed...\nPress enter to continue");
                    Console.ReadLine();
                })
                .Add("Back", () => mainMenu.Show())
                .Add("Close", ConsoleMenu.Close);

            borrowerMenu
                .Add("GetAll", () => Get<Borrower>(restService, "borrower"))
                .Add("Delete", () => Delete(restService, "borrower"))
                .Add("Create", () => CreateBorrower(restService))
                .Add("Modify name", () => {
                    Console.WriteLine("Enter a borrowerId and the new name space separated");
                    string[] line = Console.ReadLine().Split(' ');
                    ModifyBorrowerNameRequest req = new ModifyBorrowerNameRequest() { newName = line[1] };
                    restService.Put<ModifyBorrowerNameRequest>(req, $"borrower/{line[0]}");
                    Console.WriteLine("executed...\nPress enter to continue");
                    Console.ReadLine();
                })
                .Add("Back", () => mainMenu.Show())
                .Add("Close", ConsoleMenu.Close);


            queryMenu
                .Add("Average pages in libraries", () => Get<AvgPagesResult>(restService, "book/avgPages"))
                .Add("Actual late borrowers", () => Get<Borrower>(restService, "borrower/late"))
                .Add("Books by Library", () =>
                {
                    Console.WriteLine("Please type in the library id (1 or 2)");
                    string id = Console.ReadLine();
                    Get<Book>(restService, $"book/library/{id}");
                })
                .Add("Most popular library", () => Get<Library>(restService, "lending/mostPopularLib"))
                .Add("Most belated Book", () => Get<Book>(restService, "lending/mostBelatedBook"))
                .Add("Most active Borrower", () => Get<Borrower>(restService, "lending/mostActiveBorrower"))
                .Add("Back", () => mainMenu.Show())
                .Add("Close", ConsoleMenu.Close);

            crudMenu
               .Add("books", () => bookMenu.Show())
               .Add("lendings", () => lendingMenu.Show())
               .Add("borrowers", () => borrowerMenu.Show())
               .Add("Back", () => mainMenu.Show())
               .Add("Close", ConsoleMenu.Close);

            mainMenu
                .Add("Query Menu", () => queryMenu.Show())
                .Add("Crud Menu", () => crudMenu.Show())
                .Add("Close", ConsoleMenu.Close);
            mainMenu.Show();
        }

        public static void Get<T>(RestService restService, string endpoint)
        {
            var res = restService.Get<T>(endpoint);
            foreach (var item in res)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        public static void Delete(RestService restService, string endpoint)
        {
            Console.WriteLine("Type the id of the deletable element");
            int id = int.Parse(Console.ReadLine());
            restService.Delete(id, endpoint);
            Console.WriteLine("executed");
            Console.ReadLine();
        }

        public static void CreateBook(RestService service)
        {
            Console.WriteLine("Enter the following infos space separated!");
            Console.WriteLine("<title> <author> <number_of_pages(number)> <genre> <publishing(number)> <library_id(muber)>");
            string[] line = Console.ReadLine().Split(' ');
            Book book = new Book()
            {
                Title = line[0],
                Author = line[1],
                NumberOfPages = int.Parse(line[2]),
                Genre = line[3],
                Publishing = int.Parse(line[4]),
                LibraryId = int.Parse(line[5])
            };
            service.Post<Book>(book, "book");
            Console.WriteLine("executed\n press enter to continue...");
            Console.ReadLine();
        }

        public static void CreateLending(RestService service)
        {
            Console.WriteLine("Enter the following infos space separated!");
            Console.WriteLine("<borrowerid(number)> <lendingweeks(number)> <book_id(number)> ");
            string[] req = Console.ReadLine().Split(' ');
            NewLendingRequest newLending = new NewLendingRequest()
            {
                borrowerId = int.Parse(req[0]),
                lendingWeeks = int.Parse(req[1]),
                bookId = int.Parse(req[2])
            };
            service.Post<NewLendingRequest>(newLending, "lending");
            Console.WriteLine("executed\n press enter to continue...");
            Console.ReadLine();
        }
        
        public static void CreateBorrower(RestService service)
        {
            Console.WriteLine("Enter the following infos space separated!");
            Console.WriteLine("<name> <age(number)>");
            string[] req = Console.ReadLine().Split(' ');
            Borrower borrower = new Borrower()
            {
                Name = req[0],
                Age = int.Parse(req[1]),
                MembershipLevel = MembershipLevel.Bronze,
                NumberOfBooksRead = 0,
                NumberOfLateLendings = 0,
                StartOfMembership = DateTime.Now
            };
            service.Post<Borrower>(borrower, "borrower");
            Console.WriteLine("executed\n press enter to continue...");
            Console.ReadLine();

        }
    }
}
