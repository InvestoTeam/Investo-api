namespace Investo.Api.Profiles;

using AutoMapper;
using Investo.Api.ViewModels;
using Investo.BusinessLogic.Models;
using Investo.DataAccess.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<UserCreateViewModel, UserCreateModel>();
        this.CreateMap<UserCreateModel, User>();
    }
}