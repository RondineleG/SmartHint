using SmartHint.Application.Dtos.Comprador.Request;
using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.Extensions;

namespace SmartHint.Application.AutoMapper;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Comprador, CompradorResponseDto>()
            .ForMember(
                dest => dest.TipoPessoa,
                opt => opt.MapFrom(src => src.TipoPessoa.ToDescriptionString())
            )
            .ForMember(
                dest => dest.Genero,
                opt => opt.MapFrom(src => src.Genero.ToDescriptionString())
            )
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<CompradorRequestDto, Comprador>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CompradorRequestDto, Comprador>()
            .ForMember(
                dest => dest.TipoPessoa,
                opt => opt.MapFrom(src => ConvertToETipoPessoa(src.TipoPessoa))
            )
            .ForMember(
                dest => dest.Genero,
                opt => opt.MapFrom(src => ConvertToEGenero(src.Genero))
            );

        CreateMap<CompradorRequestDto, CompradorResponseDto>();

        CreateMap<CustomResult<Comprador>, CustomResult<CompradorResponseDto>>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));
    }

    private static EGenero ConvertToEGenero(int tipoGeneroValue)
    {
        var byteValue = Convert.ToByte(tipoGeneroValue);
        if (!Enum.IsDefined(typeof(EGenero), byteValue))
        {
            throw new ArgumentOutOfRangeException(
                nameof(tipoGeneroValue),
                "Valor inv�lido para o tipo de g�nero informado."
            );
        }
        return (EGenero)byteValue;
    }

    private static ETipoPessoa ConvertToETipoPessoa(int tipoPessoValue)
    {
        var byteValue = Convert.ToByte(tipoPessoValue);
        if (!Enum.IsDefined(typeof(ETipoPessoa), byteValue))
        {
            throw new ArgumentOutOfRangeException(
                nameof(tipoPessoValue),
                "Valor inv�lido para o tipo de pessoa informado."
            );
        }
        return (ETipoPessoa)byteValue;
    }
}
