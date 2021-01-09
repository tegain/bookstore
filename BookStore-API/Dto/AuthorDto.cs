using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    
    public class CreateAuthorDto
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public string Bio { get; set; }
    }
    
    public class UpdateAuthorDto
    {
        // [Required]
        // public int Id { get; set; }
        
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }

        public string Bio { get; set; }
    }
}