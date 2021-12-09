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
    public enum MembershipLevel
    {
        Silver, Bronze, Gold
    }

    [Table("borrower")]
    public class Borrower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        // bronze, silver, gold
        public MembershipLevel MembershipLevel { get; set; }

        public DateTime StartOfMembership { get; set; }

        public int Age { get; set; }

        public int NumberOfBooksRead { get; set; }

        public int NumberOfLateLendings { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Lending> BookLendings { get; set; }

        public Borrower()
        {
            BookLendings = new HashSet<Lending>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Borrower)
            {
                Borrower other = obj as Borrower;
                return this.Id == other.Id &&
                    this.Name == other.Name &&
                    this.MembershipLevel == other.MembershipLevel &&
                    this.StartOfMembership == other.StartOfMembership &&
                    this.Age == other.Age &&
                    this.NumberOfBooksRead == other.NumberOfBooksRead &&
                    this.NumberOfLateLendings == other.NumberOfLateLendings;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"[ id: {Id}, name: {Name}, memberhip: {Enum.GetName(typeof(MembershipLevel), MembershipLevel)}, start of membership: {StartOfMembership}, age: {Age}, number of finished books: {NumberOfBooksRead}, late: {NumberOfLateLendings} ]";
        }
    }
}
