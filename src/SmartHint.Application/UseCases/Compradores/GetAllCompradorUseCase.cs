using SmartHint.Application.Dtos.Comprador.Request;
using SmartHint.Application.Dtos.Comprador.Response;
using SmartHint.Application.UseCases.Compradores.Interfaces;

namespace SmartHint.Application.UseCases.Compradores;

public class GetAllCompradorUseCase(ICompradorServico clienteService, IMapper mapper)
    : IGetAllCompradorUseCase
{
    private readonly ICompradorServico _clienteService = clienteService;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomResult<IEnumerable<CompradorResponseDto>>> Execute(CompradorPaginacaoRequestDto request)
    {
        try
        {
            var compradores = await _clienteService.ObterTodosAsync(request.Pagina, request.ItensPorPagina);

            if (compradores.Status == ECustomResultStatus.Success && compradores.Data is not null)
            {
                var response = _mapper.Map<IEnumerable<CompradorResponseDto>>(compradores.Data);
                return CustomResult<IEnumerable<CompradorResponseDto>>.Success(response);
            }
            else
            {
                return CustomResult<IEnumerable<CompradorResponseDto>>.EntityNotFound("Compradores", compradores.Message, "Nenhum comprador foi encontrado!");
            }
        }
        catch (Exception exception)
        {
            return CustomResult<IEnumerable<CompradorResponseDto>>.WithError(exception);
        }
    }
}
