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

        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="filmDto">Objeto com os campos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inserção seja feita com sucesso</response>

        [HttpPost]
        public IActionResult AddFilm([FromBody] CreateFilmDTO filmDto)
        {
            var film = _mapper.Map<FilmModel>(filmDto);

            _context.Films.Add(film);
            _context.SaveChanges();
            return CreatedAtAction(nameof(FindById), new { id = film.Id }, film);
        }

        /// <summary>
        /// Lê os filmes do banco de dados
        /// </summary>
        /// <param name="skip">Parametros necessários para informar quantos filmes quer pular</param>
        /// <param name="take">Parametros necessários para informar quantos filmes quer retornar</param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Caso inserção seja feita com sucesso</response>
        [HttpGet]
        public IEnumerable<ReadFilmDto> ReadFilms([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadFilmDto>>(_context.Films.Skip(skip).Take(take).ToList());
        }


        /// <summary>
        /// Lê os filmes do banco de dados a partir do Id
        /// </summary>
        /// <param name="id">Parametros necessários o retorno do filme pelo Id</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso o filme seja encontrado</response>
        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            var filmReturned = _context.Films.FirstOrDefault(film => film.Id == id);

            if (filmReturned == null)
            {
                return NotFound("Film not found");
            }
            var filmDto = _mapper.Map<ReadFilmDto>(filmReturned);
            return Ok(filmReturned);
        }


        /// <summary>
        /// Atualiza o filme completo pelo Id
        /// </summary>
        /// <param name="id">Parametros necessários a atualização do filme pelo Id</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso o filme seja atualizado com sucesso</response>
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

        /// <summary>
        /// Atualiza o filme parcialmente pelo Id
        /// </summary>
        /// <param name="id">Parametros necessários a atualização do filme pelo Id</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso o filme seja atualizado com sucesso</response>
        [HttpPatch("{id}")]
        public IActionResult UpdateFilmPatch(int id, [FromBody] JsonPatchDocument<UpdateFilmDto> patch)
        {
            var film = _context.Films.FirstOrDefault(film => film.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            var filmToUpdate = _mapper.Map<UpdateFilmDto>(film);
            patch.ApplyTo(filmToUpdate, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            if (!TryValidateModel(filmToUpdate))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(filmToUpdate, film);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deleta o filme pelo Id
        /// </summary>
        /// <param name="id">Parametros necessários a exclusão do filme pelo Id</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso o filme seja excluído com sucesso</response>
        [HttpDelete("{id}")]

        public IActionResult DeleteFilm(int id)
        {
            var film = FindById(id);
            if (film == null)
            {
                return NotFound();
            }
            _context.Remove(film);
            return NoContent();

        }
    }
}
