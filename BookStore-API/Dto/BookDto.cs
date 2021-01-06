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

        public double? Price { get; set; }

        public virtual AuthorDto Author { get; set; }

        public int AuthorId { get; set; }
    }
}