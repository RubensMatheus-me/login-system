using login_system.Models;
using login_system.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null) return NotFound("Usuário não encontrado");

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null) return NotFound("Usuário não encontrado");

        await _userRepository.DeleteUserAsync(user.Id);
        
        return NoContent();

    }
    
}