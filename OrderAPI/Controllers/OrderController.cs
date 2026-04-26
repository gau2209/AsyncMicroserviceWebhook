using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Repository;
using Shared.Models;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController (IOrder _orderRepo) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> AddOrder (Order order)
        {
            var response = await _orderRepo.AddOrderAsync(order);
            return response.flag ? Ok(response) : BadRequest(response);
        }



    }
}
