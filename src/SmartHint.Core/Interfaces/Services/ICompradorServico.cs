namespace SmartHint.Core.Interfaces.Services;


public interface ICompradorServico
{
    Task<CustomResult<Comprador>> ObterPorIdAsync(int id);
    Task<CustomResult<IEnumerable<Comprador>>> ObterTodosAsync(int pagina, int tamanhoPagina);
    Task<CustomResult<Comprador>> AdicionarAsync(Comprador comprador);
    Task<CustomResult<Comprador>> AtualizarAsync(Comprador comprador);
    Task<CustomResult> DeletarAsync(int id);
    Task<CustomResult<Comprador>> AlterarBloqueioAsync(int id, bool bloqueado);
}
