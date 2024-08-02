namespace SmartHint.Persistence.Repositories;

public class CompradorRepositorio : ICompradorRepositorio
{
    private readonly EntityFrameworkDataContext _context;

    public CompradorRepositorio(EntityFrameworkDataContext context)
    {
        _context = context;
    }

    public async Task<CustomResult<Comprador>> ObterPorIdAsync(int id)
    {
        try
        {
            var comprador = await _context.Compradores.FindAsync(id);
            return comprador != null ?
                CustomResult<Comprador>.Success(comprador) :
                CustomResult<Comprador>.WithError(CustomMessageHandler.EntityNotFound("Comprador", id));
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Comprador>.WithError(CustomMessageHandler.DbUpdateError(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Comprador>.WithError(CustomMessageHandler.UnexpectedError("obter comprador", exception.Message));
        }
    }

    public async Task<CustomResult<IEnumerable<Comprador>>> ObterTodosAsync(int pagina, int tamanhoPagina)
    {
        try
        {
            var compradores = await _context.Compradores
                .OrderBy(c => c.Id)     
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return CustomResult<IEnumerable<Comprador>>.Success(compradores);
        }
        catch (Exception ex)
        {
            return CustomResult<IEnumerable<Comprador>>.WithError(CustomMessageHandler.UnexpectedError("obter todos os compradores", ex.Message));
        }
    }


    public async Task<CustomResult<Comprador>> AdicionarAsync(Comprador comprador)
    {
        try
        {
            _context.Compradores.Add(comprador);
            await _context.SaveChangesAsync();
            return CustomResult<Comprador>.Success(comprador);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Comprador>.WithError(CustomMessageHandler.DbUpdateError(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Comprador>.WithError(CustomMessageHandler.UnexpectedError("adicionar comprador", exception.Message));
        }
    }

    public async Task<CustomResult<Comprador>> AtualizarAsync(Comprador comprador)
    {
        try
        {
            _context.Entry(comprador).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CustomResult<Comprador>.Success(comprador);
        }
        catch (DbUpdateException exception)
        {
            return CustomResult<Comprador>.WithError(CustomMessageHandler.DbUpdateError(exception));
        }
        catch (Exception exception)
        {
            return CustomResult<Comprador>.WithError(CustomMessageHandler.UnexpectedError("atualizar comprador", exception.Message));
        }
    }

    public async Task<CustomResult> DeletarAsync(int id)
    {
        try
        {
            var comprador = await _context.Compradores.FindAsync(id);
            if (comprador == null)
            {
                return CustomResult.WithError(CustomMessageHandler.EntityNotFound("Comprador", id));
            }
            _context.Compradores.Remove(comprador);
            await _context.SaveChangesAsync();
            return CustomResult.Success();
        }
        catch (DbUpdateException exception)
        {
            return CustomResult.WithError(CustomMessageHandler.DbUpdateError(exception));
        }
        catch (Exception exception)
        {
            return CustomResult.WithError(CustomMessageHandler.UnexpectedError("deletar comprador", exception.Message));
        }
    }

    public async Task<CustomResult<bool>> ExisteAsync(int id)
    {
        try
        {
            var existe = await _context.Compradores.AnyAsync(c => c.Id == id);
            return CustomResult<bool>.Success(existe);
        }
        catch (Exception ex)
        {
            return CustomResult<bool>.WithError(CustomMessageHandler.UnexpectedError("verificar existência de comprador", ex.Message));
        }
    }

    public async Task<CustomResult<bool>> EmailExisteAsync(string email)
    {
        try
        {
            var existe = await _context.Compradores.AnyAsync(c => c.Email == email);
            return CustomResult<bool>.Success(existe);
        }
        catch (Exception ex)
        {
            return CustomResult<bool>.WithError(CustomMessageHandler.UnexpectedError("verificar existência de email", ex.Message));
        }
    }

    public async Task<CustomResult<bool>> CpfCnpjExisteAsync(string cpfCnpj)
    {
        try
        {
            var existe = await _context.Compradores.AnyAsync(c => c.CpfCnpj == cpfCnpj);
            return CustomResult<bool>.Success(existe);
        }
        catch (Exception ex)
        {
            return CustomResult<bool>.WithError(CustomMessageHandler.UnexpectedError("verificar existência de CPF/CNPJ", ex.Message));
        }
    }

    public async Task<CustomResult<bool>> InscricaoEstadualExisteAsync(string inscricaoEstadual)
    {
        try
        {
            var existe = await _context.Compradores.AnyAsync(c => c.InscricaoEstadual == inscricaoEstadual);
            return CustomResult<bool>.Success(existe);
        }
        catch (Exception ex)
        {
            return CustomResult<bool>.WithError(CustomMessageHandler.UnexpectedError("verificar existência de inscrição estadual", ex.Message));
        }
    }

    public async Task<bool> EmailJaCadastradoAsync(string email)
    {
        try
        {
            var comprador = await _context.Compradores.FirstOrDefaultAsync(c => c.Email == email);
            return comprador != null;
        }
        catch (Exception ex)
        {
            throw new CustomResultException(CustomResult.WithError(CustomMessageHandler.UnexpectedError("verificar email cadastrado", ex.Message)));
        }
    }

    public async Task<bool> CpfCnpjJaCadastradoAsync(string cpfCnpj)
    {
        try
        {
            var comprador = await _context.Compradores.FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);
            return comprador != null;
        }
        catch (Exception ex)
        {
            throw new CustomResultException(CustomResult.WithError(CustomMessageHandler.UnexpectedError("verificar CPF/CNPJ cadastrado", ex.Message)));
        }
    }

    public async Task<bool> InscricaoEstadualJaCadastradaAsync(string inscricaoEstadual)
    {
        try
        {
            var comprador = await _context.Compradores.FirstOrDefaultAsync(c => c.InscricaoEstadual == inscricaoEstadual);
            return comprador != null;
        }
        catch (Exception ex)
        {
            throw new CustomResultException(CustomResult.WithError(CustomMessageHandler.UnexpectedError("verificar inscrição estadual cadastrada", ex.Message)));
        }
    }
}
