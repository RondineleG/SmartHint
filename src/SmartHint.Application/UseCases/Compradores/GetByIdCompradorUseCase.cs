using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.UseCases.Compradores.Interfaces;

namespace SmartHint.Application.UseCases.Compradores;

public class GetByIdCompradorUseCase(ICompradorServico clienteService, IMapper mapper)
    : IGetByIdCompradorUseCase
{
    private readonly ICompradorServico _clienteService = clienteService;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<CompradorResponseDto>> Execute(int clienteId)
    {
        try
        {
            var comprador = await _clienteService.ObterPorIdAsync(clienteId);
            if (comprador.Status == ECustomResultStatus.Success && comprador.Data != null)
            {
                var response = _mapper.Map<CompradorResponseDto>(comprador.Data);
                return CustomResult<CompradorResponseDto>.Success(response);
            }
            else
            {
                return CustomResult<CompradorResponseDto>.WithError(comprador.GeneralErrors);
            }
        }
        catch (Exception exception)
        {
            return CustomResult<CompradorResponseDto>.WithError(exception);
        }
    }
}
