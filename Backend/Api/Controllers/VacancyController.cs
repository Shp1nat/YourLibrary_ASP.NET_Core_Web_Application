using Backend.Api.Contract;
using Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Xml.Linq;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly VacancyService _vacancyService;
        public VacancyController(VacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult<Guid> CreateVacancy(string text)
        {
            if (text.Length == 0)
            {
                ModelState.AddModelError("length", "None text sended");
                return BadRequest(ModelState);
            }
            Guid uIdUser = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            Guid? uIdVacancy = _vacancyService.CreateVacancy(uIdUser, text);
            if (uIdVacancy == null)
            {
                ModelState.AddModelError("error", "Vacancy creation error");
                return BadRequest(ModelState);
            }
            return uIdVacancy;
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Vacancy>?> GetVacancies()
        {
            List<Vacancy>? vacancies;
            if (User.Claims.Single(x => x.Type == ClaimTypes.Role).Value == "admin")
            {
                vacancies = _vacancyService.GetVacancies();
                if (vacancies == null)
                {
                    ModelState.AddModelError("vacancies", "None vacancies exist");
                    return BadRequest(ModelState);
                }
                return vacancies;
            }
            Guid uIdUser = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            vacancies = _vacancyService.GetVacancies(uIdUser);
            if (vacancies == null)
            {
                ModelState.AddModelError("yourVacancies", "None your vacacnies exist");
                return BadRequest(ModelState);
            }
            return vacancies;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult AcceptVacancy(Guid uIdVacancy)
        {
            if (_vacancyService.AcceptVacancy(uIdVacancy))
            { return Ok(); }
            else
            {
                ModelState.AddModelError("Accept Error", "Vacancy doesn't exist");
                return BadRequest(ModelState);
            }
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult<Guid> RejectVacancy(Guid uIdVacancy)
        {
            if (_vacancyService.RejectVacancy(uIdVacancy))
                return Ok();
            else
                return NotFound();
        }
        [HttpDelete]
        [Authorize(Roles = "admin, user")]
        public ActionResult DeleteVacancy(Guid uIdVacancy)
        {
            
            if (User.Claims.Single(x => x.Type == ClaimTypes.Role).Value == "admin")
            {
                if (_vacancyService.DeleteVacancy(uIdVacancy))
                    return Ok();
                else
                    return NotFound();
            }
            if (_vacancyService.DeleteVacancy(uIdVacancy, Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value)))
                return Ok();
            else
                return NotFound();
        }
    }
}