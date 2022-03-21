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
{
    public class LibrariesPageViewModel : ObservableRecipient
    {
        public RestCollection<Library> Libraries { get; set; }

        private Library selectedActor;

        public Library SelectedLibrary
        {
            get { return selectedActor; }
            set
            {
                SetProperty(ref selectedActor, value);
                if (value != null)
                {
                    selectedActor = new Library()
                    {
                        Name = value.Name,
                        Id = value.Id,
                        BookCapacity = value.BookCapacity
                    };
                    OnPropertyChanged();
                    (DeleteLibraryCommand as RelayCommand).NotifyCanExecuteChanged();
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

        public ICommand CreateLibraryCommand { get; set; }
        public ICommand DeleteLibraryCommand { get; set; }
        public ICommand UpdateLibraryCommand { get; set; }

        public LibrariesPageViewModel()
        {
            if (!IsInDesignMode)
            {
                Libraries = new RestCollection<Library>("http://localhost:26706/", "library", "hub");

                CreateLibraryCommand = new RelayCommand(() =>
                {
                    ;
                    Libraries.Add(new Library()
                    {
                        Name = SelectedLibrary.Name,
                        BookCapacity = 4
                    });
                });

                UpdateLibraryCommand = new RelayCommand(() =>
                {
                    Libraries.Update(SelectedLibrary);
                });

                DeleteLibraryCommand = new RelayCommand(() =>
                {
                    Libraries.Delete(SelectedLibrary.Id);
                },
                () => SelectedLibrary != null);

                SelectedLibrary = new Library();

            }
        }
    }
}
