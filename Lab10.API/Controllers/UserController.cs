using Lab10.Application.Services;
using Lab10.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lab10.API.Controllers
{
    [ApiController]
    [Route("delcarpio/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAllUsers();
        }
    }
}