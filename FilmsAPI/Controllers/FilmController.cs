using Microsoft.AspNetCore.Mvc;
using FilmsAPI.Models;
using FilmsAPI.DTOS;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly dbContextFilms _context;
        private readonly IMapper _mapper;

        public FilmController(dbContextFilms context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddFilm([FromBody] CreateFilmDTO filmDto)
        {
            var film = _mapper.Map<FilmModel>(filmDto);

            _context.Films.Add(film);
            _context.SaveChanges();
            return CreatedAtAction(nameof(FindById), new { id = film.Id }, film);
        }

        [HttpGet]
        public IEnumerable<FilmModel> ReadFilms([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _context.Films.Skip(skip).Take(take).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            var filmReturned = _context.Films.FirstOrDefault(film => film.Id == id);

            if (filmReturned == null)
            {
                return NotFound("Film not found");
            }

            return Ok(filmReturned);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFilm(int id, [FromBody] UpdateFilmDto filmDto)
        {
            var film = _context.Films.FirstOrDefault(film => film.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            _mapper.Map(filmDto, film);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateFilmPatch(int id, [FromBody] JsonPatchDocument<UpdateFilmDto> patch)
        {
            var film = _context.Films.FirstOrDefault(film => film.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            var filmToUpdate = _mapper.Map<UpdateFilmDto>(film);
            patch.ApplyTo(filmToUpdate, ModelState);

            if (!TryValidateModel(filmToUpdate))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(filmToUpdate, film);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
