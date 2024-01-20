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
    public class PublisherController : ControllerBase
    {
        private readonly PublisherService _publisherService;
        public PublisherController(PublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> CreatePublisher(string nameOfPublisher)
        {
            Guid? uid = _publisherService.CreatePublisher(nameOfPublisher);
            if (uid == null)
            {
                ModelState.AddModelError("exist", "Publisher already exists");
                return BadRequest(ModelState);
            }
            return uid;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Publisher>?> GetPublishers()
        {
            List<Publisher>? publishers = _publisherService.GetPublishers();
            if (publishers == null)
            {
                ModelState.AddModelError("publishers", "None publishers exist");
                return BadRequest(ModelState);
            }
            return publishers;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult EditPublisher(Guid uIdPublisher, string nameOfPublisher)
        {
            if (_publisherService.EditPublisher(uIdPublisher, nameOfPublisher)) return Ok();
            ModelState.AddModelError("Edit", "Publisher doesn't exist");
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeletePublisher(Guid uIdPublisher)
        {
            if (_publisherService.DeletePublisher(uIdPublisher)) return Ok();
            else return NotFound();
        }
    }
}