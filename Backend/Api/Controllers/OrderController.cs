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
using static System.Net.Mime.MediaTypeNames;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public ActionResult<Guid> CreateOrder(Guid uIdExample)
        {
            Guid uIdUser = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            Guid? uIdOrder = _orderService.CreateOrder(uIdExample, uIdUser);
            if (uIdOrder == null)
            {
                ModelState.AddModelError("error", "Example doesn't exist or taken, user doesn't exist");
                return BadRequest(ModelState);
            }
            return uIdOrder;
        }
        [HttpPut]
        [Authorize(Roles = "admin")]
        public ActionResult CloseOrder(Guid uIdOrder)
        {
            if (_orderService.CloseOrder(uIdOrder))
            { return Ok(); }
            else
            {
                ModelState.AddModelError("Accept Error", "Order doesn't exist");
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public ActionResult<List<Order>?> GetOrders()
        {
            List<Order>? orders;
            if (User.Claims.Single(x => x.Type == ClaimTypes.Role).Value == "admin")
            {
                orders = _orderService.GetOrders();
                if (orders == null)
                {
                    ModelState.AddModelError("orders", "None vacancies exist");
                    return BadRequest(ModelState);
                }
                return orders;
            }
            Guid uIdUser = Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            orders = _orderService.GetOrders(uIdUser);
            if (orders == null)
            {
                ModelState.AddModelError("yourVacancies", "None your vacacnies exist");
                return BadRequest(ModelState);
            }
            return orders;
        }
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteOrder(Guid uIdOrder)
        {
            if (_orderService.DeleteOrder(uIdOrder))
                return Ok();
            else
                return NotFound();
        }
    }
}
