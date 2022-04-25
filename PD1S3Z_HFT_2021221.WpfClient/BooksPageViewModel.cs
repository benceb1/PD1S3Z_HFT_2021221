using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PD1S3Z_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PD1S3Z_HFT_2021221.WpfClient
{ //<title> <author> <number_of_pages(number)> <genre> <publishing(number)> <library_id(muber)>
    public class BooksPageViewModel : ObservableRecipient
    {
        public RestCollection<Book> Books { get; set; }
        public RestCollection<Library> Libraries { get; set; }

        private Book selectedBook;

        private Library selectedLibrary;

        public Library SelectedLibrary
        {
            get { return selectedLibrary; }
            set { 
                SetProperty(ref selectedLibrary, value);
                OnPropertyChanged();
                (DeleteBookCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public Book SelectedBook
        {
            get { return selectedBook; }
            set
            {
                SetProperty(ref selectedBook, value);
                if (value != null)
                {
                    selectedBook = new Book()
                    {
                        Id = value.Id,
                        Title = value.Title,
                        Author = value.Author,
                        NumberOfPages = value.NumberOfPages,
                        Genre = value.Genre,
                        Publishing = value.Publishing,
                        LibraryId = value.LibraryId,
                    };
                    if(value.LibraryId != null)
                    {
                        SelectedLibrary = Libraries.Where(l => l.Id == value.LibraryId).FirstOrDefault();       
                    }

                    OnPropertyChanged();
                    (DeleteBookCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }

        }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        public ICommand CreateBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand UpdateBookCommand { get; set; }

        public BooksPageViewModel()
        {
            if (!IsInDesignMode)
            {
                Books = new RestCollection<Book>("http://localhost:26706/", "book", "hub");
                Libraries = new RestCollection<Library>("http://localhost:26706/", "library", "hub");

                CreateBookCommand = new RelayCommand(() =>
                {
                    ;
                    Books.Add(new Book()
                    {
                        Title = SelectedBook.Title,
                        Author = SelectedBook.Author,
                        NumberOfPages = SelectedBook.NumberOfPages,
                        Genre = SelectedBook.Genre,
                        Publishing = SelectedBook.Publishing,
                        LibraryId = SelectedLibrary.Id
                    });
                });

                UpdateBookCommand = new RelayCommand(() =>
                {
                    Books.Update(SelectedBook);
                });

                DeleteBookCommand = new RelayCommand(() =>
                {
                    Books.Delete(SelectedBook.Id);
                },
                () => SelectedBook != null);

                SelectedBook = new Book();

            }
        }
    }
}
