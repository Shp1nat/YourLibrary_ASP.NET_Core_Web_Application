using Backend.Api.Contract;
using Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Xml.Linq;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> CreateBook(BookUpdate bookUpdate)
        {
            Guid? uid = _bookService.CreateBook(bookUpdate);
            if (uid == null)
            {
                ModelState.AddModelError("exist", "Book already exists");
                return BadRequest(ModelState);
            }
            return uid;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Book>?> GetBooks()
        {
            List<Book>? books = _bookService.GetBooks();
            if (books == null)
            {
                ModelState.AddModelError("books", "None books exist");
                return BadRequest(ModelState);
            }
            return books;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult EditBook(Guid uIdBook, BookUpdate bookUpdate)
        {
            if (_bookService.EditBook(uIdBook, bookUpdate)) return Ok();
            ModelState.AddModelError("Edit", "Book doesn't exist");
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteBook(Guid uIdBook)
        {
            if (_bookService.DeleteBook(uIdBook)) return Ok();
            else return NotFound();
        }
    }
}