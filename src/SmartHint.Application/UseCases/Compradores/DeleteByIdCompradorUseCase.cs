using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.UseCases.Compradores.Interfaces;

namespace SmartHint.Application.UseCases.Compradores;

public class DeleteByIdCompradorUseCase(ICompradorServico compradorServico, IMapper mapper)
    : IDeleteByIdCompradorUseCase
{
    private readonly ICompradorServico _compradorServico = compradorServico;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<CompradorResponseDto>> Execute(int compradorId)
    {
        try
        {
            var comprador = _compradorServico.ObterPorIdAsync(compradorId);
            var compradorResult = await _compradorServico.DeletarAsync(compradorId);
            if (compradorResult.Status == ECustomResultStatus.Success && comprador is not null)
            {
                var response = _mapper.Map<CompradorResponseDto>(comprador);
                return CustomResult<CompradorResponseDto>.Success(response);
            }
            else
            {
                return CustomResult<CompradorResponseDto>.WithError(compradorResult.GeneralErrors);
            }
        }
        catch (Exception exception)
        {
            return CustomResult<CompradorResponseDto>.WithError(exception);
        }
    }
}
