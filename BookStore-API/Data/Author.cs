using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore_API.Data
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Bio { get; set; }

        public virtual IList<Book> Books { get; set; }
    }
}