using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AuthService _authService;

    public UserController(AuthService authService)
    {
        _authService = authService;
    }

    // POST /customer/add - Register user
    [HttpPost("add")]
    public IActionResult RegisterUser([FromBody] RegisterRequest request)
    {
        if (request.Password != request.RePassword)
        {
            return BadRequest(new { error = "Passwords do not match" });
        }

        var existingUser = _authService.GetUserByEmail(request.Email);
        if (existingUser != null)
        {
            return BadRequest(new { error = "Email already in use" });
        }

        var newUser = _authService.CreateUser(
            request.First, 
            request.Last, 
            request.Email, 
            request.Password, 
            request.Number, 
            request.Address, 
            request.City, 
            request.State, 
            request.Country, 
            request.Pincode
        );

        return Ok(new { message = "Registration successful, please confirm your email" });
    }

    // POST /customer/login - Login user
    [HttpPost("login")]
    public IActionResult LoginUser([FromBody] LoginRequest request)
    {
        var user = _authService.GetUserByEmail(request.Email);
        if (user == null || !_authService.ValidatePassword(request.Password, user.Password))
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }

        if (!user.Confirmed)
        {
            return Unauthorized(new { error = "Please verify your email address" });
        }

        var token = _authService.GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        var users = _authService.GetAllUsers();
        if (users == null || users.Count == 0)
        {
            return NotFound(new { message = "No users found" });
        }
        
        return Ok(users);
    }
}
