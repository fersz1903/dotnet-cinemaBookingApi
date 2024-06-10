using Microsoft.EntityFrameworkCore;
using UserAuthApi.Entites;

namespace UserAuthApi.Data;

public class UserDbContext : DbContext{
    //private readonly IConfiguration configuration;
    public DbSet<User> Users { get; set; }

    // public UserDbContext(IConfiguration configuration)
    // {
    //     this.configuration = configuration;    
    // }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql(configuration.GetConnectionString("cinemadb"));
    // }

}