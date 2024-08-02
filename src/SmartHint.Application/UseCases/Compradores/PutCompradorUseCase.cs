using SmartHint.Application.Dtos.Comprador.Request;
using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.UseCases.Compradores.Interfaces;

namespace SmartHint.Application.UseCases.Compradores;

public class PutCompradorUseCase(ICompradorServico clienteService, IMapper mapper)
    : IPutCompradorUseCase
{
    private readonly ICompradorServico _clienteService = clienteService;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<CompradorResponseDto>> Execute(CompradorUpdateRequestDto request)
    {
        try
        {
            var comprador = CompradorRequestDto.Update(request.Comprador, request.Id);
            var clienteAtualizado = await _clienteService.AtualizarAsync(comprador);
            switch (clienteAtualizado.Status)
            {
                case ECustomResultStatus.Success:
                var response = _mapper.Map<CompradorResponseDto>(clienteAtualizado.Data);
                return CustomResult<CompradorResponseDto>.Success(response);
                case ECustomResultStatus.EntityNotFound:
                case ECustomResultStatus.HasError:
                case ECustomResultStatus.EntityHasError:
                case ECustomResultStatus.HasValidation:
                if (clienteAtualizado.GeneralErrors != null && clienteAtualizado.GeneralErrors.Count != 0)
                    return CustomResult<CompradorResponseDto>.WithError(clienteAtualizado.GeneralErrors);
                else if (clienteAtualizado.EntityErrors != null && clienteAtualizado.EntityErrors.Count != 0)
                    return CustomResult<CompradorResponseDto>.WithError(clienteAtualizado.EntityErrors);
                else if (clienteAtualizado.Error != null)
                    return CustomResult<CompradorResponseDto>.WithError(clienteAtualizado.Error);
                else
                    return CustomResult<CompradorResponseDto>.WithError(clienteAtualizado.Message);
                default:
                return CustomResult<CompradorResponseDto>.WithError("Status inesperado.");
            }
        }
        catch (Exception ex)
        {
            return CustomResult<CompradorResponseDto>.WithError(ex.Message);
        }
    }

}
