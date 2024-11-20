using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt; // This will allow you to use the JwtSecurityTokenHandler class.
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;

    public AuthService(IConfiguration configuration, IMongoDatabase database)
    {
        _configuration = configuration;
        _database = database;
    }

    // JWT Token generation
    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));  // Correct usage of Encoding
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Get user by email from MongoDB
    public User GetUserByEmail(string email)
    {
        var collection = _database.GetCollection<User>("Users");
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        return collection.Find(filter).FirstOrDefault();
    }

    // Method to get all users
    public List<User> GetAllUsers()
    {
        var collection = _database.GetCollection<User>("Users");
        return collection.Find(user => true).ToList();
    }

    // Create a new user
    public User CreateUser(string first, string last, string email, string password, string number, string address, string city, string state, string country, string pincode)
    {
        var collection = _database.GetCollection<User>("Users");
        var hashedPassword = HashPassword(password);

        var newUser = new User
        {
            First = first,
            Last = last,
            Email = email,
            Password = hashedPassword,
            Number = number,
            Address = address,
            City = city,
            State = state,
            Country = country,
            Pincode = pincode,
            Confirmed = false
        };

        collection.InsertOne(newUser); // Insert the user into MongoDB
        return newUser;
    }

    // Password hashing
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Password validation
    public bool ValidatePassword(string enteredPassword, string storedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
    }
}
