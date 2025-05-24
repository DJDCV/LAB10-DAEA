using Lab10.Application.Services;
using Lab10.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST: delcarpio/user
        [HttpPost]
        public ActionResult<User> Post([FromBody] User newUser)
        {
            _userService.AddUser(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("El ID del usuario no coincide.");

            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
                return NotFound();

            _userService.UpdateUser(updatedUser);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
                return NotFound();

            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}