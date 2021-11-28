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
    [Table("library")]
    public class Library
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [MaxLength(5)]
        public int BookCapacity { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Lending> BookLendings { get; set; }

        // books

        public Library()
        {
            Books = new HashSet<Book>();
            BookLendings = new HashSet<Lending>();
        }
    }
}
