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
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;
        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> CreateGenre(string nameOfGenre)
        {
            Guid? uid = _genreService.CreateGenre(nameOfGenre);
            if (uid == null)
            {
                ModelState.AddModelError("exist", "Genre already exists");
                return BadRequest(ModelState);
            }
            return uid;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Genre>?> GetGenres()
        {
            List<Genre>? genres = _genreService.GetGenres();
            if (genres == null)
            {
                ModelState.AddModelError("genres", "None genres exist");
                return BadRequest(ModelState);
            }
            return genres;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult EditGenre(Guid uIdGenre, string nameOfGenre)
        {
            if (_genreService.EditGenre(uIdGenre, nameOfGenre)) return Ok();
            ModelState.AddModelError("Edit", "Genre doesn't exist");
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteGenre(Guid uIdGenre)
        {
            if (_genreService.DeleteGenre(uIdGenre)) return Ok();
            else return NotFound();
        }
    }
}