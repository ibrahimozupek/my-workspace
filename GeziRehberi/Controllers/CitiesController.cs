using Microsoft.AspNetCore.Mvc;  
using GeziRehberi.Models;
using Microsoft.EntityFrameworkCore;

namespace GeziRehberi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            var cities = await _context.Cities.ToListAsync();
            return Ok(cities);  
        }

        // GET: api/cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // GET: api/cities/5/places
        [HttpGet("{id}/places")]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlacesByCity(int id)
        {
            var places = await _context.Places.Where(p => p.CityId == id).ToListAsync();

            if (places == null || !places.Any())
            {
                return NotFound($"No places found for the city with ID {id}.");
            }

            return Ok(places);
        }



    }
}
