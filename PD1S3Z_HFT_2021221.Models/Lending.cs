using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }

        public Lending()
        {
            Books = new HashSet<Book>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Lending)
            {
                Lending other = obj as Lending;
                return this.Id == other.Id &&
                    this.StartDate == other.StartDate &&
                    this.EndDate == other.EndDate &&
                    this.Active == other.Active &&
                    this.Books.SequenceEqual(other.Books) &&
                    this.LibraryId == other.LibraryId &&
                    this.BorrowerId == other.BorrowerId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
