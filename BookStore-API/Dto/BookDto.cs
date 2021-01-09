using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Dto
{
    public class BookDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public short? Year { get; set; }

        public string Isbn { get; set; }

        public string Summary { get; set; }

        public string Cover { get; set; }

        public decimal? Price { get; set; }

        public virtual AuthorDto Author { get; set; }

        public int AuthorId { get; set; }
    }

    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }

        public short? Year { get; set; }

        [Required]
        public string Isbn { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        public string Cover { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
    
    public class UpdateBookDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        public short? Year { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        public string Cover { get; set; }

        public decimal? Price { get; set; }
        
        [Required]
        public int AuthorId { get; set; }
    }
}