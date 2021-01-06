using AutoMapper;
using BookStore_API.Data;
using BookStore_API.Dto;

namespace BookStore_API.Mappers
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}