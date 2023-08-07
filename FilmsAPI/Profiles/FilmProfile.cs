using AutoMapper;
using FilmsAPI.DTOS;
using FilmsAPI.Models;

namespace FilmsAPI.Profiles
{
    public class FilmProfile : Profile
    {
        public FilmProfile()
        {
            CreateMap<CreateFilmDTO, FilmModel>();
            CreateMap<UpdateFilmDto, FilmModel>();
            CreateMap<ReadFilmDto, FilmModel>();
            CreateMap<FilmModel, UpdateFilmDto>();
        }
    }
}
