using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Models
{
    [Table("book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int NumberOfPages { get; set; }

        public string Genre { get; set; }

        public int Publishing { get; set; }

        [NotMapped]
        public virtual ICollection<Lending> BookLendings { get; set; }

        [NotMapped]
        public virtual Library Library { get; set; }

        [NotMapped]
        public virtual Borrower Borrower { get; set; }

        public int? LibraryId { get; set; }

        public int? BorrowerId { get; set; }

        public Book()
        {
            BookLendings = new HashSet<Lending>();
        }

    }
}
