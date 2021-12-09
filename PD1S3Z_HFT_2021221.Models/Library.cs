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

        // books

        public Library()
        {
            Books = new HashSet<Book>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Library)
            {
                Library other = obj as Library;
                return this.Id == other.Id &&
                    this.Name == other.Name &&
                    this.BookCapacity == other.BookCapacity;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"[ id: {Id}, name: {Name}, bookCapacity: {BookCapacity} ]";
        }
    }
}
