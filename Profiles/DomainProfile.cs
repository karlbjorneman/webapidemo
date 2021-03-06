using AutoMapper;
using MongoDB.Bson;
using webapidemo;
using webapidemo.DTO;
using webapidemo.Model;

public class DomainProfile : Profile
{
	public DomainProfile()
	{
		CreateMap<Note, NoteDto>()
        .ForMember(dest => dest.Id, options => options.MapFrom(src => new ObjectId(src.Id)))
		.ReverseMap();

		CreateMap<Position, PositionDto>().ReverseMap();

		CreateMap<Column, ColumnDto>()
		.ForMember(dest => dest.Id, options => options.MapFrom(src => new ObjectId(src.Id)))
		.ReverseMap();

		CreateMap<Google.Apis.Auth.GoogleJsonWebSignature.Payload, UserDto>();

		CreateMap<string, ObjectId>().ConstructUsing(src => new ObjectId(src));
	}
}