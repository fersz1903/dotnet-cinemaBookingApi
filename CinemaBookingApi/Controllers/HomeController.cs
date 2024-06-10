using System.Collections;
using System.Security.Claims;
using CinemaBookingApi.Data;
using CinemaBookingApi.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingApi{
    public class HomeController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, 
        CinemaDbContext context,
        IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("listSeats")]
        public async Task<IEnumerable> GetAllSeats(){
            var seats = await _context.CinemaSeats.Select(s => new
            {
                s.Id,
                s.Name,
                s.IsBooked,
                UserId = s.User != null ? s.User.Id : (long?)null 
            })
            .ToListAsync();
            return seats;
        }
            
    

        [Authorize]
        [HttpPost("bookSeat")]
        public async Task<IActionResult> BookSeat(int seatId)
        {
            // Kullanıcı kimliğini token'dan alma
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }


            var seat = _context.CinemaSeats.FirstOrDefault(s => s.Id == seatId);

            if (seat == null){
                return BadRequest("Seat not found");
            }

            if(seat.IsBooked == true){
                return BadRequest("Seat already booked!!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == long.Parse(userId));
            if (user == null){
                return BadRequest("User not found");
            }
            
            seat.IsBooked = true;
            seat.User = user;
            await _context.SaveChangesAsync();

            return Ok("Seat booked");
        }


        [Authorize]
        [HttpPost("cancelBooking")]
        public async Task<IActionResult> CancelBooking(int seatId)
        {
            // Kullanıcı kimliğini token'dan alma
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }


            var seat = await _context.CinemaSeats.FirstOrDefaultAsync(s => s.Id == seatId);

            // var seat = _context.CinemaSeats.Where(s => s.Id == seatId)
            //     .Select(s => new
            //     {
            //         s.Id,
            //         s.Name,
            //         s.IsBooked,
            //         UserId = s.User != null ? s.User.Id : (long?)null // userid alabilmek için select sorgusu
            //     })
            //     .FirstOrDefault();
            if (seat == null){
                return BadRequest("Seat not found");
            }

            if(seat.IsBooked != true){
                return BadRequest("Seat already free!!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == long.Parse(userId));
            if (user == null){
                return BadRequest("User not found");
            }
            
            // farklı kullanıcının koltuğu iptal edilemez
            if(seat.User == null){
                return BadRequest("This seat booked from different user");
            }

            seat.IsBooked = false;
            seat.User = null;
            await _context.SaveChangesAsync();

            return Ok("Booking Cancelled");
        }
    }
}