using System.Collections.Generic;

namespace BookStore_API.Dto
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Bio { get; set; }

        public virtual IList<BookDto> Books { get; set; }
    }
}