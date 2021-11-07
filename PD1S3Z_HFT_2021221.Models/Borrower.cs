using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Models
{
    [Table("borrower")]
    public class Borrower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        // bronze, silver, gold
        public string MembershipLevel { get; set; }
        public DateTime StartOfMembership { get; set; }
        public int Age { get; set; }
        public int NumberOfBooksRead { get; set; }

        [NotMapped]
        public virtual ICollection<Book> Books { get; set; }

        [NotMapped]
        public virtual ICollection<BookLending> BookLendings { get; set; }

        public Borrower()
        {
            BookLendings = new HashSet<BookLending>();
            Books = new HashSet<Book>();
        }

    }
}
