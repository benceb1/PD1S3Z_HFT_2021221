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
    public class BorrowersPageViewModel : ObservableRecipient
    {
        public RestCollection<Borrower> Borrowers { get; set; }

        private Borrower selectedBorrower;

        public Borrower SelectedBorrower
        {
            get { return selectedBorrower; }
            set
            {
                SetProperty(ref selectedBorrower, value);
                if (value != null)
                {
                    selectedBorrower = new Borrower()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Age = value.Age,
                        MembershipLevel = value.MembershipLevel,
                        NumberOfBooksRead = value.NumberOfBooksRead,
                        NumberOfLateLendings = value.NumberOfLateLendings,
                        StartOfMembership = value.StartOfMembership,
                    };
                    OnPropertyChanged();
                    (DeleteBorrowerCommand as RelayCommand).NotifyCanExecuteChanged();
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

        public ICommand CreateBorrowerCommand { get; set; }
        public ICommand DeleteBorrowerCommand { get; set; }
        public ICommand UpdateBorrowerCommand { get; set; }

        public BorrowersPageViewModel()
        {
            if (!IsInDesignMode)
            {
                Borrowers = new RestCollection<Borrower>("http://localhost:26706/", "borrower", "hub");

                CreateBorrowerCommand = new RelayCommand(() =>
                {
                    ;
                    Borrowers.Add(new Borrower()
                    {
                        Name = SelectedBorrower.Name,
                        Age = SelectedBorrower.Age,
                        MembershipLevel = SelectedBorrower.MembershipLevel,
                        NumberOfBooksRead = SelectedBorrower.NumberOfBooksRead,
                        NumberOfLateLendings = SelectedBorrower.NumberOfLateLendings,
                        StartOfMembership = SelectedBorrower.StartOfMembership,
                    });
                });

                UpdateBorrowerCommand = new RelayCommand(() =>
                {
                    Borrowers.Update(SelectedBorrower);
                });

                DeleteBorrowerCommand = new RelayCommand(() =>
                {
                    Borrowers.Delete(SelectedBorrower.Id);
                },
                () => SelectedBorrower != null);

                SelectedBorrower = new Borrower();

            }
        }
    }
}
/*
 public int Id { get; set; }

        public string Name { get; set; }

        // bronze, silver, gold
        public MembershipLevel MembershipLevel { get; set; }

        public DateTime StartOfMembership { get; set; }

        public int Age { get; set; }

        public int NumberOfBooksRead { get; set; }

        public int NumberOfLateLendings { get; set; }
*/