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

		CreateMap<PositionEntity, PositionDto>().ReverseMap();

		CreateMap<ColumnEntity, ColumnDto>()
		.ForMember(dest => dest.Id, options => options.MapFrom(src => new ObjectId(src.Id))).ReverseMap();
	}
}