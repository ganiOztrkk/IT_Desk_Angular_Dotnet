using AutoMapper;
using IT_DeskServer.WebApi.DTOs;
using IT_DeskServer.WebApi.Models;

namespace IT_DeskServer.WebApi.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}