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
            var clienteAlterado = await _compradorServico.AlterarBloqueioAsync(request.Id, request.Bloqueado);
            switch (clienteAlterado.Status)
            {
                case ECustomResultStatus.Success:
                var response = _mapper.Map<CompradorResponseDto>(clienteAlterado);
                return CustomResult<CompradorResponseDto>.Success(response);
                case ECustomResultStatus.EntityNotFound:
                case ECustomResultStatus.HasError:
                case ECustomResultStatus.EntityHasError:
                case ECustomResultStatus.HasValidation:
                if (clienteAlterado.GeneralErrors != null && clienteAlterado.GeneralErrors.Count != 0)
                    return CustomResult<CompradorResponseDto>.WithError(clienteAlterado.GeneralErrors);
                else if (clienteAlterado.EntityErrors != null && clienteAlterado.EntityErrors.Count != 0)
                    return CustomResult<CompradorResponseDto>.WithError(clienteAlterado.EntityErrors);
                else if (clienteAlterado.Error != null)
                    return CustomResult<CompradorResponseDto>.WithError(clienteAlterado.Error);
                else
                    return CustomResult<CompradorResponseDto>.WithError(clienteAlterado.Message);
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
