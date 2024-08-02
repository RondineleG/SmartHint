using SmartHint.Application.Dtos.Comprador.Request;
using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.UseCases.Compradores.Interfaces;

namespace SmartHint.Application.UseCases.Compradores;

public class PostCompradorUseCase(ICompradorServico clienteService, IMapper mapper)
    : IPostCompradorUseCase
{
    private readonly ICompradorServico _clienteService = clienteService;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<CompradorResponseDto>> Execute(CompradorRequestDto request)
    {
        try
        {
            var comprador = CompradorRequestDto.Create(request);
            var novoComprador = await _clienteService.AdicionarAsync(comprador);
            if (novoComprador is null)
                return CustomResult<CompradorResponseDto>.EntityNotFound("Comprador", comprador.Id, "Não é possível inserir comprador nulo!");

            switch (novoComprador.Status)
            {
                case ECustomResultStatus.Success:
                var response = _mapper.Map<CompradorResponseDto>(novoComprador.Data);
                return CustomResult<CompradorResponseDto>.Success(response);

                case ECustomResultStatus.EntityNotFound:
                case ECustomResultStatus.HasError:
                case ECustomResultStatus.EntityHasError:
                case ECustomResultStatus.HasValidation:
                if (novoComprador.GeneralErrors != null && novoComprador.GeneralErrors.Count != 0)
                    return CustomResult<CompradorResponseDto>.WithError(novoComprador.GeneralErrors);
                else if (novoComprador.EntityErrors != null && novoComprador.EntityErrors.Count != 0)
                    return CustomResult<CompradorResponseDto>.WithError(novoComprador.EntityErrors);
                else if (novoComprador.Error != null)
                    return CustomResult<CompradorResponseDto>.WithError(novoComprador.Error);
                else
                    return CustomResult<CompradorResponseDto>.WithError(novoComprador.Message);
                default:
                return CustomResult<CompradorResponseDto>.WithError("Status inesperado.");
            }
        }
        catch (Exception ex)
        {
            return CustomResult<CompradorResponseDto>.WithError(ex);
        }
    }

}
