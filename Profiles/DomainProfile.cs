using AutoMapper;
using MongoDB.Bson;
using webapidemo;
using webapidemo.DTO;
using webapidemo.Entity;

public class DomainProfile : Profile
{
	public DomainProfile()
	{
		CreateMap<NoteEntity, NoteDto>()
        .ForMember(dest => dest.Id, options => options.MapFrom(src => new ObjectId(src.Id))).ReverseMap();
	}
}