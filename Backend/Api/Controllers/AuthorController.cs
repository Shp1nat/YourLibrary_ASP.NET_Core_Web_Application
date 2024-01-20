using Backend.Api.Contract;
using Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;
        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> CreateAuthor(string nameOfAuthor)
        {
            Regex regex = new Regex(@"^[a-zA-Z]{2,15}\s[a-zA-Z]{2,15}(?:\s[a-zA-Z]{2,15})?$");
            Match match = regex.Match(nameOfAuthor);
            if (!match.Success)
            {
                ModelState.AddModelError("author", "Invalid author format");
                return BadRequest(ModelState);
            }
            Guid? uid = _authorService.CreateAuthor(nameOfAuthor);
            if (uid == null)
            {
                ModelState.AddModelError("exist", "Author already exists");
                return BadRequest(ModelState);
            }
            return uid;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Author>?> GetAuthors()
        {
            List<Author>? authors = _authorService.GetAuthors();
            if (authors == null)
            {
                ModelState.AddModelError("authors", "None authors exist");
                return BadRequest(ModelState);
            }
            return authors;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult EditAuthor(Guid uIdAuthor, string nameOfAuthor)
        {
            Regex regex = new Regex(@"^[a-zA-Z]{2,15}\s[a-zA-Z]{2,15}(?:\s[a-zA-Z]{2,15})?$");
            Match match = regex.Match(nameOfAuthor);
            if (!match.Success)
            {
                ModelState.AddModelError("author", "Invalid author format");
                return BadRequest(ModelState);
            }
            if (_authorService.EditAuthor(uIdAuthor, nameOfAuthor)) return Ok();
            ModelState.AddModelError("Edit", "Author doesn't exist");
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteAuthor(Guid uIdAuthor)
        {
            if (_authorService.DeleteAuthor(uIdAuthor)) return Ok();
            else return NotFound();
        }
    }
}