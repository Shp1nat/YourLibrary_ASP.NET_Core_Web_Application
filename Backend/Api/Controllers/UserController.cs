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
using System;
namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        public UserController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService= jwtService;
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterData registerData)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(registerData.Email);
            if (match.Success)
            {
                var uid =  _userService.Register(registerData);
                if (uid == null)
                {
                    ModelState.AddModelError("exist", "Login or email already exist");
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            ModelState.AddModelError("email", "Invalid email format");
            return BadRequest(ModelState);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JwtToken> Login(LoginData loginData)
        {
            var uid = _userService.Login(loginData);
            if (uid == null)
            {
                ModelState.AddModelError("lgnOrPswrd", "Invalid login or password");
                return BadRequest(ModelState);
            }
            if(_userService.isAdmin((Guid)uid)){
                return new JwtToken { Token = _jwtService.GenerateToken(uid.Value, loginData.Email, true)};
            }
            return new JwtToken { Token = _jwtService.GenerateToken(uid.Value,loginData.Email, false)};
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<UserUpdate> GetData(Guid uIdUser)
        {
            UserUpdate? userUpdate = _userService.GetData(uIdUser);
            if (userUpdate == null)
            {
                return NotFound();
            }
            return userUpdate;
        }
        [HttpPut]
        [Authorize(Roles = "admin, user")]
        public ActionResult<Guid> EditData(Guid uIdUser, UserUpdate userUpdate)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(userUpdate.Email);
            if (User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value == uIdUser.ToString())
            {
                if (match.Success)
                {
                    var uid = _userService.EditData(uIdUser, userUpdate);
                    if (uid == null)
                    {
                        ModelState.AddModelError("sameData", "New login or email are taken");
                        return BadRequest(ModelState);
                    }
                    return uid;
                }
                else
                {
                    ModelState.AddModelError("email", "Invalid email format");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("authorization", "Error authorization");
                return BadRequest(ModelState);
            }
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult SetAdminStatus(Guid uIdUser, bool status)
        {
            _userService.SetAdminStatus(uIdUser, status);
             return Ok();
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Guid uIdUser)
        {
            if (_userService.Delete(uIdUser)) return Ok();
            else return NotFound();
        }
    }
}