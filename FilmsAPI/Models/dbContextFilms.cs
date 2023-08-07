using Microsoft.EntityFrameworkCore;

namespace FilmsAPI.Models
{
    public class dbContextFilms : DbContext
    {
        public dbContextFilms(DbContextOptions<dbContextFilms> opts)
            : base(opts)
        {

        }

        public DbSet<FilmModel> Films { get; set; }
    }
}
