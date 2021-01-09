using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, ILoggerService logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieve a list of books
        /// </summary>
        /// <returns>A list of books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDto>>(books);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"[{GetControllerActionNames()}] {e.Message} - {e.InnerException}");
            }
        }
        
        /// <summary>
        /// Retrieve a single book by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single book</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var book = await _bookRepository.FindById(id);
                if (book == null)
                    return NotFound();
                
                var response = _mapper.Map<BookDto>(book);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"[{GetControllerActionNames()}] {e.Message} - {e.InnerException}");
            }
        }
        
        /// <summary>
        /// Create a new book
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            try
            {
                if (createBookDto == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                var book = _mapper.Map<Book>(createBookDto);
                var isBookSaved = await _bookRepository.Create(book);

                if (!isBookSaved)
                    return InternalError($"Error creating book");
                
                return CreatedAtAction(nameof(CreateBook), new { id = book.Id }, book);
            }
            catch (Exception e)
            {
                return InternalError($"[{GetControllerActionNames()}] {e.Message} - {e.InnerException}");
            }
        }
        
        /// <summary>
        /// Update an existing book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateBookDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBook([Required] int id, [FromBody] UpdateBookDto updateBookDto)
        {
            try
            {
                if (updateBookDto == null || !ModelState.IsValid)
                {
                    _logger.LogWarn("Update Book: data incomplete");
                    return BadRequest(ModelState);
                }

                var bookExists = await _bookRepository.Exists(id);
                if (!bookExists)
                    return NotFound();

                var book = _mapper.Map<Book>(updateBookDto, (options) =>
                {
                    options.AfterMap((src, dest) => dest.Id = id);
                });
                var isRecordSaved = await _bookRepository.Update(book);

                if (!isRecordSaved)
                {
                    return InternalError($"Book update failed");
                }

                _logger.LogInfo($"Book updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook([Required] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var bookExists = await _bookRepository.Exists(id);
                if (!bookExists)
                    return NotFound();

                var existingBook = await _bookRepository.FindById(id);
                var isRecordDeleted = await _bookRepository.Delete(existingBook);
                
                if (!isRecordDeleted)
                    return InternalError($"Error deleting book with id {id}");

                _logger.LogInfo("Book deleted");
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{e.Message} - {e.InnerException}");
            }
        }


        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {actionName}";
        }
        
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, $"Something went wrong.");
        }
    }
}