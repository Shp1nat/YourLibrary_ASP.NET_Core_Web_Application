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
    public class TypeBkController : ControllerBase
    {
        private readonly TypeBkService _typesBkService;
        public TypeBkController(TypeBkService typeBkervice)
        {
            _typesBkService = typeBkervice;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> CreateTypeBk(string nameOfTypeBk)
        {
            Guid? uid = _typesBkService.CreateTypeBk(nameOfTypeBk);
            if (uid == null)
            {
                ModelState.AddModelError("exist", "TypeBk already exists");
                return BadRequest(ModelState);
            }
            return uid;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<TypeBk>?> GetTypesBk()
        {
            List<TypeBk>? typesBk = _typesBkService.GetTypesBk();
            if (typesBk == null)
            {
                ModelState.AddModelError("typesBk", "None typesBk exist");
                return BadRequest(ModelState);
            }
            return typesBk;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult EditTypeBk(Guid uIdTypeBk, string nameOfTypeBk)
        {
            if (_typesBkService.EditTypeBk(uIdTypeBk, nameOfTypeBk)) return Ok();
            ModelState.AddModelError("Edit", "TypeBk doesn't exist");
            return BadRequest(ModelState);
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteTypeBk(Guid uIdTypeBk)
        {
            if (_typesBkService.DeleteTypeBk(uIdTypeBk)) return Ok();
            else return NotFound();
        }
    }
}