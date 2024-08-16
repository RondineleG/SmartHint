using SmartHint.Application.Dtos.Comprador.Request;
using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.UseCases.Compradores.Interfaces;

namespace SmartHint.Application.UseCases.Compradores;

public class PatchCompradorUseCase(ICompradorServico compradorServico, IMapper mapper)
    : IPatchCompradorUseCase
{
    private readonly ICompradorServico _compradorServico = compradorServico;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<CompradorResponseDto>> Execute(
        CompradorBloqueadoRequestDto request
    )
    {
        try
        {
            var clienteAtualizado = await _compradorServico.AlterarBloqueioAsync(request.Id, request.Bloqueado);
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
        catch (Exception exception)
        {
            return CustomResult<CompradorResponseDto>.WithError(exception);
        }
    }
}
