using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD1S3Z_HFT_2021221.Models
{
    [Table("lending")]
    public class Lending
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Active { get; set; }

        public virtual Library Library { get; set; }

        public virtual Borrower Borrower { get; set; }

        public int LibraryId { get; set; }

        public int BorrowerId { get; set; }

        [NotMapped]
        public virtual ICollection<Book> Books { get; set; }

        public Lending()
        {
            Books = new HashSet<Book>();
        }
    }
}
