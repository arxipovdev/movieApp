using AutoMapper;
using Web.Models;
using Web.ViewModels;

namespace Web.MappingProfiles
{
    public class ProducerProfile : Profile
    {
        public ProducerProfile()
        {
            CreateMap<Producer, ProducerViewModel>();
            CreateMap<ProducerViewModel, Producer>();
        }
    }
}