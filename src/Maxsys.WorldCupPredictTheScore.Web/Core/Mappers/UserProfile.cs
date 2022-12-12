using AutoMapper;
using Maxsys.WorldCupPredictTheScore.Web.Models.Dtos;
using Maxsys.WorldCupPredictTheScore.Web.ViewModels;

namespace Maxsys.WorldCupPredictTheScore.Web.Core.Mappers;

public class UserProfile : Profile
{
	public UserProfile()
	{
        CreateMap<UserDTO, UserViewModel>();
    }
}
