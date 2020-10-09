using AutoMapper;
using Web.Models;
using Web.ViewModels;

namespace Web.MappingProfiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieViewModel>()
                .ForMember(
                    dest => dest.ProducerName, 
                    opts => 
                        opts.MapFrom(source => source.Producer.Name));
            CreateMap<MovieViewModel, Movie>();
        }
    }
}