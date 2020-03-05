using System;
using AutoMapper;
using CacheDecorator.Models;
using CacheDecorator.Service.Model;

namespace CacheDecorator.Infrastructure.Mappings
{
    public class WebApplicationMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApplicationMappingProfile"/> class.
        /// </summary>
        public WebApplicationMappingProfile()
        {
            this.CreateMap<FooObject, FooViewModel>().ReverseMap();

            this.CreateMap<FooObject, FooUpdateViewModel>()
                .ForMember(d => d.FooId, o => o.MapFrom(s => s.FooId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Enable, o => o.MapFrom(s => s.Enable));
        }
    }
}