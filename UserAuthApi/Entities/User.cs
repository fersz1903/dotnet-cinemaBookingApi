using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApi.Entites;

[Table("user")]
public class User
{
    [Column("id")]
    public long Id { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    
    [Column("isConfirmed")]
    public bool? IsConfirmed { get; set; }

    [Column("refreshToken")]
    public string? RefreshToken { get; set; }

    [Column("refreshTokenExpiryTime")]
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
