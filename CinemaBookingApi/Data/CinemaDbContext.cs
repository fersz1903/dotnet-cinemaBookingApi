using CinemaBookingApi.Entites;
using CinemaBookingApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingApi.Data{
    public class CinemaDbContext: DbContext{

        public DbSet<User> Users { get; set; }
        public DbSet<CinemaSeat> CinemaSeats { get; set; }

        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
        {
        }
    }
}