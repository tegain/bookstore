using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public short? Year { get; set; }

        public string Isbn { get; set; }

        public string Summary { get; set; }

        public string Cover { get; set; }

        public decimal? Price { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        public int AuthorId { get; set; }
    }
}