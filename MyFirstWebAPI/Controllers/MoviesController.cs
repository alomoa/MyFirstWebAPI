using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebAPI.context;
using MyFirstWebAPI.Models;

namespace MyFirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MediaContext _dbContext;

        public MoviesController(MediaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetMovies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            //confused to why we need to encase our IEnumerable with action result 
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            return await _dbContext.Movies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);

            if(movie == null)
            {
                return NotFound();
            }

            return movie;
        }
        [HttpPost]
        public async Task<ActionResult> PostMovie(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new {id = movie.Id}, movie);
        }
    }
}
