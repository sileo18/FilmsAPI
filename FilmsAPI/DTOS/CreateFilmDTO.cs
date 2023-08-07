using System.ComponentModel.DataAnnotations;

namespace FilmsAPI.DTOS
{
    public class CreateFilmDTO
    {
        [Required(ErrorMessage = "Title is obrigatory")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is obrigatory")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Genre is obrigatory")]
        [StringLength(50, ErrorMessage = "The size of title exceed 50 characters")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Duration is obrigatory")]
        [Range(70, 600, ErrorMessage = "The duration must be between 70 and 600 minutes")]
        public int Duration { get; set; }
    }
}
