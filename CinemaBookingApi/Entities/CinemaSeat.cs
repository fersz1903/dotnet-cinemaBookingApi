using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using CinemaBookingApi.Entites;

namespace CinemaBookingApi.Entities{
    public class CinemaSeat{
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("isBooked")]
        public bool IsBooked { get; set; }

        // foreign key User
        public User? User { get; set; }
    }

}