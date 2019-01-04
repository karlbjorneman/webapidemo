using AutoMapper;
using MongoDB.Bson;
using webapidemo;

public class DomainProfile : Profile
{
	public DomainProfile()
	{
		CreateMap<FruitEnity, Fruit>()
        .ForMember(dest => dest.Id, options => options.MapFrom(src => new ObjectId(src.Id))).ReverseMap();
	}
}