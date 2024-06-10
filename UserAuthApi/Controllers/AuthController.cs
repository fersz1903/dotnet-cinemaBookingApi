using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using SendGrid;
using SendGrid.Helpers.Mail;
using UserAuthApi.Data;
using UserAuthApi.Entites;
using UserAuthApi.Models;
using UserAuthApi.Services;
namespace UserAuthApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly UserDbContext _context;

    private readonly ILogger<AuthController> _logger;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, 
    UserDbContext context,
    IEmailService emailService,
    IConfiguration configuration
    )
    {
        _logger = logger;
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
    }

    [HttpGet("deneme")]
    public async Task<IActionResult> Index(string email){
        var token = TokenHelper.GenerateToken(email, _configuration);
        var userConf = new UserConfirmationModel
        {
            Email = email,
            AuthToken = token
        };

        
        await _emailService.SendConfirmationEmailAsync(email,token);

        return Ok("Registration successful. Please check your email to confirm.");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest("Email is already registered.");
            }

            var user = new User
            {
                Email = model.Email,
                //Password = HashPassword(model.Password),
                Password=model.Password,
                IsConfirmed = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = TokenHelper.GenerateToken(user.Email, _configuration);

            await _emailService.SendConfirmationEmailAsync(user.Email,token);

        }
       catch (Exception e)
       {
        return BadRequest(e.Message);
       }

        // TODO Doğrulama maili gönder !!!


        return Ok("User registered successfully. Please Check Your Email For Verification");
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> Confirm(string token)
    {
        try
        {
            var claimsPrincipal = TokenHelper.ValidateToken(token, _configuration);
            System.Console.WriteLine(claimsPrincipal.ToString());
            //var email = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub).Value;

            var email = claimsPrincipal.Claims.Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Token does not contain a valid email claim.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("Invalid token.");
            }
            user.IsConfirmed = true;
            await _context.SaveChangesAsync();
            return Ok("Email confirmed successfully.");
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            return BadRequest("Invalid token.");
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // Kullanıcıyı doğrula
        //var user = _userService.Authenticate(model.Email, model.Password);
        var user = _context.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
        if (user == null)
            return BadRequest(new { message = "Kullanıcı adı veya parola hatalı" });

        if (user.IsConfirmed == true){
            // JWT oluştur
            var token =  TokenHelper.GenerateLoginJwtToken(user, _configuration);
            var refreshToken = TokenHelper.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

            await _context.SaveChangesAsync();
            return Ok(new
                {
                    Token = token,
                    RefreshToken = refreshToken,
                });
        }
        return BadRequest("Email Confirmation Required");       
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken(string refreshToken)
    {
        // Refresh token'ı veritabanından doğrula
        var userId = _context.Users.FirstOrDefault(rt => rt.RefreshToken == refreshToken).Id;
        
        if (userId == null)
            return BadRequest(new { message = "Geçersiz refresh token" });
        
        // Yeni access token oluştur
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        var newAccessToken = TokenHelper.GenerateLoginJwtToken(user,_configuration);

        // Yeni access token ve refresh token'i istemciye dön
        return Ok(new {accessToken = newAccessToken});
    }





    // private string HashPassword(string password)
    // {
    //     byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
        
    //     // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
    //     string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
    //     password: password!,
    //     salt: salt,
    //     prf: KeyDerivationPrf.HMACSHA256,
    //     iterationCount: 100000,
    //     numBytesRequested: 256 / 8));

    //     return hashed;
    // }
}