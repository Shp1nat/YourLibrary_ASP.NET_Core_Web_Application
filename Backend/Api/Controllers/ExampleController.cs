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
    public class ExampleController : ControllerBase
    {
        private readonly ExampleService _exampleService;
        public ExampleController(ExampleService exampleService)
        {
            _exampleService = exampleService;
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> CreateExample(NewExample newExample)
        {
            Guid? uid = _exampleService.CreateExample(newExample);
            if (uid == null)
            {
                ModelState.AddModelError("exist", "Book or Publisher doesn't exist");
                return BadRequest(ModelState);
            }
            return uid;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Example>?> GetExamples()
        {
            List<Example>? examples = _exampleService.GetExamples();
            if (examples == null)
            {
                ModelState.AddModelError("examples", "None examples exist");
                return BadRequest(ModelState);
            }
            return examples;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult EditExample(Guid uIdExample, NewExample newExample)
        {
            if (_exampleService.EditExample(uIdExample, newExample)) return Ok();
            ModelState.AddModelError("Edit", "Example or Book or Publisher don't exist");
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult SetExampleStatus(Guid uIdExample, bool isTaken)
        {
            _exampleService.SetExampleStatus(uIdExample, isTaken);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteExample(Guid uIdExample)
        {
            if (_exampleService.DeleteExample(uIdExample)) return Ok();
            else return NotFound();
        }
    }
}