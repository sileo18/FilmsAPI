using System.ComponentModel.DataAnnotations;

namespace FilmsAPI.DTOS
{
    public class ReadFilmDto    {
        
        public string Title { get; set; }        
        public string Description { get; set; }        
        public string Genre { get; set; }        
        public int Duration { get; set; }
        public DateTime Hour { get; set; } = DateTime.Now;
    }
}
