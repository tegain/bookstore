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
    /// <summary>
    /// Interact with Authors 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, ILoggerService logger, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>A list of authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attempted get all Authors");
                var authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDto>>(authors);
                _logger.LogInfo("Authors successfully retrieved");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalError($"{exception.Message} - {exception.InnerException}");
            }
        }

        /// <summary>
        /// Returns a single author
        /// </summary>
        /// <param name="id">Author ID</param>
        /// <returns>A single record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                _logger.LogInfo($"Attempted get single author with id: {id}");
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn($"Author with id '{id}' not found.");
                    return NotFound();
                }

                var response = _mapper.Map<AuthorDto>(author);
                _logger.LogInfo("Authors successfully retrieved");
                return Ok(response);
            }
            catch (Exception exception)
            {
                return InternalError($"{exception.Message} - {exception.InnerException}");
            }
        }

        /// <summary>
        /// Create a new author
        /// </summary>
        /// <param name="createAuthorDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
        {
            try
            {
                if (createAuthorDto == null)
                {
                    _logger.LogWarn($"Create Author: empty request body");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Create Author: data incomplete");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(createAuthorDto);
                var isRecordSaved = await _authorRepository.Create(author);

                if (!isRecordSaved)
                {
                    return InternalError("Author creation failed");
                }

                _logger.LogInfo("Author created");
                return Created("Create", new { author });
            }
            catch (Exception exception)
            {
                return InternalError($"{exception.Message} - {exception.InnerException}");
            }
        }

        /// <summary>
        /// Update an existing author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAuthorDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthor([Required] int id, [FromBody] UpdateAuthorDto updateAuthorDto)
        {
            try
            {
                if (updateAuthorDto == null || id != updateAuthorDto.Id)
                {
                    _logger.LogWarn("Update Author: empty request body");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Update Author: data incomplete");
                    return BadRequest(ModelState);
                }

                var authorExists = await _authorRepository.Exists(id);
                if (!authorExists)
                    return NotFound();

                var author = _mapper.Map<Author>(updateAuthorDto);
                var isRecordSaved = await _authorRepository.Update(author);

                if (!isRecordSaved)
                {
                    return InternalError("Author update failed");
                }

                _logger.LogInfo("Author updated");
                return NoContent();
            }
            catch (Exception exception)
            {
                return InternalError($"{exception.Message} - {exception.InnerException}");
            }
        }

        /// <summary>
        /// Delete an author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor([Required] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var authorExists = await _authorRepository.Exists(id);
                if (!authorExists)
                    return NotFound();

                var author = await _authorRepository.FindById(id);
                var isRecordDeleted = await _authorRepository.Delete(author);

                if (!isRecordDeleted)
                {
                    return InternalError("Author deletion failed");
                }

                _logger.LogInfo("Author deleted");
                return NoContent();
            }
            catch (Exception exception)
            {
                return InternalError($"{exception.Message} - {exception.InnerException}");
            }
        }
        
        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, $"Something went wrong.");
        }
    }
}