using AutoMapper;
using CacheDecorator.Repository.Entities;
using CacheDecorator.Service.Model;
using System;

namespace CacheDecorator.Service.Mapping
{
    /// <summary>
    /// Class ServiceMappingProfile.
    /// Implements the <see cref="AutoMapper.Profile" />
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ServiceMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMappingProfile"/> class.
        /// </summary>
        public ServiceMappingProfile()
        {
            this.CreateMap<FooModel, FooObject>().ReverseMap();
        }
    }
}