namespace SmartHint.Core.Interfaces.Repositories;

public interface ICompradorRepositorio
{
    Task<CustomResult<Comprador>> ObterPorIdAsync(int id);
    Task<CustomResult<IEnumerable<Comprador>>> ObterTodosAsync(int pagina, int tamanhoPagina);
    Task<CustomResult<Comprador>> AdicionarAsync(Comprador comprador);
    Task<CustomResult<Comprador>> AtualizarAsync(Comprador comprador);
    Task<CustomResult> DeletarAsync(int id);
    Task<CustomResult<bool>> ExisteAsync(int id);
    Task<CustomResult<bool>> EmailExisteAsync(string email);
    Task<CustomResult<bool>> CpfCnpjExisteAsync(string cpfCnpj);
    Task<CustomResult<bool>> InscricaoEstadualExisteAsync(string inscricaoEstadual);
    Task<bool> EmailJaCadastradoAsync(string email);
    Task<bool> CpfCnpjJaCadastradoAsync(string cpfCnpj);
    Task<bool> InscricaoEstadualJaCadastradaAsync(string inscricaoEstadual);
}
