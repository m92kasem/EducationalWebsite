using AutoMapper;
using EducationalWebsite.Application.DTOs;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.ValueObjects;

namespace EducationalWebsite.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Map CreateUserDto to User for creating purposes
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id (it will be generated in the service layer)
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Ignore Password (it will be hashed in the service layer)
                //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
            
            // Map AddressDto to Address
            CreateMap<AddressDto, Address>();
            
            // Map User to UserDto for viewing purposes
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
                //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
        }
    }
}
